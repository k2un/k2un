using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS1F15 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS1F15 Instance = new clsS1F15();
        #endregion

        #region Constructors
        public clsS1F15()
        {
            this.IntStream = 1;
            this.IntFunction = 15;
            this.StrPrimaryMsgName = "S1F15";
            this.StrSecondaryMsgName = "S1F16";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintACK = 0;
            int dintCEID = 0;
            try
            {

                if (pInfo.All.ControlState == "0")
                {
                    dintACK = 1;
                }
                else
                {
                    pInfo.All.ControlStateOLD = pInfo.All.ControlState;
                    dintACK = 0;
                    dintCEID = 111;
                    pInfo.All.ControlState = "0";
                }

                
                //S1F16보고
                msgTran.Secondary().Item("OFLACK").Putvalue(dintACK);
                funSendReply(msgTran);

                if (dintACK == 0)
                {
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, dintCEID, 0);
                    if (pInfo.All.ControlStateOLD == "0")
                    {
                        for (int dintLoop = 0; dintLoop < pInfo.PortCount(); dintLoop++)
                        {
                            if (pInfo.Port(dintLoop + 1).LOTST == "1")
                            {
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_PortOccupationStatusChange, 200, dintLoop + 1);

                            }
                        }
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
