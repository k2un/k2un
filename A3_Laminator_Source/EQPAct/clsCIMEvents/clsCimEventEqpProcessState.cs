using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventEqpProcessState : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventEqpProcessState(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "EQPProcessState";
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
        //(string strEQPProcessState, string strUnitID)
        public void funProcessCIMEvent(object[] parameters)
        {
            string strEQPProcessState = parameters[0].ToString();
            int dintUnitID = Convert.ToInt32(parameters[1].ToString());

            string dstrBitAddress = "";

            try
            {
                switch (strEQPProcessState)
                {
                    case "4":       //Pause
                        dstrBitAddress = "B1013";
                        break;

                    case "8":       //Resume
                        dstrBitAddress = "B1014";
                        break;

                    default:
                        break;
                }

                pEqpAct.funBitWrite(dstrBitAddress, "1");

                //시물레이션 Test위해서 추가
                if (pInfo.EQP("Main").DummyPLC == true)
                {
                    if (strEQPProcessState == "4")
                    {
                        pInfo.All.SimulEQPProc_Pause_CMD = true;
                    }
                    else if (strEQPProcessState == "8")
                    {
                        pInfo.All.SimulEQPProc_Resume_CMD = true;
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
