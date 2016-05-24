using System;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using System.Text;
using Microsoft.VisualBasic;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F31 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F31 Instance = new clsS2F31();
        #endregion

        #region Constructors
        public clsS2F31()
        {
            this.IntStream = 2;
            this.IntFunction = 31;
            this.StrPrimaryMsgName = "S2F31";
            this.StrSecondaryMsgName = "S2F32";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrData = "";
            int dintReply = 0;

            try
            {
                try
                {
                    dstrData = msgTran.Primary().Item("TIME").Getvalue().ToString().Trim();

                    //여기서 만약 시간이 잘못되면 에외로 빠진다.
                    DateTime.ParseExact(dstrData.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture);
                    DateTime.ParseExact(dstrData.Substring(8, 6), "HHmmss", CultureInfo.InvariantCulture);

                    //PC에 시간 설정(여기까지 오면 정상임)
                    DateAndTime.Today = DateTime.ParseExact(dstrData.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture);
                    DateAndTime.TimeOfDay = DateTime.ParseExact(dstrData.Substring(8, 6), "HHmmss", CultureInfo.InvariantCulture);

                    dintReply = 0;      //0=OK

                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.DateandTimeSetting, dstrData);
                }
                catch
                {
                    dintReply = 1;      //1= Error, Not done
                }

                msgTran.Secondary().Item("ACKC2").Putvalue(dintReply);

                funSendReply(msgTran);
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
