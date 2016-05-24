using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventFILMCaseExist : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventFILMCaseExist(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actFILMCaseExist";
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
            int intUnitID = Convert.ToInt32(parameters[2]);
            int intSubUnitID = Convert.ToInt32(parameters[3]);
            int intValue = Convert.ToInt32(parameters[4]);

            Boolean dbolGLSExist = false;
            string dstrWordAddress = "";
            string dstrCaseID = "";
            int dintPortID = 0;

            try
            {
                if (intValue == 1)
                {
                    dbolGLSExist = true;
                }
                else
                {
                    dbolGLSExist = false;

                    //현재 장비에서 GLS가 없음을 저장(S1F6(SFCD=3) 보고시 사용)
                    pInfo.Unit(intUnitID).SubUnit(intSubUnitID).RemoveCurrGLS();
                }

                pInfo.Unit(intUnitID).SubUnit(intSubUnitID).FilmCaseExist = dbolGLSExist;
                if (intUnitID == 1)
                {
                    dstrWordAddress = "W2200";
                    dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 32 * (intSubUnitID - 1));
                    dstrCaseID = m_pEqpAct.funWordRead(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);
                }
                else
                {
                    dstrWordAddress = "W2280";
                    dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 32 * (intSubUnitID - 1));
                    dstrCaseID = m_pEqpAct.funWordRead(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);
                }


                if (intUnitID == 2)
                {
                    dintPortID = 4;
                }

                dintPortID += intSubUnitID;

                pInfo.Port(dintPortID).CSTID = dstrCaseID.Trim();
                pInfo.Port(dintPortID).PortState = "2";
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", intUnitID:" + intUnitID);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
