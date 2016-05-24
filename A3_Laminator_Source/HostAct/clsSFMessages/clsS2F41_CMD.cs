using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS2F41_CMD : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS2F41_CMD Instance = new clsS2F41_CMD();
        #endregion

        #region Constructors
        public clsS2F41_CMD()
        {
            this.IntStream = 2;
            this.IntFunction = 41;
            this.StrPrimaryMsgName = "S2F41_CMD";
            this.StrSecondaryMsgName = "S2F42";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            string dstrRCMD = "";
            string dstrCSTID = "";
            string dstrLOTID = "";
            string dstrPortID = "";
            int dintHCACK = 0;
            int dintPortID =0;
      
            try
            {
                dstrRCMD = msgTran.Primary().Item("RCMD").Getvalue().ToString();
                dstrCSTID = msgTran.Primary().Item("CSTID").Getvalue().ToString();
                dstrLOTID = msgTran.Primary().Item("LOTID").Getvalue().ToString();
                dstrPortID = msgTran.Primary().Item("PTID").Getvalue().ToString();
                int.TryParse(dstrPortID.Substring(1), out dintPortID);

                if (pInfo.Port(dintPortID).LotInfoFlag == false)
                {
                    if (dstrRCMD == "1")
                    {
                        dintHCACK = 1; //PTID is Invalid
                    }
                }
                else
                {
                    if (pInfo.Port(dintPortID).CSTID.Trim() != dstrCSTID.Trim())
                    {
                        dintHCACK = 2;//CSTID is invalid
                    }
                    else
                    {
                        if (pInfo.Port(dintPortID).LOTID.Trim() != dstrLOTID.Trim())
                        {
                            dintHCACK = 3;//LOTID is invalid
                        }
                    }
                }

                if (dintHCACK == 0)
                {
                    if (dstrRCMD == "1")
                    {
                        pInfo.Port(dintPortID).bolHostCommandCheck = false;

                        //Start
                        pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.PortCMD, dstrPortID, dstrCSTID, dstrLOTID, dstrRCMD);
                        pInfo.Port(dintPortID).CmdFlag = true; //명령 받음

                    }
                    else if (dstrRCMD == "2")
                    {
                        pInfo.Port(dintPortID).bolHostCommandCheck = false;
                        pInfo.Port(dintPortID).bolLotInfoDownloadCheck = false;
                        //cancel
                        pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.PortCMD, dstrPortID, dstrCSTID, dstrLOTID, dstrRCMD);

                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ProcessReport, 304, 0, dintPortID, "", ""); //Process Cancel
                        pInfo.Port(dintPortID).CmdFlag = true;//명령 받음
                    }
                    else
                    {
                        dintHCACK = 4; //Command does not exist.
                    }

                }


                msgTran.Secondary().Item("RCMD").Putvalue(dstrRCMD);
                msgTran.Secondary().Item("HCACK").Putvalue(dintHCACK);
                funSendReply(msgTran);
            }
            catch (Exception error)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
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
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
