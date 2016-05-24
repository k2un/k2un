using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using CommonAct;
using System.Data;
using System.Globalization;
using Microsoft.VisualBasic;
using System.IO;
using System.IO.Ports;

namespace EQPAct
{
    public class clsEQPAct
    {
    	public string Version
        {
            get { return "SMD A2 DMS CLEANER V1.0"; }
        }
        
        //선언
        #region "선언"
        //외부 DLL인 구조체및 MTPLC의 정의
        public InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;                               //외부에서 여기서 사용할 구조체를 넣어줌
        private ECCAct.clsECC PMTPLC;                               //EQP PLC 관련 ECC
        private ECCAct.clsECC PCIM;                               //EQP PLC 관련 ECC

        private System.Threading.Thread pThreadCommand;             //자체 Queue를 검사하여 장비 PLC로 명령을 내리는 Thread 생성
        private System.Timers.Timer InformationReadTimer;
        private System.Timers.Timer InformationWriteTimer;

        private System.Threading.Thread pThreadDataCheck;
        private Queue pQueueDataCheck;

        //RS232/485 -------------------------------------------------
        public string PstrRS232CommPort;                            //통신 포트
        public string PstrRS232Settings;                            //통신 설정 "9600,n,8,1"

        SerialPort spSerialPort;// = new SerialPort();

        //TCP/IP ----------------------------------------------------
        public string PstrTCPIPAddress;
        public string PstrTCPIPPort;
        private string[] ReceiveDataFromNCB = { "" };

        //Common ----------------------------------------------------
        public string VersionDLL;                                   //DLL의 Version

        public string[] PstrBitScanStart = new string[11];          //PLC SCAN 어드레스 시작번지
        public string[] PstrBitScanEnd = new string[11];            //PLC SCAN 어드레스 끝번지
        public Boolean[] PbolBitScanEnabled = new Boolean[11];        //각각의 어드레스 범위를 check할것인지를 결정

        public string PstrWAreaStart;                               //Word Start Area 설정(Dummy에서만 사용)
        public string PstrWAreaEnd;                                 //Word End Area 설정(Dummy에서만 사용)
        public int PintWordRetry = 2;                               //Word영역 읽을시 실패했을때 재시도할 횟수
        public string PstrRemoteIP = "192.168.1.88";                //PLC IP
        public string PstrRemotePort = "2000";                      //PLC PORT

        public int PintScanTime = 50;
        public int PintWorkingSizeMin = 1;
        public int PintWorkingSizeMax = 100;

        public string PstrAddressPath;                              //AddressMAP(SYSTEM.mdb)이 있는 디렉토리
        public Boolean PbolDummyPLC = true;                         //Dummy인지의 여부

        public int PintNetwork = 111;                               //NetWork Number
        public short PshrStationNo = 0;                             //255;
        public short PshrBStationNo = 255;
        public short PshrWStationNo = 255;
        public short PshrRStationNo = 255;
        public short PshrDStationNo = 255;
        public short PshrZStationNo = 255;
        public short PshrZRStationNo = 255;
        public short PshrMStationNo = 255;
        public short PshrTNStationNo = 255;

        private Queue pReadDataQueue = new Queue();
        private int pintTotalReadLength = 0;
        private int pPPIDReadCount = 15;                            //한번에 PLC로 부터 읽어오는(가져오는) PPID 개수
        private string PstrMCCBitReadDataBackup = "";               //MCC Bit Read Data Backup
        private long plngSEMAlarmReportTick = 0;
        private object SyncRoot;
        private object lock_DataCheck;

        //[2015/06/11](Add by HS)
        private StringBuilder pstrMCCInformationData = new StringBuilder();

        private string pstrTimeNow = "";

        /// <summary>
        /// TEST를 위한 임시변수        MMC Log 작업을 하기 위한 변수 //추가 : 20101001 이상호
        /// </summary>
        //private int pintTempMCCLogCount = 0;
        //private bool TestCheckFlag = true;
        #endregion

        private StringBuilder psbBuffer = new StringBuilder();




        //부모폼에서의 호출메소드(Open and Close)
        #region"Open and Close"

        //*******************************************************************************
        //  Function Name : subOpenPLC()
        //  Description   : PLC Open(PLC의 Scan영역등의 속성값을 설정한 후 오픈한다)
        //  Parameters    : 
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/09/22          어 경태         [L 00] 
        //*******************************************************************************
        public Boolean funOpenPLC(EnuEQP.CommunicationType intCommunication)
        {
            Boolean dbolReturn = false;
            string dstrKey;

            try
            {
                this.PMTPLC = new ECCAct.clsECC();
                this.SyncRoot = this.pReadDataQueue.SyncRoot;
                lock_DataCheck = new object();
                //AddressMAP(SYSTEM.MDB)이 있는 디렉토리를 지정한다    

                string strBasicPath = Application.StartupPath;
                string[] arrCon = strBasicPath.Split('\\');
                strBasicPath = string.Empty;
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

                string strMDBPath = strBasicPath + @"\system\System_" + PInfo.All.MODEL_NAME + ".mdb";

                //PMTPLC.proPlcDBPath = PstrAddressPath + @"\System.mdb";
                PMTPLC.proPlcDBPath = strMDBPath;

                #region PLC용
                PMTPLC.proEQPCommandType = ECCCommonAct.EnuCommunication.EQPCommandType.PLC;                   //PLC형 Command사용
                PMTPLC.proEQUIPMENT = ECCCommonAct.EnuCommunication.EQPNameType.MelsecPLC;                     //장비선택
                PMTPLC.proCommunicationType = ECCCommonAct.EnuCommunication.CommunicationType.MXComponent;     //통신방식 선택
                PMTPLC.proLogicalStationNumber = 254;     //MX Component에서 설정한 Logical Station Number(NETG 용임)

                PMTPLC.proWord1Start = PstrWAreaStart;
                PMTPLC.proWord1End = PstrWAreaEnd;

                //////////////////////////////// SCAN 설정 ///////////////////////////////////////////////
                PMTPLC.proScanTime = PintScanTime;

                PMTPLC.proAreaScan = new Boolean[11];
                PMTPLC.proAreaStart = new String[11];
                PMTPLC.proAreaEnd = new String[11];
                PMTPLC.proValue = new String[11];

                for (int dintIndex = 1; dintIndex <= 10; dintIndex++)
                {
                    dstrKey = "Area" + dintIndex.ToString() + " ";
                    PMTPLC.proAreaScan[dintIndex] = PbolBitScanEnabled[dintIndex];
                    PMTPLC.proAreaStart[dintIndex] = PstrBitScanStart[dintIndex];
                    PMTPLC.proAreaEnd[dintIndex] = PstrBitScanEnd[dintIndex];
                }
                //////////////////////////////////////////////////////////////////////////////////////////////

                PMTPLC.proDummy = PbolDummyPLC;     //Dummy(True)/Real(False)

                //OPEN
                dbolReturn = PMTPLC.funOpenConnection();
                #endregion

                this.PCIM = new ECCAct.clsECC();
                this.SyncRoot = this.pReadDataQueue.SyncRoot;
                //AddressMAP(SYSTEM.MDB)이 있는 디렉토리를 지정한다                
                PCIM.proPlcDBPath = PstrAddressPath + @"\System.mdb";

                #region PC
                //PCIM.proPlcDBPath = PstrAddressPath + @"\System.mdb";
                PCIM.proPlcDBPath = strMDBPath;
                PCIM.proEQPCommandType = ECCCommonAct.EnuCommunication.EQPCommandType.PC;                    //PC형 Command사용
                PCIM.proEQUIPMENT = ECCCommonAct.EnuCommunication.EQPNameType.Normal;                          //장비선택
                PCIM.proCommunicationType = ECCCommonAct.EnuCommunication.CommunicationType.TCPIP;     //통신방식 선택

                PCIM.proDummy = this.PInfo.EQP("Main").DummyPC;

                //Stansdard
                PCIM.proTimeOutTransfer = 5000;
                PCIM.proRetryTransferCount = 0;

                //TCP 통신 방법에 대한 설정
                PCIM.proTCPRemoteIP = "127.0.0.1"; //this.PInfo.EQP("Main").LocalIP;
                PCIM.proTCPRemotePort = Convert.ToInt32(this.PInfo.EQP("Main").LocalPort);
                PCIM.proTCPLocalPort = Convert.ToInt32(this.PInfo.EQP("Main").LocalPort);     //Server일 경우

                PCIM.proTCPSendBufferSize = 1024;
                PCIM.proTCPReceiveBufferSize = 1024;
                PCIM.proTCPReceiveDataType = ECCCommonAct.EnuCommunication.ReceiveDataType.ASCII;          //Data를 수신하면 ASC로 받는다.
                PCIM.proTCPConnectionMode = ECCCommonAct.EnuCommunication.ConnectionMode.Active;          //Server

                PCIM.proEQPName = "MCC->CIM";
                dbolReturn = PCIM.funOpenConnection();
                #endregion

                //자체 Queue를 검사하여 장비 PLC로 명령을 내리는 Thread 생성
                this.pThreadCommand = new Thread(new ThreadStart(PLCWriteMethodThread));
                this.pThreadCommand.Name = "PLCWriteCommand";
                this.pThreadCommand.Start();

                this.InformationReadTimer = new System.Timers.Timer();
                this.InformationReadTimer.Elapsed += new ElapsedEventHandler(InformationReadTimer_Elapsed);
                this.InformationReadTimer.Interval = 1000;       //2000ms
                this.InformationReadTimer.Enabled = true;

                this.InformationWriteTimer = new System.Timers.Timer();
                this.InformationWriteTimer.Elapsed += new ElapsedEventHandler(InformationWriteTimer_Elapsed);
                this.InformationWriteTimer.Interval = 100;       //2000ms
                this.InformationWriteTimer.Enabled = true;

                pQueueDataCheck = new Queue();

                this.pThreadDataCheck = new Thread(new ThreadStart(PC_DataCheck));
                this.pThreadDataCheck.Name = "PC_DataCheck";
                this.pThreadDataCheck.Start();
                
                //subStartCIMmsgThread();     // lsc
                //subSendSetValue();

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intCommunication:" + intCommunication);
            }
            return dbolReturn;
        }

        delegate void subReceiveEvent(string a, string b, string c);

        void PC_DataCheck()
        {
            string strData = "";
            string[] arrData;
            do
            {
                lock (this.lock_DataCheck)
                {
                    try
                    {
                        if (pQueueDataCheck.Count > 0)
                        {
                            strData = pQueueDataCheck.Dequeue().ToString();

                            arrData = strData.Split(new char[] { ';' });
                            //subReceiveEvent del = this.subPC_ReceiveEvent;
                            //del(arrData[1], arrData[2], arrData[3]);
                            PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Start - " + DateTime.Now.ToString("HH:mm:ss.fff"));
                            subPC_ReceiveEvent(arrData[1], arrData[2], arrData[3]);
                            PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "End - " + DateTime.Now.ToString("HH:mm:ss.fff"));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    }
                    finally
                    {
                        strData = string.Empty;
                    }

                    Thread.Sleep(50);
                }


            } while (true);
        }
		

        void InformationWriteTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string dstrMCCInformationData = string.Empty;

            try
            {
                if (DateTime.Now.ToString("ss") != pstrTimeNow)
                {
                    InformationWriteTimer.Enabled = false;
                    pstrMCCInformationData.Append(DateTime.Now.ToString("MMdd_HHmm_ss.fff") + ",I,");

                    //[2015/02/23]반복되는 Count변경(Add by HS)
                    for (int dintLoop = 0; dintLoop < PInfo.Unit(0).SubUnit(0).MCCInfoCount; dintLoop++)//PInfo.Unit(0).SubUnit(0).MCCInfoCount; dintLoop++)
                    {
                        pstrMCCInformationData.Append(PInfo.Unit(0).SubUnit(0).MCCInfo(dintLoop + 1).MCCValue + ",");
                    }
                
                    pstrMCCInformationData.Remove(pstrMCCInformationData.Length - 1, 1);
                    dstrMCCInformationData = pstrMCCInformationData.ToString();
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.MCCLog, dstrMCCInformationData);
                    pstrTimeNow = DateTime.Now.ToString("ss");
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                dstrMCCInformationData = string.Empty;
                pstrMCCInformationData.Clear();
                InformationWriteTimer.Enabled = true;
            }
        }

