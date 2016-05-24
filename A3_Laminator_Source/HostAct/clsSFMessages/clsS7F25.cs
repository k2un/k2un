using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS7F25 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS7F25 Instance = new clsS7F25();
        #endregion

        #region Constructors
        public clsS7F25()
        {
            this.IntStream = 7;
            this.IntFunction = 25;
            this.StrPrimaryMsgName = "S7F25";
            this.StrSecondaryMsgName = "S7F26";
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
            string dstrReceivedPPID = "";
            int dintPPIDType = 0;
            Boolean dbolPPIDNotExist = false;
            int dintEQPPIDCount = 0;        //HOST PPID에 Mapping되어있는 EQPPID의 개수

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dstrReceivedPPID = msgTran.Primary().Item("PPID").Getvalue().ToString().Trim();
                dintPPIDType = Convert.ToInt32(msgTran.Primary().Item("PPID_TYPE").Getvalue());

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("PPID").Putvalue(dstrReceivedPPID);
                    msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);
                    msgTran.Secondary().Item("COMMANDCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                if (dintPPIDType == 1)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SetUpPPID Start :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                    pInfo.All.isReceivedFromHOST = true;                                            //HOST로 부터 S7F101를 받았음을 저장
                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, dintPPIDType);
                    pHost.subWaitDuringReadFromPLC();
                    pInfo.All.isReceivedFromHOST = false;                                           //HOST로 부터 S7F101를 받지 않았음을 저장(초기화)
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SetUpPPID Start :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                    if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                    {
                        dbolPPIDNotExist = true;
                    }

                }
                else if (dintPPIDType == 2)
                {
                    //if (this.pInfo.EQP("Main").RecipeCheck == true)
                    
                    if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                    {
                        //설비로 부터 PPID List를 읽어온다.
                                                                 //HOST로 부터 S7F101를 받지 않았음을 저장(초기화)
                    }

                    if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null) dbolPPIDNotExist = true;
                }
                else
                {
                    dbolPPIDNotExist = true;
                }

                //PPID가 존재하지 않는 경우나 PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dbolPPIDNotExist == true)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("PPID").Putvalue(dstrReceivedPPID);
                    msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);
                    msgTran.Secondary().Item("COMMANDCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }
                else
                {
                    if (dintPPIDType == 1)
                    {
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM,"OneppidRead Start :"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        pInfo.All.isReceivedFromHOST = true;                                            //HOST로 부터 S7F101를 받았음을 저장
                        pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrReceivedPPID, "");
                        //[2015/04/20]임시add by HS)
                        funSetLog(InfoAct.clsInfo.LogType.CIM, "subWaitDuringReadFromPLC Start. Time" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        pHost.subWaitDuringReadFromPLC();
                        //[2015/04/20]임시add by HS)
                        funSetLog(InfoAct.clsInfo.LogType.CIM, "subWaitDuringReadFromPLC End. Time" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        pInfo.All.isReceivedFromHOST = false;
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "OneppidRead END :" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        ////pInfo.Unit(0).SubUnit(0).MappingEQPPPID(dstrReceivedPPID).PPIDCommand = InfoAct.clsInfo.PPIDCMD.Search;
                        ////pInfo.SetPPIDCMD(dstrReceivedPPID);
                        ////pHost.subWaitPPIDSearch();

                        //여기까지 오면 맞는 경우임
                        msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                        msgTran.Secondary().Item("PPID").Putvalue(dstrReceivedPPID);
                        msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);
                        msgTran.Secondary().Item("COMMANDCOUNT").Putvalue(1);
                        
                        msgTran.Secondary().Item("CCODE" + 0).Putvalue(0);
                        //msgTran.Secondary().Item("PPARMCOUNT" + 0).Putvalue(pInfo.Unit(0).SubUnit(0).PPIDBodyCount);

                        //int dintPPIDBodyCount = pInfo.Unit(0).SubUnit(0).PPIDBodyCount;
                        //for (int dintLoop = 1; dintLoop <= dintPPIDBodyCount; dintLoop++)
                        //{
                        //    InfoAct.clsPPIDBody currentPPIDBody = pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop);

                        //    msgTran.Secondary().Item("P_PARM_NAME" + 0 + (dintLoop - 1)).Putvalue(currentPPIDBody.Name);
                        //    msgTran.Secondary().Item("P_PARM" + 0 + (dintLoop - 1)).Putvalue(currentPPIDBody.Value);
                        //}@

                        int dintPPIDBodyCount = 0;
                        for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                        {
                            InfoAct.clsPPIDBody currentPPIDBody = pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop);
                            if (currentPPIDBody.UseMode)
                            {
                                dintPPIDBodyCount++;
                            }
                        }

                        msgTran.Secondary().Item("PPARMCOUNT" + 0).Putvalue(dintPPIDBodyCount);

                        int dintCount = 0;
                        for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                        {
                            InfoAct.clsPPIDBody currentPPIDBody = pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop);
                            if (currentPPIDBody.UseMode)
                            {
                                msgTran.Secondary().Item("P_PARM_NAME" + 0 + dintCount).Putvalue(currentPPIDBody.Name);
                                msgTran.Secondary().Item("P_PARM" + 0 + dintCount).Putvalue(currentPPIDBody.Value);
                                dintCount++;
                            }
                        }

                        //string UP_EQPPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(dstrReceivedPPID).UP_EQPPPID;
                        //string LOW_EQPPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(dstrReceivedPPID).LOW_EQPPPID;

                        //for (int dintLoop = 1; dintLoop <= dintPPIDBodyCount; dintLoop++)
                        //{
                        //    InfoAct.clsPPIDBody currentPPIDBody = pInfo.Unit(0).SubUnit(0).EQPPPID(UP_EQPPPID).PPIDBody(dintLoop);
                        //    msgTran.Secondary().Item("P_PARM_NAME" + 0 + (dintLoop - 1)).Putvalue("UPPER_" + currentPPIDBody.Name);
                        //    msgTran.Secondary().Item("P_PARM" + 0 + (dintLoop - 1)).Putvalue(currentPPIDBody.Value);
                        //}

                        //for (int dintLoop = 1; dintLoop <= dintPPIDBodyCount; dintLoop++)
                        //{
                        //    InfoAct.clsPPIDBody currentPPIDBody = pInfo.Unit(0).SubUnit(0).EQPPPID(LOW_EQPPPID).PPIDBody(dintLoop);
                        //    msgTran.Secondary().Item("P_PARM_NAME" + 0 + (dintLoop - 1 + dintPPIDBodyCount)).Putvalue("LOWER_" + currentPPIDBody.Name);
                        //    msgTran.Secondary().Item("P_PARM" + 0 + (dintLoop - 1 + dintPPIDBodyCount)).Putvalue(currentPPIDBody.Value);
                        //}
                    }
                    else
                    {
                        //여기까지 오면 맞는 경우임
                        msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                        msgTran.Secondary().Item("PPID").Putvalue(dstrReceivedPPID);
                        msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);
                        msgTran.Secondary().Item("COMMANDCOUNT").Putvalue(1);

                        //HOST PPID에 Mapping되어있는 장비 PPID의 개수를 알아온다.
                        foreach (string dstrHOSTPPID in this.pInfo.Unit(0).SubUnit(0).HOSTPPID())
                        {
                            if (dstrReceivedPPID == dstrHOSTPPID && this.pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).EQPPPID != "")
                            {
                                dintEQPPIDCount = dintEQPPIDCount + 1;
                            }
                        }

                        msgTran.Secondary().Item("CCODE" + (dintEQPPIDCount - 1)).Putvalue(0);
                        msgTran.Secondary().Item("PPARMCOUNT" + (dintEQPPIDCount - 1)).Putvalue(dintEQPPIDCount);

                        for (int dintLoop = 1; dintLoop <= dintEQPPIDCount; dintLoop++)
                        {
                            msgTran.Secondary().Item("P_PARM_NAME" + (dintEQPPIDCount - 1) + (dintLoop - 1)).Putvalue("SUBPPID");
                            msgTran.Secondary().Item("P_PARM" + (dintEQPPIDCount - 1) + (dintLoop - 1)).Putvalue(pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).EQPPPID);
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
                pInfo.All.isReceivedFromHOST = false;      //초기화
                pInfo.All.PLCActionEnd = false;            //초기화
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
