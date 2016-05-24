using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using InfoAct;

namespace LogAct
{
    /// <summary>
    /// NLog General Format
    /// 동 시간대의 Log를 묶음 단위로 출력하는 로그를 만들 경오 (예, PLC, CIM Log)
    /// </summary>
    public class clsNLogGroup : clsNLog
    {
        #region Fields
        #endregion

        #region Properties
        public string LogName
        {
            get { return pstrLogName; }
            set { pstrLogName = value; }
        }
        #endregion

        #region Constructor
        public clsNLogGroup(string strPath, string strName, clsInfo.LogType enmLogType)
        {
            pstrLogFilePath = strPath;
            pstrLogName = strName;
            pLogType = enmLogType;

            intLogInterval = pInfo.All.LogThreadInterval;

            if (pInfo.All.LogUseThread == false)
            {
                ptmrLogWrite = new System.Windows.Forms.Timer();
                ptmrLogWrite.Tick += new EventHandler(LogWrite_Tick);
                ptmrLogWrite.Interval = intLogInterval;
                ptmrLogWrite.Enabled = false;
                GC.KeepAlive(ptmrLogWrite);
            }
            else
            {
                pThreadLogWrite = new Thread(new ThreadStart(ThreadLogWrite));
                pThreadLogWrite.Name = string.Format("ThreadLogWrite_{0}", strName);
                pThreadLogWrite.IsBackground = true;
                pThreadLogWrite.Start();
            }

            CreateLogQueue();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Log Write를 하기 위한 Thread
        /// </summary>
        private void ThreadLogWrite()
        {
            do
            {
                try
                {
                    string dstrText = "";

                    while ((pInfo.LogQueueCount(pLogType.ToString()) > 0) && (intLogCount < 100))
                    {
                        dstrText = pInfo.DequeueLog(pLogType.ToString());

                        if (!string.IsNullOrWhiteSpace(dstrText))
                        {
                            intLogCount++;
                            WriteLog(dstrText);
                        }
                    }
                }
                catch
                {

                }
                finally
                {
                    intLogCount = 0;
                    Thread.Sleep(intLogInterval);
                }
            } while (!ThreadStop);
        }

        private void LogWrite_Tick(Object Object, EventArgs EventArgs)
        {
            try
            {
                ptmrLogWrite.Enabled = false;

                string dstrText = "";

                while ((pInfo.LogQueueCount(pLogType.ToString()) > 0) && (intLogCount < 100))
                {
                    dstrText = pInfo.DequeueLog(pLogType.ToString());

                    if (!string.IsNullOrWhiteSpace(dstrText))
                    {
                        intLogCount++;
                        WriteLog(dstrText);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                intLogCount = 0;
                ptmrLogWrite.Enabled = true;
            }
        }

        private void WriteLog(string strLogText)
        {
            StringBuilder dstrWirteMsg;
            try
            {
                string dstrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string dstrFileName = string.Format("\\{0}.Log", pstrLogName);
                 string dstrDateFolder = pstrLogFilePath + @"\" + DateTime.Now.ToString("yyyyMMdd");
                string dstrCurrentStreamName = dstrDateFolder + dstrFileName;
                StringBuilder dstrWriteMsg = new StringBuilder();

                if (pbolDateFolderExist == false || pstrLogDateFolder != dstrDateFolder)
                {
                    if (Directory.Exists(dstrDateFolder) == false)
                    {
                        Directory.CreateDirectory(dstrDateFolder);
                    }
                    pbolDateFolderExist = true;
                }

                //바로 이전에 출력한 로그 시간과 같으면 시간, "="은 제외하고 로그를 출력
                if (strLogText.Contains(pstrLogDate) && pstrLogDate != "")
                {
                    if(dstrFileName.Contains("PLC"))
                    {
                        dstrWriteMsg.Append(strLogText.Substring(76));      //날짜는 제외하고 로그 문자열만 뽑아온다.
                    }
                    else
                    {
                        dstrWriteMsg.Append(strLogText.Substring(73));      //날짜는 제외하고 로그 문자열만 뽑아온다.
                    }
                    //dstrWriteMsg.Append(strLogText.Substring(76));      //날짜는 제외하고 로그 문자열만 뽑아온다.
                }
                else
                {
                    dstrWriteMsg.Append(strLogText);
                }

                //pstrLogDate = strLogText.Substring(1, 19);           //출력한 시간으로 Overwrite
                pstrLogDate = strLogText.Substring(1, 23);           //출력한 시간으로 Overwrite

                pstrLogDateFolder = dstrDateFolder;   //현재 날짜를 저장

                if (strOldStreamName != dstrCurrentStreamName)
                {
                    CreateFileStream(dstrCurrentStreamName);
                }

                //pfsNLog = new FileStream(dstrDateFolder + dstrFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                //pswNLog = new StreamWriter(pfsNLog);

                pswNLog.WriteLine(dstrWriteMsg);
                pswNLog.Flush();

                //pswNLog.Close();
                //pswNLog.Close();
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion
    }
}
