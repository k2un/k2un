using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS7F101 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS7F101 Instance = new clsS7F101();
        #endregion

        #region Constructors
        public clsS7F101()
        {
            this.IntStream = 7;
            this.IntFunction = 101;
            this.StrPrimaryMsgName = "S7F101";
            this.StrSecondaryMsgName = "S7F102";
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
            int dintIndex = 0;

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dintPPIDType = Convert.ToInt32(msgTran.Primary().Item("PPID_TYPE").Getvalue());

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);
                    msgTran.Secondary().Item("PPIDCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                //PPID_TYPE이 틀린 경우(1 혹은 2가 아닌값)
                if (dintPPIDType == 1 || dintPPIDType == 2)
                {
                }
                else
                {
                    msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                    msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);
                    msgTran.Secondary().Item("PPIDCOUNT").Putvalue(0);

                    funSendReply(msgTran);

                    return;
                }

                //여기까지 오면 맞는 경우임
                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);
                msgTran.Secondary().Item("PPID_TYPE").Putvalue(dintPPIDType);

                

                if (dintPPIDType == 1)
                {
                    //설비로 부터 PPID List를 읽어온다.
                    pInfo.All.isReceivedFromHOST = true;                                            //HOST로 부터 S7F101를 받았음을 저장
                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, dintPPIDType);
                    pHost.subWaitDuringReadFromPLC();
                    pInfo.All.isReceivedFromHOST = false;                                           //HOST로 부터 S7F101를 받지 않았음을 저장(초기화)

                    msgTran.Secondary().Item("PPIDCOUNT").Putvalue(this.pInfo.Unit(0).SubUnit(0).EQPPPIDCount);

                    foreach (string dstrEQPPPID in this.pInfo.Unit(0).SubUnit(0).EQPPPID())
                    {
                        dintIndex = dintIndex + 1;  //보고 Index 증가
                        msgTran.Secondary().Item("PPID" + (dintIndex - 1)).Putvalue(dstrEQPPPID);
                    }
                }
                else if (dintPPIDType == 2)
                {
                    msgTran.Secondary().Item("PPIDCOUNT").Putvalue(this.pInfo.Unit(0).SubUnit(0).HOSTPPIDCount);

                    foreach (string dstrHOSTPPID in this.pInfo.Unit(0).SubUnit(0).HOSTPPID())
                    {
                        dintIndex = dintIndex + 1;  //보고 Index 증가
                        msgTran.Secondary().Item("PPID" + (dintIndex - 1)).Putvalue(dstrHOSTPPID);
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
