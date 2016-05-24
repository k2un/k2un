using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using InfoAct;

namespace LogAct
{
    /// <summary>
    /// Base Class of clsNLog classes
    /// </summary>
    public class clsNLog
    {
        #region Fields
        protected clsInfo pInfo = clsInfo.Instance;
        protected clsInfo.LogType pLogType;

        protected string pstrLogName = "";
        protected string pstrLogFilePath = "";
        protected System.Windows.Forms.Timer ptmrLogWrite;
        protected System.Threading.Thread pThreadLogWrite;
        protected Boolean pbolDateFolderExist = false;

        protected string pstrLogDate = "";                         //PLC 로그를 쓰기위한 현재시간(yyyy-MM-dd HH:mm:ss) 저장
        protected string pstrLogDateFolder = "";                      //yyyyMMdd 폴더경로
        protected FileStream pfsNLog = null;                           //로그를 쓰기위한 FileStream
        protected StreamWriter pswNLog = null;                         //로그를 쓰기위한 StreamWriter
        protected string strOldStreamName = "";

        protected int intLogCount = 0;
        protected int intLogInterval = 100;

        protected bool ThreadStop = false;

        #endregion

        #region Properties
        public clsInfo.LogType LogType
        {
            get { return pLogType; }
        }
        #endregion

        #region  Constructor
        public clsNLog()
        {

        }
        #endregion

        #region Methods
        public void Close()
        {
            try
            {
                ThreadStop = true;

                if (pThreadLogWrite != null)
                {
                    pThreadLogWrite.Abort();
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// InfoAct.clsInfo의 pLogQueueHash에 Queue를 추가한다.
        /// Hash의 Key 값은 pLogType의 String Value
        /// </summary>
        protected void CreateLogQueue()
        {
            try
            {
                pInfo.AddLogQueue(pLogType.ToString());
            }
            catch (Exception ex)
            {

            }
        }

        protected void CreateFileStream(string strStreamName)
        {
            try
            {
                CloseFileStream();

                strOldStreamName = strStreamName;

                pfsNLog = new FileStream(strStreamName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                pswNLog = new StreamWriter(pfsNLog);
            }
            catch
            {

            }
        }

        protected void CloseFileStream()
        {
            try
            {
                if (pfsNLog != null) pfsNLog.Close();
                if (pswNLog != null) pswNLog.Close();
            }
            catch
            {

            }
        }
        #endregion
    }
}
