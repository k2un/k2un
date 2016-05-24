using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS1F13_TOHost : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS1F13_TOHost Instance = new clsS1F13_TOHost();
        #endregion

        #region Constructors
        public clsS1F13_TOHost()
        {
            this.IntStream = 1;
            this.IntFunction = 13;
            this.StrPrimaryMsgName = "S1F13_TO_HOST";
            this.StrSecondaryMsgName = "S1F14_TO_HOST";
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
                //arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("MDLN").Putvalue(pInfo.All.MDLN);
                pMsgTran.Primary().Item("SOFTREV").Putvalue(pInfo.All.SoftVersion);

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
                pInfo.Unit(0).SubUnit(0).CMST = 0; //Enable
                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);

            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
