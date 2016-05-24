using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventDateAndTimeSetting : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventDateAndTimeSetting(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "DateandTimeSetting";
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
            string dstrTime = parameters[0].ToString();

            string dstrWordAddress = "W1000";
            string dstrWordData = "";
            string dstrBitAddress = "B1002";

            try
            {
                //Word영역에 현재시간을 Write한다.
                dstrWordData += pEqpAct.funWordWriteString(1, dstrTime.Substring(0, 4), EnuEQP.PLCRWType.Int_Data);
                dstrWordData += pEqpAct.funWordWriteString(1, dstrTime.Substring(4, 2), EnuEQP.PLCRWType.Int_Data);
                dstrWordData += pEqpAct.funWordWriteString(1, dstrTime.Substring(6, 2), EnuEQP.PLCRWType.Int_Data);
                dstrWordData += pEqpAct.funWordWriteString(1, dstrTime.Substring(8, 2), EnuEQP.PLCRWType.Int_Data);
                dstrWordData += pEqpAct.funWordWriteString(1, dstrTime.Substring(10, 2), EnuEQP.PLCRWType.Int_Data);
                dstrWordData += pEqpAct.funWordWriteString(1, dstrTime.Substring(12, 2), EnuEQP.PLCRWType.Int_Data);

                pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);        //Date and Time

                //시간 갱신 지시를 내린다.                                 
                pEqpAct.funBitWrite(dstrBitAddress, "1");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
