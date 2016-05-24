using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventOPCallMessageClear : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventOPCallMessageClear(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actOPCallMSGClear";
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
            string strCompBit = parameters[0];

            try
            {
                pInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.OPCallClear, 0, "", "");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
