using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS1F13_TOEQP : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS1F13_TOEQP Instance = new clsS1F13_TOEQP();
        #endregion

        #region Constructors
        public clsS1F13_TOEQP()
        {
            this.IntStream = 1;
            this.IntFunction = 13;
            this.StrPrimaryMsgName = "S1F13_TO_EQP";
            this.StrSecondaryMsgName = "S1F14_TO_EQP";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintACK = 0;
            int dintCEID = 0;
            try
            {
                msgTran.Secondary2("S1F14_TO_EQP").Item("COMMACK").Putvalue("0");
                msgTran.Secondary2("S1F14_TO_EQP").Item("MDLN").Putvalue(pInfo.All.MDLN);
                msgTran.Secondary2("S1F14_TO_EQP").Item("SOFTREV").Putvalue(pInfo.All.SoftVersion);

                funSendReply2(msgTran, "S1F14_TO_EQP");
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

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

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
