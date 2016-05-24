using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEvent_AlarmReport : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEvent_AlarmReport(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actAlarmReport";
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
            string strAddress = "";
            string strReadData = "";
            string[] arrCon = new  string[128];
            int dintAddID = 0;
            string dstrAlarmMsg = "";
            try
            {
                strCompBit = parameters[0];
                dintUnitNo = Convert.ToInt32(parameters[2]);
                char chrTemp = ' ';

                switch (dintUnitNo)
                {
                    case 1: //Index
                        strAddress = "W3200";
                        dintAddID = 0;
                        break;

                    case 2: //Cleaner
                        strAddress = "W4180";
                        dintAddID = 2000;
                        break;

                    case 3: //Sky C/V
                        strAddress = "W5180";
                        dintAddID = 4000;
                        break;

                    case 4: //Robot
                        break;

                    //case 5: //OVEN1
                    //    strAddress = "W7440";
                    //    dintAddID = 6000;
                    //    break;

                    //case 6: //OVEN2
                    //    strAddress = "W8440";
                    //    dintAddID = 8000;
                    //    break;
                }

                if (dintUnitNo != 5 && dintUnitNo != 6)
                {
                    strReadData = m_pEqpAct.funWordRead(strAddress, 128, EnuEQP.PLCRWType.Binary_Data);
                    for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                    {
                        StringBuilder sb = new StringBuilder(strReadData.Substring((dintLoop * 16), 16));
                        for (int dintLoop2 = 0; dintLoop2 < sb.Length / 2; dintLoop2++)
                        {
                            chrTemp = sb[dintLoop2];
                            sb[dintLoop2] = sb[(sb.Length - dintLoop2) - 1];
                            sb[(sb.Length - dintLoop2) - 1] = chrTemp;
                        }
                        arrCon[dintLoop] = sb.ToString();
                    }
                    strReadData = string.Join("", arrCon);

                    DateTime dtNow = DateTime.Now;
                    for (int dintLoop = 0; dintLoop < strReadData.Length; dintLoop++)
                    {
                        if (strReadData[dintLoop].ToString() == "1")
                        {
                            if (pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID) != null)
                            {
                                if (pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID) == null)
                                {
                                    pInfo.Unit(dintUnitNo).SubUnit(0).AddCurrAlarm(dintLoop + 1 + dintAddID);
                                    pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).SETCODE = 1;
                                    pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmCode;
                                    pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmOCCTime = dtNow.ToString("yyyy-MM-dd HH:mm:ss");
                                    pInfo.Unit(0).SubUnit(0).AddCurrAlarm(dintLoop + 1 + dintAddID);
                                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).SETCODE = 1;
                                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmCode;
                                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmOCCTime = dtNow.ToString("yyyy-MM-dd HH:mm:ss");

                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintLoop + 1 + dintAddID, dintUnitNo, "SET");
                                    dstrAlarmMsg = pInfo.Unit(0).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmOCCTime + "," +
                                                   pInfo.Unit(dintUnitNo).SubUnit(0).ReportUnitID + "," +
                                                   "SET" + "," +
                                                   (dintLoop + 1 + dintAddID).ToString() + "," +
                                                   pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmCode + "," +
                                                   pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmType + "," +
                                                   pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmDesc;

                                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);
                                    //subGLSPosLogWrite(pInfo.Unit(0).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmOCCTime, dintUnitNo, dintLoop + 1 + dintAddID);
                                }
                            }
                            else
                            {
                                int dintAlarmID = 0;
                                dintAlarmID = dintLoop + 1 + dintAddID;
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Alarm Set Error!! Alarm ID Error!! Not Exist!! AlarmID : {0}", dintAlarmID));
                            }
                        }
                        else
                        {
                            //if (pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID) != null)
                            //{
                                if (pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID) != null)
                                {
                                    pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).SETCODE = 2;
                                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).SETCODE = 2;
                                    pInfo.Unit(dintUnitNo).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmCode;
                                    pInfo.Unit(0).SubUnit(0).CurrAlarm(dintLoop + 1 + dintAddID).AlarmCode = pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmCode;

                                    dstrAlarmMsg = dtNow.ToString("yyyy-MM-dd HH:mm:ss") + "," +
                                                  pInfo.Unit(dintUnitNo).SubUnit(0).ReportUnitID + "," +
                                                  "RESET" + "," +
                                                  (dintLoop + 1 + dintAddID).ToString() + "," +
                                                  pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmCode + "," +
                                                  pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmType + "," +
                                                  pInfo.Unit(0).SubUnit(0).Alarm(dintLoop + 1 + dintAddID).AlarmDesc;

                                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.Alarm, dstrAlarmMsg);
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S5F1AlarmReportsend, dintLoop + 1 + dintAddID, dintUnitNo, "RESET");

                                }
                            //}
                            //else
                            //{
                            //    int dintAlarmID = 0;
                            //    dintAlarmID = dintLoop + 1 + dintAddID;
                            //    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Alarm ReSet Error!! Alarm ID Error!! Not Exist!! AlarmID : {0}", dintAlarmID));
                            //}
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                m_pEqpAct.subSetConfirmBit(strCompBit);
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