        void InformationReadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string dstrReadData = "";
            string dstrTemp = "";
            try
            {
                InformationReadTimer.Enabled = false;
                dstrReadData = funWordRead("W3000", PInfo.Unit(0).SubUnit(0).MCCInfoReadCount * 2, EnuEQP.PLCRWType.Hex_Data);

                for (int dintLoop = 0; dintLoop < dstrReadData.Length; dintLoop += 8)
                {
                    dstrTemp = dstrReadData.Substring(dintLoop, 8);
                    dstrTemp = dstrTemp.Substring(4, 4) + dstrTemp.Substring(0, 4);
                    dstrTemp = CommonAct.FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);

                    PInfo.Unit(0).SubUnit(0).MCCInfo((dintLoop / 8) + 1).MCCValue = dstrTemp;
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                InformationReadTimer.Enabled = true;
            }
        }

        //*******************************************************************************
        //  Function Name : funClosePLC()
        //  Description   : 사용중인 Thread를 Abort하고 PLC를 닫는다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public Boolean funClosePLC()
        {
            Boolean dbolReturn = false;

            try
            {
                PMTPLC.funCloseConnection();
                if (this.pThreadCommand != null) this.pThreadCommand.Abort();

                subStopCIMmsgThread();

                dbolReturn = true;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            return dbolReturn;
        }
      
        #endregion

        //자체적으로 발생하는 이벤트 함수(Timer)
        #region"Timer"

        private void subPCEvent()
        {
            string dstrDataPLC = "";
            string[] darrEvent;

            try
            {
                if (PCIM != null) dstrDataPLC = PCIM.funEventData();                    //PLC의 Event를 읽어온다.
                //if (dstrDataPLC == "" || dstrDataPLC == null) return;
                //darrEvent = dstrDataPLC.Split(new char[] { ';' });

                //검사기 PC Event 처리
                if (string.IsNullOrEmpty(dstrDataPLC) == false)
                {
                    darrEvent = dstrDataPLC.Split(new char[] { ';' });

                    switch (darrEvent[0])
                    {
                        case "ErrorEvent":
                            subPC_PCError(darrEvent[1], darrEvent[2], darrEvent[3]);
                            break;
                        case "ReceiveEvent"://[2015/04/23]구분자추가 darrEvent[1]=Date, darrEvent[2]=DataType, darrEvent[3]=Value(Add by HS)
                            pQueueDataCheck.Enqueue(dstrDataPLC);
                            //subPC_ReceiveEvent(darrEvent[1], darrEvent[2], darrEvent[3]);
                            break;
                        case "TransferEvent":
                            subPC_TransferEvent(darrEvent[1], darrEvent[2]);
                            break;
                        case "ConnectionEvent":
                            PMTEQP_PCConnect(darrEvent[1]);
                            break;
                        case "DisConnectionEvent":
                            PMTEQP_PCDisConnect(darrEvent[1]);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                dstrDataPLC = string.Empty;
            }
        }

        private void subPC_PCError(string strDateTime, string strErrorIndex, string strErrDesc)
        {
            try
            {
                string dstrTemp = "Error_Index:" + strErrorIndex + ",Err_Descr:" + strErrDesc;
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strDateTime, dstrTemp);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strErrorIndex:" + strErrorIndex + ", strErrDesc:" + strErrDesc);
            }
        }

        private void subPC_TransferEvent(string strDateTime, string strValue)
        {
            try
            {
                //strValue = strValue.Remove(strValue.Length);
                //EQP.Log에 로그 출력
                string dstrTemp = "SEND => Value : " + strValue.PadRight(50);
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strDateTime, dstrTemp);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strValue:" + strValue);
            }
        }

        /// <summary>
        /// Message between Stage Master and CIM ([Format : cnt|define|data1|data2(,,,)| + chr13 + chr10]) # Data 구분자는 '|'로 한다.
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <param name="strValue"></param>
        private void subPC_ReceiveEvent(string strDateTime, string strDatatype, string strValue)
        {
            //int dintTemp = 0;
            string dstrMessage = "";
            string[] arrCon;
            string[] arrValue;
            try
            {
                switch (strDatatype.ToUpper())
                {
                    case "INFOMATION":
                        {
                            dstrMessage = strValue.Trim();
                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strDateTime, "RECV => Value : " + dstrMessage);
                            arrCon = dstrMessage.Split(',');

                            for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                            {
                                arrValue = arrCon[dintLoop].Split('=');

                                if (arrValue.Length < 2 || arrValue[1] == null || arrValue[1] == "0")
                                {
                                    PInfo.Unit(0).SubUnit(0).MCCInfo(Convert.ToInt32(arrValue[0])).MCCValue = "0";
                                }
                                else
                                {
                                    PInfo.Unit(0).SubUnit(0).MCCInfo(Convert.ToInt32(arrValue[0])).MCCValue = arrValue[1];
                                }

                                arrValue = null;
                            }
                            arrCon = null;

                            break;
                        }
                    case "EVENT":
                        {
                            subEventLogProcess(strValue);
                            break;
                        }
                    case "ERROR":
                    case "WARNING":
                        {
                            subAlarmLogProcess(strDatatype, strValue);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strValue:" + strValue);
            }
            finally
            {
                dstrMessage = string.Empty;
            }
        }

        private void subMCCInfoLogData(string dstrMessage)
        {
            string dstrMCCLogData = "";
            string dstrDataTime = "";
            string[] arrVaule;
            try
            {
                if (string.IsNullOrEmpty(dstrMessage)) return;
                arrVaule = dstrMessage.Split(',');


                if (string.IsNullOrEmpty(arrVaule[1]) == false)
                {
                    dstrDataTime = arrVaule[1];
                }
                else
                {
                    dstrDataTime = DateTime.Now.ToString("MMdd_HHmm_ss.fff");
                }
                dstrMCCLogData = dstrDataTime + ",I,";

                for (int dintLoop = 0; dintLoop < arrVaule.Length-3; dintLoop++)
                {
                    dstrMCCLogData += arrVaule[dintLoop + 3] + ",";
                }
                dstrMCCLogData = dstrMCCLogData.Substring(0, dstrMCCLogData.Length - 1);
                
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.MCCLog, dstrMCCLogData);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void PMTEQP_PCConnect(string strDateTime)
        {
            try
            {
                this.PInfo.EQP("Main").MainEQPConnect = true;
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strDateTime, "검사기 PC Connect-" + PMTPLC.proVersion);

                PMTPLC.subTransfer("Connect");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void PMTEQP_PCDisConnect(string strDateTime)
        {
            try
            {
                this.PInfo.EQP("Main").MainEQPConnect = false;
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strDateTime, "검사기 PC DisConnect-" + PMTPLC.proVersion);
                //PMTPLC.subTransfer("Connect");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion

        #region =================== MemTest Methods ===================
        private DateTime _started = DateTime.Now;
        private enum MemorySizeType { Byte, KB, MB, GB, TB }

        private string funGetProcessTime()
        {
            try
            {
                TimeSpan ts = System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime;

                return string.Format("CPU Time: {0}일 {1}시간 {2}분 {3}초", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string funGetThreadCount()
        {
            try
            {
                int dintThreadCount = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;

                return string.Format("실행중인 쓰레드: {0}", dintThreadCount);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string funGetMemorySize()
        {
            try
            {
                int i = 0;
                Int64 usageMemory = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
                Int64 um = usageMemory;

                while (usageMemory > 1024L)
                {
                    um = usageMemory;
                    usageMemory = (Int64)(usageMemory / 1024L);
                    i++;
                }
                um = um % 1024L;

                MemorySizeType sizeType = (MemorySizeType)i;

                return string.Format("메모리 사용량: {0} {1} {2} {3}", usageMemory, sizeType.ToString(), um, ((MemorySizeType)i - 1).ToString());

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        private string funGetRunningTime()
        {
            try
            {
                TimeSpan ts = DateTime.Now - this._started;
                return string.Format("CIM RUN 시간: {0}일 {1}시간 {2}분 {3}초", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private int pintMemoryLogCount = 0;


        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool SetProcessWorkingSetSize(IntPtr handle, int min, int max);


        public void subMemoryLog()
        {
            try
            {
                this.pintMemoryLogCount--;

                if (this.pintMemoryLogCount > 0) return;


                string dstrLog = funGetRunningTime() + " => " + funGetMemorySize() + ", " + funGetThreadCount();// +", " + funGetProcessTime();

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrLog);

                this.pintMemoryLogCount = 299;


                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);


            }
            catch (Exception)
            {
            }
        }
        #endregion


        //PLC로부터의 Event를 처리
        #region"PLC Event Process"

        //*******************************************************************************
        //  Function Name : subPLCEvent_Seek()
        //  Description   : 발생한PLC Event를 ACT를 찾아 ACT함수로 분기 시킨다
        //  Parameters    : strPLCAddr      => 발생한 PLC Address
        //                  intBitVal       => 변화 값 0 or 1
        //                  intEventIndex   => 0:EventBit, 1:Status Bit
        //  Return Value  : None
        //  Special Notes : Event는 0에서 1로 변할때만 들어오고 Status는 0과 1 모두 들어온다.
        //*******************************************************************************
        //  2006/10/16          어 경태         [L 00] 
        //*******************************************************************************
        public void subPLCEvent_Seek(string strPLCAddr, int intBitVal)
        {
            try
            {
                string dstrCompBit = PMTPLC.AddressMapBitHash(strPLCAddr).CompBit;
                string dstrACTName = PMTPLC.AddressMapBitHash(strPLCAddr).ACTName;
                string dstrACTFrom = PMTPLC.AddressMapBitHash(strPLCAddr).ACTFrom;
                string dstrACTFromSub = PMTPLC.AddressMapBitHash(strPLCAddr).ACTFromSub;
                string dstrACTVal = PMTPLC.AddressMapBitHash(strPLCAddr).ACTVal;
                int dintActFrom = Convert.ToInt32(dstrACTFrom.PadLeft(1, '0'));

                switch (dstrACTName)
                {
                    //MCC
                    case "actMCCState":                                     //MCC비트 이벤트(log를 작성)
                        subMCCState(intBitVal, dstrACTFrom, dstrACTFromSub);
                        break;

                    case "actNewAlram":
                        subACTNewAlram(intBitVal, dstrACTFrom, dstrACTVal);
                        break;

                    case "actAlarm":
                        subACTAlarm(dstrCompBit, dstrACTVal, 0);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strPLCAddr:" + strPLCAddr + ", intBitVal:" + intBitVal);
            }
        }

        #region"DLL Event Process"
        //*******************************************************************************
        //  Function Name : PLCWriteMethodThread()
        //  Description   : 자체 Queue에 있는 명령을 PLC로 Write한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : None
        //*******************************************************************************
        //  2006/11/02         김 효주          [L 00] 
        //*******************************************************************************
        void PLCWriteMethodThread()
        {
            do
            {
                lock (this.SyncRoot)
                {
                    try
                    {
                        subPLCEvent();      //Event 처리를 한다.

                        Thread.Sleep(1);
                        subPCEvent();
                    }
                    catch (Exception ex)
                    {
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    }

                    Thread.Sleep(1);
                }


            } while (true);
        }

        #endregion


        //PLC로 명령을 내림
        #region "PLC로 명령을 내림"

        //*******************************************************************************
        //  Function Name : subACTAlarm()
        //  Description   : 설비에서 알람 발생 및 해제시 Host로 보고한다
        //  Parameters    : strStatus: 발생(Set)/해제(Reset)
        //  Return Value  : None
        //  Special Notes : Heavy Alarm이 발생, 해제하면 Alarm정보를 누적 저장하여
        //                  S6F11(CEID=51, 53) 보고시 사용한다.
        //*******************************************************************************
        //  2007/10/22          김 효주         [L 00]
        //*******************************************************************************
        private void subACTAlarm(string strCompBit, string strStatus, int dintResetAlarm)
        {
            //int dintAlarmID = 0;
            //string dstrModuleID = "";
            //string dstrWordAddress = "";
            ////string dstrAlarmType = "";
            ////int dintAlarmCode = 0;
            //string dstrAlarmDesc = "";
            ////Boolean dbolAlarmReport = false;
            ////string dstrAlarmTime = "";   //yyyy-MM-dd HH:mm:ss
            ////string dstrAlarmMsg = "";
            //////int dintHeavyAlarmCount = 0;

            //////발생한 모든 알람을 저장할 변수
            ////string[] dAlarmSplit;           //발생한 모든 알람을 저장하는 이전 변수
            ////string dNowAlarm = "";          //한개의 알람이 해제 된 후 남아있는 알람을 저장할 변수

            //string dstrStepID = "";
            //string dstrGLSID = "";
            //string dstrLOTID = "";
            //string dstrPPID = "";
            //int dintUnitID = 0;
            ////int dintSlotID = 0;
            //string dstrValue = "";
            //string dstrMCCType = "";

            try
            {
                ////MCC Log 기록 

                //if (strStatus == "Set") dstrWordAddress = "W1500";
                //else dstrWordAddress = "W1502";


                //dintAlarmID = Convert.ToInt32(funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data));

                //dstrModuleID = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).ModuleID;
                //dstrModuleID = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);

                //dstrAlarmDesc = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc;

                //dintUnitID = this.PInfo.funGetModuleIDToUnitID(dstrModuleID);

                //dstrPPID = funWordRead("W1504", 10, EnuEQP.PLCRWType.ASCII_Data);

                //strStatus = strStatus.ToUpper();
                //dstrValue = strStatus.ToUpper() + "=" + dintAlarmID + "=" + dstrAlarmDesc.ToUpper();

                //if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                //{
                //    dstrMCCType = "E";
                //}
                //else
                //{
                //    dstrMCCType = "W";
                //}

                //subMCCLogData(dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strStatus:" + strStatus);
            }
            finally
            {
                //if(System.Diagnostics.Process.GetProcessesByName("STM").Length == 0) subSetConfirmBit(strCompBit);
            }
        }



        private void subACTNewAlram(int intBitVal, string dstrACTFrom, string dstrACTVal)
        {
            //int dintAlarmID = 0;
            ////string dstrAlarmTime = "";
            ////string dstrAlarmMsg = "";
            //string strStatus = "";

            ////int dintUnitID = 0;
            //string dstrGLSID = "";
            //string dstrLOTID = "";
            ////int dintSlotID = 0;
            //string dstrPPID = "";
            //string dstrStepID = "";
            //string dstrValue = "";
            //string dstrMCCType = "";
            //string dstrModuleID = "";

            try
            {
                //dintAlarmID = Convert.ToInt32(dstrACTFrom);
                //InfoAct.clsAlarm CurrentAlram = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID);
                //strStatus = (intBitVal == 1) ? "SET" : "RESET";

                ////MCC Log 기록 111221 고석현
                //dstrModuleID = CurrentAlram.ModuleID;
                //dstrPPID = funWordRead("W1504", 10, EnuEQP.PLCRWType.ASCII_Data);

                //dstrValue = strStatus.ToUpper() + "=" + dintAlarmID + "=" + CurrentAlram.AlarmDesc.ToUpper();


                //if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                //{
                //    dstrMCCType = "E";
                //}
                //else
                //{
                //    dstrMCCType = "W";
                //}

                //dstrModuleID = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);

                //subMCCLogData(dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// MCC Bit를 읽어서 On되었을때만 Log를 기록한다
        /// </summary>
        /// <remarks>
        /// 20101103            김중권          [L 00]
        /// </remarks>
        private void subMCCState(int intBitVal, string dstrACTVal, string dstrAVTSubVal)
        {
            int dintIndex = Convert.ToInt32(dstrACTVal);
            string dstrModuleID = this.PInfo.Unit(0).SubUnit(0).MCC(dintIndex).ModuleID;
            int dintUnitID = 0;         // PInfo.funGetModuleIDToUnitID(dstrModuleID);
            //int dintSlotID = 0;
            string dstrGLSID = "";
            string dstrLOTID = "";
            string dstrPPID = "";
            string dstrStepID = "";
            string dstrMCCType = "";
            string[] dstrValue = new string[4];
            string dstrWordAddress = "W3400";
            string[] dstrMCCValue = null;
            string dstrStepNo = "";
            string dstrFilmFlag = "";
            InfoAct.clsMCC CurrentMCC = null;

            int dintModuleNo = 0;
            try
            {
                //[2015/04/09]변경(Add by HS)
                CurrentMCC = this.PInfo.Unit(0).SubUnit(0).MCC(dintIndex);
                dstrModuleID = CurrentMCC.ModuleID;

                //[2015/04/09]변경(Add by HS)
                dstrMCCType = CurrentMCC.MCCType;

                switch (dstrModuleID)
                {
                    case "A3TLM02S_TLMU_LMD1_FI01":
                        dintModuleNo = 1;
                        break;

                    case "A3TLM02S_TLMU_LMD1_FI02":
                        dintModuleNo = 2;
                        break;

                    case "A3TLM02S_TLMU_LMD1_FO03":
                        dintModuleNo = 3;
                        break;

                    case "A3TLM02S_TLMU_LMD1_FO04":
                        dintModuleNo = 4;
                        break;

                    case "A3TLM02S_TLMU_LMD2_PI01":
                        dintModuleNo = 5;
                        break;

                    case "A3TLM02S_TLMU_LMD2_PO02":
                        dintModuleNo = 6;
                        break;

                    case "A3TLM02S_TLMU_FT01":
                        dintModuleNo = 7;
                        break;

                    case "A3TLM02S_TLMU_FT02":
                        dintModuleNo = 8;
                        break;

                    case "A3TLM02S_TLMU_AL01":
                        dintModuleNo = 9;
                        break;

                    case "A3TLM02S_TLMU_LM01":
                        dintModuleNo = 10;
                        break;

                    case "A3TLM02S_TLMU_DM01":
                        dintModuleNo = 11;
                        break;

                    case "A3TLM02S_TLMU_IS01":
                        dintModuleNo = 12;
                        break;

                    case "A3TLM02S_TLMU_ST01":
                        dintModuleNo = 13;
                        break;

                    case "A3TLM02S_TLMU_ST02":
                        dintModuleNo = 14;
                        break;

                    case "A3TLM02S_TLMU_GL01":
                        dintModuleNo = 15;
                        break;
                }

                if (dstrMCCType != "I")
                {
                    #region PLC 영역 Read 시나리오
//                    dstrWordAddress = funAddressAdd(dstrWordAddress, 64 * (Convert.ToInt32(CurrentMCC.GroupNo) - 1));
//                    subWordReadSave(dstrWordAddress, 4, EnuEQP.PLCRWType.ASCII_Data);   //Step ID
//                    subWordReadSave("", 28, EnuEQP.PLCRWType.ASCII_Data);               //Film ID / Glass ID
//                    subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                //Lot ID
//                    subWordReadSave("", 10, EnuEQP.PLCRWType.ASCII_Data);               //PPID
//                    subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                  //StepNo
//                    subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                  //Film Flag
 //                   dstrMCCValue = funWordReadAction(true);
                    #endregion

                    #region CIM 영역 Read 시나리오
                    dstrWordAddress = "W1A00";
                    dstrWordAddress = funAddressAdd(dstrWordAddress, 64 * (dintModuleNo - 1));
                    subWordReadSave(dstrWordAddress, 4, EnuEQP.PLCRWType.ASCII_Data);   //Step ID
                    subWordReadSave("", 28, EnuEQP.PLCRWType.ASCII_Data);               //Film ID / Glass ID
                    subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                //Lot ID
                    subWordReadSave("", 10, EnuEQP.PLCRWType.ASCII_Data);               //PPID
                    subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                  //StepNo
                    subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                  //Film Flag
                    dstrMCCValue = funWordReadAction(true);
                    #endregion

                    dstrStepID = dstrMCCValue[0];
                    dstrGLSID = dstrMCCValue[1];
                    dstrLOTID = dstrMCCValue[2];
                    dstrPPID = dstrMCCValue[3];
                    dstrStepNo = dstrMCCValue[4];
                    dstrFilmFlag = dstrMCCValue[5];

                    //[2015/04/09]변경(Add by HS)
                    dstrValue[0] = CurrentMCC.MCCName;
                    dstrValue[1] = CurrentMCC.FromPosition;   //FromPositon
                    dstrValue[2] = CurrentMCC.ToPosition;     //ToPosition

                    if (intBitVal == 1)
                    {
                        dstrValue[3] = "START";

                        // LD01을 제외한 Component In에 대한 보고는 하지 않는다.
                        if (dstrAVTSubVal != "0")
                        {
                            subMCCLogData("", dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);
                        }
                    }
                    else
                    {
                        dstrValue[3] = "END";

                        // LD01을 제외한 Component In에 대한 보고는 하지 않는다.
                        if (dstrAVTSubVal != "0")
                        {
                            subMCCLogData("", dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);
                        }
                    }
                    CurrentMCC = null;
                }
                else
                {
                    dstrValue[0] = this.PInfo.Unit(0).SubUnit(0).MCC(dintIndex).MCCName;
                    dstrWordAddress = "W3400";
                    dstrWordAddress = funAddressAdd(dstrWordAddress, 2 * (dintIndex - 1));
                    dstrValue[1] = funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);
                    subMCCLogData("", dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

        }
        //[2015/04/23]Event Log(Add by HS)
        /// <summary>
        /// Event Log
        /// </summary>
        /// <param name="dstrAVTSubVal">값</param>
        private void subEventLogProcess( string strValue)
        {
            string dstrModuleID = "";
            string dstrGLSID = "";
            string dstrLOTID = "";
            string dstrPPID = "";
            string dstrStepID = "";
            string dstrMCCType = "V";
            string[] darrstrValue = null;
            string[] dstrValue = new string[5];
            string dstrPosition = "";
            string dstrProcessState_Old = "";
            string dstrProcessState = "";
            string dstrEQPState_Old = "";
            string dstrEQPState = "";
            string dstrAlarmID = "";
            string dstrAlarmText = "";
            string dstrPPID_Old = "";
            string dstrPPID_New = "";
            string dstrFrom = "";
            string dstrTo = "";

            try
            {
                darrstrValue = strValue.Split(',');

                dstrModuleID = darrstrValue[1];
                dstrMCCType = "V";
                dstrStepID = darrstrValue[2];
                dstrGLSID = darrstrValue[3];
                dstrLOTID = darrstrValue[4];
                dstrPPID = darrstrValue[5];

                //[2015/05/15]Event 추가(Add by HS)
                switch (darrstrValue[0])
                {
                    case "CEID_15":
                        dstrValue[1] = "JUDGEMENT_REPORT";
                        break;
                    case "CEID_16":
                        dstrValue[1] = "COMPONENT_IN";
                        break;
                    case "CEID_17":
                        dstrValue[1] = "COMPONENT_OUT";
                        break;
                    case "CEID_18":
                        dstrValue[1] = "SCRAP";
                        break;
                    case "CEID_31":
                        dstrValue[1] = "LOAD_REQUEST";
                        break;
                    case "CEID_32":
                        dstrValue[1] = "PRE_LOAD";
                        break;
                    case "CEID_33":
                        dstrValue[1] = "LOAD_COMPLETE";
                        break;
                    case "CEID_34":
                        dstrValue[1] = "UNLOAD_REQUEST";
                        break;
                    case "CEID_35":
                        dstrValue[1] = "UNLOAD_COMPLETE";
                        break;
                    case "CEID_45":
                        dstrValue[1] = "FILM_CASE_MOVING";
                        break;
                    case "CEID_51":
                        {
                            dstrProcessState_Old = darrstrValue[6];
                            dstrProcessState = darrstrValue[7];

                            dstrValue[0] = darrstrValue[0];
                            dstrValue[1] = dstrProcessState_Old;
                            dstrValue[2] = dstrProcessState;
                        }
                        break;
                    case "CEID_53":
                        {
                            dstrEQPState_Old = darrstrValue[6];
                            dstrEQPState = darrstrValue[7];
                            dstrAlarmID = darrstrValue[8];
                            dstrAlarmText = darrstrValue[9];

                            dstrValue[0] = darrstrValue[0];
                            dstrValue[1] = dstrEQPState_Old;
                            dstrValue[2] = dstrEQPState;
                            dstrValue[3] = dstrAlarmID;
                            dstrValue[4] = dstrAlarmText;
                        }
                        break;
                    case "CEID_131":
                        {
                            dstrPPID_Old = darrstrValue[6];
                            dstrPPID_New = darrstrValue[7];

                            dstrValue[0] = darrstrValue[0];
                            dstrValue[1] = dstrPPID_Old;
                            dstrValue[2] = dstrPPID_New;
                        }
                        break;
                    case "CEID_1015":
                        dstrValue[1] = "FILM_SCRAP";
                        break;
                    case "CEID_1016":
                        dstrValue[1] = "COMPONENT_IN_FILM";
                        break;
                    case "CEID_1017":
                        dstrValue[1] = "COMPONENT_OUT_FILM";
                        break;
                    default:
                        return;
                }
                if (dstrValue[1].Contains("COMPONENT") == true)
                {
                    if (dstrValue[1].Contains("IN") == true)
                    {
                        switch (dstrModuleID.Substring(dstrModuleID.Length - 4, 4))
                        {
                            case "FT01":
                                dstrFrom = "FI02";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            case "FT02":
                                dstrFrom = "AL01";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            case "AL01":
                                dstrFrom = "FT01";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            case "LM01":
                                dstrFrom = "FT02";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            case "DM01":
                                dstrFrom = "LM01";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            case "IS01":
                                dstrFrom = "LM01";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            case "ST01":
                                dstrFrom = "GL01";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            case "ST02":
                                dstrFrom = "GL01";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            case "GL01":
                                dstrFrom = "EX03";
                                dstrTo = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (dstrModuleID.Substring(dstrModuleID.Length - 4, 4))
                        {
                            case "FT01":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "AL01";
                                break;
                            case "FT02":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "LM01";
                                break;
                            case "AL01":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "FT02";
                                break;
                            case "LM01":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "ST01";
                                break;
                            case "DM01":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "PO02";
                                break;
                            case "IS01":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "LM01";
                                break;
                            case "ST01":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "EX04";
                                break;
                            case "ST02":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "EX04";
                                break;
                            case "GL01":
                                dstrFrom = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                                dstrTo = "ST01";
                                break;
                            default:
                                break;
                        }
                    }

                    dstrValue[0] = darrstrValue[0];
                    dstrValue[2] = dstrFrom;   //FromPositon
                    dstrValue[3] = dstrTo;     //ToPosition
                }
                else if (darrstrValue[0].Contains("51") == false && darrstrValue[0].Contains("53") == false && darrstrValue[0].Contains("131") == false)
                {
                    dstrPosition = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);
                    dstrValue[0] = darrstrValue[0];
                    dstrValue[2] = dstrPosition;   //FromPositon
                    dstrValue[3] = dstrPosition;
                }
                subMCCLogData("", dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);
                dstrValue = null;
                darrstrValue = null;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //[2015/04/23] Error & Warning Log(Add by HS)
        private void subAlarmLogProcess(string strDataType,string strValue)
        {
            string dstrModuleID = "";
            string dstrGLSID = "";
            string dstrLOTID = "";
            string dstrPPID = "";
            string dstrStepID = "";
            string dstrMCCType = "";
            string[] darrstrValue = null;
            string[] dstrValue = new string[4];

            try
            {
                darrstrValue = strValue.Split(',');

                dstrModuleID = darrstrValue[0];
                
                dstrStepID = darrstrValue[1];
                dstrGLSID = darrstrValue[2];
                dstrLOTID = darrstrValue[3];
                dstrPPID = darrstrValue[4];

                if (strDataType.ToUpper() == "ERROR")
                {
                    dstrMCCType = "E";
                    dstrValue[0] = darrstrValue[5]; //Set or Reset
                    dstrValue[1] = darrstrValue[6]; //Alarm ID
                    dstrValue[2] = darrstrValue[7];   //Alarm Text

                    subMCCLogData("", dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);
                }
                else if (strDataType.ToUpper() == "WARNING")
                {
                    dstrMCCType = "W";
                    dstrValue[0] = darrstrValue[5]; //Set or Reset
                    dstrValue[1] = darrstrValue[6]; //Alarm ID
                    dstrValue[2] = darrstrValue[7];   //Alarm Text

                    subMCCLogData("", dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);
                }
                else
                {
                    return;
                }
                dstrValue = null;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strModuleID">ModuelID </param>
        /// <param name="strLogType">LogType</param>
        /// <param name="strStepID">StepID</param>
        /// <param name="strGLSID">GlassID</param>
        /// <param name="strLotID">LOTID</param>
        /// <param name="strPPID">HOSTPPID=EQPPPID</param>
        /// <param name="strDataValue">Value</param>
        private void subMCCLogData(string strTime, string strModuleID, string strLogType, string strStepID, string strGLSID, string strLotID, string strPPID,params string[] strDataValue)
        {
            string dstrMCCLogData = "";
            string dstrDataTime = "";
            string[] arrVaule;
            //[2015/04/01]Eqp ppid 저장용(Add by HS)this.PInfo.Unit(0).SubUnit(0).MCC(dintIndex)
            string strEQPPPID = "";
            try
            {
                if (string.IsNullOrEmpty(strTime) == false)
                {
                    dstrDataTime = strTime;
                }
                else
                {
                    dstrDataTime = DateTime.Now.ToString("MMdd_HHmm_ss.fff");
                }
                dstrMCCLogData = dstrDataTime + "," + strModuleID.Substring(strModuleID.Length -4,4) + "," + strLogType + "," + strStepID + "," + strGLSID + "," + strLotID + "," + strPPID;

                if (strDataValue != null)
                {
                    switch (strLogType)
                    {
                        case "A":       //Action Log
                            //Action=FromPosition=ToPosition=Start_End, ex) Componet_In=LD01=UP01=End
                            arrVaule = strDataValue[0].Split('=');
                            //dstrMCCLogData = dstrMCCLogData + "," + arrVaule[0] + "=" + arrVaule[1] + "=" + arrVaule[2] + "=" + arrVaule[3];
                            //dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0];
                            //[2015/04/01]MCC 형식 변경(Modify by HS)
                            if (strPPID != "")
                            {
                                strEQPPPID = PInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID).EQPPPID;
                            }
                            dstrMCCLogData = dstrMCCLogData + "=" + strEQPPPID + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2] + "=" + strDataValue[3];
 
                            break;

                        case "I":       //Information Log
                            //Information=Value, ex) VaccumPressure=360.4
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1];
                            break;

                        case "E":
                        case "W":
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2];
                            break;

                        case "S":
                            //[2015/04/01]MCC 형식 변경(Modify by HS)
                            if (strPPID != "")
                            {
                                strEQPPPID = PInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID).EQPPPID;
                            }
                            dstrMCCLogData = dstrMCCLogData + "=" + strEQPPPID + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2] + "=" + strDataValue[3];
                            break;

                        case "V"://Event Log
                            switch (strDataValue[0])
                            {
                                case "CEID_51":
                                    //EventID=OldState=NewState 
                                    dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2];
                                    break;
                                case "CEID_53":
                                    //EventID=OldState=NewState=AlarmID=AlarmText
                                    dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2] + "=" + strDataValue[3] + "=" + strDataValue[4];
                                    break;
                                case "CEID_131":
                                    //EventID=OldPPID=NewPPID
                                    dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2];
                                    break;
                                default :
                                    //EventID=In/Out=From=To 
                                    dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2] + "=" + strDataValue[3];
                                    break;
                            }
                            
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "LogType : " + strLogType + ", Step Definition or Information Data is Null !!");
                    return;
                }

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.MCCLog, dstrMCCLogData);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        # endregion


        //Test용 메소드
        private void subTest(string strValue)
        {
            try
            {
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region "CIM 통신 부분"
        private Thread pThreadCIMmsg;

        private void subStartCIMmsgThread()
        {
            try
            {
                this.pThreadCIMmsg = new Thread(new ThreadStart(subCIMmsgThread));
                this.pThreadCIMmsg.Name = "CIM_Msg_Recv";
                this.pThreadCIMmsg.IsBackground = true;
                this.pThreadCIMmsg.Start();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subStopCIMmsgThread()
        {
            try
            {
                if (this.pThreadCIMmsg != null) this.pThreadCIMmsg.Abort();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subCIMmsgThread()
        {
            do
            {
                try
                {
                    string dstrReadLine = Console.ReadLine();

                    if (!string.IsNullOrEmpty(dstrReadLine))
                    {
                        ReceiveMCCEventArg arg = new ReceiveMCCEventArg();

                        if (arg.SetMessage(dstrReadLine)) subReceiveCIMmessage(arg);
                    }
                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                }
            }
            while (true);
        }

        private class ReceiveMCCEventArg
        {
            private string mType;
            private string mMsg;


            public ReceiveMCCEventArg()
            {
            }

            public string MessageType
            {
                get
                {
                    return this.mType;
                }
            }
            public string Message
            {
                get
                {
                    return this.mMsg;
                }
            }

            public bool SetMessage(string dstrMsg)
            {
                bool result = false;

                try
                {
                    if (string.IsNullOrEmpty(dstrMsg)) return false;

                    string[] temp = dstrMsg.Split(';');

                    if (temp.Length < 1) return false;

                    if (temp.Length == 1 && temp[0] == "GET")
                    {
                        this.mType = "GET";
                        this.mMsg = string.Empty;
                        
                        result = true;
                    }
                    else if (temp.Length > 1 && temp[0] == "SET")
                    {
                        this.mType = "SET";
                        this.mMsg = temp[1];
                        result = true;
                    }
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }

        private void subParseSetMessage(string[] dstrParams)
        {
            try
            {
                bool report = false;

                foreach (string dstrTemp in dstrParams)
                {
                    string[] strValue = dstrTemp.Split('=');

                    if (strValue.Length == 2)
                    {

                        // MCC_BASIC_PATH=MCC/BASIC/PATH/TEST,MCC_HOST_IP=127.0.0.1,MCC_LOGIN_ID=CAMEBACK,MCC_LOGIN_PW=PASSWORD,MCC_SAMPLING_TIME=60
                        switch (strValue[0])
                        {
                            case "MCC_BASIC_PATH":
                                this.PInfo.All.MCCNetworkBasicPath = strValue[1];
                                FunINIMethod.subINIWriteValue("MCC", "NetworkBasicPath", this.PInfo.All.MCCNetworkBasicPath, this.PInfo.All.SystemINIFilePath);
                                report = true;
                                break;

                            case "MCC_SAMPLING_TIME":
                                this.PInfo.All.MCCFileUploadTime = Convert.ToInt32(strValue[1]);
                                FunINIMethod.subINIWriteValue("MCC", "MCCFileUploadTime", this.PInfo.All.MCCFileUploadTime.ToString(), this.PInfo.All.SystemINIFilePath);
                                report = true;
                                break;

                            case "MCC_HOST_IP":
                                this.PInfo.All.MCCNetworkPath = strValue[1];
                                FunINIMethod.subINIWriteValue("MCC", "NetworkPath", this.PInfo.All.MCCNetworkPath, this.PInfo.All.SystemINIFilePath);
                                report = true;
                                break;

                            case "MCC_LOGIN_ID":
                                this.PInfo.All.MCCNetworkUserID = strValue[1];
                                FunINIMethod.subINIWriteValue("MCC", "NetworkUserID", this.PInfo.All.MCCNetworkUserID, this.PInfo.All.SystemINIFilePath);
                                report = true;
                                break;

                            case "MCC_LOGIN_PW":
                                this.PInfo.All.MCCNetworkPassword = strValue[1];
                                FunINIMethod.subINIWriteValue("MCC", "NetworkPassword", this.PInfo.All.MCCNetworkPassword, this.PInfo.All.SystemINIFilePath);
                                report = true;
                                break;
                        }
                    }
                }

                if (report) subSendSetValue();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subReceiveCIMmessage(ReceiveMCCEventArg arg)
        {
            try
            {
                switch (arg.MessageType)
                {
                    case "SET":
                        if (string.IsNullOrEmpty(arg.Message)) break;

                        string[] dstrTemp = arg.Message.Split(',');

                        if (dstrTemp[0].StartsWith("UPLOAD="))
                        {
                            string[] dstrValue = dstrTemp[0].Split('=');

                            if(dstrValue.Length > 1 && (!string.IsNullOrEmpty(dstrValue[1])))
                            {
                                this.PInfo.All.MCCFileUploadUse = Convert.ToBoolean(dstrValue[1]);

                                FunINIMethod.subINIWriteValue("MCC", "MCCFileUploadUse", this.PInfo.All.MCCFileUploadUse.ToString(), this.PInfo.All.SystemINIFilePath);
                            }
                        }
                        else
                        {
                            subParseSetMessage(dstrTemp);
                        }
                        break;
                    case "GET":
                        subSendSetValue();
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSendSetValue()
        {
            try
            {
                // 1:MCC_BASIC_PATH 
                // 2:MCC_SAMPLING_TIME
                // 3:MCC_HOST_IP
                // 4:MCC_LOGIN_ID
                // 5:MCC_LOGIN_PW

                string dstrTemp = string.Format("SET;{0},{1},{2},{3},{4}",
                                
                                this.PInfo.All.MCCNetworkBasicPath,     // path
                                this.PInfo.All.MCCFileUploadTime,       // sampling time
                                this.PInfo.All.MCCNetworkPath,          // ip
                                this.PInfo.All.MCCNetworkUserID,        // loginID
                                this.PInfo.All.MCCNetworkPassword);     // password


                Console.WriteLine(dstrTemp);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion



        #region "PLC 통신"
        private void subPLCEvent()
        {
            string dstrDataPLC = "";
            string[] darrEvent;

            try
            {
                if (PMTPLC != null) dstrDataPLC = PMTPLC.funEventData();                    //PLC의 Event를 읽어온다.

                if (dstrDataPLC != "" && dstrDataPLC != null)
                {
                    darrEvent = dstrDataPLC.Split(new char[] { ';' });

                    switch (darrEvent[0])
                    {
                        case "ChangeBit":
                            subPLC_ChangeBit(darrEvent[1], darrEvent[2], darrEvent[3]);
                            break;
                        case "ChangeWord":
                            //subPLC_ChangeBit(darrEvent[1], darrEvent[2], darrEvent[3]);
                            break;
                        case "BitReadEvent":
                            subPLC_BitReadEvent(darrEvent[1], darrEvent[2], darrEvent[3], darrEvent[4]);
                            break;
                        case "BitWriteEvent":
                            subPLC_BitWriteEvent(darrEvent[1], darrEvent[2], darrEvent[3], darrEvent[4]);
                            break;
                        case "WordReadEvent":
                            subPLC_WordReadEvent(darrEvent[1], darrEvent[2], darrEvent[3], darrEvent[5]);
                            break;
                        case "WordWriteEvent":
                            subPLC_WordWriteEvent(darrEvent[1], darrEvent[2], darrEvent[3], darrEvent[5]);
                            break;
                        case "ErrorEvent":
                            subPLC_PLCError(darrEvent[1], darrEvent[2], darrEvent[3]);
                            break;
                        case "ConnectionEvent":
                            PMTPLC_PLCConnect(darrEvent[1]);
                            break;
                        case "DisConnectionEvent":
                            PMTPLC_PLCDisConnect(darrEvent[1]);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                Thread.Sleep(1);
            }
        }

        #region "PLC 이벤트 수신"
        /// <summary>
        /// 통신이 Connect가 되었을때 발생한다
        /// </summary>
        /// <param name="strDateTime"></param>
        private void PMTPLC_PLCConnect(string strDateTime)
        {
            try
            {
                this.PInfo.EQP("Main").PLCConnect = true;
                this.PInfo.EQP("Main").PLCStartConnect = true;           //처음 프로그램 기동시 PLC(Main장비)와 연결되었다는걸 기억

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLC, strDateTime, "PLC Connect-" + PMTPLC.proVersion);

                //subEQPPPIDRead();

                //PInfo.All.CurrentHOSTPPID = funWordRead("W2003", 10, EnuEQP.PLCRWType.ASCII_Data);
                //PInfo.All.CurrentEQPPPID = funWordRead("W200D", 1, EnuEQP.PLCRWType.Int_Data);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subEQPPPIDRead()
        {
            string strData = "";
            string dstrEQPPPID = "";
            try
            {
                PInfo.Unit(0).SubUnit(0).RemoveEQPPPID();

                string dstrWordAddress = "W2A00";
                string dstrTemp = funWordRead(dstrWordAddress, 22, EnuEQP.PLCRWType.Binary_Data);

                for (int dintLoop = 0; dintLoop < 22; dintLoop++)
                {
                    strData = dstrTemp.Substring(dintLoop * 16, 16);
                    for (int dintLoop2 = 0; dintLoop2 < 16; dintLoop2++)
                    {
                        if (strData[dintLoop2].ToString() == "1")
                        {
                            dstrEQPPPID = ((16 - dintLoop2) + (dintLoop * 16)).ToString();
                            PInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrEQPPPID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 통신이 DisConnect가 되었을때 발생한다
        /// </summary>
        /// <param name="strDateTime"></param>
        private void PMTPLC_PLCDisConnect(string strDateTime)
        {
            int dintAlarmID = 1000;
            int dintAlarmCode = 0;
            Boolean dbolAlarmReport = false;
            string dstrModuleID = "";
            string dstrAlarmType = "";
            string dstrAlarmDesc = "";
            string dstrAlarmTime = "";   //yyyy-MM-dd HH:mm:ss
            string dstrAlarmMsg = "";
            string strStatus = "Set";

            try
            {
                this.PInfo.EQP("Main").PLCConnect = false;

                //처음 프로그램 기동시 PLC(Main장비)와 Disconnect 되었을때
                if (PInfo.EQP("Main").PLCStartConnect == false && PInfo.All.ProgramEnd == false)
                {
                    MessageBox.Show("PLC Disconnect. Please verify Melsec network" + "\r\n" + "When OK Button is click, program is end", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLC, strDateTime, "PLC Disconnect. CIM END");

                    this.funClosePLC();
                    PInfo.All.ProgramEnd = true;            //Program을 종료한다.
                }
                else
                {
                    //PLC와 연결이 끊어지면 HOST로 알람보고를 한다.
                    if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID) == null)
                    {
                        this.PInfo.Unit(0).SubUnit(0).AddCurrAlarm(dintAlarmID);
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "S";  // Set
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;

                        //현재 발생 혹은 해제한 AlarmID를 가지고 기준정보에서 Alarm 정보를 가져온다.
                        dstrAlarmType = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                        dintAlarmCode = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode;
                        dstrAlarmDesc = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc;
                        dbolAlarmReport = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;
                        dstrAlarmTime = DateTime.Now.ToString("yyyyMMddHHmmss");    // Alarm 발생, 해제시간 설정
                        dstrModuleID = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).ModuleID;

                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = dstrAlarmType;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmCode = dintAlarmCode;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmDesc = dstrAlarmDesc;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmOCCTime = dstrAlarmTime;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).ModuleID = dstrModuleID;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = dbolAlarmReport;

                        //S5F1 Alarm Host 보고
                        if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport == true)
                        {
                            this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintAlarmID);
                        }

                        // Alarm 로그 Write
                        dstrAlarmMsg = dstrAlarmTime + "," + dstrModuleID + "," + strStatus + "," + dintAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + dstrAlarmDesc;
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);

                        // Alarm Message를 MainView ComboBox에 추가 (Message에 발생시간은 제외 Combo에 시간이 이중으로 Display 되어서)
                        dstrAlarmMsg = dstrModuleID + "," + strStatus + "," + dintAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + dstrAlarmDesc;
                        this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.AlarmMsg, dstrAlarmMsg);
                    }

                    this.PInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGNoBuzzer, 0, "", "PLC Disconnect 되었습니다." + "\r\n" + "PLC 연결여부 확인 후 CIM 프로그램을 재시작하시기 바랍니다.");
                }

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLC, strDateTime, "PLC DisConnect-" + PMTPLC.proVersion);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        //#if true    // 조영훈
        //        public event EventHandler bitChangedCIM = delegate(object sender, EventArgs e) { };
        //#endif
        /// <summary>
        /// Scan중 Bit영역이 변경 되었을 때 발생한다
        /// </summary>
        /// <param name="strDateTime">변경된 Address(B1010)</param>
        /// <param name="strAddress">변경된 값</param>
        /// <param name="strValue">Event/Status 여부(0->Event, 1->Status)</param>
        private void subPLC_ChangeBit(string strDateTime, string strAddress, string strValue)
        {
            try
            {
                if (PMTPLC.AddressMapBitHash(strAddress) == null) return;

                string dstrDesc = PMTPLC.AddressMapBitHash(strAddress).ACTDesc;                   //Description
                string dstrAddressStatus = PMTPLC.AddressMapBitHash(strAddress).AddressStatus;    //Adress의 Status 종류(Req, Comp)
                string dstrPLCCommender = PMTPLC.AddressMapBitHash(strAddress).Commender;         //Event비트의 Req 주체가 누구인가? P, C
                string dstrCompBit = PMTPLC.AddressMapBitHash(strAddress).CompBit;                //Comp Bit
                string dstrACTName = PMTPLC.AddressMapBitHash(strAddress).ACTName;                //ACT Name
                string dstrACTName2 = "";
                if (dstrCompBit != null)
                {
                    dstrACTName2 = PMTPLC.AddressMapBitHash(dstrCompBit).ACTName;
                }
                string dstrBitStatus = "";
                string dstrTemp = "";
                Boolean dbolEvent = false;

                //LOG WRITE ---------------------------
                if (dstrAddressStatus == "STATUS")
                {
                    dstrBitStatus = "STATUS";
                }
                else
                {
                    if (dstrAddressStatus == "REQ")
                    {
                        if (dstrPLCCommender == "P")
                        {
                            dstrTemp = "PLC";
                        }
                        else
                        {
                            dstrTemp = "CIM";
                        }
                        dstrBitStatus = dstrTemp + " " + dstrAddressStatus;
                    }
                    else
                    {
                        if (dstrPLCCommender == "P")
                        {
                            dstrTemp = "CIM";
                        }
                        else
                        {
                            dstrTemp = "PLC";
                        }
                        dstrBitStatus = dstrTemp + " " + dstrAddressStatus;
                    }
                }

                if (strAddress.Substring(1, 2) != "10" && dstrDesc != "Alive")
                {
                    dstrTemp = "BC ,Address:" + strAddress.PadRight(8) + ",Value:" + strValue.PadRight(10) + "," + dstrBitStatus.PadRight(30) + "," + dstrDesc;
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLC, strDateTime, dstrTemp);
                    dbolEvent = false;     //false:로그남김, true:로그안남김
                }

                //Act처리하여 필요한 정보를 얻고 처리후에 Comp를 준다
                if (dstrAddressStatus == "STATUS")
                {                                             //STATUS BIT Change
                    subPLCEvent_Seek(strAddress, Convert.ToInt32(strValue));
                }
                else     //Event BIT Change
                {
                    //#if true
                    //                    if (this.PInfo.EQP("Main").DummyPLC == true)    // 조영훈
                    //                    {
                    //                        if (dstrPLCCommender == "C" && dstrAddressStatus == "REQ" && strValue == "1")
                    //                        {
                    //                            bitChangedCIM(strAddress, new EventArgs());
                    //                        }
                    //                    }
                    //#endif
                    //PLC Request가 변하였는가?
                    if (dstrPLCCommender == "P" && dstrAddressStatus == "REQ")
                    {
                        if (strValue == "1")
                        {
                            subPLCEvent_Seek(strAddress, Convert.ToInt32(strValue));
                        }
                        else
                        {
                            ////this.pTimeOutCheck.Remove(dstrCompBit);   //Time을 해제(구조체에서 제거)

                            //PLC Req(Bit Off)에 대한 CIM의 Confirm(Bit Off)을 Off한다.
                            funBitWrite(dstrCompBit, strValue, dbolEvent);
                        }
                    }
                    else if ((dstrAddressStatus == "COMP" || dstrAddressStatus == "ABORT") && strValue == "1")   //CIM의 ON명령에 PLC가 Confirm비트를 on했는가?
                    {
                        if (PMTPLC.AddressMapBitHash(dstrCompBit).ACTName != "" && dstrBitStatus != "CIM COMP")
                        {
                            subPLCEvent_Seek(dstrCompBit, Convert.ToInt32(strValue));
                        }
                        if (dstrCompBit != "")
                        {
                            ////if (dstrPLCCommender == "C")
                            ////{
                            ////    this.pTimeOutCheck.Remove(dstrCompBit);   //Time을 해제(구조체에서 제거)
                            ////}
                            funBitWrite(dstrCompBit, "0");  //CIM의 ON명령에 PLC가 Comfirm비트를 ON하고 다시 CIM이 OFF할때
                        }
                    }
                    else if (dstrPLCCommender == "C" && (dstrAddressStatus == "COMP" || dstrAddressStatus == "ABORT") && strValue == "0")
                    {
                        //CIM의 Request(Bit Off)에 대한 PLC의 COMP(Bit Off)
                        subBitOffNextAction(dstrACTName2);
                    }
                    else if (dstrPLCCommender == "P" && dstrAddressStatus == "COMP" && strValue == "0" && dstrACTName2 == "actProcessDataCheck")
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strValue:" + strValue);
            }
        }

        private void subBitOffNextAction(string strACTName)
        {
            try
            {
                switch (strACTName)
                {
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strACTName:" + strACTName);
            }
        }

        /// <summary>
        /// Bit Read명령을 User가 명령을 내렸을 경우 EventAccept = False이면 발생한다
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <param name="strAddress">Read한 Address(B1010)</param>
        /// <param name="strLength">Read한 길이</param>
        /// <param name="strReadValue">읽은 결과</param>
        private void subPLC_BitReadEvent(string strDateTime, string strAddress, string strLength, string strReadValue)
        {
            try
            {
                string dstrDesc = PMTPLC.AddressMapBitHash(strAddress).ACTDesc;                   //Description

                dstrDesc = "BR ,Address:" + strAddress.PadRight(8) + ",Len  :" + strLength.PadRight(10) + ",ReadValue:" + strReadValue.PadRight(20) + "," + dstrDesc;   //KJH_LOG
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLC, strDateTime, dstrDesc);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strLength:" + strLength + ", strReadValue:" + strReadValue);
            }
        }

        /// <summary>
        /// Bit Write명령을 User가 명령을 내렸을 경우 EventAccept = False이면 발생한다
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <param name="strAddress">Write한 Address(B1610)</param>
        /// <param name="strValue">Write한 값</param>
        /// <param name="strResult">Write한 결과(True/False)</param>
        private void subPLC_BitWriteEvent(string strDateTime, string strAddress, string strValue, string strResult)
        {
            try
            {
                string dstrDesc = PMTPLC.AddressMapBitHash(strAddress).ACTDesc;                   //Description
                string dstrCompBit = PMTPLC.AddressMapBitHash(strAddress).CompBit;                         //Comp Bit
                string dstrBitStatus;

                if (dstrCompBit == null || dstrCompBit.Trim() == "")
                {
                    dstrBitStatus = "STATUS";
                }
                else
                {
                    string dstrAddressStatus = PMTPLC.AddressMapBitHash(strAddress).AddressStatus;             //Address의 Status 종류(Req, Comp)
                    string dstrPLCCommender = PMTPLC.AddressMapBitHash(strAddress).Commender;                  //Event비트의 Req 주체가 누구인가? P, C
                    if (dstrAddressStatus == "REQ")
                    {
                        dstrBitStatus = (dstrPLCCommender == "P") ? "PLC" : "CIM";
                        dstrBitStatus = dstrBitStatus + " " + dstrAddressStatus;
                    }
                    else
                    {
                        dstrBitStatus = (dstrPLCCommender == "P") ? "CIM" : "PLC";
                        dstrBitStatus = dstrBitStatus + " " + dstrAddressStatus;
                    }
                }
                if (strAddress.Substring(1, 2) != "18")
                {
                    dstrDesc = "BW ,Address:" + strAddress.PadRight(8) + ",Value:" + strValue.PadRight(10) + "," + dstrBitStatus.PadRight(30) + "," + dstrDesc;   //KJH_LOG
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLC, strDateTime, dstrDesc);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strValue:" + strValue + ", strResult:" + strResult);
            }
        }

        /// <summary>
        /// Word Read 명령을 User가 명령을 내렸을 경우 EventAccept = False이면 발생한다
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <param name="strAddress">Read한 Address(W1010)</param>
        /// <param name="strLength">Read한 길이</param>
        /// <param name="strReadValue">읽은 결과</param>
        private void subPLC_WordReadEvent(string strDateTime, string strAddress, string strLength, string strReadValue)
        {
            try
            {
                string dstrTemp = "WR ,Address:" + strAddress.PadRight(8) + ",Len  :" + strLength.PadRight(10) + ",ReadValue:" + strReadValue;   //KJH_LOG
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLC, strDateTime, dstrTemp);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strLength:" + strLength + ", strReadValue:" + strReadValue);
            }
        }

        /// <summary>
        /// Word Write 명령을 User가 명령을 내렸을 경우 EventAccept = False이면 발생한다
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <param name="strAddress">Write한 Address(W1010)</param>
        /// <param name="strValue">Write한 값</param>
        /// <param name="strResult">Write한 결과(True/False)</param>
        private void subPLC_WordWriteEvent(string strDateTime, string strAddress, string strValue, string strResult)
        {
            try
            {
                string dstrTemp = "WW ,Address:" + strAddress.PadRight(8) + ",Value:" + strValue.PadRight(10) + ",Result   :" + strResult;
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLC, strDateTime, dstrTemp);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strValue:" + strValue + ", strResult:" + strResult);
            }
        }

        /// <summary>
        /// 통신중 Error가 발생하였을때 발생
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <param name="strErrorIndex">Error ID</param>
        /// <param name="strErrDesc">Error 내용</param>
        private void subPLC_PLCError(string strDateTime, string strErrorIndex, string strErrDesc)
        {
            try
            {
                string dstrTemp = "Error_Index:" + strErrorIndex + ",Err_Descr:" + strErrDesc;
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLCError, dstrTemp);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strErrorIndex:" + strErrorIndex + ", strErrDesc:" + strErrDesc);
            }
        }

        #endregion

        #region"MTPLC 함수호출"
        /// <summary>
        /// Bit의 값을 읽고자 할때 쓴다
        /// </summary>
        /// <param name="strAddress">읽을 시작 Address</param>
        /// <param name="intLength">읽을 길이(Bit 갯수)</param>
        /// <param name="EventAccept">False이면 subPLC_BitReadEvent Event를 발생시킨다</param>
        /// <returns></returns>
        public string funBitRead(string strAddress, int intLength, params Boolean[] EventAccept)
        {
            string dstrReturn = "";

            try
            {
                if (EventAccept.Length == 0)
                {
                    dstrReturn = PMTPLC.funBitRead(strAddress, intLength.ToString(), false);
                }
                else
                {
                    dstrReturn = PMTPLC.funBitRead(strAddress, intLength.ToString(), EventAccept[0]);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", intLength:" + intLength);
            }

            return dstrReturn;
        }

        /// <summary>
        /// PLC로부터 Bit영역에 값을 Write한다
        /// </summary>
        /// <param name="strAddress">쓸 시작 Address</param>
        /// <param name="strValue">쓸려고 하는 값(HEX값이어야 한다)</param>
        /// <param name="EventAccept">False이면 subPLC_BitWriteEvent Event를 발생시킨다</param>
        /// <returns></returns>
        public Boolean funBitWrite(string strAddress, string strValue, params Boolean[] EventAccept)
        {
            Boolean dbolReturn = false;

            try
            {
                if (EventAccept.Length == 0)
                {
                    dbolReturn = PMTPLC.funBitWrite(strAddress, strValue, false);
                }
                else
                {
                    dbolReturn = PMTPLC.funBitWrite(strAddress, strValue, EventAccept[0]);
                }

                //////TimeOut 체크를 위해 현재 Write한 Bit를 저장
                ////if (PMTPLC.AddressMapBitHash(strAddress).CompBit != null && PMTPLC.AddressMapBitHash(strAddress).Commender == "C")
                ////{
                ////    for (int dintLoop = 1; dintLoop <= strValue.Length; dintLoop++)
                ////    {
                ////        if (strValue.Substring(dintLoop - 1, 1) == "1")
                ////        {
                ////            if (this.pTimeOutCheck.Contains(strAddress) == false)
                ////            {
                ////                InfoAct.clsTimeOut dclsTimeOut = new InfoAct.clsTimeOut(strAddress);
                ////                dclsTimeOut.Address = strAddress;
                ////                dclsTimeOut.Value = strValue;
                ////                dclsTimeOut.Tick = DateTime.Now.Ticks;
                ////                this.pTimeOutCheck.Add(strAddress, dclsTimeOut);
                ////                dclsTimeOut = null;
                ////            }
                ////        }
                ////    }
                ////}
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strValue:" + strValue);
            }

            return dbolReturn;
        }

        /// <summary>
        /// PLC의 Word영역의 값을 Read 할때 사용
        /// </summary>
        /// <param name="strAddress">읽고자 하는 시작 Address</param>
        /// <param name="intLength">읽고자 하는 길이</param>
        /// <param name="DataType">읽어 들이는 Type(DLL로부터는 HEX값으로 오므로 이를 User가 원하는 Type으로 변경한다)</param>
        /// <param name="EventAccept">Event를 받을 지의 여부(True이면 subPLC_WordReadEvent 가 발생하지 않는다)</param>
        /// <returns></returns>
        public string funWordRead(string strAddress, int intLength, EnuEQP.PLCRWType DataType, params Boolean[] EventAccept)
        {
            string dstrReturn = "";

            try
            {
                //Retry Cnt갯수만큼 Read를 한다.
                for (int dintCnt = 1; dintCnt <= PintWordRetry; dintCnt++)
                {
                    if (EventAccept.Length == 0)
                    {
                        dstrReturn = PMTPLC.funWordRead(strAddress, intLength.ToString(), ECCCommonAct.EnuCommunication.StringType.Hex, false);   //로그출력
                    }
                    else
                    {
                        //dstrReturn = PMTPLC.funWordRead(strAddress, intLength.ToString(), ECCCommonAct.EnuCommunication.StringType.Hex, EventAccept[0]);   //로그 출력 혹은 출력안함.

                        if (EventAccept[0] == false)
                        {
                            dstrReturn = PMTPLC.funWordRead(strAddress, intLength.ToString(), ECCCommonAct.EnuCommunication.StringType.Hex, true);       //로그를 출력하지 않음
                        }
                        else
                        {
                            dstrReturn = PMTPLC.funWordRead(strAddress, intLength.ToString(), ECCCommonAct.EnuCommunication.StringType.Hex, false);      //로그를 출력함
                        }
                    }

                    if (dstrReturn.Trim() != "")
                    {
                        break;
                    }
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLCError, "Retry Word Read Retry Cnt=" + dintCnt.ToString());
                }

                switch (DataType)
                {
                    case EnuEQP.PLCRWType.ASCII_Data:
                        dstrReturn = FunTypeConversion.funHexSwap(dstrReturn);
                        dstrReturn = FunTypeConversion.funHexConvert(dstrReturn, EnuEQP.StringType.ASCString);
                        break;

                    case EnuEQP.PLCRWType.Binary_Data:
                        dstrReturn = FunTypeConversion.funHexConvert(dstrReturn, EnuEQP.StringType.Binary);
                        break;

                    case EnuEQP.PLCRWType.Hex_Data:
                        break;

                    case EnuEQP.PLCRWType.Int_Data:
                        if (dstrReturn.Length > 4)
                        {
                            string a = dstrReturn.Substring(0, 4);
                            string b = dstrReturn.Substring(4, 4);
                            dstrReturn = b + a;
                        }
                        dstrReturn = FunTypeConversion.funHexConvert(dstrReturn, EnuEQP.StringType.Decimal);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", intLength:" + intLength + ", DataType:" + DataType);
            }

            return dstrReturn;
        }

        /// <summary>
        /// PLC의 Word영역에 값을  Write 한다
        /// </summary>
        /// <param name="strAddress">쓰고자 하는 시작 Address</param>
        /// <param name="strValue">쓸려고 하는 값</param>
        /// <param name="DataType">현재 쓸려고 하는 Value가 어떤 데이타 인지 규명한다(PLC에는 무조건 HEX데이타만을 쓰므로 변환시 사용된다)</param>
        /// <param name="EventAccept">Event를 받을 지의 여부(True이면 subPLC_WordWriteEvent 가 발생하지 않는다)</param>
        /// <returns>결과(True, False)</returns>
        public Boolean funWordWrite(string strAddress, string strValue, EnuEQP.PLCRWType DataType, params Boolean[] EventAccept)
        {
            string dstrData = "";
            Boolean dbolReturn = false;

            try
            {
                switch (DataType)
                {
                    case EnuEQP.PLCRWType.ASCII_Data:
                        if (strValue.Length % 2 != 0)
                        {
                            strValue = strValue + " ";                                        //2자리로 맞춘다.
                        }
                        dstrData = FunTypeConversion.funAscStringConvert(strValue, EnuEQP.StringType.Hex);
                        dstrData = FunTypeConversion.funHexSwap(dstrData);
                        break;

                    case EnuEQP.PLCRWType.Binary_Data:
                        dstrData = FunTypeConversion.funBinConvert(strValue, EnuEQP.StringType.Hex);
                        break;

                    case EnuEQP.PLCRWType.Hex_Data:
                        dstrData = strValue;
                        break;

                    case EnuEQP.PLCRWType.Int_Data:
                        dstrData = FunTypeConversion.funDecimalConvert(strValue, EnuEQP.StringType.Hex);
                        break;
                }
                if (EventAccept.Length == 0)
                {
                    dbolReturn = PMTPLC.funWordWrite(strAddress, dstrData, ECCCommonAct.EnuCommunication.StringType.Hex, false);
                }
                else
                {
                    dbolReturn = PMTPLC.funWordWrite(strAddress, dstrData, ECCCommonAct.EnuCommunication.StringType.Hex, EventAccept[0]);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strValue:" + strValue + ", DataType:" + DataType);
            }
            return dbolReturn;
        }


        public Boolean funDoubleWordWrite(string strAddress, string strValue, EnuEQP.PLCRWType DataType, params Boolean[] EventAccept)
        {
            string dstrData = "";
            Boolean dbolReturn = false;

            try
            {
                switch (DataType)
                {
                    case EnuEQP.PLCRWType.Int_Data:
                        if (strValue.Length < 8)
                        {
                            strValue = strValue.PadLeft(8, '0');
                        }

                        dstrData = FunTypeConversion.funDecimalConvert(strValue, EnuEQP.StringType.Hex);
                        break;
                }
                if (EventAccept.Length == 0)
                {
                    dbolReturn = PMTPLC.funWordWrite(strAddress, dstrData, ECCCommonAct.EnuCommunication.StringType.Hex, false);
                }
                else
                {
                    dbolReturn = PMTPLC.funWordWrite(strAddress, dstrData, ECCCommonAct.EnuCommunication.StringType.Hex, EventAccept[0]);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strValue:" + strValue + ", DataType:" + DataType);
            }
            return dbolReturn;
        }

        /// <summary>
        /// PLC의 Word영역에 쓰려는 값을 리턴한다.
        /// </summary>
        /// <param name="intLength">쓰고자 하는 Word 길이</param>
        /// <param name="strValue">쓸려고 하는 값</param>
        /// <param name="DataType">현재 쓸려고 하는 Value가 어떤 데이타 인지 규명한다(PLC에는 무조건 HEX데이타만을 쓰므로 변환시 사용된다)</param>
        /// <param name="EventAccept">Event를 받을 지의 여부(True이면 subPLC_WordWriteEvent 가 발생하지 않는다)</param>
        /// <returns>PLC Word영역에 실제로 써질 값</returns>
        public string funWordWriteString(int intLength, string strValue, EnuEQP.PLCRWType DataType, params Boolean[] EventAccept)
        {
            string dstrData = "";

            try
            {
                switch (DataType)
                {
                    case EnuEQP.PLCRWType.ASCII_Data:
                        if (strValue.Length % 2 != 0)
                        {
                            strValue = strValue + " ";                                        //2자리로 맞춘다.
                        }
                        dstrData = FunTypeConversion.funAscStringConvert(strValue, EnuEQP.StringType.Hex);
                        dstrData = FunTypeConversion.funHexSwap(dstrData);
                        break;

                    case EnuEQP.PLCRWType.Binary_Data:
                        dstrData = FunTypeConversion.funBinConvert(strValue, EnuEQP.StringType.Hex);
                        break;

                    case EnuEQP.PLCRWType.Hex_Data:
                        dstrData = strValue;
                        break;

                    case EnuEQP.PLCRWType.Int_Data:
                        if (intLength == 2)
                        {
                            strValue = strValue.PadLeft(8, '0');
                        }

                        dstrData = FunTypeConversion.funDecimalConvert(strValue, EnuEQP.StringType.Hex);
                        break;
                }

                //dstrData = dstrData.PadLeft(intLength * 4, '0');
                dstrData = dstrData.PadRight(intLength * 4, '0');       //오른쪽에 0을 채워야 함.

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intLength:" + intLength + ", strValue:" + strValue + ", DataType:" + DataType);
            }

            return dstrData;
        }

        /// <summary>
        /// PLC의 Word영역에서 Mapping정보를 읽어와 앞쪽부터 Slot1이 되는 Binary값을 리턴한다
        /// (1100 0000 0000 0000 1000 0000 0000 0000 => 1,2,17번 GLS 있음)
        /// </summary>
        /// <param name="strAddress">읽고자 하는 시작 Address(W1000)</param>
        /// <param name="intLength">읽고자 하는 길이(2)</param>
        /// <param name="EventAccept">Event를 받을 지의 여부(True이면 subPLC_WordReadEvent 가 발생하지 않는다)</param>
        /// <returns>PLC로 부터 읽어들인 Mapping정보(앞쪽부터 Slot 1)</returns>
        public string funMappingRead(string strAddress, int intLength, params Boolean[] EventAccept)
        {
            string dstrReturn = "";
            string dstrTemp = "";

            try
            {
                //Retry Cnt갯수만큼 Read를 한다.
                for (int dintCnt = 1; dintCnt <= PintWordRetry; dintCnt++)
                {

                    if (EventAccept.Length == 0)
                    {
                        dstrReturn = PMTPLC.funWordRead(strAddress, intLength.ToString(), ECCCommonAct.EnuCommunication.StringType.Hex, false);
                    }
                    else
                    {
                        dstrReturn = PMTPLC.funWordRead(strAddress, intLength.ToString(), ECCCommonAct.EnuCommunication.StringType.Hex, EventAccept[0]);
                    }

                    if (dstrReturn.Trim() != "")
                    {
                        break;
                    }
                    dstrTemp = "Retry Word Read Retry Cnt=" + dintCnt.ToString();
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.PLCError, dstrTemp);
                }

                dstrReturn = FunTypeConversion.funHexConvert(dstrReturn, EnuEQP.StringType.Binary);

                dstrTemp = "";
                for (int dintCnt = 0; dintCnt <= intLength - 1; dintCnt++)
                {
                    for (int dintBack = 15; dintBack >= 0; dintBack--)
                    {
                        dstrTemp += dstrReturn.Substring(dintBack + (dintCnt * 16), 1);
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", intLength:" + intLength);
            }

            return dstrTemp;
        }

        /// <summary>
        /// PLC의 Word영역에 값을  Write 한다
        /// </summary>
        /// <param name="strAddress">쓰고자 하는 시작 Address</param>
        /// <param name="strValue">쓸려고 하는 값</param>
        /// <param name="EventAccept">Event를 받을 지의 여부(True이면 subPLC_WordWriteEvent 가 발생하지 않는다)</param>
        /// <returns>결과(True, False)</returns>
        public string funMappingWrite(string strAddress, string strValue, params Boolean[] EventAccept)
        {
            string dstrReturn = "";
            string dstrTemp = "";
            int dintTemp = 0;
            int dint16BitLength = 0;

            try
            {
                dintTemp = strValue.Length % 16;
                if (dintTemp != 0)
                {
                    strValue += dstrTemp.PadRight(16 - dintTemp).Replace(" ", "0");
                }

                dint16BitLength = strValue.Length / 16;
                for (int dintCnt = 0; dintCnt <= dint16BitLength - 1; dintCnt++)
                {
                    for (int dintBack = 15; dintBack >= 0; dintBack--)
                    {
                        dstrReturn += strValue.Substring(dintBack + (dintCnt * 16), 1);
                    }
                }

                //Mapping정보에서 앞에 1~16번 Slot모두 0이면 2진수로 변환하면 이상한 값이 나오므로 앞에 0000을 붙인다. 
                if (dstrReturn.StartsWith(FunStringH.funMakeLengthStringFirst("0", 16)) == true)
                {
                    dstrTemp = FunStringH.funMakeLengthStringFirst("0", 4);
                }
                dstrReturn = dstrTemp + FunTypeConversion.funBinConvert(dstrReturn, EnuEQP.StringType.Hex);

                if (EventAccept.Length == 0)
                {
                    PMTPLC.funWordWrite(strAddress, dstrReturn, ECCCommonAct.EnuCommunication.StringType.Hex, false);
                }
                else
                {
                    PMTPLC.funWordWrite(strAddress, dstrReturn, ECCCommonAct.EnuCommunication.StringType.Hex, EventAccept[0]);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", strValue:" + strValue);
            }

            return dstrReturn;
        }

        #endregion


        //Data를 User가 원하는 Type으로 변경한다
        #region"Data Type Change Function"

        //*******************************************************************************
        //  Function Name : funAscStringConvert()
        //  Description   : Asc String Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strData         => ASCII String Data(AB)
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/07          어 경태         [L 00] 
        //*******************************************************************************
        public string funAscStringConvert(string strData, EnuEQP.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //ASCII Data(AB) => Binary Data(0100 0001 0100 0010)
                    case EnuEQP.StringType.Binary:
                        dstrReturn = funAscStringConvert(strData, EnuEQP.StringType.Hex);           //ASC를 HEX로 바꾼다.
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.Binary);            //HEX를 Bynary로 바꾼다.
                        break;

                    //ASCII Data(AB) => Decimal Data(16706)
                    case EnuEQP.StringType.Decimal:
                        dstrReturn = funAscStringConvert(strData, EnuEQP.StringType.Hex);           //ASC를 HEX로 바꾼다.
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.Decimal);           //HEX를 10진수로 바꾼다.
                        break;

                    //ASCII Data(AB) => Hex Data(4142)
                    case EnuEQP.StringType.Hex:
                        for (int i = 0; i < strData.Length; i++)
                        {
                            char c = Convert.ToChar(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c);                                   //아스키한문자를 10진수 ASC코드(65)로 변환한다.
                            dstrTemp = string.Format("{0:X2}", dintTemp);                     //10진수를 2자리를 맞춘 16진수로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        break;

                    //ASCII Data(AB) => ASC Code(6566)
                    case EnuEQP.StringType.ASCCode:
                        for (int i = 0; i < strData.Length; i++)
                        {
                            char c = Convert.ToChar(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c);                                   //아스키한문자를 10진수 ASC코드로 변환한다.
                            dstrTemp = string.Format("{0:D2}", dintTemp);                     //10진수를 10진수 2자리 문자로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        dstrReturn = strData;
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funHexConvert()
        //  Description   : HEX Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strData         => HexData = Hex Data(4142)
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funHexConvert(string strData, EnuEQP.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //Hex Data(4142) => Binary Data(0100 0010 0100 0001)
                    case EnuEQP.StringType.Binary:
                        for (int i = 0; i < strData.Length; i++)
                        {
                            string c = Convert.ToString(strData.Substring(i, 1));
                            dintTemp = Convert.ToInt32(c, 16);                               //16진수를 10진수로 변환
                            dstrTemp = Convert.ToString(dintTemp, 2);                         //10진수를 2진수로 바꾼다.
                            dstrTemp = string.Format("{0:0000}", Convert.ToInt32(dstrTemp));  //앞에 0이 붙는 4자리의 2진수로 바꾼다.

                            dstrReturn += dstrTemp;
                        }
                        break;

                    //Hex Data(4142) => Decimal Data(16706)
                    case EnuEQP.StringType.Decimal:
                        dstrTemp = Convert.ToInt32(strData, 16).ToString();
                        dstrReturn = dstrTemp;
                        break;

                    //Hex Data(4142) => ASC String(AB)
                    case EnuEQP.StringType.ASCString:
                        for (int i = 0; i < strData.Length; i = i + 2)
                        {
                            string c = Convert.ToString(strData.Substring(i, 2));           //2개씩 끊어 읽어온다.
                            dintTemp = Convert.ToInt32(c, 16);                               //16진수를 10진수로 변환한다.
                            byte d = Convert.ToByte(dintTemp);                               //10진수를 Byte형태로 전환
                            dstrTemp = Convert.ToString(Convert.ToChar(d));                  //Byte형태의 10진수를 ASC 문자로 변환한다.
                            if (dstrTemp == "\0") { dstrTemp = " "; }
                            dstrReturn += dstrTemp;
                        }
                        break;

                    //Hex Data(4142) => ASC Code(6566)
                    case EnuEQP.StringType.ASCCode:
                        for (int i = 0; i < strData.Length; i = i + 2)
                        {
                            string c = Convert.ToString(strData.Substring(i, 2));           //2개씩 끊어 읽어온다.
                            dintTemp = Convert.ToInt32(c, 16);                               //16진수를 10진수로 변환한다.
                            dstrTemp = string.Format("{0:D2}", dintTemp);                     //10진수를 10진수 2자리 문자로 바꾼다.
                            dstrReturn += dstrTemp;
                        }
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funHexSwap()
        //  Description   : Hex Data를 Swap한다(4자리로 안끊어지면 뒤에 0을 두어  Swap한다)
        //  Parameters    : HexData => Hex Data(4142 4344 4546 47)
        //  Return Value  : Swap Data(4241 4443 4645 4700)
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funHexSwap(string strHexData)
        {
            string dstrReturn = "";
            string dstrTemp;

            try
            {
                if (strHexData.Length % 2 != 0)
                {
                    strHexData = "0" + strHexData;                                        //2자리로 맞춘다.
                }

                for (int i = 0; i < strHexData.Length; i = i + 4)
                {
                    dstrTemp = Convert.ToString(strHexData.Substring(i, 2));           //2개씩 끊어 읽어온다.
                    dstrTemp = dstrTemp.PadLeft(4, '0');

                    dstrReturn = dstrReturn + strHexData.Substring(i + 2, 2) + strHexData.Substring(i, 2);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strHexData:" + strHexData);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funDecimalConvert()
        //  Description   : 10진수 Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strData => Decimal Data(16706)
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funDecimalConvert(string strData, EnuEQP.StringType StringType)
        {
            string dstrReturn = "";
            string dstrTemp = "";

            try
            {
                switch (StringType)
                {
                    //Decimal Data(16706) => Binary Data(0100 0001 0100 0010)
                    case EnuEQP.StringType.Binary:
                        dstrReturn = funDecimalConvert(strData, EnuEQP.StringType.Hex);             //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.Binary);           //HEX를 Bynary로 바꾼다.
                        break;

                    //Decimal Data(1089) => Hex Data(0441)
                    case EnuEQP.StringType.Hex:
                        dstrTemp = string.Format("{0:X}", Convert.ToInt32(strData));         //10진수를 16진수로 바꾼다.
                        if (dstrTemp.Length % 4 != 0)
                        {
                            int dintTemp = dstrTemp.Length / 4;
                            dintTemp = dintTemp + 1;

                            dstrTemp = dstrTemp.PadLeft(dintTemp * 4, '0');
                        }
                        dstrReturn = dstrTemp;
                        break;

                    //Decimal Data(16706) => ASC String(AB)
                    case EnuEQP.StringType.ASCString:
                        dstrReturn = funDecimalConvert(strData, EnuEQP.StringType.Hex);             //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.ASCString);        //HEX를 ASC String으로 바꾼다.
                        break;

                    //Decimal Data(16706) => ASC Code(6566)
                    case EnuEQP.StringType.ASCCode:
                        dstrReturn = funDecimalConvert(strData, EnuEQP.StringType.Hex);             //10진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.ASCCode);           //HEX를 ASC Code로 바꾼다.
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funBinConvert()
        //  Description   : Binary Data 를 원하는 Data 로 바꾸어 준다
        //  Parameters    : strHEXData => Decimal Data(16706)
        //                  dStringType     => 바꾸고자 하는 Type
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funBinConvert(string strData, EnuEQP.StringType StringType)
        {
            string dstrReturn = "";
            int dintTemp = 0;

            try
            {
                switch (StringType)
                {
                    //Binary Data(0100 0001 0100 0010) => Decimal Data(1089)
                    case EnuEQP.StringType.Decimal:
                        dstrReturn = funBinConvert(strData, EnuEQP.StringType.Hex);                 //2진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.Decimal);           //HEX를 Decimal로 바꾼다.
                        break;

                    //Binary Data(0100 0001 0100 0010) => Hex Data(0441)
                    case EnuEQP.StringType.Hex:
                        if (strData.Trim() == "") strData = "0";
                        dintTemp = Convert.ToInt32(strData, 2);                            //2진수를 10진수로 변환
                        dstrReturn = funDecimalConvert(dintTemp.ToString(), EnuEQP.StringType.Hex);               //10진수를 16진수로 변환
                        break;

                    //Binary Data(0100 0001 0100 0010) => ASC String(AB)
                    case EnuEQP.StringType.ASCString:
                        dstrReturn = funBinConvert(strData, EnuEQP.StringType.Hex);                 //2진수를 16진수¡¡¡ 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.ASCString);         //HEX를 ASC String으로 바꾼다.
                        break;

                    //Binary Data(0100 0001 0100 0010) => ASC Code(6566)
                    case EnuEQP.StringType.ASCCode:
                        dstrReturn = funBinConvert(strData, EnuEQP.StringType.Hex);                 //2진수를 16진수로 변환
                        dstrReturn = funHexConvert(dstrReturn, EnuEQP.StringType.ASCCode);           //HEX를 ASC Code¡ 바꾼다.
                        break;

                    default:
                        dstrReturn = strData;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strData:" + strData + ", StringType:" + StringType);
            }

            return dstrReturn;
        }

        //*******************************************************************************
        //  Function Name : funAddressAdd()
        //  Description   : 어드레스를 원하는 만큼 증가시킨다.
        //  Parameters    : strBaseAddress => 16진수 어드레스(W1009)
        //                  intStep => 증가시킬 양(2)
        //  Return Value  : 증가시킨 16진수 어드레스(W100B)
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/08          어 경태         [L 00] 
        //*******************************************************************************
        public string funAddressAdd(string strBaseAddress, int intStep)
        {
            string dstrAddress = "";
            int dintAddress = 0;
            string dstrArea = "";


            try
            {
                dstrArea = strBaseAddress.Substring(0, 1);
                if (dstrArea == "M" || dstrArea == "D")    //M, D는 주소가 10진수임
                {
                    dstrAddress = strBaseAddress.Substring(1, strBaseAddress.Length - 1);     //어드레스만 뽑아낸다.
                    dstrAddress = Convert.ToString(Convert.ToInt32(dstrAddress) + intStep);

                    dstrAddress = strBaseAddress.Substring(0, 1) + dstrAddress;
                }
                else       //B, W는 주소가 16진수임
                {
                    dstrAddress = strBaseAddress.Substring(1, strBaseAddress.Length - 1);     //어드레스만 뽑아낸다.
                    dintAddress = Convert.ToInt32(this.funHexConvert(dstrAddress, EnuEQP.StringType.Decimal)) + intStep;     //10진수로 바꾸고 어드레스를 증가한다.
                    dstrAddress = string.Format("{0:X4}", dintAddress);                               //10진수를 16진수 4자리로 바꾼다.
                    dstrAddress = strBaseAddress.Substring(0, 1) + dstrAddress;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strBaseAddress:" + strBaseAddress + ", intStep:" + intStep);
            }

            return dstrAddress;
        }

        //*******************************************************************************
        //  Function Name : subWordReadSave()
        //  Description   : Word를 Block으로 읽기 위해 Queue에 저장한다.
        //  Parameters    : strAddress  => 해당 Address
        //                  intLength   => 길이
        //                  DataType    => DataType
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2007/01/09          김 효주         [L 00] 
        //*******************************************************************************
        private void subWordReadSave(string strAddress, int intLength, EnuEQP.PLCRWType DataType)
        {
            try
            {
                if (pintTotalReadLength == 0) this.pReadDataQueue.Clear();  //Queue 초기화
                pintTotalReadLength = pintTotalReadLength + intLength;      //Word Read Total Length를 저장

                this.pReadDataQueue.Enqueue(strAddress + "," + intLength + "," + Convert.ToInt32(DataType));
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strAddress:" + strAddress + ", intLength:" + intLength + ", DataType:" + DataType.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : funWordReadAction()
        //  Description   : Word를 Block으로 읽어와서 각 DataType에 맞게 배열로 리턴한다.
        //  Parameters    : string[]    => 각 Datatype에 맞게 저장된 문자열 배열
        //  Return Value  : None
        //  Special Notes : 연속된 영역만 읽을 수 있고
        //                  분리된 영역은 기존의 방법(funWordRead)으로 읽는다.
        //*******************************************************************************
        //  2007/01/09          김 효주         [L 00] 
        //*******************************************************************************
        private string[] funWordReadAction(Boolean bolLog)
        {
            string[] dstrReturn = new string[this.pReadDataQueue.Count];
            EnuEQP.PLCRWType dintPLCRWType;
            string dstrReadData = "";
            string[] dstrData;
            string dstrTemp = "";
            string dstrStartAddress = "";
            int dintStartIndex = 0;
            int dintLength = 0;
            int dintIndex = 0;

            try
            {
                //Queue에 읽을 내용이 없으면 그냥 빠져나간다.
                if (this.pReadDataQueue.Count == 0) return null;

                dstrTemp = this.pReadDataQueue.Peek().ToString();   //Queue에서 시작 Address를 읽는다.(Peek는 Queue에서 제거하지 않고 꺼낸다.)
                dstrData = dstrTemp.Split(new char[] { ',' });      //인자들을 분리한다.
                dstrStartAddress = dstrData[0];

                dstrReadData = funWordRead(dstrStartAddress, pintTotalReadLength, EnuEQP.PLCRWType.Hex_Data, bolLog);  //Word를 Block으로 한번에 읽는다.

                while (this.pReadDataQueue.Count > 0)
                {
                    dstrTemp = this.pReadDataQueue.Dequeue().ToString();
                    dstrData = dstrTemp.Split(new char[] { ',' });      //인자들을 분리한다.
                    dintLength = Convert.ToInt32(dstrData[1]) * 4;
                    dintPLCRWType = (EnuEQP.PLCRWType)(Convert.ToInt32(dstrData[2]));
                    dstrTemp = dstrReadData.Substring(dintStartIndex, dintLength);

                    switch (dintPLCRWType)
                    {
                        case EnuEQP.PLCRWType.ASCII_Data:
                            dstrTemp = funHexSwap(dstrTemp);
                            dstrTemp = funHexConvert(dstrTemp, EnuEQP.StringType.ASCString);
                            dstrReturn[dintIndex] = dstrTemp.Trim();
                            break;

                        case EnuEQP.PLCRWType.Binary_Data:
                            dstrTemp = funHexConvert(dstrTemp, EnuEQP.StringType.Binary);
                            dstrReturn[dintIndex] = dstrTemp.Trim();
                            break;

                        case EnuEQP.PLCRWType.Hex_Data:
                            dstrReturn[dintIndex] = dstrTemp.Trim();
                            break;

                        case EnuEQP.PLCRWType.Int_Data:
                            if (Convert.ToInt32(dstrData[1]) == 2)  //2Word
                            {
                                //상위가 0이면 65565이하 값
                                if (Convert.ToInt32(dstrTemp.Substring(4, 4)) == 0)
                                {
                                    dstrTemp = dstrTemp.Substring(0, 4);
                                    dstrTemp = funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
                                }
                                else
                                {
                                    dstrTemp = dstrTemp.Substring(0, 4);
                                    dstrTemp = funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
                                    dstrTemp = Convert.ToString(65536 + Convert.ToInt32(dstrTemp));
                                }
                            }
                            else
                            {
                                dstrTemp = funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
                            }
                            dstrReturn[dintIndex] = dstrTemp.Trim();
                            break;
                    }

                    dintStartIndex = dintStartIndex + dintLength;
                    dintIndex = dintIndex + 1;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally     //초기화
            {
                this.pReadDataQueue.Clear();
                pintTotalReadLength = 0;
            }

            return dstrReturn;
        }

        #endregion

        #endregion


    }
}
