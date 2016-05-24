using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoAct;

namespace EQPAct
{
    public class clsCimEvent_ProcessDataDel : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEvent_ProcessDataDel(string strUnitID)
        {
            //strUnitID = strUnitID;
            strEventName = "ProcessDataDel";
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
            string dstrSQL = "";
            string dstrMode = "";
            string dstrHGLSID = "";
            bool dbolReportFlag = false;

            InfoAct.clsAPC clsCurrentAPCData;
            InfoAct.clsRPC clsCurrentRPCData;
            InfoAct.clsPPC clsCurrentPPCData;

            string strData = "";

            try
            {
                dstrMode = parameters[1].ToString();
                dstrHGLSID = parameters[2].ToString();
                dbolReportFlag = (bool)parameters[3];
                string[] arrData;

                if (string.IsNullOrEmpty(dstrHGLSID))
                {
                    //전체삭제
                    switch ((InfoAct.clsInfo.ProcessDataType)parameters[0])
                    {
                        case InfoAct.clsInfo.ProcessDataType.APC:
                            foreach (string apcGLSID in pInfo.APC())
                            {
                                if (pInfo.funDBDelete(InfoAct.clsInfo.ProcessDataType.APC, apcGLSID) == false)
                                {
                                    //DB삭제 실패
                                }
                            }
                            break;

                        case InfoAct.clsInfo.ProcessDataType.PPC:
                            break;

                        case InfoAct.clsInfo.ProcessDataType.RPC:
                            foreach (string rpcGLSID in pInfo.RPC())
                            {
                                if (pInfo.funDBDelete(InfoAct.clsInfo.ProcessDataType.RPC, rpcGLSID) == false)
                                {
                                    //DB삭제 실패
                                }
                            }
                           
                            break;

                        default:
                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "ProcessDataType이 올바르지 않습니다.");
                            break;
                    }
                }
                else
                {
                    arrData = dstrHGLSID.Split('=');
                    switch ((InfoAct.clsInfo.ProcessDataType)parameters[0])
                    {
                        case InfoAct.clsInfo.ProcessDataType.APC:
                            for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                            {
                                string[] arrData2 = arrData[dintLoop].Split('!');
                                pInfo.funDBDelete((InfoAct.clsInfo.ProcessDataType)parameters[0], arrData2[1]);
                                strData += arrData2[1] + "!";
                            }
                            if (string.IsNullOrEmpty(strData) == false)
                            {
                                strData = strData.Substring(0, strData.Length - 1);
                            }
                            break;

                        case InfoAct.clsInfo.ProcessDataType.PPC:
                            break;

                        case InfoAct.clsInfo.ProcessDataType.RPC:
                            for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                            {
                                string[] arrData2 = arrData[dintLoop].Split('!');
                                pInfo.funDBDelete((InfoAct.clsInfo.ProcessDataType)parameters[0], arrData2[1]);
                                strData += arrData2[1] + "!";
                            }
                            if (string.IsNullOrEmpty(strData) == false)
                            {
                                strData = strData.Substring(0, strData.Length - 1);
                            }
                            break;

                        default:
                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "ProcessDataType이 올바르지 않습니다.");
                            break;
                    }
                }

                //삭제보고
                switch ((InfoAct.clsInfo.ProcessDataType)parameters[0])
                {
                    case InfoAct.clsInfo.ProcessDataType.APC:
                        if (dbolReportFlag == true)
                        {
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F115APCDataCMD, dstrMode, strData); //Deletion : 2
                        }
                        else
                        {
                            pInfo.RemoveAPC(strData);    //전체 RPC Data 삭제
                        }
                        break;

                    case InfoAct.clsInfo.ProcessDataType.PPC:
                        if (dbolReportFlag == true)
                        {
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F15PPCDataCMD, dstrMode, strData); //Deletion : 2
                        }
                        else
                        {
                            pInfo.RemovePPC();    //전체 RPC Data 삭제
                        }
                        break;

                    case InfoAct.clsInfo.ProcessDataType.RPC:
                        if (dbolReportFlag == true)
                        {
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F135RPCDataCMD, dstrMode, strData); //Deletion : 2
                        }
                        else
                        {
                            pInfo.RemoveRPC(strData);    //전체 RPC Data 삭제
                        }
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
