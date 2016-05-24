using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Interface of CIM Event Command Methods
/// </summary>

namespace EQPAct
{
    public interface ICIMEvent
    {
        void funProcessCIMEvent(object[] parameters);
    }
}
