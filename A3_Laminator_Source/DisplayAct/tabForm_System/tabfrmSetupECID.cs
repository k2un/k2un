using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using CommonAct;
using InfoAct;
using DBAct;
using System.Data.OleDb;
using System.IO;
using System.Collections;

namespace DisplayAct
{
    public partial class tabfrmSetupECID : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private BindingSource pECIDBindingSource = new BindingSource();
        private bool pEditMode = false;
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmSetupECID()
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

        private void tabfrmSetupECID_Load(object sender, EventArgs e)
        {
            try
            {
                if (pInfo != null)
                {
                    pECIDBindingSource.DataSource = pInfo.Table("ECID");
                    this.dataGridView1.DataSource = pECIDBindingSource;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    //dataGridView1.Columns[4].Visible = false;
                    //dataGridView1.Columns[6].Visible = false;

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
        
        private void subSaveECValue()
        {
            string ChangeECID = "";
            string ChangeECV = "";
            bool dbolErrorCheck = false;
            try
            {
                for (int dintLoop = 0; dintLoop < dataGridView1.Rows.Count; dintLoop++)
                {

                }

                if (dbolErrorCheck)
                {
                    
                }

                for (int dintLoop = 0; dintLoop < dataGridView1.Rows.Count; dintLoop++)
                {
                    int dintECID = Convert.ToInt32(dataGridView1[0, dintLoop].Value);

                    clsECID ECID = pInfo.Unit(0).SubUnit(0).ECID(dintECID);
                    if (ECID.ECDEF != dataGridView1[3, dintLoop].Value.ToString())
                    {
                        ChangeECID += dintECID + "=";
                        ChangeECV += dataGridView1[3, dintLoop].Value.ToString() + "=";
                        ECID.ECDEF = dataGridView1[3, dintLoop].Value.ToString();
                    }
                }

                pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ECIDChange);

                //if (string.IsNullOrEmpty(ChangeECID) == false)
                //{
                //    funECID_DBDelete();
                //    funECID_DBInsert();
                //    //pInfo.subSendSF_Set(clsInfo.SFName.S6F11_ECReport, ChangeECID, ChangeECV);

                //}

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                MessageBox.Show("EC Value Update Fail!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public bool funECID_DBDelete()
        {
            bool dbolDBUpdateFlag = false;
            string dstrSQL = "";
            try
            {
                dstrSQL = string.Format("DELETE FROM `tbECID` ");
                dbolDBUpdateFlag = DBAct.clsDBAct.funExecuteQuery(dstrSQL);

            }
            catch (Exception)
            {
            }
            return dbolDBUpdateFlag;
        }

        public bool funECID_DBInsert()
        {
            bool dbolDBUpdateFlag = false;
            string dstrSQL = "";
            try
            {

                foreach (int ECID in pInfo.Unit(0).SubUnit(0).ECID())
                {
                    clsECID CurrentECID = pInfo.Unit(0).SubUnit(0).ECID(ECID);

                    dstrSQL = string.Format(@"INSERT INTO tbECID VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}');",
                        ECID, CurrentECID.Name, CurrentECID.Min, CurrentECID.ECSLL, CurrentECID.ECWLL, CurrentECID.ECDEF, CurrentECID.ECWUL, CurrentECID.ECSUL, CurrentECID.Max, CurrentECID.DESC, CurrentECID.Use.ToString(), CurrentECID.ModuleID);
                    dbolDBUpdateFlag = DBAct.clsDBAct.funExecuteQuery(dstrSQL);
                }

            }
            catch (Exception)
            {
            }
            return dbolDBUpdateFlag;
        }

        private void subSaveECList()
        {
            try
            {
                if (MessageBox.Show("ECID 리스트를 업데이트 합니다.\n양산중인 장비에서 이 기능을 실행하면\n심각한 문제가 발생 할 수 있습니다.\n진행하겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

                File.Copy(Application.StartupPath + @"\system\System.mdb", Application.StartupPath + @"\system\System.mdb.bak", true);

                int dintECID = 0;   
                string dstrECName = string.Empty;
                Single dsngMin = 0;
                Single dsngECSLL = 0;
                Single dsngECWLL = 0;
                Single dsngECDEF = 0;
                Single dsngECWUL = 0;
                Single dsngECSUL = 0;
                Single dsngMax = 0;
                //string dstrDesc = string.Empty;
                bool dbolUse = false;
                string dstrModuleID = string.Empty;

                #region "유효성검사"
                for (int dintLoop = 0; dintLoop < dataGridView1.RowCount -1; dintLoop++)
                {
                    try
                    {
                        dintECID = Convert.ToInt32(dataGridView1.Rows[dintLoop].Cells[0].Value);
                        dstrECName = dataGridView1.Rows[dintLoop].Cells[1].Value.ToString();
                        dsngMin = Convert.ToInt32(dataGridView1.Rows[dintLoop].Cells[2].Value);
                        dsngECSLL = Convert.ToSingle(dataGridView1.Rows[dintLoop].Cells[3].Value);
                        dsngECWLL = Convert.ToSingle(dataGridView1.Rows[dintLoop].Cells[4].Value);
                        dsngECDEF = Convert.ToSingle(dataGridView1.Rows[dintLoop].Cells[5].Value);
                        dsngECWUL = Convert.ToSingle(dataGridView1.Rows[dintLoop].Cells[6].Value);
                        dsngECSUL = Convert.ToSingle(dataGridView1.Rows[dintLoop].Cells[7].Value);
                        dsngMax = Convert.ToInt32(dataGridView1.Rows[dintLoop].Cells[8].Value);
                        dbolUse = Convert.ToBoolean(dataGridView1.Rows[dintLoop].Cells[10].Value);
                        dstrModuleID = dataGridView1.Rows[dintLoop].Cells[11].Value.ToString().Trim();

                        if (dsngECSLL > dsngECDEF || dsngECSLL > dsngECSUL || dsngECDEF > dsngECSUL)
                        {
                            MessageBox.Show("ECID: " + dintECID.ToString() + "의 입력값의 범위가 옳지 않습니다.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        bool ModuleIDExist = false;

                        foreach (int dintUnitID in this.pInfo.Unit())
                        {
                            clsUnit tempUnit = this.pInfo.Unit(dintUnitID);

                            if (tempUnit.SubUnit(0).ModuleID.Equals(dstrModuleID))
                            {
                                ModuleIDExist = true;
                                break;
                            }
                        }

                        if (!ModuleIDExist)
                        {
                            MessageBox.Show("ECID: " + dintECID.ToString() + "의 ModuleID가 존재하지 않습니다.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                    }
                    catch
                    {
                        MessageBox.Show("ECID: " + dintECID.ToString() + "의 입력값이 형식이 맞지 않습니다.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                #endregion

                #region "EC List Update"
                if (!DBAct.clsDBAct.funBeginTransaction())
                {
                    MessageBox.Show("DB Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //DataAdapter 생성
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM tbECID order by ECID", DBAct.clsDBAct.funOleDbConnect());
                OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);

                //DataAdapter를 이용하여 DB로 업데이트를 한다.
                dataAdapter.SelectCommand.Transaction = DBAct.clsDBAct.funOleDbTransaction();
                dataAdapter.Update((DataTable)pECIDBindingSource.DataSource);

                if (!DBAct.clsDBAct.funCommitTransaction())
                {
                    MessageBox.Show("DB Commit Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DBAct.clsDBAct.funRollbackTransaction();
                    return;
                }

                #endregion

                #region "ECID Reload"
                string dstrSQL = "SELECT * FROM tbECID order by ECID";
                DataTable dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                int dintIndex = 0;


                if (dDT != null)
                {
                    pInfo.Unit(0).SubUnit(0).RemoveECID();

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["ECID"]);
                        pInfo.Unit(0).SubUnit(0).AddECID(dintIndex);

                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Name = dr["ECNAME"].ToString();
                        //pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Unit = dr["Unit"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Min = dr["Min"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECDEF = dr["ECDEF"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Max = dr["Max"].ToString();
                        //pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Use = Convert.ToBoolean(dr["Use"]);
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Format = dr["Format"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).DESC = dr["Description"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).UnitID = 0;
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).ModuleID = dr["ModuleID"].ToString();
                    }

                    this.pInfo.All.HOSTReportECIDCount = dDT.Rows.Count;

                    pInfo.DeleteTable("ECID");
                    pInfo.AddDataTable("ECID", dDT);
                    tabfrmSetupECID_Load(this, null);
                }

                
                subSaveLastModified();
                MessageBox.Show("Data Save Success!", "Jobs Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                #endregion


            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                MessageBox.Show("EC List Update Fail!", "ERROR",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void Save()
        {
            bool dbolError = false;
            float fMIN = 0;
            float fECSLL = 0;
            float fECDEF = 0;
            float fECSUL = 0;
            float fMax = 0;
            int dintECID = 0;
            BindingSource pEOIDListSource = new BindingSource();
            string arrECID = "";
            string dstrSQL = "";


            try
            {
                File.Copy(Application.StartupPath + @"\system\System.mdb", Application.StartupPath + @"\system\System.mdb.bak", true);

                try
                {
                    //유효성검사
                    for (int dintLoop = 0; dintLoop < this.dataGridView1.RowCount; dintLoop++)
                    {
                        dintECID = Convert.ToInt32(this.dataGridView1.Rows[dintLoop].Cells[0].Value);
                        fMIN = Convert.ToSingle(dataGridView1[2, dintLoop].Value.ToString());
                        fECSLL = Convert.ToSingle(dataGridView1[4, dintLoop].Value.ToString());
                        fECDEF = Convert.ToSingle(dataGridView1[5, dintLoop].Value.ToString());
                        fECSUL = Convert.ToSingle(dataGridView1[6, dintLoop].Value.ToString());
                        fMax = Convert.ToSingle(dataGridView1[8, dintLoop].Value.ToString());

                        if (fMIN > fECSLL || fECSLL > fECDEF || fECDEF > fECSUL || fECSUL > fMax)
                        {
                            dbolError = true;
                            MessageBox.Show("올바르지 않는 Data가 있습니다!!");
                            break;
                        }

                        //if (dbolError == true)
                        //{
                        //    string dstrErrorMsg = this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).Name + " 의 값은 " + this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSLL
                        //              + "~" + this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSUL + " 이어야 합니다.";
                        //    MessageBox.Show(dstrErrorMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}

                    }
                    if (dbolError == false)
                    {
                        pInfo.All.ECIDChange.Clear();
                        pInfo.All.ECIDChangeFromHost = "";
                        pInfo.All.ECIDChangeHOSTReport.Clear();

                        //바뀐 EOV 값이 있는지 검사한다.
                        for (int dintLoop = 0; dintLoop < this.dataGridView1.RowCount; dintLoop++)
                        {
                            dintECID = Convert.ToInt32(this.dataGridView1.Rows[dintLoop].Cells[0].Value);
                            fMIN = Convert.ToSingle(dataGridView1[2, dintLoop].Value.ToString());
                            fECSLL = Convert.ToSingle(dataGridView1[4, dintLoop].Value.ToString());
                            fECDEF = Convert.ToSingle(dataGridView1[5, dintLoop].Value.ToString());
                            fECSUL = Convert.ToSingle(dataGridView1[6, dintLoop].Value.ToString());
                            fMax = Convert.ToSingle(dataGridView1[8, dintLoop].Value.ToString());

                            clsECID CurrentECID = pInfo.Unit(0).SubUnit(0).ECID(dintECID);

                            if (Convert.ToSingle(CurrentECID.ECWLL) != fECSLL ||Convert.ToSingle( CurrentECID.ECDEF )!= fECDEF || Convert.ToSingle(CurrentECID.ECWUL) != fECSUL)
                            {
                                this.pInfo.All.ECIDChange.Add(dintECID,
                                    FunStringH.funMakePLCData(FunStringH.funMakeRound(fMIN.ToString(), CurrentECID.Format))
                                    + "," + FunStringH.funMakePLCData(FunStringH.funMakeRound(fECSLL.ToString(), CurrentECID.Format))
                                    + "," + FunStringH.funMakePLCData(FunStringH.funMakeRound(fECDEF.ToString(), CurrentECID.Format))
                                    + "," + FunStringH.funMakePLCData(FunStringH.funMakeRound(fECSUL.ToString(), CurrentECID.Format))
                                    + "," + FunStringH.funMakePLCData(FunStringH.funMakeRound(fMax.ToString(), CurrentECID.Format)));
                                this.pInfo.All.ECIDChangeFromHost += dintECID + ";";
                                this.pInfo.All.ECIDChangeHOSTReport.Add(dintECID, fMIN + "," + fECSLL + "," + fECDEF + "," + fECSUL + "," + fMax);
                            }
                        }

                        if (string.IsNullOrEmpty(pInfo.All.ECIDChangeFromHost))
                        {
                            MessageBox.Show("변경된 ECID가 존재하지 않습니다.");
                            pInfo.All.ECIDChange.Clear();
                            pInfo.All.ECIDChangeFromHost = "";
                            pInfo.All.ECIDChangeHOSTReport.Clear();
                            return;
                        }
                        else
                        {
                            pInfo.All.ECIDChangeBYWHO = "2";
                            //arrECID = arrECID.Substring(0, arrECID.Length - 1);
                            pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ECIDChange);
                            MessageBox.Show("PLC에 ECID 변경 요청을 하였습니다.");
                        }
                    }


                    //if (!DBAct.clsDBAct.funBeginTransaction())
                    //{
                    //    MessageBox.Show("DB Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}

                    ////DataAdapter 생성
                    //OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM tbECID order by ECID", DBAct.clsDBAct.funOleDbConnect());
                    //OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);

                    ////DataAdapter를 이용하여 DB로 업데이트를 한다.
                    //dataAdapter.SelectCommand.Transaction = DBAct.clsDBAct.funOleDbTransaction();
                    //dataAdapter.Update((DataTable)pEOIDListSource.DataSource);

                    //if (!DBAct.clsDBAct.funCommitTransaction())
                    //{
                    //    MessageBox.Show("DB Commit Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    DBAct.clsDBAct.funRollbackTransaction();
                    //    return;
                    //}

                    //dstrSQL = "SELECT * FROM tbECID order by ECID";
                    //pInfo.Unit(0).SubUnit(0).RemoveECID();
                    //DataTable dt = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                    //pInfo.AddDataTable("ECID", dt);
                    //pInfo.AddViewEvent(InfoAct.clsInfo.ViewEvent.ECIDUpdate);
                }
                catch (Exception ex)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    DBAct.clsDBAct.funRollbackTransaction();
                    DBAct.clsDBAct.funDisconnect();     // DB 연결을 끊는다.
                    MessageBox.Show("DB Update Fail, Because DB Process Error!", "DB Update Error!",
                                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                ////if (this.pEditMode) subSaveECList();
                ////else subSaveECValue();
                    
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSaveLastModified()
        {
            string dstrSection = "";
            string dstrDate = "";

            try
            {
                dstrSection = "ECIDLastModified";
                dstrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FunINIMethod.subINIWriteValue("ETCInfo", dstrSection, dstrDate, pInfo.All.SystemINIFilePath);

                //this.lblLastModified.Text = dstrDate;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subUpdateTable()
        {
            try
            {
                pECIDBindingSource.DataSource = pInfo.Table("ECID");
                this.dataGridView1.DataSource = pECIDBindingSource;
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
       

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (!this.pEditMode
                    && this.dataGridView1.Columns[e.ColumnIndex].Name != "ECSLL"
                    && this.dataGridView1.Columns[e.ColumnIndex].Name != "ECWLL"
                    && this.dataGridView1.Columns[e.ColumnIndex].Name != "ECDEF"
                    && this.dataGridView1.Columns[e.ColumnIndex].Name != "ECWUL"
                    && this.dataGridView1.Columns[e.ColumnIndex].Name != "ECSUL") e.Cancel = true;
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }


        public void Reload()
        {
            try
            {
                //if (MessageBox.Show("ECID 리스트를 업데이트 합니다.\n양산중인 장비에서 이 기능을 실행하면\n심각한 문제가 발생 할 수 있습니다.\n진행하겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

                string dstrSQL = "SELECT * FROM tbECID order by ECID";
                DataTable dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);
                int dintIndex = 0;


                if (dDT != null)
                {
                    pInfo.Unit(0).SubUnit(0).RemoveECID();

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["ECID"]);
                        pInfo.Unit(0).SubUnit(0).AddECID(dintIndex);

                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Name = dr["ECNAME"].ToString();
                        //pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Unit = dr["Unit"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Min = dr["Min"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).ECDEF = dr["ECDEF"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Max = dr["Max"].ToString();
                        //pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Use = Convert.ToBoolean(dr["Use"]);
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).Format = dr["Format"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).DESC = dr["Description"].ToString();
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).UnitID = 0;
                        pInfo.Unit(0).SubUnit(0).ECID(dintIndex).ModuleID = dr["ModuleID"].ToString();
                    }

                    this.pInfo.All.HOSTReportECIDCount = dDT.Rows.Count;

                    pInfo.DeleteTable("ECID");
                    pInfo.AddDataTable("ECID", dDT);
                    tabfrmSetupECID_Load(this, null);
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
