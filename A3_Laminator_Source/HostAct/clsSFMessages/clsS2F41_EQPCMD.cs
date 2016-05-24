using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F41_EQPCMD : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F41_EQPCMD Instance = new clsS2F41_EQPCMD();
        #endregion

        #region Constructors
        public clsS2F41_EQPCMD()
        {
            this.IntStream = 2;
            this.IntFunction = 41;
            this.StrPrimaryMsgName = "S2F41EQPCMD";
            this.StrSecondaryMsgName = "S2F42EQPCMDREPLY";
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
            string dstrRCMD = "";
            int dintHCACK = 0;
            int dintCPACK = 0;
            string dstrData = "";
            Boolean dbolLayerExist = false;
            int dintModuleCount = 0;

            try
            {
                dstrRCMD = msgTran.Primary().Item("RCMD").Getvalue().ToString();
                dintModuleCount = Convert.ToInt32(msgTran.Primary().Item("MODULECOUNT").Getvalue());
                dstrModuleID = msgTran.Primary().Item("MODULEID" + 0).Getvalue().ToString().Trim();

                //RCMD가 존재하는 경우
                if (dstrRCMD == "51" || dstrRCMD == "52" || dstrRCMD == "53" || dstrRCMD == "54")
                {
                    //51(Pause), 52(Resume)은 Layer1도 대응하고 Layer2단이 오면 Layer1로 Pause 명령을 내린다.
                    //53(PM), 54(Normal)는 Layer1만 대응한다.(Layer2단 오면 NAK로 응답)
                    if (dstrRCMD == "51" || dstrRCMD == "52")  //51(Pause)은 Layer1도 대응하고 Layer2단이 오면 Layer1로 Pause 명령을 내린다.
                    {
                        //for (int dintUnit = 0; dintUnit <= pInfo.UnitCount; dintUnit++)
                        //{
                            if (dstrModuleID == pInfo.Unit(3).SubUnit(0).ModuleID)
                            {
                                dbolLayerExist = true;
                                //break;
                            }
                        //}
                    }
                    else   //53(PM), 54(Normal)는 Layer1만 대응한다.(Layer2단 오면 NAK로 응답)
                    {
                        if (dstrModuleID == pInfo.Unit(3).SubUnit(0).ModuleID)
                        {
                            dbolLayerExist = true;
                        }
                        else
                        {
                            dbolLayerExist = false;
                        }
                    }

                    //ModuleID가 존재하지 않는 경우
                    if (dbolLayerExist == false)
                    {
                        dintHCACK = 3;          //3 = At least one parameter invalid
                        dintCPACK = 2;          //2 = Illegal Value specified for CPVAL
                    }
                    else
                    {
                        switch (dstrRCMD)
                        {
                            case "51":   //Pause
                                {
                                    if (pInfo.Unit(0).SubUnit(0).EQPProcessState == "4")   //현재 Pause인데 Pause명령이 온 경우
                                    {
                                        dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                        dintCPACK = 0;              //0 = No Error
                                    }
                                }
                                break;
                            case "52":   //Resume
                                {
                                    if (pInfo.Unit(0).SubUnit(0).EQPState == "2")      //Fault인 경우
                                    {
                                        dintHCACK = 2;              //2 = Cannot perform now (Hardware Problem)* If equipment reply HCACK=2, then reason should be report as Alarm (S5F1)
                                        dintCPACK = 14;             //14 = Denied, Module State Error
                                    }
                                    else if (pInfo.Unit(0).SubUnit(0).EQPProcessState != "4" || pInfo.All.AutoMode == false)  //Pause 상태가 아닌데 Resume이 온 경우
                                    {
                                        dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                        dintCPACK = 0;              //0 = No Error
                                    }
                                    //else
                                    //{
                                    //    dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                    //    dintCPACK = 0;              //0 = No Error
                                    //}
                                    
                                }
                                break;
                            case "53":   //PM
                                {
                                    if (pInfo.Unit(0).SubUnit(0).EQPState == "2")      //Fault인 경우
                                    {
                                        dintHCACK = 2;              //2 = Cannot perform now (Hardware Problem)* If equipment reply HCACK=2, then reason should be report as Alarm (S5F1)
                                        dintCPACK = 14;             //14 = Denied, Module State Error
                                    }
                                    else if (pInfo.Unit(0).SubUnit(0).EQPState == "3")  //PM인데 PM 명령이 온 경우
                                    {
                                        dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                        dintCPACK = 0;              //0 = No Error
                                    }
                                }
                                break;
                            case "54":   //Normal
                                {
                                    if (pInfo.Unit(0).SubUnit(0).EQPState == "2")      //Fault인 경우
                                    {
                                        dintHCACK = 2;              //2 = Cannot perform now (Hardware Problem)* If equipment reply HCACK=2, then reason should be report as Alarm (S5F1)
                                        dintCPACK = 14;             //14 = Denied, Module State Error
                                    }
                                    else if (pInfo.Unit(0).SubUnit(0).EQPState == "1")          //Normal인데 Normal 명령이 온 경우
                                    {
                                        dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                        dintCPACK = 0;              //0 = No Error
                                    }
                                    else
                                    {
                                        //현재 장비에 Heavy Alarm이 있는데 HOST에서 Normal이 오면 Nak로 응답.       //SMD 정한모 요청   수정 : 이상호
                                        if (pInfo.All.AlarmExist == true)
                                        {
                                            foreach (int dintAlarm in pInfo.Unit(0).SubUnit(0).CurrAlarm())
                                            {
                                                if (pInfo.Unit(0).SubUnit(0).Alarm(dintAlarm).AlarmType == "H")
                                                {
                                                    dintHCACK = 2;              //2 = Cannot perform now (Hardware Problem), If equipment reply HCACK=2, then reason should be report as Alarm (S5F1)
                                                    dintCPACK = 14;              //14 = Denied, Module State Error
                                                }
                                                break;               //forEach문을 빠져나간다.
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }



                    if (dintHCACK == 0 && dintCPACK == 0)
                    {
                        //여기까지 오면 모두 OK인 경우임.  어경태 20080924일 Layer2 단위로 명령을 내리도록 수정
                        if (dstrRCMD == "51" || dstrRCMD == "52")
                        {
                            pInfo.Unit(0).SubUnit(0).EQPProcessStateChangeBYWHO = "1";    //BY HOST
                            for (int dintLoop = 0; dintLoop <= this.pInfo.UnitCount; dintLoop++)
                            {
                                //해당 UNIT이거나 전체 Unit일경우
                                if (dstrModuleID == pInfo.Unit(dintLoop).SubUnit(0).ModuleID || dstrModuleID == pInfo.Unit(0).SubUnit(0).ModuleID)
                                {
                                    pInfo.Unit(dintLoop).SubUnit(0).EQPProcessStateChangeBYWHO = "1";    //BY HOST

                                    if (dstrRCMD == "51")       //Pause
                                    {
                                        pInfo.Unit(dintLoop).SubUnit(0).EQPProcessStateLastCommand = "4";      //Pause가 왔음을 저장
                                    }
                                    else if (dstrRCMD == "52")  //Resume
                                    {
                                        pInfo.Unit(dintLoop).SubUnit(0).EQPProcessStateLastCommand = "8";      //Resume이 왔음을 저장(Resume은 8로 그냥 사용함)
                                    }

                                    if (dstrRCMD == "51")       //Pause
                                    {
                                        dstrData = "4";
                                    }
                                    else if (dstrRCMD == "52")  //Resume
                                    {
                                        dstrData = "8";
                                    }

                                    //Layer2에 해당하는 명령이 온 경우에만
                                    if (dstrModuleID == pInfo.Unit(dintLoop).SubUnit(0).ModuleID)
                                    {
                                        if (pInfo.Unit(0).SubUnit(0).EQPState == "3")          //PM인데 RESUME 명령이 온 경우  //091222 SMD 정한모 요청
                                        {
                                            dintHCACK = 5;              //5 = Rejected, Already in desired Condition
                                            dintCPACK = 0;              //0 = No Error
                                        }
                                        else
                                        {
                                            pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.EQPProcessState, dstrData, dintLoop);    //EQP Process State 변경요청을 PLC로 써준다.
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int dintLoop = 0; dintLoop <= pInfo.UnitCount; dintLoop++)
                            {
                                pInfo.Unit(dintLoop).SubUnit(0).EQPStateChangeBYWHO = "1";    //BY HOST

                                if (dstrRCMD == "53")       //PM
                                {
                                    pInfo.Unit(dintLoop).SubUnit(0).EQPStateLastCommand = "3";      //PM이 왔음을 저장
                                }
                                else if (dstrRCMD == "54")  //Normal
                                {
                                    pInfo.Unit(dintLoop).SubUnit(0).EQPStateLastCommand = "1";      //Normal이 왔음을 저장
                                }
                            }

                            if (dstrRCMD == "53")       //PM
                            {
                                dstrData = "3";
                                pInfo.All.PMCode = "9999";
                            }
                            else if (dstrRCMD == "54")  //Normal
                            {
                                dstrData = "1";
                            }
                            pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.EQPState, dstrData);    //EQP State 변경요청을 PLC로 써준다.
                        }
                    }

                }
                else if (dstrRCMD == "55" || dstrRCMD == "56")
                {
                    //사용안함.. 나중에 사용하게 되면 수정해야됨.
                    dintHCACK = 3;          //3 = At least one parameter invalid
                    dintCPACK = 0;          //0 = No Error
                }
                else   //RCMD가 존재하지 않는 경우
                {
                    //ModuleID가 존재하지 않는 경우
                    if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                    {
                        dintHCACK = 1;          //1 = Command does not exist
                        dintCPACK = 2;          //2 = Illegal Value specified for CPVAL
                    }
                    else   //ModuleID가 맞는 경우 (RCMD가 안맞는 경우)
                    {
                        dintHCACK = 3;          //3 = At least one parameter invalid
                        dintCPACK = 0;          //0 = No Error
                    }
                }

                //HOST로 S2F42 보고
                msgTran.Secondary2(StrSecondaryMsgName).Item("RCMD").Putvalue(dstrRCMD);
                msgTran.Secondary2(StrSecondaryMsgName).Item("HCACK").Putvalue(dintHCACK);
                msgTran.Secondary2(StrSecondaryMsgName).Item("MODULECOUNT").Putvalue(dintModuleCount);
                msgTran.Secondary2(StrSecondaryMsgName).Item("MODULEID1" + 0).Putvalue("MODULEID");
                msgTran.Secondary2(StrSecondaryMsgName).Item("MODULEID" + 0).Putvalue(dstrModuleID);
                msgTran.Secondary2(StrSecondaryMsgName).Item("CPACK" + 0).Putvalue(dintCPACK);

                funSendReply2(msgTran, strSecondaryMsgName);
            }
            catch (Exception error)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
