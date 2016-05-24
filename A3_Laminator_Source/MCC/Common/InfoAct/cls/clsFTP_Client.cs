using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.IO;
//추가 : 20090806 이상호
using System.Windows.Forms;
using System.ComponentModel;

namespace InfoAct
{
	public class clsFTP_Client
    {
        #region "선언"

        public delegate void FTPuploadComplete();
        public event FTPuploadComplete UploadCompleted;

        public InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;
        private BackgroundWorker bFTPUpload;        //BackgroundWorker

		//error exception class
		public class FTPException : Exception
		{
			public FTPException( string sMsg) : base(sMsg){}
			public FTPException( string sMsg, Exception exFTP) : base(sMsg, exFTP){}
		}

        private FtpWebRequest pFtpRequest = null;
        private FtpWebResponse pFtpResponse = null;

		//static variable
		private static int		SIZE_SEND_BUFF = 512;				//buffer send size
		private static int		SIZE_RECV_BUFF = 1024;				//buffer receive size
		private static Encoding COMM_MODE = Encoding.ASCII;			//communication mode

		//FTP communication parameter variable
		private string m_sIP		= "LocalHost";						//FTP server address
		private string m_sPath		= string.Empty;						//FTP server directory path
		private string m_sID		= "anonymous";						//FTP server login user ID
		private string m_sPW		= "anonymous@anonymous.net";		//FTP server login password
		private string m_sMsg		= string.Empty;						//FTP server connection message
		private string m_sResult	= string.Empty;						//FTP server return message
        private string m_sFileName_T  = string.Empty;                     //FTP server File Name
        private string m_sFileName_I = string.Empty;                     //FTP server File Name
        private string m_sIndexFileName = string.Empty;
        private string m_FileDateTime = string.Empty; //20150216 고석현 추가

		private int m_iPort			= 21;								//FTP server port
		//private int m_iBytes		= 0;							
		private int m_iTimeOut		= 10;								//FTP server timeout
		//private int m_iResult		= 0;								//socket return code

		//private bool m_bConnect		= false;							//FTP server connection flage
		//private bool m_bCommMode	= false;							//FTP server connection mode

		private Byte[] m_bytBuffSend = new Byte[SIZE_SEND_BUFF];		//send buffer
		private Byte[] m_bytBuffRecv = new Byte[SIZE_RECV_BUFF];		//receive buffer

        private string pstrLootFilePath = "";                           //원본 데이터 폴더
        private string pstrIndexFilePath = "";                         //인덱스 파일 폴더

        private byte m_FileType = 0;    //  LOG = 0, INDEX = 1, LOG & INDEX = 2

        private bool m_LOGcomplete = false;
        private bool m_IDXcomplete = false;
		//socket object
		//private Socket m_FTP_Socket = null;

		/// <summary>
		/// contructor
		/// </summary>
		public clsFTP_Client() { }
		public clsFTP_Client(string sIP, string sID, string sPW)
		{
			this.m_sIP = sIP;
			this.m_sID = sID;
			this.m_sPW = sPW;
		}
		public clsFTP_Client(string sIP, string sID, string sPW, int iTimeOut, int iPort)
		{
			this.m_sIP		= sIP;
			this.m_sID		= sID;
			this.m_sPW		= sPW;
			this.m_iTimeOut	= iTimeOut;
			this.m_iPort		= iPort;
		}

        protected virtual void OnUploadCompleted()
        {
            FTPuploadComplete handler = UploadCompleted;
            if (handler != null) handler();
        }

		/// Remote server port. Typically TCP 21		
		public int Connect_Port
		{
			get	{return this.m_iPort;}
			set	{this.m_iPort = value;}
		}
		/// Timeout waiting for a response from server, in seconds.		
		public int Comm_Timeout
		{
			get	{return this.m_iTimeOut;}
			set	{this.m_iTimeOut = value;}
		}
		/// Gets and Sets the name of the FTP server.
		public string Server_IP
		{
			get	{return this.m_sIP;}
			set	{this.m_sIP = value;}
		}
		/// GetS and Sets the remote directory.
		public string Server_Path
		{
			get	{return this.m_sPath;}
			set	{this.m_sPath = value;}
		}
		/// Gets and Sets the username.
		public string Login_ID
		{
			get	{return this.m_sID;}
			set	{this.m_sID = value;	}
		}
		/// Gets and Set the password.
		public string Login_PW
		{
			get	{return this.m_sPW;}
			set	{this.m_sPW = value;}
        }
        /// server connection
        public bool bConnect
        {
            get { return this.bConnect; }
        }
        #endregion

