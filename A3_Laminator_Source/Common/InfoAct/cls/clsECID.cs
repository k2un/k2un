using System;
using System.Text;

namespace InfoAct
{
    public class clsECID
    {
        public int Index = 0;               //Index(순번) - Key 값
        public string Name = "";            //Name(항목명)
        //public string Unit = "";            //Unit(단위)
        public string Min = "0";            //HOST에서 ECID 값 변경시 허용할 수 있는 최소값
        public string ECSLL = "0";          //ECSLL
        public string ECWLL = "0";          //ECWLL
        public string ECDEF = "0";          //ECDEF
        public string ECWUL = "0";          //ECWUL
        public string ECSUL = "0";          //ECSUL
        public string Max = "0";            //HOST에서 ECID 값 변경시 허용할 수 있는 최대값
        public string DESC = "";            //DESC
        public int UnitID = 0;              //해당 Module에 대한 UnitID
        public string Format = "";          //Format(99.9, 999 등)        
        public Boolean Use = false;         //해당 ECID 사용유무(True: 사용함, False: 사용하지 않음)
        public string ModuleID = "";        //ModuleID
        
        //Constructor
        public clsECID(int intIndex)
        {
            this.Index = intIndex;
        }
    }
}
