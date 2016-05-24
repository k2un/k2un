using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventOnePPIDRead : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventOnePPIDRead(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "OnePPIDRead";
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
        /// (int intPPIDTYPE, string dstrPPID)
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            int intPPIDTYPE = Convert.ToInt32(parameters[0].ToString());
            string dstrPPID = parameters[1].ToString();

            string dstrWordAddress = "";
            string dstrBitAddress = "B1036";

            try
            {
                dstrWordAddress = "W1210";
                if (intPPIDTYPE == 1)
                {
                    pEqpAct.funWordWrite(dstrWordAddress, dstrPPID, EnuEQP.PLCRWType.Int_Data);
                    dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, 11);

                    pEqpAct.funWordWrite(dstrWordAddress, "1", EnuEQP.PLCRWType.Int_Data);
                    dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, 1);
                }
                else
                {
                    //dstrPPID = FunStringH.funStringData(dstrPPID, 20);        //20자로 맞춘다.
                    //pEqpAct.funWordWrite(dstrWordAddress, dstrPPID, EnuEQP.PLCRWType.ASCII_Data);

                    //dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, 21);
                    //pEqpAct.funWordWrite(dstrWordAddress, intPPIDTYPE.ToString(), EnuEQP.PLCRWType.Int_Data);
                }

                
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("OnePPIDRead 조회!! PPID = {0}", dstrPPID));

                //시물레이션 Test시 사용
                if (pInfo.EQP("Main").DummyPLC == true)
                {
                    pInfo.All.SimulReadPPIDType = intPPIDTYPE;
                    pInfo.All.SimulReadPPID = dstrPPID;

                    pInfo.All.simulB1036_Flag = true;
                    pInfo.All.simul_EQPPPID = dstrPPID;
                    pInfo.All.simulB1036_Data = 1;
                }
                else
                {
                    pEqpAct.funBitWrite(dstrBitAddress, "1");
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
