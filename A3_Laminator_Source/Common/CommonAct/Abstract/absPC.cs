using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public abstract class absPC : absCommunication, IPC
    {

        //함수는 추상화로 각자 구현하도록 한다.
        #region IOpenClose 멤버

        public absPC PclsCommunicationType;              //참조할 dll 안에 있는 클래스 선언

        public abstract bool funOpenConnection();
        public abstract bool funCloseConnection();

        #endregion

        #region IPCCommand 멤버

        /// <summary>
        /// 데이타를 장비로 전송한다.
        /// </summary>
        /// <param name="strMessage"></param>
        public virtual void subTransfer(string strMessage)
        {
            PclsCommunicationType.subTransfer(strMessage);
        }

        public abstract void subReceive(string Message);

        #endregion



    }
}
