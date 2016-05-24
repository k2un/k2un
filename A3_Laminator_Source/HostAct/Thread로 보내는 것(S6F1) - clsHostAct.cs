using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Timers;
using System.Threading;
using System.Globalization;
using Microsoft.VisualBasic;
using CommonAct;

namespace HostAct
{
    public class clsHostAct
    {
        #region "선언"

            public string Version
            {
                get { return "Samsung SDI Wet Etch V1.3"; }
            }

            private SDIWetEtch.SEComSDIWetEtchClass PSecsDrv;         //외부 DLL을 사용하기 위핸 SecomDriver를 정의한다.
            private Boolean pbolReplyCheck = false;         //Reply가 들어왔는지 Check한다

            public InfoAct.clsInfo PInfo;                   //외부에서 여기서 사용할 구조체를 넣어줌

            private System.Threading.Thread pThreadSFSend = null;  //구조체에 있는 HOST로 송신할 S/F을 검사하여 HOST 송신 자체 Queue에 넣기위한 Thread 정의
            private System.Threading.Thread pThreadSFReceive = null;  //구조체에 있는 HOST로 수신한 S/F을 검사하여 HOST 송신 자체 Queue에 넣기위한 Thread 정의
            private System.Threading.Thread pThreadSVIDSend = null;             //SVID 테스트

            //private System.Timers.Timer SVIDSendThread;                 //PLC로 부터 SVID값을 읽기위한 Timer 정의

        #endregion


        //부모폼에서의 호출메소드(Open and Close)
        #region"Open and Close"

