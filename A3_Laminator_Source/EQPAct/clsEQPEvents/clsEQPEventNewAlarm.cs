using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventAlarm : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventAlarm(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actAlarm";
        }
        #endregion

        #region Methods
        /// <summary>
        /// 설비에서 CIM으로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : strCompBit
        /// parameters[1] : dstrACTVal
        /// parameters[2] : dintActFrom             AlarmID
        /// parameters[3] : dstrACTFromSub          
        /// parameters[4] : intBitVal               Set/Reset
        /// parameters[5] : Special Parameter
        /// </remarks>
        public void funProcessEQPEvent(string[] parameters)
        {
            int dintAlarmID = -1;
            string dstrStatus = string.Empty;
            int dintUnitID = 0;

            int dintUnitID_TEMP = 0;
            int dintSubUnitID_TEMP = 0;
            bool dbolCheckFlag = false;
            string strMCCData = "";
            try
            {
                dintAlarmID = Convert.ToInt32(parameters[2]);
                dstrStatus = (parameters[4] == "1") ? "SET" : "RESET";

                if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID) == null || string.IsNullOrEmpty(pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc) || pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc == "Spare") return;

                if (dstrStatus.Equals("SET")) // 알람 발생
                {
                    pInfo.Unit(0).SubUnit(0).AddCurrAlarm(dintAlarmID);
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "S";  // Set
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 1;  // Set

                    pInfo.Unit(3).SubUnit(0).AddCurrAlarm(dintAlarmID);
                    pInfo.Unit(3).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "S";  // Set
                    pInfo.Unit(3).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                    pInfo.Unit(3).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;
                    pInfo.Unit(3).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 1;  // Set

                    //pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).ModuleID = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).ModuleID;

                    //Heavy Alarm이 발생하면 Alarm정보를 누적 저장한다.
                    if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                    {
                        pInfo.All.OccurHeavyAlarmID = dintAlarmID;

                        //발생한 모든 Alarm을 연속해서 저장한다.
                        if (pInfo.Unit(0).SetAlarmID == "") pInfo.Unit(0).SetAlarmID = dintAlarmID.ToString();
                        else pInfo.Unit(0).SetAlarmID = pInfo.Unit(0).SetAlarmID + "," + dintAlarmID.ToString();
                    }

                    //int.TryParse(pInfo.ModuleIDToUnitID(pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).ModuleID), out dintUnitID);

                    for (int dintLoop = 0; dintLoop <= pInfo.UnitCount; dintLoop++)
                    {
                        for (int dintLoop2 = 0; dintLoop2 < pInfo.Unit(dintLoop).SubUnitCount; dintLoop2++)
                        {
                            if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).ModuleID == pInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID)
                            {
                                dintUnitID_TEMP = dintLoop;
                                dintSubUnitID_TEMP = dintLoop2;
                                dbolCheckFlag = true;
                                break;
                            }
                        }
                        if (dbolCheckFlag)
                        {
                            break;
                        }
                    }
                    
                    if (dintUnitID_TEMP != 0)
                    {

                        pInfo.Unit(dintUnitID_TEMP).SubUnit(0).AddCurrAlarm(dintAlarmID);
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "S";  // Set
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 1;  // Set

                        pInfo.Unit(dintUnitID_TEMP).SubUnit(dintSubUnitID_TEMP).AddCurrAlarm(dintAlarmID);
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(dintSubUnitID_TEMP).CurrAlarm(dintAlarmID).AlarmEventType = "S";  // Set
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(dintSubUnitID_TEMP).CurrAlarm(dintAlarmID).AlarmType = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(dintSubUnitID_TEMP).CurrAlarm(dintAlarmID).AlarmReport = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(dintSubUnitID_TEMP).CurrAlarm(dintAlarmID).SETCODE = 1;  // Set
                        if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                        {
                            pInfo.Unit(dintUnitID_TEMP).SubUnit(0).AlarmID = dintAlarmID;
                            pInfo.Unit(dintUnitID_TEMP).SubUnit(dintSubUnitID_TEMP).AlarmID = dintAlarmID;
                        }


                    }
                    
                }
                else // 알람 해제
                {

                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "R";  // ReSet
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 0;  // Set

                    //Heavy Alarm이 해제되면 Alarm정보를 누적 저장한다.
                    if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                    {
                        pInfo.All.ClearHeavyAlarmID = dintAlarmID;
                    }

                    for (int dintLoop = 0; dintLoop <= pInfo.UnitCount; dintLoop++)
                    {
                        for (int dintLoop2 = 0; dintLoop2 < pInfo.Unit(dintLoop).SubUnitCount; dintLoop2++)
                        {
                            if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).ModuleID == pInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID)
                            {
                                dintUnitID_TEMP = dintLoop;
                                dintSubUnitID_TEMP = dintLoop2;
                                dbolCheckFlag = true;
                                break;
                            }
                        }
                        if (dbolCheckFlag)
                        {
                            break;
                        }
                    }


                    if (dintUnitID != 0)
                    {
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(0).RemoveCurrAlarm(dintAlarmID);
                        pInfo.Unit(dintUnitID_TEMP).SubUnit(dintSubUnitID_TEMP).RemoveCurrAlarm(dintAlarmID);

                        if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                        {
                            pInfo.Unit(dintUnitID_TEMP).SubUnit(0).AlarmID = 0;
                            pInfo.Unit(dintUnitID_TEMP).SubUnit(dintSubUnitID_TEMP).AlarmID = 0;
                        }
                    }
                }

                InfoAct.clsAlarm CurrentAlram = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID);


                //현재 발생 혹은 해제한 AlarmID를 가지고 기준정보에서 Alarm 정보를 가져온다.
                string dstrAlarmTime = DateTime.Now.ToString("yyyyMMddHHmmss");    // Alarm 발생, 해제시간 설정

                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = CurrentAlram.AlarmType;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmCode = CurrentAlram.AlarmCode;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmDesc = CurrentAlram.AlarmDesc;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmOCCTime = dstrAlarmTime;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).ModuleID = CurrentAlram.ModuleID;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = CurrentAlram.AlarmReport;

                //S5F1 Alarm Host 보고
                if (pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport == true)
                {
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintAlarmID);
                }

                if (dstrStatus == "RESET")        // Alarm Reset 이면 발생알람을 구조체에서 삭제한다.
                {
                    pInfo.Unit(0).SubUnit(0).RemoveCurrAlarm(dintAlarmID);
                }

                // Alarm 로그 Write
                string dstrAlarmMsg = dstrAlarmTime + "," + CurrentAlram.ModuleID + "," + dstrStatus + "," + dintAlarmID.ToString() + "," + CurrentAlram.AlarmCode + "," + CurrentAlram.AlarmType + "," + CurrentAlram.AlarmDesc;
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);

                if (dstrStatus == "SET")
                {
                    //알람발생시 장비내 Glass정보 저장
                    //subGLSPosLogWrite(dstrAlarmTime, pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).UnitID, dintAlarmID);
                }

                // Alarm Message를 MainView ComboBox에 추가 (Message에 발생시간은 제외 Combo에 시간이 이중으로 Display 되어서)
                dstrAlarmMsg = CurrentAlram.ModuleID + "," + dstrStatus + "," + dintAlarmID.ToString() + "," + CurrentAlram.AlarmCode + "," + CurrentAlram.AlarmType + "," + CurrentAlram.AlarmDesc;
                pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.AlarmMsg, dstrAlarmMsg);

                int dintSubUnitID = 0;
                bool CheckFlag = false;
                for (int dintLoop = 0; dintLoop <= pInfo.UnitCount; dintLoop++)
                {
                    for (int dintLoop2 = 0; dintLoop2 <= pInfo.Unit(dintLoop).SubUnitCount; dintLoop2++)
                    {
                        if(pInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID == CurrentAlram.ModuleID)
                        {
                            dintUnitID = dintLoop;
                            dintSubUnitID = dintLoop2;
                            CheckFlag = true;
                            break;
                        }
                    }
                    if (CheckFlag) break;
                }
                //[2015/04/26]MCC Log(Add by HS)
                if (this.pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                {
                    strMCCData = "ERROR;";
                }
                else
                {
                    strMCCData = "WARNING;";
                }

                InfoAct.clsGLS CurrentGLS = pInfo.GLSID(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).HGLSID);

                strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID.Substring(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID.Length - 4, 4) + ",";

                if (CurrentGLS != null)
                {
                    strMCCData += CurrentGLS.STEPID + ",";
                    strMCCData += CurrentGLS.H_PANELID + ",";
                    strMCCData += CurrentGLS.LOTID + ",";
                    strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ",";
                }
                else
                {
                    strMCCData += ",,,,";
                }

                strMCCData += dstrStatus + ",";
                strMCCData += dintAlarmID.ToString() + ",";
                strMCCData += pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc + ";";

                pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);

                //if (dstrStatus == "SET")
                //{
                    //strMCCData = ",";
                    //strMCCData += DateTime.Now.ToString("MMdd_HHmm_ss.fff") + ",";
                    //strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID.Substring(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID.Length - 4, 4) + ",";
                    //strMCCData +=( (CurrentAlram.AlarmType == "H")? "E":"W")  + ",";
                    //if (pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).GLSExist)
                    //{
                    //    InfoAct.clsGLS CurrentGLS = pInfo.GLSID(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).HGLSID);
                    //    strMCCData += CurrentGLS.STEPID + ",";
                    //    strMCCData += CurrentGLS.H_PANELID + ",";
                    //    strMCCData += CurrentGLS.LOTID + ",";
                    //}
                    //else
                    //{
                    //    strMCCData += ",,,";
                    //}
                    //strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ",";
                    //strMCCData += string.Format("SET={0}={1}", dintAlarmID, pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc);

                    //pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                //}
                //else
                //{
                    //strMCCData = ",";
                    //strMCCData += DateTime.Now.ToString("MMdd_HHmm_ss.fff") + ",";
                    //strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID.Substring(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID.Length - 4, 4) + ",";
                    //strMCCData += ((CurrentAlram.AlarmType == "H")? "E":"W") + ",";
                    //if (pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).GLSExist)
                    //{
                    //    InfoAct.clsGLS CurrentGLS = pInfo.GLSID(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).HGLSID);
                    //    strMCCData += CurrentGLS.STEPID + ",";
                    //    strMCCData += CurrentGLS.H_PANELID + ",";
                    //    strMCCData += CurrentGLS.LOTID + ",";
                    //}
                    //else
                    //{
                    //    strMCCData += ",,,";
                    //}
                    //strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ",";
                    //strMCCData += string.Format("RESET={0}={1}", dintAlarmID, pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc);
                    //pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                //}


            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strStatus:" + dstrStatus + ", dintAlarmID:" + dintAlarmID);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }

        /// <summary>
        /// 알람이 발생할당시에 해당장비내에 글래스위치정보를 남긴다.
        /// </summary>
        /// <param name="strLogWriteTime"></param>
        /// <param name="intUnitID"></param>
        /// <param name="intAlarmID"></param>
        private void subGLSPosLogWrite(string strLogWriteTime, int intUnitID, int intAlarmID)
        {
            string dstrHGLSID = "";
            string dstrLogdata = "";

            string dstrLOTID = "";
            int dintSlotID = 0;
            string dstrPPID = "";
            try
            {
                dstrLogdata += strLogWriteTime + "," + intUnitID + "," + intAlarmID + ",";

                for (int dintLoop = 1; dintLoop <= pInfo.UnitCount; dintLoop++)
                {
                    dstrHGLSID = pInfo.Unit(dintLoop).SubUnit(0).HGLSID;

                    if (dstrHGLSID == "")
                    {
                        dstrLogdata += "///,";
                    }
                    else
                    {
                        dstrLOTID = pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).LOTID;
                        dintSlotID = pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).SlotID;
                        dstrPPID = pInfo.LOTID(dstrLOTID).Slot(dintSlotID).HOSTPPID;

                        dstrLogdata += pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).H_PANELID + "/"
                                    + dstrLOTID + "/"
                                    + dintSlotID + "/"
                                    + dstrPPID + ",";
                    }
                }

                //마지막의 콤마는 제거
                dstrLogdata = dstrLogdata.Remove(dstrLogdata.Length - 1);

                pInfo.subLog_Set(InfoAct.clsInfo.LogType.AlarmGLSInfo, dstrLogdata);

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}

