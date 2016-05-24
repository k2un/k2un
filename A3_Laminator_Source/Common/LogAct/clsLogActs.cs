using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Timers;
using System.Collections.Generic;

namespace LogAct
{
    public class clsLogActs
    {
        
        #region 선언

        public string Version
        {
            get { return "Samsung SMD HCLN V1.0"; }
        }

        public InfoAct.clsInfo PInfo;

       
        private frmLogView pfrmLogView;
        private frmDataView pfrmDataView;

        private string pstrLogFilesPath = Application.StartupPath + @"\PLCLOG";

        private System.Windows.Forms.Timer ptmrLogWrite;            //LOG를 Write할 타이머 정의      
        private System.Windows.Forms.Timer ptmrMCCLogUpdate;        //MCC Log를 Update할 타이머 정의     //추가 : 20101001 이상호


        private FileStream pLogFS = null;                           // LOG FileStream
        private StreamWriter pLogSW = null;                         // LOG StreamWriter




        private List<clsLogFileInfo> plstFileInfo = null;

        #region 이건 일단 보류
        /// <summary>
        /// MCC Data Log를 사용하기 위한 변수                       //추가 : 20101001 이상호
        /// </summary>
        private string pstrMCCDateFolder = "";                      //yyyyMMdd 폴더경로
        private FileStream pMCCFS = null;                           //Parameter 로그를 쓰기위한 FileStream
        private StreamWriter pMCCSW = null;                         //Parameter 로그를 쓰기위한 StreamWriter
        private int pintMCCUpdateCount = 0;                         //FTP Server로 Update하는 시간을 체크
        private string pstrDateTime = "";
        private string pstrBackupDateNowTime = "";

        private string pstrDateFolder = "";
        private string pstrUploadDateFolder = "";
        private string pstrDateBackupFolder = "";
        private string pstrIDXDateFolder = "";
        private string pstrFileName = "";
        private string pstrUploadFileName = "";
        private string pstrIDXFileName = "";
        private bool StartCheckFlag = false;
        private bool UploadCheckFlag = false;

        private InfoAct.clsFTP_Client pFTP;                         //추가 : 20101001 이상호

        private int pintLogKeepDays = 90;                           //PLCLOG 보관 기간(기본 30일)
        #endregion
        #endregion

