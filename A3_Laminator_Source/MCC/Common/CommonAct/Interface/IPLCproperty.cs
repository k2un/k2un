using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface IPLCproperty
    {
        string proWord1Start
        {
            get;
            set;
        }
        string proWord1End
        {
            get;
            set;
        }
        int proScanAreaCount
        {
            get;
            set;
        }
        bool[] proAreaScan
        {
            get;
            set;
        }
        string[] proAreaStart
        {
            get;
            set;
        }
        string[] proAreaEnd
        {
            get;
            set;
        }

        Hashtable proAddressMap
        {
            get;
            set;
        }
    }
}
