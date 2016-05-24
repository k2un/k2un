using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Collections;

namespace EQPAct
{
    public class clsCimEventUnloadReqCMD : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventUnloadReqCMD(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "UnloadReqCMD";
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
            string dstrBitAddress = "B0014";
            string dstrWordAddress = "W0180";
            string dstrLOTID = "";
            string dstrGLSID = "";
            int dintPortID = 0;

            try
            {
                dintPortID = Convert.ToInt32(parameters[0].ToString());
                pEqpAct.funWordWrite(dstrWordAddress, pInfo.Port(dintPortID).HostReportPortID, EnuEQP.PLCRWType.ASCII_Data);
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

