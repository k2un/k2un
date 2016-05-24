using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using NSECS;

/// <summary>
/// 사용해야 할 StreamFunction Class를 생성하여 StreamFunction List로 통합하여 관리한다.
/// </summary>

namespace HostAct
{
    public class clsSFManager
    {
        #region Fields
        private List<clsStreamFunction> listSF = new List<clsStreamFunction>();
        private clsHostAct pHostAct;

        private object objLockPrimarySend = new object();
        private object objLockPrimaryRecv = new object();
        private object objSecondaryRecv = new object();
        #endregion

        #region Properties
        public List<clsStreamFunction> ListSF
        {
            get
            {
                return listSF;
            }
            set
            {
                listSF = value;
            }
        }
        #endregion

        #region Singleton Define
        /// <summary>
        /// clsHsmsMessage는 Singleton 객체로 생성한다.
        /// </summary>
        public static clsSFManager Instance = new clsSFManager();
        #endregion

        #region Constructors
        public clsSFManager()
        {
            Type[] typelist = Assembly.GetAssembly(typeof(clsHostAct)).GetTypes();
            for (int i = 0; i < typelist.Length; i++)
            {
                // clsStreamFunction을 상속받은 경우만 Reflect에서 Instance 개체를 가져와서 listSF에 저장한다.
                if (typelist[i].BaseType == Type.GetType("HostAct.clsStreamFunction"))
                {
                    clsStreamFunction tempStreamFunction = (clsStreamFunction)typelist[i].GetField("Instance").GetValue(0);
                    listSF.Add(tempStreamFunction);
                }
            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// clsHsmsMessage 생성 후 clsHostAct의 Reference를 받아 각 StreamFunction Class에 Reference 전달.
        /// </summary>
        /// <param name="host">clsHostAct의 Reference</param>
        public void funSetHostAct(clsHostAct host)
        {
            try
            {
                pHostAct = host;

                foreach (clsStreamFunction sf in listSF)
                {
                    sf.funSetHostAct(host);
                }
            }
            catch (Exception error)
            {
                if (pHostAct != null)
                {
                    pHostAct.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                }
            }
        }

        /// <summary>
        /// Primary Message의 Transaction을 받아 해당 Message와 같은 Stream Function의 class를 List에서 검색하여
        /// 각 Stream Function Class의 funPrimaryRecv Method를 호출한다.
        /// </summary>
        /// <param name="msgTran">수신한 Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            lock (objLockPrimaryRecv)
            {
                try
                {
                    foreach (clsStreamFunction sf in listSF)
                    {
                        if ((msgTran.Primary().Stream == sf.IntStream) &&
                           (msgTran.Primary().Function == sf.IntFunction) &&
                            (msgTran.Primary().MsgName == sf.StrPrimaryMsgName)
                            )
                        {
                            ((IStreamFunction)sf).funPrimaryReceive(msgTran);
                            return;
                        }
                    }
                }
                catch (Exception error)
                {
                    if (pHostAct != null)
                    {
                        pHostAct.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// HostAct의 Primary Message Biuld 요청에 따라 SFName으로 검색하여 같은 이름의 Stream Function Class의
        /// funPrimarySend Method를 호출하여 Build된 Transaction을 Return한다.
        /// </summary>
        /// <param name="strSFName">Build할 Stream Function의 이름</param>
        /// <param name="strParameters">Transaction Build시 필요한 Parameter</param>
        /// <returns>Build된 Message Transaction</returns>
        public Transaction funPrimarySend(string strSFName, string strParameters)
        {
            lock (objLockPrimarySend)
            {
                try
                {
                    foreach (clsStreamFunction sf in listSF)
                    {
                        if (sf.StrPrimaryMsgName == strSFName)
                        {
                            return ((IStreamFunction)sf).funPrimarySend(strParameters);
                        }
                    }
                }
                catch (Exception error)
                {
                    if (pHostAct != null)
                    {
                        pHostAct.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// HostAct에서 받은 Secondary Message에 대한 처리 담당.
        /// </summary>
        /// <param name="msgTran">Message Transaction</param>
        public void funSencondaryReceive(Transaction msgTran)
        {
            lock (objSecondaryRecv)
            {
                try
                {
                    foreach (clsStreamFunction sf in listSF)
                    {
                        if ((msgTran.Secondary().Stream == sf.IntStream) &&
                           (msgTran.Secondary().Function == (sf.IntFunction + 1)))
                        {
                            ((IStreamFunction)sf).funSecondaryReceive(msgTran);
                            return;
                        }
                    }
                }
                catch (Exception error)
                {
                    if (pHostAct != null)
                    {
                        pHostAct.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                    }
                }
            }
        }
        #endregion
    }
}
