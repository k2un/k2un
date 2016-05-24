using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EQPAct
{
    public class clsCimEventProcessStartReqPPC : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventProcessStartReqPPC(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "PPCStart";
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
            try
            {
                //string dstrStartBitAddress = pInfo.pPLCAddressInfo.bCIM_PPCStartReq;

                ////PPC Word 정보 Write

                //pEqpAct.funBitWrite(dstrStartBitAddress, "1");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
