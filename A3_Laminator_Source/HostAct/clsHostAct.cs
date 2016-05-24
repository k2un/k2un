using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Timers;
using System.Threading;
using System.Globalization;
using Microsoft.VisualBasic;
using NSECS;
using CommonAct;
using InfoAct;

namespace HostAct
{
    public class clsHostAct
    {
        #region "선언"

        public string Version
        {
            get { return "SMD A2 HCACLN V1.0"; }
        }

        public clsNSecsW PSecsDrv;

        //private Boolean pbolReplyCheck = false;         //Reply가 들어왔는지 Check한다

        public clsInfo PInfo = clsInfo.Instance;                   //외부에서 여기서 사용할 구조체를 넣어줌

        private System.Threading.Thread pThreadSFSend = null;  //구조체에 있는 HOST로 송신할 S/F을 검사하여 HOST 송신 자체 Queue에 넣기위한 Thread 정의
        private System.Threading.Thread pThreadSFReceive = null;  //구조체에 있는 HOST로 수신한 S/F을 검사하여 HOST 송신 자체 Queue에 넣기위한 Thread 정의

        private System.Timers.Timer SVIDSendThread;                 //PLC로 부터 SVID값을 읽기위한 Timer 정의
        private System.Timers.Timer XPCoverride;
        short dshtReturn = 0;
        
        //2012.07.12 Kim Youngsik
        private clsSFManager pHsmsMessages = clsSFManager.Instance;


        #endregion

        //부모폼에서의 호출메소드(Open and Close)
        #region"Open and Close"

        /// <summary>
        /// HostAct DLL의 인스턴스를 생성시키고 오픈한다.
        /// </summary>
        /// <param name="strEQPID"> SECS에서 쓰이는 EQPID </param>
        /// <returns> 성공 => true, 실패 => false </returns>
        public int funOpenSecsDrv(string strEQPID)
        {
            int dintReturn = 0;
            int dintMillisecond = 0;

            try
            {
                //HOST로 S6F1을 송신하는 System Thread의 시작시간을 결정하여 실행시킨다.
                do
                {
                    dintMillisecond = DateTime.Now.Millisecond;
                    System.Diagnostics.Debug.WriteLine(dintMillisecond);
                    if (dintMillisecond <= 100)
                    {
                        //SVIDSendThread Timer 설정
                        this.SVIDSendThread = new System.Timers.Timer();
                        this.SVIDSendThread.Elapsed += new ElapsedEventHandler(SVIDSend_Tick);
                        this.SVIDSendThread.Interval = 1000;            //1000ms
                        this.SVIDSendThread.Enabled = true;
                        GC.KeepAlive(this.SVIDSendThread);

                        // 20120927
                        this.XPCoverride = new System.Timers.Timer();
                        this.XPCoverride.Elapsed += new ElapsedEventHandler(XPCoverride_Tick);
                        this.XPCoverride.Interval = 500;
                        this.XPCoverride.Enabled = true;
                        GC.KeepAlive(this.XPCoverride);

                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SVIDSend_Tick(시작 Millisecond): " + dintMillisecond);      //로그출력

                        break;
                    }
                } while (true);


                PSecsDrv = new clsNSecsW();

                PSecsDrv.OnReceiveEvent += new NSecsEventHandler(OnReceiveEvent);
                PSecsDrv.OnReceiveMessage += new NSecsMsgHandler(OnReceiveMsg);

                dintReturn = Convert.ToInt32(this.PSecsDrv.Initialize(this.PInfo.All.CfgFilePath));

                //리턴값이 0이면 정상
                if (dintReturn == 0)
                {
                    dintReturn = Convert.ToInt32(this.PSecsDrv.Start());    //Start 한다.

                    if (dintReturn == 0)
                    {
                        this.PInfo.All.HostDriver = true;
                        this.pHsmsMessages.funSetHostAct(this);
                        //Host로 전송할 메세지를 생성하는 SF 이벤트 등록
                        this.PInfo.CreateSFEvent += new InfoAct.clsInfo.CreateEvent(subCreateSF);

                    }
                    else
                    {
                        this.PInfo.All.HostDriver = false;
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "funOpenSecsDrv, HOST Driver Open Error. dintReturn: " + dintReturn.ToString());   //Open이 되지 않아 에러가 발생한 경우
                    }
                }
                else
                {
                    this.PInfo.All.HostDriver = false;
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "funOpenSecsDrv, HOST Driver Open Error. dintReturn: " + dintReturn.ToString());   //Open이 되지 않아 에러가 발생한 경우
                }



                #region "MCC"

                this.PInfo.funMCCprocessStart();

                this.pMCCmsgThread = new Thread(new ThreadStart(subReadMCCStdOut));
                this.pMCCmsgThread.Name = "ReadMCCStdOut";
                this.pMCCmsgThread.IsBackground = true;
                this.pMCCmsgThread.Start();

                #endregion

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintReturn;
        }

