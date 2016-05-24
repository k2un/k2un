using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F29 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F29 Instance = new clsS2F29();
        #endregion

        #region Constructors
        public clsS2F29()
        {
            this.IntStream = 2;
            this.IntFunction = 29;
            this.StrPrimaryMsgName = "S2F29";
            this.StrSecondaryMsgName = "S2F30";
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
            Boolean dbolLayerExist = false;
            int dintIndex = 0;

            try
            {
                
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                //for (int dintUnit = 0; dintUnit <= pInfo.UnitCount; dintUnit++)
                //{
                    if (dstrModuleID == pInfo.Unit(3).SubUnit(0).ModuleID)
                    {
                        dbolLayerExist = true;
                        //break;
                    }
                //}

                //ModuleID가 존재하지 않는 경우(Layer2도 비교한다.)
                if (dbolLayerExist == false)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("ECCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }
                dintECIDCount = Convert.ToInt32(msgTran.Primary().Item("ECIDCNT").Getvalue());
                ArrayList arrCon = new ArrayList();
                //받은 ECID중에 존재하지 않는것이 하나라도 있으면 L,0으로 보고한다.
                for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                {
                    dintECID = Convert.ToInt32(msgTran.Primary().Item("ECID" + (dintLoop - 1)).Getvalue());
                    if (pInfo.Unit(0).SubUnit(0).ECID(dintECID) == null)
                    {
                        msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                        msgTran.Secondary().Item("ECCOUNT").Putvalue(0);

                        funSendReply(msgTran);

                        return;
                    }

                    if (arrCon.Contains(dintECID) == true)
                    {
                        msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                        msgTran.Secondary().Item("ECCOUNT").Putvalue(0);

                        funSendReply(msgTran);

                        return;
                    }

                    arrCon.Add(dintECID);

                }

                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);

                //ECID를 PLC에서 읽는다.
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS2F29): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                if (pInfo.EQP("Main").DummyPLC == false)
                {
                    pInfo.All.isReceivedFromHOST = true;                            //HOST로 부터 S2F29를 받았음을 저장
                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ECIDRead);   //ECID를 PLC로 부터 읽는다.
                    pHost.subWaitDuringReadFromPLC();                               //바로 위에 PLC로 준 명령이 완료될때까지 이 함수에서 대기한다.
                    pInfo.All.isReceivedFromHOST = false;                           //HOST로 부터 S2F29를 받지 않았음을 저장(초기화)
                }
                //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS2F29): " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                //dstrModuleID = pInfo.Unit(0).SubUnit(0).ModuleID;
                //HOST로 부터 받은 List가 L, 0이면 모든 ECID 보고
                if (dintECIDCount == 0)
                {
                    int ecidCount = pInfo.Unit(0).SubUnit(0).ECIDCount;
                    msgTran.Secondary().Item("ECCOUNT").Putvalue(ecidCount);

                    for (int dintLoop = 1; dintLoop <= ecidCount; dintLoop++)
                    {
                        //ECID 중 사용하지 않는것(Spare)는 제외하고 보고한다.
                        if (pInfo.Unit(0).SubUnit(0).ECID(dintLoop).ModuleID.Contains(dstrModuleID) == true ) //&& pInfo.Unit(0).SubUnit(0).ECID(dintLoop).Use == true)
                        {
                            InfoAct.clsECID currentECID = pInfo.Unit(0).SubUnit(0).ECID(dintIndex + 1);

                            msgTran.Secondary().Item("ECID" + dintIndex).Putvalue(currentECID.Index);
                            msgTran.Secondary().Item("ECNAME" + dintIndex).Putvalue(currentECID.Name);
                            msgTran.Secondary().Item("ECDEF" + dintIndex).Putvalue(FunStringH.funPoint(currentECID.ECDEF, currentECID.Format));
                            msgTran.Secondary().Item("ECSLL" + dintIndex).Putvalue(FunStringH.funPoint(currentECID.ECSLL, currentECID.Format));
                            msgTran.Secondary().Item("ECSUL" + dintIndex).Putvalue(FunStringH.funPoint(currentECID.ECSUL, currentECID.Format));
                            msgTran.Secondary().Item("ECWLL" + dintIndex).Putvalue(FunStringH.funPoint(currentECID.ECWLL, currentECID.Format));
                            msgTran.Secondary().Item("ECWUL" + dintIndex).Putvalue(FunStringH.funPoint(currentECID.ECWUL, currentECID.Format));

                            dintIndex = dintIndex + 1;
                        }
                    }
                }
                else
                {
                    msgTran.Secondary().Item("ECCOUNT").Putvalue(dintECIDCount);

                    if (dstrModuleID == this.pInfo.Unit(0).SubUnit(0).ModuleID)         //Layer1단 모두 보고
                    {
                        for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                        {
                            dintECID = Convert.ToInt32(msgTran.Primary().Item("ECID" + (dintLoop - 1)).Getvalue());

                            InfoAct.clsECID currentECID = pInfo.Unit(0).SubUnit(0).ECID(dintECID);

                            msgTran.Secondary().Item("ECID" + (dintLoop - 1)).Putvalue(currentECID.Index);
                            msgTran.Secondary().Item("ECNAME" + (dintLoop - 1)).Putvalue(currentECID.Name);
                            msgTran.Secondary().Item("ECDEF" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECDEF, currentECID.Format));
                            msgTran.Secondary().Item("ECSLL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECSLL, currentECID.Format));
                            msgTran.Secondary().Item("ECSUL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECSUL, currentECID.Format));
                            msgTran.Secondary().Item("ECWLL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECWLL, currentECID.Format));
                            msgTran.Secondary().Item("ECWUL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECWUL, currentECID.Format));
                        }
                    }
                    else                                    //Layer2단 해당 되는것만 보고
                    {
                        for (int dintLoop = 1; dintLoop <= dintECIDCount; dintLoop++)
                        {
                            dintECID = Convert.ToInt32(msgTran.Primary().Item("ECID" + (dintLoop - 1)).Getvalue());

                            if (dstrModuleID != this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).ModuleID )
                            {
                                if (this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).ModuleID.Length > 13)
                                {
                                    if (dstrModuleID != this.pInfo.Unit(0).SubUnit(0).ECID(dintECID).ModuleID.Substring(0, 13))
                                    {
                                        dintECIDCount = 0;
                                        break;
                                    }
                                    else
                                    {
                                        InfoAct.clsECID currentECID = pInfo.Unit(0).SubUnit(0).ECID(dintECID);

                                        msgTran.Secondary().Item("ECID" + (dintLoop - 1)).Putvalue(currentECID.Index);
                                        msgTran.Secondary().Item("ECNAME" + (dintLoop - 1)).Putvalue(currentECID.Name);
                                        msgTran.Secondary().Item("ECDEF" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECDEF, currentECID.Format));
                                        msgTran.Secondary().Item("ECSLL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECSLL, currentECID.Format));
                                        msgTran.Secondary().Item("ECSUL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECSUL, currentECID.Format));
                                        msgTran.Secondary().Item("ECWLL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECWLL, currentECID.Format));
                                        msgTran.Secondary().Item("ECWUL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECWUL, currentECID.Format));
                                    }
                                }
                                else
                                {
                                    dintECIDCount = 0;
                                    break;
                                }
                            }
                            else
                            {
                                InfoAct.clsECID currentECID = pInfo.Unit(0).SubUnit(0).ECID(dintECID);

                                msgTran.Secondary().Item("ECID" + (dintLoop - 1)).Putvalue(currentECID.Index);
                                msgTran.Secondary().Item("ECNAME" + (dintLoop - 1)).Putvalue(currentECID.Name);
                                msgTran.Secondary().Item("ECDEF" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECDEF, currentECID.Format));
                                msgTran.Secondary().Item("ECSLL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECSLL, currentECID.Format));
                                msgTran.Secondary().Item("ECSUL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECSUL, currentECID.Format));
                                msgTran.Secondary().Item("ECWLL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECWLL, currentECID.Format));
                                msgTran.Secondary().Item("ECWUL" + (dintLoop - 1)).Putvalue(FunStringH.funPoint(currentECID.ECWUL, currentECID.Format));
                            }
                        }
                    }
                }

                
                funSendReply(msgTran);
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }
            finally
            {
                this.pInfo.All.isReceivedFromHOST = false;      //초기화
                this.pInfo.All.PLCActionEnd = false;            //초기화
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
