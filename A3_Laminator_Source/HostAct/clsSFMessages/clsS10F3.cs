using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS10F3 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS10F3 Instance = new clsS10F3();
        #endregion

        #region Constructors
        public clsS10F3()
        {
            this.IntStream = 10;
            this.IntFunction = 3;
            this.StrPrimaryMsgName = "S10F3";
            this.StrSecondaryMsgName = "S10F4";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintTID = 0;
            string dstrMsg = "";
            string dstrDisplayMsg = "";
            string dstrModuleID = "";
            int dintMSGLineCount = 0;

            try
            {
                dintTID = Convert.ToInt32(msgTran.Primary().Item("TID").Getvalue());
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dintMSGLineCount = Convert.ToInt32(msgTran.Primary().Item("MSGCNT").Getvalue());

                for (int dintLoop = 1; dintLoop <= dintMSGLineCount; dintLoop++)
                {
                    dstrMsg += msgTran.Primary().Item("MSG" + (dintLoop - 1)).Getvalue().ToString();//.Trim();
                    dstrDisplayMsg += msgTran.Primary().Item("MSG" + (dintLoop - 1).ToString()).Getvalue().ToString() + "\r\n";
                }

                if (dstrModuleID == pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    switch (dintTID)
                    {
                        case 0:             //All(CIM, 장비 T/P 모두 띄운다.)		//SMD 박순종 : [Terminal MSG는 부저 울리지 않음.] 
                            {
                                this.pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, "[Terminal] " + dstrMsg);
                                this.pInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGNoBuzzer, 0, "S10F3", "[Terminal] " + dstrDisplayMsg);
                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg, "T");
                                funSetLog(InfoAct.clsInfo.LogType.OPCallMSG, "[Terminal]," + dstrMsg);  //20130128 이상창
                            }
                            break;
                        case 1:             //CIM PC
                            {
                                this.pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, "[Terminal] " + dstrMsg);
                                this.pInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGNoBuzzer, 0, "S10F3", "[Terminal] " + dstrDisplayMsg);
                                funSetLog(InfoAct.clsInfo.LogType.OPCallMSG, "[Terminal]," + dstrMsg);  //20130128 이상창
                            }
                            break;
                        case 2:             //장비 T/P 에만 띄운다.
                            {
                                this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg,"T");
                            }
                            break;
                    }
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
