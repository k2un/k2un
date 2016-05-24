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
    public partial class tabfrmSetupSVID : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private BindingSource pSVIDListSource = new BindingSource();
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmSetupSVID()
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

        public void Save()
        {
            if (MessageBox.Show("SVID 리스트를 업데이트 합니다.\n양산중인 장비에서 이 기능을 실행하면\n심각한 문제가 발생 할 수 있습니다.\n진행하겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            File.Copy(Application.StartupPath + @"\system\System.mdb", Application.StartupPath + @"\system\System.mdb.bak", true);

            string dstrSQL;
            DataTable dDT;
            int dintIndex = 0;

            try
            {
                //if (!DBAct.clsDBAct.funConnect())
                //{
                //    MessageBox.Show("DB Connection Fail!", "Connect Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                if (!DBAct.clsDBAct.funBeginTransaction())
                {
                    MessageBox.Show("DB Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //DataAdapter 생성
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM tbSVID order by SVID", DBAct.clsDBAct.funOleDbConnect());
                OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);

                //DataAdapter를 이용하여 DB로 업데이트를 한다.
                dataAdapter.SelectCommand.Transaction = DBAct.clsDBAct.funOleDbTransaction();
                dataAdapter.Update((DataTable)pSVIDListSource.DataSource);

                if (!DBAct.clsDBAct.funCommitTransaction())
                {
                    MessageBox.Show("DB Commit Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DBAct.clsDBAct.funRollbackTransaction();
                    return;
                }

                

                dstrSQL = "SELECT * FROM tbSVID order by SVID";
                //dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);                          //DataTable을 받아온다.
                dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);
                if (dDT != null)
                {
                    pInfo.Unit(0).SubUnit(0).RemoveSVID();
                    pInfo.DeleteTable("SVID");
                    pInfo.AddDataTable("SVID", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["SVID"]);
                        pInfo.Unit(0).SubUnit(0).AddSVID(dintIndex);

                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Name = dr["SVNAME"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Length = Convert.ToInt32(dr["Length"].ToString());
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Format = dr["Format"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Value = dr["SV"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Type = dr["Type"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Unit = dr["Unit"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Range = dr["Range"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).HaveMinusValue = Convert.ToBoolean(dr["HaveMinusValue"]);
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).DESC = dr["Description"].ToString();
                        //PInfo.Unit(0).SubUnit(0).SVID(dintIndex).UnitID = this.PInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).ModuleID = dr["ModuleID"].ToString();

                        if (pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Format.Trim() != "")
                        {
                            this.pInfo.All.SVIDPLCReadLength = this.pInfo.All.SVIDPLCReadLength + this.pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Length;
                        }
                    }
                }

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
                dstrSection = "SVIDLastModified";
                dstrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FunINIMethod.subINIWriteValue("ETCInfo", dstrSection, dstrDate, pInfo.All.SystemINIFilePath);

                //this.lblLastModified.Text = dstrDate;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void tabfrmSetupSVID_Load(object sender, EventArgs e)
        {
            try
            {
                if (pInfo != null)
                {
                    pSVIDListSource.DataSource = pInfo.Table("SVID");
                    this.dataGridView1.DataSource = pSVIDListSource;

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
                    pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
                }
            }
        }

        public void Reload()
        {
            try
            {
                if (MessageBox.Show("SVID 리스트를 업데이트 합니다.\n양산중인 장비에서 이 기능을 실행하면\n심각한 문제가 발생 할 수 있습니다.\n진행하겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

                

                //DB로부터 SVID를 읽어들여 저장한다.
                string dstrSQL = "SELECT * FROM tbSVID order by SVID";
                DataTable dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                int dintIndex = 0;


                if (dDT != null)
                {
                    //File.Copy(Application.StartupPath + @"\system\System.mdb", Application.StartupPath + @"\system\System.mdb.bak", true);

                    pInfo.Unit(0).SubUnit(0).RemoveSVID();

                    

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["SVID"]);
                        pInfo.Unit(0).SubUnit(0).AddSVID(dintIndex);

                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Name = dr["SVNAME"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Length = Convert.ToInt32(dr["Length"].ToString());
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Format = dr["Format"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Value = dr["SV"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Type = dr["Type"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Unit = dr["Unit"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Range = dr["Range"].ToString();
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).HaveMinusValue = Convert.ToBoolean(dr["HaveMinusValue"]);
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).DESC = dr["Description"].ToString();
                        //pInfo.Unit(0).SubUnit(0).SVID(dintIndex).UnitID = this.pInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                        pInfo.Unit(0).SubUnit(0).SVID(dintIndex).ModuleID = dr["ModuleID"].ToString();

                        if (pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Format.Trim() != "")
                        {
                            this.pInfo.All.SVIDPLCReadLength = this.pInfo.All.SVIDPLCReadLength + this.pInfo.Unit(0).SubUnit(0).SVID(dintIndex).Length;
                        }
                    }

                    pInfo.DeleteTable("SVID");
                    pInfo.AddDataTable("SVID", dDT);
                    tabfrmSetupSVID_Load(this, null);
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
