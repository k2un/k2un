using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace InfoAct
{
    public class clsAll
    {
        //전체 EQP 공통
        public string MDLN = "";                        //Equipment Model Type(S1F2의 송신 값)
        public int CurrentLOTIndex = 0;                 //현재까지 발번한 LOTIndex
        public string SoftVersion = "";                     //SOFT VERSION  ex) 2010_06_05_VER_1_0_5

        //SMD A3, V1 추가 사양
        public string SpecCode = "";                    //Spec Code ex) SMD_VER220_UNIFIED

        //Host
        public Boolean HostConnect = false;             //Host 연결 상태(True:Connection, False:DisConnection)
        public Boolean HostDriver = false;             //Secom Driver 로딩상태(True:로딩 성공, False:로딩실패)
        public Boolean SecomDriver = false;             //Secom Driver 로딩상태(True:로딩 성공, False:로딩실패)

        public string CommPort = "COM1";                //232 Connection Com Port
        public string CommSetting = "57600,e,8,1";      //232 Port Setting
        //121122 Ko Seok Hyun 추가
        public SerialPort spSerialPort = new SerialPort();



        public int DeviceID = 0;
        public string LocalPort = "";
        public int RetryCount = 0;
        public int T3 = 0;
        public int T5 = 0;
        public int T6 = 0;
        public int T7 = 0;
        public int T8 = 0;
        public int T9 = 0;                              //CT ==> System.INI파일에 있음, 나머지 TimeOut은 모두 SEComINI.EXP파일에 있음.

        //STM(응용 프로그램)
        public string CurrentHOSTPPID = "";             //현재장비에서 운영중인 HOSTPPID
        public string CurrentEQPPPID = "";              //현재장비에서 운영중인 EQPPPID
        public Boolean ProgramEnd = false;              //Pogram을 종료함 (True: 종료)
        public Boolean ONLINEModeChange = false;        //True:Online 전환중, False:대기
        public string UserID = "";                      //User ID
        //public string CV01GLSID = "";                   //CV01에 방금 들어온 H_PanelID
        public Boolean OperatorCallFormVisible = false; //Operaotr Call 폼이 떠있는지 여부(True: 떠있음, False: 떠 있지 않음)
        public Boolean AutoMode = false;                //CIM<->장비간 Auto Mode 여부(True: Auto 모드임)
        public string PMCode = "";                      //Operator가 PM을 선택시 PM Code 저장
        public Boolean AlarmExist = false;              //장비전체 알람이 발생해 있는지 여부(True: 발생해있음)
        public int UserLogInDuringTime = 0;                   //User가 LogIn한 지속시간(자동 LogOut을 하기위해 - 기본 30분)
        //public string PPIDComment = "";                 //CIM 화면에서 HOSTPPID, EQPPPID 생성시 입력하게 될 Comment
        //public Boolean PPIDListViewRefresh = false;     //System Setup 폼의 PPID 탭의 PPIID ListView를 Refresh 할지 여부

        public string CfgFilePath = "";                 //SMDA2MASK.cfg 파일의 전체 경로
        /// <summary>
        /// 기본적으로 보낼(SVID 6개) 항목의 DB의 Index 시작숫자.
        /// </summary>
        public int DefaultSVIDStartIndex = 0;

        public int CurrentRegisteredHOSTPPIDCount = 0;  //현재 장비에 등록되어 있는 HOSTPPID의 개수
        public int CurrentRegisteredEQPPPIDCount = 0;   //현재 장비에 등록되어 있는 EQPPPID의 개수

        public int EQPPPIDCommandCount = 0;                     //15개씩 끊어서 PLC에 명령을 내릴 회수
        public int HOSTPPIDCommandCount = 0;                    //15개씩 끊어서 PLC에 명령을 내릴 회수
        public int SetUpPPIDPLCWriteCount = 0;                  //PLC에 명령을 내린 회수(EQP, HOST 따로 따로 카운트 한다.)

        public Boolean PLCActionEnd = false;                    //Word Read를 했냐?     

        public Boolean isReceivedFromHOST = false;                  //HOST로 부터 Primay(S7Fy)를 받았냐? 
        public Boolean isReceivedFromCIM = false;                   //CIM의 System Setup에서 PPID 정보 요청을 하였는가?

        //SEM Controller
        public Boolean SEMControllerConnect = false;    //SEM Controller 연결 상태(True:Connection, False:DisConnection)
        
        public int SEMAlarmTime = 8;                    //SEM Config에서 설정한 Alarm Time, 설정된 시간간격 마다 Ligjht Alarm을 한번씩 발생(기본설정 8Hr)
        public Boolean SEMInterfaceAlarmReport = false; //SEM Controller Interface Alarm을 보냈는지

        public Boolean SEMStartReplyCheck = false;      //SEM Controller에 Start명령을 내리고 ACK Reply를 받앗는지
        public Boolean SEMEndReplyCheck = false;      //SEM Controller에 End명령을 내리고 ACK Reply를 받앗는지
        public long SEMStartReplyCheckTime = 0;         //Start명령을 내리고 ACK Reply를 Check할 시간(45초)
        public long SEMEndReplyCheckTime = 0;         //End명령을 내리고 ACK Reply를 Check할 시간(5초)
       

        public string ControlstateChangeBYWHO = "";         //CEID=71,72,73 보고시 사용될 BYWHO
        public string EQPSpecifiedCtrlBYWHO = "";           //CEID=401 보고시 사용될 BYWHO
        public string ECIDChangeBYWHO = "";                 //CEID=102 보고시 사용될 BYWHO
        public string EOIDChangeBYWHO = "";                 //CEID=101 보고시 사용될 BYWHO
        public string EOIDChangeBYWHO2 = "";

        public Color BackColorOffline = Color.White;
        public Color ForeColorOffline = Color.Black;
        public Color BackColorLocal = Color.White;
        public Color ForeColorLocal = Color.Black;
        public Color BackColorRemote = Color.White;
        public Color ForeColorRemote = Color.Black;

        public Color BackColorNormal = Color.White;
        public Color ForeColorNormal = Color.Black;
        public Color BackColorFault = Color.White;
        public Color ForeColorFault = Color.Black;
        public Color BackColorPM = Color.White;
        public Color ForeColorPM = Color.Black;

        public Color BackColorInit = Color.White;
        public Color ForeColorInit = Color.Black;
        public Color BackColorIdle = Color.White;
        public Color ForeColorIdle = Color.Black;
        public Color BackColorSetup = Color.White;
        public Color ForeColorSetup = Color.Black;
        public Color BackColorReady = Color.White;
        public Color ForeColorReady = Color.Black;
        public Color BackColorExecuting = Color.White;
        public Color ForeColorExecuting = Color.Black;
        public Color BackColorPause = Color.White;
        public Color ForeColorPause = Color.Black;
        public Color BackColorSTOP = Color.White;
        public Color ForeColorSTOP = Color.Black;

        public string SystemINIFilePath = "";           //System.ini 파일의 전체 경로
        public string ControlState = "1";               //ControlState(Off Line(1), On Line Local(2), On Line Remote(3))
        public string ControlStateOLD = "";            //ControlState OLD(Off Line(1), On Line Local(2), On Line Remote(3))
        public string WantControlState = "";            //Want Change ControlState(Off Line(1), On Line Local(2), On Line Remote(3))

        public SortedList ECIDChange = new SortedList();            //HOST, CIM에서 ECID 변경시 변경할 ECID를 저장함.
        public SortedList ECIDChangeHOSTReport = new SortedList();  //HOST, CIM에서 ECID 변경시 변경후 HOST로 보고할 ECID를 저장함.
        public string ECIDChangeFromHost = "";
        public Boolean EQPProcessTimeOverReset = false;             //기존에 설정되어 있던 EQP Process Time Over 체크 설정을 다시 할지 여부

        public int OccurHeavyAlarmID = 0;                           //Normal 혹은 PM -> Fault로 변경되었을때 발생한 AlarmID
        public int ClearHeavyAlarmID = 0;                           //Fault -> Normal 혹은 PM되었을때 해제된 AlarmID

        public int SVIDPLCReadLength = 0;                           //SVID를 PLC로 부터 읽어야 하는 총 길이
        public int SVIDPLCNotReadLength = 0;                        //SVID를 PLC로 부터 읽어오지 않는 총 길이
        public int GLSAPDPLCReadLength = 0;                         //GLSAPD를 PLC로 부터 읽어야 하는 총 길이
        public int HOSTReportEOIDCount = 0;                         //S1F5(SFCD=1)일 경우 HOST로 보고하는 EOID List 개수
        public int HOSTReportECIDCount = 0;

        public string RAWPATH = "";                      //Data 저장경로
        public string SUMPATH = "";                      //Data 저장경로
        public string IMGPATH = "";                      //Image 저장경로
        public string DISK = "";                         //Data 저장위치 (1 : "FILESERVER" 2 : "LOCALDISK" )

        public string FileServerPath = "";                          //S6F11보고시 사용할 File Server Path(Ini에서 읽어서 저장)
        public string HOSTPPIDFilePath = "";                        //HOST PPID List(HostPpidList.ini)가 있는 측정기 PC의 공유 파일 경로

        //Form Close
        public Boolean ModeChangeFormVisible = false;   //Mode Change창이 떠있는지 여부(True: 떠있음, False: 떠 있지 않음)

        ////VCR Reading Result
        //public int VCRPass = 0;                           //Glass Count of not read VCR
        //public int VCRPMDT = 0;                           //Glass Count of PMDT Glass(Dummy Pass)
        //public int VCRMatch = 0;                          //Glass Count of Same Reading-id as Host-id
        //public int VCRMismatch = 0;                       //Glass Count of Different Reading-id from Host-id
        //public int VCRKeyin = 0;                          //Glass Count of Operator Keyin
        //public int VCRTimeout = 0;                        //Glass Count of keyin timeout occurred
        //public int VCRSkip = 0;                           //Glass Count of Skip
        //public string VCRLastModified = "";               //최종 수정된 날짜

        //LOG 관련 설정
        public string LogFilePath = "";
        public Boolean LogUseThread = true;
        public int LogThreadInterval = 50;

        ////MMC Log Data 관련 변수 //추가 : 20101001 이상호
        //public string MCCNetworkPath = "";
        //public string MCCNetworkBasicPath = "";
        //public string MCCNetworkPort = "";
        //public string MCCNetworkUserID = "";
        //public string MCCNetworkPassword = "";
        //public string MCCLootFilePath = "";
        //public int MCCLogFileDelete = 0;                //MMC Log File를 저장할 기간

        //SEM 관련 변수 추가 - 110915 고석현
        public string SEM_BaudRate = "115200";
        public Boolean SEM_ErrorFlag = false;           //SEM 알람발생 여부 저장
        public int SEM_ErrorDelayCheckTime = 0;         //SEM 알람발생시 재연결시간 설정
        public int SEM_ErrorDelayCount = 0;             //SEM 알람발생시 재연결시간 카운트

        public int SEM_ErrorCount = 0;                  //SEM 에러 카운트
        public int SEM_ErrorCheckCount = 0;             //SEM 에러발생시 Check Count

        //Log 관련 변수 추가 - 2012.09.06 Kim Youngsik
        public int LogFileKeepDays = 0;

        //Monitor
        public int SizeWidth = 1024;                    //프로그램의 모니터 해상도
        public int SizeHeight = 768;                    //프로그램의 모니터 해상도

        //FDC Data 보고관련 변수- 12.04.04 ksh
        public Boolean FDCDataReport = false;
        public DateTime FDCDataSendTime = DateTime.Now;


        public SortedList APCChangeHOSTReport = new SortedList();
        public ArrayList APCDeleteReport = new ArrayList();
        public ArrayList PPCSetReport = new ArrayList();
        public ArrayList RPCSetReport = new ArrayList();

        public bool APCDBUpdateCheck = false;
        public bool PPCDBUpdateCheck = false;
        public bool RPCDBUpdateCheck = false;

        // HostDisconnect 관련 변수
        public int HostDisconnectedCount = 0;          // 호스트와 disconnect가 되었을때 지속된 시간을 카운트 하기위한 변수 (힘스 요청)    110411 고석현
        public int HostDisconnectChekCount = 0;       // INI에서 설정한 시간의 변수 (힘스 요청)    110411 고석현

        //Simulation Test Dummy에 사용할 변수 121106 Ko Seok Hyun
        public string SimulDummyPath = "";
        public bool SimulPPIDReadFlag = false;
        public int SimulReadPPIDType = 0;
        public string SimulReadPPID = "";
        public bool SimulEQPNormal_CMD = false;
        public bool SimulEQPPM_CMD = false;
        public bool SimulEQPProc_Pause_CMD = false;
        public bool SimulEQPProc_Resume_CMD = false;
        public string SimulEQPStateOld = "";
        public bool SimulAPCPPIDReadFlag = false;


        //Process Data 관련 변수
        public bool APCDataDel = false;
        public bool RPCDataDel = false;
        public bool PPCDataDel = false;

        public bool RPCDataSet = false;

        public bool EOIDIpdateFlag = false;
        public bool APCUSE = false;
        public bool RPCUSE = false;
        public bool PPCUSE = false;
        public int ProcDataKeepDays = 0;
        public bool APCPPIDReadFlag = false;

        public bool CreateNewDataSetbyCIM = false;

        //MultiUseData
        public string Host_IP = "";
        public string SAMPLING_TIME = "";
        public string LOGIN_ID = "";
        public string LOGIN_PW = "";
        public string MultiUseDataFilePath = "";

        //etc 20130208 lsc
        public bool SEM_ON = false;
        
        public int intProcessUnitCount = 0;
        public int SearchPPIDType = 2;

        public int dintTest = 0;
        public Boolean PLCActionEnd2 = false; //CIM에서 자체적으로 명령 내릴시 판단.
        public string DataCheckGLSID = "";
        public bool APCStartEQPPPIDCheck = false;
        public string HostPPID = "";

        public string RPCPPID = "";

        public string CreatePPIDName = "";
        public int CreatePPIDType = 0;
        public bool PPIDSearchFlag = false;

        public int simulB1036_Data = 0;
        public bool simulB1036_Flag = false;
        public bool simulB1036_Flag2 = false;
        public string simul_EQPPPID = "";



        public bool GlassUpperJobFlag = false;

        //MCC
        public bool MCC_ConnectState = false;
        public int MCCFileUploadTime = 0;
        public Boolean MCCFileUploadUse = false;
        public Process MCCprocess;
        public Boolean MCCprocStarted = false;
        public string MCCprocPath = "";
        public string MCCprocName = "MCC";
        public bool MCC_UPLOAD = false;
        public bool MCC_RunFlag = false;

        public bool FI01_DEP_Flag = false; // FI01구간에서 필림이 배출..

        public string MODEL_NAME = "";
        public string ST01GLSID = "";
        public bool MCC_Simulation = false;

        public string EQPID = "";

        //SEM UDP 관련
        public UdpClient UDPSendPort = null;  //20141106 이원규 (SEM_UDP)
        public UdpClient UDPRecvPort = null;  //20141106 이원규 (SEM_UDP)
        public IPEndPoint UDPRemoteEndPoint;  //20141106 이원규 (SEM_UDP)
        public IPEndPoint UDPLocalEndPoint;  //20141106 이원규 (SEM_UDP)
        public bool USE_UDP = false;  //20141106 이원규 (SEM_UDP)
        public IPAddress UDP_IP = new IPAddress(new byte[] { 192, 168, 100, 200 });  //20141106 이원규 (SEM_UDP)
        public int UDP_PORT = 9001;  //20141106 이원규 (SEM_UDP)
        public Boolean SEMMode = false;

        public bool dbolAliveFlag = false;

        //HostppidMapping 변수
        public bool B113CFlag = false;
        public bool B113DFlag = false;

        public bool B1119Flag = false;

        public bool B1016Flag = false;
    }
}

