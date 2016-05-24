using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Net.NetworkInformation;
using System.IO;

namespace EQPAct
{
    public class clsCimEventMultiUseDataGet : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventMultiUseDataGet(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "GetMultiUseData";
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

            StringBuilder dstrLog = new StringBuilder();
            Ping pingSender = new Ping();
            string dstrIP = "";
            int dintTimeOut = 1;                        //Ping Check TimeOut 
            string[] arrayMultiData = { "HOST_IP", "MCC_BASIC_PATH", "MCC_SAMPLING_TIME", "LootFilePath", "MCC_PORT", "MCC_UserID", "MCC_Password" };

            try
            {
                //pInfo.All.Host_IP = FunINIMethod.funINIReadValue("MCC", arrayMultiData[0], "", this.pInfo.All.MultiUseDataFilePath).Trim();
                //pInfo.All.SAMPLING_TIME = FunINIMethod.funINIReadValue("MCC", arrayMultiData[2], "", this.pInfo.All.MultiUseDataFilePath).Trim();
                //pInfo.All.MCCNetworkUserID = FunINIMethod.funINIReadValue("MCC", arrayMultiData[5], "", this.pInfo.All.MultiUseDataFilePath).Trim();
                //pInfo.All.MCCNetworkPassword = FunINIMethod.funINIReadValue("MCC", arrayMultiData[6], "", this.pInfo.All.MultiUseDataFilePath).Trim();




            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
