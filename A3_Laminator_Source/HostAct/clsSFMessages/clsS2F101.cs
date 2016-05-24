using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F101 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F101 Instance = new clsS2F101();
        #endregion

        #region Constructors
        public clsS2F101()
        {
            this.IntStream = 2;
            this.IntFunction = 101;
            this.StrPrimaryMsgName = "S2F101";
            this.StrSecondaryMsgName = "S2F102";
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
            int dintAck = 0;
            string dstrModuleID = "";
            int dintMSGLineCount = 0;

            try
            {
                dintTID = Convert.ToInt32(msgTran.Primary().Item("TID").Getvalue());
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();
                dintMSGLineCount = Convert.ToInt32(msgTran.Primary().Item("MSGCNT").Getvalue());

                //ModuleID가 존재하지 않는 경우               
                if (dstrModuleID != pInfo.Unit(3).SubUnit(0).ModuleID)
                {
                    msgTran.Secondary().Item("ACKC2").Putvalue(1);
                    funSendReply(msgTran);

                    return;
                }

                for (int dintLoop = 1; dintLoop <= dintMSGLineCount; dintLoop++)
                {
                    dstrMsg += msgTran.Primary().Item("MSG" + (dintLoop - 1)).Getvalue().ToString();//.Trim();
                    dstrDisplayMsg += msgTran.Primary().Item("MSG" + (dintLoop - 1)).Getvalue().ToString() + "\r\n";
                }

                switch (dintTID)
                {
                    case 0:             //All(CIM, 장비 T/P 모두 띄운다.)
                        {
                            pInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGList, 0, "S2F101", "[OP Call] " + dstrDisplayMsg);
                            pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg, "O");
                            pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, "[OP Call] " + dstrMsg);    // 20120320 이상창    
                            funSetLog(InfoAct.clsInfo.LogType.OPCallMSG, "[OP Call]," + dstrMsg);  //20120320 이상창
							this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn); 

                            dintAck = 0;    //ACK
                        }
                        break;
                    case 1:             //CIM PC
                        {
                            pInfo.subOPCallOverWrite_Set(InfoAct.clsInfo.OPCallOverWrite.MSGList, 0, "S2F101", "[OP Call] " + dstrDisplayMsg);
                            pInfo.subMessage_Set(InfoAct.clsInfo.MsgType.HostMsg, "[OP Call] " + dstrMsg);    // 20120320 이상창
                            funSetLog(InfoAct.clsInfo.LogType.OPCallMSG, "[OP Call]," + dstrMsg);  //20120320 이상창

                            dintAck = 0;    //ACK
                        }
                        break;
                    case 2:             //장비 T/P에만 띄운다.
                        {
                            this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOn);          //T/P만 띄워도 부저는 울린다.       //수정 : 20100211 이상호
                            this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MessageSend, dstrMsg, "O");

                            dintAck = 0;    //ACK
                        }
                        break;


                    default:
                        {
                            dintAck = 2;    //NAK
                        }
                        break;
                }

                msgTran.Secondary().Item("ACKC2").Putvalue(dintAck);
                funSendReply(msgTran);
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
