using System;
using System.Text;

namespace InfoAct
{
    public class clsCEID
    {
        public int CEID = 0;
        public string CEIDName = "";
        public bool Report = false;

        //Constructor
        public clsCEID(int intCEID)
        {
            this.CEID = intCEID;
        }
    }
}
