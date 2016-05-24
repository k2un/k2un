using System;
using System.Text;

namespace InfoAct
{
    public class clsEOID
    {
        public int Index = 0;               //Index(순번) - Key 값
        public int EOID = 0;                //EOID
        public int EOMD = 0;                //EOMD
        public int EOMDMin = 0;             //Min
        public int EOMDMax = 0;             //Max
        public int EOV = 0;                 //EOV
        public int EOVMin = 0;              //Min
        public int EOVMax = 0;              //Max
        public string DESC = "";            //DESC(항목명)
        public Boolean PLCWrite = false;    //CIM이나 HOST에서 EOID를 변경할 경우 PLC로 써주는지 여부
        //public string Name = "";            //Name(항목명)

        //Constructor
        public clsEOID(int intIndex)
        {
            this.Index = intIndex;
        }

        ////Constructor
        //public clsEOID(int EOID, int EOMD)
        //{
        //    this.EOID = EOID;
        //    this.EOMD = EOMD;
        //}
    }
}
