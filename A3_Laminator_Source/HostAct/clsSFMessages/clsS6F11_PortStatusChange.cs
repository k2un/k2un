using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_PortStatusChange : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_PortStatusChange Instance = new clsS6F11_PortStatusChange();
        #endregion

        #region Constructors
        public clsS6F11_PortStatusChange()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11_PortStatusChange";
            this.StrSecondaryMsgName = "S6F12";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            try
            {

            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }
        }

        /// <summary>
        /// Primary Message를 Biuld하여 Transaction을 Return한다.
        /// </summary>
        /// <param name="strParameters">Parameter 문자열</param>
        public Transaction funPrimarySend(string strParameters)
        {
            string[] arrayEvent;
            try
            {
                arrayEvent = strParameters.Split(',');

                int dintCEID = Convert.ToInt32(arrayEvent[1]);   //CEID
                try
                {
                    if (pInfo.CEID(dintCEID).Report == false)
                    {
                        return null;
                    }
                }
                catch (Exception)
                {

                }
                if (pInfo.All.DataID >= 9999)
                {
                    pInfo.All.DataID = -1;
                }
                pInfo.All.DataID++;
                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(pInfo.All.DataID);  //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("PTID").Putvalue(100);

                int dintPortNo = Convert.ToInt32(arrayEvent[2]);
                pMsgTran.Primary().Item("PTID").Putvalue(pInfo.Port(dintPortNo).HostReportPortID);
                pMsgTran.Primary().Item("PTTYPE").Putvalue(pInfo.Port(dintPortNo).PortType);
                pMsgTran.Primary().Item("USETYPE").Putvalue(pInfo.Port(dintPortNo).USETYPE);
                pMsgTran.Primary().Item("TRSMODE").Putvalue(pInfo.Port(dintPortNo).TRSMODE);
                pMsgTran.Primary().Item("PTATTRIBUTE").Putvalue(pInfo.Port(dintPortNo).PTATTRIBUTE);
                pMsgTran.Primary().Item("PTST").Putvalue((pInfo.Port(dintPortNo).PortUseMode) ? "5" : "4");
               
                return pMsgTran;
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                pInfo.All.DataID--;
                return null;
            }
        }

        /// <summary>
        /// Secondary Message 수신에 대해 처리한다.
        /// </summary>
        /// <param name="msgTran">Secondary Message의 Transaction</param>
        public void funSecondaryReceive(Transaction msgTran)
        {
            try
            {
               
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
