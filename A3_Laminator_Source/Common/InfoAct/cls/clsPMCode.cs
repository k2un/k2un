using System;
using System.Text;

namespace InfoAct
{
    public class clsPMCode
    {
        public int Index = 0;                //Index(순번) - Key 값
        public string PMCode = "";           //PMCode
        public string Desc = "";             //Desc


        //Constructor
        public clsPMCode(int intIndex)
        {
            this.Index = intIndex;
        }
    }
}
