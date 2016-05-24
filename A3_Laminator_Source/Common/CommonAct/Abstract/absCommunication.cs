using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public abstract class absCommunication : clsCommunicationTotalProperty, IEventReceiveEvent, IEventChangeEvent
    {

        #region IEventReceiveEvent 멤버

        public event ClientReceiveEvent ReceiveEvent;                    //이벤트 선언

        /// <summary>
        /// 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="strReceiveData">이벤트시 보낼 수신한 String Data</param>
        public void SendReceiveEvent(string strReceiveData)
        {
            //Event 발생
            if (ReceiveEvent != null)
            {
                this.ReceiveEvent(strReceiveData);
            }
        }

        #endregion

        #region IEventChangeEvent 멤버

        public event ClientChangeEvent ChangeEvent;                 //이벤트 선언

        /// <summary>
        /// 이벤트를 발생시킨다.
        /// </summary>
        /// <param name="ChangeData">이벤트시 보낼 수신한 String Data</param>
        public void SendChangeEvent(string ChangeData)
        {
            //Event 발생
            if (ChangeEvent != null)
            {

                ChangeEvent(ChangeData);
            }
        }

        #endregion



    }
}
