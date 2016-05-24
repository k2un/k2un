using System;
using System.Text;

namespace InfoAct
{
    public class clsPPIDBody
    {
        public int Index = 0;               //Index(순번) - Key 값
        public string Name = "";            //Name(항목명)
        public int Length = 0;              //각 Body값의 Length
        public double Min = 0;              //허용할 수 있는 최소값
        public double Max = 0;              //허용할 수 있는 최대값
        public string Format = "";          //Format(99.9, 999 등)
        public string Unit = "";            //Unit(단위)
        public string Range = "";           //Range(범위)
        public string DESC = "";            //DESC
        public string Value = "";           //Value
        //public int UnitID = 0;              //해당 Module에 대한 UnitID
        public string ModuleID = "";        //ModuleID
        public bool UseMode = false;

        //Constructor
        public clsPPIDBody(int intIndex)
        {
            this.Index = intIndex;
        }
    }
}