        /// <summary>
        /// DateFolder가 없으면 생성
        /// </summary>
        /// <param name="strDateFolder">DateFolder Path</param>
        /// <remarks>
        /// 20120313    이상창
        /// </remarks>
        private void subSetDateFolder(string strDateFolder)
        {
            try
            {
                if (Directory.Exists(strDateFolder) == false)
                {
                    Directory.CreateDirectory(strDateFolder);
                    this.pstrDateFolder = strDateFolder;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        private string funGetLogFileName(InfoAct.clsInfo.LogType dLogType)
        {
            string dstrTmp = string.Empty;

            switch (dLogType)
            {
                case InfoAct.clsInfo.LogType.PLC:
                    dstrTmp = "PLC.Log";
                    break;
                case InfoAct.clsInfo.LogType.PLCError:
                    dstrTmp = "PLCError.Log";
                    break;
                case InfoAct.clsInfo.LogType.CIM:
                    dstrTmp = "CIM.Log";
                    break;
                case InfoAct.clsInfo.LogType.Alarm:
                    dstrTmp = "Alarm.Log";
                    break;
                case InfoAct.clsInfo.LogType.ScrapUnScrapAbort:
                    dstrTmp = "Scrap.Log";
                    break;
                case InfoAct.clsInfo.LogType.GLSInOut:
                    dstrTmp = "GLSInOut.Log";
                    break;
                case InfoAct.clsInfo.LogType.GLSPDC:
                    dstrTmp = "GLSPDC.Log";
                    break;
                case InfoAct.clsInfo.LogType.LOTPDC:
                    dstrTmp = "LOTPDC.Log";
                    break;
                case InfoAct.clsInfo.LogType.Parameter:
                    dstrTmp = "Parameter.Log";
                    break;
                case InfoAct.clsInfo.LogType.MCCLog:
                    // subWriteMCCLog(string strLog) <--- 생각좀
                    break;
                case InfoAct.clsInfo.LogType.AlarmGLSInfo:
                    dstrTmp = "AlarmGLSInfo.Log";
                    break;
                case InfoAct.clsInfo.LogType.btnEVENT:
                    dstrTmp = "ButtonEvent.Log";
                    break;
                case InfoAct.clsInfo.LogType.SEM:
                    dstrTmp = "SEM.Log";
                    break;
                default:
                    dstrTmp = string.Empty;
                    break;
            }

            return dstrTmp;

        }

        private void subInitLogFileInfoHash()
        {
            int dintLength = Enum.GetValues(typeof(InfoAct.clsInfo.LogType)).Length;

            this.plstFileInfo = new List<clsLogFileInfo>();


            for (int i = 0; i < dintLength; i++)
            {
                clsLogFileInfo tmp = new clsLogFileInfo();

                tmp.FileName = funGetLogFileName((InfoAct.clsInfo.LogType)i);
                this.plstFileInfo.Add(tmp);
            }

           
            
        }

        /// <summary>
        /// 지정한 기간이 지나면 LOG를 삭제한다. (기본 30일)
        /// </summary>
        /// <remarks>
        /// 2007/07/07  김효주  [L 00]
        /// </remarks>
        private void subDeleteLogFile()
        {
            string strDelFolder = "";

            try
            {
                strDelFolder = DateTime.Now.AddDays(-pintLogKeepDays).ToString("yyyyMMdd");     //삭제할 로그폴더 날짜

                if (Directory.Exists(pstrLogFilesPath + "\\" + strDelFolder) == true)           //로그폴더가 존재하면 삭제한다.
                {
                    Directory.Delete(pstrLogFilesPath + "\\" + strDelFolder, true);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 초기화시 "PLCLOG" 폴더가 없으면 생성하고 로그 출력 Timer 생성
        /// </summary>
        /// <remarks>
        /// 최초 프로그램 로딩시 한번만 수행됨
        /// 2006/10/25  김효주  [L 00]
        /// </remarks>
        public void subInitialLog()
        {
            string dstrIPAddress = "192.169.1.1";
            string dstrPortNumber = "21";
            try
            {
                DirectoryInfo dir = new DirectoryInfo(pstrLogFilesPath);     //PLCLOG 폴더가 없으면 생성한다.
                dir.Create();

                this.pfrmLogView = new frmLogView();
                this.pfrmLogView.PInfo = this.PInfo;

                this.pfrmDataView = new frmDataView();
                this.pfrmDataView.PInfo = this.PInfo;
                this.pfrmDataView.subFormLoad();



                // hashtable 준비

                subInitLogFileInfoHash();


                //FTP 객체 생성     //추가 : 20101001 이상호
                pFTP = new InfoAct.clsFTP_Client(dstrIPAddress + ":" + dstrPortNumber, "", "");
                pFTP.PInfo = this.PInfo;

                //PLC Log Tick Timer 설정
                this.ptmrLogWrite = new System.Windows.Forms.Timer();
                this.ptmrLogWrite.Tick += new EventHandler(LogWrite_Tick);
                this.ptmrLogWrite.Interval = 500;      //500ms
                this.ptmrLogWrite.Enabled = true;
                GC.KeepAlive(this.ptmrLogWrite);

                //MMC Log Tick Timer 설정
                this.ptmrMCCLogUpdate = new System.Windows.Forms.Timer();
                this.ptmrMCCLogUpdate.Tick += new EventHandler(MCCLogUpdate_Tick);
                this.ptmrMCCLogUpdate.Interval = 1000;      //500ms
                this.ptmrMCCLogUpdate.Enabled = true;
                GC.KeepAlive(this.ptmrMCCLogUpdate);


                
            }
            catch(Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Log Queue에서 로그를 출력한다.
        /// </summary>
        /// <remarks>
        /// 2006/11/03  김효주  [L 00]
        /// </remarks>
        private void LogWrite_Tick(Object Object, EventArgs EventArgs)
        {
            string dstrData = "";

            try
            {
                ptmrLogWrite.Enabled = false;                           //수행중 다시 들어오는것을 방지한다.             


                //PLCLog Write
                

                //PLCErrorLog Write
                

                //CIMLog Write
                

                //AlarmLog Write
                

                //Scrap, Unscrap Log Write
                

                //GLS In/Out Log Write
                

                //GLSAPDLog Write
                

                //LOTAPDLog Write
                

                //ParameterLog Write
                

                //AlarmGLSInfo Write
               

                //Button Event Log Write
               

                //SEM Log Write
               

                ptmrLogWrite.Enabled = true;                                //Timer를 다시 기동한다.
            }

            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                ptmrLogWrite.Enabled = true;
            }
        }

        /// <summary>
        /// MCC Log를 출력한다.
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="EventArgs"></param>
        /// <remarks>
        /// 20101001            이상호          [L 00]
        /// </remarks>
        private void MCCLogUpdate_Tick(Object Object, EventArgs EventArgs)
        {
          
            try
            {
                
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                ptmrMCCLogUpdate.Enabled = true;
            }
        }
  
    }
}
