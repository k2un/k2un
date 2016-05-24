using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Threading;

namespace EQPAct
{
    public class clsEQPEventPortUseMode : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventPortUseMode(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actPortUseMode";
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
            try
            {
                int intUnitID = Convert.ToInt32(parameters[2]);
                int intPortNo = Convert.ToInt32(parameters[3]);
                int intBitVal = Convert.ToInt32(parameters[4]);
                int intACTVal = Convert.ToInt32(parameters[1]);

                if (intBitVal != 1) return;
                pInfo.Port(intUnitID).PortUseMode = Convert.ToBoolean(intACTVal);

                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_PortStatusChange, 213, intPortNo);

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }

        #endregion
    }
}
