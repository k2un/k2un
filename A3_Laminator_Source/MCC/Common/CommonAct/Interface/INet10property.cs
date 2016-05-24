using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface INet10property
    {
        string proChannelNo
        {
            get;
            set;
        }

        string proNetworkNo
        {
            get;
            set;
        }

        string proGroupNo
        {
            get;
            set;
        }

        string proStationNo
        {
            get;
            set;
        }
    }
}
