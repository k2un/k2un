using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LogAct
{
    public class clsLogFiles
    {
        private InfoAct.clsInfo.LogType pLogType;
        private string pstrFileName;
        private FileStream pFS;
        private StreamWriter pSW;
        private string pstrLastModify = string.Empty;
        private DateTime pCreateDate = DateTime.Now.Date;

        public clsLogFiles(InfoAct.clsInfo.LogType dLogType, string dstrFileName, FileStream dFS)
        {
            this.pLogType = dLogType;
            this.pstrFileName = dstrFileName;
            this.pFS = dFS;
            if (this.pFS != null) this.pSW = new StreamWriter(this.pFS);
        }
        public DateTime CreateDate
        {
            get
            {
                return this.pCreateDate;
            }
        }
        public InfoAct.clsInfo.LogType logType
        {
            get
            {
                return this.pLogType;
            }
            set
            {
                this.pLogType = value;
            }
        }
        public string Name
        {
            get
            {
                return this.pstrFileName;
            }
            set
            {
                this.pstrFileName = value;
            }
        }
        public FileStream Stream
        {
            get
            {
                return this.pFS;
            }
            set
            {
                this.pFS = value;
            }
        }
        public StreamWriter Writer
        {
            get
            {
                return this.pSW;
            }
            set
            {
                this.pSW = value;
            }
        }
        public string LastModify
        {
            get
            {
                return this.pstrLastModify;
            }
            set
            {
                this.pstrLastModify = value;
            }
        }

        public void Close()
        {
            try
            {
                if (this.pSW != null)
                {
                    this.pSW.Flush();
                    this.pSW.Close();
                    this.pSW = null;
                }

                if (this.pFS != null)
                {
                    this.pFS.Close();
                    this.pFS = null;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
