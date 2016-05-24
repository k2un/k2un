using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Threading;
using InfoAct;

namespace LogAct
{
    /// <summary>
    /// 새로운 LogAct
    /// </summary>
    public class clsLogAct
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;

        private string pstrLogFilesPath;// = Application.StartupPath + @"\PLCLOG";

        private frmLogView pfrmLogView;
        private frmDataView pfrmDataView;

        private System.Windows.Forms.Timer ptmrDateCheck;
        private System.Threading.Thread pThreadDateCheck;

        private clsNLogGroup plogPLC;
        private clsNLogGroup plogPLCError;

        private clsNLogGroup plogCIM;
        private clsNLogGeneral plogAlarmGlassInfo;
        private clsNLogGeneral plogOPCallMsg;
        private clsNLogGeneral plogScrap;
        private clsNLogGeneral plogGlassInOut;
        private clsNLogGeneral plogGlassAPD;
        private clsNLogGeneral plogLotAPD;
        private clsNLogGeneral plogAlarm;
        private clsNLogGeneral plogParameter;
        private clsNLogGeneral plogSEM;
        private clsNLogGeneral plogHostMSG;
        private clsNLogGeneral plogCleaner_DV;
        private clsNLogGeneral plogOven_DV;

        private int dintThreadCount = 0;

        private bool ThreadStopFlag = false;
        
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsLogAct Instance = new clsLogAct();
        #endregion

        #region Constructors
        public clsLogAct()
        {

        }
        #endregion

        #region Methods
        /// <summary>
        /// Log 생성
        /// </summary>
        public void subInitialLog()
        {
            try
            {
                if (pInfo.All.LogUseThread == true)
                {
                    pThreadDateCheck = new Thread(new ThreadStart(ThreadLogDateCheck));
                    pThreadDateCheck.Name = string.Format("ThreadLogDateCheck");
                    pThreadDateCheck.IsBackground = true;
                    pThreadDateCheck.Start();
                }
                else
                {
                    ptmrDateCheck = new System.Windows.Forms.Timer();
                    ptmrDateCheck.Tick += new EventHandler(DateCheck_Tick);
                    ptmrDateCheck.Interval = 10000;
                    ptmrDateCheck.Enabled = true;
                }

                pstrLogFilesPath = pInfo.All.LogFilePath;

                plogPLC = new clsNLogGroup(pstrLogFilesPath, "PLC", clsInfo.LogType.PLC);
                plogCIM = new clsNLogGroup(pstrLogFilesPath, "CIM", clsInfo.LogType.CIM);

                plogAlarmGlassInfo = new clsNLogGeneral(pstrLogFilesPath, "AlarmGLSInfo", clsInfo.LogType.AlarmGLSInfo);
                plogOPCallMsg = new clsNLogGeneral(pstrLogFilesPath, "OPCallMSG", clsInfo.LogType.OPCallMSG);
                plogPLCError = new clsNLogGroup(pstrLogFilesPath, "PLCError", clsInfo.LogType.PLCError);
                plogScrap = new clsNLogGeneral(pstrLogFilesPath, "Scrap", clsInfo.LogType.ScrapUnScrapAbort);
                plogGlassInOut = new clsNLogGeneral(pstrLogFilesPath, "GLSInOut", clsInfo.LogType.GLSInOut);
                plogGlassAPD = new clsNLogGeneral(pstrLogFilesPath, "GLSPDC", clsInfo.LogType.GLSPDC);
                plogLotAPD = new clsNLogGeneral(pstrLogFilesPath, "LOTPDC", clsInfo.LogType.LOTPDC);
                plogAlarm = new clsNLogGeneral(pstrLogFilesPath, "Alarm", clsInfo.LogType.Alarm);
                plogParameter = new clsNLogGeneral(pstrLogFilesPath, "Parameter", clsInfo.LogType.Parameter);
                plogSEM = new clsNLogGeneral(pstrLogFilesPath, "SEM", clsInfo.LogType.SEM);
                this.pfrmLogView = new frmLogView();

                //if (this.pInfo.EQP("Main").DummyPLC)
                //{
                    //this.pfrmDataView = new frmDataView();
                    //this.pfrmDataView.subFormLoad();
                //}
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// LogAct를 종료시킨다.
        /// </summary>
        public void subCloseLog()
        {
            try
            {
                ThreadStopFlag = true;
                plogPLC.Close();

                if (pThreadDateCheck != null)
                {
                    pThreadDateCheck.Abort();
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 주기적으로 Log Folder를 검색하여 삭제한다.
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="EventArgs"></param>
        private void DateCheck_Tick(Object Object, EventArgs EventArgs)
        {
            try
            {
                ptmrDateCheck.Enabled = false;

                subDeleteLogFile();
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                ptmrDateCheck.Enabled = true;
            }
        }

        /// <summary>
        /// 주기적으로 Log Folder를 검색하여 삭제하는 Thread
        /// </summary>
        private void ThreadLogDateCheck()
        {
            do
            {
                try
                {
                    if (dintThreadCount > 10)
                    {
                        subDeleteLogFile();
                        dintThreadCount = -1;
                    }
                    dintThreadCount++;
                }
                catch (Exception ex)
                {
                    pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            } while (!ThreadStopFlag);
        }

        /// <summary>
        /// Log Folder 내의 Sub Folder 중 삭제를 원하는 날짜 이전 Log Folder를 모두 삭제
        /// </summary>
        private void subDeleteLogFile()
        {
            try
            {
                DirectoryInfo LogDirectory = new DirectoryInfo(pstrLogFilesPath);
                DirectoryInfo[] subDirs = LogDirectory.GetDirectories();

                DateTime dateTarget = DateTime.ParseExact(DateTime.Now.AddDays(-pInfo.All.LogFileKeepDays).ToString("yyyyMMdd"), "yyyyMMdd", null);

                foreach (DirectoryInfo dir in subDirs)
                {
                    string[] strDate = dir.FullName.Split('\\');
                    
                    DateTime dateCurrentDir;
                    if (DateTime.TryParseExact(strDate[strDate.Length - 1], "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dateCurrentDir))
                    {

                        if (dateCurrentDir <= dateTarget)
                        {
                            dir.Delete(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
