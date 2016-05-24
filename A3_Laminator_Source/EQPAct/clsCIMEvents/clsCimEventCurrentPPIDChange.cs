using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventCurrentPPIDChange : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventCurrentPPIDChange(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "CurrentPPIDChange";
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
            string strHOSTPPID = parameters[0].ToString();

            // lsc
            string dstrBitAddress = "B1034";
            string dstrWordAddress = "W1010";
            string dstrWordData = "";

            try
            {
                strHOSTPPID = FunStringH.funStringData(strHOSTPPID, 20);        //20자로 맞춘다.
                dstrWordData += pEqpAct.funWordWriteString(10, strHOSTPPID, EnuEQP.PLCRWType.ASCII_Data);  //HOSTPPID

                pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                dstrWordData = "";   //초기화

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
