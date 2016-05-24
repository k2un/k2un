using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using CommonAct;
using System.Diagnostics;

namespace InfoAct
{
    public class clsInfo
    {
        //선언
        #region "선언"

        public string Version
        {
            get { return "Samsung SMD HCLN V1.0"; }
        }

        private Hashtable pEQPHashtable = new Hashtable();
        private Hashtable pAllHashtable = new Hashtable();
        private Hashtable pUnitHash = new Hashtable();

        private Hashtable pLOTHash = new Hashtable();               //LOTID가 Key, LOTIndex가 Item값
        private Hashtable pLOTIndexHash = new Hashtable();          //LOTIndex가 Key, LOTID가 Item값

        private Hashtable pDataTableHash = new Hashtable();

        private Hashtable pLogQueueHash = new Hashtable();
        private Hashtable pMCCLogQueueHash = new Hashtable();
        private Hashtable pModuleIDHash = new Hashtable();            //

        private Hashtable pPortHash = new Hashtable();


        private Queue pSendQueue = new Queue();                     //HOST로 전송할 S/F을 저장하는 송신 Queue
        private Queue pReceiveHostQueue = new Queue();              //HOST로부터 수신된 S/F을 저장하는 수신 Queue       20071211 어경태

        private Queue pPLCWriteMethodQueue = new Queue();           //HOSTAct에서 수신한 내용에 대해 EQPAct에 작업할 수신 Queue
        private Queue pOPCallQueue = new Queue();                   //Operator Call을 할 Queue (중요한 OPCall)
        private Queue pOPCallOverWriteQueue = new Queue();          //Operator Call을 할 Queue (OverWrite가 가능한 중요하지않은 OPCall)
        private Queue pMessageQueue = new Queue();                  //Main View의 ComboBox에 Alarm, HOST Message 출력

        private Queue pMSGQueue = new Queue();			// 20120320 이상창			//OPCallMSG Log 출력 할 Queue
        private Queue pXPCDelayQueue = new Queue(); // 20120927 이상창

        private Queue pViewEventQueue = new Queue();    //20121120 김영식

        private Queue pPPIDCommandQueue = new Queue();

        private LogActFormShowType pLogActFormShowIndex = 0;

        public delegate void CreateEvent(string dstrParameter);     //Host로 전송할 메세지를 생성하는 SF 델리게이트 선언
        public event CreateEvent CreateSFEvent;                     //Host로 전송할 메세지를 생성하는 SF 이벤트 선언

        private Hashtable pAPCHashtable = new Hashtable();
        private Hashtable pPPCHashtable = new Hashtable();
        private Hashtable pRPCHashtable = new Hashtable();
        private Hashtable pGLSHashTable = new Hashtable();

        private object pobjLockEnqueueLog = new object();
        private object pobjLockDequeueLog = new object();
        private object pobjLockLogQueueCount = new object();

        private object pobjLockMCCEnqueueLog = new object();
        private object pobjLockMCCDequeueLog = new object();
        private object pobjLockMCCLogQueueCount = new object();

        public clsPLCAddress pPLCAddressInfo = clsPLCAddress.Instance;

        private Hashtable pHashModuleIDToUnitID = new Hashtable();
                  

        #region "MCC process"

        public void funMCCprocessStart()
        {
            try
            {
                if (All.MCC_RunFlag)
                {
                    if (!System.IO.File.Exists(All.MCCprocPath)) return;

                    foreach (Process process in Process.GetProcessesByName("MCC")) process.Kill();

                    ProcessStartInfo psi = new ProcessStartInfo();
                    psi.FileName = All.MCCprocPath;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardInput = true;
                    psi.UseShellExecute = false;

                    All.MCCprocess = new Process();
                    All.MCCprocess.StartInfo = psi;

                    All.MCCprocess.Start();
                }
            }
            catch(Exception ex)
            {
                subLog_Set(LogType.CIM, ex.ToString());
            }
        }
        
        #endregion

        #region Singleton
        public static readonly clsInfo Instance = new clsInfo();
        #endregion

        #region Constructor
        public clsInfo()
        {

        }
        #endregion

        public enum SFName
        {
            S1F1AreYouThereRequest = 0,

            S5F1AlarmReportsend = 1,

            S6F1TraceDataSend = 2,
            S6F11RelatedPanelProcessEvent = 3,
            S6F11RelatedEquipmentEvent = 4,
            S6F11RelatedEquipmentParameterEvent = 5,
            S6F11EquipmentSpecifiedControlEvent = 6,
            S6F11RelatedMaterialEvent = 7,
            S6F13GLSAPD = 8,
            S6F13LOTAPD = 9,

            S7F107PPIDCreatDeleteAndPPBodyChangedReport = 10,
            S6F11RelatedPanelIDValidationEvent = 11,
            S6F11RelatedJobProcess = 12,
            S6F11EquipmentSpecifiedNetworkEvent = 13,
            S16F11PPCRunning = 14,
            S16F15PPCDataCMD = 15,
            S16F111APCStart = 16,
            S16F113APCEnd = 17,
            S16F115APCDataCMD = 18,
            S16F131RPCStart = 19,
            S16F133RPCEnd = 20,
            S16F135RPCDataCMD = 21,
            S6F13GLSAPDMea = 22,
            S6F11RelatedProcessStatusEvent = 23,
            S6F11MaterialLoadEvent = 24,
            S6F11MaterialProcEvent = 25,

            S6F11MaterialStockEvent = 26,
            S6F11RelatedPortEvent = 27,
            S6F11CSTMovingEvent = 28,
            S6F11MaterialProcessStatusEvent = 29, //150122 고석현
        }

        public enum EOID               //EOID 항목
        {
            VCRReadingMode = 1,
            APCMode = 2,
            MCCReportingMode = 3,

            //ComponentTrace = 1,
            //EquipmentStateTrace = 2,
            //EquipmentProcessStateTrace = 3,
            //LapseTimeINIT = 4,
            //LapseTimeIDLE = 5,
            //LapseTimeSETUP = 6,
            //LapseTimeREADY = 7,
            //LapseTimeEXECUTING = 8,
            //LapseTimePAUSE = 9,
            //LapseTimeSTOP = 10,
            //Waittime = 12,
        }

        //public enum ControlStates               //Equipment Control States
        //{
        //    NONE = 0,
        //    OFFLINE = 1,
        //    ONLINELOCAL = 2,
        //    ONLINEREMOTE = 3,
        //}

        public enum EquipmentState              //Equipment(Module) State
        {
            NONE = 0,
            NORMAL = 1,
            FAULT = 2,
            PM = 3,
        }

        public enum EquipmentProcessState       //Equipment(Module) Process State
        {
            NONE = 0,
            INIT = 1,
            IDLE = 2,
            SETUP = 3,
            READY = 4,
            EXECUTE = 5,
            PAUSE = 6,
        }

        //public enum GlassState                  //Glass State(Processing, Aborting만 사용)
        //{
        //    NONE = 0,
        //    Processing = 3,
        //    Aborting = 5,
        //}

        //public enum BYWHO                       //Who Initiated the event
        //{
        //    NONE = 0,
        //    ByHost = 1,
        //    ByOperator = 2,
        //    ByEquipment = 3,
        //}

        public enum PLCCommand
        {
            BuzzerOff = 0,
            BuzzerOn = 1,
            DateandTimeSetting = 2,
            MessageSend = 3,
            MemoryView = 4,
            MapInitial = 5,
            EventBitInitialCmd = 6,
            ControlState = 7,
            EQPState = 8,
            EQPProcessState = 9,
            Scrap = 10,
            Unscrap = 11,
            ECIDChange = 12,
            PPIDCreate = 13,
            PPIDDelete = 14,
            PPIDBodyModify = 15,            //EQP PPID Body Modify임.
            HOSTPPIDMappingChange = 16,
            CurrentPPIDChange = 17,
            ECIDRead = 18,
            CurrentRunningPPPIDReadFromHOST = 19,
            OnePPIDRead = 20,
            SetUpPPID = 21,                 //EQP, HOST PPID를 PLC로 부터 읽어 구조체에 저장
            SupervisorLoginOut = 22,
            VCRReadingMode = 23,
            APCMode = 24,
            HOSTDisconnect = 25,
            ECIDAllRequest = 26,            //장비 전체 ECID 조회
            SEMControllerStart = 27,
            SEMControllerEnd = 28,
            SEMAlarm = 29,

            FDCDataAlramReset = 31,
            MessageClear = 32,              //2012.09.26 김영식... MessageClear Event Send
            SerialPortOpen = 33,
            SerialPortClose = 34,

            ProcessDataLog = 39,
            ProcessDataSet = 40,
            ProcessDataDel = 41,

            APCStart = 42,
            RPCStart = 43,
            PPCStart = 44,
            NormalStart = 45,

