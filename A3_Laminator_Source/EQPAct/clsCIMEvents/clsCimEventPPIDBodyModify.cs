using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventPPIDBodyModify : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventPPIDBodyModify(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "PPIDBodyModify";
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
        /// (string strHOSTPPID, string strEQPPPID, string dstrPPIDBody, string dstrPPIDType)
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string strHOSTPPID = parameters[0].ToString();
            string dstrEQPPPID = parameters[1].ToString();
            string dstrPPIDBody = parameters[2].ToString();
            string dstrPPIDType = parameters[3].ToString();

            string dstrWordData = "";
            string dstrValue = " ";
            string[] dstrData = null;
            string dstrHOSTPPID = FunStringH.funStringData("", 20);       //HOST PPID는 항상 공백으로 써줌(필요없음)
            int dintPPIDType = 1;       //EQP PPID(1)
            string strWordAddress = "W1210";
            string dstrBitAddress = "";
            string dstrTemp = "";
            try
            {
                string[] arrCon = dstrPPIDBody.Split(';');
                dstrWordData += pEqpAct.funWordWriteString(1, dstrEQPPPID, EnuEQP.PLCRWType.Int_Data);
                dstrWordData += pEqpAct.funWordWriteString(10, "".PadRight(20), EnuEQP.PLCRWType.Hex_Data);
                dstrWordData += pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);
                for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                {
                    if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Length == 2)
                    {
                        dstrTemp = FunStringH.funMakePLCData(FunStringH.funMakeRound(arrCon[dintLoop], this.pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Format));
                        dstrWordData += pEqpAct.funWordWriteString(2, dstrTemp, EnuEQP.PLCRWType.Int_Data);
                    }
                    else
                    {
                        dstrWordData += pEqpAct.funWordWriteString(8, arrCon[dintLoop].PadRight(16), EnuEQP.PLCRWType.ASCII_Data);
                    }
                }
                pEqpAct.funWordWrite(strWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);

                //EQP PPID Body 수정
                dstrBitAddress = "B1034";
                pEqpAct.funBitWrite(dstrBitAddress, "1");

                if (pInfo.EQP("Main").DummyPLC)
                {
                    pEqpAct.funWordWrite("W2540", dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    pEqpAct.funBitWrite("B113B", "1");
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
