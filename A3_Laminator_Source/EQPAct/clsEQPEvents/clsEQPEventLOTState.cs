using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventLOTState : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventLOTState(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actLOTState";
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

            string strLotState = parameters[1];
            int intUnitID = Convert.ToInt32(parameters[2]);
            int intPortID = Convert.ToInt32(parameters[3]);
            int intBitVal = Convert.ToInt32(parameters[4]);

            if (intBitVal != 1) return;

            string dstrEQPStateBackup = "";
            string dstrWordAddress = "";
            Boolean dbolFlag = false;
            //int dintTemp = 0;

            try
            {
                if (pInfo.Port(intPortID) != null)
                {
                    if (pInfo.Port(intPortID).LOTST != strLotState)
                    {
                        if (strLotState == "4")
                        {
                            if (pInfo.All.CancelCMDFLAG)
                            {
                                pInfo.Port(intPortID).LOTST = "5";
                            }
                            else if (pInfo.All.AbortCMDFlag)
                            {
                                pInfo.Port(intPortID).LOTST = "6";
                            }
                            else
                            {
                                pInfo.Port(intPortID).LOTST = "4";
                            }
                        }
                        else
                        {
                            pInfo.Port(intPortID).LOTST = strLotState;
                            if (strLotState == "0")
                            {
                                pInfo.Port(intPortID).Initial();
                            }
                        }
                    }
                }
                //pInfo.subPortDataRecovery();

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
