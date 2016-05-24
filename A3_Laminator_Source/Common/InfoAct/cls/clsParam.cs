using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoAct
{
    public class clsParam
    {
        public InfoAct.clsInfo.PLCCommand pintPLCCommand;
        public object[] pobjParameter;


        public clsParam()
        {
        }

        public clsParam(InfoAct.clsInfo.PLCCommand intPLCCommand, object[] objParameter)
        {
            this.pintPLCCommand = intPLCCommand;
            this.pobjParameter = objParameter;
        }

        public InfoAct.clsInfo.PLCCommand Command
        {
            get
            {
                return this.pintPLCCommand;
            }
            set
            {
                this.pintPLCCommand = value;
            }
        }

        public object[] Parameter
        {
            get
            {
                return this.pobjParameter;
            }
            set
            {
                this.pobjParameter = value;
            }
        }

        public string subToString()
        {
            string dstrData = "";

            dstrData += pintPLCCommand.ToString()+", ";

            foreach (object obj in pobjParameter)
            {
                dstrData += obj.ToString() + ", "; 
            }

            return dstrData;
        }
    }
}
