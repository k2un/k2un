using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EQPAct
{
    public class clsCimEvent_ProcessDataLog : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEvent_ProcessDataLog(string strUnitID)
        {
            //strUnitID = strUnitID;
            strEventName = "ProcessDataLog";
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
        /// (ProcessDataType, string strGLSID, string strJOBID, string strEQPPPID, string SetTime, string ParamName, string ParamValue)
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string dstrMode = "";
            string dstrHGLSID = "";

            string dstrLogData = "";
            string dstrBeforeLogData = "";
            string dstrCurrentLogData = "";
            DateTime dtNowTime;

            InfoAct.clsAPC clsBeforeAPCData;
            InfoAct.clsRPC clsBeforeRPCData;
            InfoAct.clsPPC clsBeforePPCData;

            InfoAct.clsAPC clsCurrentAPCData;
            InfoAct.clsRPC clsCurrentRPCData;
            InfoAct.clsPPC clsCurrentPPCData;

            try
            {
                dstrMode = parameters[1].ToString();
                dstrHGLSID = parameters[2].ToString();
                
                switch ((InfoAct.clsInfo.ProcessDataType)parameters[0])
                {
                    case InfoAct.clsInfo.ProcessDataType.APC:
                        if (dstrMode == "4")
                        {
                            clsBeforeAPCData = (InfoAct.clsAPC)parameters[3];
                            for (int dintLoop = 0; dintLoop < clsBeforeAPCData.ParameterIndex.Length; dintLoop++)
                            {
                                dstrBeforeLogData += clsBeforeAPCData.ParameterIndex[dintLoop] + "," + clsBeforeAPCData.ParameterValue[dintLoop] + ",";   // 20121210 cho young hoon
                            }
                            if (dstrBeforeLogData.EndsWith(","))
                            {
                                dstrBeforeLogData = dstrBeforeLogData.Substring(0, dstrBeforeLogData.Length - 1);
                            }
                        }
                        clsCurrentAPCData = (InfoAct.clsAPC)parameters[4];
                        dtNowTime = clsCurrentAPCData.SetTime;


                        for (int dintLoop = 0; dintLoop < clsCurrentAPCData.ParameterIndex.Length; dintLoop++)
                        {
                            dstrCurrentLogData += clsCurrentAPCData.ParameterIndex[dintLoop] + "," + clsCurrentAPCData.ParameterValue[dintLoop] + ",";   // 20121210 cho young hoon
                        }
                        if (dstrCurrentLogData.EndsWith(","))
                        {
                            dstrCurrentLogData = dstrCurrentLogData.Substring(0, dstrCurrentLogData.Length - 1);
                        }

                        dstrLogData = string.Format("{0}|{1}|{2}|{3}|{4}", dstrMode, dstrHGLSID, clsCurrentAPCData.EQPPPID, dstrBeforeLogData, dstrCurrentLogData);

                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.APC, dtNowTime, dstrLogData);
                        break;

                    case InfoAct.clsInfo.ProcessDataType.PPC:
                        switch (dstrMode)
                        {
                            case "1":
                                break;

                            case "2":
                                break;

                            case "3":
                                break;

                            case "4":
                                break;
                        }
                        break;

                    case InfoAct.clsInfo.ProcessDataType.RPC:
                       if (dstrMode == "4")
                        {
                            clsBeforeRPCData = (InfoAct.clsRPC)parameters[3];
                            dstrBeforeLogData = clsBeforeRPCData.RPC_PPID;
                        }

                        clsCurrentRPCData = (InfoAct.clsRPC)parameters[4];
                        dtNowTime = clsCurrentRPCData.SetTime;
                        dstrCurrentLogData = clsCurrentRPCData.RPC_PPID;

                        dstrLogData = string.Format("{0}|{1}|{2}|{3}", dstrMode, dstrHGLSID, dstrBeforeLogData, dstrCurrentLogData);

                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.RPC, dtNowTime, dstrLogData);
                        break;
                }
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion
    }
}
