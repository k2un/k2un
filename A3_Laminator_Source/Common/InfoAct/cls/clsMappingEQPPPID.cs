using System;
using System.Text;

namespace InfoAct
{
    public class clsMappingEQPPPID : clsMappingEQPPPIDMethod
    {
        public string EQPPPID = "";         //장비(EQP) PPID
        public string DateTime = "";        //PPID 생성, 수정 날짜
        public string UP_EQPPPID = "";
        public string LOW_EQPPPID = "";
        public clsInfo.PPIDCMD PPIDCommand = clsInfo.PPIDCMD.None;

        //Constructor
        public clsMappingEQPPPID(string strEQPPPID)
        {
            this.EQPPPID = strEQPPPID;
        }
    }
}