        private System.Threading.Thread pMCCmsgThread;


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

                    if (temp.Length == 1)
                    {
                        if (temp[0] == "GET" || temp[0] == "ERR")
                        {
                            this.mType = temp[0];
                            this.mMsg = string.Empty;
                            result = true;
                        }
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

        private void subReadMCCStdOut()
        {
            do
            {
                try
                {
                    Thread.Sleep(50);

                    if (this.PInfo.All.MCCprocess == null) continue;

                        
                    string dstrReadLine = this.PInfo.All.MCCprocess.StandardOutput.ReadLine();

                    if (!string.IsNullOrEmpty(dstrReadLine))
                    {
                        ReceiveMCCEventArg arg = new ReceiveMCCEventArg();

                        if (arg.SetMessage(dstrReadLine)) subReceiveMCCmessage(arg);
                    }
                }
                catch (Exception ex)
                {
                   //this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
                }
            }
            while (true);
        }

        private void subReceiveMCCmessage(ReceiveMCCEventArg arg)
        {
            try
            {
                switch (arg.MessageType)
                {
                    case "ERR":
                        this.PInfo.subSendSF_Set(clsInfo.SFName.S6F11EquipmentSpecifiedNetworkEvent, 171, this.PInfo.Unit(0).SubUnit(0).MultiData.TYPES("MCC").ITEMS("MCC_HOST_IP").VALUE);//this.PInfo.All.MCCNetworkPath);
                        break;

                    case "SET":
                        string[] dstrTemp = arg.Message.Split(',');
                        

                        // 1:MCC_BASIC_PATH 
                        // 2:MCC_SAMPLING_TIME
                        // 3:MCC_HOST_IP
                        // 4:MCC_LOGIN_ID
                        // 5:MCC_LOGIN_PW



                        foreach (InfoAct.clsMultiUseDataByITEM clsITEM in this.PInfo.Unit(0).SubUnit(0).MultiData.TYPES("MCC").ITEMS_VALUE())
                        {
                            if (clsITEM.INDEX > 0 && clsITEM.INDEX < dstrTemp.Length + 1)
                            {
                                clsITEM.VALUE = dstrTemp[clsITEM.INDEX - 1];
                            }
                        }

                        //this.PInfo.All.MCCNetworkPath = dstrTemp[0];          // ip
                        //this.PInfo.All.MCCNetworkBasicPath = dstrTemp[1];     // path
                        //this.PInfo.All.MCCFileUploadTime = Convert.ToInt32(dstrTemp[2]);       // sampling timeI
                        //this.PInfo.All.MCCNetworkUserID = dstrTemp[3];        // loginID
                        //this.PInfo.All.MCCNetworkPassword = dstrTemp[4];     // password

                        break;
                    case "GET":
                        System.Diagnostics.Debug.WriteLine(arg.Message);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }



        /// <summary>
        /// Secs Driver를 Close 시킨다.
        /// </summary>
        /// <returns> int => 0(정상) </returns>
        public int funSecsDrvClose()
        {
            int dintReturn = 0;

            try
            {
                this.PInfo.All.HostConnect = false;        //HOST와 연결해제되었음
                //dintReturn = Convert.ToInt32(this.PSecsDrv.terminatePlugIn());
                dintReturn = Convert.ToInt32(this.PSecsDrv.Stop());

                //Thread 종료
                if (this.pThreadSFSend != null)
                {
                    this.pThreadSFSend.Abort();
                }

                if (this.pThreadSFReceive != null)
                {
                    this.pThreadSFReceive.Abort();
                }

                if (this.pMCCmsgThread != null) this.pMCCmsgThread.Abort();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            return dintReturn;
        }

        #endregion

        //Secs Drive로부터 이벤트를 수신한다.
        #region "이벤트 수신"

        //이벤트 핸들러 등록
        #region"이벤트 핸들러(Event Handler) 등록"

        public void OnReceiveEvent(int evtcode, int lParam)
        {
            string dstrLog = "";

            switch (evtcode)
            {
                case (int)NEVENT.STATE_NOT_CONNECTED:        //검증 OK
                    //strLog += "NOT CONNECTED";
                    PSecsDrv_SECSDisconnected();    //함수 호출
                    break;
                case (int)NEVENT.STATE_CONNECTED:            //검증 OK
                    //strLog += "CONNECTED";
                    break;
                case (int)NEVENT.STATE_NOT_SELECTED:         //검증 OK
                    //strLog += "NOT SELECTED";
                    //PSecsDrv_SECSDisconnected();    //함수 호출
                    break;
                case (int)NEVENT.STATE_SELECTED:             //검증 OK
                    //strLog += "SELECTED";
                    PSecsDrv_SECSConnected();   //함수 호출
                    break;
                case (int)NEVENT.TIMEOUT_T1:
                    //strLog += "T1 Timeout!";
                    break;
                case (int)NEVENT.TIMEOUT_T2:
                    //strLog += "T2 Timeout!";
                    break;
                case (int)NEVENT.TIMEOUT_T3:                 //검증 OK
                    //strLog += "T3 Timeout!";
                    PSecsDrv_SECSTimeout(lParam);     //함수 호출
                    break;
                case (int)NEVENT.TIMEOUT_T4:
                    //strLog += "T4 Timeout!";
                    break;
                case (int)NEVENT.TIMEOUT_T5:             //검증 OK
                    //strLog += "T5 Timeout!";
                    break;
                case (int)NEVENT.TIMEOUT_T6:
                    //strLog += "T6 Timeout!";
                    break;
                case (int)NEVENT.TIMEOUT_T7:
                    //strLog += "T7 Timeout!";
                    break;
                case (int)NEVENT.TIMEOUT_T8:
                    //strLog += "T8 Timeout!";
                    break;
                case (int)NEVENT.ERR_INVALID_MSG:                //검증 OK
                    //strLog += "Illegal Data : S9F7";
                    PSecsDrv_SECSInvalidMessage(lParam);  //함수 호출
                    break;
                case (int)NEVENT.ERR_UNKNOWN_DEVICEID:           //검증 OK
                    //strLog += "Unknown Deviceid : S9F1";
                    break;
                case (int)NEVENT.ERR_UNKNOWN_FUNCTION:           //검증 OK
                    //strLog += "Unknown Function : S9F5";
                    PSecsDrv_SECSUnknownMessage(); //함수 호출
                    break;
                case (int)NEVENT.ERR_UNKNOWN_STREAM:             //검증 OK
                    //strLog += "Unknown Stream : S9F3";
                    PSecsDrv_SECSUnknownMessage(); //함수 호출
                    break;
                case (int)NEVENT.ERR_ABORT_TRANSACTION:
                    //strLog += "ERR_ABORT_TRANSACTION";
                    break;
                case (int)NEVENT.ERR_INITIALIZE:
                    //strLog += "ERR_INITIALIZE";
                    break;
                case (int)NEVENT.ERR_RETRY_LIMIT:
                    //strLog += "ERR_RETRY_LIMIT";
                    break;
                case (int)NEVENT.ERR_START:
                    //strLog += "ERR_START";
                    break;
                case (int)NEVENT.R2KEY_DETECTED:
                    dstrLog += "R2KEY_DETECTED";
                    //this.PInfo.All.LicenseType = true;
                    break;
                case (int)NEVENT.R2KEY_DLL_NOT_FOUND:
                    dstrLog += "R2KEY_DLL_NOT_FOUND";
                    //this.PInfo.All.LicenseType = false;
                    break;
                case (int)NEVENT.R2KEY_NOT_DETECTED:
                    dstrLog += "R2KEY_NOT_DETECTED";
                    //this.PInfo.All.LicenseType = false;
                    break;
                case (int)NEVENT.R2KEY_TIMEOUT:
                    dstrLog += "R2KEY_TIMEOUT";
                    //this.PInfo.All.LicenseType = false;
                    break;
                default:
                    dstrLog += "default";
                    break;
            }

            if (dstrLog != "") this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrLog);

            //System.Diagnostics.Debug.WriteLine(this.PSecsDrv.IsLicense);
        }

        private void OnReceiveMsg(int msgcode, Transaction msgTran)
        {
            string dstrSF = "";

            try
            {
                switch (msgcode)
                {
                    case (int)NMSG.PRIMARY:

                        dstrSF = "S" + msgTran.Primary().Stream.ToString() + "F" + msgTran.Primary().Function.ToString();
                        break;

                    case (int)NMSG.SECONDARY:

                        dstrSF = "S" + msgTran.Secondary().Stream.ToString() + "F" + msgTran.Secondary().Function.ToString();
                        break;
                }

                this.PInfo.subReceiveHostSF_Set(msgTran);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region"SEComEnabler에서 발생하는 기본 Event"
        /// <summary>
        /// Host와 SECS Driver가 Connect 되었을 때 발생
        /// </summary>
        private void PSecsDrv_SECSConnected()
        {
            try
            {
                this.PInfo.All.HostConnect = true;        //HOST와 연결되었음
                
                //이전상태 기억 못하는 경우(프로그램 최초 기동시)나 Offline에서 통신이끊어지고 다시 통신이 붙으면 Online Remote로 전환한다.
                if (PInfo.All.ControlState == "1")
                {
                    PInfo.All.ControlstateChangeBYWHO = "3";    //By Equipment

                    PInfo.All.ControlStateOLD = PInfo.All.ControlState;     //현재의 ControlState를 백업
                    PInfo.All.WantControlState = "3";

                    PInfo.All.ONLINEModeChange = true;
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);
                }
                else
                {
                    PInfo.All.ControlstateChangeBYWHO = "3";    //By Equipment

                    PInfo.All.WantControlState = PInfo.All.ControlStateOLD;
                    PInfo.All.ControlStateOLD = PInfo.All.ControlState;     //현재의 ControlState를 백업
                    PInfo.All.ControlState = PInfo.All.WantControlState;

                    PInfo.All.ONLINEModeChange = true;
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Host와 SECS Driver가 Disconnect 되었을 때 발생
        /// </summary>
        private void PSecsDrv_SECSDisconnected()
        {
            try
            {
                this.PInfo.All.HostConnect = false;         //HOST와 연결해제되었음

                //현재의 ControlState를 저장해놓는다.
                this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;

                //HOST와 연결이 끊어지면 현재 걸려있는 Trace를 모두 지운다.(080119)
                //this.PInfo.Unit(0).SubUnit(0).RemoveTRID();


                //this.PInfo.All.EQPMode = "0";               //Offline임을 저장

                //HOST와 연결이 끊어지면 현재 상태를 유지하고 OP Call창을 띄워 알린다.
                if (this.PInfo.All.ControlState != "1")
                {
                    //this.PInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGBuzzer, 0, "", "HOST Disconnected !!!");
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.HOSTDisconnect);     //HOST와 연결이 끊어졌음을 PLC에게 알린다.
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// T3 시간 내에 Reply가 오지 않은 경우 발생
        /// </summary>
        /// <param name="intMsgID"> Timeout이 발생한 Message의 ID </param>
        private void PSecsDrv_SECSTimeout(int intMsgID)
        {
            //string dstrMessage = "";
            string dstrSF = "";
            short dintDeviceID = 0;
            short dintStream = 0;
            short dintFunction = 0;
            int dintSystemByte = 0;
            short dshtWaitBit = 0;
            //int dintMsgID = 0;
            int dintReturn = 0;

            try
            {
                //subReplyTrue();     //Reply가 들어왔음을 저장

                dintReturn = this.PSecsDrv.GetAlarmMsgInfo(intMsgID, out dintDeviceID, out dintStream, out dintFunction, out dintSystemByte, out dshtWaitBit);
                if (dintReturn == 0)
                {
                    dstrSF = "S" + dintStream.ToString() + "F" + dintFunction.ToString();
                    if (dstrSF != "S0F0")
                    {
                        if (this.PInfo.All.ONLINEModeChange == true)
                        {
                            this.PInfo.All.ONLINEModeChange = false;        //초기화
                            this.PInfo.All.WantControlState = "";
                        }
                        else
                        {
                        }

                        //Online Monotor <-> Control 전환시 T3 Timeout이 발생하면 Mode Change 창을 숨긴다.
                        if (this.PInfo.All.ModeChangeFormVisible == true)
                        {
                            this.PInfo.subOPCall_Set(InfoAct.clsInfo.OPCall.FormClose, 0, "", "");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Invalid Message를 수신한 경우 발생
        /// </summary>
        /// <param name="intMsgID"> Invalid Message ID </param>
        private void PSecsDrv_SECSInvalidMessage(int intMsgID)
        {
            try
            {
                //subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Abort Message를 수신한 경우 발생
        /// </summary>
        /// <param name="intMsgID"> Invalid Message ID </param>
        private void PSecsDrv_SECSAbortedMessage()
        {
            try
            {
                //subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// SECS Driver Error 발생 시
        /// </summary>
        /// <param name="theErrorCode"> Error Code </param>
        /// <param name="theErrorMsg"> Error Message </param>
        void PSecsDrv_SEComDrvError(short theErrorCode, string theErrorMsg)
        {
            try
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PSecsDrv_HostDrvError(), The method or operation is not implemented.");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// SECS Driver Shutdown 시 발생
        /// </summary>
        /// <param name="theErrorCode"> Error Code </param>
        /// <param name="theErrorMsg"> Error Message </param>
        void PSecsDrv_SEComDrvShutdown(short theErrorCode, string theErrorMsg)
        {
            try
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PSecsDrv_HostDrvShutdown()");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Unknown Message 수신시 발생
        /// </summary>
        private void PSecsDrv_SECSUnknownMessage()
        {
            try
            {
                //subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

        }
        #endregion

        #endregion

        #region "Timer, Thread 정의"
        /// <summary>
        /// Thread Initialize
        /// </summary>
        /// <returns></returns>
        public Boolean funThreadInitial()
        {
            try
            {
                this.pThreadSFSend = new Thread(new ThreadStart(SFSendThread));
                this.pThreadSFSend.Name = "SFSendThread";
                this.pThreadSFSend.IsBackground = true;
                this.pThreadSFSend.Start();

                this.pThreadSFReceive = new Thread(new ThreadStart(SFReceiveThread));
                this.pThreadSFReceive.Name = "SFReceiveThread";
                this.pThreadSFReceive.IsBackground = true;
                this.pThreadSFReceive.Start();


                return true;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Host로 전송할 메세지가 있으면 메세지를 송신한다.
        /// </summary>
        public void SFSendThread()
        {
            object dobjData;

            do
            {
                try
                {
                    try
                    {

                        if (this.PInfo.funGetSendSFCount() > 0)               //구조체의 S/F 송신 Queue에 내용이 없으면 그냥 나간다.
                        {
                            dobjData = this.PInfo.funGetSendSF();             //Queue에서 HOST로 송신할 S/F을 가져온다.
                            if (dobjData != null)
                            {
                                subSendMessage(dobjData);    //HOST로 메세지 송신
                            }
                        }
                        Thread.Sleep(30);  //50
                    }
                    catch (ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    }
                }
                catch
                {
                }

            } while (true);
        }

        /// <summary>
        /// Host로 부터 수신한 메세지를 처리한다.
        /// </summary>
        public void SFReceiveThread()
        {
            object dobjData;

            do
            {
                try
                {
                    try
                    {
                        //20071211 eogaiver 추가
                        if (this.PInfo.funGetReceiveSFCount() > 0)               //구조체의 S/F 수신 Queue에 내용이 없으면 그냥 나간다.
                        {
                            dobjData = this.PInfo.funGetReceiveSF();             //Queue에서 HOST로 수신한 S/F을 가져온다.
                            if (dobjData != null)
                            {
                                subReceiveMessage((Transaction)dobjData);
                            }
                        }

                        Thread.Sleep(30);
                    }
                    catch (ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    }
                }
                catch
                {
                }

            } while (true);
        }


        /// <summary>
        /// Host로 전송할 SF Object를 생성해서 구조체에 저장
        /// </summary>
        /// <param name="dstrParameter"> parameters </param>
        private void subCreateSF(string dstrParameter)
        {
            object dobjSF = null;
            string[] dstrValue = dstrParameter.Split(new char[] { ',' });

            try
            {
                dobjSF = funCreateSFInstance(dstrParameter);

                if (Convert.ToInt32(dstrValue[0]) == (int)InfoAct.clsInfo.SFName.S6F1TraceDataSend)
                {
                    if (dobjSF != null) subSendMessage(dobjSF);    //S6F1은 HOST로 바로 메세지 전송
                }
                else
                {
                    if (dobjSF != null) this.PInfo.subSendHostSF_Set(dobjSF);   //S6F1외는 Queue에 넣은 후 전송
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Host로 메세지를 송신한다.
        /// Primary만 여기에서 전송하고, Secondary는 Host로 부터 Primary가 왔을 때
        /// 해당 Class에서 전송한다.
        /// </summary>
        /// <param name="objSF"> S/F Object </param>
        public void subSendMessage(object objSF)
        {
            Transaction dMsgTran = null;
            //int dintReturn = 0;
            string dstrSF = "";
            //Boolean dbolReplyCheck = false;

            try
            {
                //dstrSF = objSF.GetType().Name;
                dMsgTran = (Transaction)objSF;
                dstrSF = string.Format("S{0}F{1}", dMsgTran.Primary().Stream, dMsgTran.Primary().Function);

                switch (dstrSF)
                {
                    case "S1F1":
                        if (this.PInfo.All.HostConnect == false) { return; }
                        //dbolReplyCheck = true;
                        break;

                    case "S5F1":
                        if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;
                        //dbolReplyCheck = true;
                        break;

                    case "S6F1":
                        if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;
                        break;

                    case "S6F11":
                        //CEID=71 Offline 전환 보고는 Offline이라도 가능하게 한다.
                        if (dMsgTran.Primary().Item("CEID").Getvalue().ToString() == "71")
                        {
                            if (this.PInfo.All.HostConnect == false) { return; }
                        }
                        else
                        {
                            if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;
                        }
                        //dbolReplyCheck = true;
                        break;

                    case "S6F13":
                        if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;
                        //dbolReplyCheck = true;
                        break;

                    case "S7F107":
                        if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;
                        //dbolReplyCheck = true;
                        break;

                    default:
                        break;
                }

                dshtReturn = Convert.ToInt16(dMsgTran.Send());

                if (dshtReturn < 0)
                {
                    string logText = string.Format("Message S{0}F{1} Send Fail. Return({2})", dMsgTran.Secondary().Stream.ToString(),
                                                                                                dMsgTran.Secondary().Function.ToString(),
                                                                                                dshtReturn.ToString());
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, logText);
                }
            }
            catch
            {
                //스레드 종료시 네이티브프레임 호출에러때문에 로그 주석처리
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Host로 부터 수신한 Message를 처리한다.
        /// </summary>
        /// <param name="msgTran"> 수신한 Transaction </param>
        public void subReceiveMessage(Transaction msgTran)
        {
            try
            {
                if (this.PInfo.All.HostConnect == false) return;

                if (msgTran.IsPrimary)
                {
                    if (funACTSECSAort_Send(msgTran) == true) return;
                    pHsmsMessages.funPrimaryReceive(msgTran);
                }
                else
                {
                    pHsmsMessages.funSencondaryReceive(msgTran);
                }
            }
            catch
            {
                //스레드 종료시 네이티브프레임 호출에러때문에 로그 주석처리
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// S/F의 인스턴스를 생성하여 Host 자체 Queue에 저장한다.
        /// </summary>
        /// <param name="strParameter"> S/F 이름</param>
        /// <returns> 생성한 S/F의 Instance Object </returns>
        public object funCreateSFInstance(string strParameter)
        {
            string[] darrEvent;
            Transaction dMsgTran = null;

            try
            {
                darrEvent = strParameter.Split(',');     //SF 이름과 인자들을 분리한다.

                switch (Convert.ToInt32(darrEvent[0]))
                {
                    case (int)InfoAct.clsInfo.SFName.S1F1AreYouThereRequest:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("AreYouThere", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S5F1AlarmReportsend:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S5F1", strParameter);
                        }
                        break;

                    //CEID = 51(EQ(MODULE) PROCESS STATE CHANGED)
                    //CEID = 53(EQ(MODULE) STATE CHANGED)
                    //CEID = 71(CHANGE TO OFF_LINE MODE)
                    //CEID = 72(CHANGE TO ON_LINE LOCAL MODE)
                    //CEID = 73(CHANGE TO ON_LINE REMOTE MODE)
                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F11RelatedEQPEvent", strParameter);
                        }
                        break;

                    //CEID = 10 GLASS ID READ Report : after reading GLASSID with VCR
                    //CEID = 13 GLASS PROCESS ABORT : A glass job is aborted at a single glass job controllable Eqp
                    //CEID = 16 GLASS MODULE IN : Glass moves in to the module
                    //CEID = 17 GLASS MODULE OUT : Glass moves out from the module
                    //CEID = 18 GLASS SCRAP : Deletion of glass data
                    //CEID = 19 GLASS UNSCRAP : Making glass data in EQP
                    //CEID = 21 PROCESS STEP START : Process and process step started in a unit
                    //CEID = 22 PROCESS STEP CHANGE : Process step changed
                    //CEID = 23 PROCESS STEP END : Process and process step ended in a unit

                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F11RelatedGlassProcess", strParameter);
                        }
                        break;

                    //CEID = 101(PARAMETER CHANGED (EOID))
                    //CEID = 102(PARAMETER CHANGED (ECID))
                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedEquipmentParameterEvent:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F11RelatedEQPParameter", strParameter);
                        }
                        break;

                    //CEID = 131(Equipment Current PPID Change Event)
                    case (int)InfoAct.clsInfo.SFName.S6F11EquipmentSpecifiedControlEvent:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F11EQPSpecifiedCtrl", strParameter);
                        }
                        break;

                    //CEID = 171(NETWORK SERVER NOT CONNECTED)
                    case (int)InfoAct.clsInfo.SFName.S6F11EquipmentSpecifiedNetworkEvent:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F11EQPSpecifiedNetwork", strParameter);
                        }
                        break;

                    //A2 사양에서는 기존의 "Related PanelID Validation Event"에 해당하였던 부분이 "Related JOB Process Event"로 변경되었다.
                    //이에 따라 기존의 S6F11RelatedGlassIDValidationEvent를 주석처리 하고 Related JOB Process Event에 맞추어 제작한다. 20101018 어우수
                    //CEID = 111 GLASSID READ FAIL Fail to read Glass_id
                    //CEID = 112 H_GLASSID SEARCH FAIL Fail to search the job's "Host Glass_ID" for "read Glass_id" - 사용안함
                    //CEID = 113 GLASSID MISMATCH "read Glass_id"is different from "host Glass_id"
                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedJobProcess:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F11RelatedJobProcess", strParameter);
                        }
                        break;

                    //Mode(1: 생성, 2: 삭제, 3: 수정)
                    case (int)InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S7F107", strParameter);
                        }
                        break;

                    //CEID = 81 Glass Data Collection for Normal Equipment
                    case (int)InfoAct.clsInfo.SFName.S6F13GLSAPD:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F13GLSAPD", strParameter);
                        }
                        break;

                    //CEID = 91(LOT Base)
                    case (int)InfoAct.clsInfo.SFName.S6F13LOTAPD:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F13LOTAPD", strParameter);
                        }
                        break;

                    //CEID = 82/92 Glass/Lot Data Collection For Equipment Using File Server (Inspection or Measurement)
                    case (int)InfoAct.clsInfo.SFName.S6F13GLSAPDMea:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F13GLSAPDMea", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S16F115APCDataCMD:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S16F115", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S16F111APCStart:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S16F111", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S16F113APCEnd:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S16F113", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S16F15PPCDataCMD:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S16F15", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S16F11PPCRunning:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S16F11", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S16F135RPCDataCMD:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S16F135", strParameter);
                        }
                        break;
                    case (int)InfoAct.clsInfo.SFName.S16F131RPCStart:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S16F131", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S16F133RPCEnd:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S16F133", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedProcessStatusEvent:
                        {
                            dMsgTran = pHsmsMessages.funPrimarySend("S6F11RelatedProcessStatusEvent", strParameter);
                        }
                        break;

                    case (int) InfoAct.clsInfo.SFName.S6F11MaterialLoadEvent:
                        dMsgTran = pHsmsMessages.funPrimarySend("S6F11MaterialLoadEvent", strParameter);
                        break;

                    case (int)InfoAct.clsInfo.SFName.S6F11MaterialProcEvent:
                        dMsgTran = pHsmsMessages.funPrimarySend("S6F11MaterialProcEvent", strParameter);
                        break;

                        

                    #region <Trace Data 변경 전 - 기존꺼 사용>
                    // Trace Data 의 앞에 글라스 정보를 붙이며, 1장에 대한 글라스 정보가 추가 된다.
                    //S6F1 Trace
                    case (int)InfoAct.clsInfo.SFName.S6F1TraceDataSend:
                        {

                            dMsgTran = pHsmsMessages.funPrimarySend("S6F1", strParameter);
                        }
                        break;

                    case (int)InfoAct.clsInfo.SFName.S6F11MaterialStockEvent:
                        dMsgTran = pHsmsMessages.funPrimarySend("S6F11MaterialStockEvent", strParameter);
                        break;


                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedPortEvent:
                        dMsgTran = pHsmsMessages.funPrimarySend("S6F11RelatedPortEvent", strParameter);
                        break;

                    case (int)InfoAct.clsInfo.SFName.S6F11CSTMovingEvent:
                        dMsgTran = pHsmsMessages.funPrimarySend("S6F11CSTMovingEvent", strParameter);
                        break;

                    //CEID 1025 추가 - 150122 고석현
                    case (int)InfoAct.clsInfo.SFName.S6F11MaterialProcessStatusEvent:
                        dMsgTran = pHsmsMessages.funPrimarySend("S6F11MaterialProcessStatusEvent", strParameter);
                        break;

                    #endregion

                    default:
                        return null;
                }

                if (dMsgTran == null)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "MakeSecsMsg failed: " + darrEvent[0]);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return null;
            }

            return dMsgTran;
        }
        private void XPCoverride_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.XPCoverride.Enabled = false;

                do
                {
                    clsXPCoverride xpc = (clsXPCoverride)PInfo.funPeekXPCOverride();

                    if (xpc == null) return;

                    if (xpc.SetTime <= DateTime.Now)
                    {
                        xpc = (clsXPCoverride)PInfo.funGetXPCOverride();
                        PInfo.subSendSF_Set(xpc.SFname, 1, xpc.GlassID);
                    }
                }
                while (PInfo.funGetXPCOverrideCount() > 0);
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //if (this.XPCoverride != null && this.XPCoverride.Enabled == false) 
                this.XPCoverride.Enabled = true;
            }
        }

        /// <summary>
        /// Trace를 체크하여 있으면 Host로 보고한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SVIDSend_Tick(object sender, ElapsedEventArgs e)
        {
            int dintGroupCnt = 0;
            int dintTempTRID = 0;
            string[] dstrValue;
            string dstrData = "";
            //string dstrMilliSecond = "";

            try
            {
                //Trace가 하나도 걸려있지 않으면 빠져나간다.
                if (this.PInfo.Unit(0).SubUnit(0).TRIDCount == 0) return;

                foreach (int dintTRID in this.PInfo.Unit(0).SubUnit(0).TRID())
                {
                    dintTempTRID = dintTRID;

                    if (Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).DSPER) > 0)
                    {
                        this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc += 1;  //횟수 누적
                        //this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc += 0.2;  //횟수 누적

                        if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc >= Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).DSPER))
                        {
                            this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).GrpCnt += 1;
                            dintGroupCnt = this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).GrpCnt;
                            this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc = 0;

                            if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintGroupCnt) != null)
                            {
                                //dstrMilliSecond = DateTime.Now.Millisecond.ToString().Substring(0,2);
                                //MilliSecond는 00으로 - 입고전검수 20101209 KJK
                                this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintGroupCnt).ReadTime = DateTime.Now.ToString("yyyyMMddHHmmss") + "00";
                            }

                            if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ == 1)
                            {
                                //SampleNo는 1~65535이므로 65536이상이 되면 0으로 Reset
                                if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo >= 65535) this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo = 0;

                                this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo += 1;

                                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(SVIDSend_Tick): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                                //HOST로 보고(TRID를 Key)
                                this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F1TraceDataSend, dintTRID);
                                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(SVIDSend_Tick): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());


                                this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc = 0;
                                this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).GrpCnt = 0;
                            }
                            else if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ > 1)
                            {
                                //보고는 안한다.
                                this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZCnt += 1;

                                if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZCnt >= this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ)
                                {
                                    //SampleNo는 1~65535이므로 65536이상이 되면 0으로 Reset
                                    if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo >= 65535) this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo = 0;

                                    this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo += 1;

                                    //HOST로 보고(TRID를 Key)
                                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F1TraceDataSend, dintTRID);

                                    this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZCnt = 0;
                                    this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc = 0;
                                    this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).GrpCnt = 0;

                                }
                            }
                        }
                    }

                    //TOTSMP만큼 보냈으면 더이상 보내지 않는 것을 Check해서 저장
                    if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TOTSMP != 0)
                    {
                        if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo >= this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TOTSMP)
                        {
                            dstrData = dstrData + dintTRID.ToString() + ";";
                        }
                    }
                    else
                    {
                        dstrData = dstrData + dintTRID.ToString() + ";";
                    }
                }

                //TOTSMP만큼 보냈으면 더이상 보내지 않는 것을 삭제한다.
                if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTempTRID).TOTSMP != 0)
                {
                    dstrValue = dstrData.Split(new char[] { ';' });
                    for (int dintLoop = 1; dintLoop <= dstrValue.Length - 1; dintLoop++)
                    {
                        this.PInfo.Unit(0).SubUnit(0).RemoveTRID(Convert.ToInt32(dstrValue[dintLoop - 1]));
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
            }
        }

        #endregion

        #region "PLC로 명령을 주고 모두 읽을때까지 대기하는 함수"
        /// <summary>
        /// PLC로 Command를 주고 모두 읽을 때 까지 대기하는 함수
        /// </summary>
        public void subWaitDuringReadFromPLC()
        {
            long dlngTimeTick = 0;
            long dlngSec = 0;
            int dintTimeOut = 40;   //TimeOut은 20초로 함

            try
            {
                this.PInfo.All.PLCActionEnd = false;        //초기화
                dlngTimeTick = DateTime.Now.Ticks;

                
                while (this.PInfo.All.PLCActionEnd == false)
                {
                    dlngSec = DateTime.Now.Ticks - dlngTimeTick;
                    if (dlngSec < 0) dlngTimeTick = 0;
                    if (dlngSec > dintTimeOut * 10000000 || this.PInfo.All.PLCActionEnd == true)
                    {
                        this.PInfo.All.PLCActionEnd = false;        //초기화
                        return;
                    }

                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subWaitPPIDSearch()
        {
            long dlngTimeTick = 0;
            long dlngSec = 0;
            int dintTimeOut = 30;   //TimeOut은 20초로 함

            try
            {
                dlngTimeTick = DateTime.Now.Ticks;


                while (this.PInfo.All.PPIDSearchFlag == false)
                {
                    dlngSec = DateTime.Now.Ticks - dlngTimeTick;
                    if (dlngSec < 0) dlngTimeTick = 0;
                    if (dlngSec > dintTimeOut * 10000000 || this.PInfo.All.PPIDSearchFlag == true)
                    {
                        this.PInfo.All.PPIDSearchFlag = false;        //초기화
                        return;
                    }

                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.PInfo.All.PPIDSearchFlag = false;
            }
        }
        #endregion

        #region "메세지 송신"

        /// <summary>
        /// Host로 부터 수신한 Primary Message가 Offline이면 Abort 한다.
        /// </summary>
        /// <param name="msgTran"> Primary Message</param>
        /// <returns> Abort 송신시 true return</returns>
        private Boolean funACTSECSAort_Send(Transaction msgTran)
        {
            Boolean dbolSend = false;
            short dshtReturn = 0;
            Transaction dMsgTran = null;
            short dshtStream = 0;
            short dshtFunction = 0;

            try
            {
                string dstrSF = string.Format("S{0}F{1}", msgTran.Primary().Stream, msgTran.Primary().Function);

                if (dstrSF == "S1F17" || dstrSF == "S1F1") return false;

                if (this.PInfo.All.ControlState == "1")
                {
                    dshtStream = msgTran.Primary().Stream;
                    dshtFunction = msgTran.Primary().Function;

                    string dstrSFName = string.Format("S{0}F0", dshtStream);
                    //dMsgTran = this.PSecsDrv.MakeTransaction(dstrSFName);
                    dMsgTran = this.PSecsDrv.MakeAbortTransaction(msgTran);

                    if (dMsgTran != null)
                    {
                        dshtReturn = Convert.ToInt16(dMsgTran.Send());

                        if (dshtReturn == 0)
                        {
                            dbolSend = true;
                        }
                        else
                        {
                            string logMsg = string.Format("Message S{0}F{1} Send Fail. Return({2}).", dshtStream.ToString(),
                                                                                                      dshtFunction.ToString(),
                                                                                                      dshtReturn.ToString());
                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, logMsg);
                        }
                    }
                    else
                    {
                        string logMsg = string.Format("Make Message S{0}F{1} Fail.", dshtStream.ToString(),
                                                                                     dshtFunction.ToString());
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, logMsg);
                    }
                }
                return dbolSend;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
        }
        #endregion
        
    }
}
