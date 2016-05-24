using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS5F1 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS5F1 Instance = new clsS5F1();
        #endregion

        #region Constructors
        public clsS5F1()
        {
            this.IntStream = 5;
            this.IntFunction = 1;
            this.StrPrimaryMsgName = "S5F1";
            this.StrSecondaryMsgName = "S5F2";
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

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                int dintAlarmID = Convert.ToInt32(arrayEvent[1]);

                InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID);

                pMsgTran.Primary().Item("SETCODE").Putvalue(currentAlarm.SETCODE);
                pMsgTran.Primary().Item("ALCD").Putvalue(currentAlarm.AlarmCode);
                pMsgTran.Primary().Item("ALID").Putvalue(dintAlarmID);
                pMsgTran.Primary().Item("ALTX").Putvalue(currentAlarm.AlarmDesc);
                pMsgTran.Primary().Item("MODULEID").Putvalue(currentAlarm.ModuleID);

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
