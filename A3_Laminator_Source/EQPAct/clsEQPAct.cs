using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using CommonAct;
using System.Data;
using System.Globalization;
using Microsoft.VisualBasic;
using System.IO;
using System.IO.Ports;
using InfoAct;
using ECCAct;

namespace EQPAct
{
    public class clsEQPAct
    {
    	public string Version
        {
            get { return "SMD A2 DMS CLEANER V1.0"; }
        }
        
        #region "선언"
        //외부 DLL인 구조체및 MTPLC의 정의
        public clsInfo PInfo = clsInfo.Instance;                                //외부에서 여기서 사용할 구조체를 넣어줌
        private clsECC PMTPLC;                                                  //EQP PLC 관련 ECC

        private System.Timers.Timer SVIDReadTimerThread;                        //PLC로 부터 SVID값을 읽기위한 Timer 정의
        private System.Threading.Thread pThreadSend;                            //자체 Queue를 검사하여 장비 PLC로 명령을 내리는 Thread 생성
        private System.Threading.Thread pThreadReceive;
        private System.Threading.Thread pEventCheckThread;
        private System.Threading.Thread pPPIDThread;

        private Queue APCStartCMDQueue = new Queue();

        ////RS232/485 -------------------------------------------------
        //public string PstrRS232CommPort;                            //통신 포트
        //public string PstrRS232Settings;                            //통신 설정 "9600,n,8,1"


        //TCP/IP ----------------------------------------------------
        public string PstrTCPIPAddress;
        public string PstrTCPIPPort;

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
        private int pPPIDReadCount = 20;                            //한번에 PLC로 부터 읽어오는(가져오는) PPID 개수
        private string PstrMCCBitReadDataBackup = "";               //MCC Bit Read Data Backup
        private long plngSEMAlarmReportTick = 0;
        private object SyncRoot;
        private object LockEventRead;
        private object LockEvent;
        public object LockSEMEventRead;

        //2012.07.17 Kim Youngsik
        private clsEQPEventManager m_pEQPEventList = new clsEQPEventManager();
        private clsCIMEventManager m_pCIMCommandList = new clsCIMEventManager();

        private bool bolReady = true;
        private int intEventCount = 0;
        private Stopwatch stopWatch = new Stopwatch();

        private StringBuilder psbBuffer = new StringBuilder();
        private clsECC PMCC;
        private Thread pSEMRecvThread = null; //20141106 이원규 (SEM_UDP)
        private StringBuilder pBuffer = new StringBuilder();

        #endregion

        #region"Open and Close"
        /// <summary>
        /// PLC Open(PLC의 Scan영역등의 속성값을 설정한 후 오픈한다)
        /// </summary>
        /// <param name="intCommunication"></param>
        /// <returns>성공 => True, 실패 => False</returns>
        public Boolean funOpenPLC(EnuEQP.CommunicationType intCommunication)
        {
            Boolean dbolReturn = false;
            string dstrKey;
            try
            {
                this.PMTPLC = new ECCAct.clsECC();
                this.SyncRoot = this.pReadDataQueue.SyncRoot;
                this.LockEventRead = new object();
                LockEvent = new object();
                this.LockSEMEventRead = new object();
                //AddressMAP(SYSTEM.MDB)이 있는 디렉토리를 지정한다                
                PMTPLC.proPlcDBPath = PstrAddressPath + @"\System_"+ PInfo.All.MODEL_NAME +".mdb";
                PMTPLC.proEQPCommandType = ECCCommonAct.EnuCommunication.EQPCommandType.PLC;                   //PLC형 Command사용
                PMTPLC.proEQUIPMENT = ECCCommonAct.EnuCommunication.EQPNameType.MelsecPLC;                     //장비선택
                PMTPLC.proCommunicationType = ECCCommonAct.EnuCommunication.CommunicationType.MXComponent;     //통신방식 선택
                PMTPLC.proLogicalStationNumber = 255;     //MX Component에서 설정한 Logical Station Number(NETG 용임)

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

                //Process Working Set
                SetProcessWorkingSet(1, 3);

                //20141106 이원규 (SEM_UDP)
                this.pSEMRecvThread = new Thread(new ThreadStart(SEMRecvWorker));
                this.pSEMRecvThread.Name = "SEM_RECV_WORKER";
                this.pSEMRecvThread.IsBackground = true;
                this.pSEMRecvThread.Start();

                //SVIDRead_Tick Timer 설정
                this.SVIDReadTimerThread = new System.Timers.Timer();
                this.SVIDReadTimerThread.Elapsed += new ElapsedEventHandler(SVIDRead_Tick);          
                this.SVIDReadTimerThread.Interval = 1000;       //2000ms
                this.SVIDReadTimerThread.Enabled = true;
                //GC.KeepAlive(this.SVIDReadTimerThread);

                //자체 Queue를 검사하여 장비 PLC로 명령을 내리는 Thread 생성
                this.pThreadSend = new Thread(new ThreadStart(PLCWriteMethodThread));
                this.pThreadSend.Name = "PLCWriteCommand";
                this.pThreadSend.IsBackground = true;
                this.pThreadSend.Start();

                this.pThreadReceive = new Thread(new ThreadStart(PLCReadMethodThread));
                this.pThreadReceive.Name = "PLCReadCommnad";
                this.pThreadReceive.IsBackground = true;
                this.pThreadReceive.Start();

                //2015-03-24 고석현 추가 - 추가
                this.pEventCheckThread = new Thread(new ThreadStart(EventCheckThread));
                this.pEventCheckThread.Name = "EventCheck";
                this.pEventCheckThread.IsBackground = true;
                this.pEventCheckThread.Start();


                //pPPIDThread = new Thread(new ThreadStart(PPIDMethodThread));
                //pPPIDThread.Name = "PPIDThread";
                //pPPIDThread.IsBackground = true;
                //pPPIDThread.Start();

                //2012.07.17 Kim Youngsik
                m_pEQPEventList.funCreateEQPEvent("Main");
                m_pCIMCommandList.funCreateCIMEvent("Main");

                m_pEQPEventList.funSetEqpAct(this);
                m_pCIMCommandList.funSetEqpAct(this);

                //foreach (int unitID in PInfo.Unit())
                //{

                //}

                //Event Bit를 모두 초기화한다.
                //==>코딩
                if (PInfo.EQP("Main").DummyPLC)
                {
                    //funWordWrite("W2A00", "65535", EnuEQP.PLCRWType.Int_Data);
                }

                //if (this.PInfo.All.CommPort != "NULL")
                //{
                //    //포트연결시도를한다.
                //    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SerialPortOpen);
                //    //SEM Controller에 Start명령을 내린다.
                //    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerStart);
                //    this.PInfo.EQP("Main").RS232Connect = true;
                //    this.PInfo.All.SEM_ErrorFlag = false;
                //}

                if (PInfo.All.USE_UDP == true && this.PInfo.EQP("Main").UDPConnect == false && PInfo.EQP("Main").UDPStart == false)
                {
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.UDPPortOpen);

                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerStart);
                }


                PMCC = new clsECC();
                PMCC.proPlcDBPath = PstrAddressPath + @"\System_" + PInfo.All.MODEL_NAME + ".mdb";
                PMCC.proEQPCommandType = ECCCommonAct.EnuCommunication.EQPCommandType.PC;                    //PC형 Command사용
                PMCC.proEQUIPMENT = ECCCommonAct.EnuCommunication.EQPNameType.Normal;                          //장비선택
                PMCC.proCommunicationType = ECCCommonAct.EnuCommunication.CommunicationType.TCPIP;     //통신방식 선택

                PMCC.proDummy = false; //this.PInfo.EQP("Main").DummyPC;

                //Stansdard
                PMCC.proTimeOutTransfer = 5000;
                PMCC.proRetryTransferCount = 0;

                //TCP 통신 방법에 대한 설정
                //PMCC.proTCPRemoteIP = this.PInfo.EQP("Main").LocalIP;
                //PMCC.proTCPRemotePort = Convert.ToInt32(this.PInfo.EQP("Main").LocalPort);
                PMCC.proTCPLocalPort = 7050;     //Server일 경우

                PMCC.proTCPSendBufferSize = 1024;
                PMCC.proTCPReceiveBufferSize = 1024;
                PMCC.proTCPReceiveDataType = ECCCommonAct.EnuCommunication.ReceiveDataType.ASCII;          //Data를 수신하면 ASC로 받는다.
                PMCC.proTCPConnectionMode = ECCCommonAct.EnuCommunication.ConnectionMode.Passive;          //Server

