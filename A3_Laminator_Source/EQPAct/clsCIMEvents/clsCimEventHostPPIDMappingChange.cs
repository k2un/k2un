using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventHostPPIDMappingChange : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventHostPPIDMappingChange(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "HOSTPPIDMappingChange";
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
        /// (string strHOSTPPID, string strBeforeEQPPPID, string strAfterEQPPPID, string strTime, string strPPIDRev)
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            //dstrReceivedPPID, dstrEQPPPID
            string strHOSTPPID = parameters[0].ToString();
            //string strBeforeEQPPPID = parameters[1].ToString();
            string strAfterEQPPPID = parameters[1].ToString();
            //string strTime = parameters[3].ToString();
            //string strPPIDRev = parameters[4].ToString();

            string dstrWordData = "";
            string dstrValue = " ";
            int dintPPIDType = 2;       //HOST PPID(2)

            try
            {
                ////변경후 정보를 Write한다.
                //strHOSTPPID = FunStringH.funStringData(strHOSTPPID, 20);      //20자로 맞춘다.
                //dstrWordData += pEqpAct.funWordWriteString(10, strHOSTPPID, EnuEQP.PLCRWType.ASCII_Data);
                //dstrWordData += pEqpAct.funWordWriteString(1, strAfterEQPPPID, EnuEQP.PLCRWType.Int_Data);
                //dstrWordData += pEqpAct.funWordWriteString(1, dintPPIDType.ToString(), EnuEQP.PLCRWType.Int_Data);

                //for (int dintIndex = 1; dintIndex <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintIndex++)
                //{
                //    dstrValue = "0";      //HOST Mapping정보 변경시는 항상 0으로 써줌
                //    dstrWordData += pEqpAct.funWordWriteString(1, dstrValue, EnuEQP.PLCRWType.Int_Data);
                //}

                ////PPID값을 Word영역에 쓴다.
                //pEqpAct.funWordWrite(pInfo.pPLCAddressInfo.wCIM_PPIDCMD, dstrWordData, EnuEQP.PLCRWType.Hex_Data);

                ////HOST Mapping정보 변경 지시
                //pEqpAct.funBitWrite(pInfo.pPLCAddressInfo.bCIM_PPIDMappingChangeCMD, "1");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
