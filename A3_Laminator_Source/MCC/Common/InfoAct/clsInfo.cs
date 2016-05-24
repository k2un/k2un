using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using CommonAct;
using System.Data;

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

        private Queue pSendQueue = new Queue();                     //HOST로 전송할 S/F을 저장하는 송신 Queue
        private Queue pReceiveHostQueue = new Queue();              //HOST로부터 수신된 S/F을 저장하는 수신 Queue       20071211 어경태

        private Queue pPLCWriteMethodQueue = new Queue();           //HOSTAct에서 수신한 내용에 대해 EQPAct에 작업할 수신 Queue
        private Queue pOPCallQueue = new Queue();                   //Operator Call을 할 Queue (중요한 OPCall)
        private Queue pOPCallOverWriteQueue = new Queue();          //Operator Call을 할 Queue (OverWrite가 가능한 중요하지않은 OPCall)
        private Queue pMessageQueue = new Queue();                  //Main View의 ComboBox에 Alarm, HOST Message 출력

        private Queue pPLCLogQueue = new Queue();                   //PLC Log출력 할 Queue         
        private Queue pPLCErrorQueue = new Queue();                 //PLCError Log출력 할 Queue
        private Queue pCIMQueue = new Queue();                      //CIM Log출력 할 Queue
        private Queue pAlarmQueue = new Queue();                    //Alarm Log출력 할 Queue
        private Queue pGLSInOutQueue = new Queue();                 //GLS In/Out Log출력 할 Queue
        private Queue pGLSAPDQueue = new Queue();                   //GLSAPD Log출력 할 Queue
        private Queue pLOTAPDQueue = new Queue();                   //LOTAPD Log출력 할 Queue
        private Queue pScrapUnscrapQueue = new Queue();             //Scrap, UnScrap Log출력 할 Queue
        private Queue pParameterQueue = new Queue();                //Parameter Log출력 할 Queue
        private Queue pAlarmGLSInfoQueue = new Queue();             //알람발생시 장비내 glass정보 log출력 할 Queue
        private Queue pButtonQueue = new Queue();                   //btnEVENT Log출력 할 Queue
        private Queue pSEMQueue = new Queue();                      //SEM Data Log출력 할 Queue
        private Queue pMSGQueue = new Queue();			// 20120320 이상창			//OPCallMSG Log 출력 할 Queue

        private Queue pLogQueue = new Queue();                      // 통합 로그 Queue

        /// <summary>
        /// MCC Log를 출력할 Queue 추가         //추가 : 20101001 이상호
        /// </summary>
        private Queue pMCCQueue = new Queue();

        //[2015/05/13] PPID를 저장할 HashTable(Add by HS)
        private Hashtable pDataTableHash = new Hashtable();


        #region Singleton
        public static readonly clsInfo Instance = new clsInfo();
        #endregion

        #region Constructor
        public clsInfo()
        {

        }
        #endregion

        private LogActFormShowType pLogActFormShowIndex = 0;

        public delegate void CreateEvent(string dstrParameter);     //Host로 전송할 메세지를 생성하는 SF 델리게이트 선언
        public event CreateEvent CreateSFEvent;                     //Host로 전송할 메세지를 생성하는 SF 이벤트 선언

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

        //public enum EquipmentState              //Equipment(Module) State
        //{
        //    NONE = 0,
        //    NORMAL = 1,
        //    FAULT = 2,
        //    PM = 3,
        //}

        //public enum EquipmentProcessState       //Equipment(Module) Process State
        //{
        //    NONE = 0,
        //    INIT = 1,
        //    IDLE = 2,
        //    SETUP = 3,
        //    READY = 4,
        //    EXECUTE = 5,
        //    PAUSE = 6,
        //}

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
            SerialPortOpen = 30,
            FDCDataAlramReset = 31,

            BitWrite = 96,
            BitRead = 97,
            WordWrite = 98,
            WordRead = 99,
            Test = 100,
            MSGSend = 101,
            MSGClear= 102,


            SendSetValue = 999,
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

        #endregion

        //Message 관련 Queue 작업
        #region "Message 관련 Queue 작업"

        //*******************************************************************************
        //  Function Name : funGetMessage()
        //  Description   : Message Queue에서 Message를 가져온다.
        //  Parameters    : None
        //  Return Value  : object dobj
        //  Special Notes :   
        //*******************************************************************************
        //  2007/01/16          구 정환         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : funGetMessageCount()
        //  Description   : Message Queue의 삽입항목 갯수를 가져온다.
        //  Parameters    : None
        //  Return Value  : int dintCount
        //  Special Notes :   
        //*******************************************************************************
        //  2007/01/16          구 정환         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : subMessage_Set()
        //  Description   : Message 문자열을 Queue에 넣는다.
        //  Parameters    : MsgType dMsgType, string strMessage
        //  Return Value  : None
        //  Special Notes : None
        //*******************************************************************************
        //  2007/01/16          구 정환         [L 00] 
        //*******************************************************************************
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

        //OPCall 관련 Queue 작업
        #region "OPCall"

        //*******************************************************************************
        //  Function Name : subOPCall_Set()
        //  Description   : Operator Call할 화면 Type 및 Port정보 등을 저장한다.
        //  Parameters    : intOPCall   : Operator Call할 Type
        //                  intPort     : 처리할 Port
        //                  strFromSF   : On Line모드시 발생한 Stream Function
        //                  strHostMsg  : On Line모드시 Host Message
        //  Return Value  : None
        //  Special Notes : None
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : funGetOPCall()
        //  Description   : Operator Call Queue에서 가져와 처리한다.
        //  Parameters    : None
        //  Return Value  : object => Operator Call할 화면 Type
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : funGetOPCallOverWrite()
        //  Description   : Operator Call Overwrite Queue에서 가져와 처리한다.
        //  Parameters    : None
        //  Return Value  : object => Operator Call할 화면 Type
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : funOPCallCount()
        //  Description   : Operator Call Queue개수를 가져온다.
        //  Parameters    : None
        //  Return Value  : int => Operator Call Queue 개수
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : funOPCallOverWriteCount()
        //  Description   : Operator Call Overwrite Queue개수를 가져온다.
        //  Parameters    : None
        //  Return Value  : int => Operator Call Queue 개수
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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

        //PLCCommand 관련 Queue 작업
        #region "PLCCommand"

        //*******************************************************************************
        //  Function Name : subPLCCommand_Set()
        //  Description   : PLC로 Write할 Command를 Set한다.
        //  Parameters    : intPLCCommand     : Write할 Command
        //  Return Value  : 
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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
                dstrParameter = (int)intPLCCommand + "," + dstrTemp;

                if (dstrParameter.Trim() != "")
                {
                    this.pPLCWriteMethodQueue.Enqueue(dstrParameter);
                }
                else { }
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intPLCCommand:" + intPLCCommand.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : funGetPLCCommand()
        //  Description   : PLC Comamnd Queue에서 가져와 처리한다.
        //  Parameters    : None
        //  Return Value  : object => Write할 Command
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : funGetPLCCommandCount()
        //  Description   : PLC Comamnd Queue개수를 가져온다.
        //  Parameters    : None
        //  Return Value  : int => PLC Comamnd Queue 개수
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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
        public void subLog_Set(LogType dLogType, string strDateTime, string strLog)
        {
            StringBuilder dstrWriteMsg = new StringBuilder();
            string dstrTemp = "";

            try
            {
                strLog = strLog.Replace(';', ',');

                dstrTemp = dstrTemp + strDateTime.Substring(0, 4) + "-";
                dstrTemp = dstrTemp + strDateTime.Substring(4, 2) + "-";
                dstrTemp = dstrTemp + strDateTime.Substring(6, 2) + " ";
                dstrTemp = dstrTemp + strDateTime.Substring(8, 2) + ":";
                dstrTemp = dstrTemp + strDateTime.Substring(10, 2) + ":";
                dstrTemp = dstrTemp + strDateTime.Substring(12, 2);// +".";
                //dstrTemp = dstrTemp + strDateTime.Substring(14, 3);

                dstrWriteMsg.Append("[" + dstrTemp + "]" + String.Empty.PadLeft(50, '=') + "\r\n");
                dstrWriteMsg.Append(strLog);

                this.pLogQueue.Enqueue(new clsLogData(dLogType, dstrWriteMsg.ToString()));
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dLogType:" + dLogType + ", strLog:" + strLog);
            }
        }
        public void subLog_Set(LogType dLogType, string strLog)
        {
            StringBuilder dstrWriteMsg = new StringBuilder();

            try
            {
                if (dLogType == LogType.MCCLog)
                {
                    this.pMCCQueue.Enqueue(strLog);
                }
                else
                {
                    strLog = strLog.Replace(';', ',');

                    if (dLogType == InfoAct.clsInfo.LogType.CIM || dLogType == InfoAct.clsInfo.LogType.GLSInOut || dLogType == InfoAct.clsInfo.LogType.PLCError)
                    {
                        dstrWriteMsg.Append("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]" + String.Empty.PadLeft(50, '=') + "\r\n");
                    }
                    else if (dLogType == InfoAct.clsInfo.LogType.OPCallMSG) // 20120320 이상창
                    {
                        dstrWriteMsg.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",");
                    }
                    dstrWriteMsg.Append(strLog);

                    this.pLogQueue.Enqueue(new InfoAct.clsLogData(dLogType, dstrWriteMsg.ToString()));
                }

            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dLogType:" + dLogType + ", strLog:" + strLog);
            }
        }
        #endregion

        //LOG 관련 Queue 작업
        #region "LOG"

        public clsLogData funGetLog()
        {
            clsLogData dLog = null;

            try
            {
                if (this.pLogQueue.Count > 0)
                    dLog = (clsLogData)(this.pLogQueue.Dequeue());
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dLog;
        }

        public int funGetLogCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pLogQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }

        #endregion


        #region <MCC LOG>
        public string funGetMCCLog()
        {
            string dstr = "";

            try
            {
                if (this.pMCCQueue.Count > 0)
                    dstr = Convert.ToString(this.pMCCQueue.Dequeue());
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstr;
        }

        public int funGetMCCCLogCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pMCCQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }
        #endregion

        //HOST 송신 SF 관련 Queue 작업
        #region "HOST 송신 SF"

        //*******************************************************************************
        //  Function Name : subSendSF_Set()
        //  Description   : HOST로 전송할 SF을 Set한다.
        //  Parameters    : intSFName       : HOST로 전송할 SF
        //                  objParameter    : 인자.
        //  Return Value  : 
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //  2007/04/09          김 효주         [L 00] HostAct로 이벤트를 발생시켜
        //                                              전송할 메세지를 만든다.
        //*******************************************************************************
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


        //Button LOG 관련 Queue 작업
        #region "ButtonLOG"

        //*******************************************************************************
        //  Function Name : funGetButtonLog()
        //  Description   : ButtonLog Queue에서 로그 문자열을 가져온다.
        //  Buttons    : None
        //  Return Value  : object => 출력할 Log 문자열
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
        public string funGetButtonLog()
        {
            string dstr = "";

            try
            {
                if (this.pButtonQueue.Count > 0)
                    dstr = Convert.ToString(this.pButtonQueue.Dequeue());
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstr;
        }

        //*******************************************************************************
        //  Function Name : funGetButtonLogCount()
        //  Description   : Button Queue개수를 가져온다.
        //  Buttons    : None
        //  Return Value  : int => Log Queue 개수
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
        public int funGetButtonLogCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pButtonQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }
        #endregion

        //SEM LOG 관련 Queue 작업
        #region "SEM LOG"

        //*******************************************************************************
        //  Function Name : funGetSEMLog()
        //  Description   : SEMLog Queue에서 로그 문자열을 가져온다.
        //  Buttons    : None
        //  Return Value  : object => 출력할 Log 문자열
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
        public string funGetSEMLog()
        {
            string dstr = "";

            try
            {
                if (this.pSEMQueue.Count > 0)
                    dstr = Convert.ToString(this.pSEMQueue.Dequeue());
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstr;
        }

        //*******************************************************************************
        //  Function Name : funGetSEMLogCount()
        //  Description   : SEM Queue개수를 가져온다.
        //  Buttons    : None
        //  Return Value  : int => Log Queue 개수
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
        public int funGetSEMLogCount()
        {
            int dintCount = 0;

            try
            {
                dintCount = this.pSEMQueue.Count;
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintCount;
        }
        #endregion

        //*******************************************************************************
        //  Function Name : subSendHostSF_Set()
        //  Description   : HOST로 전송할 생성된 SF Object를 구조체에 저장
        //  Parameters    : objParam: SF 개체
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/04/09          김 효주         [L 00]
        //******************************************************************************* 
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

        //*******************************************************************************
        //  Function Name : funGetSendSF()
        //  Description   : HOST로 전송할 SF을 가져와 처리한다.
        //  Parameters    : None
        //  Return Value  : object => HOST로 전송할 SF
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : funGetSendSFCount()
        //  Description   : HOST로 전송할 SF 개수를 가져온다.
        //  Parameters    : None
        //  Return Value  : int => HOST로 전송할 SF 개수
        //  Special Notes :   
        //*******************************************************************************
        //  2006/11/09          김 효주         [L 00] 
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : subReceiveHostSF_Set()
        //  Description   : HOST로 수신한 SF을 저장
        //  Parameters    : objParam: SF 개체
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/12/11          어 경태         [L 00]
        //******************************************************************************* 
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

        //*******************************************************************************
        //  Function Name : funGetSendSF()
        //  Description   : HOST로 수신한 SF을 가져와 처리한다.
        //  Parameters    : None
        //  Return Value  : object => HOST로 전송할 SF
        //  Special Notes :   
        //*******************************************************************************
        //  2007/12/11          어 경태         [L 00]
        //*******************************************************************************
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

        //*******************************************************************************
        //  Function Name : funGetSendSFCount()
        //  Description   : HOST로 전송할 SF 개수를 가져온다.
        //  Parameters    : None
        //  Return Value  : int => HOST로 전송할 SF 개수
        //  Special Notes :   
        //*******************************************************************************
        //  2007/12/11          어 경태         [L 00]
        //*******************************************************************************
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


        //LogAct 폼 Show 체크 관련
        #region "LogAct 폼 Show 체크 관련"

        //*******************************************************************************
        //  Function Name : proLogActFormShowIndex()
        //  Description   : LogAct 폼(frmAlarmList, frmGLSAPD, frmLogView, frmLOTAPD)
        //                  을 띄우기 위해 저장
        //  Parameters    : LogActFormShowType 열거형 Type
        //  Return Value  : None
        //  Special Notes : Property임
        //*******************************************************************************
        //  2006/11/17          김 효주         [L 00] 
        //*******************************************************************************
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

      
        //기타 메소드
        #region "기타 메소드"

        //*******************************************************************************
        //  Function Name : funGetModuleIDToUnitID()
        //  Description   : 각 장비(CD(A, B Type), Wet Etch & Strip, Strip)의 Unit별로
        //                  이름을 가져온다.
        //  Parameters    : intUnitCount -> 전체 Unit 개수, intUnitID -> UnitID
        //  Return Value  : Unit 이름
        //  Special Notes : 주로 DIsplay용으로 사용됨
        //*******************************************************************************
        //  2007/03/23          김 효주         [L 00] 
        //*******************************************************************************
        public int funGetModuleIDToUnitID(string strModuleID)
        {
            int dintSubUnitID = 0;

            try
            {
                //strModuleID = strModuleID.ToUpper().Trim();

                //for (int intSubUnitID = 0; intSubUnitID <= this.Unit(3).SubUnitCount; intSubUnitID++)
                //{
                //    if (this.Unit(3).SubUnit(intSubUnitID).ModuleID == strModuleID)
                //    {
                //        dintSubUnitID = intSubUnitID;
                //        break;
                //    }
                //}
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintSubUnitID;
        }

        /// <summary>
        /// A2CVD01S_CLNU_LD01 => LD01
        /// </summary>
        /// <remarks>
        /// 20101103            김중권          [L 00]
        /// </remarks>
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
        /// <remarks>
        /// 20101103            김중권          [L 00]
        /// </remarks>
        /// <param name="index"></param>
        /// <returns></returns>
        public string funGetMCCNameToActionData(int index)
        {
            string dstrMCCName = "";
            string dstrActionData = "";
            string[] dstrValue;

            try
            {
                dstrMCCName = this.Unit(0).SubUnit(0).MCC(index).MCCName;
                dstrValue = dstrMCCName.Split(new char[] { '_' });
                dstrActionData = dstrValue[dstrValue.Length - 1];
            }
            catch (Exception ex)
            {
                this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dstrActionData;
        }


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

        public int funGetEOIDItemToIndex(string strEOIDItem)
        {
            int dintIndex = 0;

            try
            {
                //tbEOID 테이블의 해당 항목에 해당하는 Index 컬럼값
                switch (strEOIDItem)
                {
                    case "VCR":
                        dintIndex = funGetEOIDItemToIndex(EOID.VCRReadingMode);
                        break;

                    case "APC":
                        dintIndex = funGetEOIDItemToIndex(EOID.APCMode);
                        break;

                    case "MCC":
                        dintIndex = funGetEOIDItemToIndex(EOID.MCCReportingMode);
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

        //public Boolean funGetEOIDLapseTime(int intIndex)
        //{
        //    Boolean dbolReturn = false;

        //    try
        //    {
        //        if (intIndex >= 4 && intIndex <= 9)
        //        {
        //            dbolReturn = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
        //    }

        //    return dbolReturn;
        //}

        //하나의(동일한) EOID에 여러개의 EOMD가 있는지 체크(HOST로 보고(S6F11)할때 사용)
        //public int funGetEOIDMultiple(int intIndex)
        //{
        //    int dintReturn = 0;

        //    try
        //    {
        //        //tbEOID테이블의 Index 4, 5, 6, 7, 8, 9에 해당하는 같은 EOID의 개수
        //        if (intIndex >= 4 && intIndex <= 9)
        //        {
        //            dintReturn = 6;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
        //    }

        //    return dintReturn;
        //}

        #endregion


        //응용프로그램 공통(All) 개체등록 및 가져오기
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

        //EQP 개체등록 및 가져오기
        #region "EQP"

        //public clsEQP EQP = new clsEQP();

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

        //Unit 개체등록 및 가져오기
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
            get { return pUnitHash.Count - 1 ; }
        }

        #endregion

        //LOT 개체등록 및 가져오기
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

        public bool RemoveLOTIndex(int dintLOTIndex)
        {
            try
            {
                if (pLOTIndexHash[dintLOTIndex] != null)
                {
                    string dstrLOTID = pLOTIndexHash[dintLOTIndex].ToString();    //LOTIndex로 LOTID를 가져온다.

                    pLOTIndexHash.Remove(dintLOTIndex); // 이상창 20120706 위치 이동

                    if (dintLOTIndex <= 0 || dintLOTIndex > 50 || dstrLOTID == "" || dstrLOTID == null)
                    {
                        return false;
                    }

                    
                    if (pLOTHash.Contains(dstrLOTID))
                    {
                        pLOTHash.Remove(dstrLOTID);     //HashTable에서 해당 LOT을 제거한다.

                        //pLOTIndexHash.Remove(dintLOTIndex);

                        this.subLog_Set(LogType.CIM, "RemoveLOTIndex, LOTID: " + dstrLOTID + ", LOTIndex: " + dintLOTIndex.ToString());

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

        //[2015/05/13]DataTable(Add by HS)
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
    }
}
