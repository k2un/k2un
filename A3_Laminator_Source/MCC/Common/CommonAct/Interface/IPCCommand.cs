using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public interface IPCCommand
    {
        //*******************************************************************************
        //  Function Name : subTransfer()
        //  Description   : PC와 통신할 경우 메세지를 보내는 함수이다.
        //  Parameters    : 보내야할 메세지
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/02/06          최 성원         [L 00] 
        //*******************************************************************************
        void subTransfer(string Message);

        //*******************************************************************************
        //  Function Name : subReceive()
        //  Description   : PC와 통신할 경우 메세지를 받는 함수이다.
        //  Parameters    : 받은 메세지
        //  Return Value  : PC 에서 보내온 응답값을 리턴한다.
        //  Special Notes : 
        //*******************************************************************************
        //  2007/02/06          최 성원         [L 00] 
        //*******************************************************************************
        void subReceive(string Message);
    }
}
