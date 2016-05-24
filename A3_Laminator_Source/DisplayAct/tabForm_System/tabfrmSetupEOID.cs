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
using System.Collections;

namespace DisplayAct
{
    public partial class tabfrmSetupEOID : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private BindingSource pEOIDListSource = new BindingSource();
        private bool pEditMode = false;
        string dstrErrorMsg = "";

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmSetupEOID()
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
            File.Copy(Application.StartupPath + @"\system\System.mdb", Application.StartupPath + @"\system\System.mdb.bak", true);

            string dstrSQL;
            DataTable dDT;
            int dintIndex = 0;
            int dintEOV = 0;
            bool dbolError = false;
            SortedList dHT = new SortedList();
            string dstrAfterEOV = "";


            try
            {
                //유효성검사
                for (int dintLoop = 0; dintLoop < this.dataGridView1.RowCount; dintLoop++)
                {
                    dintIndex = Convert.ToInt32(this.dataGridView1.Rows[dintLoop].Cells[0].Value);
                    dintEOV = Convert.ToInt32(this.dataGridView1.Rows[dintLoop].Cells[5].Value);

                    if (dintEOV >= this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin && dintEOV <= this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax)
                    {
                        if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID == 8)  //EOID=8이면 VCR Reading Mode임.
                        {
                            if (dintEOV == 0 || dintEOV == 1 || dintEOV == 2 || dintEOV == 3)       //VCR Reading Mode는 0,1,2,3 만 값이 있다.
                            {
                            }
                            else
                            {
                                dbolError = true;
                            }
                        }

                        if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID == 19) //SEM
                        {
                            if (dintEOV == 1)
                            {
                                //pInfo.All.SEM_ON = true;
                                //if (pInfo.All.CommPort.ToUpper() != "NULL") pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SerialPortOpen);

                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.UDPPortOpen);
                                //SEM Controller에 Start명령을 내린다.
                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerStart);
                            }
                            else
                            {
                                //pInfo.All.SEM_ON = false;
                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerEnd);

                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.UDPPortClose);
                            }
                        }

                        if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID == 21) //APC
                        {
                            if (dintEOV == 0 || dintEOV == 1 )       
                            {
                                if (dintEOV == 0)
                                {
                                    pInfo.All.APCUSE = false;
                                }
                                else
                                {
                                    pInfo.All.APCUSE = true;
                                }
                            }
                            else
                            {
                                dbolError = true;
                            }
                        }

                        if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID == 22) //RPC
                        {
                            if (dintEOV == 0 || dintEOV == 1)       
                            {
                                if (dintEOV == 0)
                                {
                                    pInfo.All.RPCUSE = false;
                                }
                                else
                                {
                                    pInfo.All.RPCUSE = true;
                                }
                            }
                            else
                            {
                                dbolError = true;
                            }
                        }
                    }
                    else
                    {
                        dbolError = true;
                    }

                    if (dbolError == true)
                    {
                        dstrErrorMsg = this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).DESC + " 값은 " + this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin
                                  + "~" + this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax + " 이어야 합니다.";
                        MessageBox.Show(dstrErrorMsg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }

                //바뀐 EOV 값이 있는지 검사한다.
                for (int dintLoop = 0; dintLoop < this.dataGridView1.RowCount; dintLoop++)
                {
                    //dintIndex = dintLoop + 1;
                    dintIndex = Convert.ToInt32(this.dataGridView1.Rows[dintLoop].Cells[0].Value);
                    dintEOV = Convert.ToInt32(this.dataGridView1.Rows[dintLoop].Cells[5].Value);
                    if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV != dintEOV)
                    {
                        dstrAfterEOV = dstrAfterEOV + dintIndex.ToString() + ";";       //EOID 저장 
                        dHT.Add(dintIndex, this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV);        //변경전의 EOID, EOV 저장

                        //if (this.PInfo.funGetEOIDLapseTime(dintIndex) == true) dbolProcessTimeOverChange = true;
                    }
                }

                if (dstrAfterEOV == "")
                {
                    MessageBox.Show("변경된 EOV가 하나도 없습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!DBAct.clsDBAct.funBeginTransaction())
                {
                    MessageBox.Show("DB Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //DataAdapter 생성
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT * FROM tbEOID order by Index", DBAct.clsDBAct.funOleDbConnect());
                OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(dataAdapter);

                //DataAdapter를 이용하여 DB로 업데이트를 한다.
                dataAdapter.SelectCommand.Transaction = DBAct.clsDBAct.funOleDbTransaction();
                dataAdapter.Update((DataTable)pEOIDListSource.DataSource);

                if (!DBAct.clsDBAct.funCommitTransaction())
                {
                    MessageBox.Show("DB Commit Transaction NG!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DBAct.clsDBAct.funRollbackTransaction();
                    return;
                }

                

                dstrSQL = "SELECT * FROM tbEOID order by index";
                //dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);                          //DataTable을 받아온다.
                dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);
                if (dDT != null)
                {
                    pInfo.Unit(0).SubUnit(0).RemoveEOID();

                    pInfo.DeleteTable("EOID");
                    pInfo.AddDataTable("EOID", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["Index"]);
                        pInfo.Unit(0).SubUnit(0).AddEOID(dintIndex);

                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID = Convert.ToInt32(dr["EOID"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMD = Convert.ToInt32(dr["EOMD"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMDMin = Convert.ToInt32(dr["EOMDMin"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMDMax = Convert.ToInt32(dr["EOMDMax"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV = Convert.ToInt32(dr["EOV"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin = Convert.ToInt32(dr["EOVMin"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax = Convert.ToInt32(dr["EOVMax"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).DESC = dr["Description"].ToString();
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).PLCWrite = Convert.ToBoolean(dr["PLCWrite"]);
                    }

                }
                string[] dstrData = dstrAfterEOV.Split(new char[] { ';' });
                if (dstrData.Length > 1)
                {
                    this.pInfo.All.EOIDChangeBYWHO = "2";       //BY OP

                    //HOST로 EOV값 변경 보고
                    this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentParameterEvent, 101, dstrAfterEOV);   //마지막 인자는 EOID Index임

                    this.pInfo.All.EOIDChangeBYWHO = "";       //초기화

                    //변경 이력 로그 저장
                    foreach (DictionaryEntry de in dHT)
                    {
                        dintIndex = Convert.ToInt32(de.Key);        //EOID Index

                        string dstrLog = "";
                        dstrLog = dstrLog + "EOID" + ",";
                        dstrLog = dstrLog + DateTime.Now.ToString("yyyyMMddHHmmss") + ",";
                        dstrLog = dstrLog + "변경" + ",";
                        dstrLog = dstrLog + this.pInfo.All.UserID + ",";
                        dstrLog = dstrLog + this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).DESC + ",";
                        dstrLog = dstrLog + de.Value.ToString() + ",";
                        dstrLog = dstrLog + this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV;

                        this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.Parameter, dstrLog);      //로그 출력

                        dstrLog = ""; //초기화


                        //만약 EOID중에 PLC로 변경할 것이 있으면 Write해 준다.
                        if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).PLCWrite == true)
                        {
                            switch (dintIndex)
                            {
                                //case 8:                       //VCR Reading Mode
                                //    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.VCRReadingMode, this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV);
                                //    break;

                                case 1:                       //APC(Advanced Process Control) MODE
                                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.APCMode, this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV);
                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (dintIndex)
                            {
                                case 2:                       //MCC Reporting Mode

                                    if (this.pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV == 1)
                                    {
                                        FunINIMethod.funINIReadValue("MCC", "MCCFileUploadUse", "True", this.pInfo.All.SystemINIFilePath);
                                    }
                                    else
                                    {
                                        FunINIMethod.funINIReadValue("MCC", "MCCFileUploadUse", "False", this.pInfo.All.SystemINIFilePath);
                                    }
                                    break;

                                default:
                                    break;
                            }
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
                dstrSection = "EOIDLastModified";
                dstrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FunINIMethod.subINIWriteValue("ETCInfo", dstrSection, dstrDate, pInfo.All.SystemINIFilePath);

                //this.lblLastModified.Text = dstrDate;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void tabfrmSetupEOID_Load(object sender, EventArgs e)
        {
            try
            {
                if (pInfo != null)
                {
                    pEOIDListSource.DataSource = pInfo.Table("EOID");
                    this.dataGridView1.DataSource = pEOIDListSource;

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
                //DB로부터 EOID를 읽어들여 저장한다.
                string dstrSQL = "SELECT * FROM tbEOID order by Index";

                DataTable dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                int dintIndex = 0;


                if (dDT != null)
                {
                    pInfo.Unit(0).SubUnit(0).RemoveEOID();

                    pInfo.DeleteTable("EOID");
                    pInfo.AddDataTable("EOID", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["Index"]);
                        pInfo.Unit(0).SubUnit(0).AddEOID(dintIndex);

                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID = Convert.ToInt32(dr["EOID"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMD = Convert.ToInt32(dr["EOMD"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMDMin = Convert.ToInt32(dr["EOMDMin"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMDMax = Convert.ToInt32(dr["EOMDMax"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV = Convert.ToInt32(dr["EOV"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMin = Convert.ToInt32(dr["EOVMin"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOVMax = Convert.ToInt32(dr["EOVMax"]);
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).DESC = dr["Description"].ToString();
                        pInfo.Unit(0).SubUnit(0).EOID(dintIndex).PLCWrite = Convert.ToBoolean(dr["PLCWrite"]);
                    }

                    this.pInfo.All.HOSTReportEOIDCount = dDT.Rows.Count; 

                    tabfrmSetupEOID_Load(this, null);
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
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

        #endregion
    }
}
