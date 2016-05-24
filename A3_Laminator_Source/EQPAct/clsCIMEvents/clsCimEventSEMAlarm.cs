using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventSEMAlarm : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventSEMAlarm(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "SEMAlarm";
        }
        #endregion

        #region Methods
        /// <summary>
        /// CIM에서 설비로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : cmdName
        /// parameters[1] : 1st parameter
        /// parameters[2] : 2nd parameter
        /// parameters[3] : 3rd parameter
        /// parameters[4] : 4th parameter
        /// parameters[5] : 5th Parameter
        /// (int intAlarmID, string strAlarmDesc, string strStatus)
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            int intAlarmID = Convert.ToInt32(parameters[0].ToString());
            string strAlarmDesc = parameters[1].ToString();
            string strStatus = parameters[2].ToString();

            //int dintAlarmID = 20001;
            string dstrModuleID = pInfo.EQP("Main").EQPID;
            string dstrAlarmType = "";
            int dintAlarmCode = 0;
            //string dstrAlarmDesc = "";
            string dstrAlarmTime = "";   //yyyy-MM-dd HH:mm:ss
            string dstrAlarmMsg = "";
            //string strStatus = "ReSet";

            try
            {
                //this.plngCIMLive = DateAndTime.Now.Ticks;

                if (strStatus == "S") // Alarm 발생
                {
                    if (pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID) == null)
                    {
                        pInfo.Unit(0).SubUnit(0).AddCurrAlarm(intAlarmID);
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmEventType = "S";       // Set
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmType = "L";            //Light Alarm
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmCode = 1;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmDesc = strAlarmDesc;
                        dstrAlarmTime = DateTime.Now.ToString("yyyyMMddHHmmss");    // Alarm 발생, 해제시간 설정
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmOCCTime = dstrAlarmTime;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).ModuleID = dstrModuleID;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).SETCODE = 1;

                        dstrAlarmType = pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmType;
                        dintAlarmCode = pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmCode;
                        dstrModuleID = pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).ModuleID;

                        //S5F1 Alarm Host 보고
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, intAlarmID);

                        // Alarm 로그 Write
                        dstrAlarmMsg = dstrAlarmTime + "," + dstrModuleID + "," + strStatus + "," + intAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + strAlarmDesc;
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);

                        // Alarm Message를 MainView ComboBox에 추가 (Message에 발생시간은 제외 Combo에 시간이 이중으로 Display 되어서)
                        dstrAlarmMsg = dstrModuleID + "," + strStatus + "," + intAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + strAlarmDesc;
                        pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.AlarmMsg, dstrAlarmMsg);

                        //LAYER0 (ALL) Alarm 여부를 On한다.
                        pEqpAct.subACTAlamExist(1);

                        //SEM Controller Interface Alarm 보고를 했다는 것을 설정
                        if (intAlarmID == 1000000)
                        {
                            pInfo.All.SEMInterfaceAlarmReport = true;
                           // this.plngSEMAlarmReportTick = DateAndTime.Now.Ticks;    //2012.07.17 Youngsik... 이 부분은 어떻게???
                        }
                    }
                }
                else
                {
                    //Alarm 해제보고
                    if (pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID) != null)
                    {
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmEventType = "R";  // ReSet
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmType = "L";            //Light Alarm
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmCode = 1;
                        //m_pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmDesc = strAlarmDesc;
                        dstrAlarmTime = DateTime.Now.ToString("yyyyMMddHHmmss");    // Alarm 발생, 해제시간 설정
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmOCCTime = dstrAlarmTime;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).ModuleID = dstrModuleID;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).SETCODE = 0;

                        strAlarmDesc = pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmDesc;
                        dstrAlarmType = pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmType;
                        dintAlarmCode = pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).AlarmCode;
                        dstrModuleID = pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID).ModuleID;

                        //S5F1 Alarm Host 보고
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, intAlarmID);

                        // Alarm 로그 Write
                        dstrAlarmMsg = dstrAlarmTime + "," + dstrModuleID + "," + strStatus + "," + intAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + strAlarmDesc;
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);

                        // Alarm Message를 MainView ComboBox에 추가 (Message에 발생시간은 제외 Combo에 시간이 이중으로 Display 되어서)
                        dstrAlarmMsg = dstrModuleID + "," + strStatus + "," + intAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + strAlarmDesc;
                        pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.AlarmMsg, dstrAlarmMsg);

                        pInfo.Unit(0).SubUnit(0).RemoveCurrAlarm(intAlarmID); //Alarm 삭제

                        //LAYER0 (ALL) Alarm 여부를 Off한다.
                        pEqpAct.subACTAlamExist(0);

                        //SEM Controller Interface Alarm 해제보고를 했다는 것을 설정
                        if (intAlarmID == 1000000)
                        {
                            pInfo.All.SEMInterfaceAlarmReport = false;
                            //this.plngSEMAlarmReportTick = 0;  //2012.07.17 Youngsik... 이 부분은 어떻게???
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intAlarmID:" + intAlarmID);
            }
        }
        #endregion
    }
}
