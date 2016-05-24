using System;
using System.Text;

namespace InfoAct
{
    public class clsMaterial
    {
        public int Index = 0;                //Index(순번) - Key 값
        public string Name = "";            //Name

        //Constructor
        public clsMaterial(int intIndex)
        {
            this.Index = intIndex;
        }
    }
}
