using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventBuzzerOn : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventBuzzerOn(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "BuzzerOn";
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
            string dstrBitAddress = "B1000";

            try
            {
                pEqpAct.funBitWrite(dstrBitAddress, "1");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
