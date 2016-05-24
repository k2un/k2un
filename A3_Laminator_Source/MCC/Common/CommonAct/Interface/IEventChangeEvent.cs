using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{

    public delegate void ClientChangeEvent(string ChangeData);     //델리게이트 선언
    
    public interface IEventChangeEvent
    {

        event ClientChangeEvent ChangeEvent;                    //이벤트 선언

        //*******************************************************************************
        //  Function Name : SendReceiveEvent()
        //  Description   : 이벤트를 발생시킨다.
        //  Parameters    : ChangeData => scan Data가 변경되었음을 알린다.
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/03/05          어 경태         [L 00] 
        //*******************************************************************************
        void SendChangeEvent(string ChangeData);


    }
}
