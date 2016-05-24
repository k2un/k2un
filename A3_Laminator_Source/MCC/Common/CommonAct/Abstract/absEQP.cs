using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public abstract class absEQP : absPLC, IPCCommand
    {

        #region IPCCommand 멤버

        public abstract void subTransfer(string Message);
        public abstract void subReceive(string Message);

        #endregion
    }
}
