using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{

    public delegate void ClientChangeEvent(string ChangeData);     //델리게이트 선언
    
    public interface IEventChangeEvent
    {

        event ClientChangeEvent ChangeEvent;                    //이벤트 선언

        /// <summary>
        /// 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="ChangeData">scan Data가 변경되었음을 알린다.</param>
        void SendChangeEvent(string ChangeData);


    }
}
