using System;
using System.Collections.Generic;
using System.Text;

namespace LogAct
{
    public class clsLogFileInfo
    {
        private string pstrLastWriteTime = string.Empty;
        private string pstrLogFileName = string.Empty;


        public string LastModify
        {
            get
            {
                return this.pstrLastWriteTime;
            }
            set
            {
                this.pstrLastWriteTime = value;
            }
        }
        public string FileName
        {
            get
            {
                return this.pstrLogFileName;
            }
            set
            {
                this.pstrLogFileName = value;
            }
        }

        public string funGetFullPath(string strPath)
        {
            return strPath + @"\" + this.pstrLogFileName;
        }

    }
}
