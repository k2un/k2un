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
    public partial class tabfrmSetupAlarm : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private BindingSource pAlarmListSource = new BindingSource();
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmSetupAlarm()
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

        private void tabfrmSetupAlarm_Load(object sender, EventArgs e)
        {
            try
            {
                if (pInfo != null)
                {
                    pAlarmListSource.DataSource = pInfo.Table("ALARM");
                    this.dataGridView1.DataSource = pAlarmListSource;

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
            int dintIndex = 0;

            try
            {
                if (MessageBox.Show("Alarm 리스트를 업데이트 합니다.\n양산중인 장비에서 이 기능을 실행하면\n심각한 문제가 발생 할 수 있습니다.\n진행하겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

                File.Copy(Application.StartupPath + @"\system\System.mdb", Application.StartupPath + @"\system\System.mdb.bak", true);

                if (!DBAct.clsDBAct.funBeginTransaction())
                {
                    MessageBox.Show("DB Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //DataAdapter 생성
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM tbAlarm order by AlarmID", DBAct.clsDBAct.funOleDbConnect());
                OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);

                //DataAdapter를 이용하여 DB로 업데이트를 한다.
                dataAdapter.SelectCommand.Transaction = DBAct.clsDBAct.funOleDbTransaction();
                dataAdapter.Update((DataTable)pAlarmListSource.DataSource);

                if (!DBAct.clsDBAct.funCommitTransaction())
                {
                    MessageBox.Show("DB Commit Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DBAct.clsDBAct.funRollbackTransaction();
                    return;
                }

                //DB에 저장이 성공적으로 완료되면 다시 DB에서 Data를 로딩한다.
                //구조체의 내용 갱신
                //현재 등록되어 있는 모든 Alarm을 삭제한다.
                pInfo.Unit(0).SubUnit(0).RemoveAlarm();

                dstrSQL = "SELECT * FROM tbAlarm order by AlarmID";
                //dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);                          //DataTable을 받아온다.
                dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);
                if (dDT != null)
                {
                    pInfo.DeleteTable("ALARM");
                    pInfo.AddDataTable("ALARM", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        if (Convert.ToBoolean(dr["AlarmReport"].ToString().Trim()))
                        {
                            dintIndex = Convert.ToInt32(dr["AlarmID"]);
                            pInfo.Unit(0).SubUnit(0).AddAlarm(dintIndex);

                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmCode = Convert.ToInt32(dr["AlarmCD"].ToString().Trim());
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmType = dr["AlarmType"].ToString().Trim();
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmDesc = dr["AlarmDesc"].ToString().Trim();
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmReport = Convert.ToBoolean(dr["AlarmReport"].ToString().Trim());
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).UnitID = pInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).ModuleID = dr["ModuleID"].ToString();
                        }
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
                dstrSection = "ALARMLastModified";
                dstrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FunINIMethod.subINIWriteValue("ETCInfo", dstrSection, dstrDate, pInfo.All.SystemINIFilePath);

                //this.lblLastModified.Text = dstrDate;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void Reload()
        {
            try
            {

                //if (MessageBox.Show("Alarm 리스트를 업데이트 합니다.\n양산중인 장비에서 이 기능을 실행하면\n심각한 문제가 발생 할 수 있습니다.\n진행하겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

                string dstrSQL = "SELECT * FROM tbAlarm order by AlarmID";
                DataTable dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                int dintIndex = 0;


                if (dDT != null)
                {
                    pInfo.Unit(0).SubUnit(0).RemoveAlarm();

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["AlarmID"]);
                        if (Convert.ToBoolean(dr["AlarmReport"].ToString().Trim()))
                        {
                            pInfo.Unit(0).SubUnit(0).AddAlarm(dintIndex);

                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmCode = Convert.ToInt32(dr["AlarmCD"].ToString().Trim());
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmType = dr["AlarmType"].ToString().Trim();
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmDesc = dr["AlarmDesc"].ToString().Trim();
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmReport = Convert.ToBoolean(dr["AlarmReport"].ToString().Trim());
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).UnitID = pInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                            pInfo.Unit(0).SubUnit(0).Alarm(dintIndex).ModuleID = dr["ModuleID"].ToString();
                        }
                    }

                    pInfo.DeleteTable("ALARM");
                    pInfo.AddDataTable("ALARM", dDT);
                    tabfrmSetupAlarm_Load(this, null);
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
