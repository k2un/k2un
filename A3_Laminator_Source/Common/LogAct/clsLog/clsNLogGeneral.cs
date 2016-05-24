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
    /// 일반적인 형태의 Log를 만들 경우 사용
    /// </summary>
    public class clsNLogGeneral : clsNLog
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
        public clsNLogGeneral(string strPath, string strName, clsInfo.LogType enmLogType)
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
                ptmrLogWrite.Enabled = true;
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
                    Thread.Sleep(intLogInterval);
                    intLogCount = 0;
                }
            } while (!ThreadStop);
        }

        /// <summary>
        /// Queue를 검색하여 저장된 Log를 파일로 기록한다.
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="EventArgs"></param>
        private void LogWrite_Tick(Object Object, EventArgs EventArgs)
        {
            try
            {
                ptmrLogWrite.Enabled = false;

                string dstrText = "";

                while ((pInfo.LogQueueCount(pLogType.ToString()) > 0) && (intLogCount < 100))
                {
                    dstrText = pInfo.DequeueLog(pLogType.ToString());

                    if (!string.IsNullOrWhiteSpace(dstrText))//(dstrText.Trim() != "") && (dstrText != null))
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
            try
            {
                string dstrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string dstrFileName = string.Format("\\{0}.Log", pstrLogName);
                string dstrDateFolder = pstrLogFilePath + @"\" + DateTime.Now.ToString("yyyyMMdd");
                string dstrCurrentStreamName = dstrDateFolder + dstrFileName;

                if (pbolDateFolderExist == false || pstrLogDateFolder != dstrDateFolder)
                {
                    if (Directory.Exists(dstrDateFolder) == false)
                    {
                        Directory.CreateDirectory(dstrDateFolder);
                    }
                    pbolDateFolderExist = true;
                }

                pstrLogDateFolder = dstrDateFolder;   //현재 날짜를 저장

                if (strOldStreamName != dstrCurrentStreamName)
                {
                    CreateFileStream(dstrCurrentStreamName);
                }

                //pfsNLog = new FileStream(dstrCurrentStreamName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                //pswNLog = new StreamWriter(pfsNLog);

                pswNLog.WriteLine(strLogText);
                pswNLog.Flush();

                //pfsNLog.Close();
                //pswNLog.Close();
            }
            catch (Exception ex)
            {
                
            }
        }

        #endregion
    }
}
