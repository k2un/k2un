using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS5F5 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS5F5 Instance = new clsS5F5();
        #endregion

        #region Constructors
        public clsS5F5()
        {
            this.IntStream = 5;
            this.IntFunction = 5;
            this.StrPrimaryMsgName = "S5F5";
            this.StrSecondaryMsgName = "S5F6";

        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintAlarmCount = 0;
            int dintIndex = 0;
            int dintTemp = 0;
            string dstrModuleID = "";

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(0).SubUnit(0).ModuleID && dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("ALARMCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);

                dintAlarmCount = Convert.ToInt32(msgTran.Primary().Item("ALARMCOUNT").Getvalue());

                if (dintAlarmCount == 0)
                {
                    msgTran.Secondary().Item("ALARMCOUNT").Putvalue(pInfo.Unit(0).SubUnit(0).AlarmCount);
                    //DateTime testTime = DateTime.Now;   //test
                    foreach (int dintAlarmID in pInfo.Unit(0).SubUnit(0).Alarm())
                    {
                        InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID);

                        if (currentAlarm.AlarmReport)
                        {
                            msgTran.Secondary().Item2("ALARMCOUNT", "L2", "ALCD", dintIndex).Putvalue(currentAlarm.AlarmCode);   // 20130314 NSECS 버전업에 따른 변경
                            msgTran.Secondary().Item2("ALARMCOUNT", "L2", "ALID", dintIndex).Putvalue(dintAlarmID);
                            msgTran.Secondary().Item2("ALARMCOUNT", "L2", "ALTX", dintIndex).Putvalue(currentAlarm.AlarmDesc);
                            msgTran.Secondary().Item2("ALARMCOUNT", "L2", "MODULEID1", dintIndex).Putvalue(currentAlarm.ModuleID);

                            dintIndex++;
                        }
                    }
                }
                else
                {
                    for (int dintLoop = 1; dintLoop <= dintAlarmCount; dintLoop++)
                    {
                        dintTemp = Convert.ToInt32(msgTran.Primary().Item("ALID" + (dintLoop - 1)).Getvalue());
                        if (pInfo.Unit(0).SubUnit(0).Alarm(dintTemp) == null || pInfo.Unit(0).SubUnit(0).Alarm(dintTemp).AlarmReport==false)
                        {
                            msgTran.Secondary().Item("ALARMCOUNT").Putvalue(0);
                            funSendReply(msgTran);
                            return;
                        }
                    }
                    
                    msgTran.Secondary().Item("ALARMCOUNT").Putvalue(dintAlarmCount);

                    for (int dintLoop = 1; dintLoop <= dintAlarmCount; dintLoop++)
                    {

                        dintTemp = Convert.ToInt32(msgTran.Primary().Item("ALID" + (dintLoop - 1)).Getvalue());
                        InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).Alarm(dintTemp);

                        msgTran.Secondary().Item2("ALARMCOUNT", "L2", "ALCD", dintIndex).Putvalue(currentAlarm.AlarmCode);
                        msgTran.Secondary().Item2("ALARMCOUNT", "L2", "ALID", dintIndex).Putvalue(dintTemp);
                        msgTran.Secondary().Item2("ALARMCOUNT", "L2", "ALTX", dintIndex).Putvalue(currentAlarm.AlarmDesc);
                        msgTran.Secondary().Item2("ALARMCOUNT", "L2", "MODULEID1", dintIndex).Putvalue(currentAlarm.ModuleID);
                        dintIndex++;
                    }
                }

                //HOST로 응답
                funSendReply(msgTran);
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());

                funSendReply(msgTran);

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
