using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Interface of Equipment Event Action Methods
/// </summary>

namespace EQPAct
{
    public interface IEQPEvent
    {
        void funProcessEQPEvent(string[] parameters);
        void funProcessEQPStatus(string[] parameters);
    }
}
