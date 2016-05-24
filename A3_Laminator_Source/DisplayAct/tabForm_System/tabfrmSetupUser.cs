using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using CommonAct;
using InfoAct;
using DBAct;
using System.IO;

namespace DisplayAct
{
    public partial class tabfrmSetupUser : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private BindingSource pUserBindingSource = new BindingSource();
        private bool pEditMode = false;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmSetupUser()
        {
            InitializeComponent();
            funSetDoubleBuffered(this.dataGridView1);
        }
        #endregion

        #region Methods
        public static void funSetDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                                          BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                                           null, control, new object[] { true });
        }

        private void tabfrmSetupUser_Load(object sender, EventArgs e)
        {
            try
            {
                if (pInfo != null)
                {
                    pUserBindingSource.DataSource = pInfo.Table("User");
                    this.dataGridView1.DataSource = pUserBindingSource;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null)
                {
                    pInfo.subLog_Set(clsInfo.LogType.PLC, ex.ToString());
                }
            }
        }

        public void Save()
        {
            string dstrSQL;
            DataTable dDT;
            string dstrName = "";

            try
            {
                File.Copy(Application.StartupPath + @"\system\System.mdb", Application.StartupPath + @"\system\System.mdb.bak", true);

                if (!DBAct.clsDBAct.funBeginTransaction())
                {
                    MessageBox.Show("DB Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //DataAdapter 생성
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM tbUser order by UserLevel", DBAct.clsDBAct.funOleDbConnect());
                OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);

                //DataAdapter를 이용하여 DB로 업데이트를 한다.
                dataAdapter.SelectCommand.Transaction = DBAct.clsDBAct.funOleDbTransaction();
                dataAdapter.Update((DataTable)pUserBindingSource.DataSource);

                if (!DBAct.clsDBAct.funCommitTransaction())
                {
                    MessageBox.Show("DB Commit Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DBAct.clsDBAct.funRollbackTransaction();
                    return;
                }

                //DB에 저장이 성공적으로 완료되면 다시 DB에서 Data를 로딩한다.
                //구조체의 내용 갱신
                pInfo.Unit(0).SubUnit(0).RemoveUser();

                //DB로부터 User 정보를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbUser order by UserLevel";
                dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    pInfo.DeleteTable("USER");
                    pInfo.AddDataTable("USER", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dstrName = dr["UserID"].ToString();
                        pInfo.Unit(0).SubUnit(0).AddUser(dstrName);

                        pInfo.Unit(0).SubUnit(0).User(dstrName).Level = Convert.ToInt32(dr["UserLevel"].ToString());
                        pInfo.Unit(0).SubUnit(0).User(dstrName).PassWord = dr["Pass"].ToString();
                        pInfo.Unit(0).SubUnit(0).User(dstrName).Desc = dr["Description"].ToString();
                    }
                }

                //DBAct.clsDBAct.funDisconnect();     // DB 연결을 끊는다.

                subSaveLastModified(); //최종 수정된 날짜를 Ini파일에 저장한다.

                MessageBox.Show("Data Save Success!", "Jobs Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                DBAct.clsDBAct.funRollbackTransaction();
                DBAct.clsDBAct.funDisconnect();     // DB 연결을 끊는다.
                MessageBox.Show("DB Update Fail, Because DB Process Error!", "DB Update Error!",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void subSaveLastModified()
        {
            string dstrSection = "";
            string dstrDate = "";

            try
            {
                dstrSection = "USERLastModified";
                dstrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FunINIMethod.subINIWriteValue("ETCInfo", dstrSection, dstrDate, pInfo.All.SystemINIFilePath);

                //this.lblLastModified.Text = dstrDate;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void EditMode()
        {
            this.pEditMode = !this.pEditMode;

            if (this.pEditMode)
            {
                this.dataGridView1.AllowUserToAddRows = true;
                this.dataGridView1.AllowUserToDeleteRows = true;
                this.dataGridView1.RowHeadersVisible = true;
            }
            else
            {
                this.dataGridView1.AllowUserToAddRows = false;
                this.dataGridView1.AllowUserToDeleteRows = false;
                this.dataGridView1.RowHeadersVisible = false;
            }
        }
        

        public void Reload()
        {

            try
            {
                //if (MessageBox.Show("User 리스트를 업데이트 합니다.\n양산중인 장비에서 이 기능을 실행하면\n심각한 문제가 발생 할 수 있습니다.\n진행하겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;



                //DB로부터 SVID를 읽어들여 저장한다.
                string dstrSQL = "SELECT * FROM tbUser order by UserLevel";
                DataTable dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                string dstrName = string.Empty;


                if (dDT != null)
                {
                    pInfo.Unit(0).SubUnit(0).RemoveUser();

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dstrName = dr["UserID"].ToString();
                        pInfo.Unit(0).SubUnit(0).AddUser(dstrName);

                        pInfo.Unit(0).SubUnit(0).User(dstrName).Level = Convert.ToInt32(dr["UserLevel"].ToString());
                        pInfo.Unit(0).SubUnit(0).User(dstrName).PassWord = dr["Pass"].ToString();
                        pInfo.Unit(0).SubUnit(0).User(dstrName).Desc = dr["Description"].ToString();
                    }

                    pInfo.DeleteTable("User");
                    pInfo.AddDataTable("User", dDT);
                    tabfrmSetupUser_Load(this, null);
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
