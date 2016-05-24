using System;
using System.Text;

namespace InfoAct
{
    public class clsSVID
    {
        public int Index = 0;               //Index(순번) - Key 값
        public int SVID = 0;                //SVID
        public string Name = "";            //Name(항목명)
        public int Length = 0;              //Length
        public string Format = "";          //Format(99.9, 999 등)
        public string Value = "";           //Value(PLC로 부터 읽은 값)
        public string Type = "";            //Type(범주)
        public string Unit = "";            //Unit(단위)
        public string Range = "";           //Range(범위)
        public Boolean HaveMinusValue = false;           //음수 값을 가지는지 여부
        public string DESC = "";            //DESC
        public int UnitID = 0;              //해당 Module에 대한 UnitID
        public string ModuleID = "";        //ModuleID
        public int MCCInfoIndex = 0;
        
        //Constructor
        public clsSVID(int intSVID)
        {
            this.SVID = intSVID;
        }
    }
}
