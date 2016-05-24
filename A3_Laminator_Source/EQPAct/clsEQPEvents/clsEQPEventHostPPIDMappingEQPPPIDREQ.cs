using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventHostPPIDMappingEQPPPIDREQ : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventHostPPIDMappingEQPPPIDREQ(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actHOSTPPIDMappingEQPPPIDREQ";
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
            string strCompBit = "";
            string PLCWordAddress = "";
            string CIMWordAddress = "";
            string strHostPPID = "";
            try
            {
                strCompBit = parameters[0].ToString();
                if (parameters[1] == "1")
                {
                    PLCWordAddress = "W2520";
                    strHostPPID = m_pEqpAct.funWordRead(PLCWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();

                    CIMWordAddress = "W100E";
                    if (pInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPID) != null)
                    {
                        m_pEqpAct.funWordWrite(CIMWordAddress, pInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPID).EQPPPID, EnuEQP.PLCRWType.Int_Data);
                    }
                    else
                    {
                        m_pEqpAct.funWordWrite(CIMWordAddress, "-1", EnuEQP.PLCRWType.Int_Data);
                    }
                }
                else
                {
                    PLCWordAddress = "W2530";
                    strHostPPID = m_pEqpAct.funWordRead(PLCWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();

                    CIMWordAddress = "W100F";
                    if (pInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPID) != null)
                    {
                        m_pEqpAct.funWordWrite(CIMWordAddress, pInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPID).EQPPPID, EnuEQP.PLCRWType.Int_Data);
                    }
                    else
                    {
                        m_pEqpAct.funWordWrite(CIMWordAddress, "-1", EnuEQP.PLCRWType.Int_Data);
                    }
                }

                m_pEqpAct.funBitWrite(strCompBit, "1");
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
