using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsLOTAPD
    {
        public int Index = 0;               //Index(순번)
        public string Name = "";            //Name(항목명)
        public int Length = 0;              //Length
        public string Format = "";          //Format(99.9, 999, YYYY, MM, DD 등)
        public Boolean HaveMinusValue = false;           //음수 값을 가지는지 여부
        public string Value = "";           //Value
        public string Type = "";            //Data Type(DataTime, Numeric(평균값 계산 가능한 것), Boolean(Use 혹은 Unuse), Other)
        //public int UnitID = 0;              //해당 Module에 대한 UnitID
        public string ModuleID = "";        //ModuleID

        //Constructor
        public clsLOTAPD(int intIndex)
        {
            this.Index = intIndex;
        }

        public void CopyFrom(clsLOTAPD lotApd)
        {
            this.Name = lotApd.Name;
            this.Length = lotApd.Length;
            this.Format = lotApd.Format;
            this.Type = lotApd.Type;
            this.ModuleID = lotApd.ModuleID;
        }
    }
}
