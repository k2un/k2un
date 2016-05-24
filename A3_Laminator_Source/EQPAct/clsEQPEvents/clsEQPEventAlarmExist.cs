using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventAlarmExist : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventAlarmExist(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actAlamExist";
        }
        #endregion

        #region Methods
        /// <summary>
        /// 설비에서 CIM으로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : strCompBit
        /// parameters[1] : dstrACTVal
        /// parameters[2] : dintActFrom
        /// parameters[3] : dstrACTFromSub
        /// parameters[4] : intBitVal
        /// parameters[5] : Special Parameter
        /// </remarks>
        public void funProcessEQPEvent(string[] parameters)
        {
            int intValue = Convert.ToInt32(parameters[4]);

            Boolean dbolAlarmExist = false;

            try
            {
                if (intValue == 1)
                {
                    dbolAlarmExist = true;
                }
                else
                {
                    dbolAlarmExist = false;

                    //현재 Alarm이 발생해있고 Reset이 모두 안되었는데 PLC에서 'Alarm없음' 신호를 받으면 
                    //현재 발생한 Alarm으로 모두 Reset 보고하고 구조체에서 삭제한다.
                    foreach (int dintAlarm in pInfo.Unit(0).SubUnit(0).CurrAlarm())
                    {
                        m_pEqpAct.subACTAlarm("", "ReSet", dintAlarm);    //Alarm 해제 보고 후 구조체에서 Alarm정보를 지운다.
                    }

                    //장비전체에 Alarm이 해제되었으면 모든 Alarm 정보를 지운다.
                    pInfo.Unit(0).SubUnit(0).RemoveCurrAlarm();
                }

                pInfo.All.AlarmExist = dbolAlarmExist;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intValue:" + intValue);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
