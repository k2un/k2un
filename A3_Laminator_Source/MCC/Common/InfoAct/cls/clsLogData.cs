using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsLogData
    {
        private InfoAct.clsInfo.LogType pLogType;
        private string pstrbLogData;

        public clsLogData(InfoAct.clsInfo.LogType logType, string strLogData)
        {
            this.pLogType = logType;
            this.pstrbLogData = strLogData;
        }

        public InfoAct.clsInfo.LogType Type
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
        public string Log
        {
            get
            {
                return this.pstrbLogData;
            }
            set
            {
                this.pstrbLogData = value;
            }
        }
    }
}
