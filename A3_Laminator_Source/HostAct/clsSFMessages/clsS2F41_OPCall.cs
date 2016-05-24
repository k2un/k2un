using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F41_OPCall : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F41_OPCall Instance = new clsS2F41_OPCall();
        #endregion

        #region Constructors
        public clsS2F41_OPCall()
        {
            this.IntStream = 2;
            this.IntFunction = 41;
            this.StrPrimaryMsgName = "S2F41_OPCall";
            this.StrSecondaryMsgName = "S2F42";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrRCMD = "";
            string dstrIOPCall = "";
            try
            {
                dstrRCMD = msgTran.Primary().Item("RCMD").Getvalue().ToString();
                dstrIOPCall = msgTran.Primary().Item("MESSAGE").Getvalue().ToString();


                if (dstrRCMD != "6")
                {
                    msgTran.Secondary().Item("HCACK").Putvalue(1);
                }
                else
                {
                    pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, "[OPcall] " + dstrIOPCall);

                    msgTran.Secondary().Item("HCACK").Putvalue(0);
                }
                msgTran.Secondary().Item("RCMD").Putvalue(dstrRCMD);

                pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, "[OPcall] " + dstrIOPCall);

                funSendReply(msgTran);
            }
            catch (Exception error)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
