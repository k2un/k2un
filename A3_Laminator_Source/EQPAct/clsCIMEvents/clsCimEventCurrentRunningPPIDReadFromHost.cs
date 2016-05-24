using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventCurrentRunningPPIDReadFromHost : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventCurrentRunningPPIDReadFromHost(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "CurrentRunningPPPIDReadFromHOST";
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
            int intPPIDType = Convert.ToInt32(parameters[0]);

            string dstrWordAddress = "";
            string[] dstrValue = null;

            try
            {
                //if (intPPIDType == 1)   //EQPPID
                //{
                //    dstrWordAddress = pInfo.pPLCAddressInfo.wEQP_CurrentEQPPPID;
                //    pEqpAct.subWordReadSave(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);             //현재 운영중인 EQP_PPID

                //    dstrValue = pEqpAct.funWordReadAction(true);

                //    pInfo.All.CurrentEQPPPID = dstrValue[0];
                //}
                //else if (intPPIDType == 2) //HOSTPPID
                //{
                //    dstrWordAddress = pInfo.pPLCAddressInfo.wEQP_CurrentHostPPID;
                //    pEqpAct.subWordReadSave(dstrWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data);          //현재 운영중인 HOST_PPID

                //    dstrValue = pEqpAct.funWordReadAction(true);

                //    pInfo.All.CurrentHOSTPPID = dstrValue[0];
                //}
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                //HOST로 부터 S7F109 혹은 S7F23이 와서 PLC로 부터 모두 읽었다고 저장, 그리고 HOST Act단에서 응답
                if (pInfo.All.isReceivedFromHOST == true)
                {
                    pInfo.All.isReceivedFromHOST = false;          //초기화
                    pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                }
            }
        }
        #endregion
    }
}
