using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS7F21 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS7F21 Instance = new clsS7F21();
        #endregion

        #region Constructors
        public clsS7F21()
        {
            this.IntStream = 7;
            this.IntFunction = 21;
            this.StrPrimaryMsgName = "S7F21";
            this.StrSecondaryMsgName = "S7F22";
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
            int dintPPIDType = 0;
            string dstrReceivedPPID = "";
            int dintACK = 0;
            int dintPPIDBodyCount = 0;
            int dintPPIDBodyID = 0;

            string dstrHOSTPPID = "";
            string dstrEQPPPID = "";
            string dstrPPIDBody = "";
            string dstrMode = "";
            bool dbochekFlag = false;
            Hashtable phashBodyList = new Hashtable();
            InfoAct.clsEQPPPID CreatePPID = null;
            string dstrTemp = "";

            try
            {
                dstrMode = msgTran.Primary().Item("MODE").Getvalue().ToString().Trim();
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dstrReceivedPPID = msgTran.Primary().Item("PPID").Getvalue().ToString().Trim();
                dintPPIDType = Convert.ToInt32(msgTran.Primary().Item("PPID_TYPE").Getvalue());

                if (dstrMode != "1" && dstrMode != "2")
                {
                    msgTran.Secondary().Item("ACKC7").Putvalue(1);
                    funSendReply(msgTran);

                    return;
                }

                //for (int dintLoop = 0; dintLoop <= pInfo.EQP("Main").UnitCount; dintLoop++)
                //{
                    //ModuleID가 존재하지 않는 경우
                    if (dstrModuleID == pInfo.Unit(3).SubUnit(0).ModuleID)
                    {
                        dbochekFlag = true;
                        //break;
                    }
                //}

                if (dbochekFlag == false)
                {
                    msgTran.Secondary().Item("ACKC7").Putvalue(8);
                    funSendReply(msgTran);
                    return;
                }

                if (dintPPIDType != 1 && dintPPIDType != 2)
                {
                    msgTran.Secondary().Item("ACKC7").Putvalue(9);
                    funSendReply(msgTran);
                    return;
                }
                else
                {
                    if(dstrMode == "1" || dstrMode =="2")
                    {
                        if (dstrMode == "1")
                        {
                            if (dintPPIDType == 1)
                            {

                                //msgTran.Secondary().Item("ACKC7").Putvalue(9);
                                //funSendReply(msgTran);
                                //return;

                                dstrEQPPPID = dstrReceivedPPID;
                                dintPPIDBodyCount = Convert.ToInt32(msgTran.Primary().Item("PPARMCOUNT" + 0).Getvalue().ToString().Trim());

                                if (dintPPIDBodyCount == 0)
                                {
                                    dintACK = 7; //The list of P_PARM is not enough
                                }
                                else if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID) != null)
                                {
                                    dintACK = 5; //Already Existing PPID
                                }
                                else
                                {
                                    dintACK = 3;
                                    #region 사용안함

                                    //string strPPIDBodyName = "";
                                    //CreatePPID = new InfoAct.clsEQPPPID(dstrEQPPPID);

                                    //for (int dintLoop = 0; dintLoop < dintPPIDBodyCount; dintLoop++)
                                    //{


                                    //    strPPIDBodyName = msgTran.Primary().Item("P_PARM_NAME" + 0 + (dintLoop)).Getvalue().ToString().Trim();
                                    //    if (phashBodyList.Contains(strPPIDBodyName) == false)
                                    //    {
                                    //        //dintPPIDBodyID = funGetBodyNameToBodyID(msgTran.Primary().Item("P_PARM_NAME" + 0 + (dintLoop)).Getvalue().ToString().Trim());
                                    //        dintPPIDBodyID = (int)pInfo.Unit(0).SubUnit(0).pHashPPIDBodyName_GetIndex[msgTran.Primary().Item("P_PARM_NAME" + 0 + dintLoop).Getvalue().ToString().Trim()];

                                    //        if (dintPPIDBodyID != 0)
                                    //        {
                                    //            dstrPPIDBody = msgTran.Primary().Item("P_PARM" + 0 + (dintLoop)).Getvalue().ToString().Trim();
                                    //            if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Length == 2)
                                    //            {
                                    //                if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Min <= Convert.ToDouble(dstrPPIDBody) &&
                                    //                    Convert.ToDouble(dstrPPIDBody) <= pInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Max)
                                    //                {
                                    //                    phashBodyList.Add(strPPIDBodyName, dstrPPIDBody);
                                    //                    CreatePPID.AddPPIDBody(dintPPIDBodyID);
                                    //                    CreatePPID.PPIDBody(dintPPIDBodyID).Name = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Name;
                                    //                    CreatePPID.PPIDBody(dintPPIDBodyID).Value = dstrPPIDBody;


                                    //                }
                                    //                else
                                    //                {
                                    //                    phashBodyList.Clear();
                                    //                    dintACK = 6; //Length Error
                                    //                    break;
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                phashBodyList.Add(strPPIDBodyName, dstrPPIDBody);
                                    //                CreatePPID.AddPPIDBody(dintPPIDBodyID);
                                    //                CreatePPID.PPIDBody(dintPPIDBodyID).Name = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Name;
                                    //                CreatePPID.PPIDBody(dintPPIDBodyID).Value = dstrPPIDBody;
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            phashBodyList.Clear();
                                    //            dintACK = 6; //Length Error
                                    //            break;
                                    //        }

                                    //    }
                                    //    else
                                    //    {
                                    //        dintACK = 7; //The list of P_PARM is not enough
                                    //        break;
                                    //    }
                                    //}
                                    #endregion
                                }
                            }
                            else
                            {
                                dstrHOSTPPID = dstrReceivedPPID;
                                dstrEQPPPID = msgTran.Primary().Item("P_PARM" + 0 + 0).Getvalue().ToString().Trim();
                                if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID) != null)
                                {
                                    dintACK = 5; //Already Existing PPID
                                }
                                else if (msgTran.Primary().Item("P_PARM_NAME" + 0 + 0).Getvalue().ToString().Trim() != "SUBPPID")
                                {
                                    dintACK = 11; //At least one of P_PARM exceeded its defined range
                                }
                                else if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID) == null)
                                {
                                    dintACK = 5; //Already Existing PPID
                                }
                            }
                        }
                        else
                        {
                            switch (dintPPIDType)
                            {
                                case 1:
                                    dstrEQPPPID = dstrReceivedPPID;
                                    if (pInfo.All.CurrentEQPPPID == dstrEQPPPID)
                                    {
                                        dintACK = 2; //Busy, not in the situation to accept this message
                                    }
                                    else if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID) == null)
                                    {
                                        dintACK = 4; //Not existing PPID
                                    }
                                    else
                                    {
                                        //bool dbolCheckFlag = false;

                                        //foreach (string strPPID in pInfo.Unit(0).SubUnit(0).HOSTPPID())
                                        //{
                                        //    if(pInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID).EQPPPID == dstrEQPPPID)
                                        //    {
                                        //        dbolCheckFlag = true;
                                        //        break;
                                        //    }
                                        //}

                                        //if (dbolCheckFlag)
                                        //{
                                        //    dintACK = 12; //Reserve
                                        //}
                                    }
                                    break;

                                case 2:
                                    dstrHOSTPPID = dstrReceivedPPID;
                                    if (pInfo.All.CurrentHOSTPPID == dstrHOSTPPID)
                                    {
                                        dintACK = 2; //Busy, not in the situation to accept this message
                                    }
                                    else if(pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID) == null)
                                    {
                                        dintACK = 4; //Not existing PPID
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        dintACK = 3; //Mode Unsupported
                    }
                }

                msgTran.Secondary().Item("ACKC7").Putvalue(dintACK);
                funSendReply(msgTran);

                if (dintACK == 0)
                {
                    //PPID Command 작업
                    if (dstrMode == "1") // 생성
                    {
                        if (dintPPIDType == 1)
                        {
                            if (CreatePPID != null)
                            {
                                pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.PPIDCreate, CreatePPID);
                            }
                            else
                            {
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "CreatePPID Null");
                            }
                        }
                        else
                        {
                            pInfo.Unit(0).SubUnit(0).AddHOSTPPID(dstrHOSTPPID);
                            pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).EQPPPID = dstrEQPPPID;
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, dstrMode, dstrHOSTPPID, dintPPIDType);
                        }
                    }
                    else //삭제
                    {
                        switch (dintPPIDType)
                        {
                            case 1:

                                pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.PPIDDelete, dstrEQPPPID);
                                break;

                            case 2:
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, dstrMode, dstrHOSTPPID, dintPPIDType);
                                pInfo.Unit(0).SubUnit(0).RemoveHOSTPPID(dstrHOSTPPID);
                                break;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }
            finally
            {
                //PLC로 부터 리턴받은 값에 대해 사용된 변수 초기화
                //pInfo.All.isReceivedFromHOST = false;      //초기화
                //pInfo.All.PLCActionEnd = false;            //초기화
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

        /// <summary>
        /// Host로 부터 내려온 Body Name이 맞는지 검사
        /// </summary>
        /// <param name="dstrName"> Body Name</param>
        /// <returns> Body ID </returns>
        private int funGetBodyNameToBodyID(string dstrName)
        {
            int dintBodyID = 0;

            try
            {
                foreach (int dintIndex in this.pInfo.Unit(0).SubUnit(0).PPIDBody())
                {
                    if (this.pInfo.Unit(0).SubUnit(0).PPIDBody(dintIndex).Name == dstrName)
                    {
                        dintBodyID = dintIndex;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dintBodyID;
        }
        #endregion
    }
}
