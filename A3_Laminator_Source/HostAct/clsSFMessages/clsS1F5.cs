using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS1F5 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS1F5 Instance = new clsS1F5();
        #endregion

        #region Constructors
        public clsS1F5()
        {
            this.IntStream = 1;
            this.IntFunction = 5;
            this.StrPrimaryMsgName = "S1F5";
            this.StrSecondaryMsgName = "S1F6";
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
            string dstrSFCD = "";
            int dintIndex = 0;
            string dstrLOTID = "";
            int dintSlotID = 0;
            Hashtable dhtGLSID = new Hashtable();
            int dintSendIndex = 0;
            Boolean dbolLayerExist = false;
            bool dbolLayer1Report = false;
            bool dbolLayer2Report = false;
            int dintUnitID = 0;
            int dintSubUnitID = 0;

            try
            {
                dstrSFCD = msgTran.Primary().Item("SFCD").Getvalue().ToString().Trim();
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                switch (dstrSFCD)
                {
                    case "1":
                        if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID)
                        {
                            msgTran.Secondary2("S1F6WrongMODID").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6WrongMODID").Item("SFCD").Putvalue(dstrSFCD);

                            funSendReply2(msgTran, "S1F6WrongMODID");

                            return;
                        }
                        break;

                    case "2":

                        if (dstrModuleID == pInfo.Unit(3).SubUnit(0).ModuleID)
                        {
                            dintUnitID = 0;
                            dintSubUnitID = 0;
                        }
                        else if (dstrModuleID == this.pInfo.Unit(1).SubUnit(0).ModuleID || dstrModuleID == this.pInfo.Unit(2).SubUnit(0).ModuleID)
                        {
                            if (dstrModuleID == this.pInfo.Unit(1).SubUnit(0).ModuleID)
                            {
                                dintUnitID = 1;
                                dintSubUnitID = 0;
                            }
                            else
                            {
                                dintUnitID = 2;
                                dintSubUnitID = 0;
                            }
                        }
                        else
                        {
                            for (int dintLoop = 0; dintLoop < pInfo.Unit(1).SubUnitCount; dintLoop++)
                            {
                                if (dstrModuleID == pInfo.Unit(1).SubUnit(dintLoop + 1).ModuleID)
                                {
                                    dbolLayerExist = true;
                                    dintUnitID = 1;
                                    dintSubUnitID = dintLoop + 1;

                                }

                                if (dbolLayerExist)
                                {
                                    break;
                                }
                            }
                            if (dbolLayerExist == false)
                            {
                                for (int dintLoop = 0; dintLoop < pInfo.Unit(2).SubUnitCount; dintLoop++)
                                {
                                    if (dstrModuleID == pInfo.Unit(2).SubUnit(dintLoop + 1).ModuleID)
                                    {
                                        dbolLayerExist = true;
                                        dintUnitID = 2;
                                        dintSubUnitID = dintLoop + 1;
                                    }

                                    if (dbolLayerExist)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (!dbolLayerExist)
                            {
                                msgTran.Secondary2("S1F6WrongMODID").Item("MODULEID").Putvalue(dstrModuleID);
                                msgTran.Secondary2("S1F6WrongMODID").Item("SFCD").Putvalue(dstrSFCD);

                                funSendReply2(msgTran, "S1F6WrongMODID");
                                return;
                            }
                        }
                        break;

                    case "3":
                        if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID && dstrModuleID != this.pInfo.Unit(3).SubUnit(7).ModuleID
                            && dstrModuleID != this.pInfo.Unit(3).SubUnit(8).ModuleID)
                        {
                            msgTran.Secondary2("S1F6WrongMODID").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6WrongMODID").Item("SFCD").Putvalue(dstrSFCD);

                            funSendReply2(msgTran, "S1F6WrongMODID");

                            return;
                        }
                        break;

                    case "4":
                        if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID)
                        {
                            msgTran.Secondary2("S1F6WrongMODID").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6WrongMODID").Item("SFCD").Putvalue(dstrSFCD);

                            funSendReply2(msgTran, "S1F6WrongMODID");

                            return;
                        }
                        break;

                    case "31":
                        if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID)
                        {
                            msgTran.Secondary2("S1F6WrongMODID").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6WrongMODID").Item("SFCD").Putvalue(dstrSFCD);

                            funSendReply2(msgTran, "S1F6WrongMODID");

                            return;
                        }
                        break;
                }


                //for (int dintLoop = 0; dintLoop <= pInfo.UnitCount; dintLoop++)
                //{
                //    for (int dintLoop2 = 0; dintLoop2 <= pInfo.Unit(dintLoop).SubUnitCount; dintLoop2++)
                //    {
                //        if (dstrModuleID == pInfo.Unit(dintLoop).SubUnit(dintLoop2).ModuleID)
                //        {
                //            dintUnitID = dintLoop;
                //            dintSubUnitID = dintLoop2;
                //            dbolLayerExist = true;
                //            break;
                //        }
                //    }
                //    if (dbolLayerExist)
                //    {
                //        break;
                //    }
                //}

                switch (dstrSFCD)
                {
                    case "1":       //EOID별 EOV값 List 보고
                        {
                            msgTran.Secondary2("S1F6SFCD1").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6SFCD1").Item("SFCD").Putvalue(dstrSFCD);
                            msgTran.Secondary2("S1F6SFCD1").Item("EOIDCNT").Putvalue(this.pInfo.All.HOSTReportEOIDCount);

                            int eoidCount = pInfo.Unit(0).SubUnit(0).EOIDCount;
                            for (int dintLoop = 1; dintLoop <= eoidCount; dintLoop++)
                            {
                                InfoAct.clsEOID eoID = pInfo.Unit(0).SubUnit(0).EOID(dintLoop);
                                msgTran.Secondary2("S1F6SFCD1").Item("EOMDCNT" + dintSendIndex).Putvalue(1);
                                msgTran.Secondary2("S1F6SFCD1").Item("EOID" + dintSendIndex).Putvalue(eoID.EOID);
                                msgTran.Secondary2("S1F6SFCD1").Item("EOMD" + dintSendIndex + 0).Putvalue(eoID.EOMD);
                                msgTran.Secondary2("S1F6SFCD1").Item("EOV" + dintSendIndex + 0).Putvalue(eoID.EOV);
                                dintSendIndex += 1;
                            }

                            funSendReply2(msgTran, "S1F6SFCD1");
                        }
                        break;

                    case "2":
                            msgTran.Secondary2("S1F6SFCD2").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6SFCD2").Item("SFCD").Putvalue(dstrSFCD);

                            int dintCount = 0;
                            if (dintUnitID == 0)
                            {
                                dintCount = pInfo.Unit(1).SubUnitCount + pInfo.Unit(2).SubUnitCount;
                                msgTran.Secondary2("S1F6SFCD2").Item("PORTCNT").Putvalue(dintCount);

                                for (int dintLoop = 1; dintLoop <= dintCount; dintLoop++)
                                {
                                    InfoAct.clsPort CurrentPort = pInfo.Port(dintLoop);
                                    
                                    msgTran.Secondary2("S1F6SFCD2").Item("PORTID" + (dintLoop-1)).Putvalue(CurrentPort.HostReportPortID);
                                    msgTran.Secondary2("S1F6SFCD2").Item("PTSTATE" + (dintLoop - 1)).Putvalue(CurrentPort.PortState);
                                    msgTran.Secondary2("S1F6SFCD2").Item("PTTYPE" + (dintLoop - 1)).Putvalue(CurrentPort.PortType);
                                    msgTran.Secondary2("S1F6SFCD2").Item("PTMODE" + (dintLoop - 1)).Putvalue(CurrentPort.PortMode);
                                    msgTran.Secondary2("S1F6SFCD2").Item("SORTTYPE" + (dintLoop - 1)).Putvalue(0);

                                    msgTran.Secondary2("S1F6SFCD2").Item("CSTID" + (dintLoop - 1)).Putvalue(CurrentPort.CSTID);
                                    msgTran.Secondary2("S1F6SFCD2").Item("CSTTYPE" + (dintLoop - 1)).Putvalue(CurrentPort.CSTType);
                                    msgTran.Secondary2("S1F6SFCD2").Item("MAPSTIF" + (dintLoop - 1)).Putvalue(CurrentPort.GLSLoaderMapping10);
                                    msgTran.Secondary2("S1F6SFCD2").Item("CURSTIF" + (dintLoop - 1)).Putvalue(CurrentPort.GLSRealMapping10);
                                    msgTran.Secondary2("S1F6SFCD2").Item("BATORDER" + (dintLoop - 1)).Putvalue(CurrentPort.BATCH_ORDER);

                                    dintSendIndex += 1;
                                }

                            }
                            else
                            {

                                if (dintSubUnitID == 0)
                                {
                                    msgTran.Secondary2("S1F6SFCD2").Item("PORTCNT").Putvalue(pInfo.Unit(dintUnitID).SubUnitCount);

                                    if (dintUnitID == 1)
                                    {
                                        dintCount = 0;
                                    }
                                    else
                                    {
                                        dintCount = 4;
                                    }

                                    for (int dintLoop = 1; dintLoop <= pInfo.Unit(dintUnitID).SubUnitCount; dintLoop++)
                                    {
                                        InfoAct.clsPort CurrentPort = pInfo.Port(dintLoop + dintCount);

                                        msgTran.Secondary2("S1F6SFCD2").Item("PORTID" + (dintLoop - 1)).Putvalue(CurrentPort.HostReportPortID);
                                        msgTran.Secondary2("S1F6SFCD2").Item("PTSTATE" + (dintLoop - 1)).Putvalue(CurrentPort.PortState);
                                        msgTran.Secondary2("S1F6SFCD2").Item("PTTYPE" + (dintLoop - 1)).Putvalue(CurrentPort.PortType);
                                        msgTran.Secondary2("S1F6SFCD2").Item("PTMODE" + (dintLoop - 1)).Putvalue(CurrentPort.PortMode);
                                        msgTran.Secondary2("S1F6SFCD2").Item("SORTTYPE" + (dintLoop - 1)).Putvalue(0);

                                        msgTran.Secondary2("S1F6SFCD2").Item("CSTID" + (dintLoop - 1)).Putvalue(CurrentPort.CSTID);
                                        msgTran.Secondary2("S1F6SFCD2").Item("CSTTYPE" + (dintLoop - 1)).Putvalue(CurrentPort.CSTType);
                                        msgTran.Secondary2("S1F6SFCD2").Item("MAPSTIF" + (dintLoop - 1)).Putvalue(CurrentPort.GLSLoaderMapping10);
                                        msgTran.Secondary2("S1F6SFCD2").Item("CURSTIF" + (dintLoop - 1)).Putvalue(CurrentPort.GLSRealMapping10);
                                        msgTran.Secondary2("S1F6SFCD2").Item("BATORDER" + (dintLoop - 1)).Putvalue(CurrentPort.BATCH_ORDER);

                                        dintSendIndex += 1;
                                    }
                                }
                                else
                                {
                                    msgTran.Secondary2("S1F6SFCD2").Item("PORTCNT").Putvalue(1);

                                    int dintPortNo = 0;
                                    if (dintUnitID == 1)
                                    {
                                        dintPortNo = dintSubUnitID;
                                    }
                                    else
                                    {
                                        dintPortNo = dintSubUnitID + 4;
                                    }

                                    InfoAct.clsPort CurrentPort = pInfo.Port(dintPortNo);

                                    msgTran.Secondary2("S1F6SFCD2").Item("PORTID" + 0).Putvalue(CurrentPort.HostReportPortID);
                                    msgTran.Secondary2("S1F6SFCD2").Item("PTSTATE" + 0).Putvalue(CurrentPort.PortState);
                                    msgTran.Secondary2("S1F6SFCD2").Item("PTTYPE" + 0).Putvalue(CurrentPort.PortType);
                                    msgTran.Secondary2("S1F6SFCD2").Item("PTMODE" + 0).Putvalue(CurrentPort.PortMode);
                                    msgTran.Secondary2("S1F6SFCD2").Item("SORTTYPE" + 0).Putvalue(0);

                                    msgTran.Secondary2("S1F6SFCD2").Item("CSTID" + 0).Putvalue(CurrentPort.CSTID);
                                    msgTran.Secondary2("S1F6SFCD2").Item("CSTTYPE" + 0).Putvalue(CurrentPort.CSTType);
                                    msgTran.Secondary2("S1F6SFCD2").Item("MAPSTIF" + 0).Putvalue(CurrentPort.GLSLoaderMapping10);
                                    msgTran.Secondary2("S1F6SFCD2").Item("CURSTIF" + 0).Putvalue(CurrentPort.GLSRealMapping10);
                                    msgTran.Secondary2("S1F6SFCD2").Item("BATORDER" + 0).Putvalue(CurrentPort.BATCH_ORDER);

                                }
                            }
                            funSendReply2(msgTran, "S1F6SFCD2");

                        break;

                    case "3":           //현재 장비내에 존재하는 GLS 정보 보고
                        {
                            msgTran.Secondary2("S1F6SFCD3").Item("UNITCNT").Putvalue(1);
                            msgTran.Secondary2("S1F6SFCD3").Item("MODULEID" + 0).Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6SFCD3").Item("SFCD" + 0).Putvalue(dstrSFCD);
                            int dintTemp = 0;
                            if (pInfo.Unit(3).SubUnit(0).ModuleID == dstrModuleID)
                            {
                                dintTemp = 0;
                                
                            }
                            else if (pInfo.Unit(3).SubUnit(7).ModuleID == dstrModuleID)
                            {
                                dintTemp = 7;
                            }
                            else
                            {
                                dintTemp = 8;
                            }

                            //장비안에 GLS가 존재하지 않으면 L,0으로 보고한다.
                            if (this.pInfo.Unit(3).SubUnit(dintTemp).GLSExist == false)
                            {
                                msgTran.Secondary2("S1F6SFCD3").Item("GLSCNT" + 0).Putvalue(0);
                                funSendReply2(msgTran, "S1F6SFCD3");
                                //shtReturn = Convert.ToInt16(msgTran.Reply2("S1F6SFCD3"));
                            }
                            else
                            {
                                msgTran.Secondary2("S1F6SFCD3").Item("GLSCNT" + 0).Putvalue(this.pInfo.Unit(3).SubUnit(dintTemp).CurrGLSCount);

                                foreach (string dstrHGLSID in this.pInfo.Unit(3).SubUnit(dintTemp).CurrGLS())
                                {

                                    InfoAct.clsGLS CurrentGLS = pInfo.GLSID(dstrHGLSID);

                                    //혹시나 1개의 GLS가 막 나갈려는 찰나에 양쪽 Unit에 걸쳐있을떄 GLSID 중복 보고 방지
                                    if (dhtGLSID.ContainsKey(dstrHGLSID) == false && CurrentGLS.Scrap == false)
                                    {
                                        dhtGLSID.Add(dstrHGLSID, dstrHGLSID);

                                        msgTran.Secondary2("S1F6SFCD3").Item("H_PANELID" + 0 + dintIndex).Putvalue(dstrHGLSID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("E_PANELID" + 0 + dintIndex).Putvalue(CurrentGLS.E_PANELID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("LOTID" + 0 + dintIndex).Putvalue(CurrentGLS.LOTID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("BATCHID" + 0 + dintIndex).Putvalue(CurrentGLS.BATCHID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("JOBID" + 0 + dintIndex).Putvalue(CurrentGLS.JOBID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("PORTID" + 0 + dintIndex).Putvalue(CurrentGLS.PORTID.Trim());
                                        msgTran.Secondary2("S1F6SFCD3").Item("SLOTNO" + 0 + dintIndex).Putvalue(CurrentGLS.SLOTNO);
                                        msgTran.Secondary2("S1F6SFCD3").Item("PROD_TYPE" + 0 + dintIndex).Putvalue(CurrentGLS.PRODUCT_TYPE);
                                        msgTran.Secondary2("S1F6SFCD3").Item("PRODID" + 0 + dintIndex).Putvalue(CurrentGLS.PRODUCTID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("RUNSPECID" + 0 + dintIndex).Putvalue(CurrentGLS.RUNSPECID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("LAYERID" + 0 + dintIndex).Putvalue(CurrentGLS.LAYERID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("STEPID" + 0 + dintIndex).Putvalue(CurrentGLS.STEPID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("PPID" + 0 + dintIndex).Putvalue(CurrentGLS.HOSTPPID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("FLOWID" + 0 + dintIndex).Putvalue(CurrentGLS.FLOWID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("SIZE" + 0 + dintIndex).Putvalue(CurrentGLS.SIZE);
                                        msgTran.Secondary2("S1F6SFCD3").Item("THICKNESS" + 0 + dintIndex).Putvalue(CurrentGLS.THICKNESS);
                                        msgTran.Secondary2("S1F6SFCD3").Item("STATE" + 0 + dintIndex).Putvalue(CurrentGLS.GLASS_STATE);
                                        msgTran.Secondary2("S1F6SFCD3").Item("ORDER" + 0 + dintIndex).Putvalue(CurrentGLS.GLASS_ORDER);
                                        msgTran.Secondary2("S1F6SFCD3").Item("COMMENT" + 0 + dintIndex).Putvalue(CurrentGLS.COMMENT);

                                        msgTran.Secondary2("S1F6SFCD3").Item("USE_CNT" + 0 + dintIndex).Putvalue(CurrentGLS.USE_COUNT);
                                        msgTran.Secondary2("S1F6SFCD3").Item("JUDGE" + 0 + dintIndex).Putvalue(CurrentGLS.JUDGEMENT);
                                        msgTran.Secondary2("S1F6SFCD3").Item("REASONCODE" + 0 + dintIndex).Putvalue(CurrentGLS.REASON_CODE);
                                        msgTran.Secondary2("S1F6SFCD3").Item("INS_FLAG" + 0 + dintIndex).Putvalue(CurrentGLS.INS_FLAG);
                                        msgTran.Secondary2("S1F6SFCD3").Item("ENC_FLAG" + 0 + dintIndex).Putvalue(CurrentGLS.ENC_FLAG);
                                        msgTran.Secondary2("S1F6SFCD3").Item("PRERUNFLAG" + 0 + dintIndex).Putvalue(CurrentGLS.PRERUN_FLAG);
                                        msgTran.Secondary2("S1F6SFCD3").Item("TURN_DIR" + 0 + dintIndex).Putvalue(CurrentGLS.TURN_DIR);
                                        msgTran.Secondary2("S1F6SFCD3").Item("FLIPSTATE" + 0 + dintIndex).Putvalue(CurrentGLS.FLIP_STATE);
                                        msgTran.Secondary2("S1F6SFCD3").Item("WORKSTATE" + 0 + dintIndex).Putvalue(CurrentGLS.WORK_STATE);
                                        msgTran.Secondary2("S1F6SFCD3").Item("MULTIUSE" + 0 + dintIndex).Putvalue(CurrentGLS.MULTI_USE);

                                        msgTran.Secondary2("S1F6SFCD3").Item("PAIR_GLSID" + 0 + dintIndex).Putvalue(CurrentGLS.PAIR_GLASSID);
                                        msgTran.Secondary2("S1F6SFCD3").Item("PAIR_PPID" + 0 + dintIndex).Putvalue(CurrentGLS.PAIR_PPID);

                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_NAME1" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_NAME[0]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_VALUE1" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_VALUE[0]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_NAME2" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_NAME[1]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_VALUE2" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_VALUE[1]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_NAME3" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_NAME[2]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_VALUE3" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_VALUE[2]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_NAME4" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_NAME[3]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_VALUE4" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_VALUE[3]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_NAME5" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_NAME[4]);
                                        msgTran.Secondary2("S1F6SFCD3").Item("OPT_VALUE5" + 0 + dintIndex).Putvalue(CurrentGLS.OPTION_VALUE[4]);

                                        dintIndex++;
                                    }
                                }



                                if (dintIndex == 0)
                                {
                                    msgTran.Secondary2("S1F6SFCD3").Item("GLSCNT").Putvalue(0);
                                }

                                funSendReply2(msgTran, "S1F6SFCD3");
                            }
                        }
                        break;

                    case "4":       //각 Unit(Module) 상태 보고
                        {
                            msgTran.Secondary2("S1F6SFCD4").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6SFCD4").Item("MODULE_STATE").Putvalue(this.pInfo.Unit(3).SubUnit(0).EQPState);
                            msgTran.Secondary2("S1F6SFCD4").Item("PROC_STATE").Putvalue(this.pInfo.Unit(3).SubUnit(0).EQPProcessState);
                            msgTran.Secondary2("S1F6SFCD4").Item("MCMD").Putvalue(this.pInfo.All.ControlState);
                            //msgTran.Secondary2("S1F6SFCD4").Item("LAYER1CNT").Putvalue(this.pInfo.UnitCount);
                            msgTran.Secondary2("S1F6SFCD4").Item("LAYER1CNT").Putvalue(pInfo.Unit(3).SubUnitCount + 2);
                            msgTran.Secondary2("S1F6SFCD4").Item("SFCD").Putvalue(dstrSFCD);

                            for (int dintLoop = 1; dintLoop <= this.pInfo.Unit(3).SubUnitCount; dintLoop++)
                            {
                                msgTran.Secondary2("S1F6SFCD4").Item("MODULEID1" + (dintLoop - 1)).Putvalue(this.pInfo.Unit(3).SubUnit(dintLoop).ModuleID);
                                msgTran.Secondary2("S1F6SFCD4").Item("MODULE_STATE1" + (dintLoop - 1)).Putvalue(this.pInfo.Unit(3).SubUnit(dintLoop).EQPState);
                                msgTran.Secondary2("S1F6SFCD4").Item("PROC_STATE1" + (dintLoop - 1)).Putvalue(this.pInfo.Unit(3).SubUnit(dintLoop).EQPProcessState);
                                msgTran.Secondary2("S1F6SFCD4").Item("LAYER2CNT" + (dintLoop - 1)).Putvalue(0);
                            }

                            //[2015/01/22] Port단 상태 보고(Add by HS)
                            for (int dintLoop = 1; dintLoop <= 2; dintLoop++)
                            {
                                msgTran.Secondary2("S1F6SFCD4").Item("MODULEID1" + (this.pInfo.Unit(3).SubUnitCount - 1 + dintLoop)).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).ModuleID);
                                msgTran.Secondary2("S1F6SFCD4").Item("MODULE_STATE1" + (this.pInfo.Unit(3).SubUnitCount - 1 + dintLoop)).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).EQPState);
                                msgTran.Secondary2("S1F6SFCD4").Item("PROC_STATE1" + (this.pInfo.Unit(3).SubUnitCount - 1 + dintLoop)).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).EQPProcessState);
                                msgTran.Secondary2("S1F6SFCD4").Item("LAYER2CNT" + (this.pInfo.Unit(3).SubUnitCount - 1 + dintLoop)).Putvalue(0);
                            }

                            funSendReply2(msgTran, "S1F6SFCD4");
                        }
                        break;

                    case "31":
                        {
                            msgTran.Secondary2("S1F6SFCD31").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6SFCD31").Item("SFCD").Putvalue(dstrSFCD);
                            msgTran.Secondary2("S1F6SFCD31").Item("MODULE_STATE").Putvalue(this.pInfo.Unit(0).SubUnit(0).EQPState);
                            msgTran.Secondary2("S1F6SFCD31").Item("PROC_STATE").Putvalue(this.pInfo.Unit(0).SubUnit(0).EQPProcessState);
                            msgTran.Secondary2("S1F6SFCD31").Item("CUR_STEPNO").Putvalue(0);   // 협의 필요


                            msgTran.Secondary2("S1F6SFCD31").Item("CUR_STEPCNT").Putvalue(this.pInfo.Unit(0).SubUnit(0).CurrGLSCount);
                            msgTran.Secondary2("S1F6SFCD31").Item("CUR_GLSCNT").Putvalue(this.pInfo.Unit(0).SubUnit(0).CurrGLSCount);
                            msgTran.Secondary2("S1F6SFCD31").Item("CUR_M_CNT").Putvalue(this.pInfo.Unit(0).SubUnit(0).CurrGLSCount);

                            int dintStepNoIndex = 0;
                            foreach (InfoAct.clsGLS tmpGLS in this.pInfo.Unit(0).SubUnit(0).CurrGLSValues())
                            {
                                InfoAct.clsSlot currentSlot = pInfo.LOTID(tmpGLS.LOTID).Slot(tmpGLS.SlotID);

                                msgTran.Secondary2("S1F6SFCD31").Item("STEPNO" + dintStepNoIndex).Putvalue(currentSlot.StepNo);
                                msgTran.Secondary2("S1F6SFCD31").Item("STEP_DESC" + dintStepNoIndex).Putvalue(this.pInfo.Unit(0).SubUnit(0).ProcessStep(currentSlot.StepNo).StepDesc);

                                msgTran.Secondary2("S1F6SFCD31").Item("H_GLASSID" + dintStepNoIndex).Putvalue(currentSlot.H_PANELID);
                                msgTran.Secondary2("S1F6SFCD31").Item("MATERIAL_ID" + dintStepNoIndex).Putvalue(currentSlot.H_PANELID);

                                dintStepNoIndex++;
                            }



                            msgTran.Secondary2("S1F6SFCD31").Item("LAYER1CNT").Putvalue(this.pInfo.UnitCount);

                            for (int dintLoop = 1; dintLoop <= this.pInfo.UnitCount; dintLoop++)
                            {
                                msgTran.Secondary2("S1F6SFCD31").Item("MODULEID1" + (dintLoop - 1)).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).ModuleID);
                                msgTran.Secondary2("S1F6SFCD31").Item("MODULE_STATE1" + (dintLoop - 1)).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).EQPState);
                                msgTran.Secondary2("S1F6SFCD31").Item("PROC_STATE1" + (dintLoop - 1)).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).EQPProcessState);


                                InfoAct.clsSlot currentSlot = null;

                                if (!string.IsNullOrWhiteSpace(this.pInfo.Unit(dintLoop).SubUnit(0).HGLSID))//this.pInfo.Unit(dintLoop).SubUnit(0).GLSExist)
                                {
                                    InfoAct.clsGLS tmpGLS = this.pInfo.Unit(0).SubUnit(0).CurrGLS(this.pInfo.Unit(dintLoop).SubUnit(0).HGLSID);

                                    currentSlot = pInfo.LOTID(tmpGLS.LOTID).Slot(tmpGLS.SlotID);
                                }

                                msgTran.Secondary2("S1F6SFCD31").Item("CUR_STEPCNT1" + (dintLoop - 1)).Putvalue((currentSlot != null) ? 1 : 0);
                                msgTran.Secondary2("S1F6SFCD31").Item("CUR_GLSCNT1" + (dintLoop - 1)).Putvalue((currentSlot != null) ? 1 : 0);
                                msgTran.Secondary2("S1F6SFCD31").Item("CUR_M_CNT1" + (dintLoop - 1)).Putvalue((currentSlot != null) ? 1 : 0);

                                int dintStepNo = (currentSlot != null) ? currentSlot.StepNo : 0;

                                msgTran.Secondary2("S1F6SFCD31").Item("CUR_STEPNO1" + (dintLoop - 1)).Putvalue(dintStepNo);

                                if (dintStepNo != 0)
                                {
                                    msgTran.Secondary2("S1F6SFCD31").Item("STEPNO1" + (dintLoop - 1) + 0).Putvalue(dintStepNo);
                                    msgTran.Secondary2("S1F6SFCD31").Item("STEP_DESC1" + (dintLoop - 1) +0).Putvalue(this.pInfo.Unit(0).SubUnit(0).ProcessStep(dintStepNo).StepDesc);
                                    msgTran.Secondary2("S1F6SFCD31").Item("H_GLASSID1" + (dintLoop - 1) +0).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).HGLSID);
                                    msgTran.Secondary2("S1F6SFCD31").Item("MATERIAL_ID1" + (dintLoop - 1) +0).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).HGLSID);
                                    //msgTran.Secondary2("S1F6SFCD31").Item("STEP_DESC1" /*+ (dintLoop - 1)*/).Putvalue(this.pInfo.Unit(0).SubUnit(0).ProcessStep(dintStepNo).StepDesc);
                                    //msgTran.Secondary2("S1F6SFCD31").Item("H_GLASSID1" /*+ (dintLoop - 1)*/).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).HGLSID);
                                    //msgTran.Secondary2("S1F6SFCD31").Item("MATERIAL_ID1" /*+ (dintLoop - 1)*/).Putvalue(this.pInfo.Unit(dintLoop).SubUnit(0).HGLSID);
                                }

                                msgTran.Secondary2("S1F6SFCD31").Item("LAYER2CNT" + (dintLoop - 1)).Putvalue(0);
                            }

                            funSendReply2(msgTran, "S1F6SFCD31");
                        }
                        break;

                    default:   //잘못된 SFCD가 내려올 경우
                        {
                            msgTran.Secondary2("S1F6WrongMODID").Item("MODULEID").Putvalue(dstrModuleID);
                            msgTran.Secondary2("S1F6WrongMODID").Item("SFCD").Putvalue(dstrSFCD);

                            funSendReply2(msgTran, "S1F6WrongMODID");
                        }
                        break;
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
