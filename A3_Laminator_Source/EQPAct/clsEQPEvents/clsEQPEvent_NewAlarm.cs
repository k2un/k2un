using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventNewAlarm : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventNewAlarm(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actNewAlarm";
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
            string strCompBit = "";
            int dintUnitNo = 0;
            int dintAlarmID = 0;
            string dstrAlarmMsg = "";
            int dintBItVal = 0;
            try
            {
                //strCompBit = parameters[0];
                dintUnitNo = Convert.ToInt32(parameters[2]);
                dintAlarmID = Convert.ToInt32(parameters[3]);
                dintBItVal = Convert.ToInt32(parameters[4]);
                if(dintUnitNo == 5)
                {
                    dintAlarmID += 6000;
                }
                else
                {
                    dintAlarmID += 8000;
                }

                DateTime dtNow = DateTime.Now;

                if (dintBItVal == 1)
                {
                    if (pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintAlarmID) == null)
                    {
                        pInfo.Unit(dintUnitNo).SubUnit(0).AddCurrAlarm(dintAlarmID);
                        pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 1;
                        pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintAlarmID).AlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode;
                        pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintAlarmID).AlarmOCCTime = dtNow.ToString("yyyy-MM-dd HH:mm:ss");
                        pInfo.Unit(0).SubUnit(0).AddCurrAlarm(dintAlarmID);
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 1;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmOCCTime = dtNow.ToString("yyyy-MM-dd HH:mm:ss");

                        if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport)
                        {
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintAlarmID, dintUnitNo, "SET");
                        }
                        else
                        {
                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Alarm Set - Alarm Report False! AlarmID = {0}", dintAlarmID));
                        }
                        dstrAlarmMsg = pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmOCCTime + "," +
                                       pInfo.Unit(dintUnitNo).SubUnit(0).ReportUnitID + "," +
                                       "SET" + "," +
                                       (dintAlarmID).ToString() + "," +
                                       pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode + "," +
                                       pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType + "," +
                                       pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc;

                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);
                        //subGLSPosLogWrite(pInfo.Unit(0).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmOCCTime, dintUnitNo, dintLoop + 1 + dintAddID);
                    }
                }
                else
                {
                    if (pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintAlarmID) != null)
                    {
                        pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 2;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).SETCODE = 2;
                        pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintAlarmID).AlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode;
                        pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarmID).AlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode;

                        dstrAlarmMsg = dtNow.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                                      pInfo.Unit(dintUnitNo).SubUnit(0).ReportUnitID + "," +
                                      "RESET" + "," +
                                      (dintAlarmID).ToString() + "," +
                                      pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmCode + "," +
                                      pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmType + "," +
                                      pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc;

                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);

                        if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmReport)
                        {
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintAlarmID, dintUnitNo, "RESET");
                        }
                        else
                        {
                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Alarm ReSet - Alarm Report False! AlarmID = {0}", dintAlarmID));
                            pInfo.Unit(0).SubUnit(0).RemoveCurrAlarm(dintAlarmID);
                            pInfo.Unit(dintUnitNo).SubUnit(0).RemoveCurrAlarm(dintAlarmID);
                        }

                    }
                }

                //m_pEqpAct.subSetConfirmBit(strCompBit);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 알람이 발생할당시에 해당장비내에 글래스위치정보를 남긴다.
        /// </summary>
        /// <param name="strLogWriteTime"></param>
        /// <param name="intUnitID"></param>
        /// <param name="intAlarmID"></param>
        private void subGLSPosLogWrite(string strLogWriteTime, int intUnitID, int intAlarmID)
        {
            string dstrGLSID = "";
            string dstrLogdata = "";

            string dstrLOTID = "";
            int dintSlotID = 0;
            string dstrPPID = "";
            try
            {
                dstrLogdata += strLogWriteTime + "," + intUnitID + "," + intAlarmID + ",";

                for (int dintLoop = 1; dintLoop <= pInfo.Unit(intUnitID).SubUnitCount; dintLoop++)
                {
                    dstrLOTID = pInfo.Unit(intUnitID).SubUnit(dintLoop).LOTID.Trim();
                    dstrGLSID = pInfo.Unit(intUnitID).SubUnit(dintLoop).GLSID.Trim();

                    if (string.IsNullOrEmpty(dstrLOTID) == false || string.IsNullOrEmpty(dstrGLSID) == false || pInfo.LOTID(dstrLOTID) == null || pInfo.LOTID(dstrLOTID).GLSID(dstrGLSID) == null)
                    {
                        dstrLogdata += "///,";
                    }
                    else
                    {
                        dstrLogdata += pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrGLSID).GLSID + "/"
                                    + dstrLOTID + "/"
                                    + pInfo.LOTID(dstrLOTID).InPortID + "/"
                                    + pInfo.Port(pInfo.LOTID(dstrLOTID).InPortID).HostPPID + ",";
                    }
                }

                //for (int dintLoop = 1; dintLoop <= pInfo.UnitCount; dintLoop++)
                //{
                //    dstrHGLSID = pInfo.Unit(dintLoop).SubUnit(0).GLSID;

                //    if (dstrHGLSID == "")
                //    {
                //        dstrLogdata += "///,";
                //    }
                //    else
                //    {
                //        dstrLOTID = pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).LOTID;
                //        dintSlotID = pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).SlotID;
                //        

                //        dstrLogdata += pInfo.Unit(dintLoop).SubUnit(0).CurrGLS(dstrHGLSID).GLSID + "/"
                //                    + dstrLOTID + "/"
                //                    + dintSlotID + "/"
                //                    + dstrPPID + ",";
                //    }
                //}

                //마지막의 콤마는 제거
                dstrLogdata = dstrLogdata.Remove(dstrLogdata.Length - 1);

                pInfo.subLog_Set(InfoAct.clsInfo.LogType.AlarmGLSInfo, dstrLogdata);

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
