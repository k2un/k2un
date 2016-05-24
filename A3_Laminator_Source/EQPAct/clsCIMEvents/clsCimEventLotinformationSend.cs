using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventLotinformationSend : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventLotinformationSend(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "LotinformationSend";
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

            string dstrBitAddress = "B1023"; ;
            string dstrWordAddress = "W1680";
            string strFlimID = "";
            int dintCount = 0;
            string strFilmCode = "";
            try
            {
                strFlimID = parameters[0].ToString();
                dintCount = Convert.ToInt32(parameters[1].ToString());
                strFilmCode = parameters[2].ToString();

                pEqpAct.funWordWrite(dstrWordAddress, strFlimID.PadRight(100, ' ') , EnuEQP.PLCRWType.ASCII_Data);
                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 50);
                pEqpAct.funWordWrite(dstrWordAddress, dintCount.ToString(), EnuEQP.PLCRWType.Int_Data);
                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 1);
                pEqpAct.funWordWrite(dstrWordAddress, strFilmCode.PadRight(16, ' '), EnuEQP.PLCRWType.ASCII_Data);

                pEqpAct.funBitWrite(dstrBitAddress, "1", false);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strMessage:" + strMessage);
            }
        }
        #endregion
    }
}
