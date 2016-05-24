using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoAct
{
    public class clsPPC
    {
        public string HGLSID = "";
        public string JOBID = "";
        public string EQPPPID = "";
        public DateTime SetTime = new DateTime();
        public string[] P_MODULEID;
        public string[] P_ORDER;
        public string[] P_STATUS;
        public int RunState = 1;


        //Constructor
        public clsPPC(string strGLSID)
        {
            this.HGLSID = strGLSID;
        }
    }
}
