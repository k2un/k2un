using System;
using System.Text;

namespace InfoAct
{
    public class clsGLSAPD
    {
        public int Index = 0;               //Index(순번) - Key 값
        public string Name = "";            //Name(항목명)
        public int Length = 0;              //읽을 길이
        public string Format = "";          //Format(99.9, 999 등)
        public Boolean HaveMinusValue = false;           //음수 값을 가지는지 여부
        public string Value = "";           //Value
        //public string Type = "";            //Data Type(DataTime, Numeric(평균값 계산 가능한 것), Boolean(Use 혹은 Unuse), Other)
        //public int UnitID = 0;              //해당 Unit
        public string ModuleID = "";        //ModuleID

        //Constructor
        public clsGLSAPD(int intIndex)
        {
            this.Index = intIndex;
        }
    }
}
