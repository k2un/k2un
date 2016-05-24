using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using CommonAct;
using InfoAct;

namespace STM
{
    class clsInfoActPlugIn
    {
        private InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;

        //*******************************************************************************
        //  Function Name : funInitialInfo()
        //  Description   : 구조체를 초기화한다.
        //  Parameters    : 
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/12          어 경태         [L 00] 
        //*******************************************************************************
        public void subInitialInfo(string strModuleID)
        {
            try
            {
                //PInfo = new InfoAct.clsInfo();                       //InfoAct.Dll 정의및 생성
                //clsConstant.gInfo = PInfo;                           //전역으로 사용하기 위해 구조체를 Constant에 등록한다.

                subEQPInfoInitial(strModuleID);                                 //INI의 데이타를 읽어 구조체 EQP에 저장한다.
                subUnitInfoInitial(strModuleID);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subClose()
        //  Description   : Act를 종료시킨다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/02          어 경태         [L 00] 
        //*******************************************************************************
        public void subClose()
        {
            
        }

        //*******************************************************************************
        //  Function Name : subUnitInfoInitial()
        //  Description   : UnitInfo 구조체를 초기화 한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : DB로부터 데이타를 읽어 저장한다.
        //*******************************************************************************
        //  2006/10/25          어 경태          [L 00] 
        //*******************************************************************************
        private void subUnitInfoInitial(string strEQPName)
        {
            string dstrSQL;
            DataTable dDT;
            DataTable dDT2;
            string dstrName;
            int dintIndex = 0;
            int dintPPIDBodyID = 0;
            //int dintCount = 0;

            int dintSVID = 0;
            int dintUnitID = 0;
            int dintSubUnitID = 0;
            //[2015/06/03](Add by HS)
            clsMCC MCCinfo;

            try
            {
                dstrSQL = "SELECT * FROM tbUnit";
                dDT = DBAct.clsDBAct.funSelectQuery2(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintUnitID = Convert.ToInt32(dr["UnitID"]);
                        dintSubUnitID = Convert.ToInt32(dr["SubUnitID"]);

                        if (PInfo.Unit(dintUnitID) == null)
                        {
                            if (PInfo.AddUnit(dintUnitID) == false)
                            {
                                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subUnitInfoInitial: Unit 등록 에러");
                            }
                        }

                        if (PInfo.Unit(dintUnitID).SubUnit(dintSubUnitID) == null)
                        {
                            if (PInfo.Unit(dintUnitID).AddSubUnit(dintSubUnitID) == false)
                            {
                                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subUnitInfoInitial: SubUnit 등록 에러");
                            }
                        }

                        PInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID = dr["ModuleID"].ToString();
                        PInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).Index = dr["Index"].ToString();

                        if (dintUnitID == 1 || dintUnitID == 2)
                        {
                            if (dintSubUnitID != 0)
                            {
                                //dintIndex = Convert.ToInt32(dr["Index"]);
                                //PInfo.AddPort(dintIndex);
                                //PInfo.Port(dintIndex).PortState = "0";
                                //PInfo.Port(dintIndex).HostReportPortID = dr["ModuleID"].ToString().Substring(dr["ModuleID"].ToString().Length - 4, 4);
                            }
                        }
                    }
                }



                //DB로부터 MCC List Name를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbMCC order by Index";                         //tbMCC를 추가
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                           //DataTable을 받아온다.
                if (dDT != null)
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["Index"].ToString());
                        PInfo.Unit(0).SubUnit(0).AddMCC(dintIndex);                     //tbMCC Data 추가
                        PInfo.Unit(0).SubUnit(0).MCC(dintIndex).MCCType = dr["Type"].ToString();
                        PInfo.Unit(0).SubUnit(0).MCC(dintIndex).MCCName = dr["MCCName"].ToString();
                        PInfo.Unit(0).SubUnit(0).MCC(dintIndex).ModuleID = dr["ModuleID"].ToString();
                        PInfo.Unit(0).SubUnit(0).MCC(dintIndex).FromPosition = dr["FromPosition"].ToString();
                        PInfo.Unit(0).SubUnit(0).MCC(dintIndex).ToPosition = dr["ToPosition"].ToString();
                        PInfo.Unit(0).SubUnit(0).MCC(dintIndex).MCCDesc = dr["Description"].ToString();
                        PInfo.Unit(0).SubUnit(0).MCC(dintIndex).MCCOnlyItem = Convert.ToBoolean(dr["OnlyItem"].ToString().Trim());
                        //[2015/07/31]
                        //PInfo.Unit(0).SubUnit(0).MCC(dintIndex).GroupNo = Convert.ToInt32(dr["GroupNo"].ToString().Trim());
                    }
                }

