using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS1F17 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS1F17 Instance = new clsS1F17();
        #endregion

        #region Constructors
        public clsS1F17()
        {
            this.IntStream = 1;
            this.IntFunction = 17;
            this.StrPrimaryMsgName = "S1F17";
            this.StrSecondaryMsgName = "S1F18";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintCEID = 0;
            string dstrModuleID = "";
            string dstrMCMD = "";
            int dintACK = 0;

            try
            {
                dstrMCMD = msgTran.Primary().Item("MCMD").Getvalue().ToString().Trim();
                dstrModuleID = msgTran.Primary().Item("MODULEID").Getvalue().ToString().Trim();

                msgTran.Secondary().Item("MCMD").Putvalue(dstrMCMD);
                msgTran.Secondary().Item("MODULEID").Putvalue(dstrModuleID);

                if ((dstrModuleID != this.pInfo.Unit(3).SubUnit(0).ModuleID) ||
                    (dstrMCMD == "2"))
                {
                    msgTran.Secondary().Item("ONLACK").Putvalue(2);
                    funSendReply(msgTran);

                    return;
                }
                else if ((dstrMCMD != "2") &&
                    (dstrMCMD == this.pInfo.All.ControlState))
                {
                    msgTran.Secondary().Item("ONLACK").Putvalue(1);
                    funSendReply(msgTran);

                    return;
                }

                switch (dstrMCMD)
                {
                    case "1":           //OFFLine Request
                        {
                            dintACK = 0;
                            dintCEID = 71;
                        }
                        break;
                    case "3":           //Remote Request
                        {
                            dintACK = 0;
                            dintCEID = 73;
                        }
                        break;
                    default:
                        {
                            dintACK = 3;    //틀린 MCMD가 온경우
                        }
                        break;
                }

                //S1F18보고
                msgTran.Secondary().Item("ONLACK").Putvalue(dintACK);
                funSendReply(msgTran);

                //OFFLine, ONLine Remote로 변환 성공시 PLC에 써주고 HOST로 변경보고를 한다.
                if (dintACK == 0)
                {
                    pInfo.All.ControlstateChangeBYWHO = "1";    //By HOST
                    pInfo.All.ControlStateOLD = pInfo.All.ControlState;     //현재의 ControlState를 백업
                    pInfo.All.ControlState = dstrMCMD;
                    pInfo.All.WantControlState = "";    //초기화

                    //this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, PInfo.All.ControlState); //PLC로 현재 ControlState 를 써준다.

                    //HOST로 Online 변경 보고를 한다.(S6F11, CEID=71(OFFLine) / CEID=73(Remote))
                    this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, dintCEID, 3, 0);   //뒤에 0은 전체장비를 의미

                    if (dintCEID == 71)
                    {
                        this.pInfo.Unit(0).SubUnit(0).RemoveTRID();
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
