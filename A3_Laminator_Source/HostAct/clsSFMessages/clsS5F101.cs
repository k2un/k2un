using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS5F101 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS5F101 Instance = new clsS5F101();
        #endregion

        #region Constructors
        public clsS5F101()
        {
            this.IntStream = 5;
            this.IntFunction = 101;
            this.StrPrimaryMsgName = "S5F101";
            this.StrSecondaryMsgName = "S5F102";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrModuleID = "";
            int dintIndex = 0;

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                 
                
                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(0).SubUnit(0).ModuleID && dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("ALARMCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                int alarmCount = pInfo.Unit(0).SubUnit(0).CurrAlarm().Count;
                msgTran.Secondary().Item("ALARMCOUNT").Putvalue(alarmCount);

                List<int> arrCon2 = new List<int>();

                foreach (int dintAlarm in pInfo.Unit(0).SubUnit(0).CurrAlarm())
                {
                    arrCon2.Add(dintAlarm);
                }

                arrCon2.Sort();

                for (int dintLoop = 0; dintLoop < arrCon2.Count; dintLoop++)
                {
                    InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).CurrAlarm(arrCon2[dintLoop]);

                    msgTran.Secondary().Item("ALCD" + dintLoop).Putvalue(Convert.ToByte(currentAlarm.AlarmCode));
                    msgTran.Secondary().Item("ALID" + dintLoop).Putvalue(arrCon2[dintLoop]);
                    msgTran.Secondary().Item("ALTX" + dintLoop).Putvalue(currentAlarm.AlarmDesc);
                    msgTran.Secondary().Item("MODULEID" + dintLoop).Putvalue(currentAlarm.ModuleID);
                }

                //foreach (int dintAlarm in pInfo.Unit(0).SubUnit(0).CurrAlarm())
                //{
                //    dintIndex = dintIndex + 1;
                //    InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarm);

                //    msgTran.Secondary().Item("ALCD" + (dintIndex - 1)).Putvalue(Convert.ToByte(currentAlarm.AlarmCode));
                //    msgTran.Secondary().Item("ALID" + (dintIndex - 1)).Putvalue(dintAlarm);
                //    msgTran.Secondary().Item("ALTX" + (dintIndex - 1)).Putvalue(currentAlarm.AlarmDesc);
                //    msgTran.Secondary().Item("MODULEID" + (dintIndex - 1)).Putvalue(currentAlarm.ModuleID);
                //}
                //}

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
