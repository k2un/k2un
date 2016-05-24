using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventMessageSend : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventMessageSend(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "MessageSend";
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
            string strMessage = parameters[0].ToString();

            string dstrBitAddress = "B1003"; ;
            string dstrWordAddress = "W1010";
            string strType = ""; 
            try
            {
                strType = parameters[1].ToString().Trim();
                if (strMessage.Length > 1000)     //메시지 길이 초과시 1000자로 조정
                {
                    strMessage = strMessage.Substring(0, 1000);
                }
                else
                {
                    strMessage = FunStringH.funStringData(strMessage, 1000);      //1000자로 맞춘다.
                }
                pEqpAct.funWordWrite("W1010", strMessage, EnuEQP.PLCRWType.ASCII_Data, false);

                if (strType == "O")
                {
                    pEqpAct.funBitWrite("B1003", "1", false);
                }
                else
                {
                    pEqpAct.funBitWrite("B1005", "1", false);


                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strMessage:" + strMessage);
            }
        }
        #endregion
    }
}
