using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{

    public delegate void ClientReceiveEvent(string strReceiveData);     //델리게이트 선언

    public interface IEventReceiveEvent
    {

        event ClientReceiveEvent ReceiveEvent;                    //이벤트 선언

        //*******************************************************************************
        //  Function Name : SendReceiveEvent()
        //  Description   : 이벤트를 발생시킨다.
        //  Parameters    : strReceiveData => Data가 수신되었음을 알린다.
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/06          어 경태         [L 00] 
        //*******************************************************************************
        void SendReceiveEvent(string strReceiveData);

    }

}
