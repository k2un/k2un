using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventEqpState : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventEqpState(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "EQPState";
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
            string strEQPState = parameters[0].ToString();

            string dstrBitAddress = "";

            try
            {
                switch (strEQPState)
                {
                    case "1":
                        dstrBitAddress = "B1010";
                        break;

                    case "3":
                        dstrBitAddress = "B1011";
                        break;

                    default:
                        break;
                }

                pEqpAct.funBitWrite(dstrBitAddress, "1");

                //시물레이션 Test위해서 추가
                if (pInfo.EQP("Main").DummyPLC == true)
                {
                    if (strEQPState == "1")
                    {
                        pInfo.All.SimulEQPNormal_CMD = true;
                    }
                    else
                    {
                        pInfo.All.SimulEQPPM_CMD = true;
                    }
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
