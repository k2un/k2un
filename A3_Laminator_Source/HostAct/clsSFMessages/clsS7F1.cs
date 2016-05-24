using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS7F1 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS7F1 Instance = new clsS7F1();
        #endregion

        #region Constructors
        public clsS7F1()
        {
            this.IntStream = 7;
            this.IntFunction = 1;
            this.StrPrimaryMsgName = "S7F1";
            this.StrSecondaryMsgName = "S7F2";
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
            string dstrPPID = "";
            int dintPPGNT = 0;

            try
            {
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dstrPPID = msgTran.Primary().Item("PPID").Getvalue().ToString().Trim();
                dintPPIDType = Convert.ToInt32(msgTran.Primary().Item("PPID_TYPE").Getvalue());

                //ModuleID가 존재하지 않는 경우
                if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("PPGNT").Putvalue(2);

                    funSendReply(msgTran);

                    return;
                }
                //사용안함

                if (dintPPIDType == 2)
                {
                    if (pInfo.All.CurrentHOSTPPID == dstrPPID)
                    {
                        dintPPGNT = 1;
                    }

                    //if (pInfo.EQP("Main").RecipeCheck == true)
                    //{
                    //    pInfo.All.isReceivedFromHOST = true;
                    //    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SetUpPPID, dintPPIDType);
                    //    pHost.subWaitDuringReadFromPLC();
                    //    pInfo.All.isReceivedFromHOST = false;
                    //}

                    if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrPPID) == null)
                    {
                        dintPPGNT = 5;
                    }
                }
                else
                {
                    dintPPGNT = 4;
                }

                if (dintPPGNT == 0)
                {
                    dintPPGNT = 7;       //7=Will not accept
                }

                msgTran.Secondary().Item("PPGNT").Putvalue(dintPPGNT);

                funSendReply(msgTran);

                //if(dintPPGNT == 0)
                //{
                //    pInfo.All.EQPSpecifiedCtrlBYWHO = "1";  //by Host
                //    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.CurrentPPIDChange, dstrPPID);
                //}
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
                return;
            }
            finally
            {
                //PLC로 부터 리턴받은 값에 대해 사용된 변수 초기화
                //this.PInfo.All.ReceivedFromHOST_HOSTPPIDExist = false;
                //this.PInfo.All.ReceivedFromHOST_EQPPPIDExist = false;

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