                PMCC.proEQPName = "CIM->MCC";
                dbolReturn = PMCC.funOpenConnection();


            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intCommunication:" + intCommunication);
            }
            return dbolReturn;
        }

        /// <summary>
        /// 2015-03-24 고석현 추가 - 추가
        /// </summary>
        void EventCheckThread()
        {
            string strReadData = "";
            do
            {
                lock (LockEvent)
                {
                    #region B1119 ProcessDataCheck 이벤트 처리
                    if (funBitRead("B1119", 1, true) == "1")
                    {
                        if (PInfo.All.B1119Flag == false)
                        {
                            PInfo.All.B1119Flag = true;
                            string[] strtemp = new string[1] { "B1019" };
                            m_pEQPEventList.funArrangeEQPEvent("actProcessDataCheck", strtemp);
                        }
                    }
                    else
                    {
                        if (PInfo.All.B1119Flag)
                        {
                            funBitWrite("B1019", "0");
                            PInfo.All.B1119Flag = false;

                            this.JobStart();
                        }
                    }
                    #endregion

                    
                    #region HostPPIDMapping 비트 체크


                    strReadData = funBitRead("B113C", 2, true);
                    //Port01
                    if (strReadData != null && strReadData.Length == 2)
                    {
                        if (strReadData.Substring(0, 1) == "1")
                        {
                            if (PInfo.All.B113CFlag == false)
                            {
                                PInfo.All.B113CFlag = true;
                                string strHostPPID = funWordRead("W2520", 10, EnuEQP.PLCRWType.ASCII_Data).Trim();

                                if (PInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPID) != null)
                                {
                                    funWordWrite("W100E", PInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPID).EQPPPID, EnuEQP.PLCRWType.Int_Data);
                                }
                                else
                                {
                                    funWordWrite("W100E", "-1", EnuEQP.PLCRWType.Int_Data);
                                }

                                funBitWrite("B103C", "1");
                            }
                        }
                        else
                        {
                            if (PInfo.All.B113CFlag)
                            {
                                funBitWrite("B103C", "0");
                                PInfo.All.B113CFlag = false;
                            }
                        }

                        //Port02
                        if (strReadData.Substring(1, 1) == "1")
                        {
                            if (PInfo.All.B113DFlag == false)
                            {
                                PInfo.All.B113DFlag = true;
                                string strHostPPID = funWordRead("W2530", 10, EnuEQP.PLCRWType.ASCII_Data).Trim();

                                if (PInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPID) != null)
                                {
                                    funWordWrite("W100F", PInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPID).EQPPPID, EnuEQP.PLCRWType.Int_Data);
                                }
                                else
                                {
                                    funWordWrite("W100F", "-1", EnuEQP.PLCRWType.Int_Data);
                                }
                                funBitWrite("B103D", "1");
                            }
                        }
                        else
                        {
                            if (PInfo.All.B113DFlag)
                            {
                                funBitWrite("B103D", "0");
                                PInfo.All.B113DFlag = false;
                            }
                        }
                    }
                    #endregion

                    //[2015/06/05]Normal작업지시 Bit가 정상적으로 꺼지지 않아 State Bit로 변경(Add by HS)
                    if (funBitRead("B1016", 1, true) == "1" && funBitRead("B1116", 1, true) == "1")
                    {
                        funBitWrite("B1016", "0", true);
                    }
                    //[2015/06/03]미사용(Comment by HS)
                    ////[2015/03/27] Normal작업지시 Bit가 꺼지지 않는 현상으로 인하여..(Add by HS)
                    //if (funBitRead("B1116", 1, true) == "0")
                    //{
                    //    if (PInfo.All.B1016Flag == true)
                    //    {
                    //        funBitWrite("B1016", "0");
                    //        PInfo.All.B1016Flag = false;
                    //    }
                    //}
                    //else
                    //{
                    //    if (PInfo.All.B1016Flag == true)
                    //    {
                    //        funBitWrite("B1016", "0");
                    //    }
                    //}
                    //if (funBitRead("B1117", 1, true) == "0")
                    //{
                    //    if (funBitRead("B1017", 1, true) == "1")
                    //    {
                    //        funBitWrite("B1017", "0");
                    //    }
                    //}
                    //if (funBitRead("B1118", 1, true) == "0")
                    //{
                    //    if (funBitRead("B1018", 1, true) == "1")
                    //    {
                    //        funBitWrite("B1018", "0");
                    //    }
                    //}
                }
                Thread.Sleep(400);
            }
            while (true);
        }

        #region  "SEM"
        /// <summary>
        /// SEM Recv Worker
        ///  //20141106 이원규 (SEM_UDP)
        /// </summary>
        void SEMRecvWorker()
        {
            System.Text.RegularExpressions.MatchCollection regexMC;
            int sindex = -1;
            byte[] recvBytes = null;

            do
            {
                lock (this.LockSEMEventRead)
                {
                    try
                    {
                        if (PInfo.All.UDPRecvPort != null)
                        {
                            try
                            {
                                PInfo.All.UDPRecvPort.Client.ReceiveTimeout = 500;
                                recvBytes = PInfo.All.UDPRecvPort.Receive(ref PInfo.All.UDPLocalEndPoint);
                            }
                            catch 
                            {
                                PInfo.All.SEM_ErrorFlag = true;
                            }

                            if (recvBytes != null && recvBytes.Length > 0)
                            {
                                lock (this.pBuffer) this.pBuffer.Append(Encoding.Default.GetString(recvBytes));

                                PInfo.All.SEM_ErrorFlag = false;
                            }
                        }

                        if (this.pBuffer.Length > 0)
                        {
                            lock (this.pBuffer) regexMC = System.Text.RegularExpressions.Regex.Matches(this.pBuffer.ToString(), @"[$].*(\r\n)");

                            if (regexMC.Count > 0)
                            {
                                for (int i = 0; i < regexMC.Count; i++)
                                {
                                    subSEMDataRecv(regexMC[i].Value);
                                    this.pBuffer.Replace(regexMC[i].Value, string.Empty);
                                }
                            }

                            sindex = -1;

                            lock (this.pBuffer)
                            {
                                sindex = this.pBuffer.ToString().IndexOf('$');

                                if (sindex > 0) this.pBuffer.Remove(0, sindex);
                             }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    }
                    finally
                    {
                        Thread.Sleep(100);
                        
                        recvBytes = null;
                    }
                }
            }
            while (true);
        }

        //20141106 이원규 (SEM_UDP)
        private void subSEMDataRecv(string dstrReceiveData)
        {
            string[] darrReceiveData;
            string[] darrValue;
            int dintSVID = 0;
            int dintAlarmID = 0;
            string dstrAlarmDesc = "";
            byte[] darrSendTemp;

            try
            {
                if (dstrReceiveData.Equals("$START\r\n"))
                {
                    darrSendTemp = Encoding.ASCII.GetBytes("$ACK\r\n");
                    if (PInfo.All.UDPSendPort != null) PInfo.All.UDPSendPort.Send(darrSendTemp, darrSendTemp.Length, PInfo.All.UDPRemoteEndPoint);
                }
                else if (dstrReceiveData.Equals("$END\r\n"))
                {
                    darrSendTemp = Encoding.ASCII.GetBytes("$ACK\r\n");
                    if (PInfo.All.UDPSendPort != null) PInfo.All.UDPSendPort.Send(darrSendTemp, darrSendTemp.Length, PInfo.All.UDPRemoteEndPoint);
                }
                else if (dstrReceiveData.Equals("$ACK"))
                {
                    if (PInfo.All.SEMStartReplyCheck == true)
                    {
                        PInfo.All.SEMStartReplyCheck = false;       //ACK Reply를 받았음
                        PInfo.All.SEMControllerConnect = true;      //SEM Controller Connection 설정
                        //Light Alarm 발생 해제(AlarmID = 1000000고정, Alarm Text : Samsung Environment Monitoring Interface 고정)
                        if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(1000000) != null) subSEMAlarm(1000000, "Samsung Environment Monitoring Interface", "R");
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller Start!!");
                    }
                    else
                    {
                        PInfo.All.SEMControllerConnect = false;      //SEM Controller Disconnection 설정
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller End!!");
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.UDPPortClose);
                    }
                }
                else if (dstrReceiveData.Contains("$ERROR") == true)
                {
                    //AlarmID를 가져온다
                    dintAlarmID = Convert.ToInt32(FunStringH.funGetMiddleString(dstrReceiveData, "(", ")"));
                    darrValue = dstrReceiveData.Split('=');
                    dstrAlarmDesc = darrValue[1];

                    //Alarm 발생보고
                    subSEMAlarm(dintAlarmID, dstrAlarmDesc, "S");

                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller Error : " + dstrReceiveData);

                    //SEM 알람이 일정카운트 만큼 지속적으로 발생시 SEM 컨트롤러 종료시킴 - 11.11.22 ksh
                    if (this.PInfo.All.SEM_ErrorCount > this.PInfo.All.SEM_ErrorCheckCount)
                    {
                        // SEM컨트럴러 Error 발생 저장 - 11.10.04 ksh
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerEnd);
                        this.PInfo.All.SEM_ErrorFlag = true;
                        //pInfo.All.SEMControllerConnect = false;      //SEM Controller Disconnection 설정
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller End!!");
                        this.PInfo.All.SEM_ErrorCount = 0;
                    }
                    else
                    {
                        this.PInfo.All.SEM_ErrorCount++;
                    }
                }
                else  //수신데이타 처리
                {

                    /* 20120315 수정내용 적용시
                     * SEM에서 수신된 패킷의 유효한 데이터만 업데이트 된다.
                     */
                    funGetCorrectData(dstrReceiveData, out darrReceiveData);       // 20120315 이상창

                    if (darrReceiveData != null)                                // 20120315 이상창
                    {
                        for (int intLoop = 0; intLoop < darrReceiveData.Length; intLoop++)
                        {
                            //SVID를 가져온다
                            dintSVID = Convert.ToInt32(FunStringH.funGetMiddleString(darrReceiveData[intLoop], "(", ")"));
                            //기존에 Alarm이 발생해있으면 해제보고를 한다
                            if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSVID) != null) subSEMAlarm(dintSVID, "", "R");

                            darrValue = darrReceiveData[intLoop].Split('=');
                            if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID) == null)
                            {
                                this.PInfo.Unit(0).SubUnit(0).AddSVID(dintSVID);
                                this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name = darrValue[0].Remove(darrValue[0].IndexOf("("));
                                this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).SVID = dintSVID;
                                this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value = darrValue[1];
                                this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).ModuleID = PInfo.Unit(3).SubUnit(0).ModuleID;
                                this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Type = "FLOAT";
                                this.PInfo.All.SVIDPLCNotReadLength = this.PInfo.All.SVIDPLCNotReadLength + 1;
                            }
                            else
                            {
                                this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name = darrValue[0].Remove(darrValue[0].IndexOf("("));
                                this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value = darrValue[1];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                darrReceiveData = null;
                darrValue = null;
                dstrReceiveData = "";
            }
        }

        private void funSemDataReceived()
        {
            //SerialPort spSerialPort2;
            string[] darrReceiveData;
            string[] darrValue;
            string dstrReceiveData = "";
            int dintSVID = 0;
            int dintAlarmID = 0;
            string dstrAlarmDesc = "";

            try
            {
                dstrReceiveData = PInfo.All.spSerialPort.ReadLine();
                //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.SEM, dstrReceiveData.ToString());

                //dstrReceiveData = spSerialPort.ReadExisting();
                if (dstrReceiveData == "$START" || dstrReceiveData == "$END" || dstrReceiveData == "") return;

                //Start Commend에 대한 Reply Check
                if (PInfo.All.SEMStartReplyCheck == true && dstrReceiveData.Contains("ACK") == true)        //Start Reply Msg
                {
                    PInfo.All.SEMStartReplyCheck = false;       //ACK Reply를 받았음
                    PInfo.All.SEMControllerConnect = true;      //SEM Controller Connection 설정
                    //Light Alarm 발생 해제(AlarmID = 1000000고정, Alarm Text : Samsung Environment Monitoring Interface 고정)
                    if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(1000000) != null) subSEMAlarm(1000000, "Samsung Environment Monitoring Interface", "R");
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller Start!!");
                    return;
                }
                else
                {
                    //End Reply Msg
                    if (dstrReceiveData.Contains("ACK") == true)
                    {
                        PInfo.All.SEMControllerConnect = false;      //SEM Controller Disconnection 설정
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller End!!");
                        //확인필요
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.UDPPortClose);
                        return;
                    }
                }

                if (dstrReceiveData.Contains("ERROR") == true)  //Error 수신
                {
                    //AlarmID를 가져온다
                    dintAlarmID = Convert.ToInt32(FunStringH.funGetMiddleString(dstrReceiveData, "(", ")"));
                    darrValue = dstrReceiveData.Split('=');
                    dstrAlarmDesc = darrValue[1];

                    //Alarm 발생보고
                    //subSEMAlarm(dintAlarmID, dstrAlarmDesc, "S");

                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller Error : " + dstrReceiveData);

                    //SEM 알람이 일정카운트 만큼 지속적으로 발생시 SEM 컨트롤러 종료시킴 - 11.11.22 ksh
                    if (this.PInfo.All.SEM_ErrorCount > this.PInfo.All.SEM_ErrorCheckCount)
                    {
                        // SEM컨트럴러 Error 발생 저장 - 11.10.04 ksh
                        this.PInfo.All.SEM_ErrorFlag = true;
                        PInfo.All.SEMControllerConnect = false;      //SEM Controller Disconnection 설정
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller End!!");
                        //확인필요
                        PInfo.subPLCCommand_Set(clsInfo.PLCCommand.UDPPortClose);
                        this.PInfo.All.SEM_ErrorCount = 0;
                    }
                    else
                    {
                        this.PInfo.All.SEM_ErrorCount++;
                    }
                    return;
                }
                else  //수신데이타 처리
                {

                    dstrReceiveData = dstrReceiveData.Replace("$", "");
                    //dstrReceiveData = dstrReceiveData.Replace("\r\n", "");
                    darrReceiveData = dstrReceiveData.Split(',');

                    PInfo.All.SEMStartReplyCheck = false;       //ACK Reply를 받았음
                    PInfo.All.SEMStartReplyCheckTime = DateTime.Now.Ticks;
                    if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(1000000) != null) subSEMAlarm(1000000, "Samsung Environment Monitoring Interface", "R");

                    for (int intLoop = 0; intLoop <= darrReceiveData.Length - 1; intLoop++)
                    {
                        //SVID를 가져온다
                        dintSVID = Convert.ToInt32(FunStringH.funGetMiddleString(darrReceiveData[intLoop], "(", ")"));
                        //기존에 Alarm이 발생해있으면 해제보고를 한다
                        if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSVID) != null) subSEMAlarm(dintSVID, "", "R");

                        darrValue = darrReceiveData[intLoop].Split('=');
                        if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID) == null)
                        {
                            this.PInfo.Unit(0).SubUnit(0).AddSVID(dintSVID);
                            this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name = darrValue[0].Remove(darrValue[0].IndexOf("("));
                            this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).SVID = dintSVID;
                            this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value = darrValue[1];
                            this.PInfo.All.SVIDPLCNotReadLength = this.PInfo.All.SVIDPLCNotReadLength + 1;
                        }
                        else
                        {
                            this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name = darrValue[0].Remove(darrValue[0].IndexOf("("));
                            this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value = darrValue[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());

                if (this.PInfo.All.SEM_ErrorCount > this.PInfo.All.SEM_ErrorCheckCount)
                {
                    // SEM컨트럴러 Error 발생 저장 - 11.10.04 ksh
                    this.PInfo.All.SEM_ErrorFlag = true;
                    PInfo.All.SEMControllerConnect = false;      //SEM Controller Disconnection 설정
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller End!!");
                    //확인필요
                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.UDPPortClose);
                    this.PInfo.All.SEM_ErrorCount = 0;
                }
                else
                {
                    this.PInfo.All.SEM_ErrorCount++;
                }
            }
            finally
            {
                //spSerialPort = null;
                darrReceiveData = null;
                darrValue = null;
                dstrReceiveData = "";
            }
        }
        #endregion

        public static void SetProcessWorkingSet(int minMB, int maxMB)
        {
            Process currentProcess = Process.GetCurrentProcess();
            IntPtr iptrMinWorkingSet = minMB > 0 ? new IntPtr(minMB * 1024 * 1024) : new IntPtr(204800);
            IntPtr iptrMaxWorkingSet = new IntPtr(maxMB * 1024 * 1024);
            currentProcess.MinWorkingSet = iptrMinWorkingSet;
            currentProcess.MaxWorkingSet = iptrMaxWorkingSet;
        }

        /// <summary>
        /// 사용중인 Thread를 Abort하고 PLC를 닫는다.
        /// </summary>
        /// <returns></returns>
        public Boolean funClosePLC()
        {
            Boolean dbolReturn = false;

            try
            {
                

                if (this.SVIDReadTimerThread != null)
                {
                    this.SVIDReadTimerThread.Enabled = false;
                    this.SVIDReadTimerThread.Close();
                }
                if (this.pThreadSend != null) this.pThreadSend.Abort();
                if (this.pThreadReceive != null) this.pThreadReceive.Abort();
                if (this.pEventCheckThread != null) this.pEventCheckThread.Abort();

                PMTPLC.funCloseConnection();

                dbolReturn = true;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            return dbolReturn;
        }

       
      
        #endregion

        #region"Serial Port Data 처리"
        
        /// <summary>
        /// SEM 에서 받은 패킷에서 유효한 데이터 값추출 ("Name(SVID)=VALUE")
        /// </summary>
        /// <param name="strRecvData">SEM에서 받은 패킷</param>
        /// <returns>추출한 유효 데이터 String[] 또는 null</returns>
        private void funGetCorrectData(string strRecvData, out string[] darrTemp)
        {
            //string[] darrTemp = null;

            try
            {
                /* strRecvData에서 "Name(SVID)=VALUE" 해당 구문만 추출
                 *
                 *  - 구문 시작    : '$' 또는 ','                 
                 *  - Name         : 대문자,숫자,_ 조합
                 *  - SVID         : 60000번대 숫자
                 *  - VALUE        : 소수점 유무에 따른 형태 확인 
                 */
                System.Text.RegularExpressions.MatchCollection regexMC =
                    System.Text.RegularExpressions.Regex.Matches(strRecvData, @"[,$][_A-Z0-9]+\(6[0-9]{4}\)=-{0,1}(?([0-9]+[{.}])([0-9]+[{.}][0-9]+)|([0-9]+))", System.Text.RegularExpressions.RegexOptions.Compiled);


                darrTemp = new string[regexMC.Count];

                for (int i = 0; i < regexMC.Count; i++)
                {
                    darrTemp[i] = regexMC[i].Value.Replace("$", "");
                    darrTemp[i] = darrTemp[i].Replace(",", "");
                }
            }
            catch (Exception ex)
            { 
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                darrTemp = null;
            }

            //return darrTemp;
        }

        private void ComPortDataRecv()
        {
            System.Text.RegularExpressions.MatchCollection regexMC;

            int sindex = -1;

            try
            {
                if (PInfo.All.spSerialPort != null)
                {
                    if (PInfo.All.spSerialPort.IsOpen && PInfo.All.spSerialPort.BytesToRead > 0) this.psbBuffer.Append(PInfo.All.spSerialPort.ReadExisting());

                    if (this.psbBuffer.Length > 0)
                    {
                        // 라인을 찾아보자

                        regexMC = System.Text.RegularExpressions.Regex.Matches(this.psbBuffer.ToString(), @"[$].*(\r\n)", System.Text.RegularExpressions.RegexOptions.Compiled);

                        if (regexMC.Count > 0)
                        {
                            for (int i = 0; i < regexMC.Count; i++)
                            {
                                // 패킷 수신된거다... 넘기자..
                                subSEMDataRecv(regexMC[i].Value);
                                this.psbBuffer.Replace(regexMC[i].Value, string.Empty);
                            }
                        }

                        sindex = this.psbBuffer.ToString().IndexOf('$');

                        if (sindex > 0)
                        {
                            this.psbBuffer.Remove(0, sindex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #region 사용안함
        //private void subSEMDataRecv(string dstrReceiveData)
        //{
        //    string[] darrReceiveData;
        //    string[] darrValue;
        //    int dintSVID = 0;
        //    int dintAlarmID = 0;
        //    string dstrAlarmDesc = "";

        //    try
        //    {
        //        if (dstrReceiveData.Equals("$START\r\n"))
        //        {
        //            PInfo.All.spSerialPort.DiscardOutBuffer();
        //            PInfo.All.spSerialPort.WriteLine("$ACK");
        //        }
        //        else if (dstrReceiveData.Equals("$END\r\n"))
        //        {
        //            PInfo.All.spSerialPort.DiscardInBuffer();
        //            PInfo.All.spSerialPort.DiscardOutBuffer();
        //            PInfo.All.spSerialPort.WriteLine("$ACK");
        //        }
        //        else if (dstrReceiveData.Equals("$ACK"))
        //        {
        //            if (PInfo.All.SEMStartReplyCheck == true)
        //            {
        //                PInfo.All.SEMStartReplyCheck = false;       //ACK Reply를 받았음
        //                PInfo.All.SEMControllerConnect = true;      //SEM Controller Connection 설정
        //                //Light Alarm 발생 해제(AlarmID = 1000000고정, Alarm Text : Samsung Environment Monitoring Interface 고정)
        //                if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(1000000) != null) subSEMAlarm(1000000, "Samsung Environment Monitoring Interface", "R");
        //                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller Start!!");
        //            }
        //            else
        //            {
        //                PInfo.All.SEMControllerConnect = false;      //SEM Controller Disconnection 설정
        //                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller End!!");
        //                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.SerialPortClose);
        //            }
        //        }
        //        else if (dstrReceiveData.Contains("$ERROR") == true)
        //        {
        //            //AlarmID를 가져온다
        //            dintAlarmID = Convert.ToInt32(FunStringH.funGetMiddleString(dstrReceiveData, "(", ")"));
        //            darrValue = dstrReceiveData.Split('=');
        //            dstrAlarmDesc = darrValue[1];

        //            //Alarm 발생보고
        //            //subSEMAlarm(dintAlarmID, dstrAlarmDesc, "S");

        //            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller Error : " + dstrReceiveData);

        //            //SEM 알람이 일정카운트 만큼 지속적으로 발생시 SEM 컨트롤러 종료시킴 - 11.11.22 ksh
        //            if (this.PInfo.All.SEM_ErrorCount > this.PInfo.All.SEM_ErrorCheckCount)
        //            {
        //                // SEM컨트럴러 Error 발생 저장 - 11.10.04 ksh
        //                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerEnd);
        //                this.PInfo.All.SEM_ErrorFlag = true;
        //                //PInfo.All.SEMControllerConnect = false;      //SEM Controller Disconnection 설정
        //                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SEM Controller End!!");
        //                this.PInfo.All.SEM_ErrorCount = 0;
        //            }
        //            else
        //            {
        //                this.PInfo.All.SEM_ErrorCount++;
        //            }
        //        }
        //        else  //수신데이타 처리
        //        {

        //            /* 20120315 수정내용 적용시
        //             * SEM에서 수신된 패킷의 유효한 데이터만 업데이트 된다.
        //             */
        //            funGetCorrectData(dstrReceiveData, out darrReceiveData);       // 20120315 이상창

        //            if (darrReceiveData != null)                                // 20120315 이상창
        //            {
        //                for (int intLoop = 0; intLoop < darrReceiveData.Length; intLoop++)
        //                {
        //                    //SVID를 가져온다
        //                    dintSVID = Convert.ToInt32(FunStringH.funGetMiddleString(darrReceiveData[intLoop], "(", ")"));
        //                    //기존에 Alarm이 발생해있으면 해제보고를 한다
        //                    if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSVID) != null) subSEMAlarm(dintSVID, "", "R");

        //                    darrValue = darrReceiveData[intLoop].Split('=');
        //                    if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID) == null)
        //                    {
        //                        this.PInfo.Unit(0).SubUnit(0).AddSVID(dintSVID);
        //                        this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name = darrValue[0].Remove(darrValue[0].IndexOf("("));
        //                        this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).SVID = dintSVID;
        //                        this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value = darrValue[1];
        //                        this.PInfo.All.SVIDPLCNotReadLength = this.PInfo.All.SVIDPLCNotReadLength + 1;
        //                    }
        //                    else
        //                    {
        //                        this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Name = darrValue[0].Remove(darrValue[0].IndexOf("("));
        //                        this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value = darrValue[1];
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
        //    }
        //    finally
        //    {
        //        darrReceiveData = null;
        //        darrValue = null;
        //        dstrReceiveData = "";
        //    }
        //}
        #endregion 

        /// <summary>
        /// SEM Controller에 Start지시를 보낸다
        /// </summary>
        private void subSEMStart()
        {
            try
            {
                PInfo.All.spSerialPort.WriteLine("$START");
                PInfo.All.SEMStartReplyCheck = true;       //ACK Reply Check설정
                PInfo.All.SEMStartReplyCheckTime = DateTime.Now.Ticks;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// SEM Controller에 End지시를 보내고 Port를 닫는다
        /// </summary>
        private void subSEMEnd()
        {
            try
            {
                if (PInfo.All.SEMControllerConnect == true)
                {
                    PInfo.All.spSerialPort.WriteLine("$END");
                    funComPortClose();                           //Port Close
                }
                else
                {
                    funComPortClose();                           //Port Close
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Serial Port를 Close한다
        /// </summary>
        /// <returns></returns>
        private void funComPortClose()
        {
            try
            {
                this.PInfo.All.SEMEndReplyCheck = false;

                lock (this.psbBuffer)
                {
                    this.psbBuffer.Remove(0, this.psbBuffer.Length);
                }

                if (PInfo.All.spSerialPort != null)
                {
                    PInfo.All.spSerialPort.DiscardInBuffer();
                    PInfo.All.spSerialPort.DiscardOutBuffer();

                    PInfo.All.spSerialPort.Close();                             //포트를 닫는다

                    //시리얼 포트 연결 상태를 가져온다.
                    PInfo.EQP("Main").RS232Connect = PInfo.All.spSerialPort.IsOpen;

                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SerialPort : " + PInfo.All.spSerialPort.PortName + " CLOSE");

                    PInfo.All.spSerialPort.Dispose();
                    PInfo.All.spSerialPort = null;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

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

                subEQPPPIDRead();

                PInfo.All.CurrentHOSTPPID = funWordRead("W2003", 10, EnuEQP.PLCRWType.ASCII_Data);
                PInfo.All.CurrentEQPPPID = funWordRead("W200D",1, EnuEQP.PLCRWType.Int_Data);

                //PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ECIDRead);   //ECID를 PLC로 부터 읽는다.
                PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, 1);   //ECID를 PLC로 부터 읽는다.

                //[2015/03/10]Recovery(Add by HS)
                PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.Recovery);
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
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
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

                if (strAddress.Substring(1,2)!= "10" && dstrDesc != "Alive")
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
                        //bool dbolXPCStart = false;
                        //if (this.PInfo.Unit(0).SubUnit(0).EOID(this.PInfo.funGetEOIDNameToIndex("APC")).EOV == 1 && PInfo.APC(PInfo.All.DataCheckGLSID) != null)
                        //{
                        //    PInfo.All.APCStartEQPPPIDCheck = true;
                        //    clsMappingEQPPPID CurrentPPID = PInfo.Unit(0).SubUnit(0).MappingEQPPPID(PInfo.APC(PInfo.All.DataCheckGLSID).EQPPPID);
                        //    //CurrentPPID.PPIDCommand = clsInfo.PPIDCMD.APC;
                        //    PInfo.SetPPIDCMD(PInfo.APC(PInfo.All.DataCheckGLSID).EQPPPID);
                        //    dbolXPCStart = true;
                        //}
                        //else if (this.PInfo.Unit(0).SubUnit(0).EOID(this.PInfo.funGetEOIDNameToIndex("RPC")).EOV == 1 && PInfo.RPC(PInfo.All.DataCheckGLSID) != null)
                        //{
                        //    PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.RPCStart, PInfo.All.DataCheckGLSID);
                        //    //pInfo.All.RPCDBUpdateCheck = true;
                        //    dbolXPCStart = true;
                        //}

                        //if (dbolXPCStart == false)
                        //{
                        //    PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.NormalStart, PInfo.All.DataCheckGLSID);
                        //}
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
                    case "actSetupPPID":

                        if (this.PInfo.All.EQPPPIDCommandCount > 0)   
                        {
                            //1개 감소
                            this.PInfo.All.EQPPPIDCommandCount = this.PInfo.All.EQPPPIDCommandCount - 1;

                            if (this.PInfo.All.EQPPPIDCommandCount <= 0)
                            {
                                this.PInfo.All.EQPPPIDCommandCount = 0;    //초기화

                                //EQP PPID가 끝나면 HOST PPID를 요청한다.
                                if (this.PInfo.All.HOSTPPIDCommandCount > 0)
                                {
                                    this.PInfo.All.SetUpPPIDPLCWriteCount = 0;  //PLC에 Write한 회수 초기화

                                    subSetupPPIDCmd(false, 2);          //HOST PPID 정보 요청 지시(처음)
                                }
                                else
                                {
                                    //EQP PPID는 끝났는데 HOST PPID 개수가 0인 경우 여기서 바로 종료한다.
                                    if (this.PInfo.All.isReceivedFromHOST == true)
                                    {
                                        this.PInfo.All.isReceivedFromHOST = false;      //초기화
                                        this.PInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                                    }
                                    else if (this.PInfo.All.isReceivedFromCIM == true)
                                    {
                                        this.PInfo.All.isReceivedFromCIM = false;       //초기화
                                        this.PInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                                    }

                                }
                            }
                            else
                            {
                                //다음 EQP PPID 정보를 요청한다.
                                subSetupPPIDCmd(false, 1);          //EQP PPID 정보 요청 지시(Next 정보)
                            }
                        }
                        else if (this.PInfo.All.HOSTPPIDCommandCount > 0)
                        {
                            //1개 감소
                            this.PInfo.All.HOSTPPIDCommandCount = this.PInfo.All.HOSTPPIDCommandCount - 1;

                            //EQP PPID 동기화가 완료가 되었으면 HOSTPPID 동기화는 시작한다.
                            if (this.PInfo.All.HOSTPPIDCommandCount <= 0)
                            {
                                this.PInfo.All.HOSTPPIDCommandCount = 0;    //초기화


                                //EQP, HOST PPID 정보 요청 지시 모두 완료됨.
                                if (this.PInfo.All.isReceivedFromHOST == true)
                                {
                                    this.PInfo.All.isReceivedFromHOST = false;      //초기화
                                    this.PInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                                }
                                else if (this.PInfo.All.isReceivedFromCIM == true)
                                {
                                    this.PInfo.All.isReceivedFromCIM = false;       //초기화
                                    this.PInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                                }

                            }
                            else
                            {
                                //다음 HOST PPID 정보를 요청한다.
                                subSetupPPIDCmd(false, 2);      //HOST PPID 시작
                            }
                        }

                        break;

                    case "actOnePPIDInfoRequest":

                        //HOST에서 S7F23 수신시 등 여러 번의 정보 요청 지시가 필요할때 여기를 완료 시점으로 잡는다.
                        if (this.PInfo.All.isReceivedFromHOST == true)
                        {
                            this.PInfo.All.isReceivedFromHOST = false;      //초기화
                            this.PInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                        }

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strACTName:" + strACTName);
            }
        }

        public void subWaitDuringReadFromPLC()
        {
            long dlngTimeTick = 0;
            long dlngSec = 0;
            int dintTimeOut = 40;   //TimeOut은 20초로 함
            try
            {
                PInfo.All.PLCActionEnd2 = false;
                dlngTimeTick = DateTime.Now.Ticks;

                while (PInfo.All.PLCActionEnd2 == false)
                {
                    dlngSec = DateTime.Now.Ticks - dlngTimeTick;
                    if (dlngSec < 0) dlngTimeTick = 0;
                    if (dlngSec > dintTimeOut * 10000000 || PInfo.All.PLCActionEnd2 == true)
                    {
                        PInfo.All.PLCActionEnd2 = false;        //초기화
                        PInfo.All.PLCActionEnd = false;
                        return;
                    }

                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
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

        #region"Timer"
        private void subPLCEvent()
        {
            string dstrDataPLC = "";
            string[] darrEvent;

            try
            {
                if (PMTPLC != null) dstrDataPLC = PMTPLC.funEventData();                    //PLC의 Event를 읽어온다.

                if (dstrDataPLC == "" || dstrDataPLC == null) return;

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
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
            }
        }

        public bool SemAlarmReportflag = false;
        /// <summary>
        /// SVID값을 PLC로 부터 읽어서 구조체에 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SVIDRead_Tick(object sender, ElapsedEventArgs e)
        {
            string dstrWordAddress = "W2D00";
            string dstrReadData = "";
            string dstrValue = "";
            int dintLength = 0;
            int dintStartIndex = 0;
            string dstrTemp = "";
            string dstrDateTime = "";

            long dintNowTicks = 0;
            long dintLapseSec = 0;
            long dintSEMAlarmTime = this.PInfo.All.SEMAlarmTime * 3600;

            ArrayList array = new ArrayList();
            ArrayList array2 = new ArrayList();
            int dintMIN = 999999;

            int dintSVID = 0;
            //SEM Data Send Error 보고시 필요한 변수 - 12.04.04 ksh
            DateTime dtNowTime = DateTime.Now;
            long dlongTimeSpan = 0;
            int dintSemAlarmID = 1000000;
            //[2015/06/03]위치변경(Modify by HS)
            InfoAct.clsMCC MCCInfo;
            string dstrMCCInfoData;

            try
            {
                SVIDReadTimerThread.Enabled = false;

                #region "SEM"

                if (PInfo.All.SEM_ErrorFlag)
                {
                    if (SemAlarmReportflag == false)
                    {
                        SemAlarmReportflag = true;
                        if (PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID) == null)
                        {
                            this.PInfo.Unit(0).SubUnit(0).AddCurrAlarm(dintSemAlarmID);
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmEventType = "S";  // Set
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmType = this.PInfo.Unit(0).SubUnit(0).Alarm(dintSemAlarmID).AlarmType;
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmReport = this.PInfo.Unit(0).SubUnit(0).Alarm(dintSemAlarmID).AlarmReport;

                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmType = PInfo.Unit(0).SubUnit(0).Alarm(dintSemAlarmID).AlarmType;
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmCode = PInfo.Unit(0).SubUnit(0).Alarm(dintSemAlarmID).AlarmCode;
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmDesc = "Samsung Environment Monitoring Interface";
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmOCCTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).ModuleID = this.PInfo.Unit(0).SubUnit(0).Alarm(dintSemAlarmID).ModuleID;
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmReport = PInfo.Unit(0).SubUnit(0).Alarm(dintSemAlarmID).AlarmReport;
                            this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).SETCODE = 1;

                            //S5F1 Alarm Host 보고
                            this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintSemAlarmID);
                        }
                    }
                }
                else
                {
                    SemAlarmReportflag = false;
                    if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID) != null)
                    {
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).SETCODE = 0;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintSemAlarmID).AlarmEventType = "R";  // Set
                        this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintSemAlarmID);
                        PInfo.Unit(0).SubUnit(0).RemoveCurrAlarm(dintSemAlarmID);
                    }
                }

                //if (PInfo.All.SEM_ON)
                //{
                //    //12.04.02 - ksh 추가
                //    if (PInfo.All.spSerialPort != null && PInfo.All.spSerialPort.IsOpen && this.PInfo.EQP("Main").RS232Connect)
                //    {
                //        ComPortDataRecv(); // funSemDataReceived();
                //    }

                //    //SEM Interface Alarm발생후 Alarm Time마다 다시 알람보고
                //    if (PInfo.All.SEMInterfaceAlarmReport == true)       //if (this.PInfo.EQP("Main").DummyPLC == false)
                //    {
                //        dintNowTicks = DateTime.Now.Ticks;
                //        dintLapseSec = dintNowTicks - this.plngSEMAlarmReportTick;

                //        if (dintLapseSec > dintSEMAlarmTime * 10000000)    //Alarm Time이상 경과되면 다시 알람보고를 한다.
                //        {
                //            //S5F1 Alarm Host 보고
                //            this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, 1000000);
                //            this.plngSEMAlarmReportTick = DateAndTime.Now.Ticks;
                //        }
                //    }

                //    //SEM Data Error가 발생되면 SEM Port를 일정시간동안 정지후 다시 연결한다.
                //    if (this.PInfo.All.SEM_ErrorFlag == true)
                //    {
                //        if (this.PInfo.All.SEM_ErrorDelayCount >= this.PInfo.All.SEM_ErrorDelayCheckTime)
                //        {
                //            if (this.PInfo.All.CommPort != "NULL")
                //            {
                //                //포트연결시도를한다.
                //                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SerialPortOpen);
                //                //SEM Controller에 Start명령을 내린다.
                //                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerStart);
                //                this.PInfo.EQP("Main").RS232Connect = true;
                //                this.PInfo.All.SEM_ErrorFlag = false;
                //            }
                //            this.PInfo.All.SEM_ErrorDelayCount = 0;
                //        }
                //        else
                //        {
                //            this.PInfo.All.SEM_ErrorDelayCount++;
                //        }
                //    }
                //}



                #endregion
                #region "SVID READ"
                //SVID 의 LOTID 를 6000 번 부터 시작한다.
                //SVID 1~6까지는 CIM이 자체적으로 데이터를 저장하는데 1~5까지는 GLS Arrive일때 저장(Overwrite)한다.
                //LOTID(6000), PPID(6001), H_PANELID(6002), SLOTNO(6003), STEPID(6004), MODULE_STATE(6005)
                //this.PInfo.Unit(0).SubUnit(0).SVID(PInfo.All.DefaultSVIDStartIndex + 5).Value = this.PInfo.Unit(0).SubUnit(0).EQPState;

                //if (PInfo.EQP("Main").DummyPLC ==false && PInfo.EQP("Main").PLCConnect )
                {
                    //Word를 Block으로 한번에 읽는다.
                    dstrReadData = funWordRead("W2D00", this.PInfo.All.SVIDPLCReadLength, EnuEQP.PLCRWType.Hex_Data, false).Trim();


                    if (!string.IsNullOrWhiteSpace(dstrReadData))
                    {
                        foreach (int intSVID in this.PInfo.Unit(0).SubUnit(0).SVID())
                        {
                            if (intSVID >= 60000) continue;
                            array.Add(intSVID);
                        }

                        while (array.Count != 1)
                        {
                            for (int dintLoop = 0; dintLoop < array.Count; dintLoop++)
                            {
                                if ((int)array[dintLoop] < dintMIN)
                                {
                                    dintMIN = (int)array[dintLoop];
                                }
                            }
                            array2.Add(dintMIN);
                            array.Remove(dintMIN);
                            dintMIN = 99999;
                        }
                        array2.Add((int)array[0]);
                        array.Clear();

                        for (int dintLoop2 = 0; dintLoop2 < array2.Count; dintLoop2++)
                        {
                            try
                            {
                                dintSVID = Convert.ToInt32(array2[dintLoop2].ToString());
                                if (dintSVID < 60000)
                                {
                                    dintLength = this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Length * 4;   //Length에 맞게 자를 문자열 개수를 가져온다.
                                    dstrTemp = dstrReadData.Substring(dintStartIndex, dintLength);          //문자열을 자른다.
                                    if (dstrTemp != "")
                                    {
                                        if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).HaveMinusValue == false)
                                        {
                                            //날짜 데이터일 경우 읽은 값을 4자리로 끊어서 구조체에 저장
                                            if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Format.ToUpper() == "YYYYMMDDHHMMSS")
                                            {
                                                dstrValue = dstrTemp;

                                                for (int dintStep = 1; dintStep <= dintLength / 4; dintStep++)
                                                {
                                                    dstrTemp = dstrValue.Substring((dintStep - 1) * 4, 4);    //문자열을 잘라온다.
                                                    dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);  //PLC로 읽은 Hex를 10진수(Int)로 변환한다.
                                                    if (dintStep != 1)
                                                    {
                                                        dstrTemp = FunStringH.funMakeLengthStringFirst(dstrTemp, 2); //월, 일, 시, 분, 초를 2자리로 맞춘다.
                                                    }
                                                    dstrDateTime = dstrDateTime + dstrTemp;
                                                }

                                                dstrValue = dstrDateTime;    //최종 변환한 값
                                                dstrDateTime = "";          //초기화
                                            }
                                            else
                                            {
                                                if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Length > 1)
                                                {
                                                    string dstrTemp1 = dstrTemp.Substring(0, 4);
                                                    string dstrTemp2 = dstrTemp.Substring(4, 4);
                                                    dstrTemp = dstrTemp2 + dstrTemp1;

                                                    dstrValue = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
                                                }
                                                else
                                                {
                                                    //1 Word임.
                                                    dstrValue = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);  //PLC로 읽은 Hex를 10진수(Int)로 변환한다.
                                                }

                                                //dstrValue = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);  //PLC로 읽은 Hex를 10진수(Int)로 변환한다.
                                            }
                                        }
                                        else
                                        {
                                            //항목의 값에 음수값이 있는 경우 음수 값 처리
                                            dstrValue = FunTypeConversion.funPlusMinusAPDCalc(dstrTemp);
                                        }

                                        if (dintSVID == 92)
                                        {

                                        }

                                        //숫자형 Data일 경우 소수점을 붙인다.
                                        dstrValue = FunStringH.funPoint(dstrValue, this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Format);
                                    }
                                    else
                                    {
                                        dstrValue = "0";
                                    }
                                }

                            }
                            catch
                            {
                                dstrValue = "0";
                            }
                            finally
                            {
                                if (dintSVID >= 60000)
                                {
                                    if (this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value == "")
                                    {
                                        this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value = "0";  //구조체에 저장
                                    }
                                }
                                else
                                {
                                    this.PInfo.Unit(0).SubUnit(0).SVID(dintSVID).Value = dstrValue;  //구조체에 저장
                                }

                                //읽을 Index 증가
                                dintStartIndex = dintStartIndex + dintLength;
                            }
                        }
                    }
                }

                //this.PInfo.Unit(0).SubUnit(0).SVID(1).Value = PInfo.All.dintTest.ToString();
                //PInfo.All.dintTest++;

                #endregion

                //[2015/04/23]구분자추가(Add by HS)
                dstrMCCInfoData = "INFOMATION;";

                for (int dintLoop = 0; dintLoop < PInfo.Unit(0).SubUnit(0).MCCInfoCount; dintLoop++)
                {
                    MCCInfo = PInfo.Unit(0).SubUnit(0).MCCInfo(dintLoop + 1);

                        if (! MCCInfo.PLCReadFlag)
                        {
                            if (PInfo.Unit(0).SubUnit(0).SVID(MCCInfo.SVIDIndex) != null)
                            {
                                dstrMCCInfoData += MCCInfo.Index + "=" + PInfo.Unit(0).SubUnit(0).SVID(MCCInfo.SVIDIndex).Value + ",";
                            }
                        }
                    MCCInfo = null;
                    }
                    if (string.IsNullOrEmpty(dstrMCCInfoData)) return;
                    dstrMCCInfoData = dstrMCCInfoData.Substring(0, dstrMCCInfoData.Length - 1) + ";";

                    PInfo.subPLCCommand_Set(clsInfo.PLCCommand.MCCDataSend, dstrMCCInfoData);
                //}


                    if (PInfo.All.dbolAliveFlag)
                    {
                        funBitWrite("B10FF", "0",true);
                        PInfo.All.dbolAliveFlag = false;
                    }
                    else
                    {
                        funBitWrite("B10FF", "1", true);
                        PInfo.All.dbolAliveFlag = true;
                    }


            }
            catch (Exception ex)
            {
                //Dummy일 경우 'Dummy PLC'에 종종 버그가 있어 에러가 발생 할 수도 있고 안할 수도 있다. 
                if (this.PInfo.EQP("Main").DummyPLC == false) this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                GC.Collect();                       //더이상 사용하지 않는 객체를 수집하라.
                GC.WaitForPendingFinalizers();      //수집한 객체들이 메모리에서 사라질때까지 대기하라.
                SVIDReadTimerThread.Enabled = true;
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
                            if (strValue.Contains("-"))
                            {
                                strValue = "-" + strValue.Substring(1).PadLeft(8, '0');

                            }
                            else
                            {
                                strValue = strValue.PadLeft(8, '0');
                            }
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
                            //if (strValue.Contains("-"))
                            //{
                            //    strValue = strValue.Substring(1, strValue.Length - 1).PadLeft(8, '0');
                            //    strValue = "-" + strValue;
                            //}
                            //else
                            //{
                            //    strValue = strValue.PadLeft(8, '0');
                            //}
                        }

                        dstrData = FunTypeConversion.funDecimalConvert(strValue, EnuEQP.StringType.Hex);

                        break;
                }

                ////dstrData = dstrData.PadLeft(intLength * 4, '0');
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

        #region"Data Type Change Function"
        /// <summary>
        /// Word를 Block으로 읽기 위해 Queue에 저장한다.
        /// </summary>
        /// <param name="strAddress">해당 Address</param>
        /// <param name="intLength">길이</param>
        /// <param name="DataType">DataType</param>
        public void subWordReadSave(string strAddress, int intLength, EnuEQP.PLCRWType DataType)
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

        /// <summary>
        /// Word를 Block으로 읽어와서 각 DataType에 맞게 배열로 리턴한다.
        /// </summary>
        /// <param name="bolLog"></param>
        /// <returns></returns>
        /// <comment>
        /// 연속된 영역만 읽을 수 있고, 분리된 영역은 기존의 방법(funWordRead)으로 읽는다.
        /// </comment>
        public string[] funWordReadAction(Boolean bolLog)
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
                            dstrTemp = FunTypeConversion.funHexSwap(dstrTemp);
                            dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.ASCString);
                            dstrReturn[dintIndex] = dstrTemp.Trim();
                            break;

                        case EnuEQP.PLCRWType.Binary_Data:
                            dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Binary);
                            dstrReturn[dintIndex] = dstrTemp.Trim();
                            break;

                        case EnuEQP.PLCRWType.Hex_Data:
                            dstrReturn[dintIndex] = dstrTemp.Trim();
                            break;

                        case EnuEQP.PLCRWType.Int_Data:
                            if (Convert.ToInt32(dstrData[1]) == 2)  //2Word
                            {
                                dstrTemp = dstrTemp.Substring(4, 4) + dstrTemp.Substring(0, 4);
                                dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);

                            //    ////상위가 0이면 65565이하 값
                            //    //if (Convert.ToInt32(dstrTemp.Substring(4, 4)) == 0)
                            //    //{
                            //    //    dstrTemp = dstrTemp.Substring(0, 4);
                            //    //    dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
                            //    //}
                            //    //else
                            //    //{
                            //    //    dstrTemp = dstrTemp.Substring(0, 4);
                            //    //    dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
                            //    //    dstrTemp = Convert.ToString(65536 + Convert.ToInt32(dstrTemp));
                            //    //}
                            }
                            else
                            {
                                dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
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

        #region"PLC Event Process"
        /// <summary>
        /// 설비에서 알람 발생 및 해제시 Host로 보고한다
        /// </summary>
        /// <param name="strCompBit"></param>
        /// <param name="strStatus">발생(Set)/해제(Reset)</param>
        /// <param name="dintResetAlarm"></param>
        /// <comment>
        /// Heavy Alarm이 발생, 해제하면 Alarm정보를 누적 저장하여 S6F11(CEID=51, 53) 보고시 사용한다.
        /// </comment>
        public void subACTAlarm(string strCompBit, string strStatus, int dintResetAlarm)
        {
            int dintAlarmID = 0;
            string dstrModuleID = "";
            string dstrWordAddress = "";
            string dstrAlarmType = "";
            int dintAlarmCode = 0;
            string dstrAlarmDesc = "";
            Boolean dbolAlarmReport = false;
            string dstrAlarmTime = "";   //yyyy-MM-dd HH:mm:ss
            string dstrAlarmMsg = "";
            //int dintHeavyAlarmCount = 0;

            //발생한 모든 알람을 저장할 변수
            string[] dAlarmSplit;           //발생한 모든 알람을 저장하는 이전 변수
            string dNowAlarm = "";          //한개의 알람이 해제 된 후 남아있는 알람을 저장할 변수

            string dstrStepID = "";
            string dstrGLSID = "";
            string dstrLOTID = "";
            string dstrPPID = "";
            int dintUnitID = 0;
            int dintSlotID = 0;
            string dstrValue = "";
            string dstrMCCType = "";
            //[2015/04/26]MCC(Add by HS)
            string strMCCData = "";


            try
            {
                if (strStatus == "Set") // Alarm 발생
                {
                    //현재 Unit에서 Alarm이 발생했을때 AlarmID를 읽어온다.
                    //dstrWordAddress = PInfo.pPLCAddressInfo.wEQP_AlarmSetCode;
                    dintAlarmID = Convert.ToInt32(funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data));

                    //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                    subSetConfirmBit(strCompBit);

                    //Alarm이 등록이 안되있으면 로그를 출력하고 빠져나간다.
                    if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID) == null)
                    {
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subACTAlarm: AlarmID not exist, AlarmID:" + dintAlarmID);
                        return;
                    }
                    else if (PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID) != null)
                    {
                        PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subACTAlarm: Already Reported, AlarmID:" + dintAlarmID);
                        return;
                    }

                    this.PInfo.Unit(0).SubUnit(0).AddCurrAlarm(dintAlarmID);
                    this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "S";  // Set
                    this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                    this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;
                    this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 1;  // Set
                    this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmDesc = this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc;//160510 임근호

                    //Heavy Alarm이 발생하면 Alarm정보를 누적 저장한다.
                    if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                    {
                        this.PInfo.All.OccurHeavyAlarmID = dintAlarmID;

                        //발생한 모든 Alarm을 연속해서 저장한다.
                        if (this.PInfo.Unit(0).SetAlarmID == "") this.PInfo.Unit(0).SetAlarmID = dintAlarmID.ToString();
                        else this.PInfo.Unit(0).SetAlarmID = this.PInfo.Unit(0).SetAlarmID + "," + dintAlarmID.ToString();
                    }

                }
                else                    // Alarm 해제
                {
                    if (dintResetAlarm == 0) //실제 설비에서 Alarm 해제 보고가 들어올때는 Alarm ID를 PLC에서 Read한다.
                    {
                        //현재 Unit에서 Alarm이 발생했을때 AlarmID를 읽어온다.
                        //dstrWordAddress = PInfo.pPLCAddressInfo.wEQP_AlarmResetCode;
                        dintAlarmID = Convert.ToInt32(funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data));

                        //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                        subSetConfirmBit(strCompBit);
                    }
                    else  //현재 Alarm이 발생해있고 Reset이 모두 안되었는데 PLC에서 'Alarm없음' 신호를 받으면 AlarmID를 현재 저장하고 있는 것으로 한다.  
                    {
                        dintAlarmID = dintResetAlarm;
                    }

                    if (PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID) == null)
                    {
                        PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subACTAlarm: Already Cleared, AlarmID:" + dintAlarmID);
                        return;
                    }

                    this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "R";  // ReSet
                    this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 0;  // Set


                    //Heavy Alarm이 해제되면 Alarm정보를 누적 저장한다.
                    if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                    {
                        this.PInfo.All.ClearHeavyAlarmID = dintAlarmID;
                    }
                }

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

                if (strStatus == "Set")
                {
                    //알람발생시 장비내 Glass정보 저장
                    subGLSPosLogWrite(dstrAlarmTime, this.PInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).UnitID, dintAlarmID);

                }

                // Alarm Message를 MainView ComboBox에 추가 (Message에 발생시간은 제외 Combo에 시간이 이중으로 Display 되어서)
                dstrAlarmMsg = dstrModuleID + "," + strStatus + "," + dintAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + dstrAlarmDesc;
                this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.AlarmMsg, dstrAlarmMsg);

                if (strStatus == "ReSet")        // Alarm Reset 이면 발생알람을 구조체에서 삭제한다.
                {
                    if (dintResetAlarm == 0)  //PLC에서 Alarm 해제를 받을때는 여기서 삭제하고 나머지는 subACTAlamExist에서 삭제한다.
                    {
                        #region <Alarm 시나리오 변경 추가       091216  이상호>
                        if (this.PInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                        {
                            dAlarmSplit = this.PInfo.Unit(0).SetAlarmID.Split(',');

                            //ReSet 된 Alarm은 삭제 한다.
                            for (int iArmCnt = 0; iArmCnt < dAlarmSplit.Length; iArmCnt++)
                            {
                                if (dintAlarmID.ToString() == dAlarmSplit[iArmCnt]) dAlarmSplit[iArmCnt] = "";
                            }

                            //남아있는 Alarm중 가장 먼저 발생한 AlarmID를 첫번째 Alarm으로 올린다.
                            for (int iArmCnt = 0; iArmCnt < dAlarmSplit.Length; iArmCnt++)
                            {
                                if (dAlarmSplit[iArmCnt] == "") { }
                                else
                                {
                                    if (dNowAlarm == "") dNowAlarm = dAlarmSplit[iArmCnt];
                                    else dNowAlarm = dNowAlarm + "," + dAlarmSplit[iArmCnt];
                                }
                            }

                            dAlarmSplit = dNowAlarm.Split(',');
                            this.PInfo.Unit(0).SetAlarmID = dNowAlarm;
                            //등록한 첫번째 알람을 EQP 상태 변화시 보고하기 위해 넣는다.

                            if (dAlarmSplit[0] == "") this.PInfo.All.OccurHeavyAlarmID = 0;
                            else this.PInfo.All.OccurHeavyAlarmID = Convert.ToInt32(dAlarmSplit[0]);
                        }
                        #endregion


                        this.PInfo.Unit(0).SubUnit(0).RemoveCurrAlarm(dintAlarmID);
                    }
                }

                //MCC Log 기록 111221 고석현
                //dintUnitID = this.PInfo.funGetModuleIDToUnitID(dstrModuleID);
                //dstrGLSID = this.PInfo.Unit(dintUnitID).SubUnit(0).HGLSID;
                //if (dstrGLSID.Trim() != "")
                //{
                //    dstrLOTID = this.PInfo.Unit(dintUnitID).SubUnit(0).CurrGLS(dstrGLSID).LOTID;
                //    dintSlotID = this.PInfo.Unit(dintUnitID).SubUnit(0).CurrGLS(dstrGLSID).SlotID;
                //    dstrPPID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).HOSTPPID;
                //    dstrStepID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).STEPID;
                //}

                //dstrValue = strStatus.ToUpper() + "=" + dintAlarmID + "=" + dstrAlarmDesc.ToUpper();


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
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strStatus:" + strStatus);
            }
        }

        /// <summary>
        /// 장비전체 Alarm 존재여부
        /// </summary>
        /// <param name="intValue"></param>
        public void subACTAlamExist(int intValue)
        {
            Boolean dbolAlarmExist = false;

            try
            {
                if (intValue == 1)
                {
                    dbolAlarmExist = true;
                }
                else
                {
                    dbolAlarmExist = false;

                    //현재 Alarm이 발생해있고 Reset이 모두 안되었는데 PLC에서 'Alarm없음' 신호를 받으면 
                    //현재 발생한 Alarm으로 모두 Reset 보고하고 구조체에서 삭제한다.
                    foreach (int dintAlarm in this.PInfo.Unit(0).SubUnit(0).CurrAlarm())
                    {
                        subACTAlarm("", "ReSet", dintAlarm);    //Alarm 해제 보고 후 구조체에서 Alarm정보를 지운다.
                    }

                    //장비전체에 Alarm이 해제되었으면 모든 Alarm 정보를 지운다.
                    this.PInfo.Unit(0).SubUnit(0).RemoveCurrAlarm();
                }

                this.PInfo.All.AlarmExist = dbolAlarmExist;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intValue:" + intValue);
            }
        }

        /// <summary>
        /// 알람이 발생할당시에 해당장비내에 글래스위치정보를 남긴다.
        /// </summary>
        /// <param name="strLogWriteTime"></param>
        /// <param name="intUnitID"></param>
        /// <param name="intAlarmID"></param>
        private void subGLSPosLogWrite(string strLogWriteTime, int intUnitID, int intAlarmID)
        {
            string dstrHGLSID = "";
            string dstrLogdata = "";

            string dstrLOTID = "";
            int dintSlotID = 0;
            string dstrPPID = "";
            try
            {
                dstrLogdata += strLogWriteTime + "," + intUnitID + "," + intAlarmID + ",";

                for (int dintLoop = 1; dintLoop <= this.PInfo.UnitCount; dintLoop++)
                {
                    dstrHGLSID = this.PInfo.Unit(dintLoop).SubUnit(0).HGLSID;

                    if (dstrHGLSID == "")
                    {
                        dstrLogdata += "///,";
                    }
                    else
                    {
                        dstrLOTID = this.PInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).LOTID;
                        dintSlotID = this.PInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).SlotID;
                        dstrPPID = this.PInfo.LOTID(dstrLOTID).Slot(dintSlotID).HOSTPPID;

                        dstrLogdata += this.PInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).H_PANELID + "/"
                                    + dstrLOTID + "/"
                                    + dintSlotID + "/"
                                    + dstrPPID + ",";
                    }
                }

                //마지막의 콤마는 제거
                dstrLogdata = dstrLogdata.Remove(dstrLogdata.Length - 1);

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.AlarmGLSInfo, dstrLogdata);

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// EventBit가 살아있는 경우를 대비하여 적절한 타임에 Initilal
        /// </summary>
        public void subEventBitInitialCmd()
        {
            string dstrBitAddress = "B1000";
            string dstrData = FunStringH.funMakeLengthStringFirst("0", 256);     //384개임

            try
            {
                //EventBit를 초기화한다.
                funBitWrite(dstrBitAddress, dstrData);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 발생한PLC Event를 ACT를 찾아 ACT함수로 분기 시킨다
        /// </summary>
        /// <param name="strPLCAddr">발생한 PLC Address</param>
        /// <param name="intBitVal">변화 값 0 or 1</param>
        /// <comment>
        /// Event는 0에서 1로 변할때만 들어오고 Status는 0과 1 모두 들어온다.
        /// </comment>
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
                //int dintActFromSub = Convert.ToInt32(dstrACTFromSub.PadLeft(1, '0'));
               

                //Boxing Parameter Array that send to Event Class
                //param[0] : dstrCompBit
                //param[1] : dstrACTVal
                //param[2] : dintActFrom
                //param[3] : dstrACTFromSub
                //param[4] : intBitVal;
                //param[5] : Special Parameter
                string[] parameters = new string[6];
                parameters[0] = (string)dstrCompBit;
                parameters[1] = (string)dstrACTVal;
                parameters[2] = (string)dintActFrom.ToString();
                parameters[3] = (string)dstrACTFromSub;
                parameters[4] = (string)intBitVal.ToString();
                parameters[5] = (string)"0";

                m_pEQPEventList.funArrangeEQPEvent(dstrACTName, parameters);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strPLCAddr:" + strPLCAddr + ", intBitVal:" + intBitVal);
            }
        }

        /// <summary>
        /// Confirm 비트를 On한다.
        /// </summary>
        /// <param name="strCompBit"></param>
        public void subSetConfirmBit(string strCompBit)
        {
            try
            {
                if (strCompBit.Trim() == "") return;

                funBitWrite(strCompBit, "1");                      //Confirm비트를 변경한다
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// SEM Controller Alarm(Light Alarm)
        /// </summary>
        /// <param name="intValue"></param>
        private void subSEMAlarm(int intAlarmID, string strAlarmDesc, string strStatus)
        {
            //int dintAlarmID = 20001;
            string dstrModuleID = this.PInfo.EQP("Main").EQPID;
            string dstrAlarmType = "";
            int dintAlarmCode = 0;
            //string dstrAlarmDesc = "";
            string dstrAlarmTime = "";   //yyyy-MM-dd HH:mm:ss
            string dstrAlarmMsg = "";
            //string strStatus = "ReSet";

            try
            {
                //this.plngCIMLive = DateAndTime.Now.Ticks;

                if (strStatus == "S") // Alarm 발생
                {
                    if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID) == null)
                    {
                        this.PInfo.Unit(0).SubUnit(0).AddCurrAlarm(intAlarmID);
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmEventType = "S";       // Set
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmType = "L";            //Light Alarm
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmCode = 1;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmDesc = strAlarmDesc;
                        dstrAlarmTime = DateTime.Now.ToString("yyyyMMddHHmmss");    // Alarm 발생, 해제시간 설정
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmOCCTime = dstrAlarmTime;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).ModuleID = dstrModuleID;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).SETCODE = 1;

                        dstrAlarmType = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmType;
                        dintAlarmCode = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmCode;
                        dstrModuleID = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).ModuleID;

                        //S5F1 Alarm Host 보고
                        this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, intAlarmID);

                        // Alarm 로그 Write
                        dstrAlarmMsg = dstrAlarmTime + "," + dstrModuleID + "," + strStatus + "," + intAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + strAlarmDesc;
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);

                        // Alarm Message를 MainView ComboBox에 추가 (Message에 발생시간은 제외 Combo에 시간이 이중으로 Display 되어서)
                        dstrAlarmMsg = dstrModuleID + "," + strStatus + "," + intAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + strAlarmDesc;
                        this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.AlarmMsg, dstrAlarmMsg);

                        //LAYER0 (ALL) Alarm 여부를 On한다.
                        subACTAlamExist(1);

                        //SEM Controller Interface Alarm 보고를 했다는 것을 설정
                        if (intAlarmID == 1000000)
                        {
                            PInfo.All.SEMInterfaceAlarmReport = true;
                            this.plngSEMAlarmReportTick = DateAndTime.Now.Ticks;
                        }
                    }
                }
                else
                {
                    //Alarm 해제보고
                    if (this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID) != null)
                    {
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmEventType = "R";  // ReSet
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmType = "L";            //Light Alarm
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmCode = 1;
                        //this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmDesc = strAlarmDesc;
                        dstrAlarmTime = DateTime.Now.ToString("yyyyMMddHHmmss");    // Alarm 발생, 해제시간 설정
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmOCCTime = dstrAlarmTime;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).ModuleID = dstrModuleID;
                        this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).SETCODE = 0;

                        strAlarmDesc = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmDesc;
                        dstrAlarmType = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmType;
                        dintAlarmCode = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmCode;
                        dstrModuleID = this.PInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).ModuleID;

                        //S5F1 Alarm Host 보고
                        this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, intAlarmID);

                        // Alarm 로그 Write
                        dstrAlarmMsg = dstrAlarmTime + "," + dstrModuleID + "," + strStatus + "," + intAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + strAlarmDesc;
                        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);

                        // Alarm Message를 MainView ComboBox에 추가 (Message에 발생시간은 제외 Combo에 시간이 이중으로 Display 되어서)
                        dstrAlarmMsg = dstrModuleID + "," + strStatus + "," + intAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + strAlarmDesc;
                        this.PInfo.subMessage_Set(InfoAct.clsInfo.MsgType.AlarmMsg, dstrAlarmMsg);

                        this.PInfo.Unit(0).SubUnit(0).RemoveCurrAlarm(intAlarmID); //Alarm 삭제

                        //LAYER0 (ALL) Alarm 여부를 Off한다.
                        subACTAlamExist(0);

                        //SEM Controller Interface Alarm 해제보고를 했다는 것을 설정
                        if (intAlarmID == 1000000)
                        {
                            PInfo.All.SEMInterfaceAlarmReport = false;
                            this.plngSEMAlarmReportTick = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intAlarmID:" + intAlarmID);
            }
        }

        /// <summary>
        /// PPID 변경 내용 로그를 남긴다.
        /// </summary>
        /// <param name="strLog"></param>
        /// <param name="dstrHOSTPPID"></param>
        /// <param name="dstrBeforeEQPPPIDBody">변경이전의 EQP PPID Body 값</param>
        /// <param name="dstrBeforeEQPPPID">변경이전의 EQP PPID 값</param>
        /// <param name="dstrAfterEQPPPID">변경이후의 EQP PPID 값</param>
        public void subSetParameterLog(string strLog, string dstrHOSTPPID, string[] dstrBeforeEQPPPIDBody, string dstrBeforeEQPPPID, string dstrAfterEQPPPID)
        {
            string dstrWriteData = "";
            string dstrTemp = "";
            int dintBodyIndex = 3;     //PPID Body Index

            try
            {
                //로그에 출력할 Data를 구성한다.
                dstrWriteData = "PPID," +DateTime.Now.ToString("yyyyMMddHHmmss") +",";


                if (dstrHOSTPPID.Trim() != "")                                            //PPIDVer
                {
                    dstrTemp = this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).PPIDVer;
                }
                else
                {
                    dstrTemp = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrAfterEQPPPID).PPIDVer;
                }

                dstrWriteData = dstrWriteData + dstrTemp + ",";
                if (dstrHOSTPPID.Trim() != "")                                            //PPID Comment
                {
                    dstrTemp = this.PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).Comment;
                }
                else
                {
                    dstrTemp = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrAfterEQPPPID).Comment;
                }

                dstrWriteData = dstrWriteData + dstrTemp + ",";

                dstrTemp = strLog;                                                 //strItem(변경 혹은 생성 혹은 삭제 등의 자세한 내용이 들어감)
                dstrWriteData = dstrWriteData + dstrTemp + ",";
                dstrTemp = this.PInfo.All.UserID;                                   //UserID
                dstrWriteData = dstrWriteData + dstrTemp + ",";
                dstrTemp = dstrHOSTPPID;                                            //HOSTPPID
                dstrWriteData = dstrWriteData + dstrTemp + ",";
                dstrTemp = dstrBeforeEQPPPID;                                       //Before EQPPPID
                dstrWriteData = dstrWriteData + dstrTemp + ",";
                dstrTemp = dstrAfterEQPPPID;                                        //After EQPPPID
                dstrWriteData = dstrWriteData + dstrTemp + ",";

                if (dstrBeforeEQPPPIDBody != null)
                {
                    //EQP PPID Body 값의 수정인 경우 변경 전의 EQPPPID의 Body값
                    for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                    {
                        dstrTemp = dstrBeforeEQPPPIDBody[dintLoop - 1];
                        dstrWriteData = dstrWriteData + dstrTemp + ",";
                    }

                    //EQP PPID Body 값의 수정인 경우 변경 후의 EQPPPID의 Body값
                    for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrAfterEQPPPID).PPIDBodyCount; dintLoop++)
                    {
                        dstrTemp = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrAfterEQPPPID).PPIDBody(dintLoop).Value;
                        dstrWriteData = dstrWriteData + dstrTemp + ",";
                    }
                }
                else
                {
                    if (strLog == "EQP PPID 생성")
                    {
                        //Body 추가(현재 EQPPPID의 Body 값)
                        for (int dintLoop = 1; dintLoop <= this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrBeforeEQPPPID).PPIDBodyCount; dintLoop++)
                        {
                            dstrTemp = this.PInfo.Unit(0).SubUnit(0).EQPPPID(dstrBeforeEQPPPID).PPIDBody(dintLoop).Value;
                            dstrWriteData = dstrWriteData + dstrTemp + ",";
                        }

                        //EQP PPID Body 값의 수정이 아니면 그냥 콤마로 채운다.
                        dstrTemp = ",".PadLeft(this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount, ',');
                        dstrWriteData = dstrWriteData + dstrTemp;
                    }
                    else
                    {
                        //EQP PPID Body 값의 수정이 아니면 그냥 콤마로 채운다.
                        dstrTemp = ",".PadLeft(this.PInfo.Unit(0).SubUnit(0).PPIDBodyCount * 2, ',');
                        dstrWriteData = dstrWriteData + dstrTemp;
                    }
                }

                //마지막의 콤마는 제거
                dstrWriteData = dstrWriteData.Remove(dstrWriteData.Length - 1);

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.Parameter, dstrWriteData);
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region"DLL Event Process"
        /// <summary>
        /// 자체 Queue에 있는 명령을 PLC로 Write한다.
        /// </summary>
        void PLCWriteMethodThread()
        {
            string dstrData = "";
            string[] darrEvent;

            do
            {
                lock (this.SyncRoot)
                {
                    try
                    {
                        try
                        {
                            //subPLCEvent();      //Event 처리를 한다.

                            //subMCCBitRead();    //MCC Bit를 읽어서 Log를 기록한다. - 20101103 김중권

                            if (this.PInfo.funGetPLCCommandCount() > 0)
                            {
                                //dstrData = this.PInfo.funGetPLCCommand().ToString();      //방금 Queue에서 읽은 내용을 삭제한다.
                                //darrEvent = dstrData.Split(new char[] { ',' });

                                //m_pCIMCommandList.funArrangeCIMEvent(darrEvent);
                                object dobjEvent = PInfo.funGetPLCCommand();
                                clsParam pParam = (clsParam)dobjEvent;

                                m_pCIMCommandList.funArrangeCIMEvent(pParam);
                            }
                        }
                        catch (Exception ex)
                        {
                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                        }
                    }
                    catch { }
                }
                Thread.Sleep(1);

            } while (true);
        }

        void PLCReadMethodThread()
        {
            string dstrDataPC = "";

            do
            {
                string dstrDataPLC = "";
                string[] darrEvent;

                lock (this.LockEventRead)
                {
                    try
                    {
                        #region PLC 통신부분
                        if (PMTPLC != null) dstrDataPLC = PMTPLC.funEventData();                    //PLC의 Event를 읽어온다.

                        if (dstrDataPLC != "" && dstrDataPLC != null)
                        {
                            //if (bolReady == true)
                            //{
                            //    bolReady = false;
                            //    //longStartTick = DateTime.Now.Ticks;
                            //    stopWatch.Start();
                            //}
                            //intEventCount++;
                            //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrDataPLC);
                            darrEvent = dstrDataPLC.Split(new char[] { ';' });

                            //PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                            //float cpuUsage = cpu.NextValue();
                            //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "CPU Usage : " + cpuUsage.ToString() + "%");

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

                            //string strLogText = string.Format("StopWatchTime : {0}", stopWatch.Elapsed.TotalMilliseconds);
                            //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strLogText);
                        }
                        else
                        {
                            //if (bolReady == false)
                            //{
                            //    bolReady = true;
                            //    //longEndTick = DateTime.Now.Ticks;
                            //    //longTimeSpan = longEndTick - longStartTick;
                            //    stopWatch.Stop();

                            //    //string strLogText = string.Format("({0} EA) Event Process Time is {1}ms", intEventCount, stopWatch.Elapsed.TotalMilliseconds);
                            //    //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strLogText);
                            //    intEventCount = 0;
                            //    stopWatch.Reset();
                            //}
                        }
                        #endregion

                        #region MCC 통신 부분
                        //MCC프로그램 이벤트 확인
                        if (PMCC != null) dstrDataPC = PMCC.funEventData();                      //PC의 Event를 읽어온다.

                        //if (dstrDataPC == "" || dstrDataPC == null)
                        //{
                        //    //continue;
                        //}
                        //else
                        //{

                        //}

                        //검사기 PC Event 처리
                        if (dstrDataPC != "" && dstrDataPC != null)
                        {

                            darrEvent = dstrDataPC.Split(new char[] { ';' });
                            string dstrTemp = "";
                            switch (darrEvent[0])
                            {
                                case "ErrorEvent":
                                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "MCC 통신 Error" + darrEvent[1] + "Error_Index:" + darrEvent[2] + ",Err_Descr:" + darrEvent[3]);
                                    break;
                                case "ReceiveEvent":
                                    subMCC_ReceiveEvent(darrEvent[1], darrEvent[2]);
                                    break;
                                case "TransferEvent":
                                    subMCC_TransferEvent(darrEvent[1], darrEvent[2]);
                                    break;
                                case "ConnectionEvent":
                                    MCC_PCConnect(darrEvent[1]);
                                    break;
                                case "DisConnectionEvent":
                                    MCC_PCDisConnect(darrEvent[1]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        #endregion

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
            }
            while (true);
        }

        #endregion


        #region MCC 통신
        private void MCC_PCDisConnect(string p)
        {
            try
            {
                PInfo.All.MCC_ConnectState = false;
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void MCC_PCConnect(string p)
        {
            try
            {
                PInfo.All.MCC_ConnectState = true;
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subMCC_TransferEvent(string p, string p_2)
        {
            try
            {

            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subMCC_ReceiveEvent(string strDateTime, string strValue)
        {
            //int dintTemp = 0;
            string dstrMessage = "";
            string dstrDefine = "";
            string dstrData1 = "";
            string dstrData2 = "";
            string dstrName = "";
            try
            {
                dstrMessage = strValue.Trim();

                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strDateTime, "RECV => Value : " + dstrMessage);



            }
            catch (Exception ex)
            {
            }

        }
        public void MCCSend(string strMSG)
        {
            try
            {
                if (PMCC != null && PInfo.All.MCC_ConnectState)
                {
                    PMCC.subTransfer(strMSG);
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion

        #region "DLL Event Process"
        #endregion

        #region "PLC로 명령을 내림"
        /// <summary>
        /// 모든 EQP, HOST PPID 정보 요청 지시를 PLC에 한다.
        /// </summary>
        /// <param name="bolFirst"></param>
        /// <param name="intPPIDTYPE"></param>
        /// <comment>
        /// 여러개(15개씩) 요청한다.
        /// </comment>
        public void subSetupPPIDCmd(Boolean bolFirst, int intPPIDTYPE)
        {
            //string dstrWordAddress = "";
            
            

            try
            {
                //처음으로 명령을 내릴 경우 명령 내릴 회수를 구한다.
                if (bolFirst == true)
                {
                    int dintRemainder = 0;
                    double ddolCount = 0;

                    ////15개씩 끊어서 명령을 내릴 회수를 구한다.
                    ////EQP PPID 명령 회수 구함
                    //dintRemainder = this.PInfo.All.CurrentRegisteredEQPPPIDCount % this.pPPIDReadCount;  //몫을 가져온다.
                    //if (dintRemainder == 0) ddolCount = Math.Truncate(Convert.ToDouble(this.PInfo.All.CurrentRegisteredEQPPPIDCount / this.pPPIDReadCount));
                    //else ddolCount = Math.Truncate(Convert.ToDouble(this.PInfo.All.CurrentRegisteredEQPPPIDCount / this.pPPIDReadCount) + 1);
                    //this.PInfo.All.EQPPPIDCommandCount = Convert.ToInt32(ddolCount); //PLC에 EQP PPID 정보 요청할 회수를 저장한다.

                    //HOST PPID 명령 회수 구함
                    dintRemainder = this.PInfo.All.CurrentRegisteredHOSTPPIDCount % this.pPPIDReadCount;  //몫을 가져온다.
                    if (dintRemainder == 0) ddolCount = Math.Truncate(Convert.ToDouble(this.PInfo.All.CurrentRegisteredHOSTPPIDCount / this.pPPIDReadCount));
                    else ddolCount = Math.Truncate(Convert.ToDouble(this.PInfo.All.CurrentRegisteredHOSTPPIDCount / this.pPPIDReadCount) + 1);
                    this.PInfo.All.HOSTPPIDCommandCount = Convert.ToInt32(ddolCount); //PLC에 HOST PPID 정보 요청할 회수를 저장한다.
                }

                ////요청할 PPID Type을 써준다.(EQPPPID(1) 혹은 HOSTPPID(2)) 
                ////dstrWordAddress = "W104B";
                //funWordWrite(this.PInfo.pPLCAddressInfo.wCIM_PPIDtype, intPPIDTYPE.ToString(), EnuEQP.PLCRWType.Int_Data); //PPID Type

                PInfo.All.SearchPPIDType = intPPIDTYPE;

                this.PInfo.All.SetUpPPIDPLCWriteCount = this.PInfo.All.SetUpPPIDPLCWriteCount + 1;  //PLC에 Write한 회수 초기화
                //Group No.를 써준다.(순번)
                funWordWrite("W100B", this.PInfo.All.SetUpPPIDPLCWriteCount.ToString(), EnuEQP.PLCRWType.Int_Data);
                funBitWrite("B1033", "1");


            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// MCC Data Log를 생성한다.
        /// </summary>
        public void subMCCLogData(string strModuleID, string strLogType, string strStepID, string strGLSID, string strLotID, string strPPID, params string[] strDataValue)
        {
            string dstrMCCLogData = "";
            string dstrDataTime = "";

            try
            {
                dstrDataTime = DateTime.Now.ToString("MMdd_HHmm_ss.fff");
                dstrMCCLogData = dstrDataTime + "," + strModuleID + "," + strLogType + "," + strStepID + "," + strGLSID + "," + strLotID + "," + strPPID;

                if (strDataValue != null)
                {
                    switch (strLogType)
                    {
                        case "A":       //Action Log
                            //Action=FromPosition=ToPosition=Start_End, ex) Componet_In=LD01=UP01=End
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2] + "=" + strDataValue[3];
                            break;

                        case "I":       //Information Log
                            //Information=Value, ex) VaccumPressure=360.4
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1];
                            break;

                        case "E":
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0];
                            break;

                        case "W":
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0];
                            break;

                        case "S":
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2] + "=" + strDataValue[3];
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

        #region APC 작업지시
        public string funGetAPCStartCMD()
        {
            string dstrHGLSID = "";
            try
            {
                if (APCStartCMDQueue.Count > 0)
                {
                    if (PInfo.All.APCPPIDReadFlag == true)
                    {
                        dstrHGLSID = Convert.ToString(APCStartCMDQueue.Dequeue());
                        PInfo.All.APCPPIDReadFlag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
            return dstrHGLSID;
        }


        public void ACPStartCMD_Set(string strHGLSID)
        {
            APCStartCMDQueue.Enqueue(strHGLSID);
        }

        #endregion

        //[2015/03/27] 작업지시(Add by HS)
        private void JobStart()
        {
            bool dbolXPCStart = false;

            if (this.PInfo.Unit(0).SubUnit(0).EOID(this.PInfo.funGetEOIDNameToIndex("APC")).EOV == 1 && PInfo.APC(PInfo.All.DataCheckGLSID) != null)
            {
                PInfo.All.APCStartEQPPPIDCheck = true;
                clsMappingEQPPPID CurrentPPID = PInfo.Unit(0).SubUnit(0).MappingEQPPPID(PInfo.APC(PInfo.All.DataCheckGLSID).EQPPPID);
                CurrentPPID.PPIDCommand = clsInfo.PPIDCMD.APC;
                PInfo.SetPPIDCMD(PInfo.APC(PInfo.All.DataCheckGLSID).EQPPPID);
                dbolXPCStart = true;
            }
            else if (this.PInfo.Unit(0).SubUnit(0).EOID(this.PInfo.funGetEOIDNameToIndex("RPC")).EOV == 1 && PInfo.RPC(PInfo.All.DataCheckGLSID) != null)
            {
                PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.RPCStart, PInfo.All.DataCheckGLSID);
                //pInfo.All.RPCDBUpdateCheck = true;
                dbolXPCStart = true;
            }

            if (dbolXPCStart == false)
            {
                PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.NormalStart, PInfo.All.DataCheckGLSID);
            }
        }
    }
}
