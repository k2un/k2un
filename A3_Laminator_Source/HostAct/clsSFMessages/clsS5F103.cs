using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS5F103 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS5F103 Instance = new clsS5F103();
        #endregion

        #region Constructors
        public clsS5F103()
        {
            this.IntStream = 5;
            this.IntFunction = 103;
            this.StrPrimaryMsgName = "S5F103";
            this.StrSecondaryMsgName = "S5F104";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrUnitID = "";
            string dstrAlarmCode = "";
            try
            {
                 dstrUnitID = msgTran.Primary().Item("UNITID").Getvalue().ToString();
                 dstrAlarmCode = msgTran.Primary().Item("ALCD").Getvalue().ToString();
                 int dintUnitNo = 0;
                 ArrayList arrCon = new ArrayList();
                 for (int dintLoop = 0; dintLoop < pInfo.UnitCount; dintLoop++)
                 {
                     if (pInfo.Unit(dintLoop + 1).SubUnit(0).ReportUnitID == dstrUnitID)
                     {
                         dintUnitNo = dintLoop + 1;
                         break;
                     }
                 }
                
                 foreach( int dintAlarmID in pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm())
                 {
                     if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode == Convert.ToInt32(dstrAlarmCode))
                     {
                         arrCon.Add(dintAlarmID);
                     }
                 }

                 msgTran.Secondary().Item("UNITID").Putvalue(dstrUnitID);
                 msgTran.Secondary().Item("L2").Putvalue(arrCon.Count);
                 for (int dintLoop = 0; dintLoop < arrCon.Count; dintLoop++)
                 {
                     msgTran.Secondary().Item("ALID" + dintLoop).Putvalue(arrCon[dintLoop].ToString());
                 }

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
