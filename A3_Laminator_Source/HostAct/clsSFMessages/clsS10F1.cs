using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS10F1 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS10F1 Instance = new clsS10F1();
        #endregion

        #region Constructors
        public clsS10F1()
        {
            this.IntStream = 10;
            this.IntFunction = 1;
            this.StrPrimaryMsgName = "S10F1";
            this.StrSecondaryMsgName = "S10F2";
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
            string strTID = "";
            string strMSG = "";
            try
            {
                arrayEvent = strParameters.Split(',');
                strTID = arrayEvent[1];
                strMSG = arrayEvent[2];
                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("TID").Putvalue(strTID);
                pMsgTran.Primary().Item("TEXT").Putvalue(strMSG);
                

                return pMsgTran;
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
