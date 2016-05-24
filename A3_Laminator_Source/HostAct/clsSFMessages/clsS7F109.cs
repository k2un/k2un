using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS7F109 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS7F109 Instance = new clsS7F109();
        #endregion

        #region Constructors
        public clsS7F109()
        {
            this.IntStream = 7;
            this.IntFunction = 109;
            this.StrPrimaryMsgName = "S7F109";
            this.StrSecondaryMsgName = "S7F109";
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
            string dstrPPID = "";
            int dintPPIDType = 0;
            int dintAck = 0;

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dintPPIDType = Convert.ToInt32(msgTran.Primary().Item("PPID_TYPE").Getvalue());

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("ACKC7").Putvalue(1);
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("PPID").Putvalue("");
                    msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);

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
                    //    pInfo.All.isReceivedFromHOST = true;                                                                    //HOST로 부터 S7F109를 받았음을 저장
                    //    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.CurrentRunningPPPIDReadFromHOST, dintPPIDType);      //현재 운영중인 PPID를 PLC로 부터 읽는다.
                    //    pHost.subWaitDuringReadFromPLC();
                    //    pInfo.All.isReceivedFromHOST = false;                                                                   //HOST로 부터 S7F109를 받지 않았음을 저장(초기화)
                    //}

                    if (dintPPIDType == 1)   //EQPPID
                    {
                        dstrPPID = this.pInfo.All.CurrentEQPPPID;
                    }
                    else if (dintPPIDType == 2) //HOSTPPID
                    {
                        dstrPPID = this.pInfo.All.CurrentHOSTPPID;
                    }

                    dintAck = 0;
                }
                else
                {
                    dstrPPID = "";
                    dintAck = 2;
                }


                if (dstrPPID == "0") dstrPPID = "";
                msgTran.Secondary().Item("ACKC7").Putvalue(dintAck);
                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                msgTran.Secondary().Item("PPID").Putvalue(dstrPPID);
                msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);

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
