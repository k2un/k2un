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
        /// parameters[2] : dintActFrom
        /// parameters[3] : dstrACTFromSub
        /// parameters[4] : intBitVal
        /// parameters[5] : Special Parameter
        /// </remarks>
        public void funProcessEQPEvent(string[] parameters)
        {
            string strCompBit = parameters[0];
            string strStatus = parameters[1];
            int dintResetAlarm = Convert.ToInt32(parameters[5]);

            int dintAlarmID = 0;
            string dstrModuleID = "";
            string dstrWordAddress = "";
            string dstrAlarmType = "";
            int dintAlarmCode = 0;
            string dstrAlarmDesc = "";
            Boolean dbolAlarmReport = false;
            string dstrAlarmTime = "";   //yyyy-MM-dd HH:mm:ss
            string dstrAlarmMsg = "";
            //int dintHeavyAlarmCount = 0;

            //발생한 모든 알람을 저장할 변수
            string[] dAlarmSplit;           //발생한 모든 알람을 저장하는 이전 변수
            string dNowAlarm = "";          //한개의 알람이 해제 된 후 남아있는 알람을 저장할 변수

            string dstrStepID = "";
            string dstrGLSID = "";
            string dstrLOTID = "";
            string dstrPPID = "";
            int dintUnitID = 0;
            int dintSlotID = 0;
            string dstrValue = "";
            string dstrMCCType = "";

            try
            {
                if (strStatus == "Set") // Alarm 발생
                {
                    //현재 Unit에서 Alarm이 발생했을때 AlarmID를 읽어온다.
                    //dstrWordAddress = pInfo.pPLCAddressInfo.wEQP_AlarmSetCode;
                    dintAlarmID = Convert.ToInt32(m_pEqpAct.funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data));

                    //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                    m_pEqpAct.subSetConfirmBit(strCompBit);

                    //Alarm이 등록이 안되있으면 로그를 출력하고 빠져나간다.
                    if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID) == null)
                    {
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subACTAlarm: AlarmID not exist, AlarmID:" + dintAlarmID);
                        return;
                    }

                    pInfo.Unit(0).SubUnit(0).AddCurrAlarm(dintAlarmID);
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "S";  // Set
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 1;  // Set

                    //Heavy Alarm이 발생하면 Alarm정보를 누적 저장한다.
                    if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                    {
                        pInfo.All.OccurHeavyAlarmID = dintAlarmID;

                        //발생한 모든 Alarm을 연속해서 저장한다.
                        if (pInfo.Unit(0).SetAlarmID == "") pInfo.Unit(0).SetAlarmID = dintAlarmID.ToString();
                        else pInfo.Unit(0).SetAlarmID = pInfo.Unit(0).SetAlarmID + "," + dintAlarmID.ToString();
                    }

                }
                else                    // Alarm 해제
                {
                    if (dintResetAlarm == 0) //실제 설비에서 Alarm 해제 보고가 들어올때는 Alarm ID를 PLC에서 Read한다.
                    {
                        //현재 Unit에서 Alarm이 발생했을때 AlarmID를 읽어온다.
                        //dstrWordAddress = pInfo.pPLCAddressInfo.wEQP_AlarmResetCode;
                        dintAlarmID = Convert.ToInt32(m_pEqpAct.funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data));

                        //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                        m_pEqpAct.subSetConfirmBit(strCompBit);
                    }
                    else  //현재 Alarm이 발생해있고 Reset이 모두 안되었는데 PLC에서 'Alarm없음' 신호를 받으면 AlarmID를 현재 저장하고 있는 것으로 한다.  
                    {
                        dintAlarmID = dintResetAlarm;
                    }

                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmEventType = "R";  // ReSet
                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 0;  // Set


                    //Heavy Alarm이 해제되면 Alarm정보를 누적 저장한다.
                    if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                    {
                        pInfo.All.ClearHeavyAlarmID = dintAlarmID;
                    }
                }

                //현재 발생 혹은 해제한 AlarmID를 가지고 기준정보에서 Alarm 정보를 가져온다.
                dstrAlarmType = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType;
                dintAlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode;
                dstrAlarmDesc = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc;
                dbolAlarmReport = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport;
                dstrAlarmTime = DateTime.Now.ToString("yyyyMMddHHmmss");    // Alarm 발생, 해제시간 설정
                dstrModuleID = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).ModuleID;

                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmType = dstrAlarmType;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmCode = dintAlarmCode;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmDesc = dstrAlarmDesc;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmOCCTime = dstrAlarmTime;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).ModuleID = dstrModuleID;
                pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport = dbolAlarmReport;

                //S5F1 Alarm Host 보고
                if (pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmReport == true)
                {
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintAlarmID);
                }

                // Alarm 로그 Write
                dstrAlarmMsg = dstrAlarmTime + "," + dstrModuleID + "," + strStatus + "," + dintAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + dstrAlarmDesc;
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);

                if (strStatus == "Set")
                {
                    //알람발생시 장비내 Glass정보 저장
                    subGLSPosLogWrite(dstrAlarmTime, pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).UnitID, dintAlarmID);

                }

                // Alarm Message를 MainView ComboBox에 추가 (Message에 발생시간은 제외 Combo에 시간이 이중으로 Display 되어서)
                dstrAlarmMsg = dstrModuleID + "," + strStatus + "," + dintAlarmID.ToString() + "," + dintAlarmCode + "," + dstrAlarmType + "," + dstrAlarmDesc;
                pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.AlarmMsg, dstrAlarmMsg);

                if (strStatus == "ReSet")        // Alarm Reset 이면 발생알람을 구조체에서 삭제한다.
                {
                    if (dintResetAlarm == 0)  //PLC에서 Alarm 해제를 받을때는 여기서 삭제하고 나머지는 subACTAlamExist에서 삭제한다.
                    {
                        #region <Alarm 시나리오 변경 추가       091216  이상호>
                        if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                        {
                            dAlarmSplit = pInfo.Unit(0).SetAlarmID.Split(',');

                            //ReSet 된 Alarm은 삭제 한다.
                            for (int iArmCnt = 0; iArmCnt < dAlarmSplit.Length; iArmCnt++)
                            {
                                if (dintAlarmID.ToString() == dAlarmSplit[iArmCnt]) dAlarmSplit[iArmCnt] = "";
                            }

                            //남아있는 Alarm중 가장 먼저 발생한 AlarmID를 첫번째 Alarm으로 올린다.
                            for (int iArmCnt = 0; iArmCnt < dAlarmSplit.Length; iArmCnt++)
                            {
                                if (dAlarmSplit[iArmCnt] == "") { }
                                else
                                {
                                    if (dNowAlarm == "") dNowAlarm = dAlarmSplit[iArmCnt];
                                    else dNowAlarm = dNowAlarm + "," + dAlarmSplit[iArmCnt];
                                }
                            }

                            dAlarmSplit = dNowAlarm.Split(',');
                            pInfo.Unit(0).SetAlarmID = dNowAlarm;
                            //등록한 첫번째 알람을 EQP 상태 변화시 보고하기 위해 넣는다.

                            if (dAlarmSplit[0] == "") pInfo.All.OccurHeavyAlarmID = 0;
                            else pInfo.All.OccurHeavyAlarmID = Convert.ToInt32(dAlarmSplit[0]);
                        }
                        #endregion


                        pInfo.Unit(0).SubUnit(0).RemoveCurrAlarm(dintAlarmID);
                    }
                }

                //MCC Log 기록 111221 고석현
                dintUnitID = pInfo.funGetModuleIDToUnitID(dstrModuleID);
                dstrGLSID = pInfo.Unit(dintUnitID).SubUnit(0).HGLSID;
                if (dstrGLSID.Trim() != "")
                {
                    dstrLOTID = pInfo.Unit(dintUnitID).SubUnit(0).CurrGLS(dstrGLSID).LOTID;
                    dintSlotID = pInfo.Unit(dintUnitID).SubUnit(0).CurrGLS(dstrGLSID).SlotID;
                    dstrPPID = pInfo.LOTID(dstrLOTID).Slot(dintSlotID).HOSTPPID;
                    dstrStepID = pInfo.LOTID(dstrLOTID).Slot(dintSlotID).STEPID;
                }

                dstrValue = strStatus.ToUpper() + "=" + dintAlarmID + "=" + dstrAlarmDesc.ToUpper();


                if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType == "H")
                {
                    dstrMCCType = "E";
                }
                else
                {
                    dstrMCCType = "W";
                }

                dstrModuleID = dstrModuleID.Substring(dstrModuleID.Length - 4, 4);

                subMCCLogData(dstrModuleID, dstrMCCType, dstrStepID, dstrGLSID, dstrLOTID, dstrPPID, dstrValue);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strStatus:" + strStatus);
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

                        dstrLogdata += pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).GLSID + "/"
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