            MultiUseDataSet = 50,
            GetMultiUseData = 51,
            BitWrite = 96,
            BitRead = 97,
            WordWrite = 98,
            WordRead = 99,
            Test = 100,
            Simulation = 101,
            LotinformationSend = 102,
            FilmJobCommand = 103,

            UDPPortOpen = 104,
            UDPPortClose = 105,

            FI01_Check = 106, // 150122 고석현
            Recovery = 107, //[2015/03/10](Add by HS)

            Msg2MCC = 369,
            MCCDataSend = 370,

        }

        public enum OPCall
        {
            MSG = 0,                                                //일반 Message        
            OnlineModeChangeT3TimeOut = 1,                          //Online Mode전환시 T3 Time OUT Error 발생시
            T3TimeOut = 2,                                          //일반 T3 Time Out
            FormClose = 3,                                          //Message창을 닫는다.
            ModeChangeShow = 4,                                     //frmModeChange Form을 띄울시 사용한다.   
            Scrap = 5,                                              //Scrap 발생시 폼을 띄운다.(Comment를 입력하기 위해)
            Unscrap = 6,                                            //Unscrap 발생시 폼을 띄운다.(Comment를 입력하기 위해)
            GLSAbort = 7,                                           //GLSAbort 발생시 폼을 띄운다.(Comment를 입력하기 위해)
        }

        public enum OPCallOverWrite
        {
            MSGBuzzer = 0,                                          //일반 Message(Buzzer On)
            MSGNoBuzzer = 1,                                        //일반 Message(No Buzzer)
            MSGList = 2,                                            //Message List
            OPCallClear = 3,                                        //PLC 에서 OPCall Clear가 오면 CIM에서도 Clear 한다.        //추가 : 20100211 이상호
        }

        public enum LogType             //Log를 Write할 종류
        {
            PLC = 0,
            PLCError = 1,
            CIM = 2,
            Alarm = 3,
            ScrapUnScrapAbort = 4,
            GLSInOut = 5,
            GLSPDC = 6,
            LOTPDC = 7,
            Parameter = 8,
            MCCLog = 9,
            AlarmGLSInfo = 10,
            btnEVENT = 11,
            SEM = 12,
            OPCallMSG = 13,     // 20120320 이상창

            APC = 14,
            PPC = 15,
            RPC = 16,
        }

        public enum MsgType             //Main View의 Combo Display 종류
        {
            AlarmMsg = 0,
            HostMsg = 1,
        }

        public enum LogActFormShowType  //현재 활성화되어 있는 폼 종류
        {
            None = 0,
            AlarmListForm = 1,
            GLSAPDForm = 2,
            LogViewForm = 3,
            LOTAPDForm = 4,
            ScrapForm = 5,
            BrokenForm = 6,
        }

        /// <summary>
        /// PPC = 0, APC = 1, RPC = 2
        /// </summary>
        public enum ProcessDataType
        {
            PPC = 0,
            APC = 1,
            RPC = 2,
        }
        public enum ViewEvent
        {
            None = 0,
            EOIDUpdate = 1,
            ECIDUpdate = 2,
        }

        public enum PPIDCMD
        {
            None = 0,
            Create = 1,
            Delete = 2,
            Modify = 3,
            Search = 4,
            EQPPPID_Delete = 5,
            APC = 6,
        }

        #endregion

        #region "Message 관련 Queue 작업"
        /// <summary>
        /// Message Queue에서 Message를 가져온다.
        /// </summary>
        /// <returns></returns>
        public object funGetMessage()
        {
            object dobj = null;

            try
            {
                dobj = this.pMessageQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dobj;
        }

        /// <summary>
        /// Message Queue의 삽입항목 갯수를 가져온다.
        /// </summary>
        /// <returns></returns>
        public int funGetMessageCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pMessageQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            return dintCount;
        }

        /// <summary>
        /// Message 문자열을 Queue에 넣는다.
        /// </summary>
        /// <param name="dMsgType"></param>
        /// <param name="strMessage"></param>
        public void subMessage_Set(MsgType dMsgType, string strMessage)
        {
            StringBuilder dstrWriteMsg = new StringBuilder();

            try
            {
                strMessage = strMessage.Replace(';', ',');      //세미콜론(;)을 (,)로 바꾼다.

                dstrWriteMsg.Append((int)dMsgType + ";");
                dstrWriteMsg.Append("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] ");
                dstrWriteMsg.Append(strMessage);

                this.pMessageQueue.Enqueue(dstrWriteMsg);
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dMsgType:" + dMsgType + ", strMessage:" + strMessage);
            }
        }

        #endregion

        #region "OPCall"
        /// <summary>
        /// Operator Call할 화면 Type 및 Port정보 등을 저장한다.
        /// </summary>
        /// <param name="intOPCall">Operator Call할 Type</param>
        /// <param name="intPort">처리할 Port</param>
        /// <param name="strFromSF">On Line모드시 발생한 Stream Function</param>
        /// <param name="strHostMsg">On Line모드시 Host Message</param>
        public void subOPCall_Set(OPCall intOPCall, int intPort, string strFromSF, string strHostMsg)
        {
            clsOPCall dclsOPCall = new clsOPCall();

            try
            {
                dclsOPCall.intOPCallType = (int)intOPCall;
                dclsOPCall.intPortID = intPort;
                dclsOPCall.strFromSF = strFromSF;
                dclsOPCall.strHostMsg = strHostMsg;

                this.pOPCallQueue.Enqueue(dclsOPCall);
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intOPCall:" + intOPCall.ToString() + ", intPort:" + intPort +
                               ", strFromSF:" + strFromSF + ", strHostMsg:" + strHostMsg);
            }
        }

