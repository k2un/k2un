using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface ITCPproperty
    {
        string proConnectionMode
        {
            get;
            set;
        }
        
        string proRemoteIP
        {
            get;
            set;
        }

        int proRemotePort
        {
            get;
            set;
        }

    }
}