                //Alarm Data
                dstrSQL = "SELECT * FROM tbAlarm order by AlarmID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["AlarmID"]);
                        PInfo.Unit(0).SubUnit(0).AddAlarm(dintIndex);                                         //Recipe Data 추가

                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmCode = Convert.ToInt32(dr["AlarmCD"].ToString().Trim());
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmType = dr["AlarmType"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmDesc = dr["AlarmDesc"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).AlarmReport = Convert.ToBoolean(dr["AlarmReport"].ToString().Trim());
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).UnitID = this.PInfo.funGetModuleIDToUnitID(dr["ModuleID"].ToString());
                        PInfo.Unit(0).SubUnit(0).Alarm(dintIndex).ModuleID = dr["ModuleID"].ToString();
                    }
                }

                #region "MCC Info"
                //DB로부터 PPID Body 기준정보를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbMCC_I";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    foreach (DataRow dr in dDT.Rows)
                    {
                        dintIndex = Convert.ToInt32(dr["index"].ToString());
                        PInfo.Unit(0).SubUnit(0).AddMCCInfo(dintIndex);

                        MCCinfo = PInfo.Unit(0).SubUnit(0).MCCInfo(dintIndex);
                        MCCinfo.MCCName = dr["MCCName"].ToString().Trim();
                        MCCinfo.MCCDesc = dr["Description"].ToString().Trim();
                        MCCinfo.MCCOnlyItem = Convert.ToBoolean(dr["OnlyItem"].ToString().Trim());
                        MCCinfo.MCCType = dr["Type"].ToString().Trim();
                        MCCinfo.ModuleID = dr["ModuleID"].ToString().Trim();
                        MCCinfo.ToPosition = dr["ToPosition"].ToString().Trim();
                        MCCinfo.FromPosition = dr["FromPosition"].ToString().Trim();
                        MCCinfo.Unit = dr["Unit"].ToString().Trim();
                        MCCinfo.PLCReadFlag = Convert.ToBoolean(dr["WordReadFlag"].ToString());

                        MCCinfo.SVIDIndex = Convert.ToInt32(dr["SVID"].ToString());
                        if (PInfo.Unit(0).SubUnit(0).SVID(MCCinfo.SVIDIndex) != null)
                        {
                            PInfo.Unit(0).SubUnit(0).SVID(MCCinfo.SVIDIndex).MCCInfoIndex = dintIndex;
                        }
                        MCCinfo = null;
                    }
                }

                #endregion 

                //[2015/05/13] HOST PPID 기준정보(Add by HS)
                #region "HostPPID"
                //DB로부터 PPID Body 기준정보를 읽어들여 저장한다.
                dstrSQL = "SELECT * FROM tbHOSTPPID";
                dDT = DBAct.clsDBAct.funSelectQuery(dstrSQL);                          //DataTable을 받아온다.
                if (dDT != null)
                {
                    PInfo.DeleteTable("HOSTPPID");
                    PInfo.AddDataTable("HOSTPPID", dDT);

                    foreach (DataRow dr in dDT.Rows)
                    {
                        PInfo.Unit(0).SubUnit(0).AddHOSTPPID(dr["HOSTPPID"].ToString().Trim());

                        PInfo.Unit(0).SubUnit(0).HOSTPPID(dr["HOSTPPID"].ToString().Trim()).EQPPPID = dr["EQPPPID"].ToString().Trim();
                        PInfo.Unit(0).SubUnit(0).HOSTPPID(dr["HOSTPPID"].ToString().Trim()).DateTime = dr["DTIME"].ToString().Trim();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                MessageBox.Show("UclsInfoActPlugIn subUnitInfoInitial중 Error발생!!" + ex.ToString(), "INFORM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //*******************************************************************************
        //  Function Name : subEQPInfoInitial()
        //  Description   : System.ini를 읽어 EQP 정보를 구성한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : INI로부터 데이타를 읽어 저장한다.
        //*******************************************************************************
        //  2006/10/12          어 경태         [L 00] 
        //  2007/02/26          김 효주         [L 01]  All(공통), EQP별로 구분 
        //*******************************************************************************
        private void subEQPInfoInitial(string strEQPName)
        {
            string dSystemINI = "";     //= Application.StartupPath + @"\system\System_" + strEQPName + ".ini";
            string dSecomINI = Application.StartupPath + @"\system\SEComINI.EXP";
            string dCFGFile = Application.StartupPath + @"\system\SDCA3.cfg";
            string dstrSectionUnit = "UNIT";
            string dstrSectionPort = "PORT";
            string dstrSectionHOST = "HOST";
            string dstrSectionInfo = "ETCInfo";
            string dstrSectionVCR = "VCRReadingResult";
            string dstrSectionMCC = "MCC";              //추가 : 20100101 이상호
            string dstrSectionSEM = "SEM";              //SEM 추가 - 110915 고석현
            string dstrSectionSecom = "EAP01";   //Secom Driver 이름
            string dstrSectionEQP = "CommunicationEQP";   //EQP 이름
            string dstrEQPID = "";
            string dstrKey = "";
            int dintCount = 0;

            try
            {

                string strBasicPath = Application.StartupPath;
                string[] arrCon = strBasicPath.Split('\\');
                strBasicPath = "";
                for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                {
                    if (arrCon[dintLoop] != "MCC")
                    {
                        if (dintLoop == arrCon.Length - 1)
                        {
                            strBasicPath += arrCon[dintLoop];
                        }
                        else
                        {
                            strBasicPath += arrCon[dintLoop] + "\\";
                        }
                    }
                }
                dSystemINI = strBasicPath + @"\system\System_" + strEQPName + ".ini";

                //EQP에 관계없이 응용 프로그램 공통으로 사용할 정보(All)를 구성한다.
                //if (this.PInfo.AddAll() == true)
                //{
                    //INI파일경로를 저장한다.
                    this.PInfo.All.SystemINIFilePath = dSystemINI;
                    PInfo.All.MCCINIFilePath = Application.StartupPath + @"\system\System.ini";

                    this.PInfo.All.SecomINIFilePath = dSecomINI;

                    //프로그램 기동시 HOST 로그 보관 기간을 91일(13주)로 Write한다.
                    //왜냐하면 SecomClient에서 91일(13주)은 설정이 안되기 때문임
                    //FunINIMethod.subINIWriteValue("EAP01", "LOGBACKUP", "91", this.PInfo.All.SecomINIFilePath);

                    //HOST정보를 INI에서 읽어 구조체이 저장한다.
                    this.PInfo.All.DeviceID = Convert.ToInt32(FunINIMethod.funINIReadValue("EAP01", "DEVICEID", "1", this.PInfo.All.SecomINIFilePath));
                    this.PInfo.All.RetryCount = Convert.ToInt32(FunINIMethod.funINIReadValue("EAP01", "RETRYCOUNT", "3", this.PInfo.All.SecomINIFilePath));
                    this.PInfo.All.LocalPort = FunINIMethod.funINIReadValue("EAP01", "LOCALPORTNUMBER", "7000", this.PInfo.All.SecomINIFilePath);

                    this.PInfo.All.T3 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T3", "45", this.PInfo.All.SecomINIFilePath));
                    this.PInfo.All.T5 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T5", "10", this.PInfo.All.SecomINIFilePath));
                    this.PInfo.All.T6 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T6", "5", this.PInfo.All.SecomINIFilePath));
                    this.PInfo.All.T7 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T7", "10", this.PInfo.All.SecomINIFilePath));
                    this.PInfo.All.T8 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSecom, "T8", "5", this.PInfo.All.SecomINIFilePath));
                    this.PInfo.All.T9 = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionHOST, "T9", "45", this.PInfo.All.SystemINIFilePath));

                    //VCR Reading Result 정보를 읽는다.
                    this.PInfo.All.VCRPass = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionVCR, "Pass", "0", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.VCRPMDT = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionVCR, "PMDT", "0", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.VCRMatch = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionVCR, "Match", "0", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.VCRMismatch = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionVCR, "Mismatch", "0", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.VCRKeyin = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionVCR, "Keyin", "0", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.VCRTimeout = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionVCR, "Timeout", "0", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.VCRSkip = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionVCR, "Skip", "0", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.VCRLastModified = FunINIMethod.funINIReadValue(dstrSectionVCR, "LastModified", "", this.PInfo.All.SystemINIFilePath);

                    //MMC Log Data 관련 설정을 읽어 온다.           //추가 : 20101001 이상호
                    this.PInfo.All.MCCNetworkPath = FunINIMethod.funINIReadValue(dstrSectionMCC, "NetworkPath", "Ftp://192.168.1.1/", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.MCCNetworkBasicPath = FunINIMethod.funINIReadValue(dstrSectionMCC, "NetworkBasicPath", "", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.MCCNetworkPort = FunINIMethod.funINIReadValue(dstrSectionMCC, "NetworkPort", "21", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.MCCNetworkUserID = FunINIMethod.funINIReadValue(dstrSectionMCC, "NetworkUserID", "USERID", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.MCCNetworkPassword = FunINIMethod.funINIReadValue(dstrSectionMCC, "NetworkPassword", "PASSWORD", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.MCCLootFilePath = FunINIMethod.funINIReadValue(dstrSectionMCC, "LootFilePath", @"C:\MCCLOG", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.MCCFileUploadTime = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionMCC, "MCCFileUploadTime", "0", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.MCCFileUploadUse = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrSectionMCC, "MCCFileUploadUse", "True", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.MCCLogFileDelete = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionMCC, "MCCLogFileDelete", "0", this.PInfo.All.SystemINIFilePath));

                    //SEM 관련 설정을 읽어온다. - 110915 고석현
                    this.PInfo.All.CommPort = FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_Port", "NULL", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.SEM_BaudRate = FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_BaudRate", "57600", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.SEMAlarmTime = Convert.ToInt32( FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_AlarmTime", "8", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.SEM_ErrorDelayCheckTime = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_ErrorDelayCheckTime", "10", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.All.SEM_ErrorCheckCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionSEM, "SEM_ErrorCheckCount", "60", this.PInfo.All.SystemINIFilePath));


                    //ETCInfo정보를 INI에서 읽어 구조체이 저장한다.
                    this.PInfo.All.MDLN = FunINIMethod.funINIReadValue(dstrSectionInfo, "MDLN", "", this.PInfo.All.SystemINIFilePath);
                    //this.PInfo.All.UserID = FunINIMethod.funINIReadValue(dstrSectionInfo, "UserID", "", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.All.CurrentLOTIndex = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionInfo, "CurrentLOTIndex", "0", this.PInfo.All.SystemINIFilePath));                 //현재까지 발번한 LOTIndex(1~999)
                    this.PInfo.All.SoftVersion = FunINIMethod.funINIReadValue(dstrSectionInfo, "SOFTREV", "", this.PInfo.All.SystemINIFilePath);

                    this.PInfo.All.LoginFalg = (FunINIMethod.funINIReadValue(dstrSectionInfo, "LoginFlag", "TRUE", this.PInfo.All.SystemINIFilePath).ToUpper() == "FALSE") ? false : true;

                    //모니터 해상도
                    this.PInfo.All.SizeWidth = 1024;
                    this.PInfo.All.SizeHeight = 768; // SystemInformation.PrimaryMonitorSize.Height;
                //}


                //EQPCount를 가져온다.
                dintCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPCount", "1", this.PInfo.All.SystemINIFilePath));

                //EQPCount 만큼 돌면서 기준정보를 저장한다.
                for (int dintLoop = 1; dintLoop <= dintCount; dintLoop++)
                {
                    dstrKey = "EQP" + dintLoop.ToString();
                    dstrEQPID = FunINIMethod.funINIReadValue(dstrSectionEQP, dstrKey, "Main", this.PInfo.All.SystemINIFilePath);
                    if (this.PInfo.AddEQP(dstrEQPID) == true)
                    {
                        //UNIT정보를 INI에서 읽어 구조체에 저장한다.
                        this.PInfo.EQP(dstrEQPID).UnitCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionUnit, "Count", "0", this.PInfo.All.SystemINIFilePath));

                        //PORT정보를 INI에서 읽어 구조체에 저장한다.
                        this.PInfo.EQP(dstrEQPID).SlotCount = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrSectionPort, "SlotCount", "25", this.PInfo.All.SystemINIFilePath));

                        //PLC정보를 INI에서 읽어 구조체에 저장한다.
                        this.PInfo.EQP(dstrEQPID).DummyPLC = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrEQPID, "Dummy", "true", this.PInfo.All.MCCINIFilePath));
                        this.PInfo.EQP(dstrEQPID).Type = FunINIMethod.funINIReadValue(dstrEQPID, "Type", "PLC", this.PInfo.All.MCCINIFilePath);
                        this.PInfo.EQP(dstrEQPID).WordStart = FunINIMethod.funINIReadValue(dstrEQPID, "Word1 Start", "W0000", this.PInfo.All.MCCINIFilePath);
                        this.PInfo.EQP(dstrEQPID).WordEnd = FunINIMethod.funINIReadValue(dstrEQPID, "Word1 End", "W0000", this.PInfo.All.MCCINIFilePath);
                        this.PInfo.EQP(dstrEQPID).BitScanCount = FunINIMethod.funINIReadValue(dstrEQPID, "Scan Area Count", "3", this.PInfo.All.MCCINIFilePath);

                        //Scan에 필요한 값들을 읽어온다. 어경태 20071119
                        this.PInfo.EQP(dstrEQPID).ScanTime = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrEQPID, "ScanTime", "200", PInfo.All.MCCINIFilePath));
                        this.PInfo.EQP(dstrEQPID).WorkingSizeMin = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrEQPID, "WorkingSizeMin", "1", PInfo.All.MCCINIFilePath));
                        this.PInfo.EQP(dstrEQPID).WorkingSizeMax = Convert.ToInt32(FunINIMethod.funINIReadValue(dstrEQPID, "WorkingSizeMax", "3", PInfo.All.MCCINIFilePath));

                        for (int dintIndex = 1; dintIndex <= 10; dintIndex++)
                        {
                            dstrKey = "Area" + dintIndex.ToString() + " ";
                            this.PInfo.EQP(dstrEQPID).BitScanEnabled[dintIndex] = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrEQPID, dstrKey + "Scan", "", this.PInfo.All.MCCINIFilePath));
                            this.PInfo.EQP(dstrEQPID).BitScanStart[dintIndex] = FunINIMethod.funINIReadValue(dstrEQPID, dstrKey + "Start", "", this.PInfo.All.MCCINIFilePath);
                            this.PInfo.EQP(dstrEQPID).BitScanEnd[dintIndex] = FunINIMethod.funINIReadValue(dstrEQPID, dstrKey + "End", "", this.PInfo.All.MCCINIFilePath);
                        }

                        //EQP
                        this.PInfo.EQP("Main").EQPID = FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPID", "EAP01", this.PInfo.All.SystemINIFilePath);
                        this.PInfo.EQP("Main").EQPName = FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPName", "WET ETCH", this.PInfo.All.SystemINIFilePath);
                        this.PInfo.EQP("Main").EQPType = FunINIMethod.funINIReadValue(dstrSectionInfo, "EQPType", "", this.PInfo.All.SystemINIFilePath);
                        this.PInfo.EQP("Main").RecipeCheck = Convert.ToBoolean(FunINIMethod.funINIReadValue(dstrSectionInfo, "RecipeCheck", "true", this.PInfo.All.SystemINIFilePath));
                    }

                    //검사기 PC 설정 ---------------------------------------------------------------------------------------------------------------------------------------
                    this.PInfo.EQP(dstrEQPID).DummyPC = Convert.ToBoolean(FunINIMethod.funINIReadValue("MCC_PC", "DummyPC", "true", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.EQP(dstrEQPID).LocalPort = FunINIMethod.funINIReadValue("MCC_PC", "LocalPort", "7050", this.PInfo.All.SystemINIFilePath);
                    this.PInfo.EQP(dstrEQPID).T3 = Convert.ToInt32(FunINIMethod.funINIReadValue("MCC_PC", "T3", "60", this.PInfo.All.SystemINIFilePath));
                    this.PInfo.EQP(dstrEQPID).RetryCount = Convert.ToInt32(FunINIMethod.funINIReadValue("MCC_PC", "RETRY", "3", this.PInfo.All.SystemINIFilePath));
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
