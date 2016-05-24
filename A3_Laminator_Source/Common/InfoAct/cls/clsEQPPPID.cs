using System;
using System.Text;

namespace InfoAct
{
    public class clsEQPPPID : clsEQPPPIDMethod
    {
        //public int UnitID = 0;
        //public int SubUnitID = 1;

        //public string HostPPID = "";        //HOST PPID
        public string EQPPPID = "";         //장비(EQP) PPID
        public string PPIDVer = "";         //PPIDVer Version
        public string DateTime = "";        //PPID 생성, 수정 날짜
        public string Comment = "";         //HOST PPID에 대한 Comment
        //public string PPIDBody = "";        //EQPPPID에 해당되는 PPID Body값이 저장
        public bool PPIDBodyCheck = false;

        //Constructor
        public clsEQPPPID(string strEQPPPID)
        {
            this.EQPPPID = strEQPPPID;
        }
    }
}
