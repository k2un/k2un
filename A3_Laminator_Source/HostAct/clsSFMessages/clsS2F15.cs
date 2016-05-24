using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F15 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F15 Instance = new clsS2F15();
        #endregion

        #region Constructors
        public clsS2F15()
        {
            this.IntStream = 2;
            this.IntFunction = 15;
            this.StrPrimaryMsgName = "S2F15";
            this.StrSecondaryMsgName = "S2F16";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrModuleID = "";
            int dintECIDCount = 0;
            int dintECID = 0;
            string dstrECName = "";
            Boolean dbolFail = false;
            bool dbolECNameFail = false;
            bool dbolECIDFail = false;
            int dintFailCount = 0;
            //int dintECSLL = 0;
            //int dintECWLL = 0;
            //int dintECDEF = 0;
            //int dintECWUL = 0;
            //int dintECSUL = 0;

            float dintECSLL = 0;
            float dintECWLL = 0;
            float dintECDEF = 0;
            float dintECWUL = 0;
            float dintECSUL = 0;

            int dintEAC = 0;
            int dintEACName = 0;
            int dintEACECID = 0;
            int dintTEAC = 0;
            int dintEAC_SLL = 0;
            int dintEAC_WLL = 0;
            int dintEAC_DEF = 0;
            int dintEAC_WUL = 0;
            int dintEAC_SUL = 0;

            bool dbolErrorCheck = true;

            Boolean dbolLayerExist = false;
            Queue dqECID = new Queue();


            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                
                //for (int dintUnit = 0; dintUnit <= this.pInfo.UnitCount; dintUnit++)
                //{
                    if (dstrModuleID == pInfo.Unit(3).SubUnit(0).ModuleID)
                    {
                        dbolLayerExist = true;
                        //break;
                    }
                //}

                //ModuleID가 존재하지 않는 경우
                if (dbolLayerExist == false)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("MIACK").Putvalue(1);      //1 : There is no such a MOUDLEID
                    msgTran.Secondary().Item("ECCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                msgTran.Secondary().Item("MIACK").Putvalue(0);
                dintECIDCount = Convert.ToInt32(msgTran.Primary().Item("ECCOUNT").Getvalue());
                msgTran.Secondary().Item("ECCOUNT").Putvalue(dintECIDCount);

                //받은 ECID중에 존재하지 않는것이 하나라도 있으면 NAK으로 보고하고 PLC에 모두 적용하지 않는다.
                for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                {
                    dintECID = Convert.ToInt32(msgTran.Primary().Item("ECID" + (dintLoop - 1)).Getvalue());
                    dstrECName = msgTran.Primary().Item("ECNAME" + (dintLoop - 1)).Getvalue().ToString().Trim();

                    //존재하지 않는 ECID가 왔을때
                    if (pInfo.Unit(0).SubUnit(0).ECID(dintECID) == null)
                    {
                        dbolECIDFail = true;
                    }
                    else
                    {
                        if (dstrECName != this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).Name)  //존재하지 않는 ECID가 왔을때
                        {
                            dbolECNameFail = true;
                        }
                        else if (this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).ModuleID.Contains(dstrModuleID) == false)
                        {
                            dbolFail = true;
                        }
                        else
                        {
                            string dstrFormat = this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).Format;

                            dintECSLL = Convert.ToSingle(FunStringH.funMakeRound(msgTran.Primary().Item("ECSLL" + (dintLoop - 1)).Getvalue().ToString().Trim(), dstrFormat));
                            dintECWLL = Convert.ToSingle(FunStringH.funMakeRound(msgTran.Primary().Item("ECWLL" + (dintLoop - 1)).Getvalue().ToString().Trim(), dstrFormat));
                            dintECDEF = Convert.ToSingle(FunStringH.funMakeRound(msgTran.Primary().Item("ECDEF" + (dintLoop - 1)).Getvalue().ToString().Trim(), dstrFormat));
                            dintECWUL = Convert.ToSingle(FunStringH.funMakeRound(msgTran.Primary().Item("ECWUL" + (dintLoop - 1)).Getvalue().ToString().Trim(), dstrFormat));
                            dintECSUL = Convert.ToSingle(FunStringH.funMakeRound(msgTran.Primary().Item("ECSUL" + (dintLoop - 1)).Getvalue().ToString().Trim(), dstrFormat));

                            Single dSingleMin = Convert.ToSingle(this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).Min);
                            Single dSingleMax = Convert.ToSingle(this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).Max);

                            if (Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSLL) != dintECSLL)
                            {
                                dbolFail = true;
                                dintEAC_SLL = 2;
                            }
                            else if (Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSUL) != dintECSUL)
                            {
                                dbolFail = true;
                                dintEAC_SUL = 2;
                            }
                            else 
                            #region Min & Max 값 확인
                            if ((dintECDEF < dSingleMin) || (dintECDEF > dSingleMax))
                            {
                                dbolFail = true;
                                dintEAC_DEF = 2;
                            }
                            else if ((dintECSUL < dSingleMin) || (dintECSUL > dSingleMax))
                            {
                                dbolFail = true;
                                dintEAC_SUL = 2;
                            }
                            else if ((dintECSLL < dSingleMin) || (dintECSLL > dSingleMax))
                            {
                                dbolFail = true;
                                dintEAC_SLL = 2;
                            }
                            else if ((dintECWUL < dSingleMin) || (dintECWUL > dSingleMax))
                            {
                                dbolFail = true;
                                dintEAC_WUL = 2;
                            }
                            else if ((dintECWLL < dSingleMin) || (dintECWLL > dSingleMax))
                            {
                                dbolFail = true;
                                dintEAC_WLL = 2;
                            }




                            //if ((dintECWUL < dSingleMin) || (dintECWUL > dSingleMax))
                            //{
                            //    dbolFail = true;
                            //    dintEAC_WUL = 2;
                            //}
                            //if ((dintECWLL < dSingleMin) || (dintECWLL > dSingleMax))
                            //{
                            //    dbolFail = true;
                            //    dintEAC_WLL = 2;
                            //}
                            #endregion

                            if (!dbolFail)
                            {
                                dbolErrorCheck = false;
                                // 각 항목별 Data 확인
                                if (dintECSUL >= dintECWUL && dintECSUL >= dintECDEF && dintECSUL >= dintECWLL && dintECSUL >= dintECSLL)
                                {
                                    if (dintECWUL >= dintECDEF && dintECWUL >= dintECWLL && dintECWLL >= dintECSLL)
                                    {
                                        if (dintECDEF >= dintECWLL && dintECDEF >= dintECSLL)
                                        {
                                            if (dintECWLL < dintECSLL)
                                            {
                                                dbolFail = true;
                                                dintEAC_WLL = 2;
                                            }
                                            else
                                            {
                                                dqECID.Enqueue(dintECID);                       //Update를 위해 Queue에 저장
                                            }
                                        }
                                        else
                                        {
                                            if (!(dintECDEF > dintECWLL))
                                            {
                                                dintEAC_DEF = 2;
                                                dintEAC_WLL = 2;
                                            }
                                            else
                                            {
                                                dintEAC_DEF = 2;
                                                dintEAC_SLL = 2;
                                            }
                                            dbolFail = true;
                                            //dintEAC_DEF = 2;
                                        }
                                    }
                                    else
                                    {
                                        if (!(dintECWUL > dintECDEF))
                                        {
                                            dintEAC_WUL = 2;
                                            dintEAC_DEF = 2;
                                        }
                                        else if(! (dintECWUL > dintECWLL))
                                        {
                                            dintEAC_WUL = 2;
                                            dintEAC_WLL = 2;
                                        }
                                        else
                                        {
                                            dintEAC_WUL = 2;
                                            dintEAC_SLL = 2;
                                        }
                                        dbolFail = true;
                                        //dintEAC_DEF = 2;
                                    }
                                }
                                else
                                {
                                    if(! (dintECSUL > dintECWUL))
                                    {
                                        dintEAC_SUL = 2;
                                        dintEAC_WUL = 2;
                                    }
                                    else if(! (dintECSUL > dintECDEF))
                                    {
                                        dintEAC_SUL = 2;
                                        dintEAC_DEF = 2;
                                    }
                                    else if (!(dintECSUL > dintECWLL))
                                    {
                                        dintEAC_SUL = 2;
                                        dintEAC_WLL = 2;
                                    }
                                    else
                                    {
                                        dintEAC_SUL = 2;
                                        dintEAC_SLL = 2;
                                    }
                                    dbolFail = true;
                                    //dintEAC_DEF = 2;
                                }
                            }
                        }
                    }

                    if (dbolFail == true)  //Validation 체크 에러
                    {
                        dintFailCount = dintFailCount + 1;      //각 항목별로 Fail이 났을때 1씩 증가
                        dintEAC = 2;
                        if (dbolErrorCheck == false)
                        {
                            if (Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSLL) != dintECSLL)
                            {
                                dintEAC_SLL = 2;
                            }
                            else if (Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSUL) != dintECSUL)
                            {
                                dintEAC_SUL = 2;
                            }
                            else
                            {
                                //dintEAC_SLL = 0;
                                //dintEAC_WLL = 0;
                                //dintEAC_DEF = 0;
                                //dintEAC_SUL = 0;
                                //dintEAC_WUL = 0;
                                //WLL 확인
                                if (!(Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSLL) <= dintECWLL && dintECWLL <= Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECDEF)))
                                {
                                    dintEAC_WLL = 2;
                                    dintEAC_SLL = 0;
                                    //dintEAC_WLL = 0;
                                    dintEAC_DEF = 0;
                                    dintEAC_SUL = 0;
                                    dintEAC_WUL = 0;
                                }
                                else
                                {
                                    if (!(Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWLL) <= dintECDEF && dintECDEF <= Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECWUL)))
                                    {
                                        dintEAC_DEF = 2;
                                        dintEAC_SLL = 0;
                                        dintEAC_WLL = 0;
                                        //dintEAC_DEF = 0;
                                        dintEAC_SUL = 0;
                                        dintEAC_WUL = 0;
                                    }
                                    else
                                    {
                                        if (!(Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECDEF) <= dintECWUL && dintECWUL <= Convert.ToSingle(pInfo.Unit(0).SubUnit(0).ECID(dintECID).ECSUL)))
                                        {
                                            dintEAC_WUL = 2;
                                            dintEAC_SLL = 0;
                                            dintEAC_WLL = 0;
                                            dintEAC_DEF = 0;
                                            dintEAC_SUL = 0;
                                            //dintEAC_WUL = 0;
                                        }
                                        else
                                        {
                                            int a = 0;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        dintEAC = 0;
                    }

                    if (dbolECIDFail == true)
                    {
                        dintEACECID = 2;
                    }
                    else
                    {
                        dintEACECID = 0;
                    }

                    if (dbolECNameFail == true)
                    {
                        dintEACName = 2;
                    }
                    else
                    {
                        dintEACName = 0;
                    }

                    if (dbolFail || dbolECIDFail || dbolECNameFail)
                    {
                        dintTEAC = 1;
                    }
                    else
                    {
                        dintTEAC = 0;
                    }


                    dbolFail = false;
                    dbolECIDFail = false;
                    dbolECNameFail = false;

                    msgTran.Secondary().Item("TEAC" + (dintLoop - 1)).Putvalue(dintTEAC);

                    msgTran.Secondary().Item("ECID" + (dintLoop - 1)).Putvalue(dintECID);
                    msgTran.Secondary().Item("EAC1" + (dintLoop - 1)).Putvalue(dintEACECID);

                    msgTran.Secondary().Item("ECNAME" + (dintLoop - 1)).Putvalue(dstrECName);
                    msgTran.Secondary().Item("EAC2" + (dintLoop - 1)).Putvalue(dintEACName);

                    msgTran.Secondary().Item("ECDEF" + (dintLoop - 1)).Putvalue(dintECDEF);
                    msgTran.Secondary().Item("EAC3" + (dintLoop - 1)).Putvalue(dintEAC_DEF);

                    msgTran.Secondary().Item("ECSLL" + (dintLoop - 1)).Putvalue(dintECSLL);
                    msgTran.Secondary().Item("EAC4" + (dintLoop - 1)).Putvalue(dintEAC_SLL);

                    msgTran.Secondary().Item("ECSUL" + (dintLoop - 1)).Putvalue(dintECSUL);
                    msgTran.Secondary().Item("EAC5" + (dintLoop - 1)).Putvalue(dintEAC_SUL);

                    msgTran.Secondary().Item("ECWLL" + (dintLoop - 1)).Putvalue(dintECWLL);
                    msgTran.Secondary().Item("EAC6" + (dintLoop - 1)).Putvalue(dintEAC_WLL);

                    msgTran.Secondary().Item("ECWUL" + (dintLoop - 1)).Putvalue(dintECWUL);
                    msgTran.Secondary().Item("EAC7" + (dintLoop - 1)).Putvalue(dintEAC_WUL);

                    dintEAC_SLL = 0;
                    dintEAC_WLL = 0;
                    dintEAC_DEF = 0;
                    dintEAC_WUL = 0;
                    dintEAC_SUL = 0;
                }



                //HOST로 S2F16 보고
                funSendReply(msgTran);


                if (dintFailCount > 0)
                {
                    //Error가 발생한것임.
                    dqECID.Clear();
                    return;
                }

                ////NACK값이 하나라도 존재하면 PLC에 적용하지 않느다.
                ////2011.01.13        송은선
                //if (dintEAC != 0 && dintTEAC != 0)
                //{
                //    dqECID.Clear();
                //    return;
                //}

                //카운트가 0이상이면 맞는게 있음
                if (dqECID.Count > 0)
                {
                    this.pInfo.All.ECIDChangeBYWHO = "1";   //BY HOST

                    this.pInfo.All.ECIDChange.Clear();      //초기화
                    this.pInfo.All.ECIDChangeHOSTReport.Clear();      //초기화
                    this.pInfo.All.ECIDChangeFromHost = "";


                    string dstrECSLL = string.Empty;
                    string dstrECWLL = string.Empty;
                    string dstrECDEF = string.Empty;
                    string dstrECWUL = string.Empty;
                    string dstrECSUL = string.Empty;


                    //PLC에 Write할 ECID를 저장한다.                        
                    for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                    {
                        dintECID = Convert.ToInt32(msgTran.Primary().Item("ECID" + (dintLoop - 1)).Getvalue());

                        if (dqECID.Contains(dintECID) == true)
                        {
                            string dstrFormat = this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).Format;

                            dstrECSLL = FunStringH.funMakePLCData(FunStringH.funMakeRound(msgTran.Primary().Item("ECSLL" + (dintLoop - 1)).Getvalue().ToString(), dstrFormat));
                            dstrECWLL = FunStringH.funMakePLCData(FunStringH.funMakeRound(msgTran.Primary().Item("ECWLL" + (dintLoop - 1)).Getvalue().ToString(), dstrFormat));
                            dstrECDEF = FunStringH.funMakePLCData(FunStringH.funMakeRound(msgTran.Primary().Item("ECDEF" + (dintLoop - 1)).Getvalue().ToString(), dstrFormat));
                            dstrECWUL = FunStringH.funMakePLCData(FunStringH.funMakeRound(msgTran.Primary().Item("ECWUL" + (dintLoop - 1)).Getvalue().ToString(), dstrFormat));
                            dstrECSUL = FunStringH.funMakePLCData(FunStringH.funMakeRound(msgTran.Primary().Item("ECSUL" + (dintLoop - 1)).Getvalue().ToString(), dstrFormat));

                            this.pInfo.All.ECIDChange.Add(dintECID, dstrECSLL + "," + dstrECWLL + "," + dstrECDEF + "," + dstrECWUL + "," + dstrECSUL);

                            this.pInfo.All.ECIDChangeFromHost += dintECID + ";";

                            this.pInfo.All.ECIDChangeHOSTReport.Add(dintECID, dstrECSLL + "," + dstrECWLL + "," + dstrECDEF + "," + dstrECWUL + "," + dstrECSUL);

                          
                        }
                    }

                    //HOST에서 받은것외에 없는 항목은 CIM이 가지고 있는 Data를 써준다.
                    //변경할 ECID만 써주는게 아니고 모든 ECID를 써주기 때문임
                    for (int dintLoop = 1; dintLoop <= this.pInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop++)
                    {
                        if (this.pInfo.All.ECIDChange.ContainsKey(dintLoop) == true)
                        {

                        }
                        else
                        {
                            InfoAct.clsECID pECID = pInfo.Unit(0).SubUnit(0).ECID(dintLoop);

                            this.pInfo.All.ECIDChange.Add(dintLoop, FunStringH.funMakePLCData(FunStringH.funMakeRound(pECID.ECSLL, pECID.Format)) + ","
                                                                  + FunStringH.funMakePLCData(FunStringH.funMakeRound(pECID.ECWLL, pECID.Format)) + ","
                                                                  + FunStringH.funMakePLCData(FunStringH.funMakeRound(pECID.ECDEF, pECID.Format)) + ","
                                                                  + FunStringH.funMakePLCData(FunStringH.funMakeRound(pECID.ECWUL, pECID.Format)) + ","
                                                                  + FunStringH.funMakePLCData(FunStringH.funMakeRound(pECID.ECSUL, pECID.Format)));
                        }
                    }

                    //PLC로 ECID 변경 Write
                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ECIDChange);
                }
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
            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

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
        #endregion
    }
}
