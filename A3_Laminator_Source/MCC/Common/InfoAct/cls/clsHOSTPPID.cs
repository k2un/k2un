using System;
using System.Text;

namespace InfoAct
{
    public class clsHOSTPPID
    {
        //public int UnitID = 0;
        //public int SubUnitID = 1;

        public string HostPPID = "";        //HOST PPID
        public string PPIDVer = "";         //PPIDVer Version
        public string DateTime = "";        //PPID 생성, 수정 날짜
        public string EQPPPID = "";         //HOST PPID에 해당되는 장비(EQP) PPID
        public string Comment = "";         //HOST PPID에 대한 Comment

        //Constructor
        public clsHOSTPPID(string strHOSTPPID)
        {
            this.HostPPID = strHOSTPPID;
        }

        //public clsRecipe(int intUnitID, string strRecipeID)
        //{
        //    this.UnitID = intUnitID;
        //    this.HostRecipe = strRecipeID;
        //}
  
    }
}
