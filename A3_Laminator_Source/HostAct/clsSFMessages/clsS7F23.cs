using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS7F23 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS7F23 Instance = new clsS7F23();
        #endregion

        #region Constructors
        public clsS7F23()
        {
            this.IntStream = 7;
            this.IntFunction = 23;
            this.StrPrimaryMsgName = "S7F23";
            this.StrSecondaryMsgName = "S7F24";
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
            int dintBodyIndex = 0;
            string dstrTemp = "";
            string[] dstrValue;

            string dstrBeforeEQPPPID = "";
            int dintPPIDBodyCount = 0;
            int dintPPIDBodyID = 0;

            string dstrHOSTPPID = "";
            string dstrPPIDBody = "";
            //string dstrPPIDTime = "";
            string dstrEQPPPID = "";

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dstrReceivedPPID = msgTran.Primary().Item("PPID").Getvalue().ToString().Trim();
                dintPPIDType = Convert.ToInt32(msgTran.Primary().Item("PPID_TYPE").Getvalue());

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("ACKC7").Putvalue(7);
                    funSendReply(msgTran);

                    return;
                }

                //현재 장비가 RUN중이면
                if (((dstrReceivedPPID == pInfo.All.CurrentHOSTPPID) && (dintPPIDType == 2)) ||
                    ((dstrReceivedPPID == pInfo.All.CurrentEQPPPID) && (dintPPIDType == 1)))
                {
                    msgTran.Secondary().Item("ACKC7").Putvalue(1);
                    funSendReply(msgTran);

                    return;
                }

                //dintACK = 1;


                switch (dintPPIDType)
                {
                    case 1:
                        // 20121021 이상창
                        // 해당 레시피 없을경우 PLC와 레시피 동화 시도.
                        if (this.pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                        {
                            //if (this.pInfo.EQP("Main").RecipeCheck == true)
                            //{
                            this.pInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F101를 받았음을 저장
                            this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, dintPPIDType);      //현재 운영중인 PPID를 PLC로 부터 읽는다.
                            pHost.subWaitDuringReadFromPLC();
                            this.pInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F101를 받지 않았음을 저장(초기화)
                            //}
                        }

                        //EQP PPID 존재여부 체크
                        if (this.pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                        {
                            dintACK = 10;
                        }
                        else
                        {
                            if (pInfo.EQP("Main").DummyPLC)
                            {
                                if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                                {
                                    pInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrReceivedPPID);

                                    for (int dintLoop = 0; dintLoop < pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                                    {
                                        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).AddPPIDBody(dintLoop + 1);
                                        InfoAct.clsPPIDBody CurrentPPIDBody = pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop + 1);
                                        CurrentPPIDBody.DESC = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).DESC;
                                        CurrentPPIDBody.Format = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Format;
                                        CurrentPPIDBody.Index = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Index;
                                        CurrentPPIDBody.Length = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Length;
                                        CurrentPPIDBody.Max = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Max;
                                        CurrentPPIDBody.Min = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Min;
                                        CurrentPPIDBody.ModuleID = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).ModuleID;
                                        CurrentPPIDBody.Name = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Name;
                                        CurrentPPIDBody.Range = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Range;
                                        CurrentPPIDBody.Unit = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).Unit;
                                        CurrentPPIDBody.UseMode = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).UseMode;
                                        CurrentPPIDBody.Value = "0";
                                    }
                                }
                            }
                            else
                            {
                                this.pInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F101를 받았음을 저장
                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, dintPPIDType, dstrReceivedPPID, "");      //현재 운영중인 PPID를 PLC로 부터 읽는다.
                                pHost.subWaitDuringReadFromPLC();
                                this.pInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F101를 받지 않았음을 저장(초기화)
                            }
                        }

                        //PPID Body 개수 체크
                        dintPPIDBodyCount = Convert.ToInt32(msgTran.Primary().Item("PPARMCOUNT" + 0).Getvalue());
                        int dintCount = 0;
                        for (int dintLoop = 0; dintLoop < pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                        {
                            if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop + 1).UseMode)
                            {
                                dintCount++;
                            }
                        }

                        if (dintACK == 0 && dintPPIDBodyCount > dintCount)
                        {
                            dintACK = 3;
                        }

                        //PPID Body 범위 체크(NAK시 3으로 보냄)
                        if (dintACK == 0)
                        {
                            for (int dintLoop = 1; dintLoop <= dintPPIDBodyCount; dintLoop++)
                            {
                                //dintPPIDBodyID = funGetBodyNameToBodyID(msgTran.Primary().Item("P_PARM_NAME" + 0 + (dintLoop - 1)).Getvalue().ToString().Trim());
                                dintPPIDBodyID = (int)pInfo.Unit(0).SubUnit(0).pHashPPIDBodyName_GetIndex[msgTran.Primary().Item("P_PARM_NAME" + 0 + (dintLoop - 1)).Getvalue().ToString().Trim()];

                                if (dintPPIDBodyID == 0)  //Body Name이 존재하지 않는 경우 NAK
                                {
                                    //A2 사양에서 S7F24의 ACK Code가 변경됨. 기존에 쓰이던 3번항목(Matrix Overflow)이 삭제되었으나 대체할 Code가 없음. 협의 필요 20101018 어우수
                                    dintACK = 3;
                                    break;
                                }

                                dstrPPIDBody = msgTran.Primary().Item("P_PARM" + 0 + (dintLoop - 1)).Getvalue().ToString().Trim();

                                if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Length == 2)
                                {
                                    if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Min <= Convert.ToDouble(dstrPPIDBody) &&
                                        Convert.ToDouble(dstrPPIDBody) <= pInfo.Unit(0).SubUnit(0).PPIDBody(dintPPIDBodyID).Max)
                                    {
                                        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintPPIDBodyID).Value = dstrPPIDBody;
                                    }
                                    else
                                    {
                                        dintACK = 11;   //PPID Body 범위를 벗어난 경우(11 = P_PARM exceeds in defined range)
                                        break;
                                    }
                                }
                                else
                                {
                                    pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintPPIDBodyID).Value = dstrPPIDBody;
                                }
                            }
                        }

                        //여기까지 오면 정상임
                        if (dintACK == 0)
                        {
                            dstrHOSTPPID = "";
                            dstrPPIDBody = "";

                            //조금 전 위에서 PLC에서 읽은 Data를 구성한다.
                            int dintBodyCount = pInfo.Unit(0).SubUnit(0).PPIDBodyCount;
                            for (int dintLoop = 1; dintLoop <= dintBodyCount; dintLoop++)
                            {
                                dstrTemp = this.pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop).Value;
                                //if (this.pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintLoop).Length == 2)
                                //{
                                //    dstrTemp = FunStringH.funMakePLCData(FunStringH.funMakeRound(dstrTemp, this.pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format));
                                //}
                               
                                dstrPPIDBody = dstrPPIDBody + dstrTemp + ";";
                            }

                            dstrPPIDBody = dstrPPIDBody.Substring(0, dstrPPIDBody.Length - 1);

                            //PLC에서 읽은 Data 외에 HOST에서 받은(변경할) Data 저장
                            dstrValue = dstrPPIDBody.Split(new char[] { ';' });      //인자들을 분리한다.

                            for (int dintLoop = 1; dintLoop <= dintPPIDBodyCount; dintLoop++)
                            {
                                //dintBodyIndex = funGetBodyNameToBodyID(msgTran.Primary().Item("P_PARM_NAME" + 0 + (dintLoop - 1)).Getvalue().ToString().Trim());
                                dintBodyIndex = (int)pInfo.Unit(0).SubUnit(0).pHashPPIDBodyName_GetIndex[msgTran.Primary().Item("P_PARM_NAME" + 0 + (dintLoop - 1)).Getvalue().ToString().Trim()];

                                dstrTemp = msgTran.Primary().Item("P_PARM" + 0 + (dintLoop - 1)).Getvalue().ToString().Trim();
                                //if (this.pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID).PPIDBody(dintBodyIndex).Length == 2)
                                //{
                                //    dstrTemp = FunStringH.funMakePLCData(FunStringH.funMakeRound(dstrTemp, this.pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format));
                                //}
                           
                                dstrValue[dintBodyIndex - 1] = dstrTemp;
                            }

                            dstrPPIDBody = "";
                            for (int dintLoop = 0; dintLoop < dstrValue.Length; dintLoop++)
                            {
                                dstrPPIDBody = dstrPPIDBody + dstrValue[dintLoop] + ";";
                            }
                            dstrPPIDBody = dstrPPIDBody.Substring(0, dstrPPIDBody.Length - 1);
                            
                            //dstrPPIDTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            dintPPIDType = 1;   //EQP PPID임.

                            //장비로 EQP PPID Body 수정 지시(PPID Type=1)
                            //인자: HOSTPPID, EQPPPID, Body, 시간, Version, PPID Type
                            this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.PPIDBodyModify, dstrHOSTPPID, dstrReceivedPPID, dstrPPIDBody, dintPPIDType.ToString());
                        }
                        break;

                    case 2:
                        // 20121021 이상창
                        // 해당 레시피 없을경우 PLC와 레시피 동화 시도.
                        //if (this.pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                        //{
                        //    ////RecipeCheck = true일때만 PLC에서 Recipe 정보를 읽어서 동기화한다.
                        //    ////왜냐하면 Dummy로 테스트시 시간 단축 및 시나리오가 복잡해지기 때문임
                        //    //if (this.pInfo.EQP("Main").RecipeCheck == true)
                        //    //{
                        //        //this.pInfo.All.isReceivedFromHOST = true;    ///HOST로 부터 S7F101를 받았음을 저장
                        //        //this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, dintPPIDType);      //현재 운영중인 PPID를 PLC로 부터 읽는다.
                        //        //pHost.subWaitDuringReadFromPLC();
                        //        //this.pInfo.All.isReceivedFromHOST = false;    ///HOST로 부터 S7F101를 받지 않았음을 저장(초기화)
                        //    //}
                        //}

                        //HOST PPID 존재여부 체크
                        if (this.pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                        {
                            dintACK = 10;
                        }

                        //받은 HOST PPID에 Mapping할 EQPPPID가 존재하는지 체크
                        dstrEQPPPID = msgTran.Primary().Item("P_PARM" + 0 + 0).Getvalue().ToString().Trim();

                        if (dintACK == 0 && pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID) == null)
                        {
                            dintACK = 10;
                        }

                        //여기까지 오면 정상임
                        if (dintACK == 0)
                        {
                            //dstrBeforeEQPPPID = pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).EQPPPID;   //변경전 EQPPPID

                            //dstrPPIDTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            //dintPPIDType = 2;   //HOST PPID임.

                            //장비로 HOST PPID Mapping 변경 지시(PPID Type=2)
                            //인자: HOSTPPID, 변경전 EQPPPID, 변경후 EQPPPID, 시간)
                            //this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.HOSTPPIDMappingChange, dstrReceivedPPID, dstrEQPPPID);//, dstrPPIDTime);
                            this.pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID).EQPPPID = dstrEQPPPID;
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, "3", 2, dstrReceivedPPID, dstrEQPPPID);

                        }
                        break;

                    default:  //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                        {
                            dintACK = 8;
                        }
                        break;
                }
                

                msgTran.Secondary().Item("ACKC7").Putvalue(dintACK);
                funSendReply(msgTran);
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }
            finally
            {
                //PLC로 부터 리턴받은 값에 대해 사용된 변수 초기화
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
