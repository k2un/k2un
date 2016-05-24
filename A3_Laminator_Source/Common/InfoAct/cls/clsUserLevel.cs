using System;
using System.Text;

namespace InfoAct
{
    public class clsUserLevel
    {
        public int Level = 0;               //User Level
        public string Desc = "";            //Desc
        public string Comment = "";         //Comment

        //Constructor
        public clsUserLevel(int intLevel)
        {
            this.Level = intLevel;
        }
    }
}
