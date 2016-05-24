using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS7F103 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS7F103 Instance = new clsS7F103();
        #endregion

        #region Constructors
        public clsS7F103()
        {
            this.IntStream = 7;
            this.IntFunction = 103;
            this.StrPrimaryMsgName = "S7F103";
            this.StrSecondaryMsgName = "S7F104";
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
            int dintACK = 0;
            string dstrReceivedPPID = "";

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dstrReceivedPPID = msgTran.Primary().Item("PPID").Getvalue().ToString().Trim();
                dintPPIDType = Convert.ToInt32(msgTran.Primary().Item("PPID_TYPE").Getvalue());

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("ACKC7").Putvalue(1);
                    funSendReply(msgTran);

                    return;
                }

                //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dintPPIDType == 1 || dintPPIDType == 2)
                {
                    //RecipeCheck = true일때만 PLC에서 Recipe 정보를 읽어서 동기화한다.
                    //왜냐하면 Dummy로 테스트시 시간 단축 및 시나리오가 복잡해지기 때문임
                    //if (pInfo.EQP("Main").RecipeCheck == true)
                    //{
                    //    //PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "시작(subS7F101)-EQP, HOST PPID Set up: " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    //    pInfo.All.isReceivedFromHOST = true;                                           //HOST로 부터 S7F101를 받았음을 저장
                    //    //PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID);
                    //    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, dintPPIDType);   //현재 운영중인 PPID를 PLC로 부터 읽는다.
                    //    pHost.subWaitDuringReadFromPLC();
                    //    pInfo.All.isReceivedFromHOST = false;                                          //HOST로 부터 S7F101를 받지 않았음을 저장(초기화)
                    //    //PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "종료(subS7F101)-EQP, HOST PPID Set up: " + DateTime.Now.ToString("yyyyMMddHHmmss") + ": " + DateTime.Now.Millisecond.ToString());
                    //}

                    if (dintPPIDType == 1)
                    {
                        if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrReceivedPPID) == null)
                        {
                            dintACK = 3;        //3=PPID is not match
                        }
                    }
                    else if (dintPPIDType == 2)
                    {
                        if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrReceivedPPID) == null)
                        {
                            dintACK = 3;        //3=PPID is not match
                        }
                    }
                }
                else
                {
                    dintACK = 2;
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
