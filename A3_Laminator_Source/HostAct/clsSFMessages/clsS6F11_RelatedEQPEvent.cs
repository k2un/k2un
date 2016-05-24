using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_RelatedEQPEvent : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_RelatedEQPEvent Instance = new clsS6F11_RelatedEQPEvent();
        #endregion

        #region Constructors
        public clsS6F11_RelatedEQPEvent()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11RelatedEQPEvent";
            this.StrSecondaryMsgName = "S6F12";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            try
            {

            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }
        }

        /// <summary>
        /// Primary Message를 Biuld하여 Transaction을 Return한다.
        /// </summary>
        /// <param name="strParameters">Parameter 문자열</param>
        public Transaction funPrimarySend(string strParameters)
        {
            string[] arrayEvent;
            bool dbolHeavyAlarm = false;
            int dintAlarmID = 0;
            string strMCCData = "";
            try
            {
                arrayEvent = strParameters.Split(',');
                int dintCEID = Convert.ToInt32(arrayEvent[1]);                       //CEID
                int dintUnitID = Convert.ToInt32(arrayEvent[2]);                     //UnitID
                int dintSubUnitID = Convert.ToInt32(arrayEvent[3]);

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                InfoAct.clsSubUnit subUnit = pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(1);
                pMsgTran.Primary().Item("MODULEID").Putvalue(subUnit.ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(subUnit.EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(subUnit.EQPProcessState);

                //각 CEID별로값을 설정한다.
                if (dintCEID == 51)
                {
                    pMsgTran.Primary().Item("BYWHO").Putvalue(subUnit.EQPProcessStateChangeBYWHO);
                    pMsgTran.Primary().Item("OLD_STATE").Putvalue(subUnit.EQPProcessStateOLD);
                    pMsgTran.Primary().Item("NEW_STATE").Putvalue(subUnit.EQPProcessState);
                }
                else if (dintCEID == 53)
                {
                    pMsgTran.Primary().Item("BYWHO").Putvalue(subUnit.EQPStateChangeBYWHO);
                    pMsgTran.Primary().Item("OLD_STATE").Putvalue(subUnit.EQPStateOLD);
                    pMsgTran.Primary().Item("NEW_STATE").Putvalue(subUnit.EQPState);
                }
                else if (dintCEID == 71 || dintCEID == 72 || dintCEID == 73)
                {
                    pMsgTran.Primary().Item("BYWHO").Putvalue(pInfo.All.ControlstateChangeBYWHO);
                    pMsgTran.Primary().Item("OLD_STATE").Putvalue(pInfo.All.ControlStateOLD);
                    pMsgTran.Primary().Item("NEW_STATE").Putvalue(pInfo.All.ControlState);

                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, dintCEID - 70);
                }
                pMsgTran.Primary().Item("OPERID").Putvalue(this.pInfo.All.UserID);
                pMsgTran.Primary().Item("RPTID1").Putvalue(4);  //Fixed Value
                pMsgTran.Primary().Item("RPTID2").Putvalue(8);  //Fixed Value

                switch (dintCEID)
                {
                    case 51:
                        ///////////////발생 체크////////////////////////////
                        //이전 상태가 정상상태(Pause가 아닌상태)이고 현재 상태가 Pause이면 현재 발생한 Alarm정보를 입력한다.
                        if (this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPProcessStateOLD != "4" && this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPProcessState == "4")
                        {
                            if (dintUnitID == 0 || dintUnitID == 3)//160510 KEUN 조건식 변경 (dintUnitID == 0)
                            {
                                //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                foreach (int dintAlarm in this.pInfo.Unit(0).SubUnit(0).CurrAlarm())
                                {
                                    if (this.pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarm).AlarmType == "H")
                                    {
                                        dbolHeavyAlarm = true;
                                        break;
                                    }
                                }

                                if (dbolHeavyAlarm == true && this.pInfo.All.OccurHeavyAlarmID != 0)
                                {
                                    InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).Alarm(pInfo.All.OccurHeavyAlarmID);

                                    pMsgTran.Primary().Item("ALCD").Putvalue(currentAlarm.AlarmCode);
                                    pMsgTran.Primary().Item("ALID").Putvalue(pInfo.All.OccurHeavyAlarmID);
                                    pMsgTran.Primary().Item("ALTX").Putvalue(currentAlarm.AlarmDesc);
                                    pMsgTran.Primary().Item("MODULEID1").Putvalue(currentAlarm.ModuleID);
                                    dintAlarmID = pInfo.All.OccurHeavyAlarmID;

                                }
                                else
                                {
                                    pMsgTran.Primary().Item("ALCD").Putvalue(0);
                                    pMsgTran.Primary().Item("ALID").Putvalue(0);
                                    pMsgTran.Primary().Item("ALTX").Putvalue("");
                                    pMsgTran.Primary().Item("MODULEID1").Putvalue("");
                                }
                            }
                            else
                            {
                                if (pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPState == "2")
                                {
                                    int intAlarmID = this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).AlarmID;
                                    if (intAlarmID == 0)
                                    {
                                        intAlarmID = this.pInfo.Unit(dintUnitID).SubUnit(0).AlarmID;
                                    }

                                    if (intAlarmID != 0)
                                    {
                                        if (pInfo.Unit(0).SubUnit(0).CurrAlarm(intAlarmID) != null)
                                        {
                                            InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).Alarm(intAlarmID);

                                            pMsgTran.Primary().Item("ALCD").Putvalue(currentAlarm.AlarmCode);
                                            pMsgTran.Primary().Item("ALID").Putvalue(pInfo.All.OccurHeavyAlarmID);
                                            pMsgTran.Primary().Item("ALTX").Putvalue(currentAlarm.AlarmDesc);
                                            pMsgTran.Primary().Item("MODULEID1").Putvalue(currentAlarm.ModuleID);
                                            dintAlarmID = pInfo.All.OccurHeavyAlarmID;
                                        }
                                        else
                                        {
                                            pMsgTran.Primary().Item("ALCD").Putvalue(0);
                                            pMsgTran.Primary().Item("ALID").Putvalue(0);
                                            pMsgTran.Primary().Item("ALTX").Putvalue("");
                                            pMsgTran.Primary().Item("MODULEID1").Putvalue("");
                                        }
                                    }
                                    else
                                    {
                                        pMsgTran.Primary().Item("ALCD").Putvalue(0);
                                        pMsgTran.Primary().Item("ALID").Putvalue(0);
                                        pMsgTran.Primary().Item("ALTX").Putvalue("");
                                        pMsgTran.Primary().Item("MODULEID1").Putvalue("");
                                    }
                                }
                                else
                                {
                                    pMsgTran.Primary().Item("ALCD").Putvalue(0);
                                    pMsgTran.Primary().Item("ALID").Putvalue(0);
                                    pMsgTran.Primary().Item("ALTX").Putvalue("");
                                    pMsgTran.Primary().Item("MODULEID1").Putvalue("");
                                }
                            }
                        }

                        ///////////////해제 체크////////////////////////////
                        //이전 상태가 Pause이고 현재 상태가 정상상태(Pause가 아닌상태), Fault가 아니면 현재 Clear된 Alarm정보를 입력한다.
                        else if (this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPState == "2" && this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPProcessStateOLD == "4"
                                        && this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPProcessState != "4")
                        {

                            pMsgTran.Primary().Item("ALCD").Putvalue(0);
                            pMsgTran.Primary().Item("ALID").Putvalue(0);
                            pMsgTran.Primary().Item("ALTX").Putvalue("");
                            pMsgTran.Primary().Item("MODULEID1").Putvalue("");
                        }

                        //[2015/04/26] Event Log(Modify by HS)
                        if (dintSubUnitID != 0)
                        {
                            strMCCData = "EVENT;";
                            strMCCData += "CEID_51" + ",";
                            strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                            if (pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).GLSExist)
                            {
                                InfoAct.clsGLS CurrentGLS = pInfo.GLSID(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).HGLSID);
                                if (CurrentGLS == null)
                                {
                                    strMCCData += ",,,";
                                }
                                else
                                {
                                    strMCCData += CurrentGLS.STEPID + ",";
                                    strMCCData += CurrentGLS.H_PANELID + ",";
                                    strMCCData += CurrentGLS.LOTID + ",";
                                }
                            }
                            else
                            {
                                strMCCData += ",,,";
                            }
                            strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ",";
                            PROCESSSTATE PROC_OLD = (PROCESSSTATE)(Convert.ToInt32(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPProcessStateOLD));
                            PROCESSSTATE PROC = (PROCESSSTATE)(Convert.ToInt32(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPProcessState));

                            strMCCData += PROC_OLD.ToString() + ",";
                            strMCCData += PROC.ToString() + ";";

                            pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                        }

                        break;


                    case 53:
                        ///////////////발생 체크////////////////////////////
                        //이전 상태가 정상상태(Normal 혹은 PM)이고 현재 상태가 Fault이면 현재 발생한 Alarm정보를 입력한다.
                        if (this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPStateOLD != "2" && this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPState == "2")
                        {
                            if (dintUnitID == 0 || dintUnitID == 3)//160510 KEUN 조건식 변경 (dintUnitID == 0)
                            {
                                //현재 Unit에서 Heavy Alarm이 발생해있는지 여부
                                foreach (int dintAlarm in this.pInfo.Unit(0).SubUnit(0).CurrAlarm())
                                {
                                    if (this.pInfo.Unit(0).SubUnit(0).CurrAlarm(dintAlarm).AlarmType == "H")
                                    {
                                        dbolHeavyAlarm = true;
                                        break;
                                    }
                                }

                                if (dbolHeavyAlarm == true && this.pInfo.All.OccurHeavyAlarmID != 0)
                                {
                                    InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).Alarm(pInfo.All.OccurHeavyAlarmID);

                                    pMsgTran.Primary().Item("ALCD").Putvalue(currentAlarm.AlarmCode);
                                    pMsgTran.Primary().Item("ALID").Putvalue(pInfo.All.OccurHeavyAlarmID);
                                    pMsgTran.Primary().Item("ALTX").Putvalue(currentAlarm.AlarmDesc);
                                    pMsgTran.Primary().Item("MODULEID1").Putvalue(currentAlarm.ModuleID);
                                    dintAlarmID = pInfo.All.OccurHeavyAlarmID;
                                }
                                else
                                {
                                    pMsgTran.Primary().Item("ALCD").Putvalue(0);
                                    pMsgTran.Primary().Item("ALID").Putvalue(0);
                                    pMsgTran.Primary().Item("ALTX").Putvalue("");
                                    pMsgTran.Primary().Item("MODULEID1").Putvalue("");
                                }
                            }
                            else
                            {
                                int intAlarmID = this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).AlarmID;
                                if (intAlarmID == 0)
                                {
                                    intAlarmID = this.pInfo.Unit(dintUnitID).SubUnit(0).AlarmID;
                                }


                                if (intAlarmID != 0)
                                {
                                    InfoAct.clsAlarm currentAlarm = pInfo.Unit(0).SubUnit(0).Alarm(intAlarmID);

                                    pMsgTran.Primary().Item("ALCD").Putvalue(currentAlarm.AlarmCode);
                                    pMsgTran.Primary().Item("ALID").Putvalue(pInfo.All.OccurHeavyAlarmID);
                                    pMsgTran.Primary().Item("ALTX").Putvalue(currentAlarm.AlarmDesc);
                                    pMsgTran.Primary().Item("MODULEID1").Putvalue(currentAlarm.ModuleID);
                                    dintAlarmID = pInfo.All.OccurHeavyAlarmID;

                                }
                                else
                                {
                                    pMsgTran.Primary().Item("ALCD").Putvalue(0);
                                    pMsgTran.Primary().Item("ALID").Putvalue(0);
                                    pMsgTran.Primary().Item("ALTX").Putvalue("");
                                    pMsgTran.Primary().Item("MODULEID1").Putvalue("");
                                }
                            }
                        }

                        ///////////////해제 체크////////////////////////////
                        //이전 상태가 Fault이고 현대 상태가 정상상태(Normal 혹은 PM)이면 현재 Clear된 Alarm정보를 입력한다.
                        else if (this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPStateOLD == "2" && this.pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPState != "2")
                        {
                            //Alarm Reset 후 51, 53 보고시 Alarm Reset 정보를 보내지 않아도 됨.
                            pMsgTran.Primary().Item("ALCD").Putvalue(0);
                            pMsgTran.Primary().Item("ALID").Putvalue(0);
                            pMsgTran.Primary().Item("ALTX").Putvalue("");
                            pMsgTran.Primary().Item("MODULEID1").Putvalue("");
                        }

                        //[2015/04/26] Event Log(Modify by HS)
                        if (dintSubUnitID != 0)
                        {
                            strMCCData = "EVENT;";
                            strMCCData += "CEID_53" + ",";
                            strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                            if (pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).GLSExist)
                            {
                                InfoAct.clsGLS CurrentGLS = pInfo.GLSID(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).HGLSID);
                                //[2015/05/20](Add by HS)
                                if (CurrentGLS == null)
                                {
                                    strMCCData += ",,,";
                                }
                                else
                                {
                                    strMCCData += CurrentGLS.STEPID + ",";
                                    strMCCData += CurrentGLS.H_PANELID + ",";
                                    strMCCData += CurrentGLS.LOTID + ",";
                                }
                            }
                            else
                            {
                                strMCCData += ",,,";
                            }
                            strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ",";
                            EQPSTATE EQP_OLD = (EQPSTATE)(Convert.ToInt32(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPStateOLD));
                            EQPSTATE EQP = (EQPSTATE)(Convert.ToInt32(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPState));

                            strMCCData += EQP_OLD + ",";
                            strMCCData += EQP + ",";

                            if (dintAlarmID != 0)
                            {
                                strMCCData += dintAlarmID.ToString() + ",";
                                strMCCData += pInfo.Unit(0).SubUnit(0).Alarm(dintAlarmID).AlarmDesc + ";";
                            }
                            else
                            {
                                strMCCData += dintAlarmID.ToString() + ",";
                                strMCCData += ";";
                            }
                            pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                        }
                            break;
                       

                    default:
                        pMsgTran.Primary().Item("ALCD").Putvalue(0);
                        pMsgTran.Primary().Item("ALID").Putvalue(0);
                        pMsgTran.Primary().Item("ALTX").Putvalue("");
                        pMsgTran.Primary().Item("MODULEID1").Putvalue("");

                        if (dintCEID == 73)
                        {
                            pInfo.Unit(0).SubUnit(0).RemoveTRID();
                        }
                        break;
                }

                return pMsgTran;
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return null;
            }
        }

        /// <summary>
        /// Secondary Message 수신에 대해 처리한다.
        /// </summary>
        /// <param name="msgTran">Secondary Message의 Transaction</param>
        public void funSecondaryReceive(Transaction msgTran)
        {
            try
            {

            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
            
        }

        enum EQPSTATE
        {
           NORMAL = 1,
            FAULT = 2,
            PM = 3,
        }

        enum PROCESSSTATE
        {
            IDLE = 1,
            SETUP = 2,
            EXCUTING = 3,
            PAUSE = 4,
        }
        #endregion
    }
}
