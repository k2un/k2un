using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EQPAct
{
    public class clsCimEventSimulation : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventSimulation(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "Simulation";
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
            string dstrBitAddress = "";
            string dstrWordAddress = "";
            string dstrWordData = "";
            string dstrType = "";
            try
            {
                dstrBitAddress = parameters[0].ToString();
                dstrWordAddress = parameters[1].ToString();
                dstrWordData = parameters[2].ToString();
                dstrType = parameters[3].ToString();

                if (dstrWordAddress != "")
                {
                    switch (dstrType)
                    {
                        case "":
                            pEqpAct.funWordWrite(dstrWordAddress, " ".PadRight(dstrWordData.Length, ' '), CommonAct.EnuEQP.PLCRWType.Hex_Data);
                            pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, CommonAct.EnuEQP.PLCRWType.Hex_Data);
                            break;

                        case "A":
                            pEqpAct.funWordWrite(dstrWordAddress, " ".PadRight(dstrWordData.Length, ' '), CommonAct.EnuEQP.PLCRWType.Hex_Data);
                            pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                            break;

                        case "H":
                            pEqpAct.funWordWrite(dstrWordAddress, " ".PadRight(dstrWordData.Length, ' '), CommonAct.EnuEQP.PLCRWType.Hex_Data);
                            pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, CommonAct.EnuEQP.PLCRWType.Hex_Data);
                            break;

                        case "B":
                            pEqpAct.funWordWrite(dstrWordAddress, " ".PadRight(dstrWordData.Length, ' '), CommonAct.EnuEQP.PLCRWType.Hex_Data);
                            pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, CommonAct.EnuEQP.PLCRWType.Binary_Data);
                            break;

                        case "D":
                            pEqpAct.funWordWrite(dstrWordAddress, " ".PadRight(dstrWordData.Length, ' '), CommonAct.EnuEQP.PLCRWType.Hex_Data);
                            pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, CommonAct.EnuEQP.PLCRWType.Int_Data);
                            break;

                        case "D2":
                            //pEqpAct.funWordWrite(dstrWordAddress, " ".PadRight(dstrWordData.Length, ' '), CommonAct.EnuEQP.PLCRWType.Hex_Data);
                            ////pEqpAct.funDoubleWordWrite(dstrWordAddress, dstrWordData, CommonAct.EnuEQP.PLCRWType.Int_Data);
                            //pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, CommonAct.EnuEQP.PLCRWType.Int_Data);

                            //string strTest = CommonAct.FunTypeConversion.funDecimalConvert(dstrWordData, CommonAct.EnuEQP.StringType.Hex);
                            pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, CommonAct.EnuEQP.PLCRWType.Int_Data);



                            break;
                    }

                    if (dstrBitAddress != "")
                    {
                        pEqpAct.funBitWrite(dstrBitAddress, "1");
                    }
                }
                else
                {
                    pEqpAct.funBitWrite(dstrBitAddress, dstrWordData);
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
