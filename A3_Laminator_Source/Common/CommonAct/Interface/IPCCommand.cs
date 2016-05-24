using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface IPCCommand
    {
        /// <summary>
        /// PC와 통신할 경우 메세지를 보내는 함수이다.
        /// </summary>
        /// <param name="Message">보내야할 메세지</param>
        void subTransfer(string Message);

        /// <summary>
        /// PC와 통신할 경우 메세지를 받는 함수이다.
        /// </summary>
        /// <param name="Message">받은 메세지</param>
        void subReceive(string Message);
    }
}
