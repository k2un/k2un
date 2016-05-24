using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEvent_RPCEnd : clsEQPEvent, IEQPEvent
    {
        #region Variables
        InfoAct.clsInfo pInfo = InfoAct.clsInfo.Instance;

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEvent_RPCEnd(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "actRPCEND";
        }
        #endregion


        #region Methods
        /// <summary>
        /// 설비에서 CIM으로 발생하는 Event에 대한 처리
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
        public void funProcessEQPEvent(string[] parameters)
        {
            string dstrHGLSID = "";
            try
            {
                //dstrHGLSID = Convert.ToString(parameters[0]);
                dstrHGLSID = m_pEqpAct.funWordRead("", 8, EnuEQP.PLCRWType.ASCII_Data);

                this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F133RPCEnd, dstrHGLSID);
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
