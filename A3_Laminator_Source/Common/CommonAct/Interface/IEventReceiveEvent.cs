using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{

    public delegate void ClientReceiveEvent(string strReceiveData);     //델리게이트 선언

    public interface IEventReceiveEvent
    {

        event ClientReceiveEvent ReceiveEvent;                    //이벤트 선언

        /// <summary>
        /// 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="strReceiveData">Data가 수신되었음을 알린다.</param>
        void SendReceiveEvent(string strReceiveData);

    }

}
