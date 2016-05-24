using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEvent_APCStart : clsEQPEvent, IEQPEvent
    {
        #region Variables
        InfoAct.clsInfo pInfo = InfoAct.clsInfo.Instance;

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEvent_APCStart(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "actAPCSTART";
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
            //string dstrHGLSID = "";
            //string dstrWordAddress = "";
            //string dstrBitAddress = "";
            //try
            //{
            //    dstrHGLSID = parameters[0];
            //    dstrWordAddress = pInfo.pPLCAddressInfo.WordEQP_APCReport;
            //    dstrBitAddress = pInfo.pPLCAddressInfo.BitCIM_MCCModeChange;

            //    for (int dintLoop = 0; dintLoop < pInfo.Unit(0).SubUnit(0).EQPPPID(pInfo.APC(dstrHGLSID).EQPPPID).PPIDBodyCount; dintLoop++)
            //    {
            //        m_pEqpAct.funWordWrite(dstrWordAddress, pInfo.Unit(0).SubUnit(0).EQPPPID(pInfo.APC(dstrHGLSID).EQPPPID).PPIDBody(dintLoop).Value, EnuEQP.PLCRWType.Int_Data);
            //    }
                
            //   `
            //}
            //catch (Exception ex)
            //{
            //    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            //}
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