        public void subOPCallOverWrite_Set(OPCallOverWrite intOPCall, int intPort, string strFromSF, string strHostMsg)
        {
            clsOPCall dclsOPCallOverWrite = new clsOPCall();

            try
            {
                dclsOPCallOverWrite.intOPCallType = (int)intOPCall;
                dclsOPCallOverWrite.intPortID = intPort;
                dclsOPCallOverWrite.strFromSF = strFromSF;
                dclsOPCallOverWrite.strHostMsg = strHostMsg;

                this.pOPCallOverWriteQueue.Enqueue(dclsOPCallOverWrite);
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intOPCall:" + intOPCall.ToString() + ", intPort:" + intPort +
                               ", strFromSF:" + strFromSF + ", strHostMsg:" + strHostMsg);
            }
        }

        /// <summary>
        /// Operator Call Queue에서 가져와 처리한다.
        /// </summary>
        /// <returns>Operator Call할 화면 Type</returns>
        public object funGetOPCall()
        {
            object dobj = null;

            try
            {
                dobj = this.pOPCallQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dobj;
        }

        /// <summary>
        /// Operator Call Overwrite Queue에서 가져와 처리한다.
        /// </summary>
        /// <returns>Operator Call할 화면 Type</returns>
        public object funGetOPCallOverWrite()
        {
            object dobj = null;

            try
            {
                dobj = this.pOPCallOverWriteQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dobj;
        }

        /// <summary>
        /// Operator Call Queue개수를 가져온다.
        /// </summary>
        /// <returns>Operator Call Queue 개수</returns>
        public int funOPCallCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pOPCallQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }

        /// <summary>
        /// Operator Call Overwrite Queue개수를 가져온다.
        /// </summary>
        /// <returns>Operator Call Queue 개수</returns>
        public int funOPCallOverWriteCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pOPCallOverWriteQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }

        #endregion

        #region "PLCCommand"
        /// <s>
        /// PLC로 Write할 Command를 Set한다.
        /// </summary>
        /// <param name="intPLCCommand"> Write할 Command</param>
        /// <param name="objParameter"></param>
        public void subPLCCommand_Set(PLCCommand intPLCCommand, params object[] objParameter)        //PLCCommand Set
        {
            string dstrParameter = "";
            string dstrTemp = "";

            try
            {
                //PLC Command에 인자가 있을경우 콤마로 구분하여 결합한다.
                for (int i = 0; i <= objParameter.Length - 1; i++)
                {
                    dstrTemp = dstrTemp + objParameter[i] + ",";
                }

                //2012.07.17 Kim Youngsik... 
                //enum PLCCommand의 String 값으로 Command Class의 cmdName과 Matching 시켜 해당 Class를 찾는다.
                //dstrParameter = (int)intPLCCommand + "," + dstrTemp;
                dstrParameter = intPLCCommand.ToString() + "," + dstrTemp;
                clsParam pParam = new clsParam(intPLCCommand, objParameter);

                if (dstrParameter.Trim() != "")
                {
                    //this.pPLCWriteMethodQueue.Enqueue(dstrParameter);
                    this.pPLCWriteMethodQueue.Enqueue(pParam);
                }
                else { }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intPLCCommand:" + intPLCCommand.ToString());
            }
        }

        /// <summary>
        /// PLC Comamnd Queue에서 가져와 처리한다.
        /// </summary>
        /// <returns>Write할 Command</returns>
        public object funGetPLCCommand()                        //PLCCommand Get
        {
            object dobj = null;

            try
            {
                dobj = this.pPLCWriteMethodQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dobj;
        }

        /// <summary>
        /// PLC Comamnd Queue개수를 가져온다.
        /// </summary>
        /// <returns>PLC Comamnd Queue 개수</returns>
        public int funGetPLCCommandCount()                      //PLCCommand Count 반환
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pPLCWriteMethodQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }

        #endregion

        //LogSet 관련 작업
        #region "LogSet"

        /// <summary>
        /// xPC관련 로그만 이쪽일듯
        /// </summary>
        /// <param name="dLogType"></param>
        /// <param name="dDateTime"></param>
        /// <param name="strLog"></param>
        public void subLog_Set(LogType dLogType, DateTime dDateTime, string strLog)
        {
            StringBuilder dstrWriteMsg = new StringBuilder();

            try
            {
                strLog = strLog.Replace(';', ',');

                dstrWriteMsg.Append("[" + dDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "]" + String.Empty.PadLeft(50, '=') + "\r\n");
                dstrWriteMsg.Append(strLog);

                //2012.09.06 Kim Youngsik 추가
                if (dLogType == LogType.MCCLog)
                {
                    EnqueueMCCLog(dLogType.ToString(), dstrWriteMsg.ToString());
                }
                else
                {
                    EnqueueLog(dLogType.ToString(), dstrWriteMsg.ToString());
                }

                //lock (this.objPLCLogQueueLock)
                //{
                //    this.pPLCLogQueue.Enqueue(dstrWriteMsg);
                //}

                //Dummy Test시 Exception이 일어난 곳을 바로바로 찾아내기 위한 코드
                if (EQP("Main").DummyPLC == true)
                {
                    if (strLog.Contains("Exception") == true)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dLogType:" + dLogType + ", strLog:" + strLog);
            }
        }

        /// <summary>
        /// PLC 로그만 사용한다.
        /// </summary>
        /// <param name="dLogType"></param>
        /// <param name="strDateTime"></param>
        /// <param name="strLog"></param>
        public void subLog_Set(LogType dLogType, string strDateTime, string strLog)
        {
            StringBuilder dstrWriteMsg = new StringBuilder();
            string dstrTemp = "";

            try
            {
                strLog = strLog.Replace(';', ',');
                if (dLogType == LogType.PLC)
                {
                    dstrTemp = dstrTemp + strDateTime.Substring(0, 4) + "-";
                    dstrTemp = dstrTemp + strDateTime.Substring(4, 2) + "-";
                    dstrTemp = dstrTemp + strDateTime.Substring(6, 2) + " ";
                    dstrTemp = dstrTemp + strDateTime.Substring(8, 2) + ":";
                    dstrTemp = dstrTemp + strDateTime.Substring(10, 2) + ":";
                    dstrTemp = dstrTemp + strDateTime.Substring(12, 2) +".";
                    dstrTemp = dstrTemp + strDateTime.Substring(14, 3);

                    dstrWriteMsg.Append("[" + dstrTemp + "]" + String.Empty.PadLeft(50, '=') + "\r\n");
                    dstrWriteMsg.Append(strLog);
                }
                else
                {
                    dstrTemp = dstrTemp + strDateTime.Substring(0, 4) + "-";
                    dstrTemp = dstrTemp + strDateTime.Substring(4, 2) + "-";
                    dstrTemp = dstrTemp + strDateTime.Substring(6, 2) + " ";
                    dstrTemp = dstrTemp + strDateTime.Substring(8, 2) + ":";
                    dstrTemp = dstrTemp + strDateTime.Substring(10, 2) + ":";
                    dstrTemp = dstrTemp + strDateTime.Substring(12, 2);// +".";
                    //dstrTemp = dstrTemp + strDateTime.Substring(14, 3);

                    dstrWriteMsg.Append("[" + dstrTemp + "]" + String.Empty.PadLeft(50, '=') + "\r\n");
                    dstrWriteMsg.Append(strLog);
                }

                //2012.09.06 Kim Youngsik 추가
                if (dLogType == LogType.MCCLog)
                {
                    EnqueueMCCLog(dLogType.ToString(), dstrWriteMsg.ToString());
                }
                else
                {
                    EnqueueLog(dLogType.ToString(), dstrWriteMsg.ToString());
                }

                //lock (this.objPLCLogQueueLock)
                //{
                //    this.pPLCLogQueue.Enqueue(dstrWriteMsg);
                //}

                //Dummy Test시 Exception이 일어난 곳을 바로바로 찾아내기 위한 코드
                if (EQP("Main").DummyPLC == true)
                {
                    if (strLog.Contains("Exception") == true)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dLogType:" + dLogType + ", strLog:" + strLog);
            }
        }

        /// <summary>
        /// Log 문자열을 Queue에 넣는다. 로그타입별로 Queue 분리
        /// </summary>
        /// <param name="dLogType"></param>
        /// <param name="strLog">출력할 Log 문자열</param>
        /// <comment>
        /// 각 Log 종류별로 각각 다른 Queue에 넣는다.
        /// </comment>
        public void subLog_Set(LogType dLogType, string strLog)
        {
            StringBuilder dstrWriteMsg = new StringBuilder();

            try
            {
                strLog = strLog.Replace(';', ',');

                //CIM, GLS In/Out 로그는 시간을 제외한다.
                if (dLogType == InfoAct.clsInfo.LogType.CIM || dLogType == InfoAct.clsInfo.LogType.PLCError)
                {
                    dstrWriteMsg.Append("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + String.Empty.PadLeft(50, '=') + "\r\n");
                }
                else if (dLogType == InfoAct.clsInfo.LogType.OPCallMSG) // 20120320 이상창
                {
                    dstrWriteMsg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",");
                }
                else if (dLogType == clsInfo.LogType.GLSInOut)
                {
                    dstrWriteMsg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+",");
                }
                dstrWriteMsg.Append(strLog);

                //2012.09.06 Kim Youngsik 추가
                if (dLogType == LogType.MCCLog)
                {
                    EnqueueMCCLog(dLogType.ToString(), dstrWriteMsg.ToString());
                }
                else
                {
                    EnqueueLog(dLogType.ToString(), dstrWriteMsg.ToString());
                }

                //Dummy Test시 Exception이 일어난 곳을 바로바로 찾아내기 위한 코드
                if (EQP("Main").DummyPLC == true)
                {
                    if (strLog.Contains("Exception") == true)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dLogType:" + dLogType + ", strLog:" + strLog);
            }
        }
        #endregion

        #region "HOST 송신 SF"
        /// <summary>
        /// HOST로 전송할 SF을 Set한다.
        /// </summary>
        /// <param name="intSFName">HOST로 전송할 SF</param>
        /// <param name="objParameter"></param>
        public void subSendSF_Set(SFName intSFName, params object[] objParameter)                 //전송할 SF Set
        {
            string dstrParameter = "";
            string dstrTemp = "";

            try
            {
                //SF 송신시 인자가 있을경우 ";"로 구분하여 결합한다.
                for (int i = 0; i <= objParameter.Length - 1; i++)
                {
                    dstrTemp = dstrTemp + objParameter[i] + ",";
                }
                dstrParameter = (int)intSFName + "," + dstrTemp;

                if (dstrParameter.Trim() != "")
                {
                    //HostAct로 이벤트를 발생시켜 전송할 메세지를 만든다.
                    if (CreateSFEvent != null)
                    {
                        CreateSFEvent(dstrParameter);
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intSFName:" + intSFName.ToString());
            }
        }

        /// <summary>
        /// HOST로 전송할 생성된 SF Object를 구조체에 저장
        /// </summary>
        /// <param name="objParam">SF 개체</param>
        public void subSendHostSF_Set(object objParam)
        {
            try
            {
                this.pSendQueue.Enqueue(objParam);
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// HOST로 전송할 SF을 가져와 처리한다.
        /// </summary>
        /// <returns>HOST로 전송할 SF</returns>
        public object funGetSendSF()                            //전송할 SF Get
        {
            object dstrReturn = null;

            try
            {
                if (this.pSendQueue.Count > 0)
                    dstrReturn = this.pSendQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrReturn;
        }

        /// <summary>
        /// HOST로 전송할 SF 개수를 가져온다.
        /// </summary>
        /// <returns>HOST로 전송할 SF 개수</returns>
        public int funGetSendSFCount()                          //전송할 SF Count 반환
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pSendQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }

        /// <summary>
        /// HOST로 수신한 SF을 저장
        /// </summary>
        /// <param name="strParam">SF 개체</param>
        public void subReceiveHostSF_Set(object strParam)
        {
            try
            {
                this.pReceiveHostQueue.Enqueue(strParam);
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// HOST로 수신한 SF을 가져와 처리한다.
        /// </summary>
        /// <returns>HOST로 전송할 SF</returns>
        public object funGetReceiveSF()
        {
            object dstrReturn = null;

            try
            {
                if (this.pReceiveHostQueue.Count > 0)
                    dstrReturn = this.pReceiveHostQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrReturn;
        }

        /// <summary>
        /// HOST로 전송할 SF 개수를 가져온다.
        /// </summary>
        /// <returns>HOST로 전송할 SF 개수</returns>
        public int funGetReceiveSFCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pReceiveHostQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }

        #endregion

        // XPC Override Delay 작업
        #region "XPC Override Delay 작업"
        public void subXPCOverride_Set(SFName intSFName, string strGLSID)
        {

            try
            {
                //clsXPCoverride xpc = new clsXPCoverride(DateTime.Now.AddSeconds(2), intSFName, strGLSID);
                clsXPCoverride xpc = new clsXPCoverride(DateTime.Now, intSFName, strGLSID);
                this.pXPCDelayQueue.Enqueue(xpc);

            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        public int funGetXPCOverrideCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pXPCDelayQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }
        public object funPeekXPCOverride()
        {
            object dstrReturn = null;

            try
            {
                if (this.pXPCDelayQueue.Count > 0)
                    dstrReturn = this.pXPCDelayQueue.Peek();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrReturn;
        }
        public object funGetXPCOverride()
        {
            object dstrReturn = null;

            try
            {
                if (this.pXPCDelayQueue.Count > 0)
                    dstrReturn = this.pXPCDelayQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrReturn;
        }

        #endregion

        #region "LogAct 폼 Show 체크 관련"
        /// <summary>
        /// LogAct 폼(frmAlarmList, frmGLSAPD, frmLogView, frmLOTAPD)을 띄우기 위해 저장
        /// </summary>
        public LogActFormShowType proLogActFormShowIndex
        {
            get
            {
                return pLogActFormShowIndex;
            }

            set
            {
                if (this.pLogActFormShowIndex == LogActFormShowType.None || value == LogActFormShowType.None)
                {
                    pLogActFormShowIndex = value;
                }
            }
        }

        #endregion

        #region "기타 메소드"
        /// <summary>
        /// 각 장비(CD(A, B Type), Wet Etch & Strip, Strip)의 Unit별로 이름을 가져온다.
        /// </summary>
        /// <param name="strModuleID">Module ID</param>
        /// <returns></returns>
        /// <comment>
        /// 주로 DIsplay용으로 사용됨
        /// </comment>
        public int funGetModuleIDToUnitID(string strModuleID)
        {
            int dintUnitID = 0;

            try
            {
                strModuleID = strModuleID.ToUpper().Trim();

                for (int dintUnit = 0; dintUnit <= this.EQP("Main").UnitCount; dintUnit++)
                {
                    if (this.Unit(dintUnit).SubUnit(0).ModuleID == strModuleID)
                    {
                        dintUnitID = dintUnit;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintUnitID;
        }

        /// <summary>
        /// A2CVD01S_CLNU_LD01 => LD01
        /// </summary>
        /// <param name="intUnitID"></param>
        /// <returns></returns>
        public string funGetModuleIDToUnitName(int intUnitID)
        {
            string dstrUnitName = "";
            string dstrModuleID = "";
            string[] dstrValue;

            try
            {
                dstrModuleID = this.Unit(intUnitID).SubUnit(0).ModuleID;

                if (intUnitID != 0)
                {
                    dstrValue = dstrModuleID.Split(new char[] { '_' });
                    dstrUnitName = dstrValue[dstrValue.Length - 1];
                }
                else
                {
                    dstrUnitName = dstrModuleID;
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrUnitName;
        }

        /// <summary>
        /// SWR2_UP_MOVE_START => START
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        //public string funGetMCCNameToActionData(int index)
        //{
        //    string dstrMCCName = "";
        //    string dstrActionData = "";
        //    string[] dstrValue;

        //    try
        //    {
        //        dstrMCCName = this.Unit(0).SubUnit(0).MCCList(index).MCCListName;
        //        dstrValue = dstrMCCName.Split(new char[] { '_' });
        //        dstrActionData = dstrValue[dstrValue.Length - 1];
        //    }
        //    catch (Exception ex)
        //    {
        //        this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
        //    }

        //    return dstrActionData;
        //}


        public string funEQPState(string strEQPState)
        {
            string dstrEQPState = "";

            try
            {
                switch (strEQPState)
                {
                    case "1":
                        dstrEQPState = "NORMAL";
                        break;

                    case "2":
                        dstrEQPState = "FAULT";
                        break;

                    case "3":
                        dstrEQPState = "PM";
                        break;

                    default:
                        dstrEQPState = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrEQPState;
        }

        public string funEQPProcessState(string strEQPProcessState)
        {
            string dstrEQPProcessState = "";

            try
            {
                switch (strEQPProcessState)
                {
                    case "1":
                        dstrEQPProcessState = "IDLE";
                        break;

                    case "2":
                        dstrEQPProcessState = "SETUP";
                        break;

                    case "3":
                        dstrEQPProcessState = "EXECUTING";
                        break;

                    case "4":
                        dstrEQPProcessState = "PAUSE";
                        break;

                    default:
                        dstrEQPProcessState = "";
                        break;
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrEQPProcessState;
        }

        public int funGetEOIDToIndex(int intEOID)
        {
            int dintReturn = 0;

            try
            {
                foreach (int dintIndex in this.Unit(0).SubUnit(0).EOID())
                {
                    if (this.Unit(0).SubUnit(0).EOID(dintIndex).EOID == intEOID)
                    {
                        dintReturn = dintIndex;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintReturn;
        }

        public int funGetEOIDEOMDToIndex(int intEOID, int intEOMD)
        {
            int dintReturn = 0;

            try
            {
                foreach (int dintIndex in this.Unit(0).SubUnit(0).EOID())
                {
                    if (this.Unit(0).SubUnit(0).EOID(dintIndex).EOID == intEOID && this.Unit(0).SubUnit(0).EOID(dintIndex).EOMD == intEOMD)
                    {
                        dintReturn = dintIndex;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintReturn;
        }

        public int funGetEOIDItemToIndex(EOID eEOIDItem)
        {
            int dintIndex = 0;

            try
            {
                //tbEOID 테이블의 해당 항목에 해당하는 Index 컬럼값
                switch (eEOIDItem)
                {
                    case EOID.VCRReadingMode:
                        dintIndex = 1;
                        break;

                    case EOID.APCMode:
                        dintIndex = 2;
                        break;

                    case EOID.MCCReportingMode:
                        dintIndex = 3;
                        break;

                    default:
                        dintIndex = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintIndex;
        }

        public int funGetEOIDNameToIndex(string strEOIDName)
        {
            int dintIndex = 0;

            try
            {
                foreach (int index in Unit(0).SubUnit(0).EOID())
                {
                    clsEOID tempEOID = this.Unit(0).SubUnit(0).EOID(index);

                    if (tempEOID.DESC.Equals(strEOIDName.Trim()))
                    {
                        dintIndex = tempEOID.Index;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintIndex;
        }
        #endregion

        #region "응용프로그램 공통(All)"
        public bool AddAll()
        {
            clsAll dclsAll = null;     //EQP 개체 선언

            try
            {
                dclsAll = new clsAll();
                pAllHashtable.Add("All", dclsAll);      //오직 1개뿐이다(Key값은 "All"로 한다.)

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsAll All
        {
            get { return (clsAll)pAllHashtable["All"]; }
        }

        public int AllCount
        {
            get { return pAllHashtable.Count; }
        }

        #endregion

        #region "EQP"
        public bool AddEQP(string strEQPID)
        {
            clsEQP dclsEQP = null;     //EQP 개체 선언

            string dstrEQPID = "";

            try
            {
                dstrEQPID = strEQPID.Trim();
                if (dstrEQPID == "" || dstrEQPID == null || pEQPHashtable.Contains(dstrEQPID))
                {
                    return false;
                }

                dclsEQP = new clsEQP(strEQPID);

                pEQPHashtable.Add(dstrEQPID, dclsEQP);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsEQP EQP(string strEQPID)
        {
            try
            {
                string dstrEQPID = strEQPID.Trim();

                if (pEQPHashtable.Contains(dstrEQPID))
                {
                    return (clsEQP)pEQPHashtable[dstrEQPID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection EQP()
        {
            try
            {
                return pEQPHashtable.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int EQPCount
        {
            get { return pEQPHashtable.Count; }
        }

        #endregion

        #region "Unit"
        public bool AddUnit(int intUnitID)
        {
            clsUnit dclsUnit;     //Unit 개체 선언

            try
            {
                if (intUnitID < 0)
                {
                    return false;
                }
                else
                {
                    if (pUnitHash.Contains(intUnitID))
                    {
                        return true;
                    }
                    else
                    {
                        dclsUnit = new clsUnit(intUnitID);
                        pUnitHash.Add(intUnitID, dclsUnit);

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsUnit Unit(int intUnitID)
        {
            try
            {
                if (pUnitHash.Contains(intUnitID))
                {
                    return (clsUnit)pUnitHash[intUnitID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection Unit()
        {
            try
            {
                return pUnitHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int UnitCount
        {
            //장비전체(UnitID=0)는 개수에서 제외한다.
            get { return pUnitHash.Count - 1; }
        }

        #endregion

        #region "ModuleID"
        public bool AddModuleID(string ModuleID)
        {
            clsUnit dclsUnit;     //Unit 개체 선언

            try
            {
                if (ModuleID == "")
                {
                    return false;
                }
                else
                {
                    if (pModuleIDHash.Contains(ModuleID))
                    {
                        return true;
                    }
                    else
                    {
                        dclsUnit = new clsUnit(ModuleID);
                        pModuleIDHash.Add(ModuleID, dclsUnit);

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsUnit ModuleID(string strModuleID)
        {
            try
            {
                if (pModuleIDHash.Contains(strModuleID))
                {
                    return (clsUnit)pModuleIDHash[strModuleID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection ModuleID()
        {
            try
            {
                return pModuleIDHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public int ModuleIDCount
        {
            //장비전체(UnitID=0)는 개수에서 제외한다.
            get { return pModuleIDHash.Count - 1; }
        }

        #endregion

        #region "LOT"
        public int AddLOT(string strLOTID)
        {
            clsLOT dclsLot = null;     //LOT 개체 선언
            string dstrLOTID = "";
            int dintLOTIndex = 0;

            try
            {
                dstrLOTID = strLOTID.Trim();
                if (dstrLOTID == "" || dstrLOTID == null)
                {
                    return 0;
                }

                //현재까지 발번한 LOTIndex(1~49)
                dintLOTIndex = Convert.ToInt32(FunINIMethod.funINIReadValue("ETCInfo", "CurrentLOTIndex", "0", this.All.SystemINIFilePath));
                dintLOTIndex = dintLOTIndex + 1;
                if (dintLOTIndex > 50) dintLOTIndex = 1;


                //Dummy로 테스트시는 무조건 LOTIndex를 1로 한다. 만약 1로 하지 않으면 시나리오 돌리면서 에러 발생.
                if (this.EQP("Main").DummyPLC == true) dintLOTIndex = 1;

                //만약 현재 발번할 LOTIndex가 존재하면 지운다.
                if (pLOTIndexHash.Contains(dintLOTIndex))
                {
                    this.subLog_Set(LogType.CIM, "Because LOTIndex exists, LOTIndex be deleted !!!, LOTIndex: " + dintLOTIndex.ToString());
                    RemoveLOTIndex(dintLOTIndex);
                }


                dclsLot = new clsLOT(dintLOTIndex, dstrLOTID);

                //LOT정보 생성시 Slot정보도 같이 생성한다.
                for (int dintSlotID = 1; dintSlotID <= this.EQP("Main").SlotCount; dintSlotID++)
                {
                    if (dclsLot.AddSlot(dintSlotID) == false) return 0;
                }

                //pLOTHash.Add(dstrLOTID + "," + dintLOTIndex.ToString(), dclsLot);
                pLOTHash.Add(dstrLOTID, dclsLot);
                pLOTIndexHash.Add(dintLOTIndex, dstrLOTID);

                //발번한 LOT Index를 저장한다.
                this.All.CurrentLOTIndex = dintLOTIndex;      //구조체에 저장
                FunINIMethod.subINIWriteValue("ETCInfo", "CurrentLOTIndex", dintLOTIndex.ToString(), this.All.SystemINIFilePath);   //파일에 저장

                return dintLOTIndex;
            }
            catch
            {
                return 0;
            }
            finally
            {
            }
        }

        public clsLOT LOTID(string strLOTID)
        {
            try
            {
                string dstrLOTID = strLOTID.Trim();

                if (pLOTHash.Contains(dstrLOTID))
                {
                    return (clsLOT)pLOTHash[dstrLOTID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public clsLOT LOTIndex(int intLOTIndex)
        {
            try
            {
                int dintLOTIndex = intLOTIndex;
                if (pLOTIndexHash[dintLOTIndex] != null)
                {
                    string dstrLOTID = pLOTIndexHash[dintLOTIndex].ToString();  //LOTIndex에 매핑되는 LOTID를 가져온다.

                    if (dintLOTIndex <= 0 || dintLOTIndex > 50 || dstrLOTID == "" || dstrLOTID == null)
                    {
                        return null;
                    }

                    //if (pLOTHash.Contains(dstrLOTID + "," + intLOTIndex))
                    if (pLOTHash.Contains(dstrLOTID))
                    {
                        return (clsLOT)pLOTHash[dstrLOTID];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection LOT()
        {
            try
            {
                return pLOTHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection LOTIndex()
        {
            try
            {
                return pLOTIndexHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveLOTID(string strLOTID)
        {
            int dintLOTIndex = 0;

            try
            {
                string dstrLOTID = strLOTID.Trim();

                if (pLOTHash.Contains(dstrLOTID))
                {
                    dintLOTIndex = ((clsLOT)pLOTHash[dstrLOTID]).LOTIndex;   //LOTID에 매핑되는 LOTIndex를 가져온다.
                }

                if (dintLOTIndex <= 0 || dintLOTIndex > 50 || dstrLOTID == "" || dstrLOTID == null)
                {
                    return false;
                }

                if (pLOTHash.Contains(dstrLOTID))
                {
                    pLOTHash.Remove(dstrLOTID);     //HashTable에서 해당 LOT을 제거한다.
                    pLOTIndexHash.Remove(dintLOTIndex);

                    this.subLog_Set(LogType.CIM, "RemoveLOTID, LOTID: " + dstrLOTID + ", LOTIndex: " + dintLOTIndex.ToString());

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveLOTIndex(int intLOTIndex)
        {
            try
            {
                int dintLOTIndex = intLOTIndex;
                if (pLOTIndexHash[dintLOTIndex] != null)
                {
                    //string dstrLOTID = pLOTIndexHash[dintLOTIndex].ToString();    //LOTIndex로 LOTID를 가져온다.

                    if (dintLOTIndex <= 0 || dintLOTIndex > 50) // || dstrLOTID == "" || dstrLOTID == null)
                    {
                        return false;
                    }

                    if (pLOTIndexHash.Contains(dintLOTIndex))
                    {
                        //pLOTHash.Remove(dstrLOTID);     //HashTable에서 해당 LOT을 제거한다.
                        pLOTIndexHash.Remove(dintLOTIndex);

                        this.subLog_Set(LogType.CIM, "RemoveLOTIndex, LOTID: " + dintLOTIndex.ToString());

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;

                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveLOT()
        {
            try
            {
                pLOTHash.Clear();        //HashTable에서 모든 LOT을 제거한다.
                pLOTIndexHash.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int LOTCount
        {
            get { return pLOTIndexHash.Count; }
        }

        #endregion

        #region "DataTable"
        public int AddDataTable(string strTableName, DataTable dtTable)
        {
            DataTable dtNew = new DataTable();
            dtNew = dtTable.Clone();

            if (pDataTableHash.Contains(strTableName.ToUpper())) return -1;

            pDataTableHash.Add(strTableName.ToUpper(), dtTable);

            return 0;
        }

        public DataTable Table(string strTableName)
        {
            if (pDataTableHash.Contains(strTableName.ToUpper()))
            {
                return (DataTable)pDataTableHash[strTableName.ToUpper()];
            }
            else
            {
                return null;
            }
        }

        public bool DeleteTable(string strTableName)
        {
            if (pDataTableHash.Contains(strTableName.ToUpper()))
            {
                pDataTableHash.Remove(strTableName.ToUpper());
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region "APC"
        public bool AddAPC(clsAPC dclsAPC)
        {
            try
            {
                if (dclsAPC == null) return false;

                if (pAPCHashtable.Contains(dclsAPC.GLSID))
                {
                    this.subLog_Set(LogType.CIM, "Because GlassID exists, GlassID be deleted !!!, GlassID: " + dclsAPC.GLSID);
                    RemoveAPC(dclsAPC.GLSID);
                }

                pAPCHashtable.Add(dclsAPC.GLSID, dclsAPC);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool AddAPC(string strGLSID)
        {
            clsAPC dclsAPC = null;     //APC 객체 선언
            string dstrGLSID = "";

            try
            {
                dstrGLSID = strGLSID.Trim();
                if (dstrGLSID == "" || dstrGLSID == null)
                {
                    return false;
                }

                //만약 현재 발번할 LOTIndex가 존재하면 지운다.
                if (pAPCHashtable.Contains(dstrGLSID))
                {
                    this.subLog_Set(LogType.CIM, "Because GlassID exists, GlassID be deleted !!!, GlassID: " + dstrGLSID.ToString());
                    RemoveAPC(dstrGLSID);
                }

                dclsAPC = new clsAPC(dstrGLSID);
                pAPCHashtable.Add(dstrGLSID, dclsAPC);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsAPC APC(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pAPCHashtable.Contains(dstrGLSID))
                {
                    return (clsAPC)pAPCHashtable[dstrGLSID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection APC()
        {
            try
            {
                return pAPCHashtable.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveAPC(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pAPCHashtable.Contains(dstrGLSID))
                {
                    pAPCHashtable.Remove(dstrGLSID);
                    this.subLog_Set(LogType.CIM, "RemoveAPC, GLSID: " + dstrGLSID);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveAPC()
        {
            try
            {
                pAPCHashtable.Clear();        //HashTable에서 모든 APC을 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int APCCount
        {
            get { return pAPCHashtable.Count; }
        }

        #endregion

        #region "RPC"
        public bool AddRPC(clsRPC dclsRPC)
        {
            try
            {
                if (dclsRPC == null) return false;

                if (pRPCHashtable.Contains(dclsRPC.HGLSID))
                {
                    this.subLog_Set(LogType.CIM, "Because GlassID exists, GlassID be deleted !!!, GlassID: " + dclsRPC.HGLSID);
                    RemoveRPC(dclsRPC.HGLSID);
                }

                pRPCHashtable.Add(dclsRPC.HGLSID, dclsRPC);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }
        public bool AddRPC(string strGLSID)
        {
            clsRPC dclsRPC = null;     //RPC 객체 선언
            string dstrGLSID = "";

            try
            {
                dstrGLSID = strGLSID.Trim();
                if (dstrGLSID == "" || dstrGLSID == null)
                {
                    return false;
                }

                if (pRPCHashtable.Contains(dstrGLSID))
                {
                    this.subLog_Set(LogType.CIM, "Because GlassID exists, GlassID be deleted !!!, GlassID: " + dstrGLSID.ToString());
                    RemoveRPC(dstrGLSID);
                }

                dclsRPC = new clsRPC(dstrGLSID);
                pRPCHashtable.Add(dstrGLSID, dclsRPC);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsRPC RPC(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pRPCHashtable.Contains(dstrGLSID))
                {
                    return (clsRPC)pRPCHashtable[dstrGLSID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection RPC()
        {
            try
            {
                return pRPCHashtable.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveRPC(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pRPCHashtable.Contains(dstrGLSID))
                {
                    pRPCHashtable.Remove(dstrGLSID);
                    this.subLog_Set(LogType.CIM, "RemoveRPC, GLSID: " + dstrGLSID);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveRPC()
        {
            try
            {
                pRPCHashtable.Clear();        //HashTable에서 모든 RPC을 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int RPCCount
        {
            get { return pRPCHashtable.Count; }
        }

        #endregion

        #region "PPC"
        public bool AddPPC(string strGLSID)
        {
            clsPPC dclsPPC = null;     //PPC 객체 선언
            string dstrGLSID = "";

            try
            {
                dstrGLSID = strGLSID.Trim();
                if (dstrGLSID == "" || dstrGLSID == null)
                {
                    return false;
                }

                //만약 현재 발번할 LOTIndex가 존재하면 지운다.
                if (pPPCHashtable.Contains(dstrGLSID))
                {
                    this.subLog_Set(LogType.CIM, "Because GlassID exists, GlassID be deleted !!!, GlassID: " + dstrGLSID.ToString());
                    RemovePPC(dstrGLSID);
                }

                dclsPPC = new clsPPC(dstrGLSID);
                pPPCHashtable.Add(dstrGLSID, dclsPPC);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsPPC PPC(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pPPCHashtable.Contains(dstrGLSID))
                {
                    return (clsPPC)pPPCHashtable[dstrGLSID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection PPC()
        {
            try
            {
                return pPPCHashtable.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemovePPC(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pPPCHashtable.Contains(dstrGLSID))
                {
                    pPPCHashtable.Remove(dstrGLSID);
                    this.subLog_Set(LogType.CIM, "RemovePPC, GLSID: " + dstrGLSID);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemovePPC()
        {
            try
            {
                pPPCHashtable.Clear();        //HashTable에서 모든 PPC을 제거한다.
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int PPCCount
        {
            get { return pPPCHashtable.Count; }
        }


        #endregion

        #region "Log Queue Hash"
        public int AddLogQueue(string strLogName)
        {
            if (pLogQueueHash.Contains(strLogName))
            {
                return -1;
            }
            else
            {
                Queue queueNew = new Queue();
                pLogQueueHash.Add(strLogName, queueNew);
            }

            return 0;
        }

        public int LogQueueCount(string strLogName)
        {
            lock (pobjLockLogQueueCount)
            {
                if (pLogQueueHash.Contains(strLogName))
                {
                    return ((Queue)pLogQueueHash[strLogName]).Count;
                }

                return 0;
            }
        }

        public int EnqueueLog(string strLogName, string strLogText)
        {
            lock (pobjLockEnqueueLog)
            {
                if (pLogQueueHash.Contains(strLogName))
                {
                    ((Queue)pLogQueueHash[strLogName]).Enqueue(strLogText);
                    return 0;
                }

                return -1;
            }
        }

        public string DequeueLog(string strLogName)
        {
            lock (pobjLockDequeueLog)
            {
                if (pLogQueueHash.Contains(strLogName))
                {
                    return (string)((Queue)pLogQueueHash[strLogName]).Dequeue();
                }

                return "";
            }
        }
        #endregion

        #region "MCC Log Queue Hash"
        public int AddMCCLogQueue(string strMCCLogName)
        {
            if (pMCCLogQueueHash.Contains(strMCCLogName))
            {
                return -1;
            }
            else
            {
                Queue queueMCCNew = new Queue();
                pMCCLogQueueHash.Add(strMCCLogName, queueMCCNew);
            }

            return 0;

        }

        public int MCCLogQueueCount(string strMCCLogName)
        {
            lock (pobjLockMCCLogQueueCount)
            {
                if (pMCCLogQueueHash.Contains(strMCCLogName))
                {
                    return ((Queue)pMCCLogQueueHash[strMCCLogName]).Count;
                }

                return 0;
            }
        }

        public int EnqueueMCCLog(string strMCCLogName, string strMCCLogText)
        {
            lock (pobjLockMCCEnqueueLog)
            {
                if (pMCCLogQueueHash.Contains(strMCCLogName))
                {
                    ((Queue)pMCCLogQueueHash[strMCCLogName]).Enqueue(strMCCLogText);
                    return 0;
                }

                return -1;
            }
        }

        public string DequeueMCCLog(string strMCCLogName)
        {
            lock (pobjLockMCCDequeueLog)
            {
                if (pMCCLogQueueHash.Contains(strMCCLogName))
                {
                    return (string)((Queue)pMCCLogQueueHash[strMCCLogName]).Dequeue();
                }

                return "";
            }
        }
        #endregion

        #region "ModuleID To UnitID Queue Hash"

        public bool AddModuleIDToUnitID(string strModuleID, string strUnitID)
        {
            if(pHashModuleIDToUnitID.Contains(strModuleID) == false)
            {
                pHashModuleIDToUnitID.Add(strModuleID, strUnitID);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ModuleIDToUnitID(string strModuleID)
        {
            if (pHashModuleIDToUnitID.Contains(strModuleID))
            {
                return pHashModuleIDToUnitID[strModuleID].ToString();
            }
            else
            {
                return null;
            }
        }

        public void RemoveModuleIDToUnitID()
        {
            pHashModuleIDToUnitID.Clear();
        }


        public bool RemoveModuleIDToUnitID(string strModuleID)
        {
            if (pHashModuleIDToUnitID.Contains(strModuleID))
            {
                pHashModuleIDToUnitID.Remove(strModuleID);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int ModuleIDToUnitIDCount()
        {
            return pHashModuleIDToUnitID.Count;
        }

        public ICollection ModuleIDToUnitID()
        {
            return pHashModuleIDToUnitID.Keys;
        }
        #endregion

        #region ProcessData Report Command
        ///// <summary>
        ///// APC, RPC, PPD Data를 DB에서 삭제하는 함수
        ///// </summary>
        ///// <param name="Type">APC, PPC, RPC</param> 
        ///// <param name="strHGLSID">삭제할 GlassID(strHGLSID가 ""시 전체 삭제)</param>
        ///// <param name="strMode">2:Fix</param>
        ///// <param name="ReportFlag">True : 삭제보고 함, Flase : 삭제보고 안함.</param>
        //public bool ProcessDataDel(InfoAct.clsInfo.ProcessDataType Type, string strMode, string strHGLSID, bool ReportFlag)
        //{
        //    string dstrSQL = "";
        //    bool dbolResult = true;
        //    try
        //    {
        //        if (strHGLSID == "") // 전체 삭제
        //        {
        //            switch (Type)
        //            {
        //                case ProcessDataType.APC:
        //                    foreach (string apcGLSID in APC())
        //                    {
        //                        if (funDBDelete(Type, apcGLSID) == false)
        //                        {
        //                            dbolResult = false;
        //                        }
        //                    }
        //                    break;

        //                case ProcessDataType.PPC:
        //                    foreach (string ppcGLSID in PPC())
        //                    {
        //                        if (funDBDelete(Type, ppcGLSID) == false)
        //                        {
        //                            dbolResult = false;
        //                        }
        //                    }
        //                    break;

        //                case ProcessDataType.RPC:
        //                    foreach (string rpcGLSID in RPC())
        //                    {
        //                        if (funDBDelete(Type, rpcGLSID) == false)
        //                        {
        //                            dbolResult = false;
        //                        }
        //                    }
        //                    break;

        //                default:
        //                    subLog_Set(LogType.CIM, "ProcessDataType이 올바르지 않습니다.");
        //                    break;
        //            }
        //        }
        //        else //개별삭제
        //        {
        //            funDBDelete(Type, strHGLSID);
        //        }


        //        //삭제보고
        //        switch (Type)
        //        {
        //            case ProcessDataType.APC:
        //                if (ReportFlag == true)
        //                {
        //                    subSendSF_Set(InfoAct.clsInfo.SFName.S16F115APCDataCMD, strMode, strHGLSID); //Deletion : 2
        //                }
        //                else
        //                {
        //                    RemoveAPC();    //전체 RPC Data 삭제
        //                }
        //                break;

        //            case ProcessDataType.PPC:
        //                if (ReportFlag == true)
        //                {
        //                    subSendSF_Set(InfoAct.clsInfo.SFName.S16F15PPCDataCMD, strMode, strHGLSID); //Deletion : 2
        //                }
        //                else
        //                {
        //                    RemovePPC();    //전체 RPC Data 삭제
        //                }
        //                break;

        //            case ProcessDataType.RPC:
        //                if (ReportFlag == true)
        //                {
        //                    subSendSF_Set(InfoAct.clsInfo.SFName.S16F135RPCDataCMD, strMode, strHGLSID); //Deletion : 2
        //                }
        //                else
        //                {
        //                    RemoveRPC();    //전체 RPC Data 삭제
        //                }
        //                break;
        //        }
        //        return dbolResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        subLog_Set(LogType.CIM, ex.ToString());
        //        return dbolResult;
        //    }
        //}

        public bool funDBDelete(ProcessDataType Type, string strHGLSID)
        {
            bool dbolDBUpdateFlag = false;
            string dstrSQL = "";
            try
            {
                dstrSQL = string.Format("DELETE FROM `tb{0}` WHERE `H_GLASSID`='{1}';", Type.ToString(), strHGLSID);
                dbolDBUpdateFlag = DBAct.clsDBAct.funExecuteQuery(dstrSQL);

            }
            catch (Exception ex)
            {
                subLog_Set(LogType.CIM, ex.ToString());
            }
            return dbolDBUpdateFlag;
        }

        ///// <summary>
        ///// APC, RPC, PPD Data를 생성, 수정 보고
        ///// </summary>
        ///// <param name="Type">APC, PPC, RPC</param> 
        ///// <param name="strHGLSID">GlassID</param>
        ///// <param name="strMode">1 : 생성, 3: 수정</param>
        ///// <param name="ReportFlag"></param>
        //public bool ProcessDataSet(InfoAct.clsInfo.ProcessDataType Type, string strMode, string strHGLSID)
        //{
        //    string dstrSQL = "";
        //    bool dbolResult = true;
        //    try
        //    {
        //        //DB Update
        //        if (funDBUpdate(Type, strHGLSID) == false)
        //        {
        //            //DB Update Fail !!
        //            dbolResult = false;
        //        }

        //        //Log
                
        //        //MES 보고
        //        switch (Type)
        //        {
        //            case ProcessDataType.APC:
        //                if (strMode == "4")
        //                {
        //                    subXPCOverride_Set(InfoAct.clsInfo.SFName.S16F115APCDataCMD, strHGLSID);
        //                }
        //                subSendSF_Set(InfoAct.clsInfo.SFName.S16F115APCDataCMD, strMode, strHGLSID); //Deletion : 2
        //                break;

        //            case ProcessDataType.PPC:
        //                if (strMode == "4")
        //                {
        //                    subXPCOverride_Set(InfoAct.clsInfo.SFName.S16F15PPCDataCMD, strHGLSID);
        //                }
        //                subSendSF_Set(InfoAct.clsInfo.SFName.S16F15PPCDataCMD, strMode, strHGLSID); //Deletion : 2
        //                break;

        //            case ProcessDataType.RPC:
        //                if (strMode == "4")
        //                {
        //                    subXPCOverride_Set(InfoAct.clsInfo.SFName.S16F135RPCDataCMD, strHGLSID);
        //                }
        //                subSendSF_Set(InfoAct.clsInfo.SFName.S16F135RPCDataCMD, strMode, strHGLSID); //Deletion : 2
        //                break;
        //        }

        //        return dbolResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        subLog_Set(LogType.CIM, ex.ToString());
        //        return dbolResult;
        //    }
        //}

        private object DBlock = "DBLOCK";

        public bool funDBUpdate(ProcessDataType Type, string strHGLSID)
        {
            bool dbolDBUpdateFlag = true;
            string dstrSQL = "";
            int dintCenterIndex = 0;
            string dstrCenterName = "";
            ArrayList arrCon;
            string dstrCenterOrder = "";

            try
            {

                //lock (DBlock)
                //{
                    switch (Type)
                    {
                        case ProcessDataType.APC:
                            if (APC(strHGLSID) != null) funDBDelete(ProcessDataType.APC, strHGLSID);
                            clsAPC CurrentAPC = APC(strHGLSID);

                            dstrSQL = "insert into `tbAPC` (`SET_TIME`,`H_GLASSID`,`JOBID`,`RECIPE`, `APC_STATE`) VALUES ('" + CurrentAPC.SetTime.ToString("yyyy-MM-dd HH:mm:ss.ff") + "','" + CurrentAPC.GLSID + "','" + CurrentAPC.JOBID + "','" + CurrentAPC.EQPPPID + "','" + CurrentAPC.State + "');";
                            if (DBAct.clsDBAct.funExecuteQuery(dstrSQL) == false)
                            {
                                dbolDBUpdateFlag = false;
                            }

                            dintCenterIndex = CurrentAPC.ParameterName.Length / 2;
                            if ((CurrentAPC.ParameterName.Length % 2) == 0) dintCenterIndex -= 1;

                            arrCon = new ArrayList(CurrentAPC.ParameterName.Length);
                            for (int dintLoop = 0; dintLoop < CurrentAPC.ParameterName.Length; dintLoop++)
                            {
                                arrCon.Add(CurrentAPC.ParameterName[dintLoop]);
                            }

                            arrCon.Sort(new CaseInsensitiveComparer());
                            dstrCenterName = arrCon[dintCenterIndex].ToString();

                            for (int dintLoop = 0; dintLoop < CurrentAPC.ParameterName.Length; dintLoop++)
                            {
                                dstrSQL = "insert into `tbAPC_Sub` (`H_GLASSID`,`P_PARM_NAME`,`P_PARM_VALUE`,`isCenter`) VALUES ('" + CurrentAPC.GLSID + "','" + CurrentAPC.ParameterName[dintLoop] + "','" + CurrentAPC.ParameterValue[dintLoop] + "'," + ((CurrentAPC.ParameterName[dintLoop] == dstrCenterName) ? "True" : "False") + ");";
                                if (DBAct.clsDBAct.funExecuteQuery(dstrSQL) == false)
                                {
                                    dbolDBUpdateFlag = false;
                                }
                            }
                            All.APCDBUpdateCheck = true;
                            break;

                        case ProcessDataType.PPC:
                            if (PPC(strHGLSID) != null) funDBDelete(ProcessDataType.PPC, strHGLSID);
                            clsPPC CurrentPPC = PPC(strHGLSID);

                            dintCenterIndex = CurrentPPC.P_MODULEID.Length / 2;
                            if ((CurrentPPC.P_MODULEID.Length % 2) == 0) dintCenterIndex -= 1;
                            arrCon = new ArrayList(CurrentPPC.P_ORDER.Length);
                            for (int dintLoop = 0; dintLoop < CurrentPPC.P_ORDER.Length; dintLoop++)
                            {
                                arrCon.Add(CurrentPPC.P_ORDER[dintLoop]);
                            }
                            arrCon.Sort(new CaseInsensitiveComparer());
                            dstrCenterOrder = arrCon[dintCenterIndex].ToString();

                            dstrSQL = string.Format("INSERT INTO `tbPPC`(`SET_TIME`,`H_GLASSID`,`JOBID`,`isRun`) VALUES ('{0}','{1}','{2}', {3});", CurrentPPC.SetTime.ToString("yyyy-MM-dd HH:mm:ss.ff"), CurrentPPC.HGLSID, CurrentPPC.JOBID, (CurrentPPC.RunState == 2) ? true : false);
                            if (DBAct.clsDBAct.funExecuteQuery(dstrSQL) == false)
                            {
                                dbolDBUpdateFlag = false;
                            }

                            for (int dintLoop = 0; dintLoop < CurrentPPC.P_MODULEID.Length; dintLoop++)
                            {
                                dstrSQL = "insert into `tbPPC_Sub` (`P_MODULEID`,`P_ORDER`,`P_STATE`,`isCenter`,`H_GLASSID`) VALUES ('" + CurrentPPC.P_MODULEID[dintLoop] + "','" + CurrentPPC.P_ORDER[dintLoop] + "','" + CurrentPPC.P_STATUS[dintLoop] + "'," + ((dstrCenterOrder == CurrentPPC.P_ORDER[dintLoop]) ? "True" : "False") + ",'" + CurrentPPC.HGLSID + "');";
                                if (DBAct.clsDBAct.funExecuteQuery(dstrSQL))
                                {
                                    dbolDBUpdateFlag = false;
                                }
                            }
                            All.PPCDBUpdateCheck = true;
                            break;

                        case ProcessDataType.RPC:
#region  20130308 변경전
#if false
                            if (RPC(strHGLSID) != null) funDBDelete(ProcessDataType.RPC, strHGLSID);
                            clsRPC CurrentRPC = RPC(strHGLSID);

                            dstrSQL = string.Format("INSERT INTO `tbRPC`(`SET_TIME`,`H_GLASSID`,`JOBID`,`RPC_PPID`,`ORIGINAL_PPID`) VALUES ('{0}','{1}','{2}', '{3}','{4}');", CurrentRPC.SetTime.ToString("yyyy-MM-dd HH:mm:ss.ff"), CurrentRPC.HGLSID, CurrentRPC.JOBID, CurrentRPC.RPC_PPID, CurrentRPC.OriginPPID);
                            if (DBAct.clsDBAct.funExecuteQuery(dstrSQL) == false)
                            {
                                dbolDBUpdateFlag = false;
                            }
                            All.RPCDBUpdateCheck = true;
                            break;
#endif
#endregion
                            //
                            if (RPC(strHGLSID) != null) funDBDelete(ProcessDataType.RPC, strHGLSID);
                            clsRPC CurrentRPC = RPC(strHGLSID);

                            // 20130308 lsc
                            dstrSQL = string.Format("INSERT INTO `tbRPC`(`SET_TIME`,`H_GLASSID`,`JOBID`,`RPC_PPID`,`ORIGINAL_PPID`, `RPC_STATE`) VALUES ('{0}','{1}','{2}', '{3}','{4}','{5}');", CurrentRPC.SetTime.ToString("yyyy-MM-dd HH:mm:ss.ff"), CurrentRPC.HGLSID, CurrentRPC.JOBID, CurrentRPC.RPC_PPID, CurrentRPC.OriginPPID, CurrentRPC.RPC_STATE);
                            if (DBAct.clsDBAct.funExecuteQuery(dstrSQL) == false)
                            {
                                dbolDBUpdateFlag = false;
                            }
                            All.RPCDBUpdateCheck = true;
                            break;
                            
                    }
                //}
            }
            catch (Exception ex)
            {
                subLog_Set(LogType.CIM, ex.ToString());
            }

            return dbolDBUpdateFlag;
        }

        public bool subProcessDataStatusSet(ProcessDataType Type, string strHGLSID)
        {
            bool dbolResult = false;
            try
            {
                dbolResult = funDBUpdate(Type, strHGLSID);
            }
            catch (Exception ex)
            {
                subLog_Set(LogType.CIM, ex.ToString());
                dbolResult = false;
            }
            return dbolResult;
        }



        #endregion

        #region View Event Queue
        public void AddViewEvent(ViewEvent evtView)
        {
            try
            {
                pViewEventQueue.Enqueue((object)evtView);
            }
            catch (Exception ex)
            {
                this.subLog_Set(LogType.CIM, ex.ToString() + ", evtView : " + evtView.ToString());
            }
        }

        public int ViewEventCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = pViewEventQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(LogType.CIM, ex.ToString());
            }

            return dintCount;
        }

        public ViewEvent GetViewEvent()
        {
            try
            {
                return (ViewEvent)pViewEventQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(LogType.CIM, ex.ToString());
                return ViewEvent.None;
            }
        }
        #endregion

        #region "PPID Command"
        public void SetPPIDCMD(string strEQPPPID)        //PLCCommand Set
        {
            string dstrParameter = "";
            string dstrTemp = "";

            try
            {
                pPPIDCommandQueue.Enqueue(strEQPPPID);
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// PLC Comamnd Queue에서 가져와 처리한다.
        /// </summary>
        /// <returns>Write할 Command</returns>
        public object GetPPIDCMD()                        //PLCCommand Get
        {
            object dobj = null;

            try
            {
                dobj = this.pPPIDCommandQueue.Dequeue();
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dobj;
        }

        /// <summary>
        /// PLC Comamnd Queue개수를 가져온다.
        /// </summary>
        /// <returns>PLC Comamnd Queue 개수</returns>
        public int GetPPIDCMDCount()                      //PLCCommand Count 반환
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pPPIDCommandQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }

        #endregion

        #region "GLS"
        public bool AddGLS(string strGLSID)
        {
            clsGLS dclsGLS = null;     //GLS 개체 선언
            string dstrGLSID = "";
            int dintGLSIndex = 0;

            try
            {
                dstrGLSID = strGLSID.Trim();
                if (dstrGLSID == "" || dstrGLSID == null)
                {
                    return false;
                }
                else
                {
                    dclsGLS = new clsGLS(dstrGLSID);
                    if (pGLSHashTable.Contains(dstrGLSID) == false)
                    {
                        pGLSHashTable.Add(dstrGLSID, dclsGLS);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsGLS GLSID(string strGLSID)
        {
            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pGLSHashTable.Contains(dstrGLSID))
                {
                    return (clsGLS)pGLSHashTable[dstrGLSID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }
             
        public ICollection GLS()
        {
            try
            {
                return pGLSHashTable.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemoveGLSID(string strGLSID)
        {
            int dintGLSIndex = 0;

            try
            {
                string dstrGLSID = strGLSID.Trim();

                if (pGLSHashTable.Contains(dstrGLSID))
                {
                    pGLSHashTable.Remove(dstrGLSID);
                    this.subLog_Set(LogType.CIM, "RemoveGLSID, GLSID: " + dstrGLSID);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemoveGLS()
        {
            try
            {
                pGLSHashTable.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int GLSCount
        {
            get { return pGLSHashTable.Count; }
        }

        #endregion

        #region "Port"
        public bool AddPort(int  intPortID)
        {
            clsPort CurrentPort;

            try
            {
                if (intPortID <= 0)
                {
                    return false;
                }
                else
                {
                    CurrentPort = new clsPort(intPortID);

                    if (pPortHash.Contains(intPortID) == false)
                    {
                        pPortHash.Add(intPortID, CurrentPort);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public clsPort Port(int intPortID)
        {
            try
            {

                if (pPortHash.Contains(intPortID))
                {
                    return (clsPort)pPortHash[intPortID];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ICollection Port()
        {
            try
            {
                return pPortHash.Keys;
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public bool RemovePort(int intPortID)
        {
            try
            {

                if (pPortHash.Contains(intPortID))
                {
                    pPortHash.Remove(intPortID);
                    this.subLog_Set(LogType.CIM, "RemovePort, PortID: " + intPortID);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public bool RemovePort()
        {
            try
            {
                pPortHash.Clear();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
            }
        }

        public int PortCount
        {
            get { return pPortHash.Count; }
        }

        #endregion

    }
}