            //*******************************************************************************
            //  Function Name : funOpenSecsDrv()
            //  Description   : HostAct DLL의 인스턴스를 생성시키고 오픈한다.
            //  Parameters    : strEQPID => SECS에서 쓰이는 EQPID
            //  Return Value  : 성공 => True, 실패 => False
            //  Special Notes : 
            //*******************************************************************************
            //  2006/09/22          어 경태         [L 00]
            //*******************************************************************************   
            public int funOpenSecsDrv(string strEQPID)
            {
                int dintReturn = 0;

                try
                {
                    //Secom 클래스의 인스턴스를 생성 해서 클래스 변수(SECSDriv)에 넣는다.
                    this.PSecsDrv = new SDIWetEtch.SEComSDIWetEtchClass();

                    //Secs Drive의 이벤트 등록
                    subRegisterEvent();

                    //Open을 시도한다.
                    dintReturn = PSecsDrv.initializePlugIn(strEQPID, SEComPlugInLib.
                        ConnectionRetryCount.rty40, SEComPlugInLib.StartSystemByte.ssbNormal);

                    //리턴값이 1006이면 정상
                    if (dintReturn == 1006)
                    {
                        this.PInfo.All.SecomDriver = true;
                    }
                    else
                    {
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "clsHostAct(funOpenSecsDrv), SECom Driver Open Error.");   //Open이 되지 않아 에러가 발생한 경우
                    }

                    //SVIDSendThread Timer 설정
                    //this.SVIDSendThread = new System.Timers.Timer();
                    //this.SVIDSendThread.Elapsed += new ElapsedEventHandler(SVIDSend_Tick);
                    //this.SVIDSendThread.Interval = 1000;            //1000ms
                    //this.SVIDSendThread.Enabled = true;
                    //GC.KeepAlive(this.SVIDSendThread);

                }
                catch(Exception ex)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());

                }
                return dintReturn;
            }

            //*******************************************************************************
            //  Function Name : funSecsDrvClose()
            //  Description   : SEComSECS Driver를 Close시킨다.
            //  Parameters    : None
            //  Return Value  : int => 0(정상)
            //  Special Notes : 
            //*******************************************************************************
            //  2006/09/22          어 경태         [L 00]
            //*******************************************************************************   
            public int funSecsDrvClose()
            {
                int dintReturn = 0;

                try 
                {
                    this.PInfo.All.HostConnect = false;        //HOST와 연결해제되었음
                    dintReturn = Convert.ToInt32(this.PSecsDrv.terminatePlugIn());

                    //Thread 종료
                    if(this.pThreadSFSend != null)
                    {
                        this.pThreadSFSend.Abort();
                    }

                    if (this.pThreadSFReceive != null)
                    {
                        this.pThreadSFReceive.Abort();
                    }

                    if (this.pThreadSVIDSend != null)
                    {
                        this.pThreadSVIDSend.Abort();
                    }
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

        //*******************************************************************************
        //  Function Name : subRegisterEvent()
        //  Description   : 참조한 Secom DLL의 이벤트를 수신하기 위해 이벤트 핸들러를 등록한다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : VB.NET에서는 WithEvent를 사용하면 별도의 코딩없이
        //                  이벤트를 수신하지만 C#은 아래와 같이 이벤트 핸들러를
        //                  등록해줘야 한다.
        //*******************************************************************************
        //  2006/10/25          김 효주         [L 00]
        //*******************************************************************************   
        private void subRegisterEvent()
        {
            try
            {
                this.PSecsDrv.SECSConnected += new SDIWetEtch.__SEComSDIWetEtch_SECSConnectedEventHandler(PSecsDrv_SECSConnected);
                this.PSecsDrv.SECSDisconnected += new SDIWetEtch.__SEComSDIWetEtch_SECSDisconnectedEventHandler(PSecsDrv_SECSDisconnected);
                this.PSecsDrv.SECSTimeout += new SDIWetEtch.__SEComSDIWetEtch_SECSTimeoutEventHandler(PSecsDrv_SECSTimeout);
                this.PSecsDrv.SECSInvalidMessage += new SDIWetEtch.__SEComSDIWetEtch_SECSInvalidMessageEventHandler(PSecsDrv_SECSInvalidMessage);
                this.PSecsDrv.SECSAbortedMessage += new SDIWetEtch.__SEComSDIWetEtch_SECSAbortedMessageEventHandler(PSecsDrv_SECSAbortedMessage);
                this.PSecsDrv.SECSUnknownMessage += new SDIWetEtch.__SEComSDIWetEtch_SECSUnknownMessageEventHandler(PSecsDrv_SECSUnknownMessage);
                this.PSecsDrv.SEComDrvError += new SDIWetEtch.__SEComSDIWetEtch_SEComDrvErrorEventHandler(PSecsDrv_SEComDrvError);

                this.PSecsDrv.__SEComSDIWetEtch_Event_AreYouThere +=new SDIWetEtch.__SEComSDIWetEtch_AreYouThereEventHandler(PSecsDrv_AreYouThere);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S1F2IAmHere += new SDIWetEtch.__SEComSDIWetEtch_S1F2IAmHereEventHandler(PSecsDrv_S1F2IAmHere);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S10F3 +=new SDIWetEtch.__SEComSDIWetEtch_S10F3EventHandler(PSecsDrv_S10F3);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S10F9 +=new SDIWetEtch.__SEComSDIWetEtch_S10F9EventHandler(PSecsDrv_S10F9);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S1F11 +=new SDIWetEtch.__SEComSDIWetEtch_S1F11EventHandler(PSecsDrv_S1F11);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S1F15 +=new SDIWetEtch.__SEComSDIWetEtch_S1F15EventHandler(PSecsDrv_S1F15);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S1F17 +=new SDIWetEtch.__SEComSDIWetEtch_S1F17EventHandler(PSecsDrv_S1F17);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S1F3 +=new SDIWetEtch.__SEComSDIWetEtch_S1F3EventHandler(PSecsDrv_S1F3);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S1F5 +=new SDIWetEtch.__SEComSDIWetEtch_S1F5EventHandler(PSecsDrv_S1F5);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S2F101 +=new SDIWetEtch.__SEComSDIWetEtch_S2F101EventHandler(PSecsDrv_S2F101);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S2F103 +=new SDIWetEtch.__SEComSDIWetEtch_S2F103EventHandler(PSecsDrv_S2F103);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S2F15 +=new SDIWetEtch.__SEComSDIWetEtch_S2F15EventHandler(PSecsDrv_S2F15);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S2F23 +=new SDIWetEtch.__SEComSDIWetEtch_S2F23EventHandler(PSecsDrv_S2F23);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S2F29 +=new SDIWetEtch.__SEComSDIWetEtch_S2F29EventHandler(PSecsDrv_S2F29);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S2F31 +=new SDIWetEtch.__SEComSDIWetEtch_S2F31EventHandler(PSecsDrv_S2F31);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S2F41EQPCMD +=new SDIWetEtch.__SEComSDIWetEtch_S2F41EQPCMDEventHandler(PSecsDrv_S2F41EQPCMD);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S2F41PROCCMD +=new SDIWetEtch.__SEComSDIWetEtch_S2F41PROCCMDEventHandler(PSecsDrv_S2F41PROCCMD);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S3F1 +=new SDIWetEtch.__SEComSDIWetEtch_S3F1EventHandler(PSecsDrv_S3F1);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S5F101 +=new SDIWetEtch.__SEComSDIWetEtch_S5F101EventHandler(PSecsDrv_S5F101);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S5F2 +=new SDIWetEtch.__SEComSDIWetEtch_S5F2EventHandler(PSecsDrv_S5F2);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S6F12 +=new SDIWetEtch.__SEComSDIWetEtch_S6F12EventHandler(PSecsDrv_S6F12);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S6F14 +=new SDIWetEtch.__SEComSDIWetEtch_S6F14EventHandler(PSecsDrv_S6F14);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F1 +=new SDIWetEtch.__SEComSDIWetEtch_S7F1EventHandler(PSecsDrv_S7F1);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F101 +=new SDIWetEtch.__SEComSDIWetEtch_S7F101EventHandler(PSecsDrv_S7F101);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F103 +=new SDIWetEtch.__SEComSDIWetEtch_S7F103EventHandler(PSecsDrv_S7F103);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F105 +=new SDIWetEtch.__SEComSDIWetEtch_S7F105EventHandler(PSecsDrv_S7F105);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F108 +=new SDIWetEtch.__SEComSDIWetEtch_S7F108EventHandler(PSecsDrv_S7F108);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F109 +=new SDIWetEtch.__SEComSDIWetEtch_S7F109EventHandler(PSecsDrv_S7F109);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F23 +=new SDIWetEtch.__SEComSDIWetEtch_S7F23EventHandler(PSecsDrv_S7F23);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F25 +=new SDIWetEtch.__SEComSDIWetEtch_S7F25EventHandler(PSecsDrv_S7F25);
                this.PSecsDrv.__SEComSDIWetEtch_Event_S7F33 +=new SDIWetEtch.__SEComSDIWetEtch_S7F33EventHandler(PSecsDrv_S7F33);

                //Host로 전송할 메세지를 생성하는 SF 이벤트 등록
                this.PInfo.CreateSFEvent += new InfoAct.clsInfo.CreateEvent(subCreateSF);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

       

        #endregion

        //SEComEnabler에서 발생하는 기본 Event
        #region"SEComEnabler에서 발생하는 기본 Event"

        //*******************************************************************************
        //  Function Name : PSecsDrv_SECSConnected()
        //  Description   : Host와 SECsDriver가 Connect되었을때 발생한다.
        //  Parameters    : theEquipmentID => 장비ID
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00] 
        //*******************************************************************************               
        private void PSecsDrv_SECSConnected(string theEquipmentID)
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장

                this.PInfo.All.HostConnect = true;        //HOST와 연결되었음


                //CIM <-> PLC간 Auto Mode일때만 HOST와 연결되면 자동으로 Online 전환을 한다.
                //if (PInfo.All.AutoMode == true)
                //{
                    //이전상태 기억 못하는 경우(프로그램 최초 기동시)나 Offline에서 통신이끊어지고 다시 통신이 붙으면 Online Remote로 전환한다.
                    if (PInfo.All.ControlState == "1")
                    {
                        PInfo.All.ControlstateChangeBYWHO = "2";    //By Operator

                        PInfo.All.ControlStateOLD = PInfo.All.ControlState;     //현재의 ControlState를 백업
                        PInfo.All.WantControlState = "3";

                        PInfo.All.ONLINEModeChange = true;
                        this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);
                    }
                    else
                    {
                        PInfo.All.ControlstateChangeBYWHO = "2";    //By Operator

                        PInfo.All.WantControlState = PInfo.All.ControlStateOLD;
                        PInfo.All.ControlStateOLD = PInfo.All.ControlState;     //현재의 ControlState를 백업
                        PInfo.All.ControlState = PInfo.All.WantControlState;

                        PInfo.All.ONLINEModeChange = true;
                        this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);
                    }
                //}
            }
            catch(Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_SECSDisconnected()
        //  Description   : Host와 SECsDriver가 Disconnect되었을때 발생한다.
        //  Parameters    : theEquipmentID => 장비ID
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00] 
        //*******************************************************************************               
        private void PSecsDrv_SECSDisconnected(string theEquipmentID)
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
                this.PInfo.All.HostConnect = false;         //HOST와 연결해제되었음

                //현재의 ControlState를 저장해놓는다.
                this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;

                //this.PInfo.All.EQPMode = "0";               //Offline임을 저장

                //HOST와 연결이 끊어지면 현재 상태를 유지하고 OP Call창을 띄워 알린다.
                //if (this.PInfo.All.ControlState != "1")
                //{
                //    this.PInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSG, 0, "", "HOST Disconnected !!!");
                //}
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_SECSTimeout()
        //  Description   : T3 시간내에 Reply가 오지 않은 경우 발생한다.
        //  Parameters    : theCommonInfo => CommonInfo object. It contains common 
        //                      informations such as ErrorCode, ErrorMsg, etc.
        //                  theRelatedHeader => The Header object of SECSMessage 
        //                      that has been occurred by timeout.
        //                  theRelatedDataBlock => The DataBlock object of SECSMessage that 
        //                      has been occurred by timeout.
        //                      If related SECSMessage has header only, it returns Nothing.
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00] 
        //*******************************************************************************      
        private void PSecsDrv_SECSTimeout(ref SEComPlugInLib.CCommonInfo theCommonInfo,
                                            ref SEComPlugInLib.CHeader theRelatedHeader, ref SEComPlugInLib.CDataBlock theRelatedDataBlock)
        {
            string dstrSF = "";
           
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장

                dstrSF = "S" + theRelatedHeader.get_StreamNum().ToString() + "F" + theRelatedHeader.get_FunctionNum().ToString();
                if (dstrSF != "S0F0")
                {
                    if (this.PInfo.All.ONLINEModeChange == true)
                    {
                        this.PInfo.All.ONLINEModeChange = false;        //초기화
                        this.PInfo.All.WantControlState = "";

                        //Operator Call
                        //080114 - 주석처리
                        //this.PInfo.subOPCall_Set(InfoAct.clsInfo.OPCall.OnlineModeChangeT3TimeOut, 0, dstrSF, "The Online the T3 Time Out occurred from the conversing " + dstrSF);
                    }
                    else
                    {
                        //080114 - 주석처리
                        //this.PInfo.subOPCall_Set(InfoAct.clsInfo.OPCall.T3TimeOut, 0, dstrSF, "The T3 Time Out occurred from the " + dstrSF);
                    }

                    //Online Monotor <-> Control 전환시 T3 Timeout이 발생하면 Mode Change 창을 숨긴다.
                    if (this.PInfo.All.ModeChangeFormVisible == true)      
                    {
                        this.PInfo.subOPCall_Set(InfoAct.clsInfo.OPCall.FormClose, 0, "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_SECSInvalidMessage()
        //  Description   : Occurred when SEComPlugIn received invalid SECSMessage.
        //  Parameters    : theCommonInfo => The reference to the object of SEComPlugIn.
        //                  theHeader => Invalid SECSMessage’s Header.
        //                  theRelatedDataBlock => Invalid SECSMessage’s DataBlock.
        //                      If received SECSMessage has header only, it returns Nothing.
        //  Return Value  : None
        //  Special Notes : 특별히 할 작업이 없음
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00] 
        //*******************************************************************************      
        private void PSecsDrv_SECSInvalidMessage(ref SEComPlugInLib.CCommonInfo theCommonInfo,
                                                    ref SEComPlugInLib.CHeader theHeader, ref SEComPlugInLib.CDataBlock theRelatedDataBlock)
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_SECSAbortedMessage()
        //  Description   : Occurred when a primary SECS message is aborted by the equipment 
        //                  and there is no corresponding about SECS message defined.
        //  Parameters    : theCommonInfo => The reference to the object of SEComPlugIn.
        //                  theHeader => The header object of received SECSMessage.
        //                  theRelatedHeader => The Header object of SECSMessage that has been aborted.
        //                  theRelatedDataBlock => The DataBlock object of SECSMessage that has been aborted.
        //                      If related SECSMessage has header only, it returns Nothing.
        //  Return Value  : None
        //  Special Notes : 특별히 할 작업이 없음
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00] 
        //*******************************************************************************      
        private void PSecsDrv_SECSAbortedMessage(ref SEComPlugInLib.CCommonInfo theCommonInfo, ref SEComPlugInLib.CHeader theHeader,
                                                    ref SEComPlugInLib.CHeader theRelatedHeader, ref SEComPlugInLib.CDataBlock theRelatedDataBlock)
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_SEComDrvError()
        //  Description   : None
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : 특별히 할 작업이 없음
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00] 
        //*******************************************************************************      
        void PSecsDrv_SEComDrvError(short theErrorCode, string theErrorMsg)
        {
            try
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PSecsDrv_SEComDrvError(), The method or operation is not implemented.");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_SECSUnknownMessage()
        //  Description   : Occurred when the SEComDriver receives the SECS message whose 
        //                  structure is not defined in the Message Definitions.
        //  Parameters    : theCommonInfo => CommonInfo object. It contains common 
        //                      informations such as ErrorCode, ErrorMsg, etc.
        //                  theHeader => The reference to the object of CHeader for the 
        //                      SECS message that has been received.
        //                  theDataBlock => The reference to the object of CDataBlock for 
        //                      the SECS message that has been received. 
        //                  If the received message has header only, this DataBlock object is Nothing.
        //  Return Value  : None
        //  Special Notes : None 
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00] 
        //  2007/03/21          김 효주         [L 01] Host로 부터 등록되지 않은 Stream, Function이 내려오면
        //                                             S9F3(UnrecognizedStreamType) 혹은 S9F5(UnrecognizedFunctionType)를
        //                                             먼저 Secom Driver단에서 보내고 여기서는 SxF0을 보낸다.
        //*******************************************************************************      
        private void PSecsDrv_SECSUnknownMessage(ref SEComPlugInLib.CCommonInfo theCommonInfo, ref SEComPlugInLib.CHeader theHeader,
                                                    ref SEComPlugInLib.CDataBlock theDataBlock)
        {
            try
            {
                //string dstrSF = theHeader.get_StreamNum().ToString();
                ////SEComPlugInLib.CDataBlock objDataBlock = new SEComPlugInLib.CDataBlock();
                ////SEComPlugInLib.CHeader objHeader = new SEComPlugInLib.CHeader();

                //switch (dstrSF)
                //{
                //    case "1":
                //        this.PSecsDrv.S10F0.Reply(theHeader);
                //        break;
                //    case "2":
                //        this.PSecsDrv.S2F0.Reply(theHeader);
                //        break;
                //    case "5":
                //        this.PSecsDrv.S5F0.Reply(theHeader);
                //        break;
                //    case "6":
                //        this.PSecsDrv.S6F0.Reply(theHeader);
                //        break;
                //    case "7":
                //        this.PSecsDrv.S7F0.Reply(theHeader);
                //        break;
                //    case "9":
                //        this.PSecsDrv.S9F0.Reply(theHeader);
                //        break;
                //    case "10":
                //        this.PSecsDrv.S10F0.Reply(theHeader);
                //        break;
                //    default:    //Host로 부터 등록되지 않은 Stream, Function이 내려오면 아래에서 SxF0을 보낸다.
                //        //string dstrSFName = "S" + dstrSF + "F0";
                //        //short shStream = Convert.ToInt16(dstrSF);
                //        //short shFunction = 0;
                //        //Boolean dbolWait = false;

                //        //objHeader.setHeader(ref dstrSFName, ref shStream, ref shFunction, ref dbolWait);  //Header에 Data를 입력한다.

                //        //this.PSecsDrv.sendSECSMessage(ref objHeader, ref objDataBlock, ref dstrSFName);   //메세지 송신

                //        break;
                //}

                subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

        }

        #endregion

        //정의한 이벤트 수신
        #region"정의한 이벤트 수신"

        //*******************************************************************************
        //  Function Name : PSecsDrv_AreYouThere()
        //  Description   : Host로부터 S1F1을 수신하였을때(S1F1수신 후 S1F2 송신)
        //                  HOST에서 장비가 살아있는지 확인용임.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : 특별이 할 것은 없고 S1F2만 송신해주면 됨.
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_AreYouThere()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S1F1");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS1F1()
        {
            string dstrControlState = "";

            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
                if (this.PInfo.All.HostConnect == false) return;

                this.PSecsDrv.S1F2OnLineData.MDLN = this.PInfo.All.MDLN;

                switch (this.PInfo.All.ControlState)
                {
                    case "1":
                        dstrControlState = "OFF";
                        break;

                    case "2":
                        dstrControlState = "LOCAL";
                        break;

                    case "3":
                        dstrControlState = "REMOTE";
                        break;

                    default:
                        break;
                }

                //HOST로 S1F2 송신
                this.PSecsDrv.S1F2OnLineData.ONLINEMODE = dstrControlState;
                this.PSecsDrv.S1F2OnLineData.Reply(this.PSecsDrv.AreYouThere.Header);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S1F2IAmHere()
        //  Description   : Host로부터 On line 확인 Stream Function을 수신시 
        //                  On line Data를 송신 한다.(S1F1송신 후 S1F2 수신)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F2 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S1F2IAmHere()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S1F2");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS1F2()
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
                if (this.PInfo.All.HostConnect == false) return;

                this.PInfo.All.ONLINEModeChange = false;
                this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;     //현재의 ControlState를 백업
                this.PInfo.All.ControlState = this.PInfo.All.WantControlState;

                this.PInfo.subOPCall_Set(InfoAct.clsInfo.OPCall.FormClose, 0, "", "");      //Mode Change Form을 닫는다

                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.PInfo.All.ControlState);


                if (this.PInfo.All.ControlState == "2")
                {
                    //HOST로 Online 변경 보고를 한다.(S6F11, CEID=73(Remote) 보고)
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 72, 0);   //뒤에 0은 전체장비를 의미
                }
                else if (this.PInfo.All.ControlState == "3")
                {
                    //HOST로 Online 변경 보고를 한다.(S6F11, CEID=73(Remote) 보고)
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 73, 0);   //뒤에 0은 전체장비를 의미
                }

                this.PInfo.All.WantControlState = "";   //S6F11 보고 후 초기화
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S10F3()
        //  Description   : Host로부터 Terminal Msg 받을때
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S10F3 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S10F3()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S10F3");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS10F3()
        {
            int dintTID = 0;
            string dstrMsg = "";

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S10F3.Header) == true) return;

                dintTID = Convert.ToInt32(this.PSecsDrv.S10F3.TID);
                dstrMsg = this.PSecsDrv.S10F3.TEXT.ToString().Trim();


                switch (dintTID)
                {
                    case 0:             //CIM, 장비 T/P 모두 띄운다.
                        this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, dstrMsg);
                        this.PInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGNoBuzzer, 0, "S10F3", dstrMsg);
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg);
                        break;
                    case 1:             //Loader T/P이므로 무시

                        break;
                    case 2:             //CIM, 장비 T/P 모두 띄운다.
                        this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, dstrMsg);
                        this.PInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGNoBuzzer, 0, "S10F3", dstrMsg);
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg);

                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S10F9()
        //  Description   : Host로부터 Terminal Msg 를 수신하였을떄
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S10F9 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S10F9()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S10F9");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS10F9()
        {
            string dstrMsg = "";

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S10F9.Header) == true) return;

                dstrMsg = this.PSecsDrv.S10F9.TEXT.ToString().Trim();

                //CIM, 장비 T/P 모두 띄운다.
                this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, dstrMsg);
                this.PInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGNoBuzzer, 0, "S10F9", dstrMsg);
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S1F11()
        //  Description   : Host로부터 S1F11(State Variable Namelist Req.)을 받을때
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F11 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S1F11()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S1F11");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS1F11()
        {
            try
            {
                string dstrModuleID = "";
                int dintSVIDCount = 0;
                int dintSVID = 0;

                try
                {
                    if (this.PInfo.All.HostConnect == false) return;
                    if (funACTSECSAort_Send(this.PSecsDrv.S1F11.Header) == true) return;

                    dstrModuleID = this.PSecsDrv.S1F11.MODULEID.Trim();
                    //ModuleID가 존재하지 않는 경우
                    if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                    {
                        this.PSecsDrv.S1F12.MODULEID = dstrModuleID;
                        this.PSecsDrv.S1F12.SVCOUNT = 0;
                        this.PSecsDrv.S1F12.Reply(this.PSecsDrv.S1F11.Header);
                        return;
                    }


                    this.PSecsDrv.S1F12.MODULEID = dstrModuleID;
                    dintSVIDCount = Convert.ToInt32(this.PSecsDrv.S1F11.SVCOUNT);
                    if (dintSVIDCount == 0)  //모든 SVID를 보고
                    {
                        //모든 SVID 보고
                        this.PSecsDrv.S1F12.SVCOUNT = this.PInfo.Unit(0).SubUnit(0).SVIDCount;
                        for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).SVIDCount; dintLoop++)
                        {
                            this.PSecsDrv.S1F12.set_SVID(dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintLoop).SVID);
                            this.PSecsDrv.S1F12.set_SVNAME(dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintLoop).Name);
                        }

                        this.PSecsDrv.S1F12.Reply(this.PSecsDrv.S1F11.Header);
                    }
                    else
                    {
                        //받은 SVID중에 존재하지 않는것이 하나라도 있으면 L, 0으로 보고한다.
                        this.PSecsDrv.S1F12.SVCOUNT = dintSVIDCount;
                        for (int dintLoop = 1; dintLoop <= dintSVIDCount; dintLoop++)
                        {
                            dintSVID = Convert.ToInt32(this.PSecsDrv.S1F11.get_SVID(dintLoop));

                            if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID) == null)
                            {
                                this.PSecsDrv.S1F12.SVCOUNT = 0;
                                this.PSecsDrv.S1F12.Reply(this.PSecsDrv.S1F11.Header);
                                return;
                            }
                            else
                            {
                                this.PSecsDrv.S1F12.set_SVID(dintLoop, dintSVID);
                                this.PSecsDrv.S1F12.set_SVNAME(dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name);
                            }
                        }

                        //HOST로 받은 SVID가 모두 존재함
                        this.PSecsDrv.S1F12.Reply(this.PSecsDrv.S1F11.Header);
                    }

                }
                catch (Exception ex)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S1F15()
        //  Description   : Host로부터 Offline 요청을 받는다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F15 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S1F15()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S1F15");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS1F15()
        {
            int dintACK = 0;

            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
                if (this.PInfo.All.HostConnect == false) return;

                if (this.PInfo.All.ControlState == "1")
                {
                    dintACK = 2;         //2 = Equipment Already Offline
                }
                else
                {
                    dintACK = 0;         //0 = Offline Accepted

                    PInfo.All.ControlstateChangeBYWHO = "1";    //By HOST
                    PInfo.All.ControlStateOLD = PInfo.All.ControlState;     //현재의 ControlState를 백업
                    PInfo.All.ControlState = "1";
                    PInfo.All.WantControlState = "";    //초기화

                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, PInfo.All.ControlState); //PLC로 현재 ControlState 를 써준다.

                    //HOST로 Online 변경 보고를 한다.(S6F11, CEID=72(Local), CEID=73(Remote))
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 71, 0);   //뒤에 0은 전체장비를 의미
                }

                //HOST로 S1F16응답
                this.PSecsDrv.S1F16.OFLACK = dintACK;
                this.PSecsDrv.S1F16.Reply(this.PSecsDrv.S1F15.Header);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S1F17()
        //  Description   : Host로부터 On line 확인 Stream Function을 수신시 
        //                  On line Data를 송신 한다.(S1F1수신 후 S1F2 송신)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F2 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S1F17()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S1F17");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS1F17()
        {
            int dintCEID = 0;
            string dstrModuleID = "";
            string dstrMCMD = "";
            int dintACK = 0;

            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
                if (this.PInfo.All.HostConnect == false) return;

                dstrModuleID = this.PSecsDrv.S1F17.MODULEID.Trim();
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S1F18.MODULEID = dstrModuleID;
                    this.PSecsDrv.S1F18.ONLACK = 7;         //7 = There is no such a MODULEID
                    this.PSecsDrv.S1F18.Reply(this.PSecsDrv.S1F17.Header);
                    return;
                }


                dstrMCMD = this.PSecsDrv.S1F17.MCMD.ToString();

                if (dstrMCMD == "2" && this.PInfo.All.ControlState == "2")
                {
                    this.PSecsDrv.S1F18.MODULEID = dstrModuleID;
                    this.PSecsDrv.S1F18.ONLACK = 6;         //6 = Equipment Already ON_LINE LOCAL
                    this.PSecsDrv.S1F18.Reply(this.PSecsDrv.S1F17.Header);
                    return;
                }

                if (dstrMCMD == "3" && this.PInfo.All.ControlState == "3")
                {
                    this.PSecsDrv.S1F18.MODULEID = dstrModuleID;
                    this.PSecsDrv.S1F18.ONLACK = 2;         //2 = Equipment Already ON_LINE REMOTE
                    this.PSecsDrv.S1F18.Reply(this.PSecsDrv.S1F17.Header);
                    return;
                }


                switch (dstrMCMD)
                {
                    case "1":    //HOST부터 Online Request이기 때문에 이것은 오지 않는다.
                        break;

                    case "2":           //Local
                        //if (PInfo.All.AutoMode == true)
                        //{
                        //    dintACK = 3;
                        //    dintCEID = 72;
                        //}
                        //else
                        //{
                            dintACK = 5;    //5 = ON_LINE LOCAL Not Allowed.
                        //}
                     
                        break;

                    case "3":           //Remote
                        //if (PInfo.All.AutoMode == true)
                        //{
                        //    dintACK = 0;
                        //    dintCEID = 73;
                        //}
                        //else
                        //{
                            dintACK = 1;    //1 = ON_LINE REMOTE Not Allowed.
                        //}
                      
                        break;

                    default:
                        dintACK = 8;    //틀린 MCMD가 온경우
                        break;
                }

                //S1F18보고
                this.PSecsDrv.S1F18.MODULEID = dstrModuleID;
                this.PSecsDrv.S1F18.ONLACK = dintACK;
                this.PSecsDrv.S1F18.Reply(this.PSecsDrv.S1F17.Header);

                //Local, Remote로 변환 성공시 PLC에 써주고 HOST로 변경보고를 한다.
                if ((dintACK == 0 || dintACK == 3) && (dstrMCMD == "2" || dstrMCMD == "3"))
                {
                    PInfo.All.ControlstateChangeBYWHO = "1";    //By HOST
                    PInfo.All.ControlStateOLD = PInfo.All.ControlState;     //현재의 ControlState를 백업
                    PInfo.All.ControlState = dstrMCMD;
                    PInfo.All.WantControlState = "";    //초기화

                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, PInfo.All.ControlState); //PLC로 현재 ControlState 를 써준다.

                    //HOST로 Online 변경 보고를 한다.(S6F11, CEID=72(Local), CEID=73(Remote))
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, dintCEID, 0);   //뒤에 0은 전체장비를 의미
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S1F3()
        //  Description   : Host로부터 S1F3(Selected EQP State Req.)을 받을때
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F3 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S1F3()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S1F3");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS1F3()
        {
            string dstrModuleID = "";
            int dintSVIDCount = 0;
            int dintSVID = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S1F3.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S1F3.MODULEID.Trim();
                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S1F4.MODULEID = dstrModuleID;
                    this.PSecsDrv.S1F4.SVIDCOUNT = 0;
                    this.PSecsDrv.S1F4.Reply(this.PSecsDrv.S1F3.Header);
                    return;
                }


                this.PSecsDrv.S1F4.MODULEID = dstrModuleID;
                dintSVIDCount = Convert.ToInt32(this.PSecsDrv.S1F3.SVIDCOUNT);
                if (dintSVIDCount == 0)  //모든 SVID를 보고
                {
                    //모든 SVID 보고
                    this.PSecsDrv.S1F4.SVIDCOUNT = this.PInfo.Unit(0).SubUnit(0).SVIDCount;
                    for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).SVIDCount; dintLoop++)
                    {
                        dintSVID = dintLoop;

                        this.PSecsDrv.S1F4.set_SVID(dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).SVID);
                        this.PSecsDrv.S1F4.set_SV(dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value);
                        this.PSecsDrv.S1F4.set_SVNAME(dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name);
                    }

                    this.PSecsDrv.S1F4.Reply(this.PSecsDrv.S1F3.Header);
                }
                else
                {
                    //받은 SVID중에 존재하지 않는것이 하나라도 있으면 L, 0으로 보고한다.
                    this.PSecsDrv.S1F4.SVIDCOUNT = dintSVIDCount;
                    for (int dintLoop = 1; dintLoop <= dintSVIDCount; dintLoop++)
                    {
                        dintSVID = Convert.ToInt32(this.PSecsDrv.S1F3.get_SVID(dintLoop));

                        if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID) == null)
                        {
                            this.PSecsDrv.S1F4.SVIDCOUNT = 0;
                            this.PSecsDrv.S1F4.Reply(this.PSecsDrv.S1F3.Header);
                            return;
                        }
                        else
                        {
                            this.PSecsDrv.S1F4.set_SVID(dintLoop, dintSVID);
                            this.PSecsDrv.S1F4.set_SV(dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value);
                            this.PSecsDrv.S1F4.set_SVNAME(dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name);
                        }
                    }

                    //HOST로 받은 SVID가 모두 존재함
                    this.PSecsDrv.S1F4.Reply(this.PSecsDrv.S1F3.Header);
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S1F5()
        //  Description   : Host로부터 On line 확인 Stream Function을 수신시 
        //                  On line Data를 송신 한다.(S1F1수신 후 S1F2 송신)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F2 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S1F5()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S1F5");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS1F5()
        {
            string dstrModuleID = "";
            string dstrSFCD = "";
            int dintIndex = 0;
            string dstrLOTID = "";
            int dintSlotID = 0;
            Hashtable dhtGLSID = new Hashtable();

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S1F5.Header) == true) return;

                dstrSFCD = this.PSecsDrv.S1F5.SFCD.ToString();
                dstrModuleID = this.PSecsDrv.S1F5.MODULEID.Trim();
                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S1F6WrongMODID.MODULEID = dstrModuleID;
                    this.PSecsDrv.S1F6WrongMODID.SFCD = dstrSFCD;
                    this.PSecsDrv.S1F6WrongMODID.Reply(this.PSecsDrv.S1F5.Header);
                    return;
                }

                switch (dstrSFCD)
                {
                    case "1":       //EOID별 EOV값 List 보고

                        this.PSecsDrv.S1F6SFCD1.MODULEID = dstrModuleID;
                        this.PSecsDrv.S1F6SFCD1.SFCD = dstrSFCD;
                        this.PSecsDrv.S1F6SFCD1.EOCOUNT = 4;        //EOID 개수는 4개로 함

                        for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).EOIDCount; dintLoop++)
                        {
                            if (this.PInfo.Unit(0).SubUnit(0).EOID(dintLoop).EOID == 4)
                            {
                                dintIndex = dintIndex + 1;

                                this.PSecsDrv.S1F6SFCD1.set_EOID(4, this.PInfo.Unit(0).SubUnit(0).EOID(dintLoop).EOID);
                                this.PSecsDrv.S1F6SFCD1.set_EOMDCOUNT(4, 6);
                                this.PSecsDrv.S1F6SFCD1.set_EOMD(4, dintIndex, this.PInfo.Unit(0).SubUnit(0).EOID(dintLoop).EOMD);
                                this.PSecsDrv.S1F6SFCD1.set_EOV(4, dintIndex, this.PInfo.Unit(0).SubUnit(0).EOID(dintLoop).EOV);
                            }
                            else
                            {
                                this.PSecsDrv.S1F6SFCD1.set_EOID(dintLoop, this.PInfo.Unit(0).SubUnit(0).EOID(dintLoop).EOID);
                                this.PSecsDrv.S1F6SFCD1.set_EOMDCOUNT(dintLoop, 1);
                                this.PSecsDrv.S1F6SFCD1.set_EOMD(dintLoop, 1, this.PInfo.Unit(0).SubUnit(0).EOID(dintLoop).EOMD);
                                this.PSecsDrv.S1F6SFCD1.set_EOV(dintLoop, 1, this.PInfo.Unit(0).SubUnit(0).EOID(dintLoop).EOV);
                            }
                        }
                        this.PSecsDrv.S1F6SFCD1.Reply(this.PSecsDrv.S1F5.Header);   //S1F6 송신

                        break;

                    case "3":           //현재 장비내에 존재하는 GLS 정보 보고

                        this.PSecsDrv.S1F6SFCD3.MODULEID = dstrModuleID;
                        this.PSecsDrv.S1F6SFCD3.SFCD = dstrSFCD;


                        //장비안에 GLS가 존재하지 않으면 L,0으로 보고한다.
                        //if (this.PInfo.LOTCount == 0)
                        if (this.PInfo.LOTCount == 0 || this.PInfo.Unit(0).SubUnit(0).GLSExist == false)
                        {
                            this.PSecsDrv.S1F6SFCD3.PANELCOUNT = 0;
                            this.PSecsDrv.S1F6SFCD3.Reply(this.PSecsDrv.S1F5.Header);   //S1F6 송신
                        }
                        else
                        {
                            for (int dintUnit = 1; dintUnit <= this.PInfo.UnitCount; dintUnit++)
                            {
                                foreach (string dstrGLSID in this.PInfo.Unit(dintUnit).SubUnit(0).CurrGLS())
                                {
                                    dstrLOTID = this.PInfo.Unit(dintUnit).SubUnit(0).CurrGLS(dstrGLSID).LOTID;
                                    dintSlotID = this.PInfo.Unit(dintUnit).SubUnit(0).CurrGLS(dstrGLSID).SlotID;


                                    //혹시나 1개의 GLS가 막 나갈려는 찰나에 양쪽 Unit에 걸쳐있을떄 GLSID 중복 보고 방지
                                    if (dhtGLSID.ContainsKey(dstrGLSID) == false && this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).Scrap == false)
                                    {
                                        dhtGLSID.Add(dstrGLSID, dstrGLSID);

                                        dintIndex = dintIndex + 1;
                                        this.PSecsDrv.S1F6SFCD3.PANELCOUNT = dintIndex;    //현재 장비안에 있는 GLS 개수


                                        this.PSecsDrv.S1F6SFCD3.set_H_PANELID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).H_PANELID);
                                        this.PSecsDrv.S1F6SFCD3.set_E_PANELID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).E_PANELID);
                                        this.PSecsDrv.S1F6SFCD3.set_LOTID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).LOTID);
                                        this.PSecsDrv.S1F6SFCD3.set_BATCHID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).BATCHID);
                                        this.PSecsDrv.S1F6SFCD3.set_SLOTNO(dintIndex, FunStringH.funMakeLengthStringFirst(this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).SlotID.ToString(), 2));
                                        this.PSecsDrv.S1F6SFCD3.set_PROD_TYPE(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROD_TYPE);
                                        this.PSecsDrv.S1F6SFCD3.set_PROD_KIND(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROD_KIND);
                                        this.PSecsDrv.S1F6SFCD3.set_DEVICEID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).DEVICEID);
                                        this.PSecsDrv.S1F6SFCD3.set_RUNSHEETID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).RUNSHEETID);
                                        this.PSecsDrv.S1F6SFCD3.set_FLOWID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FLOWID);
                                        this.PSecsDrv.S1F6SFCD3.set_STEPID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).STEPID);
                                        this.PSecsDrv.S1F6SFCD3.set_PPID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PPID);
                                        this.PSecsDrv.S1F6SFCD3.set_THICKNESS(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).THICKNESS);
                                        this.PSecsDrv.S1F6SFCD3.set_INS_FLAG(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).INS_FLAG);
                                        this.PSecsDrv.S1F6SFCD3.set_FLOWRECIPE(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FlowRecipe);
                                        this.PSecsDrv.S1F6SFCD3.set_PROCESS_STEP(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROCESS_STEP);
                                        this.PSecsDrv.S1F6SFCD3.set_PANEL_SIZE(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_SIZE);
                                        this.PSecsDrv.S1F6SFCD3.set_PANEL_POSITION(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_POSITION);
                                        this.PSecsDrv.S1F6SFCD3.set_COUNT(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).COUNT);
                                        this.PSecsDrv.S1F6SFCD3.set_GRADE(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).GRADE);
                                        this.PSecsDrv.S1F6SFCD3.set_SORT_FLAG(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).SORT_FLAG);
                                        this.PSecsDrv.S1F6SFCD3.set_HOT_FLOW_FLAG(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).HOTFLOWFLAG);
                                        this.PSecsDrv.S1F6SFCD3.set_M_START_FLAG(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).M_START_FLAG);
                                        this.PSecsDrv.S1F6SFCD3.set_PANEL_TYPE(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_TYPE);
                                        this.PSecsDrv.S1F6SFCD3.set_COMMENT(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).COMMENT);
                                        this.PSecsDrv.S1F6SFCD3.set_FILE_LOC(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FILE_LOC);
                                        this.PSecsDrv.S1F6SFCD3.set_DEFECT_COUNT(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).DEFECT_COUNT);

                                        this.PSecsDrv.S1F6SFCD3.set_PANEL_STATE(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANELSTATE);
                                        this.PSecsDrv.S1F6SFCD3.set_JUDGEMENT(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).JUDGEMENT);
                                        this.PSecsDrv.S1F6SFCD3.set_CODE(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).CODE);
                                        this.PSecsDrv.S1F6SFCD3.set_RUNLINE(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).RUNLINE);
                                        this.PSecsDrv.S1F6SFCD3.set_UNIQUEID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).UNIQUEID);

                                        //this.PSecsDrv.S1F6SFCD3.set_PAIR_H_PANELID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PAIR_H_PANELID);
                                        //this.PSecsDrv.S1F6SFCD3.set_PAIR_E_PANELID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PAIR_E_PANELID);
                                        //this.PSecsDrv.S1F6SFCD3.set_PAIR_UNIQUEID(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PAIR_UNIQUEID);
                                        //this.PSecsDrv.S1F6SFCD3.set_MATCH_GROUP(dintIndex, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).MATCH_GROUP);
                                    }
                                }
                            }

                            if (dintIndex == 0) this.PSecsDrv.S1F6SFCD3.PANELCOUNT = 0; //만약 장비안에 GLS가 하나도 존재하지 않으면 L, 0으로 보고
                            this.PSecsDrv.S1F6SFCD3.Reply(this.PSecsDrv.S1F5.Header);   //S1F6 송신
                        }

                        break;

                    case "4":       //각 Unit(Module) 상태 보고

                        this.PSecsDrv.S1F6SFCD4.MODULEID = dstrModuleID;
                        this.PSecsDrv.S1F6SFCD4.SFCD = dstrSFCD;
                        this.PSecsDrv.S1F6SFCD4.EQP_STATE = this.PInfo.Unit(0).SubUnit(0).EQPState;
                        this.PSecsDrv.S1F6SFCD4.PROC_STATE = this.PInfo.Unit(0).SubUnit(0).EQPProcessState;
                        this.PSecsDrv.S1F6SFCD4.MCMD = this.PInfo.All.ControlState;

                        this.PSecsDrv.S1F6SFCD4.LAYER1COUNT = this.PInfo.UnitCount;

                        for (int dintLoop = 1; dintLoop <= this.PInfo.UnitCount; dintLoop++)
                        {
                            this.PSecsDrv.S1F6SFCD4.set_MODULEID1(dintLoop, this.PInfo.Unit(dintLoop).SubUnit(0).ModuleID);
                            this.PSecsDrv.S1F6SFCD4.set_EQP_STATE1(dintLoop, this.PInfo.Unit(dintLoop).SubUnit(0).EQPState);
                            this.PSecsDrv.S1F6SFCD4.set_PROC_STATE1(dintLoop, this.PInfo.Unit(dintLoop).SubUnit(0).EQPProcessState);
                        }
                        this.PSecsDrv.S1F6SFCD4.Reply(this.PSecsDrv.S1F5.Header);   //S1F6 송신

                        break;

                    default:   //잘못된 SFCD가 내려올 경우
                        this.PSecsDrv.S1F6WrongSFCD.MODULEID = dstrModuleID;
                        this.PSecsDrv.S1F6WrongSFCD.Reply(this.PSecsDrv.S1F5.Header);
                        break;
                }


            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S2F101()
        //  Description   : Host로부터 S2F101(Operator Call)을 수신하였을때
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S2F101 수신(3 LAMP BLINK & BUZZER ON)
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S2F101()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S2F101");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS2F101()
        {
            int dintTID = 0;
            string dstrMsg = "";
            int dintAck = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S2F101.Header) == true) return;

                dintTID = Convert.ToInt32(this.PSecsDrv.S2F101.TID);
                dstrMsg = this.PSecsDrv.S2F101.MSG.ToString().Trim();

                switch (dintTID)
                {
                    case 0:             //CIM, 장비 T/P 모두 띄운다.
                        this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, dstrMsg);
                        this.PInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGBuzzer, 0, "S2F101", dstrMsg);
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg);

                        dintAck = 0;    //ACK
                        break;
                    case 1:             //Loader T/P이므로 무시

                        dintAck = 2;    //NAK
                        break;
                    case 2:             //장비 T/P에만 띄운다.
                        this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, dstrMsg);
                        this.PInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGBuzzer, 0, "S2F101", dstrMsg);
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg);

                        dintAck = 0;    //ACK
                        break;

                    default:
                        dintAck = 2;    //NAK
                        break;
                }

                this.PSecsDrv.S2F102.ACKC2 = dintAck;
                this.PSecsDrv.S2F102.Reply(this.PSecsDrv.S2F101.Header);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S2F103()
        //  Description   : Host로부터 EQP Online Parameter 변경 요청을 받았을때
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S2F103 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S2F103()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S2F103");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS2F103()
        {
            string dstrModuleID = "";
            int dintEOIDCount = 0;
            int dintEOID = 0;
            Boolean dbolFail = false;
            int dintEOMD = 0;
            int dintEOV = 0;
            int dintEAC = 0;
            string dstrEOIDIndex = "";
            int dintIndex = 0;
            int dintEOMDCount = 0;
            Boolean dbolEQPProcessTimeOverReset = false;        //기존에 설정되어 있던 EQP Process Time Over 체크 설정을 다시 할지 여부
            string dstrSQL = "";

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S2F103.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S2F103.MODULEID.Trim();
                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S2F104.MODULEID = dstrModuleID;
                    this.PSecsDrv.S2F104.MIACK = 1;          //1 = There is no such a MODULEID
                    this.PSecsDrv.S2F104.EOIDCOUNT = 0;
                    this.PSecsDrv.S2F104.Reply(this.PSecsDrv.S2F103.Header);
                    return;
                }

                this.PSecsDrv.S2F104.MODULEID = dstrModuleID;
                this.PSecsDrv.S2F104.MIACK = 0;          //0 = No error
                dintEOIDCount = Convert.ToInt32(this.PSecsDrv.S2F103.EOIDCOUNT.ToString());
                this.PSecsDrv.S2F104.EOIDCOUNT = dintEOIDCount;

                //받은 EOID중에 존재하지 않는것이 하나라도 있으면 NAK으로 보고하고 모두 적용하지 않는다.
                for (int dintLoop = 1; dintLoop <= dintEOIDCount; dintLoop++)
                {
                    dintEOMDCount = Convert.ToInt32(this.PSecsDrv.S2F103.get_EOMDCOUNT(dintLoop).ToString());
                    for (int dintLoop2 = 1; dintLoop2 <= dintEOMDCount; dintLoop2++)
                    {
                        dintEOID = Convert.ToInt32(this.PSecsDrv.S2F103.get_EOID(dintLoop).ToString());
                        dintEOMD = Convert.ToInt32(this.PSecsDrv.S2F103.get_EOMD(dintLoop, dintLoop2).ToString());
                        dintEOV = Convert.ToInt32(this.PSecsDrv.S2F103.get_EOV(dintLoop, dintLoop2).ToString());

                        //유효성검사
                        if (dintEOID == 1 || dintEOID == 2 || dintEOID == 3)
                        {
                            if (dintEOMD == 0)
                            {
                                if (dintEOV == 1 || dintEOV == 2)
                                {
                                    if (this.PInfo.Unit(0).SubUnit(0).EOID(dintEOID).EOV != dintEOV)
                                    {
                                        dintEAC = 0;    //정상
                                    }
                                    else   //현재 CIM에서 가지고 있는 EOV값과 HOST에서 내려온 EOV값이 같을 경우
                                    {
                                        dbolFail = true;    //유효성 검사에서 에러임을 저장
                                        dintEAC = 2;    //2 = Denied, Busy
                                    }
                                }
                                else
                                {
                                    dbolFail = true;    //유효성 검사에서 에러임을 저장
                                    dintEAC = 3;   //3 = Denied, At least one constant out of range
                                }
                            }
                            else
                            {
                                dbolFail = true;    //유효성 검사에서 에러임을 저장
                                dintEAC = 1;   //EOMD는 항상 0이어야 함.(틀린 EOMD)
                            }
                        }
                        else if (dintEOID == 4)  //EQP Process State Time Over
                        {
                            //EOMD, EOMD값으로 Index를 찾는다.(Index가 Key값이기 떄문)
                            dintIndex = this.PInfo.funGetEOIDEOMDToIndex(dintEOID, dintEOMD);
                            if (dintIndex == 0)  //EOMD값이 틀림
                            {
                                dbolFail = true;    //유효성 검사에서 에러임을 저장
                                dintEAC = 1;   //EOMD는 항상 0이어야 함.
                            }

                            if (this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV != dintEOV)
                            {
                                if (dintEOV < 0 || dintEOV > 99)   //시간(분)은 0~99분까지만 온다.
                                {
                                    dbolFail = true;    //유효성 검사에서 에러임을 저장
                                    dintEAC = 3;   //NAK로 응답
                                }
                                else
                                {
                                    dintEAC = 0;    //정상
                                }
                            }
                            else   //현재 CIM에서 가지고 있는 EOV값과 HOST에서 내려온 EOV값이 같을 경우
                            {
                                dbolFail = true;    //유효성 검사에서 에러임을 저장
                                dintEAC = 2;    //2 = Denied, Busy
                            }
                        }
                        else  //존재하지 않는 EOID
                        {
                            dbolFail = true;    //유효성 검사에서 에러임을 저장
                            dintEAC = 1;   //1 = Denied, At least one constant does not exist
                        }

                        this.PSecsDrv.S2F104.set_EOID(dintLoop, dintEOID);
                        this.PSecsDrv.S2F104.set_EOMDCOUNT(dintLoop, this.PSecsDrv.S2F103.get_EOMDCOUNT(dintLoop));

                        this.PSecsDrv.S2F104.set_EOMD(dintLoop, dintLoop2, dintEOMD);
                        this.PSecsDrv.S2F104.set_EAC(dintLoop, dintLoop2, dintEAC);
                    }
                }

                //HOST로 S2F104 보고
                this.PSecsDrv.S2F104.Reply(this.PSecsDrv.S2F103.Header);


                //유효성 검사에서 OK이면 S6F11(CEID=101) 보고
                if (dbolFail == false)
                {
                    this.PInfo.All.EOIDChangeBYWHO = "1";   //BY HOST

                    //구조체를 변경
                    for (int dintLoop = 1; dintLoop <= dintEOIDCount; dintLoop++)
                    {
                        dintEOMDCount = Convert.ToInt32(this.PSecsDrv.S2F103.get_EOMDCOUNT(dintLoop).ToString());
                        for (int dintLoop2 = 1; dintLoop2 <= dintEOMDCount; dintLoop2++)
                        {
                            dintEOID = Convert.ToInt32(this.PSecsDrv.S2F103.get_EOID(dintLoop).ToString());
                            dintEOMD = Convert.ToInt32(this.PSecsDrv.S2F103.get_EOMD(dintLoop, dintLoop2).ToString());
                            dintEOV = Convert.ToInt32(this.PSecsDrv.S2F103.get_EOV(dintLoop, dintLoop2).ToString());

                            //EOMD, EOMD값으로 Index를 찾는다.(Index가 Key값이기 떄문)
                            dintIndex = this.PInfo.funGetEOIDEOMDToIndex(dintEOID, dintEOMD);

                            this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV = dintEOV;

                            //기존에 설정되어 있던 EQP Process Time Over 체크 설정을 다시 할지 여부
                            if (dintEOID == 4) dbolEQPProcessTimeOverReset = true;

                            //DB 정보 변경
                            //EOID가 4일경우 Time Over 체크 Timer 초기화 한후 다시 설정하는 루틴 필요
                            dstrEOIDIndex = dstrEOIDIndex + dintIndex.ToString() + ";";

                            //DB Update
                            dstrSQL = "Update tbEOID set EOV=" + dintEOV.ToString() + " Where Index=" + dintIndex.ToString();

                            if (DBAct.clsDBAct.funExecuteQuery(dstrSQL) == false)
                            {
                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "DB Update Fail: EOID Index: " + dintIndex.ToString());
                            }
                        }
                    }

                    //CEID=101, Host로 보고
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentParameterEvent, 101, dstrEOIDIndex);   //마지막 인자는 EOID Index임

                    this.PInfo.All.EOIDChangeBYWHO = "";   //초기화
                }

                if (dbolEQPProcessTimeOverReset == true) this.PInfo.All.EQPProcessTimeOverReset = true;

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S2F15()
        //  Description   : HOST로 부터 ECID 값 변경을 요청받았을때 
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S2F15 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S2F15()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S2F15");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS2F15()
        {
            string dstrModuleID = "";
            int dintECIDCount = 0;
            int dintECID = 0;
            string dstrECName = "";
            Boolean dbolFail = false;
            int dintFailCount = 0;
            int dintECSLL = 0;
            int dintECWLL = 0;
            int dintECDEF = 0;
            int dintECWUL = 0;
            int dintECSUL = 0;
            int dintEAC = 0;
            int dintTEAC = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S2F15.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S2F15.MODULEID.Trim();
                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S2F16.MODULEID = dstrModuleID;
                    this.PSecsDrv.S2F16.MIACK = 1;          //1 = There is no such a MODULEID
                    this.PSecsDrv.S2F16.ECCOUNT = 0;
                    this.PSecsDrv.S2F16.Reply(this.PSecsDrv.S2F15.Header);
                    return;
                }

                //장비에서 현재 ECID변경을 못하는 경우
                //if (this.PInfo.All.ECIDChangePossible == false)
                //{
                //    this.PSecsDrv.S2F16.MODULEID = dstrModuleID;
                //    this.PSecsDrv.S2F16.MIACK = 1;          //1 = There is no such a MODULEID
                //    this.PSecsDrv.S2F16.ECCOUNT = 0;
                //    this.PSecsDrv.S2F16.Reply(this.PSecsDrv.S2F15.Header);
                //    return;
                //}

                this.PSecsDrv.S2F16.MODULEID = dstrModuleID;
                this.PSecsDrv.S2F16.MIACK = 0;          //0 = No error
                dintECIDCount = Convert.ToInt32(this.PSecsDrv.S2F15.ECCOUNT.ToString());
                this.PSecsDrv.S2F16.ECCOUNT = dintECIDCount;


                //받은 ECID중에 존재하지 않는것이 하나라도 있으면 NAK으로 보고하고 PLC에 모두 적용하지 않는다.
                for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                {
                    dintECID = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECID(dintLoop).ToString());
                    dstrECName = this.PSecsDrv.S2F15.get_ECNAME(dintLoop).ToString().Trim();

                    dintECSLL = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECSLL(dintLoop).ToString());
                    dintECWLL = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECWLL(dintLoop).ToString());
                    dintECDEF = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECDEF(dintLoop).ToString());
                    dintECWUL = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECWUL(dintLoop).ToString());
                    dintECSUL = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECSUL(dintLoop).ToString());

                    //존재하지 않는 ECID가 왔을때
                    if (this.PInfo.Unit(0).SubUnit(0).ECID(dintECID) == null)
                    {
                        dbolFail = true;
                    }
                    else if (dstrECName != this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).Name)  //존재하지 않는 ECID가 왔을때
                    {
                        dbolFail = true;
                    }
                    else
                    {
                        if (dintECSLL <= dintECWLL && dintECWLL <= dintECDEF && dintECDEF <= dintECWUL && dintECWUL <= dintECSUL)
                        {
                        }
                        else
                        {
                            dbolFail = true;
                        }
                    }

                    if (dbolFail == true)  //Validation 체크 에러
                    {
                        dintFailCount = dintFailCount + 1;      //각 항목별로 Fail이 났을때 1씩 증가
                        dintEAC = 1;
                        dintTEAC = 2;                           
                    }
                    else
                    {
                        dintEAC = 0;
                        dintTEAC = 0;
                    }

                    this.PSecsDrv.S2F16.set_TEAC(dintLoop, dintTEAC);

                    this.PSecsDrv.S2F16.set_ECID(dintLoop, dintECID);
                    this.PSecsDrv.S2F16.set_EAC1(dintLoop, dintEAC);

                    this.PSecsDrv.S2F16.set_ECNAME(dintLoop, dstrECName);
                    this.PSecsDrv.S2F16.set_EAC2(dintLoop, dintEAC);

                    this.PSecsDrv.S2F16.set_ECDEF(dintLoop, dintECDEF.ToString());
                    this.PSecsDrv.S2F16.set_EAC3(dintLoop, dintEAC);

                    this.PSecsDrv.S2F16.set_ECSLL(dintLoop, dintECSLL.ToString());
                    this.PSecsDrv.S2F16.set_EAC4(dintLoop, dintEAC);

                    this.PSecsDrv.S2F16.set_ECSUL(dintLoop, dintECSUL.ToString());
                    this.PSecsDrv.S2F16.set_EAC5(dintLoop, dintEAC);

                    this.PSecsDrv.S2F16.set_ECWLL(dintLoop, dintECWLL.ToString());
                    this.PSecsDrv.S2F16.set_EAC6(dintLoop, dintEAC);

                    this.PSecsDrv.S2F16.set_ECWUL(dintLoop, dintECWUL.ToString());
                    this.PSecsDrv.S2F16.set_EAC7(dintLoop, dintEAC);
                }

                //HOST로 S2F16 보고
                this.PSecsDrv.S2F16.Reply(this.PSecsDrv.S2F15.Header);


                //카운트가 0이면 모두 맞는 것임
                if (dintFailCount == 0)
                {
                    this.PInfo.All.ECIDChangeBYWHO = "1";   //BY HOST

                    this.PInfo.All.ECIDChange.Clear();      //초기화

                    //구조체를 변경
                    for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                    {
                        dintECID = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECID(dintLoop).ToString());

                        dintECSLL = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECSLL(dintLoop).ToString());
                        dintECWLL = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECWLL(dintLoop).ToString());
                        dintECDEF = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECDEF(dintLoop).ToString());
                        dintECWUL = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECWUL(dintLoop).ToString());
                        dintECSUL = Convert.ToInt32(this.PSecsDrv.S2F15.get_ECSUL(dintLoop).ToString());

                        //PLC에 Write할 ECID를 저장한다.                      
                        this.PInfo.All.ECIDChange.Add(dintECID, dintECSLL.ToString() + "," + dintECWLL.ToString() + "," + dintECDEF.ToString() + "," +
                                                        dintECWUL.ToString() + "," + dintECSUL.ToString());
                    }

                    //HOST에서 받은것외에 없는 항목은 CIM이 가지고 있는 Data를 써준다.
                    for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop++)
                    {
                        if (this.PInfo.All.ECIDChange.ContainsKey(dintLoop) == true)
                        {
                        }
                        else
                        {
                            dintECID = dintLoop;

                            dintECSLL = Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSLL);
                            dintECWLL = Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWLL);
                            dintECDEF = Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECDEF);
                            dintECWUL = Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWUL);
                            dintECSUL = Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSUL);

                            //PLC에 Write할 ECID를 저장한다.                      
                            this.PInfo.All.ECIDChange.Add(dintECID, dintECSLL.ToString() + "," + dintECWLL.ToString() + "," + dintECDEF.ToString() + "," +
                                                            dintECWUL.ToString() + "," + dintECSUL.ToString());
                        }
                    }

                    //PLC로 ECID 변경 Write
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ECIDChange);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S2F23()
        //  Description   : Trace Data Set(S2F23 수신)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F2 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S2F23()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S2F23");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS2F23()
        {
            string dstrModuleID = "";
            DateTime dDateTime;
            int intTRID = 0;
            string dstrDSPER = "";
            int dintTOTSMP = 0;
            int dintREPGSZ = 0;
            int dintSVID = 0;
            int dintSVCount = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S2F23.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S2F23.MODULEID.Trim();
                intTRID = Convert.ToInt32(this.PSecsDrv.S2F23.TRID);
                dstrDSPER = this.PSecsDrv.S2F23.DSPER.Trim();
                dintTOTSMP = Convert.ToInt32(this.PSecsDrv.S2F23.TOTSMP);
                dintREPGSZ = Convert.ToInt32(this.PSecsDrv.S2F23.REPGSZ);
                dintSVCount = Convert.ToInt32(this.PSecsDrv.S2F23.SVCOUNT);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S2F24.TIACK = 2;
                    this.PSecsDrv.S2F24.Reply(this.PSecsDrv.S2F23.Header);
                    return;
                }

                //TRID Reset의 경우는 해당 TRID가 있는지만 체크한다.(다른 항목은 체크안함)
                if (this.PInfo.Unit(0).SubUnit(0).TRID(intTRID) != null)
                {
                    if (dintSVCount == 0)
                    {
                        //해당 TRID를 Reset하고 제거한다.
                        this.PInfo.Unit(0).SubUnit(0).RemoveTRID(intTRID);

                        this.PSecsDrv.S2F24.TIACK = 0;
                        this.PSecsDrv.S2F24.Reply(this.PSecsDrv.S2F23.Header);
                        return;
                    }
                }

                //시간 Format 체크
                try
                {
                    dDateTime = DateTime.ParseExact(dstrDSPER, "HHmmss", CultureInfo.InvariantCulture);
                }
                catch
                {
                    this.PSecsDrv.S2F24.TIACK = 1;
                    this.PSecsDrv.S2F24.Reply(this.PSecsDrv.S2F23.Header);
                    return;
                }


                //REPGSZ가 0인지 체크
                if (dintREPGSZ == 0)
                {
                    this.PSecsDrv.S2F24.TIACK = 1;
                    this.PSecsDrv.S2F24.Reply(this.PSecsDrv.S2F23.Header);
                    return;
                }

                //받은 SVID중에 하나라도 존재하지 않는 것이면 NAK
                for (int dintLoop = 1; dintLoop <= dintSVCount; dintLoop++)
                {
                    dintSVID = Convert.ToInt32(this.PSecsDrv.S2F23.get_SVID(dintLoop));
                    if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID) == null)
                    {
                        this.PSecsDrv.S2F24.TIACK = 1;
                        this.PSecsDrv.S2F24.Reply(this.PSecsDrv.S2F23.Header);
                        return;
                    }
                }



                if (dintSVCount > 0)
                {
                    if (this.PInfo.Unit(0).SubUnit(0).AddTRID(intTRID) == true)
                    {
                        this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).DSPER = dstrDSPER;
                        this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).REPGSZ = dintREPGSZ;
                        this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).TOTSMP = dintTOTSMP;

                        for (int dintGroup = 1; dintGroup <= dintREPGSZ; dintGroup++)
                        {
                            if (this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).AddGroup(dintGroup) == true)
                            {
                                //기본 6개항목을 먼저 추가한다. 
                                //for (int dintLoop = 1; dintLoop <= 6; dintLoop++)
                                //{
                                //    if (this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).AddSVID(dintLoop) == true)
                                //    {
                                //        this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).SVID(dintLoop).SVID = dintLoop;
                                //        this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).SVID(dintLoop).Name = this.PInfo.Unit(0).SubUnit(0).SVID(dintLoop).Name;
                                //    }
                                //}

                                //HOST에서 받은 항목을 추가한다.
                                for (int dintLoop = 1; dintLoop <= dintSVCount; dintLoop++)
                                {
                                    dintSVID = Convert.ToInt32(this.PSecsDrv.S2F23.get_SVID(dintLoop));
                                    if (this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).AddSVID(dintLoop) == true)
                                    {
                                        this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).SVID(dintLoop).SVID = dintSVID;
                                        this.PInfo.Unit(0).SubUnit(0).TRID(intTRID).Group(dintGroup).SVID(dintLoop).Name = this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name;
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    //현재 TRID가 한개도 없는데 Trace Reset이 온 경우 NAK
                    if (this.PInfo.Unit(0).SubUnit(0).TRIDCount == 0)
                    {
                        this.PSecsDrv.S2F24.TIACK = 1;
                        this.PSecsDrv.S2F24.Reply(this.PSecsDrv.S2F23.Header);
                        return;
                    }

                    if (this.PInfo.Unit(0).SubUnit(0).TRID(intTRID) == null)
                    {
                        this.PSecsDrv.S2F24.TIACK = 1;
                        this.PSecsDrv.S2F24.Reply(this.PSecsDrv.S2F23.Header);
                        return;
                    }

                    //해당 TRID를 Reset하고 제거한다.
                    this.PInfo.Unit(0).SubUnit(0).RemoveTRID(intTRID);
                }

                this.PSecsDrv.S2F24.TIACK = 0;
                this.PSecsDrv.S2F24.Reply(this.PSecsDrv.S2F23.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S2F29()
        //  Description   : HOST에서 ECID List 값 조회시
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S2F29 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S2F29()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S2F29");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS2F29()
        {
            string dstrModuleID = "";
            int dintECIDCount = 0;
            int dintECID = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S2F29.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S2F29.MODULEID.Trim();
                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S2F30.MODULEID = dstrModuleID;
                    this.PSecsDrv.S2F30.ECCOUNT = 0;
                    this.PSecsDrv.S2F30.Reply(this.PSecsDrv.S2F29.Header);
                    return;
                }

                dintECIDCount = Convert.ToInt32(this.PSecsDrv.S2F29.ECCOUNT.ToString());
                //받은 ECID중에 존재하지 않는것이 하나라도 있으면 L,0으로 보고한다.
                for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                {
                    dintECID = Convert.ToInt32(this.PSecsDrv.S2F29.get_ECID(dintLoop).ToString());
                    if (this.PInfo.Unit(0).SubUnit(0).ECID(dintECID) == null)
                    {
                        this.PSecsDrv.S2F30.MODULEID = dstrModuleID;
                        this.PSecsDrv.S2F30.ECCOUNT = 0;
                        this.PSecsDrv.S2F30.Reply(this.PSecsDrv.S2F29.Header);
                        return;
                    }
                }

                this.PSecsDrv.S2F30.MODULEID = dstrModuleID;

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS2F29): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S2F29를 받았음을 저장
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ECIDRead);      //ECID를 PLC로 부터 읽는다.
                subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S2F29를 받지 않았음을 저장(초기화)
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS2F29): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());


                //HOST로 부터 받은 List가 L, 0이면 모든 ECID 보고
                if (dintECIDCount == 0)
                {
                    this.PSecsDrv.S2F30.ECCOUNT = this.PInfo.Unit(0).SubUnit(0).ECIDCount;

                    for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop++)
                    {
                        this.PSecsDrv.S2F30.set_ECID(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).Index);
                        this.PSecsDrv.S2F30.set_ECNAME(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).Name);
                        this.PSecsDrv.S2F30.set_ECDEF(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECDEF);
                        this.PSecsDrv.S2F30.set_ECSLL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSLL);
                        this.PSecsDrv.S2F30.set_ECSUL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECSUL);
                        this.PSecsDrv.S2F30.set_ECWLL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWLL);
                        this.PSecsDrv.S2F30.set_ECWUL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintLoop).ECWUL);
                    }
                }
                else
                {
                    this.PSecsDrv.S2F30.ECCOUNT = dintECIDCount;

                    for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                    {
                        dintECID = Convert.ToInt32(this.PSecsDrv.S2F29.get_ECID(dintLoop).ToString());  //ECID 추출

                        this.PSecsDrv.S2F30.set_ECID(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).Index);
                        this.PSecsDrv.S2F30.set_ECNAME(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).Name);
                        this.PSecsDrv.S2F30.set_ECDEF(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECDEF);
                        this.PSecsDrv.S2F30.set_ECSLL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSLL);
                        this.PSecsDrv.S2F30.set_ECSUL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSUL);
                        this.PSecsDrv.S2F30.set_ECWLL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWLL);
                        this.PSecsDrv.S2F30.set_ECWUL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWUL);
                    }
                }

                this.PSecsDrv.S2F30.Reply(this.PSecsDrv.S2F29.Header);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S2F31()
        //  Description   : Host로부터 TimeDate를 받아서 PC에 세팅한다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S2F31 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S2F31()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S2F31");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS2F31()
        {
            string dstrData = "";
            int dintReply = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S2F31.Header) == true) return;


                dstrData = this.PSecsDrv.S2F31.TIME.Trim();

                //PC에 시간 설정
                DateAndTime.Today = DateTime.ParseExact(dstrData.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture);
                DateAndTime.TimeOfDay = DateTime.ParseExact(dstrData.Substring(8, 6), "HHmmss", CultureInfo.InvariantCulture);

                dintReply = 0;      //0=OK

                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.DateandTimeSetting, dstrData);
            }
            catch
            {
                dintReply = 1;      //1= Error, Not done
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.PSecsDrv.S2F32.ACKC2 = dintReply;
                this.PSecsDrv.S2F32.Reply(this.PSecsDrv.S2F31.Header);
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S2F41EQPCMD()
        //  Description   : Host로부터 On line 확인 Stream Function을 수신시 
        //                  On line Data를 송신 한다.(S1F1수신 후 S1F2 송신)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F2 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S2F41EQPCMD()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S2F41EQPCMD");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS2F41EQPCMD()
        {
            string dstrModuleID = "";
            string dstrRCMD = "";
            int dintHCACK = 0;
            int dintCPACK = 0;
            string dstrData = "";

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S2F41EQPCMD.Header) == true) return;

                dstrRCMD = this.PSecsDrv.S2F41EQPCMD.RCMD.ToString();
                dstrModuleID = this.PSecsDrv.S2F41EQPCMD.MODULEID.Trim();

                //RCMD가 존재하는 경우
                if (dstrRCMD == "51" || dstrRCMD == "52" || dstrRCMD == "53" || dstrRCMD == "54")
                {
                    //ModuleID가 존재하지 않는 경우
                    if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                    {
                        dintHCACK = 3;          //3 = At least one parameter invalid
                        dintCPACK = 2;          //2 = Illegal Value specified for CPVAL
                    }
                    else
                    {
                        switch (dstrRCMD)
                        {
                            case "51":   //Pause
                                if (this.PInfo.Unit(0).SubUnit(0).EQPProcessState == "6")   //현재 Pause인데 Pause명령이 온 경우
                                {
                                    dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                    dintCPACK = 0;              //0 = No Error
                                }

                                break;
                            case "52":   //Resume
                                if (this.PInfo.Unit(0).SubUnit(0).EQPProcessState != "6")  //Pause 상태가 아닌데 Resume이 온 경우
                                {
                                    dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                    dintCPACK = 0;              //0 = No Error
                                }

                                break;
                            case "53":   //PM
                                if (this.PInfo.Unit(0).SubUnit(0).EQPState == "3")          //PM인데 PM 명령이 온 경우
                                {
                                    dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                    dintCPACK = 0;              //0 = No Error
                                }

                                break;
                            case "54":   //Normal

                                if (this.PInfo.Unit(0).SubUnit(0).EQPState == "1")          //Normal인데 Normal 명령이 온 경우
                                {
                                    dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                    dintCPACK = 0;              //0 = No Error
                                }
                                else
                                {
                                    //현재 장비에 Heavy Alarm이 있는데 HOST에서 Normal이 오면 Nak로 응답.
                                    //if (this.PInfo.All.AlarmExist == true)
                                    //{
                                    //    foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                    //    {
                                    //        if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H")
                                    //        {
                                    //            dintHCACK = 2;              //2 = Cannot perform now (Hardware Problem), If equipment reply HCACK=2, then reason should be report as Alarm (S5F1)
                                    //            dintCPACK = 0;              //0 = No Error
                                    //        }
                                    //        break;               //forEach문을 빠져나간다.
                                    //    }
                                    //}

                                    ////현재 PM상태인데 HOST에서 Normal이 왔을때 장비에 Heavy 알람이 발생해 있으면 Fault지시를 내린다. 
                                    //foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                    //{
                                    //    if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H")
                                    //    {
                                    //        dbolHeavyAlarmExist = true;   //for문을 빠져나간다.
                                    //        break;
                                    //    }
                                    //}
                                }

                                break;
                        }
                    }

                    if (dintHCACK == 0 && dintCPACK == 0)
                    {
                        //여기까지 오면 모두 OK인 경우임.
                        if (dstrRCMD == "51" || dstrRCMD == "52")
                        {
                            for (int dintLoop = 0; dintLoop <= this.PInfo.UnitCount; dintLoop++)
                            {
                                this.PInfo.Unit(dintLoop).SubUnit(0).EQPProcessStateChangeBYWHO = "1";    //BY HOST

                                if (dstrRCMD == "51")       //Pause
                                {
                                    this.PInfo.Unit(dintLoop).SubUnit(0).EQPProcessStateLastCommand = "6";      //Pause가 왔음을 저장
                                }
                                else if (dstrRCMD == "52")  //Resume
                                {
                                    this.PInfo.Unit(dintLoop).SubUnit(0).EQPProcessStateLastCommand = "7";      //Resume이 왔음을 저장(Resume은 7로 그냥 사용함)
                                }
                            }

                            if (dstrRCMD == "51")       //Pause
                            {
                                dstrData = "6";
                            }
                            else if (dstrRCMD == "52")  //Resume
                            {
                                dstrData = "7";
                            }
                            this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.EQPProcessState, dstrData);    //EQP Process State 변경요청을 PLC로 써준다.
                        }
                        else
                        {
                            for (int dintLoop = 0; dintLoop <= this.PInfo.UnitCount; dintLoop++)
                            {
                                this.PInfo.Unit(dintLoop).SubUnit(0).EQPStateChangeBYWHO = "1";    //BY HOST

                                if (dstrRCMD == "53")       //PM
                                {
                                    this.PInfo.Unit(dintLoop).SubUnit(0).EQPStateLastCommand = "3";      //PM이 왔음을 저장
                                }
                                else if (dstrRCMD == "54")  //Normal
                                {
                                    this.PInfo.Unit(dintLoop).SubUnit(0).EQPStateLastCommand = "1";      //Normal이 왔음을 저장
                                }
                            }



                            if (dstrRCMD == "53")       //PM
                            {
                                dstrData = "3";
                                this.PInfo.All.PMCode = "9999";
                            }
                            else if (dstrRCMD == "54")  //Normal
                            {
                                dstrData = "1";
                            }
                            this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.EQPState, dstrData);    //EQP State 변경요청을 PLC로 써준다.
                        }
                    }

                }
                else   //RCMD가 존재하지 않는 경우
                {
                    //ModuleID가 존재하지 않는 경우
                    if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                    {
                        dintHCACK = 1;          //1 = Command does not exist
                        dintCPACK = 2;          //2 = Illegal Value specified for CPVAL
                    }
                    else   //ModuleID가 맞는 경우
                    {
                        dintHCACK = 1;          //1 = Command does not exist
                        dintCPACK = 0;          //0 = No Error
                    }
                }


                //HOST로 S2F42 보고
                this.PSecsDrv.S2F42_EQPCMDREPLY.RCMD = dstrRCMD;
                this.PSecsDrv.S2F42_EQPCMDREPLY.HCACK = dintHCACK;
                this.PSecsDrv.S2F42_EQPCMDREPLY.MODULEID = dstrModuleID;
                this.PSecsDrv.S2F42_EQPCMDREPLY.CPACK = dintCPACK;
                this.PSecsDrv.S2F42_EQPCMDREPLY.Reply(this.PSecsDrv.S2F41EQPCMD.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S2F41PROCCMD()
        //  Description   : Host로부터 On line 확인 Stream Function을 수신시 
        //                  On line Data를 송신 한다.(S1F1수신 후 S1F2 송신)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F2 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S2F41PROCCMD()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S2F41PROCCMD");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS2F41PROCCMD()
        {
            try
            {
                //    subReplyTrue();     //Reply가 들어왔음을 저장
                //    if (this.PInfo.All.HostConnect == false) return;

                //    ////구조체에 값을 저장한다.
                //    //this.PInfo.All.MDLN = this.PSecsDrv.S1F2OnLineData.MDLN.Trim();
                //    //this.PInfo.All.SOFTREV = this.PSecsDrv.S1F2OnLineData.SOFTREV.Trim();

                //    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S2F17DateandTimeRequest);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S3F1()
        //  Description   : Host로부터 Material State Request 요청시
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S3F1 수신(나중에 HOST, 장비하고 상의해봐야 함)
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S3F1()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S3F1");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS3F1()
        {
            string dstrModuleID = "";

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S3F1.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S3F1.MODULEID.Trim();
                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S3F2.MODULEID = dstrModuleID;
                    this.PSecsDrv.S3F2.LISTCOUNT = 0;
                    this.PSecsDrv.S3F2.Reply(this.PSecsDrv.S3F1.Header);
                    return;
                }

                this.PSecsDrv.S3F2.MODULEID = dstrModuleID;
                this.PSecsDrv.S3F2.LISTCOUNT = this.PInfo.Unit(0).SubUnit(0).MaterialCount ;

                for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).MaterialCount; dintLoop++)
                {
                    this.PSecsDrv.S3F2.set_M_ID(dintLoop, this.PInfo.Unit(0).SubUnit(0).Material(dintLoop).Name);
                    this.PSecsDrv.S3F2.set_M_TYPE(dintLoop, "Filter");
                    this.PSecsDrv.S3F2.set_LIBRARYID(dintLoop, "");
                    this.PSecsDrv.S3F2.set_MP_STATE(dintLoop, "");
                    this.PSecsDrv.S3F2.set_M_STATE(dintLoop, "");
                    this.PSecsDrv.S3F2.set_M_LOC(dintLoop, "");
                    this.PSecsDrv.S3F2.set_M_SIZE(dintLoop, "");
                    this.PSecsDrv.S3F2.set_DEVICELIST(dintLoop, 1);
                    this.PSecsDrv.S3F2.set_m_DEVICEID(dintLoop, 1, "");
                    this.PSecsDrv.S3F2.set_m_STEPID(dintLoop, 1, "");
                    this.PSecsDrv.S3F2.set_m_PPID(dintLoop, 1, "");
                }

                this.PSecsDrv.S3F2.Reply(this.PSecsDrv.S3F1.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S5F101()
        //  Description   : Host로부터 AlarmList 조회
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S5F101 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S5F101()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S5F101");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS5F101()
        {
            string dstrModuleID = "";
            int dintIndex = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S5F101.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S5F101.MODULEID.Trim();
                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S5F102.ALARMCOUNT = 0;
                    this.PSecsDrv.S5F102.Reply(this.PSecsDrv.S5F101.Header);
                    return;
                }

                this.PSecsDrv.S5F102.ALARMCOUNT = 0;
                if (this.PInfo.All.AlarmExist == false)
                {
                }
                else
                {
                    foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                    {
                        dintIndex = dintIndex + 1;
                        this.PSecsDrv.S5F102.ALARMCOUNT = dintIndex;

                        this.PSecsDrv.S5F102.set_ALCD(dintIndex, Convert.ToByte(128 + this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarm).AlarmCode));
                        this.PSecsDrv.S5F102.set_ALID(dintIndex, dintAlarm);
                        this.PSecsDrv.S5F102.set_ALTX(dintIndex, this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarm).AlarmDesc);
                        this.PSecsDrv.S5F102.set_ALTM(dintIndex, this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarm).AlarmOCCTime);
                        this.PSecsDrv.S5F102.set_MODULEID(dintIndex, this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarm).ModuleID);
                    }
                }

                this.PSecsDrv.S5F102.Reply(this.PSecsDrv.S5F101.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S5F2()
        //  Description   : Host로부터 On line 확인 Stream Function을 수신시 
        //                  On line Data를 송신 한다.(S1F1수신 후 S1F2 송신)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F2 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S5F2()
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S6F12()
        //  Description   : Host로부터 S6F12를 받는다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S6F11 수신(할 작업이 없음)
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S6F12()
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S6F14()
        //  Description   : Host로부터 On line 확인 Stream Function을 수신시 
        //                  On line Data를 송신 한다.(S1F1수신 후 S1F2 송신)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F78 수신(할 작업이 없음)
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S6F14()
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F1()
        //  Description   : Host로부터 Current PPID 변경 요청
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S7F101 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F1()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S7F1");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS7F1()
        {
            string dstrModuleID = "";
            int dintPPIDType = 0;
            string dstrPPID = "";
            int dintPPGNT = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S7F1.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S7F1.MODULEID.Trim();
                dstrPPID = this.PSecsDrv.S7F1.PPID.Trim();
                dintPPIDType = Convert.ToInt32(this.PSecsDrv.S7F1.PPID_TYPE);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S7F2.PPGNT = 2;       //2=ModuleID is not exist
                    this.PSecsDrv.S7F2.Reply(this.PSecsDrv.S7F1.Header);
                    return;
                }

                //PPID_TYPE이 틀린 경우(2가 아닌값)
                //S7F1은 PPID_TYPE=2만 내려온다.
                if (dintPPIDType == 2)
                {
                    if (this.PInfo.All.CurrentHOSTPPID == dstrPPID)
                    {
                        dintPPGNT = 1;      //1=Already have
                    }

                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F1): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F1를 받았음을 저장
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrPPID);      //HOST PPID 존재여부를 PLC에게 물어본다.
                    subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                    this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F1를 받지 않았음을 저장(초기화)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F1): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                                  

                    //if (this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrPPID) == null)
                    if (this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist == false)  //PLC로 부터 리턴받은 값
                    {
                        dintPPGNT = 5;      //5=Invalid PPID
                    }
                }
                else
                {
                    dintPPGNT = 4;      //4=PPID_TYPE is not match
                }

                this.PSecsDrv.S7F2.PPGNT = dintPPGNT;
                this.PSecsDrv.S7F2.Reply(this.PSecsDrv.S7F1.Header);

                //PLC로 Current PPID 변경지시를 내린다.
                if (dintPPGNT == 0)
                {
                    this.PInfo.All.EQPSpecifiedCtrlBYWHO = "1"; //BY HOST
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.CurrentPPIDChange, dstrPPID);
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //PLC로 부터 리턴받은 값에 대해 사용된 변수 초기화
                this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist = false;
                this.PInfo.All.ReceivedFromHOST_EQPPPIDExist = false;

                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F101()
        //  Description   : Host로부터 Current PPID List Request를 받았을때
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S7F101 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F101()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S7F101");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS7F101()
        {
            string dstrModuleID = "";
            int dintPPIDType = 0;
            int dintIndex = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S7F101.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S7F101.MODULEID.Trim();
                dintPPIDType = Convert.ToInt32(this.PSecsDrv.S7F101.PPID_TYPE);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S7F102.MODULEID = dstrModuleID;
                    this.PSecsDrv.S7F102.PPID_TYPE = dintPPIDType;
                    this.PSecsDrv.S7F102.PPIDCOUNT = 0;
                    this.PSecsDrv.S7F102.Reply(this.PSecsDrv.S7F101.Header);
                    return;
                }

                //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dintPPIDType == 1 || dintPPIDType == 2)
                {
                }
                else
                {
                    this.PSecsDrv.S7F102.MODULEID = dstrModuleID;
                    this.PSecsDrv.S7F102.PPID_TYPE = dintPPIDType;
                    this.PSecsDrv.S7F102.PPIDCOUNT = 0;
                    this.PSecsDrv.S7F102.Reply(this.PSecsDrv.S7F101.Header);
                    return;
                }

                //여기까지 오면 맞는 경우임
                this.PSecsDrv.S7F102.MODULEID = dstrModuleID;
                this.PSecsDrv.S7F102.PPID_TYPE = dintPPIDType;

              
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F101)-EQP, HOST PPID Set up: " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F101를 받았음을 저장
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID);      //현재 운영중인 PPID를 PLC로 부터 읽는다.
                subWaitDuringReadFromPLC();
                this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F101를 받지 않았음을 저장(초기화)
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F101)-EQP, HOST PPID Set up: " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                

                if (dintPPIDType == 1)
                {
                    this.PSecsDrv.S7F102.PPIDCOUNT = this.PInfo.Unit(0).SubUnit(0).EQPPPIDCount;

                    foreach (string dstrEQPPPID in this.PInfo.Unit(0).SubUnit(0).EQPPPID())
                    {
                        dintIndex = dintIndex + 1;  //보고 Index 증가
                        this.PSecsDrv.S7F102.set_PPID(dintIndex, dstrEQPPPID);
                    }
                }
                else if (dintPPIDType == 2)
                {
                    this.PSecsDrv.S7F102.PPIDCOUNT = this.PInfo.Unit(0).SubUnit(0).HOSTPPIDCount;

                    foreach (string dstrHOSTPPID in this.PInfo.Unit(0).SubUnit(0).HOSTPPID())
                    {
                        dintIndex = dintIndex + 1;  //보고 Index 증가
                        this.PSecsDrv.S7F102.set_PPID(dintIndex, dstrHOSTPPID);
                    }
                }

                this.PSecsDrv.S7F102.Reply(this.PSecsDrv.S7F101.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F103()
        //  Description   : Host로부터 PPID Existence Check
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S7F103 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F103()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S7F103");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS7F103()
        {
            string dstrModuleID = "";
            int dintPPIDType = 0;
            int dintACK = 0;
            string dstrReceivedPPID = "";

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S7F103.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S7F103.MODULEID.Trim();
                dstrReceivedPPID = this.PSecsDrv.S7F103.PPID.Trim();
                dintPPIDType = Convert.ToInt32(this.PSecsDrv.S7F103.PPID_TYPE);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S7F104.ACKC7 = 1;
                    this.PSecsDrv.S7F104.Reply(this.PSecsDrv.S7F103.Header);
                    return;
                }

                //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dintPPIDType == 1 || dintPPIDType == 2)
                {
                  
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F103): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F103를 받았음을 저장
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrReceivedPPID);       //PPID 존재여부를 PLC에게 물어본다.
                    subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                    this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F103를 받지 않았음을 저장(초기화)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F103): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    
                   
                    if (dintPPIDType == 1)
                    {
                        //if (this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_EQPPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dintACK = 3;        //3=PPID is not match
                        }
                    }
                    else if (dintPPIDType == 2)
                    {
                        //if (this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dintACK = 3;        //3=PPID is not match
                        }
                    }
                }
                else
                {
                    dintACK = 2;
                }

                this.PSecsDrv.S7F104.ACKC7 = dintACK;
                this.PSecsDrv.S7F104.Reply(this.PSecsDrv.S7F103.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //PLC로 부터 리턴받은 값에 대해 사용된 변수 초기화
                this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist = false;
                this.PInfo.All.ReceivedFromHOST_EQPPPIDExist = false;

                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F105()
        //  Description   : Host로부터 PPID SoftRev, DateTime 체크
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S1F2 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F105()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S7F105");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS7F105()
        {
            string dstrModuleID = "";
            int dintPPIDType = 0;
            int dintACK = 0;
            string dstrReceivedPPID = "";
            string dstrSOFTRev = "";
            string dstrTime = "";

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S7F105.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S7F105.MODULEID.Trim();
                dstrReceivedPPID = this.PSecsDrv.S7F105.PPID.Trim();
                dintPPIDType = Convert.ToInt32(this.PSecsDrv.S7F105.PPID_TYPE);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S7F106.ACK7 = 1;
                    this.PSecsDrv.S7F106.MODULEID = dstrModuleID;
                    this.PSecsDrv.S7F106.PPID = dstrReceivedPPID;
                    this.PSecsDrv.S7F106.SOFTREV = "";
                    this.PSecsDrv.S7F106.PPID_TYPE = dintPPIDType;
                    this.PSecsDrv.S7F106.TIME = "";
                    this.PSecsDrv.S7F106.Reply(this.PSecsDrv.S7F105.Header);
                    return;
                }

                //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dintPPIDType == 1 || dintPPIDType == 2)
                {
                  
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F105): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F105를 받았음을 저장
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrReceivedPPID);       //PPID 존재여부를 PLC에게 물어본다.
                    subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                    this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F105를 받지 않았음을 저장(초기화)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F105): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    

                    if (dintPPIDType == 1)
                    {
                        //if (this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_EQPPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dintACK = 3;        //3=PPID is not match
                            dstrSOFTRev = "";
                            dstrTime = "";
                        }
                        else   //맞는 경우임
                        {
                            dintACK = 0;
                            dstrSOFTRev = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDVer;
                            dstrTime = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).DateTime;
                        }
                    }
                    else if (dintPPIDType == 2)
                    {
                        //if (this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dintACK = 3;        //3=PPID is not match
                            dstrSOFTRev = "";
                            dstrTime = "";
                        }
                        else   //맞는 경우임
                        {
                            dintACK = 0;
                            dstrSOFTRev = this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).PPIDVer;
                            dstrTime = this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).DateTime;
                        }
                    }
                }
                else
                {
                    dintACK = 2;
                    dstrSOFTRev = "";
                    dstrTime = "";
                }

                this.PSecsDrv.S7F106.ACK7 = dintACK;
                this.PSecsDrv.S7F106.MODULEID = dstrModuleID;
                this.PSecsDrv.S7F106.PPID = dstrReceivedPPID;
                this.PSecsDrv.S7F106.SOFTREV = dstrSOFTRev;
                this.PSecsDrv.S7F106.PPID_TYPE = dintPPIDType;
                this.PSecsDrv.S7F106.TIME = dstrTime;
                this.PSecsDrv.S7F106.Reply(this.PSecsDrv.S7F105.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //PLC로 부터 리턴받은 값에 대해 사용된 변수 초기화
                this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist = false;
                this.PInfo.All.ReceivedFromHOST_EQPPPIDExist = false;

                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F108()
        //  Description   : PPID Create, Delete & PP Body Modify 보고(S7F107)에 대한 응답
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : None
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F108()
        {
            try
            {
                subReplyTrue();     //Reply가 들어왔음을 저장
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F109()
        //  Description   : Host로부터 Current Running PPID Req.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S7F109 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F109()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S7F109");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS7F109()
        {
            string dstrModuleID = "";
            string dstrPPID = "";
            int dintPPIDType = 0;
            int dintAck = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S7F109.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S7F109.MODULEID.Trim();
                dintPPIDType = Convert.ToInt32(this.PSecsDrv.S7F109.PPID_TYPE);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S7F110.ACK = 1;
                    this.PSecsDrv.S7F110.MODULEID = dstrModuleID;
                    this.PSecsDrv.S7F110.PPID = "";
                    this.PSecsDrv.S7F110.PPID_TYPE = dintPPIDType;
                    this.PSecsDrv.S7F110.Reply(this.PSecsDrv.S7F109.Header);
                    return;
                }

                //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dintPPIDType == 1 || dintPPIDType == 2)
                {
                   
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F109): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F109를 받았음을 저장
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.CurrentRunningPPPIDReadFromHOST, dintPPIDType);      //현재 운영중인 PPID를 PLC로 부터 읽는다.
                    subWaitDuringReadFromPLC();
                    this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F109를 받지 않았음을 저장(초기화)
                    

                    if (dintPPIDType == 1)   //EQPPID
                    {
                        dstrPPID = this.PInfo.All.CurrentEQPPPID;
                    }
                    else if (dintPPIDType == 2) //HOSTPPID
                    {
                        dstrPPID = this.PInfo.All.CurrentHOSTPPID;
                    }

                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F109): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());

                    dintAck = 0;
                }
                else
                {
                    dstrPPID = "";
                    dintAck = 2;
                }

                this.PSecsDrv.S7F110.ACK = dintAck;
                this.PSecsDrv.S7F110.MODULEID = dstrModuleID;
                this.PSecsDrv.S7F110.PPID = dstrPPID;
                this.PSecsDrv.S7F110.PPID_TYPE = dintPPIDType;
                this.PSecsDrv.S7F110.Reply(this.PSecsDrv.S7F109.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F23()
        //  Description   : Host로부터 PPID Body 수정 요청이 올때
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S7F23 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F23()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S7F23");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS7F23()
        {
            string dstrModuleID = "";
            int dintPPIDType = 0;
            string dstrReceivedPPID = "";
            int dintACK = 0;
            int dintBodyIndex = 0;
            string dstrTemp = "";
            string[] dstrValue;

            string dstrReceivedSOFTREV = "";
            string dstrBeforeEQPPPID = "";
            int dintPPIDBodyCount = 0;
            int dintPPIDBodyID = 0;

            string dstrHOSTPPID = "";
            string dstrPPIDBody = "";
            string dstrPPIDVer = "";
            string dstrPPIDTime = "";
            string dstrEQPPPID = "";

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S7F23.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S7F23.MODUELID.Trim();
                dstrReceivedPPID = this.PSecsDrv.S7F23.PPID.Trim();
                dstrReceivedSOFTREV = this.PSecsDrv.S7F23.SOFT_REV.Trim();  //받은것은 무시한다.
                dintPPIDType = Convert.ToInt32(this.PSecsDrv.S7F23.PPID_TYPE);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S7F24.ACKC7 = 9;
                    this.PSecsDrv.S7F24.Reply(this.PSecsDrv.S7F23.Header);
                    return;
                }


                switch (dintPPIDType)
                {
                    case 1:

                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F23를 받았음을 저장
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrReceivedPPID);       //EQP PPID 존재여부를 PLC에게 물어본다.
                        subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                        this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F23를 받지 않았음을 저장(초기화)
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        

                        //EQP PPID 존재여부 체크
                        //if (this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_EQPPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dintACK = 12;
                        }


                     
                        //먼저 현재 운영중인 EQP PPID를 읽는다.
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F23를 받았음을 저장
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.CurrentRunningPPPIDReadFromHOST, dintPPIDType);      //현재 운영중인 PPID를 PLC로 부터 읽는다.
                        subWaitDuringReadFromPLC();
                        this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F23를 받지 않았음을 저장(초기화)
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        
                      
                        //EQP PPID가 현재 사용중인지 체크
                        if (dintACK == 0 && this.PInfo.All.CurrentEQPPPID == dstrReceivedPPID)
                        {
                            dintACK = 1;
                        }

                        //PPID Body 개수 체크
                        dintPPIDBodyCount = Convert.ToInt32(this.PSecsDrv.S7F23.get_PPARMCOUNT(1));
                        if (dintACK == 0 && dintPPIDBodyCount > this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount)
                        {
                            dintACK = 3;
                        }

                        //PPID Body 범위 체크(NAK시 3으로 보냄)
                        if (dintACK == 0)
                        {
                            for (int dintLoop = 1; dintLoop <= dintPPIDBodyCount; dintLoop++)
                            {
                                dintPPIDBodyID = funGetBodyNameToBodyID(this.PSecsDrv.S7F23.get_P_PARM_NAME(1, dintLoop).ToString().Trim());
                                if (dintPPIDBodyID == 0)  //Body Name이 존재하지 않는 경우 NAK
                                {
                                    dintACK = 3;
                                    break;
                                }
                                dstrPPIDBody = this.PSecsDrv.S7F23.get_P_PARM(1, dintLoop).Trim();

                                if (this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Min <= Convert.ToDouble(dstrPPIDBody) &&
                                    Convert.ToDouble(dstrPPIDBody) <= this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Max)
                                {
                                }
                                else
                                {
                                    dintACK = 3;   //PPID Body 범위를 벗어난 경우(NAK시 3으로 보냄)
                                    break;
                                }
                            }
                        }


                        //여기까지 오면 정상임
                        if (dintACK == 0)
                        {
                            dstrHOSTPPID = "";
                            dstrPPIDBody = "";

                            //조금 전 위에서 PLC에서 읽은 Data를 구성한다.
                            for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                            {
                                dstrTemp = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop).Value;
                                if (this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format.Contains(".") == true)
                                {
                                    dstrTemp = Convert.ToString(Convert.ToSingle(dstrTemp) * 10);
                                }
                                else
                                {
                                    dstrTemp = Convert.ToString(Convert.ToSingle(dstrTemp));
                                }
                                dstrPPIDBody = dstrPPIDBody + dstrTemp + ";";
                            }

                            //PLC에서 읽은 Data 외에 HOST에서 받은(변경할) Data 저장
                            dstrValue = dstrPPIDBody.Split(new char[] { ';' });      //인자들을 분리한다.
                            for (int dintLoop = 1; dintLoop <= dintPPIDBodyCount; dintLoop++)
                            {
                                dintBodyIndex = funGetBodyNameToBodyID(this.PSecsDrv.S7F23.get_P_PARM_NAME(1, dintLoop).ToString().Trim());
                                dstrTemp = this.PSecsDrv.S7F23.get_P_PARM(1, dintLoop).ToString().Trim();
                                if (this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintBodyIndex).Format.Contains(".") == true)
                                {
                                    dstrTemp = Convert.ToString(Convert.ToSingle(dstrTemp) * 10);
                                }
                                else
                                {
                                    dstrTemp = Convert.ToString(Convert.ToSingle(dstrTemp));
                                }
                                dstrValue[dintBodyIndex - 1] = dstrTemp;
                            }



                            dstrPPIDBody = "";
                            for (int dintLoop = 1; dintLoop < dstrValue.Length; dintLoop++)
                            {
                                dstrPPIDBody = dstrPPIDBody + dstrValue[dintLoop - 1] + ";";
                            }



                            dstrPPIDTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            dstrPPIDVer = Convert.ToString(Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDVer) + 1);
                            dintPPIDType = 1;   //EQP PPID임.

                            //장비로 EQP PPID Body 수정 지시(PPID Type=1)
                            //인자: HOSTPPID, EQPPPID, Body, 시간, Version, PPID Type
                            this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.PPIDBodyModify, dstrHOSTPPID, dstrReceivedPPID, dstrPPIDBody, dstrPPIDTime, dstrPPIDVer, dintPPIDType.ToString());
                        }

                        break;

                    case 2:

                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F23를 받았음을 저장
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrReceivedPPID);       //HOST PPID 존재여부를 PLC에게 물어본다.
                        subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                        this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F23를 받지 않았음을 저장(초기화)
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        
                     
                        //HOST PPID 존재여부 체크
                        //if (this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dintACK = 12;
                        }

                        //받은 HOST PPID에 대해 EQPPPID가 존재하는지 체크
                        dstrEQPPPID = this.PSecsDrv.S7F23.get_P_PARM(1, 1).ToString().Trim();

                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F23를 받았음을 저장
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, 1, dstrEQPPPID);       //EQP PPID 존재여부를 PLC에게 물어본다.
                        subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                        this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F23를 받지 않았음을 저장(초기화)
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        
                      
                        //HOST PPID에 Mapping되어 있는 EQPPPID가 존재하는지 체크
                        //if (dintACK == 0 && this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).EQPPPID == null)
                        if (dintACK == 0 && this.PInfo.All.ReceivedFromHOST_EQPPPIDExist == false)
                        {
                            dintACK = 12;
                        }


                        //먼저 현재 운영중인 HOST PPID를 저장한다.
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F23를 받았음을 저장
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.CurrentRunningPPPIDReadFromHOST, dintPPIDType);      //현재 운영중인 PPID를 PLC로 부터 읽는다.
                        subWaitDuringReadFromPLC();
                        this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F23를 받지 않았음을 저장(초기화)
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F23): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                        
                       
                        if (dintACK == 0 && this.PInfo.All.CurrentHOSTPPID == dstrReceivedPPID)   //HOST PPID가 현재 사용중인지 체크
                        {
                            dintACK = 1;
                        }

                        //여기까지 오면 정상임
                        if (dintACK == 0)
                        {
                            dstrBeforeEQPPPID = this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).EQPPPID;   //변경전 EQPPPID
                            //dstrEQPPPID = this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).EQPPPID;

                            dstrPPIDTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            dstrPPIDVer = Convert.ToString(Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).PPIDVer) + 1);
                            dintPPIDType = 2;   //HOST PPID임.

                            //장비로 HOST PPID Mapping 변경 지시(PPID Type=2)
                            //인자: HOSTPPID, 변경전 EQPPPID, 변경후 EQPPPID, 시간, Version)
                            this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.HOSTPPIDMappingChange, dstrReceivedPPID, dstrBeforeEQPPPID, dstrEQPPPID, dstrPPIDTime, dstrPPIDVer);
                        }

                        break;

                    default:  //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                        dintACK = 10;

                        break;
                }


                this.PSecsDrv.S7F24.ACKC7 = dintACK;
                this.PSecsDrv.S7F24.Reply(this.PSecsDrv.S7F23.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //PLC로 부터 리턴받은 값에 대해 사용된 변수 초기화
                this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist = false;
                this.PInfo.All.ReceivedFromHOST_EQPPPIDExist = false;

                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        //*******************************************************************************
        //  Function Name : funGetBodyNameToBodyID()
        //  Description   : HOST로 부터 내려온 Body Name이 맞는지 검사
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : None
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private int funGetBodyNameToBodyID(string dstrName)
        {
            int dintBodyID = 0;

            try
            {
                foreach (int dintIndex in this.PInfo.Unit(0).SubUnit(0).PPIDBody())
                {
                    if (this.PInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name == dstrName)
                    {
                        dintBodyID = dintIndex;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintBodyID;
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F25()
        //  Description   : Host로부터 PPID 정보 요청
        //                  (A request of EQ's Formatted Process Program parameter)
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S7F25 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F25()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S7F25");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            
        }

        private void subS7F25()
        {
            string dstrModuleID = "";
            string dstrReceivedPPID = "";
            int dintPPIDType = 0;
            Boolean dbolPPIDNotExist = false;
            string dstrPPIDVer = "";
            int dintEQPPIDCount = 0;        //HOST PPID에 Mapping되어있는 EQPPID의 개수

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S7F25.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S7F25.MODULEID.Trim();
                dstrReceivedPPID = this.PSecsDrv.S7F25.PPID.Trim();
                dintPPIDType = Convert.ToInt32(this.PSecsDrv.S7F25.PPID_TYPE);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S7F26.MODUELID = dstrModuleID;
                    this.PSecsDrv.S7F26.PPID = dstrReceivedPPID;
                    this.PSecsDrv.S7F26.SOFT_REV = "";
                    this.PSecsDrv.S7F26.PPID_TYPE = dintPPIDType;
                    this.PSecsDrv.S7F26.COMMANDCOUNT = 0;
                    this.PSecsDrv.S7F26.Reply(this.PSecsDrv.S7F25.Header);
                    return;
                }


                if (dintPPIDType == 1 || dintPPIDType == 2)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F25): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F25를 받았음을 저장
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrReceivedPPID);       //PPID 존재여부를 PLC에게 물어본다.
                    subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                    this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F25를 받지 않았음을 저장(초기화)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F25): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    
                 
                    if (dintPPIDType == 1)   //EQPPID
                    {
                        //if (this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_EQPPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dbolPPIDNotExist = true;
                        } 
                    }
                    else
                    {
                        //if (this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dbolPPIDNotExist = true;
                        } 
                    }
                }
                else
                {
                    dbolPPIDNotExist = true;
                }


                //PPID가 존재하지 않는 경우나 PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dbolPPIDNotExist == true)
                {
                    this.PSecsDrv.S7F26.MODUELID = dstrModuleID;
                    this.PSecsDrv.S7F26.PPID = dstrReceivedPPID;
                    this.PSecsDrv.S7F26.SOFT_REV = "";
                    this.PSecsDrv.S7F26.PPID_TYPE = dintPPIDType;
                    this.PSecsDrv.S7F26.COMMANDCOUNT = 0;
                    this.PSecsDrv.S7F26.Reply(this.PSecsDrv.S7F25.Header);

                    return;
                }

                //여기까지 오면 맞는 경우임
                this.PSecsDrv.S7F26.MODUELID = dstrModuleID;
                this.PSecsDrv.S7F26.PPID = dstrReceivedPPID;
                this.PSecsDrv.S7F26.PPID_TYPE = dintPPIDType;
                this.PSecsDrv.S7F26.COMMANDCOUNT = 1;


                //EQP, HOST PPID Set up
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F25)-EQP, HOST PPID Set up: " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F25를 받았음을 저장
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID);      //현재 운영중인 EQP PPID를 PLC로 부터 읽는다.
                subWaitDuringReadFromPLC();
                this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F25를 받지 않았음을 저장(초기화)
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F25)-EQP, HOST PPID Set up: " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                
              

                if (dintPPIDType == 1)   //EQPPID
                {
                    this.PSecsDrv.S7F26.set_CCODE(1, 0);     //CCODE는 0으로
                    this.PSecsDrv.S7F26.set_PPARMCOUNT(1, this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBodyCount);
                    for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBodyCount; dintLoop++)
                    {
                        this.PSecsDrv.S7F26.set_P_PARM_NAME(1, dintLoop, this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop).Name);
                        this.PSecsDrv.S7F26.set_P_PARM(1, dintLoop, this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop).Value);
                    }

                    dstrPPIDVer = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDVer;
                }
                else if (dintPPIDType == 2) //HOSTPPID
                {
                    //HOST PPID에 Mapping되어있는 장비 PPID의 개수를 알아온다.
                    foreach (string dstrHOSTPPID in this.PInfo.Unit(0).SubUnit(0).HOSTPPID())
                    {
                        if (dstrReceivedPPID == dstrHOSTPPID && this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).EQPPPID != "")
                        {
                            dintEQPPIDCount = dintEQPPIDCount + 1;
                        }
                    }

                    this.PSecsDrv.S7F26.set_CCODE(dintEQPPIDCount, 0);     //CCODE는 0으로
                    this.PSecsDrv.S7F26.set_PPARMCOUNT(dintEQPPIDCount, dintEQPPIDCount);
                    for (int dintLoop = 1; dintLoop <= dintEQPPIDCount; dintLoop++)
                    {
                        this.PSecsDrv.S7F26.set_P_PARM_NAME(dintEQPPIDCount, dintLoop, "SUBPPID");
                        this.PSecsDrv.S7F26.set_P_PARM(dintEQPPIDCount, dintLoop, this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).EQPPPID);
                    }

                    dstrPPIDVer = this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).PPIDVer;
                }

                this.PSecsDrv.S7F26.SOFT_REV = dstrPPIDVer;
                this.PSecsDrv.S7F26.Reply(this.PSecsDrv.S7F25.Header);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        //*******************************************************************************
        //  Function Name : PSecsDrv_S7F33()
        //  Description   : HOST로 부터 내려온 PPID가 사용가능한지 체크
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S7F33 수신
        //*******************************************************************************
        //  2006/11/01          김 효주         [L 00]
        //******************************************************************************* 
        private void PSecsDrv_S7F33()
        {
            try
            {
                this.PInfo.subReceiveHostSF_Set("S7F33");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subS7F33()
        {
            string dstrModuleID = "";
            int dintPPIDType = 0;
            string dstrReceivedPPID = "";
            int dintFRMLEN = 0;

            try
            {
                if (this.PInfo.All.HostConnect == false) return;
                if (funACTSECSAort_Send(this.PSecsDrv.S7F33.Header) == true) return;

                dstrModuleID = this.PSecsDrv.S7F33.MODULEID.Trim();
                dstrReceivedPPID = this.PSecsDrv.S7F33.PPID.Trim();
                dintPPIDType = Convert.ToInt32(this.PSecsDrv.S7F33.PPID_TYPE);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.PInfo.Unit(0).SubUnit(0).ModuleID)
                {
                    this.PSecsDrv.S7F34.PPID = dstrReceivedPPID;
                    this.PSecsDrv.S7F34.UNFLEN = 0;
                    this.PSecsDrv.S7F34.FRMLEN = 3;
                    this.PSecsDrv.S7F34.Reply(this.PSecsDrv.S7F33.Header);
                    return;
                }

                //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dintPPIDType == 1 || dintPPIDType == 2)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F33): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    this.PInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F33를 받았음을 저장
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrReceivedPPID);       //PPID 존재여부를 PLC에게 물어본다.
                    subWaitDuringReadFromPLC();                      //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                    this.PInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F33를 받지 않았음을 저장(초기화)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F33): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    

                    if (dintPPIDType == 1)
                    {
                        //if (this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_EQPPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dintFRMLEN = 5;
                        }
                        else   //맞는 경우임
                        {
                            dintFRMLEN = 0;
                        }
                    }
                    else if (dintPPIDType == 2)
                    {
                        //if (this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                        if (this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist == false)  //PLC로 부터 리턴받은 값
                        {
                            dintFRMLEN = 5;
                        }
                        else   //맞는 경우임
                        {
                            dintFRMLEN = 0;
                        }
                    }
                }
                else
                {
                    dintFRMLEN = 4;
                }

                this.PSecsDrv.S7F34.PPID = dstrReceivedPPID;
                this.PSecsDrv.S7F34.UNFLEN = 0;
                this.PSecsDrv.S7F34.FRMLEN = dintFRMLEN;
                this.PSecsDrv.S7F34.Reply(this.PSecsDrv.S7F33.Header);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //PLC로 부터 리턴받은 값에 대해 사용된 변수 초기화
                this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist = false;
                this.PInfo.All.ReceivedFromHOST_EQPPPIDExist = false;

                this.PInfo.All.isReceivedFromHOST = false;      //초기화
                this.PInfo.All.PLCActionEnd = false;            //초기화
            }
        }

        #endregion

        #endregion

        //HOST로 메세지를 전송하기위한 Thread 정의
        #region "Timer, Thread 정의"

        //*******************************************************************************
        //  Function Name : funThreadInitial()
        //  Description   : 구조체에 있는 HOST로 송신할 S/F을 검사하여 HOST 송신 자체 
        //                  Queue에 넣기위한 Thread 정의
        //  Parameters    : 
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00] 
        //*******************************************************************************                 
        public Boolean funThreadInitial()
        {
            try
            {
                this.pThreadSFSend = new Thread(new ThreadStart(SFSendThread));
                this.pThreadSFSend.Name = "SFSendThread";
                this.pThreadSFSend.Start();

                this.pThreadSFReceive = new Thread(new ThreadStart(SFReceiveThread));
                this.pThreadSFReceive.Name = "SFReceiveThread";
                this.pThreadSFReceive.Start();

                this.pThreadSVIDSend = new Thread(new ThreadStart(subSVIDSend));
                this.pThreadSVIDSend.Name = "subSVIDSend";
                this.pThreadSVIDSend.Start();

                return true;
            }
            catch(Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
        }

        //*******************************************************************************
        //  Function Name : SFSendThread()
        //  Description   : HOST로 전송할 메세지가 있으면 메세지를 송신한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00] 
        //*******************************************************************************               
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

        public void subSVIDSend()
        {
            //object dobjData;

            do
            {
                try
                {
                    try
                    {

                        //if (this.PInfo.funGetSendSFCount() > 0)               //구조체의 S/F 송신 Queue에 내용이 없으면 그냥 나간다.
                        //{
                        //    dobjData = this.PInfo.funGetSendSF();             //Queue에서 HOST로 송신할 S/F을 가져온다.
                        //    if (dobjData != null)
                        //    {
                        //        subSendMessage(dobjData);    //HOST로 메세지 송신
                        //    }
                        //}

                        SVIDSend();

                        Thread.Sleep(1);  //50
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

        //*******************************************************************************
        //  Function Name : SFReceiveThread()
        //  Description   : HOST로 수신한 메세지를 처리한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/12/11          어 경태         [L 00] 
        //*******************************************************************************               
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
                                subReceiveMessage(dobjData.ToString());
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


        //*******************************************************************************
        //  Function Name : subCreateSF()
        //  Description   : HOST로 전송할 SF Object를 생성해서 구조체에 저장
        //  Parameters    : dstrParameter: SF 구분자, 인자
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/04/09          김 효주         [L 00]
        //******************************************************************************* 
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

        //*******************************************************************************
        //  Function Name : subSendMessage()
        //  Description   : HOST로 메세지를 송신한다.(Primary만 여기서 전송하고
        //                  Secondary메세지는 HOST로부터 Primary가 왔을때 해당
        //                  Event를 받는 메소드에서 전송한다.
        //  Parameters    : 
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2007/01/25          김 효주         [L 00]
        //******************************************************************************* 
        public void subSendMessage(object objSF)
        {
            string dstrSF = "";

            try
            {
                dstrSF = objSF.GetType().Name;
                switch (dstrSF)
                {
                    case "CAreYouThereClass":
                        subACTAreYouThere_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS5F1Class":
                        subACTS5F1_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS6F1Class":
                        subACTS6F1_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS6F11RelatedPanelProcessClass":
                        subACTS6F11RelatedPanelProcess_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS6F11RelatedEQPEventClass":
                        subACTS6F11RelatedEQPEvent_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS6F11RelatedEQPParameterClass":
                        subACTS6F11RelatedEQPParameter_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS6F11EQPSpecifiedCtrlClass":
                        subACTSS6F11EQPSpecifiedCtrl_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS6F11RelatedMaterialEventClass":
                        subACTSS6F11S6F11RelatedMaterialEvent_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS6F13GLSAPDClass":
                        subACTS6F13GLSAPD_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS6F13LOTAPDClass":
                        subACTS6F13LOTAPD_Send(objSF);
                        //subReplyCheck();
                        break;

                    case "CS7F107Class":
                        subACTS7F107_Send(objSF);
                        //subReplyCheck();
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                //스레드 종료시 네이티브프레임 호출에러때문에 로그 주석처리
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subReceiveMessage()
        //  Description   : 호스트로투터 수신한 메세지를 처리한다.
        //  Parameters    : 
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2007/12/11          어 경태         [L 00]
        //******************************************************************************* 
        public void subReceiveMessage(string strSF)
        {
            string dstrSF = "";

            try
            {
                dstrSF = strSF;
                switch (dstrSF)
                {
                    case "S10F3":
                        subS10F3();    //처리할 함수 호출
                        break;

                    case "S10F9":
                        subS10F9();    //처리할 함수 호출
                        break;

                    case "S1F1":
                        subS1F1();    //처리할 함수 호출
                        break;

                    case "S1F11":
                        subS1F11();    //처리할 함수 호출
                        break;

                    case "S1F15":
                        subS1F15();    //처리할 함수 호출
                        break;

                    case "S1F17":
                        subS1F17();    //처리할 함수 호출
                        break;

                    case "S1F2":
                        subS1F2();    //처리할 함수 호출

                        break;
                    case "S1F3":
                        subS1F3();    //처리할 함수 호출
                        break;

                    case "S1F5":
                        subS1F5();    //처리할 함수 호출
                        break;

                    case "S2F101":
                        subS2F101();    //처리할 함수 호출
                        break;

                    case "S2F103":
                        subS2F103();    //처리할 함수 호출
                        break;

                    case "S2F15":
                        subS2F15();    //처리할 함수 호출
                        break;

                    case "S2F23":
                        subS2F23();    //처리할 함수 호출
                        break;

                    case "S2F29":
                        subS2F29();    //처리할 함수 호출
                        break;

                    case "S2F31":
                        subS2F31();    //처리할 함수 호출
                        break;

                    case "S2F41EQPCMD":
                        subS2F41EQPCMD();    //처리할 함수 호출
                        break;

                    case "S2F41PROCCMD":
                        subS2F41PROCCMD();    //처리할 함수 호출
                        break;

                    case "S3F1":
                        subS3F1();    //처리할 함수 호출
                        break;

                    case "S5F101":
                        subS5F101();    //처리할 함수 호출
                        break;

                    case "S7F1":
                        subS7F1();    //처리할 함수 호출
                        break;

                    case "S7F101":
                        subS7F101();    //처리할 함수 호출
                        break;

                    case "S7F103":
                        subS7F103();    //처리할 함수 호출
                        break;

                    case "S7F105":
                        subS7F105();    //처리할 함수 호출
                        break;

                    case "S7F109":
                        subS7F109();    //처리할 함수 호출
                        break;

                    case "S7F23":
                        subS7F23();    //처리할 함수 호출
                        break;

                    case "S7F25":
                        subS7F25();    //처리할 함수 호출
                        break;

                    case "S7F33":
                        subS7F33();    //처리할 함수 호출
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                //스레드 종료시 네이티브프레임 호출에러때문에 로그 주석처리
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subReplyCheck()
        //  Description   : HOST로 부터 Reply가 들어왔음을 체크한다.
        //  Parameters    : 
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2007/01/25          김 효주         [L 00]
        //******************************************************************************* 
        private void subReplyCheck()
        {
            long dlngTimeTick = 0;
            long dlngSec = 0;

            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false)
                {
                    subReplyTrue();
                    return;
                }

                this.pbolReplyCheck = false;
                dlngTimeTick = DateTime.Now.Ticks;
                while (this.pbolReplyCheck == false)
                {
                    dlngSec = DateTime.Now.Ticks - dlngTimeTick;
                    if (dlngSec < 0) dlngTimeTick = 0;
                    if (dlngSec > this.PInfo.All.T9 * 10000000 || this.PInfo.All.ControlState == "1" || this.pbolReplyCheck == true)
                    {
                        subReplyTrue();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subReplyTrue()
        //  Description   : HOST로 부터 Reply가 들어왔음을 저장한다.
        //  Parameters    : 
        //  Return Value  : None
        //  Special Notes : 
        //*******************************************************************************
        //  2007/01/25          김 효주         [L 00]
        //******************************************************************************* 
        private void subReplyTrue()
        {
            try
            {
                this.pbolReplyCheck = true;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }


        //*******************************************************************************
        //  Function Name : funCreateSFInstance()
        //  Description   : S/F의 인스턴스를 생성하여 HOST 자체 Queue에 저장한다.
        //  Parameters    : strSFName => S/F 이름
        //  Return Value  : object => 생성한 S/F의 인스턴스
        //  Special Notes : 각 SF별 (가변)인자 설명, 인자는 콤마로 분리
        //                  S1F1AreYouThereRequest      => None
        //                  S1F13EstablishCommRequest   => None
        //                  S1F65EquipmentStatusChange  => dintUnitID
        //                  S1F69EquipmentModeChange    => None
        //                  S1F73PortStatusChange       => dintPortID
        //                  S1F77PortTypeChange         => dintPortID
        //                  S1F81LotStatusChangePROC    => dintPortID
        //                  S1F83LotStatusChangeCAEN    => dintPortID
        //                  S1F85LotStatusChangePREN    => dintPortID
        //                  S1F87LotStatusChangeEMPT    => dintPortID
        //                  S2F17DateandTimeRequest     => None
        //                  S2F25LoopBackSend           => strLoopback(LoopBack 문자열)
        //                  S5F65AlarmReportSend        => dintUnitID, dintAlarmID
        //                  S6F65EDCDataReport          => dstrREPUNIT, dintUnitID, dintLOTIndex, dintSlotID
        //                  S6F71ReticleStatusChange    => dintUnitID
        //                  S6F73MaterialStatusChange   => dintUnitID
        //                  S6F77ScrapGlassReport       => dintUnitID, dintLOTIndex, dintSlotID
        //                  S7F65CSTMapDownload         => dintPortID
        //                  S7F71OperationStartConfirm  => dintPortID
        //                  S7F73PPIDCreate             => Recipe ID action flag(R: 등록,U: 업데이트,D: 삭제), PPID, Time(YYYYMMDDhhmmss)
        //                  S9F13ConversationTimeout    => strMEXP, strEDID
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        public object funCreateSFInstance(string strParameter)
        {
            string[] darrEvent;
            int dintAlarmID = 0;
            int dintUnitID = 0;
            int dintCEID = 0;
            string dstrEQPProcessStateTimeOver = "";
            int dintBYWHO = 0;
            string dstrLOTID = "";
            int dintSlotID = 0;
            string dstrValue = "";
            string[] dstrData;
            int dintCount = 0;
            int dintECID = 0;
            int dintIndex = 0;
            int dintEOIDCount = 0;
            int dintEOIDIndexCount = 0;
            int dintEOID = 0;
            int dintEOMD = 0;
            int dintEOV = 0;
            int dint4Below = 0;             //EOID Index가 4미만인것의 개수
            int dint4Over = 0;              //EOID Index가 4이상인것의 개수
            int dintTemp = 0;
            int dintFixIndex = 0;
            int dintTRID = 0;
            string dstrUserID = "";
            string dstrOLDHOSTPPID = "";
            string dstrNEWHOSTPPID = "";
            string dstrMode = "";
            string dstrPPIDType = "";
            string dstrHOSTPPID = "";
            string dstrEQPPPID = "";
            string dstrMID = "";
            string dstrMType = "";
            int dintSVID = 0;
            Boolean dbolHeavyAlarm = false;

            try
            {
                darrEvent = strParameter.Split(',');     //SF 이름과 인자들을 분리한다.

                switch (Convert.ToInt32(darrEvent[0]))
                {
                    case (int)InfoAct.clsInfo.SFName.S1F1AreYouThereRequest:
                        SDIWetEtch.CAreYouThereClass S1F1 = new SDIWetEtch.CAreYouThereClass();

                        return S1F1;

                    case (int)InfoAct.clsInfo.SFName.S5F1AlarmReportsend:
                        dintAlarmID = Convert.ToInt32(darrEvent[1]);                    //AlarmID

                        SDIWetEtch.CS5F1Class S5F1 = new SDIWetEtch.CS5F1Class();

                        if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType == "S")   //Set
                        {
                            S5F1.ALCD = 128 + this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode;
                        }
                        else  //Reset
                        {
                            S5F1.ALCD = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode;
                        }
                        
                        S5F1.ALID = dintAlarmID;
                        S5F1.ALTX = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmDesc;
                        S5F1.MODULEID = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).ModuleID;

                        return S5F1;


                    //CEID = 51(EQ(MODULE) PROCESS STATE CHANGED)
                    //CEID = 52(EQ(MODULE) PROCESS STATE TIME OVER)
                    //CEID = 53(EQ(MODULE) STATE CHANGED)
                    //CEID = 71(CHANGE TO OFF_LINE MODE)
                    //CEID = 72(CHANGE TO ON_LINE LOCAL MODE)
                    //CEID = 73(CHANGE TO ON_LINE REMOTE MODE)
                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent:
                        dintCEID = Convert.ToInt32(darrEvent[1]);                       //CEID
                        dintUnitID = Convert.ToInt32(darrEvent[2]);                     //UnitID

                        SDIWetEtch.CS6F11RelatedEQPEventClass S6F11EQPEvent = new SDIWetEtch.CS6F11RelatedEQPEventClass();

                        S6F11EQPEvent.DATAID = 0;   //Fix
                        S6F11EQPEvent.CEID = dintCEID;
                        S6F11EQPEvent.RPTID1 = 0;   //Fix
                        S6F11EQPEvent.MODULEID1 = this.PInfo.Unit(dintUnitID).SubUnit(0).ModuleID;
                        S6F11EQPEvent.MCMD = this.PInfo.All.ControlState;
                        S6F11EQPEvent.MODULE_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPState;
                        S6F11EQPEvent.PROC_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState;

                        //각 CEID별로값을 설정한다.
                        if (dintCEID == 51)
                        {
                            S6F11EQPEvent.BYWHO = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessStateChangeBYWHO;
                            S6F11EQPEvent.OLD_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessStateOLD;
                            S6F11EQPEvent.NEW_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState;
                            S6F11EQPEvent.LIMIT_TIME = "";
                            S6F11EQPEvent.RCODE = "";
                        }
                        else if (dintCEID == 52)
                        {
                            S6F11EQPEvent.BYWHO = 3;
                            S6F11EQPEvent.OLD_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState;  //Time Over이기 때문에 똑같다.
                            S6F11EQPEvent.NEW_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState;

                            //현재 EQP Process State 에 맞는 Time Over 시간을 가져온다.
                            switch (this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState)
                            {
                                case "1":
                                    dstrEQPProcessStateTimeOver = FunStringH.funConvertTime(this.PInfo.Unit(dintUnitID).SubUnit(0).EOID(4).EOV.ToString());
                                    break;

                                case "2":
                                    dstrEQPProcessStateTimeOver = FunStringH.funConvertTime(this.PInfo.Unit(dintUnitID).SubUnit(0).EOID(5).EOV.ToString());
                                    break;

                                case "3":
                                    dstrEQPProcessStateTimeOver = FunStringH.funConvertTime(this.PInfo.Unit(dintUnitID).SubUnit(0).EOID(6).EOV.ToString());
                                    break;

                                case "4":
                                    dstrEQPProcessStateTimeOver = FunStringH.funConvertTime(this.PInfo.Unit(dintUnitID).SubUnit(0).EOID(7).EOV.ToString());
                                    break;

                                case "5":
                                    dstrEQPProcessStateTimeOver = FunStringH.funConvertTime(this.PInfo.Unit(dintUnitID).SubUnit(0).EOID(8).EOV.ToString());
                                    break;

                                case "6":
                                    dstrEQPProcessStateTimeOver = FunStringH.funConvertTime(this.PInfo.Unit(dintUnitID).SubUnit(0).EOID(9).EOV.ToString());
                                    break;
                            }

                            S6F11EQPEvent.LIMIT_TIME = dstrEQPProcessStateTimeOver;     //나중에 다시 고려(분을 hhmmss로 변경)
                            S6F11EQPEvent.RCODE = "";
                        }
                        else if (dintCEID == 53)
                        {
                            S6F11EQPEvent.BYWHO = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPStateChangeBYWHO;
                            S6F11EQPEvent.OLD_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPStateOLD;
                            S6F11EQPEvent.NEW_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPState;
                            S6F11EQPEvent.LIMIT_TIME = "";

                            //PM일때만 PMCode입력
                            if (this.PInfo.Unit(dintUnitID).SubUnit(0).EQPState == "3")
                            {
                                S6F11EQPEvent.RCODE = this.PInfo.All.PMCode;
                            }
                            else
                            {
                                S6F11EQPEvent.RCODE = "";
                            }
                            
                        }
                        else if (dintCEID == 71 || dintCEID == 72 || dintCEID == 73)
                        {
                            S6F11EQPEvent.BYWHO = this.PInfo.All.ControlstateChangeBYWHO;
                            S6F11EQPEvent.OLD_STATE = this.PInfo.All.ControlStateOLD;
                            S6F11EQPEvent.NEW_STATE = this.PInfo.All.ControlState;
                            S6F11EQPEvent.LIMIT_TIME = "";
                            S6F11EQPEvent.RCODE = "";
                        }
                        S6F11EQPEvent.OPERID = this.PInfo.All.UserID;
                        S6F11EQPEvent.RPTID2 = 4;   //Fix
                        S6F11EQPEvent.RPTID3 = 11;  //Fix


                        switch (dintCEID)
                        {
                            case 51:

                                ///////////////발생 체크////////////////////////////
                                //이전 상태가 정상상태(Pause가 아닌상태)이고 현재 상태가 Pause이면 현재 발생한 Alarm정보를 입력한다.
                                if (this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessStateOLD != "6" && this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState == "6")
                                {
                                    if (dintUnitID == 0)
                                    {
                                        //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                        foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                        {
                                            if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H")
                                            {
                                                dbolHeavyAlarm = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                        foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                        {
                                            if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H" && dintUnitID == this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).UnitID)
                                            {
                                                dbolHeavyAlarm = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (dbolHeavyAlarm == true && this.PInfo.All.OccurHeavyAlarmID != 0)
                                    {
                                        S6F11EQPEvent.ALCD = 128 + this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.OccurHeavyAlarmID).AlarmCode;
                                        S6F11EQPEvent.ALID = this.PInfo.All.OccurHeavyAlarmID;
                                        S6F11EQPEvent.ALTX = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.OccurHeavyAlarmID).AlarmDesc;
                                        S6F11EQPEvent.MODULEID2 = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.OccurHeavyAlarmID).ModuleID;
                                    }
                                    else
                                    {
                                        S6F11EQPEvent.ALCD = 0;
                                        S6F11EQPEvent.ALID = 0;
                                        S6F11EQPEvent.ALTX = "";
                                        S6F11EQPEvent.MODULEID2 = "";
                                    }
                                }

                                ///////////////해제 체크////////////////////////////
                                //이전 상태가 Pause이고 현재 상태가 정상상태(Pause가 아닌상태), Fault가 아니면 현재 Clear된 Alarm정보를 입력한다.
                                else if (this.PInfo.Unit(dintUnitID).SubUnit(0).EQPState == "2" && this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessStateOLD == "6" 
                                                && this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState != "6")
                                {
                                    if (dintUnitID == 0)
                                    {
                                        //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                        foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                        {
                                            if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H")
                                            {
                                                dbolHeavyAlarm = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                        foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                        {
                                            if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H" && dintUnitID == this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).UnitID)
                                            {
                                                dbolHeavyAlarm = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (dbolHeavyAlarm == false && this.PInfo.All.ClearHeavyAlarmID != 0)
                                    {
                                        if ((dintUnitID == 0) || (dintUnitID == this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.ClearHeavyAlarmID).UnitID))
                                        {
                                            S6F11EQPEvent.ALCD = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.ClearHeavyAlarmID).AlarmCode;
                                            S6F11EQPEvent.ALID = this.PInfo.All.ClearHeavyAlarmID;
                                            S6F11EQPEvent.ALTX = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.ClearHeavyAlarmID).AlarmDesc;
                                            S6F11EQPEvent.MODULEID2 = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.ClearHeavyAlarmID).ModuleID;
                                        }
                                        else
                                        {
                                            S6F11EQPEvent.ALCD = 0;
                                            S6F11EQPEvent.ALID = 0;
                                            S6F11EQPEvent.ALTX = "";
                                            S6F11EQPEvent.MODULEID2 = "";
                                        }
                                    }
                                    else
                                    {
                                        S6F11EQPEvent.ALCD = 0;
                                        S6F11EQPEvent.ALID = 0;
                                        S6F11EQPEvent.ALTX = "";
                                        S6F11EQPEvent.MODULEID2 = "";
                                    }
                                }

                                break;

                            case 53:

                                ///////////////발생 체크////////////////////////////
                                //이전 상태가 정상상태(Normal 혹은 PM)이고 현재 상태가 Fault이면 현재 발생한 Alarm정보를 입력한다.
                                if (this.PInfo.Unit(dintUnitID).SubUnit(0).EQPStateOLD != "2" && this.PInfo.Unit(dintUnitID).SubUnit(0).EQPState == "2")
                                {
                                    if (dintUnitID == 0)
                                    {
                                        //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                        foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                        {
                                            if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H")
                                            {
                                                dbolHeavyAlarm = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                        foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                        {
                                            if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H" && dintUnitID == this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).UnitID)
                                            {
                                                dbolHeavyAlarm = true;
                                                break;
                                            }
                                        }
                                    }


                                    if (dbolHeavyAlarm == true && this.PInfo.All.OccurHeavyAlarmID != 0)
                                    {
                                        S6F11EQPEvent.ALCD = 128 + this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.OccurHeavyAlarmID).AlarmCode;
                                        S6F11EQPEvent.ALID = this.PInfo.All.OccurHeavyAlarmID;
                                        S6F11EQPEvent.ALTX = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.OccurHeavyAlarmID).AlarmDesc;
                                        S6F11EQPEvent.MODULEID2 = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.OccurHeavyAlarmID).ModuleID;
                                    }
                                    else
                                    {
                                        S6F11EQPEvent.ALCD = 0;
                                        S6F11EQPEvent.ALID = 0;
                                        S6F11EQPEvent.ALTX = "";
                                        S6F11EQPEvent.MODULEID2 = "";
                                    }
                                }

                                ///////////////해제 체크////////////////////////////
                                //이전 상태가 Fault이고 현대 상태가 정상상태(Normal 혹은 PM)이면 현재 Clear된 Alarm정보를 입력한다.
                                else if (this.PInfo.Unit(dintUnitID).SubUnit(0).EQPStateOLD == "2" && this.PInfo.Unit(dintUnitID).SubUnit(0).EQPState != "2")
                                {
                                    if (dintUnitID == 0)
                                    {
                                        //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                        foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                        {
                                            if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H")
                                            {
                                                dbolHeavyAlarm = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                        foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                                        {
                                            if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H" && dintUnitID == this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).UnitID)
                                            {
                                                dbolHeavyAlarm = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (dbolHeavyAlarm == false && this.PInfo.All.ClearHeavyAlarmID != 0)
                                    {
                                        if ((dintUnitID == 0) || (dintUnitID == this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.ClearHeavyAlarmID).UnitID))
                                        {
                                            S6F11EQPEvent.ALCD = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.ClearHeavyAlarmID).AlarmCode;
                                            S6F11EQPEvent.ALID = this.PInfo.All.ClearHeavyAlarmID;
                                            S6F11EQPEvent.ALTX = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.ClearHeavyAlarmID).AlarmDesc;
                                            S6F11EQPEvent.MODULEID2 = this.PInfo.Unit(0).SubUnit(0).Alarm(this.PInfo.All.ClearHeavyAlarmID).ModuleID;
                                        }
                                        else
                                        {
                                            S6F11EQPEvent.ALCD = 0;
                                            S6F11EQPEvent.ALID = 0;
                                            S6F11EQPEvent.ALTX = "";
                                            S6F11EQPEvent.MODULEID2 = "";
                                        }
                                    }
                                    else
                                    {
                                        S6F11EQPEvent.ALCD = 0;
                                        S6F11EQPEvent.ALID = 0;
                                        S6F11EQPEvent.ALTX = "";
                                        S6F11EQPEvent.MODULEID2 = "";
                                    }
                                }

                                break;

                            default:

                                S6F11EQPEvent.ALCD = 0;
                                S6F11EQPEvent.ALID = 0;
                                S6F11EQPEvent.ALTX = "";
                                S6F11EQPEvent.MODULEID2 = "";

                                break;
                        }

                        return S6F11EQPEvent;


                    //CEID = 13(GLASS IS ABORTED)
                    //CEID = 14(GLASS SCRAP)
                    //CEID = 15(GLASS UNSCRAP)
                    //CEID = 16(PANEL PROCESS START for MODULE)
                    //CEID = 17(PANEL PROCESS END for MODULE)
                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent:
                        dintCEID = Convert.ToInt32(darrEvent[1]);                       //CEID
                        dintUnitID = Convert.ToInt32(darrEvent[2]);                     //UnitID
                        dstrLOTID = darrEvent[3];                                       //LOTID
                        dintSlotID = Convert.ToInt32(darrEvent[4]);                     //SlotID

                        SDIWetEtch.CS6F11RelatedPanelProcessClass S6F11RelatedPanel = new SDIWetEtch.CS6F11RelatedPanelProcessClass();

                        S6F11RelatedPanel.DATAID = 0;   //Fix
                        S6F11RelatedPanel.CEID = dintCEID;
                        S6F11RelatedPanel.RPTID = 0;   //Fix
                        S6F11RelatedPanel.MCMD = this.PInfo.All.ControlState;
                        S6F11RelatedPanel.MODULEID = this.PInfo.Unit(dintUnitID).SubUnit(0).ModuleID;
                        S6F11RelatedPanel.MODULE_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPState;
                        S6F11RelatedPanel.PROC_STATE = this.PInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState;

                        //각 CEID별로 BYWHO값을 설정한다.
                        switch (dintCEID)
                        {
                            case 13:
                                dintBYWHO = 3;      
                                break;
                            case 14:
                                dintBYWHO = 3;
                                break;
                            case 15:
                                dintBYWHO = 3;     
                                break;
                            case 16:
                                dintBYWHO = 3;
                                break;
                            case 17:
                                dintBYWHO = 3;
                                break;
                            default:
                                break;
                        }
                        S6F11RelatedPanel.BYWHO = dintBYWHO;
                        S6F11RelatedPanel.OPERID = this.PInfo.All.UserID;
                        S6F11RelatedPanel.RPTID1 = 3;   //Fix

                        S6F11RelatedPanel.H_PANELID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).H_PANELID;
                        S6F11RelatedPanel.E_PANELID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).E_PANELID;
                        S6F11RelatedPanel.LOTID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).LOTID;
                        S6F11RelatedPanel.BATCHID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).BATCHID;
                        S6F11RelatedPanel.SLOTNO = FunStringH.funMakeLengthStringFirst(this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).SlotID.ToString(), 2);
                        S6F11RelatedPanel.PROD_TYPE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROD_TYPE;
                        S6F11RelatedPanel.PROD_KIND = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROD_KIND;
                        S6F11RelatedPanel.DEVICEID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).DEVICEID;
                        S6F11RelatedPanel.RUNSHEETID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).RUNSHEETID;
                        S6F11RelatedPanel.FLOWID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FLOWID;
                        S6F11RelatedPanel.STEPID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).STEPID;
                        S6F11RelatedPanel.PPID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PPID;
                        S6F11RelatedPanel.THICKNESS = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).THICKNESS;
                        S6F11RelatedPanel.INS_FLAG = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).INS_FLAG;
                        S6F11RelatedPanel.FLOWRECIPE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FlowRecipe;
                        S6F11RelatedPanel.PROCESS_STEP = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROCESS_STEP;
                        S6F11RelatedPanel.PANEL_SIZE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_SIZE;
                        S6F11RelatedPanel.PANEL_POSITION = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_POSITION;
                        S6F11RelatedPanel.COUNT = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).COUNT;
                        S6F11RelatedPanel.GRADE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).GRADE;
                        S6F11RelatedPanel.SORT_FLAG = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).SORT_FLAG;
                        S6F11RelatedPanel.HOT_FLOW_FLAG = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).HOTFLOWFLAG;
                        S6F11RelatedPanel.M_START_FLAG = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).M_START_FLAG;
                        S6F11RelatedPanel.PANEL_TYPE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_TYPE;
                        S6F11RelatedPanel.COMMENT = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).COMMENT;
                        S6F11RelatedPanel.FILE_LOC = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FILE_LOC;
                        S6F11RelatedPanel.DEFECT_COUNT = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).DEFECT_COUNT;

                        S6F11RelatedPanel.PANEL_STATE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANELSTATE;
                        S6F11RelatedPanel.JUDGEMENT = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).JUDGEMENT;
                        S6F11RelatedPanel.CODE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).CODE;
                        S6F11RelatedPanel.RUNLINE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).RUNLINE;
                        S6F11RelatedPanel.UNIQUEID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).UNIQUEID;

                        //S6F11RelatedPanel.PAIR_H_PANELID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PAIR_H_PANELID;
                        //S6F11RelatedPanel.PAIR_E_PANELID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PAIR_E_PANELID;
                        //S6F11RelatedPanel.PAIR_UNIQUEID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PAIR_UNIQUEID;
                        //S6F11RelatedPanel.MATCH_GROUP = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).MATCH_GROUP;


                        return S6F11RelatedPanel;


                    //CEID = 101(PARAMETER CHANGED (EOID))
                    //CEID = 102(PARAMETER CHANGED (ECID))
                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedEquipmentParameterEvent:
                        dintCEID = Convert.ToInt32(darrEvent[1]);   //CEID
                        dstrValue = darrEvent[2];                   //Value(102는 ECID값(KEY값), EOID의 경우 Index이다.)


                        SDIWetEtch.CS6F11RelatedEQPParameterClass S6F11EQPParameter = new SDIWetEtch.CS6F11RelatedEQPParameterClass();

                        S6F11EQPParameter.DATAID = 0;   //Fix
                        S6F11EQPParameter.CEID = dintCEID;
                        S6F11EQPParameter.RPTID1 = 0;   //Fix
                        S6F11EQPParameter.MODULEID = this.PInfo.Unit(0).SubUnit(0).ModuleID;
                        S6F11EQPParameter.MCMD = this.PInfo.All.ControlState;
                        S6F11EQPParameter.MODULE_STATE = this.PInfo.Unit(0).SubUnit(0).EQPState;
                        S6F11EQPParameter.PROC_STATE = this.PInfo.Unit(0).SubUnit(0).EQPProcessState;

                        //각 CEID별로 BYWHO값을 설정한다.
                        switch (dintCEID)
                        {
                            case 101:
                                dintBYWHO = Convert.ToInt32(this.PInfo.All.EOIDChangeBYWHO);
                                
                                S6F11EQPParameter.ECCOUNT = 0;
                                dintEOIDIndexCount = dstrValue.Split(';').Length - 1;    //보고할 EOID Index 개수
                                dstrData = dstrValue.Split(';');                        //보고할 EOID Index 값

                                //EOID Index가 4미만, 4이상인것의 개수, 존재여부를 체크
                                for (int dintLoop = 1; dintLoop <= dintEOIDIndexCount; dintLoop++)
                                {
                                    if (Convert.ToInt32(dstrData[dintLoop - 1]) >= 4)
                                    {
                                        dint4Over = dint4Over + 1;      //EOID Index가 4이상인 것 개수
                                        //dint4OverExist = true;      //EOID Idnex가 4이상인것이 존재
                                    }
                                    else
                                    {
                                        dint4Below = dint4Below + 1;    //EOID Index가 4미만인것의 개수
                                    }
                                }

                                if (dint4Over > 0)
                                {
                                    dintEOIDCount = dint4Below + 1;     //EOID Index가 4이상인것이 존재하면 4미만인것의 개수 + 1개(4이상인것은 EOIDCount를 1개로 하기 때문)
                                }
                                else
                                {
                                    dintEOIDCount = dint4Below;         //EOID Index가 4미만인것만 있으면 개수만큼 EOIDCount로 한다.
                                }
                                S6F11EQPParameter.EOCOUNT = dintEOIDCount;    //보고할 EOIDCount List 개수

                                if (dint4Over > 0)
                                {
                                    for (int dintLoop = 1; dintLoop <= dintEOIDIndexCount; dintLoop++)
                                    {
                                        dintIndex = Convert.ToInt32(dstrData[dintLoop - 1]);
                                        dintEOID = this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID;
                                        dintEOMD = this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMD;
                                        dintEOV = this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV;

                                        //EOMD, EOMD값으로 Index를 찾는다.(Index가 Key값이기 떄문)
                                        //dintIndex = this.PInfo.funGetEOIDEOMDToIndex(dintEOID, dintEOMD);

                                        S6F11EQPParameter.set_EOID(dintLoop, dintEOID);
                                       
                                        if (dintIndex >= 4)
                                        {
                                            S6F11EQPParameter.set_EOMDCOUNT(dintLoop, dint4Over);
                                            dintTemp = dintTemp + 1;

                                            if (dintFixIndex == 0) dintFixIndex = dintLoop;         //같은 List에 있어야 한다.
                                            S6F11EQPParameter.set_EOMD(dintFixIndex, dintTemp, dintEOMD);
                                            S6F11EQPParameter.set_EOV(dintFixIndex, dintTemp, dintEOV);
                                        }
                                        else
                                        {
                                            S6F11EQPParameter.set_EOMDCOUNT(dintLoop, 1);
                                            S6F11EQPParameter.set_EOMD(dintLoop, 1, dintEOMD);
                                            S6F11EQPParameter.set_EOV(dintLoop, 1, dintEOV);
                                        }
                                    }
                                }
                                else
                                {
                                    for (int dintLoop = 1; dintLoop <= dintEOIDCount; dintLoop++)
                                    {
                                        dintIndex = Convert.ToInt32(dstrData[dintLoop - 1]);
                                        dintEOID = this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOID;

                                        dintEOMD = this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOMD;
                                        dintEOV = this.PInfo.Unit(0).SubUnit(0).EOID(dintIndex).EOV;

                                        //EOMD, EOMD값으로 Index를 찾는다.(Index가 Key값이기 떄문)
                                        //dintIndex = this.PInfo.funGetEOIDEOMDToIndex(dintEOID, dintEOMD);

                                        S6F11EQPParameter.set_EOID(dintLoop, dintEOID);
                                        S6F11EQPParameter.set_EOMDCOUNT(dintLoop, 1);
                                        S6F11EQPParameter.set_EOMD(dintLoop, 1, dintEOMD);
                                        S6F11EQPParameter.set_EOV(dintLoop, 1, dintEOV);
                                    }
                                }

                              

                                break;
                            case 102:
                                dintBYWHO = Convert.ToInt32(this.PInfo.All.ECIDChangeBYWHO);

                                S6F11EQPParameter.EOCOUNT = 0;

                                dintCount = dstrValue.Split(';').Length - 1;    //보고할 List 개수
                                dstrData = dstrValue.Split(';');

                                S6F11EQPParameter.ECCOUNT = dintCount;    //보고할 List 개수
                                for (int dintLoop = 1; dintLoop <= dintCount; dintLoop++)
                                {
                                    dintECID = Convert.ToInt32(dstrData[dintLoop - 1]);
                                    S6F11EQPParameter.set_ECID(dintLoop, dintECID);
                                    S6F11EQPParameter.set_ECNAME(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).Name);
                                    S6F11EQPParameter.set_ECDEF(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECDEF);
                                    S6F11EQPParameter.set_ECSLL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSLL);
                                    S6F11EQPParameter.set_ECSUL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSUL);
                                    S6F11EQPParameter.set_ECWLL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWLL);
                                    S6F11EQPParameter.set_ECWUL(dintLoop, this.PInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWUL);
                                }

                                break;
                            default:
                                break;
                        }
                        S6F11EQPParameter.BYWHO = dintBYWHO;
                        S6F11EQPParameter.OPERID = this.PInfo.All.UserID;
                        S6F11EQPParameter.RPTID1 = 5;   //Fix
                        S6F11EQPParameter.RPTID2 = 6;   //Fix

                        return S6F11EQPParameter;


                    //CEID = 161(USER LOGIN)
                    //CEID = 162(USER LOGOUT)
                    //CEID = 401(Equipment Current PPID Change Event)
                    case (int)InfoAct.clsInfo.SFName.S6F11EquipmentSpecifiedControlEvent:
                        dintCEID = Convert.ToInt32(darrEvent[1]);   //CEID

                        SDIWetEtch.CS6F11EQPSpecifiedCtrlClass S6F11EQPSpecified = new SDIWetEtch.CS6F11EQPSpecifiedCtrlClass();

                        S6F11EQPSpecified.DATAID = 0;   //Fix
                        S6F11EQPSpecified.CEID = dintCEID;
                        S6F11EQPSpecified.RPTID1 = 0;   //Fix
                        S6F11EQPSpecified.MODULEID = this.PInfo.Unit(0).SubUnit(0).ModuleID;
                        S6F11EQPSpecified.MCMD = this.PInfo.All.ControlState;
                        S6F11EQPSpecified.MODULE_STATE = this.PInfo.Unit(0).SubUnit(0).EQPState;
                        S6F11EQPSpecified.PROC_STATE = this.PInfo.Unit(0).SubUnit(0).EQPProcessState;
                        S6F11EQPSpecified.BYWHO = "3";  //By EQP
                        S6F11EQPSpecified.OPERID = this.PInfo.All.UserID;
                        S6F11EQPSpecified.RPTID2 = 7;   //Fix
                        
                        switch (dintCEID)
                        {
                            case 161:
                                S6F11EQPSpecified.BYWHO = "2";  //By OP

                                dstrUserID = darrEvent[2];                          //USerID임.

                                S6F11EQPSpecified.COUNT = 1;
                                S6F11EQPSpecified.set_ITEM_NAME(1, "LOGIN");
                                S6F11EQPSpecified.set_ITEM_VALUE(1, dstrUserID);
                                break;
                            case 162:
                                S6F11EQPSpecified.BYWHO = "2";  //By OP

                                dstrUserID = darrEvent[2];                          //USerID임.

                                S6F11EQPSpecified.COUNT = 1;
                                S6F11EQPSpecified.set_ITEM_NAME(1, "LOGOUT");
                                S6F11EQPSpecified.set_ITEM_VALUE(1, dstrUserID);
                                break;
                            case 401:
                                S6F11EQPSpecified.BYWHO = this.PInfo.All.EQPSpecifiedCtrlBYWHO;

                                dstrOLDHOSTPPID = darrEvent[2];                   //OLD HOSTPPID임.
                                dstrNEWHOSTPPID = darrEvent[3];                   //NEW HOSTPPID임.

                                S6F11EQPSpecified.COUNT = 2;
                                S6F11EQPSpecified.set_ITEM_NAME(1, "OLDPPID");
                                S6F11EQPSpecified.set_ITEM_VALUE(1, dstrOLDHOSTPPID);
                                S6F11EQPSpecified.set_ITEM_NAME(2, "NEWPPID");
                                S6F11EQPSpecified.set_ITEM_VALUE(2, dstrNEWHOSTPPID);
                                break;
                        }

                        return S6F11EQPSpecified;


                    //Mode(1: 생성, 2: 삭제, 3: 수정)
                    case (int)InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport:
                        dstrMode = darrEvent[1];    //Mode(1: 생성, 2: 삭제, 3: 수정)
                        dstrPPIDType = darrEvent[2];
                        dstrHOSTPPID = darrEvent[3];
                        dstrEQPPPID = darrEvent[4];

                        SDIWetEtch.CS7F107Class S7F107 = new SDIWetEtch.CS7F107Class();

                        S7F107.MODE = dstrMode;     //Mode(1: 생성, 2: 삭제, 3: 수정)
                        S7F107.MODUELID1 = this.PInfo.Unit(0).SubUnit(0).ModuleID;
                        S7F107.PPID_TYPE = dstrPPIDType;
                        S7F107.set_CCODE(1, 0);

                        switch (dstrPPIDType)
                        {
                            case "1":
                                S7F107.PPID = dstrEQPPPID;     //EQPPPID
                                S7F107.SOFT_REV = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDVer;
                                S7F107.COMMANDCOUNT = 1;
                                S7F107.set_PPARMCOUNT(1, this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount);
                                for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                                {
                                    S7F107.set_P_PARM_NAME(1, dintLoop, this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Name);
                                    S7F107.set_P_PARM(1, dintLoop, this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value);
                                }

                                break;
                            case "2":
                                S7F107.PPID = dstrHOSTPPID;      //HOSTPPID
                                S7F107.SOFT_REV = this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).PPIDVer;
                                S7F107.COMMANDCOUNT = 1;
                                S7F107.set_PPARMCOUNT(1, 1);
                                S7F107.set_P_PARM_NAME(1, 1, "SUBPPID");
                                S7F107.set_P_PARM(1, 1, dstrEQPPPID);
                                break;
                        }
                        
                        return S7F107;


                    //CEID = 203(Material Dock_In)
                    //CEID = 204(Material Dock_Out)
                    //CEID = 206(Material Supplement Request)
                    case (int)InfoAct.clsInfo.SFName.S6F11RelatedMaterialEvent:
                        dintCEID = Convert.ToInt32(darrEvent[1]);   //CEID
                        dstrMID = darrEvent[2];
                        dstrMType = darrEvent[3];

                        SDIWetEtch.CS6F11RelatedMaterialEventClass S6F11RelatedMaterial = new SDIWetEtch.CS6F11RelatedMaterialEventClass();

                        S6F11RelatedMaterial.DATAID = 0;   //Fix
                        S6F11RelatedMaterial.CEID = dintCEID;
                        S6F11RelatedMaterial.RPTID1 = 0;   //Fix
                        S6F11RelatedMaterial.MODULEID = this.PInfo.Unit(0).SubUnit(0).ModuleID;
                        S6F11RelatedMaterial.MCMD = this.PInfo.All.ControlState;
                        S6F11RelatedMaterial.MODULE_STATE = this.PInfo.Unit(0).SubUnit(0).EQPState;
                        S6F11RelatedMaterial.PROC_STATE = this.PInfo.Unit(0).SubUnit(0).EQPProcessState;
                        S6F11RelatedMaterial.BYWHO = "3";  //By EQP
                        S6F11RelatedMaterial.OPERID = this.PInfo.All.UserID;
                        S6F11RelatedMaterial.RPTID2 = 8;   //Fix

                        S6F11RelatedMaterial.M_ID = dstrMID;
                        S6F11RelatedMaterial.M_TYPE = dstrMType;
                        S6F11RelatedMaterial.LIBRARYID = "";
                        S6F11RelatedMaterial.MP_STATE = "";
                        S6F11RelatedMaterial.M_STATE = "";
                        S6F11RelatedMaterial.M_LOC = "";
                        S6F11RelatedMaterial.M_SIZE = "";
                        S6F11RelatedMaterial.RelatedCount = 1;          //우선은 1개로 함.
                        S6F11RelatedMaterial.set_M_DEVICE(1, "");
                        S6F11RelatedMaterial.set_m_STEPID(1, "");
                        S6F11RelatedMaterial.set_m_PPID(1, "");

                        return S6F11RelatedMaterial;


                    //CEID = 10(GLASS Base)
                    case (int)InfoAct.clsInfo.SFName.S6F13GLSAPD:
                        dstrLOTID = darrEvent[1];                                       //LOTID
                        dintSlotID = Convert.ToInt32(darrEvent[2]);                     //SlotID

                        SDIWetEtch.CS6F13GLSAPDClass S6F13GLSAPD = new SDIWetEtch.CS6F13GLSAPDClass();

                        S6F13GLSAPD.DATAID = 0;   //Fix
                        S6F13GLSAPD.CEID = 10;    //Fix
                        S6F13GLSAPD.RPTID1 = 1;   //Fix

                        S6F13GLSAPD.H_PANELID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).H_PANELID;
                        S6F13GLSAPD.E_PANELID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).E_PANELID;
                        S6F13GLSAPD.LOTID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).LOTID;
                        S6F13GLSAPD.BATCHID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).BATCHID;
                        S6F13GLSAPD.SLOTNO = FunStringH.funMakeLengthStringFirst(this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).SlotID.ToString(), 2);
                        S6F13GLSAPD.PROD_TYPE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROD_TYPE;
                        S6F13GLSAPD.PROD_KIND = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROD_KIND;
                        S6F13GLSAPD.DEVICEID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).DEVICEID;
                        S6F13GLSAPD.RUNSHEETID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).RUNSHEETID;
                        S6F13GLSAPD.FLOWID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FLOWID;
                        S6F13GLSAPD.STEPID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).STEPID;
                        S6F13GLSAPD.PPID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PPID;
                        S6F13GLSAPD.THICKNESS = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).THICKNESS;
                        S6F13GLSAPD.INS_FLAG = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).INS_FLAG;
                        S6F13GLSAPD.FLOWRECIPE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FlowRecipe;
                        S6F13GLSAPD.PROCESS_STEP = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROCESS_STEP;

                        S6F13GLSAPD.PANEL_SIZE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_SIZE;
                        S6F13GLSAPD.PANEL_POSITION = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_POSITION;
                        S6F13GLSAPD.COUNT = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).COUNT;
                        S6F13GLSAPD.GRADE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).GRADE;
                        S6F13GLSAPD.SORT_FLAG = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).SORT_FLAG;
                        S6F13GLSAPD.HOT_FLOW_FLAG = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).HOTFLOWFLAG;
                        S6F13GLSAPD.M_START_FLAG = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).M_START_FLAG;
                        S6F13GLSAPD.PANEL_TYPE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANEL_TYPE;
                        S6F13GLSAPD.COMMENT = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).COMMENT;
                        S6F13GLSAPD.FILE_LOC = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FILE_LOC;
                        S6F13GLSAPD.DEFECT_COUNT = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).DEFECT_COUNT;

                        S6F13GLSAPD.PANEL_STATE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PANELSTATE;
                        S6F13GLSAPD.JUDGEMENT = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).JUDGEMENT;
                        S6F13GLSAPD.CODE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).CODE;
                        S6F13GLSAPD.RUNLINE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).RUNLINE;
                        S6F13GLSAPD.UNIQUEID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).UNIQUEID;

                        //GLS APD 추가
                        S6F13GLSAPD.MODULEID = this.PInfo.Unit(0).SubUnit(0).ModuleID;
                        S6F13GLSAPD.DATACOUNT = this.PInfo.Unit(0).SubUnit(0).GLSAPDCount;
                        for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).GLSAPDCount; dintLoop++)
                        {
                            S6F13GLSAPD.set_DATA_ITEM(dintLoop, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).GLSAPD(dintLoop).Name);
                            S6F13GLSAPD.set_DATA_VALUE(dintLoop, this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).GLSAPD(dintLoop).Value);
                        }

                        return S6F13GLSAPD;


                    //CEID = 11(LOT Base)
                    case (int)InfoAct.clsInfo.SFName.S6F13LOTAPD:
                        dstrLOTID = darrEvent[1];                                       //LOTID
                        dintSlotID = Convert.ToInt32(darrEvent[2]);                     //SlotID

                        SDIWetEtch.CS6F13LOTAPDClass S6F13LOTAPD = new SDIWetEtch.CS6F13LOTAPDClass();

                        S6F13LOTAPD.DATAID = 0;   //Fix
                        S6F13LOTAPD.CEID = 11;    //Fix
                        S6F13LOTAPD.RPTID1 = 1;   //Fix

                        S6F13LOTAPD.H_PANELID = "";
                        S6F13LOTAPD.E_PANELID = "";
                        S6F13LOTAPD.LOTID = dstrLOTID;
                        S6F13LOTAPD.BATCHID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).BATCHID;
                        S6F13LOTAPD.SLOTNO = "";
                        S6F13LOTAPD.PROD_TYPE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROD_TYPE;
                        S6F13LOTAPD.PROD_KIND = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROD_KIND;
                        S6F13LOTAPD.DEVICEID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).DEVICEID;
                        S6F13LOTAPD.RUNSHEETID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).RUNSHEETID;
                        S6F13LOTAPD.FLOWID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FLOWID;
                        S6F13LOTAPD.STEPID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).STEPID;
                        S6F13LOTAPD.PPID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PPID;
                        S6F13LOTAPD.THICKNESS = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).THICKNESS;
                        S6F13LOTAPD.INS_FLAG = "";
                        S6F13LOTAPD.FLOWRECIPE = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).FlowRecipe;
                        S6F13LOTAPD.PROCESS_STEP = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).PROCESS_STEP;

                        //LOT APD 추가
                        S6F13LOTAPD.MODULEID = this.PInfo.Unit(0).SubUnit(0).ModuleID;
                        S6F13LOTAPD.DATACOUNT = this.PInfo.Unit(0).SubUnit(0).LOTAPDCount;
                        for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).LOTAPDCount; dintLoop++)
                        {
                            S6F13LOTAPD.set_DATA_ITEM(dintLoop, this.PInfo.LOTID(dstrLOTID).LOTAPD(dintLoop).Name);
                            S6F13LOTAPD.set_DATA_VALUE(dintLoop, this.PInfo.LOTID(dstrLOTID).LOTAPD(dintLoop).Value);
                        }

                        return S6F13LOTAPD;


                    //S6F1 Trace
                    case (int)InfoAct.clsInfo.SFName.S6F1TraceDataSend:
                        dintTRID = Convert.ToInt32(darrEvent[1]);

                        SDIWetEtch.CS6F1 S6F1 = new SDIWetEtch.CS6F1();

                        S6F1.MODULEID = this.PInfo.Unit(0).SubUnit(0).ModuleID;
                        S6F1.TRID = dintTRID;
                        S6F1.SMPLN = this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).SampleNo;

                        if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ == 1)       //Group Size가 1인경우
                        {
                            S6F1.REPGSZCOUNTER = 1;
                            S6F1.set_SCTIME(1, this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(1).ReadTime);
                            S6F1.set_SVCOUNT(1, this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(1).SVIDCount);

                            for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(1).SVIDCount; dintLoop++)
                            {
                                dintSVID = this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(1).SVID(dintLoop).SVID;
                                S6F1.set_SVID(1, dintLoop, dintSVID);
                                S6F1.set_SVNAME(1, dintLoop, this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(1).SVID(dintLoop).Name);
                                S6F1.set_SV(1, dintLoop, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value);
                            }
                        }
                        else if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ > 1)    //Group Size가 1이상인 경우
                        {
                            S6F1.REPGSZCOUNTER = this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ;
                            for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ; dintLoop++)
                            {
                                S6F1.set_SCTIME(dintLoop, this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintLoop).ReadTime);
                                S6F1.set_SVCOUNT(dintLoop, this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintLoop).SVIDCount);

                                for (int dintLoop2 = 1; dintLoop2 <= this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintLoop).SVIDCount; dintLoop2++)
                                {
                                    dintSVID = this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintLoop).SVID(dintLoop2).SVID;
                                    S6F1.set_SVID(dintLoop, dintLoop2, dintSVID);
                                    S6F1.set_SVNAME(dintLoop, dintLoop2, this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintLoop).SVID(dintLoop2).Name);
                                    S6F1.set_SV(dintLoop, dintLoop2, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value);
                                }
                            }
                        }

                        return S6F1;


                    default:
                        return null;
                }
            }
            catch(Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return null;
            }
        }

        //*******************************************************************************
        //  Function Name : subTraceReport()
        //  Description   : Trace를 체크하여 있으면 HOST로 보고한다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : S6F1 Trace 보고용임.
        //*******************************************************************************
        //  2007/01/19          김 효주         [L 00] 
        //*******************************************************************************
        //void SVIDSend_Tick(object sender, ElapsedEventArgs e)
        void SVIDSend()
        {
            int dintGroupCnt = 0;
            string[] dstrValue;
            string dstrData = "";

            try
            {
                //Trace가 하나도 걸려있지 않으면 빠져나간다.
                if (this.PInfo.Unit(0).SubUnit(0).TRIDCount == 0) return;

                foreach (int dintTRID in this.PInfo.Unit(0).SubUnit(0).TRID())
                {
                    if (Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).DSPER) > 0)
                    {
                        this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc += 1;  //횟수 누적
                        //this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc += 0.2;  //횟수 누적

                        if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc >= Convert.ToInt32(this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).DSPER))
                        {
                            this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).GrpCnt += 1;
                            dintGroupCnt = this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).GrpCnt;
                            this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).TimeAcc = 0;

                            this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).Group(dintGroupCnt).ReadTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                            if (this.PInfo.Unit(0).SubUnit(0).TRID(dintTRID).REPGSZ == 1)
                            {
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
                }

                //TOTSMP만큼 보냈으면 더이상 보내지 않는 것을 삭제한다.
                dstrValue = dstrData.Split(new char[] { ';' });
                for (int dintLoop = 1; dintLoop <= dstrValue.Length - 1; dintLoop++)
                {
                    this.PInfo.Unit(0).SubUnit(0).RemoveTRID(Convert.ToInt32(dstrValue[dintLoop - 1]));
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


        //PLC로 명령을 주고 모두 읽을때까지 대기하는 함수
        #region "PLC로 명령을 주고 모두 읽을때까지 대기하는 함수"

        public void subWaitDuringReadFromPLC()
        {
            long dlngTimeTick = 0;
            long dlngSec = 0;
            int dintTimeOut = 20;   //TimeOut은 20초로 함

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

        #endregion


        //Secs Drive로 메세지를 송신한다.
        #region "메세지 송신"

        //*******************************************************************************
        //  Function Name : funACTSECSAort_Send()
        //  Description   : Host로 부터 수신한 Primary Message가 Off-Line이면 Abort한다.
        //  Parameters    : None
        //  Return Value  : Return Value  : Abort 송신함 => True, Abort 송신하지 않음 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private Boolean funACTSECSAort_Send(SEComPlugInLib.CHeader SECSHeader)
        {
            Boolean dbolSend = false;

            try
            {
                if (this.PInfo.All.ControlState == "1")
                {
                    switch (SECSHeader.get_StreamNum())
                    {
                        case 1:
                            this.PSecsDrv.S1F0.Reply(SECSHeader);
                            dbolSend = true;
                            break;
                        case 2:
                            this.PSecsDrv.S2F0.Reply(SECSHeader);
                            dbolSend = true;
                            break;
                        case 3:
                            this.PSecsDrv.S3F0.Reply(SECSHeader);
                            dbolSend = true;
                            break;
                        case 5:
                            this.PSecsDrv.S5F0.Reply(SECSHeader);
                            dbolSend = true;
                            break;
                        case 6:
                            this.PSecsDrv.S6F0.Reply(SECSHeader);
                            dbolSend = true;
                            break;
                        case 7:
                            this.PSecsDrv.S7F0.Reply(SECSHeader);
                            dbolSend = true;
                            break;
                        case 9:
                            this.PSecsDrv.S9F0.Reply(SECSHeader);
                            dbolSend = true;
                            break;
                        case 10:
                            this.PSecsDrv.S10F0.Reply(SECSHeader);
                            dbolSend = true;
                            break;
                        default:
                            break;
                    }
                }
                return dbolSend;
            }
            catch(Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
        }


        //*******************************************************************************
        //  Function Name : subACTS5F1_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTAreYouThere_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CAreYouThereClass dclsSF = (SDIWetEtch.CAreYouThereClass)objSF;

                this.PSecsDrv.AreYouThere.CopyMessage(dclsSF);
                this.PSecsDrv.AreYouThere.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTS5F1_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTS5F1_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS5F1Class dclsSF = (SDIWetEtch.CS5F1Class)objSF;

                this.PSecsDrv.S5F1.CopyMessage(dclsSF);
                this.PSecsDrv.S5F1.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTS6F1_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTS6F1_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS6F1 dclsSF = (SDIWetEtch.CS6F1)objSF;

                this.PSecsDrv.S6F1.CopyMessage(dclsSF);
                this.PSecsDrv.S6F1.Request();

                System.Diagnostics.Debug.WriteLine(this.PSecsDrv.isSECSConnected);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTS6F11RelatedPanelProcess_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTS6F11RelatedPanelProcess_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS6F11RelatedPanelProcessClass dclsSF = (SDIWetEtch.CS6F11RelatedPanelProcessClass)objSF;

                this.PSecsDrv.S6F11RelatedPanelProcess.CopyMessage(dclsSF);
                this.PSecsDrv.S6F11RelatedPanelProcess.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTS6F11RelatedEQPEvent_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTS6F11RelatedEQPEvent_Send(object objSF)
        {
            try
            {
                //형변환을 명시적으로 한다.
                SDIWetEtch.CS6F11RelatedEQPEventClass dclsSF = (SDIWetEtch.CS6F11RelatedEQPEventClass)objSF;

                //CEID=71 Offline 전환 보고는 Offline이라도 가능하게 한다.
                if (dclsSF.CEID.ToString() == "71")
                {
                    if (this.PInfo.All.HostConnect == false) return;
                }
                else
                {
                    if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;
                }

                this.PSecsDrv.S6F11RelatedEQPEvent.CopyMessage(dclsSF);
                this.PSecsDrv.S6F11RelatedEQPEvent.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTS6F11RelatedEQPParameter_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTS6F11RelatedEQPParameter_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS6F11RelatedEQPParameterClass dclsSF = (SDIWetEtch.CS6F11RelatedEQPParameterClass)objSF;

                this.PSecsDrv.S6F11RelatedEQPParameter.CopyMessage(dclsSF);
                this.PSecsDrv.S6F11RelatedEQPParameter.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTSS6F11EQPSpecifiedCtrl_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTSS6F11EQPSpecifiedCtrl_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS6F11EQPSpecifiedCtrlClass dclsSF = (SDIWetEtch.CS6F11EQPSpecifiedCtrlClass)objSF;

                this.PSecsDrv.S6F11EQPSpecifiedCtrl.CopyMessage(dclsSF);
                this.PSecsDrv.S6F11EQPSpecifiedCtrl.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTSS6F11S6F11RelatedMaterialEvent_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTSS6F11S6F11RelatedMaterialEvent_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS6F11RelatedMaterialEventClass dclsSF = (SDIWetEtch.CS6F11RelatedMaterialEventClass)objSF;

                this.PSecsDrv.S6F11RelatedMaterialEvent.CopyMessage(dclsSF);
                this.PSecsDrv.S6F11RelatedMaterialEvent.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTS6F13GLSAPD_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTS6F13GLSAPD_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS6F13GLSAPDClass dclsSF = (SDIWetEtch.CS6F13GLSAPDClass)objSF;

                this.PSecsDrv.S6F13GLSAPD.CopyMessage(dclsSF);
                this.PSecsDrv.S6F13GLSAPD.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTS6F13LOTAPD_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTS6F13LOTAPD_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS6F13LOTAPDClass dclsSF = (SDIWetEtch.CS6F13LOTAPDClass)objSF;

                this.PSecsDrv.S6F13LOTAPD.CopyMessage(dclsSF);
                this.PSecsDrv.S6F13LOTAPD.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subACTS7F107_Send()
        //  Description   : HOST로 Online 전환 요청을 한다.
        //  Parameters    : objSF => S/F의 인스턴스
        //  Return Value  : None
        //  Special Notes : S1F13 송신
        //*******************************************************************************
        //  2006/10/27          김 효주         [L 00]
        //******************************************************************************* 
        private void subACTS7F107_Send(object objSF)
        {
            try
            {
                if (this.PInfo.All.ControlState == "1" || this.PInfo.All.HostConnect == false) return;

                //형변환을 명시적으로 한다.
                SDIWetEtch.CS7F107Class dclsSF = (SDIWetEtch.CS7F107Class)objSF;

                this.PSecsDrv.S7F107.CopyMessage(dclsSF);
                this.PSecsDrv.S7F107.Request();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion
    }
}
