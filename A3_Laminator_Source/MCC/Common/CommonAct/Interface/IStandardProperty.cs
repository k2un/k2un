using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace CommonAct
{
    interface IStandardProperty
    {
        EnuCommunication.EQPNameType proEQPName
        {
            get;
            set;
        }
        EnuCommunication.EQPCommandType proEQPCommandType
        {
            get;
            set;
        }
        EnuCommunication.CommunicationType proCommunicationType
        {
            get;
            set;
        }
        bool proDummy
        {
            get;
            set;
        }
    }

}
