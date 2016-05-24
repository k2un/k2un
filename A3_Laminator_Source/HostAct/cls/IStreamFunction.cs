using System;
using System.Collections.Generic;
using System.Text;
using NSECS;

/// <summary>
/// Interface of StreamFunction Class
/// </summary>
namespace HostAct
{
    public interface IStreamFunction
    {
        //void funSetHostAct(clsHostAct host);
        void funPrimaryReceive(Transaction msgTran);
        Transaction funPrimarySend(string strParameters);
        void funSecondaryReceive(Transaction msgTran);
    }
}
