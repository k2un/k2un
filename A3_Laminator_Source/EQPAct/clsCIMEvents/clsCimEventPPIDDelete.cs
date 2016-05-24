using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventPPIDDelete : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventPPIDDelete(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "PPIDDelete";
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
        /// (string strHOSTPPID, string strEQPPPID, string strTime, string strPPIDRev, string dstrPPIDType)
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string strEQPPPID = parameters[0].ToString();

            string dstrWordAddress = "W1210";
            string dstrWordData = "";
            string dstrBitAddress = "";
            int dintLength = 0;
            string dstrValue = " ";
            string dstrTemp = "";

            try
            {
                InfoAct.clsEQPPPID CurrentEQPPPID = pInfo.Unit(0).SubUnit(0).EQPPPID(strEQPPPID);

                dstrWordData += pEqpAct.funWordWriteString(1, CurrentEQPPPID.EQPPPID, EnuEQP.PLCRWType.Int_Data);
                dstrWordData += pEqpAct.funWordWriteString(10, "".PadRight(20), EnuEQP.PLCRWType.Hex_Data);
                dstrWordData += pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);
                for (int dintLoop = 0; dintLoop < pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                {
                    if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Length == 2)
                    {
                        if (CurrentEQPPPID.PPIDBody(dintLoop + 1) == null)
                        {
                            dstrTemp = "0";
                        }
                        else
                        {
                            dstrTemp = FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentEQPPPID.PPIDBody(dintLoop + 1).Value, this.pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Format));
                        }
                        dstrWordData += pEqpAct.funWordWriteString(2, dstrTemp, EnuEQP.PLCRWType.Int_Data);
                    }
                    else
                    {
                        if (CurrentEQPPPID.PPIDBody(dintLoop + 1) == null)
                        {
                            dstrTemp = "";
                        }
                        else
                        {
                            dstrTemp = CurrentEQPPPID.PPIDBody(dintLoop + 1).Value;
                        }
                        dstrWordData += pEqpAct.funWordWriteString(8, dstrTemp.PadRight(16), EnuEQP.PLCRWType.ASCII_Data);
                    }
                }
                pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);

                //PPID 삭제지시        
                dstrBitAddress = "B1033";
                pEqpAct.funBitWrite(dstrBitAddress, "1");

                if (pInfo.EQP("Main").DummyPLC)
                {
                    pEqpAct.funWordWrite("W2540", dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    pEqpAct.funBitWrite("B1139", "1");
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
