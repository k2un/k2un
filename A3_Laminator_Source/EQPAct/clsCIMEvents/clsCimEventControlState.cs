using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventControlState : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventControlState(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "ControlState";
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
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string strControlState = parameters[0].ToString();

            string dstrBitAddress_Offline = "B10F0";
            //string dstrBitAddress_Local = "B10F1";
            string dstrBitAddress_Remote = "B10F2";

            try
            {
                switch (strControlState)
                {
                    case "1":                       //Offline
                        pEqpAct.funBitWrite(dstrBitAddress_Offline, "1");
                        pEqpAct.funBitWrite(dstrBitAddress_Remote, "0");
                        break;

                    case "3":                       //Online Remote
                        pEqpAct.funBitWrite(dstrBitAddress_Offline, "0");
                        pEqpAct.funBitWrite(dstrBitAddress_Remote, "1");
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
