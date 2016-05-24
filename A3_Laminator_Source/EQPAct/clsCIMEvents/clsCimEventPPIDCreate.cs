using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventPPIDCreate : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventPPIDCreate(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "PPIDCreate";
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
            string dstrWordAddress = "W1210";
            string dstrWordData = "";
            string dstrBitAddress = "";
            StringBuilder dstrLog = new StringBuilder();
            string dstrTemp = "";
            try
            {
                InfoAct.clsEQPPPID CurrentEQPPPID = (InfoAct.clsEQPPPID)parameters[0];

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

                //PPID 생성지시        
                dstrBitAddress = "B1032";
                pEqpAct.funBitWrite(dstrBitAddress, "1");

                if (pInfo.EQP("Main").DummyPLC)
                {
                    pEqpAct.funWordWrite("W2540", dstrWordData, EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터
                    pEqpAct.funBitWrite("B113A", "1");
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
