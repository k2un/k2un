using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventEventBitInitial : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventEventBitInitial(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "EventBitInitialCmd";
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
            string dstrData = FunStringH.funMakeLengthStringFirst("0", 256);     //384개임

            try
            {
                //EventBit를 초기화한다.
                pEqpAct.funBitWrite(dstrBitAddress, dstrData);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
