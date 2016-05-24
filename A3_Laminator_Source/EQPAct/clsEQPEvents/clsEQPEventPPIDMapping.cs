using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Collections;

namespace EQPAct
{
    public class clsEQPEventPPIDMapping : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventPPIDMapping(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actEQPEventPPIDMapping";
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

            string[] dstrValue;
            string dstrWordData = "";

            try
            {
                m_pEqpAct.subWordReadSave("W2000", 10, EnuEQP.PLCRWType.ASCII_Data);

                dstrValue = m_pEqpAct.funWordReadAction(true);//HOST PPID를 받아옴

                if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrValue[0].Trim()) == null)
                {
                    dstrWordData = m_pEqpAct.funWordWriteString(1, "-1", EnuEQP.PLCRWType.ASCII_Data);//HOST PPID를 검색하여 등록이 되어있지 않으면 -1을 써줌
                }
                else
                {
                    if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrValue[0].Trim()).HostPPID == dstrValue[0])//HOST PPID를 검색하여 등록이 되어있을 경우 한번더 체크
                    {
                        dstrWordData = m_pEqpAct.funWordWriteString(1, pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrValue[0].Trim()).EQPPPID, EnuEQP.PLCRWType.ASCII_Data);
                    }
                    else
                    {
                        dstrWordData = m_pEqpAct.funWordWriteString(1, "-1", EnuEQP.PLCRWType.ASCII_Data);
                    }
                }

                m_pEqpAct.funWordWrite("W100F", dstrWordData, EnuEQP.PLCRWType.Hex_Data);

                m_pEqpAct.subSetConfirmBit(strCompBit);

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