        public void subFTPFunc_1(string strFileType, string strUploadPath, string strDatePath, string strLocalPath, string strFileName_T, string strFileName_I)
        {
            string dstrMCCNetworkPath = "";
            string dstrMCCNetworkPort = "";

            try
            {
                dstrMCCNetworkPath = this.PInfo.All.MCCNetworkPath;
                dstrMCCNetworkPort = this.PInfo.All.MCCNetworkPort;
                if (dstrMCCNetworkPath.EndsWith("/")) dstrMCCNetworkPath = dstrMCCNetworkPath.Remove(dstrMCCNetworkPath.Length - 1);
                if (this.PInfo.All.MCCNetworkPort == "") dstrMCCNetworkPort = "21";

                m_sIP = "FTP://"+dstrMCCNetworkPath + ":" + dstrMCCNetworkPort + "/%2f";              //Ftp Server IP

                //저장할 경로
                m_sPath = strDatePath;
            
                m_sFileName_T = strFileName_T;                                                //저장파일
                m_sFileName_I = strFileName_I;
                m_sIndexFileName = strFileName_T.Substring(2); //(strFileType != "INDEX") ? "index-" + strFileName.Replace("\\", "") : strFileName;
                m_sID = this.PInfo.All.MCCNetworkUserID;
                m_sPW = this.PInfo.All.MCCNetworkPassword;

                if (strDatePath != "")
                {
                    switch (strFileType)
                    {
                        case "Both":
                            m_FileType = 2;
                            break;
                        case "LOG":
                            m_FileType = 0;
                            break;
                        case "INDEX":
                            m_FileType = 1;
                            break;
                        default:
                            // 이건 걍 전부라고 생각하자.
                            m_FileType = 2;
                            break;
                    }


                    pstrIndexFilePath = strLocalPath.Substring(0, strLocalPath.Length - m_sPath.Length) + "index";    //Index 데이터 폴더
                    pstrLootFilePath = strLocalPath;                                                                  //원본 데이터 폴더

                    bFTPUpload_Start();     //업로드 백그라운드 작업시작
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subFTPFunc(string strFileType, string strUploadPath, string strDatePath, string strLocalPath, string strFileName)
        {
            string dstrMCCNetworkPath = "";
            string dstrMCCNetworkPort = "";

            try
            {
                dstrMCCNetworkPath = this.PInfo.All.MCCNetworkPath;
                dstrMCCNetworkPort = this.PInfo.All.MCCNetworkPort;
                if (dstrMCCNetworkPath.EndsWith("/")) dstrMCCNetworkPath = dstrMCCNetworkPath.Remove(dstrMCCNetworkPath.Length - 1);
                if (this.PInfo.All.MCCNetworkPort == "") dstrMCCNetworkPort = "21";

                m_sIP = "FTP://" + dstrMCCNetworkPath + ":" + dstrMCCNetworkPort + "/%2f";              //Ftp Server IP

                //저장할 경로
                m_sPath = strDatePath;

                m_sFileName_T = strFileName;                                                //저장파일
                m_sIndexFileName = strFileName.Substring(2); //(strFileType != "INDEX") ? "index-" + strFileName.Replace("\\", "") : strFileName;
                m_sID = this.PInfo.All.MCCNetworkUserID;
                m_sPW = this.PInfo.All.MCCNetworkPassword;

                if (strDatePath != "")
                {
                    switch (strFileType)
                    {
                        case "Both":
                            m_FileType = 2;
                            break;
                        case "LOG":
                            m_FileType = 0;
                            break;
                        case "INDEX":
                            m_FileType = 1;
                            break;
                        default:
                            // 이건 걍 전부라고 생각하자.
                            m_FileType = 2;
                            break;
                    }


                    pstrIndexFilePath = strLocalPath.Substring(0, strLocalPath.Length - m_sPath.Length) + "index";    //Index 데이터 폴더
                    pstrLootFilePath = strLocalPath;                                                                  //원본 데이터 폴더

                    bFTPUpload_Start();     //업로드 백그라운드 작업시작
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        /// <summary>
        /// File Upload 작업을 구현 확장자 및 (FTP / 네트워크에 따라 구분)
        /// </summary>
        /// <param name="strSaveType">0:검사후 바로 저장, 1:지정시간에 저장, 2:일정시간 단위로 저장</param>
        /// <param name="strUploadPath">파일을 Upload 할 경로 (Ftp Server Address 또는 네트워크 경로</param>
        /// <param name="strFilePath">저장할 파일 경로</param>
        /// <param name="strFileName">저장할 파일 이름</param>
        /// <param name="strFileType">저장할 파일 확장자 Type (0:Image Type, 1:Txt Type, 2:CSV Type)</param>
        /// <remarks>
        /// 2008/10/16          김지호          [L 00]
        /// 20090807            이상호          [L 01]  //파일 확장자로 구분해서 업로드
        /// 20120626            이상창          [L 92]  // 파일 확장자 구분 삭제, SaveType 용도 변경
        /// 20150216 고석현 추가
        /// </remarks>
        public void subFTPFunc_2(string strSaveType, string strUploadPath, string strFilePath, string strFileName_T, string strFileName_I, string strFileDteTime)
        {
            string dstrMCCNetworkPath = "";
            string dstrMCCNetworkPort = "";
            string[] arrPath;

            try
            {
                dstrMCCNetworkPath = this.PInfo.All.MCCNetworkPath;
                dstrMCCNetworkPort = this.PInfo.All.MCCNetworkPort;
                if (dstrMCCNetworkPath.EndsWith("/")) dstrMCCNetworkPath = dstrMCCNetworkPath.Remove(dstrMCCNetworkPath.Length - 1);
                if (this.PInfo.All.MCCNetworkPort == "") dstrMCCNetworkPort = "21";

                m_sIP = "FTP://" + dstrMCCNetworkPath + ":" + dstrMCCNetworkPort + "/%2f";              //Ftp Server IP

                //m_sPath = strFileName_T.Substring((strFileName_T.IndexOf("-")+12), 6); // 20120509 이상창
                arrPath = strFileName_T.Split('-');
                if (arrPath.Length > 2)
                {
                    m_sPath = arrPath[1];
                }

                m_sFileName_T = strFileName_T;                                                //저장파일
                m_sFileName_I = strFileName_I;
                m_sIndexFileName = "index" + strFileName_T.Substring(1); //"index-" + strFileName.Replace("\\", "");
                m_sID = this.PInfo.All.MCCNetworkUserID;
                m_sPW = this.PInfo.All.MCCNetworkPassword;
                m_FileDateTime = strFileDteTime;
                
                if (strFilePath != "")
                {
                    pstrIndexFilePath = strFilePath.Substring(0, strFilePath.Length - 6) + "index";    //Index 데이터 폴더
                    pstrLootFilePath = strFilePath;                                                                  //원본 데이터 폴더

                    m_FileType = 2;

                    bFTPUpload_Start();     //업로드 백그라운드 작업시작
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void FtpUploadStart()
        {
            try
            {
                bFTPUpload_Start();     //업로드 백그라운드 작업시작
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Image File Upload
        /// </summary>
        /// <param name="strUplodUrl">Ftp Server Url</param>
        /// <param name="strFileName"></param>
        /// <param name="strUserName"></param>
        /// <param name="strPassWord"></param>
        /// <returns></returns>
        public bool funFTPFileUpload()
        {
            Stream requestStream = null;
            FileStream fileStream = null;
            string dstrFTPPath = "";
            string dstrFilePath = "";
            string dstrIndexPath = "";
            string dstrBackupFTPPath = "";
            WebClient dWC = new WebClient();
            string pstrUploadFileName_T = "";
            string pstrUploadFileName_I = "";
            string pstrUploadDateFolder = "";
            string pstrDateFolder = "";
            string pstrFileDateTime = "";

            try
            {
                m_LOGcomplete = true;
                m_IDXcomplete = true;

                dstrFTPPath = m_sIP;

                //20150216 고석현 추가
                for (int dintLoop = 1; dintLoop <= PInfo.UnitCount; dintLoop++)
                {
                    for (int dintLoop2 = 1; dintLoop2 <= PInfo.Unit(dintLoop).SubUnitCount; dintLoop2++)
                    {
                        pstrUploadFileName_T = string.Format("T-{0}-{1}.csv", PInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID, pstrFileDateTime);
                        pstrUploadFileName_I = string.Format("T-{0}-{1}.csv", PInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID, pstrFileDateTime);


                        pstrUploadDateFolder = pstrUploadDateFolder.Substring(0, 9);
                        string[] arrPath = PInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID.Split('_');
                        string strPath = "";
                        for (int dintLoop3 = 0; dintLoop3 < arrPath.Length; dintLoop3++)
                        {
                            strPath += arrPath[dintLoop3];
                            pstrUploadDateFolder += @"\" + strPath;
                            strPath += "_";

                        }

                        pstrUploadDateFolder += pstrDateFolder.Substring(18);

                    }

                }

                if (this.PInfo.All.MCCNetworkBasicPath.EndsWith("/"))
                {
                    this.PInfo.All.MCCNetworkBasicPath = this.PInfo.All.MCCNetworkBasicPath.Substring(0, this.PInfo.All.MCCNetworkBasicPath.Length - 1);
                }

                string[] dstrFTPBasicPath = this.PInfo.All.MCCNetworkBasicPath.Split('/');
                foreach (string dstrBasicPath in dstrFTPBasicPath)
                {
                    dstrFTPPath = FtpListDirectory(dstrFTPPath, dstrBasicPath);
                    if (dstrFTPPath == "")
                    {
                        m_LOGcomplete = false;
                        m_IDXcomplete = false;
                        return false;
                    }
                    dstrFTPPath += "/";
                }
                
                dstrBackupFTPPath = dstrFTPPath;


                if (m_FileType == 0 || m_FileType == 2)
                {

                    try
                    {
                        //오늘 날짜 폴더를 검색하고 경로를 받아온다.
                        dstrFTPPath = FtpListDirectory(dstrFTPPath, m_sPath);
                        if (dstrFTPPath == "")
                        {
                            m_LOGcomplete = false;
                            m_IDXcomplete = false;
                            return false;
                        }
                         FtpListDirectory(dstrFTPPath, "/T");
                         FtpListDirectory(dstrFTPPath, "/I");

                        //파일 업로드경로
                        //dstrFTPPath = dstrFTPPath + "/I/"+ m_sFileName_I.Replace("\\", "");
                         string strFTPUploadPath = dstrFTPPath + "/I/" + m_sFileName_I.Replace("\\", "");

                        // 원본데이터 파일경로 (strFilePath)
                        dstrFilePath = pstrLootFilePath +@"\I"+ m_sFileName_I;

                        if (dstrFilePath == "")
                        {
                            m_LOGcomplete = false;
                            m_IDXcomplete = false;
                            return false;
                        }

                        fileStream = File.Open(dstrFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        pFtpRequest = (FtpWebRequest)WebRequest.Create(strFTPUploadPath);
                        pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                        pFtpRequest.Proxy = null;
                        pFtpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                        pFtpRequest.Timeout = 10000;
                        requestStream = pFtpRequest.GetRequestStream();


                        byte[] buffer = new byte[1024];
                        int bytesRead = 0; ;
                        while (true)
                        {
                            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                                break;
                            requestStream.Write(buffer, 0, bytesRead);
                            Thread.Sleep(0);
                        }

                        requestStream.Close();
                        pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();

                       
                        //dstrFTPPath = dstrFTPPath + "/" + m_sFileName_T.Replace("\\", "");
                        strFTPUploadPath = dstrFTPPath + "/T/" + m_sFileName_T.Replace("\\", "");

                        // 원본데이터 파일경로 (strFilePath)
                        dstrFilePath = pstrLootFilePath + @"\T" + m_sFileName_T;

                        if (dstrFilePath == "")
                        {
                            m_LOGcomplete = false;
                            m_IDXcomplete = false;
                            return false;
                        }

                        fileStream = File.Open(dstrFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        pFtpRequest = (FtpWebRequest)WebRequest.Create(strFTPUploadPath);
                        pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                        pFtpRequest.Proxy = null;
                        pFtpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                        pFtpRequest.Timeout = 10000;
                        requestStream = pFtpRequest.GetRequestStream();


                        buffer = new byte[1024];
                        bytesRead = 0;
                        while (true)
                        {
                            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                                break;
                            requestStream.Write(buffer, 0, bytesRead);
                            Thread.Sleep(0);
                        }

                        requestStream.Close();
                        pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();

                        //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", dstrFilePath.Substring(dstrFilePath.LastIndexOf(@"\") + 1)));
                    }
                    catch (Exception ex)
                    {

                        // DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", dstrFilePath.Substring(dstrFilePath.LastIndexOf(@"\")+1)));
                        m_LOGcomplete = false;

                        if (m_FileType == 0) throw ex;
                        else this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    }
                }

                if (m_FileType == 1 || m_FileType == 2)
                {

                    try
                    {
                        dstrFTPPath = FtpListDirectory(dstrBackupFTPPath, "index");

                        //파일 업로드경로
                        dstrFTPPath = dstrFTPPath + "/" + m_sIndexFileName.Replace("\\", "");

                        // 원본 Index데이터 파일경로 (strFilePath)
                        dstrIndexPath = pstrIndexFilePath + @"\" + m_sIndexFileName.Replace("\\", "");

                        fileStream = File.Open(dstrIndexPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        pFtpRequest = (FtpWebRequest)WebRequest.Create(dstrFTPPath);
                        pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                        pFtpRequest.Proxy = null;
                        pFtpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                        pFtpRequest.Timeout = 10000;
                        requestStream = pFtpRequest.GetRequestStream();


                        byte[] buffer = new byte[1024];
                        int bytesRead;

                        while (true)
                        {
                            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                                break;
                            requestStream.Write(buffer, 0, bytesRead);
                            Thread.Sleep(0);
                        }

                        requestStream.Close();
                        pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();

                        //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", dstrIndexPath.Substring(dstrIndexPath.LastIndexOf(@"\") + 1)));
                    }
                    catch (Exception ex)
                    {
                        m_IDXcomplete = false;
                        //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", dstrIndexPath.Substring(dstrIndexPath.LastIndexOf(@"\") + 1)));
                        // 디비 Ing 수정
                        //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                        throw ex;
                    }
                }

                if (m_LOGcomplete) return true;
                else return false;
            }
            catch (UriFormatException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            catch (IOException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            catch (WebException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            finally
            {
                if (pFtpRequest != null) pFtpRequest.Abort();
                if (pFtpResponse != null) pFtpResponse.Close();
                if (fileStream != null) fileStream.Close();
                if (requestStream != null) requestStream.Close();
            }
        }

        /// <summary>
        /// 20150216 고석현 추가
        /// </summary>
        /// <returns></returns>
        public bool funFTPFileUpload2()
        {
            Stream requestStream = null;
            FileStream fileStream = null;
            string dstrFTPPath = "";
            string dstrFilePath = "";
            string dstrIndexPath = "";
            string dstrBackupFTPPath = "";
            WebClient dWC = new WebClient();
            string pstrUploadFileName_T = "";
            string pstrUploadFileName_I = "";
            string pstrUploadDateFolder = "";
            string pstrDateFolder = "";
            string pstrUploadFileName_index = "";
            string strTemp = "";
            string strPath = "";
            string[] arrPath;
            string strFTPUploadPath = string.Empty;
            byte[] buffer = new byte[1024];
            int bytesRead = 0; ;

            try
            {
                m_LOGcomplete = true;
                m_IDXcomplete = true;

                //20150216 고석현 추가
                for (int dintLoop = 1; dintLoop <= PInfo.UnitCount; dintLoop++)
                {
                    for (int dintLoop2 = 1; dintLoop2 <= PInfo.Unit(dintLoop).SubUnitCount; dintLoop2++)
                    {
                        dstrFTPPath = m_sIP;
                        pstrUploadDateFolder = "";
                        pstrUploadFileName_T = string.Format("T-{0}-{1}.csv", PInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID, m_FileDateTime);
                        pstrUploadFileName_I = string.Format("I-{0}-{1}.csv", PInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID, m_FileDateTime);
                        pstrUploadFileName_index = string.Format("index-{0}-{1}.csv", PInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID, m_FileDateTime);

                        arrPath = PInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID.Split('_');
                        dstrFTPPath += "/";
                        strPath = "fs2";
                        dstrFTPPath = FtpListDirectory(dstrFTPPath, strPath) + "/";
                        strPath = "fab02";
                        dstrFTPPath = FtpListDirectory(dstrFTPPath, strPath) + "/";
                        strPath = "";
                        for (int dintLoop3 = 0; dintLoop3 < arrPath.Length; dintLoop3++)
                        {
                            strPath += arrPath[dintLoop3];
                            //[2015/04/13]MCC담당자 요청으로 Layer1 삭제(Add by HS)
                            if (dintLoop3 == 1)
                            {
                                strPath += "_";

                                continue;
                            }
                            dstrFTPPath = FtpListDirectory(dstrFTPPath, strPath);
                            if (string.IsNullOrEmpty(dstrFTPPath))
                            {
                                m_LOGcomplete = false;
                                m_IDXcomplete = false;
                                return false;
                            }
                            dstrFTPPath += "/";

                            pstrUploadDateFolder += @"\" + strPath;
                            strPath += "_";

                        }

                        dstrBackupFTPPath = dstrFTPPath;


                        strTemp = FtpListDirectory(dstrFTPPath, m_FileDateTime.Substring(2, 6)) + "/";
                        FtpListDirectory(strTemp, "T");
                        FtpListDirectory(strTemp, "I");
                        FtpListDirectory(dstrFTPPath, "index");

                        #region I폴더 업로드
                        strFTPUploadPath = dstrFTPPath + m_FileDateTime.Substring(2, 6) + "/I/" + pstrUploadFileName_I.Replace("\\", "");

                        // 원본데이터 파일경로 (strFilePath)
                        dstrFilePath = @"D:\MCCLOG" + pstrUploadDateFolder + @"\" + m_FileDateTime.Substring(2,6) + @"\I\" + pstrUploadFileName_I;

                        if (dstrFilePath == "")
                        {
                            m_LOGcomplete = false;
                            m_IDXcomplete = false;
                            return false;
                        }

                        fileStream = File.Open(dstrFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        pFtpRequest = (FtpWebRequest)WebRequest.Create(strFTPUploadPath);
                        pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                        pFtpRequest.Proxy = null;
                        pFtpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                        pFtpRequest.Timeout = 10000;
                        requestStream = pFtpRequest.GetRequestStream();
                        
                        System.Array.Clear(buffer,0,buffer.Length);
                        
                        bytesRead = 0;
                        while (true)
                        {
                            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0) break;
                            requestStream.Write(buffer, 0, bytesRead);
                            Thread.Sleep(1);
                        }

                        requestStream.Close();
                        pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();

                        //[2015/06/10] File Close(Add by HS)
                        fileStream.Close();
                        fileStream = null;
                        #endregion

                        #region T폴더 업로드

                        strFTPUploadPath = dstrFTPPath + m_FileDateTime.Substring(2, 6) + "/T/" + pstrUploadFileName_T.Replace("\\", "");

                        // 원본데이터 파일경로 (strFilePath)
                        dstrFilePath = @"D:\MCCLOG" + pstrUploadDateFolder + @"\" + m_FileDateTime.Substring(2, 6) + @"\T\" + pstrUploadFileName_T;

                        if (dstrFilePath == "")
                        {
                            m_LOGcomplete = false;
                            m_IDXcomplete = false;
                            return false;
                        }

                        fileStream = File.Open(dstrFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        pFtpRequest = (FtpWebRequest)WebRequest.Create(strFTPUploadPath);
                        pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                        pFtpRequest.Proxy = null;
                        pFtpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                        pFtpRequest.Timeout = 10000;
                        requestStream = pFtpRequest.GetRequestStream();


                        System.Array.Clear(buffer, 0, buffer.Length);

                        bytesRead = 0;
                        while (true)
                        {
                            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                                break;
                            requestStream.Write(buffer, 0, bytesRead);
                            Thread.Sleep(0);
                        }

                        requestStream.Close();
                        pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();

                        //[2015/06/10] File Close(Add by HS)
                        fileStream.Close();
                        fileStream = null;
                        #endregion

                        #region index 폴더 업로드

                        strFTPUploadPath = dstrFTPPath + "index/" + pstrUploadFileName_index.Replace("\\", "");

                        // 원본데이터 파일경로 (strFilePath)
                        dstrFilePath = @"D:\MCCLOG" + pstrUploadDateFolder + @"\index\" + pstrUploadFileName_index;

                        if (dstrFilePath == "")
                        {
                            m_LOGcomplete = false;
                            m_IDXcomplete = false;
                            return false;
                        }

                        fileStream = File.Open(dstrFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        pFtpRequest = (FtpWebRequest)WebRequest.Create(strFTPUploadPath);
                        pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                        pFtpRequest.Proxy = null;
                        pFtpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                        pFtpRequest.Timeout = 10000;
                        requestStream = pFtpRequest.GetRequestStream();


                        System.Array.Clear(buffer, 0, buffer.Length);

                        bytesRead = 0; ;
                        while (true)
                        {
                            bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                                break;
                            requestStream.Write(buffer, 0, bytesRead);
                            Thread.Sleep(0);
                        }

                        requestStream.Close();
                        pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();

                        //[2015/06/10] File Close(Add by HS)
                        fileStream.Close();
                        fileStream = null;
                        //buffer = null;
                        arrPath = null;

                        #endregion

                    }

                }       

                if (m_LOGcomplete) return true;
                else return false;
            }
            catch (UriFormatException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            catch (IOException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            catch (WebException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            finally
            {
                if (pFtpRequest != null) pFtpRequest.Abort();
                if (pFtpResponse != null) pFtpResponse.Close();
                if (fileStream != null) fileStream.Close();
                if (requestStream != null) requestStream.Close();
            }
        }

        /// <summary>A
        /// Ftp Server의 특정경로의 날짜폴더를 검색
        /// </summary>
        /// <param name="directory">날짜폴더가 있는 경로</param>
        /// <returns>날짜폴더가 있으면 : True</returns>
        public string FtpListDirectory(string directory, string dstrPath)
        {
            StreamReader dstrReader = null;
            string dstrListDir = "";
            Boolean bolTodayDir = false;
            Boolean bolTodayCreateDir = false;
            string dstrFTPPath = "";

            try
            {
                //Uri dUri = new Uri(directory);
                pFtpRequest = (FtpWebRequest)WebRequest.Create(directory);
                pFtpRequest.Proxy = null;
                pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                pFtpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                pFtpRequest.Timeout = 10000;

                //FTP Directory
                pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();
                dstrReader = new StreamReader(pFtpResponse.GetResponseStream(), System.Text.Encoding.Default);
                dstrListDir = dstrReader.ReadToEnd();

                string[] fileInDirectory;

                //if (this.PInfo.EQP("Main").EQPID.StartsWith("A2"))
                {
                    fileInDirectory = dstrListDir.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);    // A2
                }
                //else
                //{
                //    fileInDirectory = dstrListDir.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);      // V1
                //}


                for (int i = 0; i <= fileInDirectory.Length - 1; i++)
                {
                    if (fileInDirectory[i] == dstrPath)
                    {
                        bolTodayDir = true;
                        break;
                    }
                }

                dstrFTPPath = directory + dstrPath;
                //오늘 날짜 폴더가 없으면 생성
                if (bolTodayDir != true)
                {
                    bolTodayCreateDir = FtpCreateDirectory(dstrFTPPath);
                    if (bolTodayCreateDir != true) dstrFTPPath = "";
                }
            }
            catch (UriFormatException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return dstrFTPPath;
            }
            catch (IOException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return dstrFTPPath;
            }
            catch (WebException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return dstrFTPPath;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return dstrFTPPath;
            }
            return dstrFTPPath;
        }

        /// <summary>
        /// Ftp Server의 특정경로의 폴더생성
        /// </summary>
        /// <param name="dirpath">폴더를 생성할 경로</param>
        /// <returns></returns>
        public bool FtpCreateDirectory(string dirpath)
        {
            try
            {
                try
                {
                    pFtpRequest = (FtpWebRequest)WebRequest.Create(dirpath);
                    pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                    pFtpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    pFtpRequest = (FtpWebRequest)WebRequest.Create(dirpath);
                    pFtpRequest.Credentials = new NetworkCredential(m_sID, m_sPW);
                    pFtpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    pFtpResponse = (FtpWebResponse)pFtpRequest.GetResponse();
                }
                
                return true;
            }
            catch (UriFormatException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            catch (IOException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
            catch (WebException ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
        }

        //FTP Upload BackgroundWorker
        #region bFTPUpload

        private void bFTPUpload_Start()
        {
            bFTPUpload = new BackgroundWorker();

            // set the working thread
            bFTPUpload.DoWork += (DoWorkEventHandler)bFTPUpload_DoWork;

            // set the handler and property for the progress
            //bFTPUpload.WorkerReportsProgress = true;
            //bFTPUpload.ProgressChanged += (ProgressChangedEventHandler)bFTPUpload_ProgressChanged;

            // set the property for the cancellation
            bFTPUpload.WorkerSupportsCancellation = true;

            // set the handler for the completion
            bFTPUpload.RunWorkerCompleted += (RunWorkerCompletedEventHandler)bFTPUpload_RunWorkerCompleted;

            // get starting the BackgroundWorker.
            bFTPUpload.RunWorkerAsync();
        }

        private void bFTPUpload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (e.Error != null)
            //{
            //}
            //else if (e.Cancelled)
            //{
            //    // Next, handle the case where the user canceled 
            //    // the operation.
            //    // Note that due to a race condition in 
            //    // the DoWork event handler, the Cancelled
            //    // flag may not have been set, even though
            //    // CancelAsync was called.
            //}
            //else
            //{
            //    // Finally, handle the case where the operation 
            //    // succeeded.
            //}


            OnUploadCompleted();

            GC.Collect(); 
            //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, e.ToString());
        }

        private void bFTPUpload_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        /// <summary>
        /// This belongs to another thread.
        /// It cannot access UI controls within this thread.
        /// </summary>
        private void bFTPUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;

            try
            {
                lock (new object())
                {
                    if (!funFTPFileUpload2())
                    {
                        //MessageBox.Show("Ftp Transfer Error!\r\nFile Upload Fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);   //20090813 구정환
                        //this.PInfo.subSendSF_Set(clsInfo.SFName.S6F11EquipmentSpecifiedNetworkEvent, 171, this.PInfo.All.MCCNetworkPath);

                        Console.WriteLine("ERR");

                        // 실패한거네...
                        // 여기서 디비에서 Ing 변경...
                        // 근데 어느건지 어케 아니?
                        // FtpFileUpload() 안에서 해야하나?
                        if (m_FileType == 2)    // "Both"
                        {
                            if (m_LOGcomplete)
                            {
                                // 로그 파일은 성공했다네..
                                //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName.Replace("\\", "")));

                                if (this.PInfo.All.pblUseMDB)
                                {
                                    if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                    DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName_T.Replace("\\", ""));
                                    DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                }

                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sFileName_T.Substring(1));

                                if (this.PInfo.All.pblUseMDB)
                                {
                                    if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                    DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName_I.Replace("\\", ""));
                                    DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                }

                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sFileName_I.Substring(1));
                            }
                            else
                            {
                                //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sFileName.Replace("\\", "")));

                                if (this.PInfo.All.pblUseMDB)
                                {
                                    if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                    DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sFileName_T.Replace("\\", ""));
                                    DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                }

                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Failure, Filename : " + m_sFileName_T.Substring(1));

                                if (this.PInfo.All.pblUseMDB)
                                {
                                    if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                    DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sFileName_I.Replace("\\", ""));
                                    DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                }

                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Failure, Filename : " + m_sFileName_I.Substring(1));
                            }

                            //Thread.Sleep(200);

                            if (m_IDXcomplete)
                            {
                                // 인덱스 파일은 성공했다네.
                                //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", "")));

                                if (this.PInfo.All.pblUseMDB)
                                {
                                    if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                    DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", ""));
                                    DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                }

                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sIndexFileName);
                            }
                            else
                            {
                                //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", "")));

                                if (this.PInfo.All.pblUseMDB)
                                {
                                    if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                    DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", ""));
                                    DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                }

                                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Failure, Filename : " + m_sIndexFileName);
                            }
                        }
                        else if (m_FileType == 1)    // "Index"
                        {
                            //m_sIndexFileName.Replace("\\", "")
                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", "")));

                            if (this.PInfo.All.pblUseMDB)
                            {
                                if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                            }

                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Failure, Filename : " + m_sIndexFileName.Substring(1));
                        }
                        else if (m_FileType == 0)   // "LOG"
                        {
                            //m_sFileName.Replace("\\", "")
                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sFileName.Replace("\\", "")));

                            if (this.PInfo.All.pblUseMDB)
                            {
                                if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sFileName_T.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                            }

                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Failure, Filename : " + m_sFileName_T.Substring(1));

                                if (this.PInfo.All.pblUseMDB)
                            {
                                if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"UPDATE `tbMCCfile` SET `Ing`='False' WHERE `FileName`='{0}';", m_sFileName_I.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                            }

                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Failure, Filename : " + m_sFileName_I.Substring(1));
                        }
                    }
                    else
                    {
                        //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sFileName);

                        if (m_FileType == 2)    // "Both"
                        {
                            // 디비에서 둘다 지우자..
                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName.Replace("\\", "")));
                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", "")));


                            if (this.PInfo.All.pblUseMDB)
                            {
                                if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName_T.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName_I.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                            }


                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sFileName_T.Substring(1));
                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sFileName_I.Substring(1));
                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sIndexFileName);
                        }
                        else if (m_FileType == 1)    // "Index"
                        {
                            //m_sIndexFileName.Replace("\\", "")
                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", "")));

                            if (this.PInfo.All.pblUseMDB)
                            {
                                if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sIndexFileName.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                            }

                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sIndexFileName.Substring(1));
                        }
                        else if (m_FileType == 0)   // "LOG"
                        {
                            //m_sFileName.Replace("\\", "")
                            //DBAct.clsDBAct.funExecuteQuery(DBAct.clsDBAct.pstrMCCConnection, string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName.Replace("\\", "")));

                            if (this.PInfo.All.pblUseMDB)
                            {
                                if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName_T.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                            }

                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sFileName_T.Substring(1));

                            if (this.PInfo.All.pblUseMDB)
                            {
                                if (DBAct.clsDBAct.MCCcommand.Transaction == null) DBAct.clsDBAct.funMCCBeginTransaction();
                                DBAct.clsDBAct.MCCcommand.CommandText = string.Format(@"DELETE FROM `tbMCCfile` WHERE `FileName`='{0}';", m_sFileName_I.Replace("\\", ""));
                                DBAct.clsDBAct.MCCcommand.ExecuteNonQuery();
                            }

                            this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "FTP File Upload Sucess, Filename : " + m_sFileName_I.Substring(1));
                        }

                    }


                    if (this.PInfo.All.pblUseMDB) DBAct.clsDBAct.MCCcommand.Transaction.Commit();
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                //if (this.PInfo.All.pblUseMDB) DBAct.clsDBAct.MCCcommand.Transaction.Rollback();
            }
            finally
            {
                worker.CancelAsync();
                worker.Dispose();
            }
        }

        #endregion
    }
}

