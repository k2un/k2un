using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventGlassExist : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventGlassExist(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actGLSExist";
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
            int intUnitID = Convert.ToInt32(parameters[3]);
            int intValue = Convert.ToInt32(parameters[4]);

            Boolean dbolGLSExist = false;

            try
            {
                //if (intUnitID == 0)
                {
                    if (intValue == 1)
                    {
                       dbolGLSExist = true;
                    }
                    else
                    {
                        dbolGLSExist = false;

                        //현재 장비에서 GLS가 없음을 저장(S1F6(SFCD=3) 보고시 사용)
                        pInfo.Unit(3).SubUnit(intUnitID).RemoveCurrGLS();
                    }

                    pInfo.Unit(3).SubUnit(intUnitID).GLSExist = dbolGLSExist;
                }

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
