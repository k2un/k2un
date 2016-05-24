using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonAct;
using System.Collections;
using InfoAct;

namespace EQPAct
{
    public class clsCimEvent_ProcessDataSet : clsCIMEvent, ICIMEvent
    {
         #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEvent_ProcessDataSet(string strUnitID)
        {
            //strUnitID = strUnitID;
            strEventName = "ProcessDataSet";
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
        /// (ProcessDataType, OverrideMode(1:생성, 4:수정), ProcessDataType에 해당하는 구조체 )
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string dstrSQL = "";
            string dstrMode = "";
            string dstrHGLSID = "";
            InfoAct.clsInfo.ProcessDataType ProcessType;

            try
            {
                ProcessType = (InfoAct.clsInfo.ProcessDataType)parameters[0];
                dstrMode = parameters[1].ToString();
                dstrHGLSID = parameters[2].ToString();

                string[] arrData = dstrHGLSID.Split('=');
                string[] arrData2;

                ArrayList arrCon_Modify = new ArrayList();
                ArrayList arrCon_Create = new ArrayList();
                ArrayList arrCon_DayOver = new ArrayList();

                for (int dintLoop = 0; dintLoop < arrData.Length; dintLoop++)
                {
                    arrData2 = arrData[dintLoop].Split('!');

                    //DB Update
                   if (pInfo.funDBUpdate(ProcessType, arrData2[1]) == false)
                    {
                        //DB Update Fail !!
                    }

                    switch (arrData2[0])
                    {
                        case "1":
                            arrCon_Create.Add(arrData2[1]);
                            break;

                        case "3":
                            arrCon_DayOver.Add(arrData2[2]);
                            break;

                        case "4":
                            arrCon_Modify.Add(arrData2[1]);
                            break;
                    }
                }


               
                //Log
                string strProcessData = "";

                //MES 보고
                switch (ProcessType)
                {
                    case InfoAct.clsInfo.ProcessDataType.APC:
                        {
                            strProcessData = "";
                            if (arrCon_Modify.Count > 0)
                            {
                                for (int dintLoop = 0; dintLoop < arrCon_Modify.Count; dintLoop++)
                                {
                                    strProcessData += arrCon_Modify[dintLoop] + "!";
                                }
                                if (string.IsNullOrEmpty(strProcessData) == false)
                                {
                                    strProcessData = strProcessData.Substring(0, strProcessData.Length - 1);
                                    pInfo.subXPCOverride_Set(InfoAct.clsInfo.SFName.S16F115APCDataCMD, strProcessData);
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F115APCDataCMD, "4", strProcessData);
                                }
                            }

                            strProcessData = "";
                            if (arrCon_Create.Count > 0)
                            {
                                for (int dintLoop = 0; dintLoop < arrCon_Create.Count; dintLoop++)
                                {
                                    strProcessData += arrCon_Create[dintLoop] + "!";
                                }
                                if (string.IsNullOrEmpty(strProcessData) == false)
                                {
                                    strProcessData = strProcessData.Substring(0, strProcessData.Length - 1);
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F115APCDataCMD, "1", strProcessData);
                                }
                            }

                            strProcessData = "";
                            if (arrCon_DayOver.Count > 0)
                            {
                                for (int dintLoop = 0; dintLoop < arrCon_DayOver.Count; dintLoop++)
                                {
                                    strProcessData += arrCon_DayOver[dintLoop] + "!";
                                }

                                if (string.IsNullOrEmpty(strProcessData) == false)
                                {
                                    strProcessData = strProcessData.Substring(0, strProcessData.Length - 1);
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F115APCDataCMD, "3", strProcessData);
                                }
                            }
                        }
                        break;

                    case InfoAct.clsInfo.ProcessDataType.PPC:
                        if (dstrMode == "4")
                        {
                            pInfo.subXPCOverride_Set(InfoAct.clsInfo.SFName.S16F15PPCDataCMD, dstrHGLSID);
                            //pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataLog, InfoAct.clsInfo.ProcessDataType.PPC, "4", parameters[3], parameters[4]);

                        }
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F15PPCDataCMD, dstrMode, dstrHGLSID);
                        //pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ProcessDataLog, InfoAct.clsInfo.ProcessDataType.PPC, "1", dstrHGLSID, parameters[3], parameters[4]);

                        break;

                    case InfoAct.clsInfo.ProcessDataType.RPC:
                        {
                            strProcessData = "";
                            if (arrCon_Modify.Count > 0)
                            {
                                for (int dintLoop = 0; dintLoop < arrCon_Modify.Count; dintLoop++)
                                {
                                    strProcessData += arrCon_Modify[dintLoop] + "!";
                                }
                                if (string.IsNullOrEmpty(strProcessData) == false)
                                {
                                    strProcessData = strProcessData.Substring(0, strProcessData.Length - 1);
                                    pInfo.subXPCOverride_Set(InfoAct.clsInfo.SFName.S16F135RPCDataCMD, strProcessData);
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F135RPCDataCMD, "4", strProcessData);
                                }
                            }

                            strProcessData = "";
                            if (arrCon_Create.Count > 0)
                            {
                                for (int dintLoop = 0; dintLoop < arrCon_Create.Count; dintLoop++)
                                {
                                    strProcessData += arrCon_Create[dintLoop] + "!";
                                }
                                if (string.IsNullOrEmpty(strProcessData) == false)
                                {
                                    strProcessData = strProcessData.Substring(0, strProcessData.Length - 1);
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F135RPCDataCMD, "1", strProcessData);
                                }
                            }

                            strProcessData = "";
                            if (arrCon_DayOver.Count > 0)
                            {
                                for (int dintLoop = 0; dintLoop < arrCon_DayOver.Count; dintLoop++)
                                {
                                    strProcessData += arrCon_DayOver[dintLoop] + "!";
                                }

                                if (string.IsNullOrEmpty(strProcessData) == false)
                                {
                                    strProcessData = strProcessData.Substring(0, strProcessData.Length - 1);
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F135RPCDataCMD, "3", strProcessData);
                                }
                            }
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
