using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoAct
{
    public class clsRPC
    {
        public string HGLSID = "";
        public string JOBID = "";
        public string JOBID_Old = "";
        public string RPC_PPID = "";
        public DateTime SetTime = new DateTime();
        public string RPC_PPID_Old = "";
        public DateTime SetTime_Old = new DateTime();
        public string OriginPPID = "";
        public int RPC_STATE = 1;


        public int Mode = 1;

        //Constructor
        public clsRPC(string strGLSID)
        {
            this.HGLSID = strGLSID;
        }
    }
}
