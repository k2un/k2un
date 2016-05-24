using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS1F1 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS1F1 Instance = new clsS1F1();
        #endregion

        #region Constructors
        public clsS1F1()
        {
            this.IntStream = 1;
            this.IntFunction = 1;
            this.StrPrimaryMsgName = "AreYouThere";
            this.StrSecondaryMsgName = "S1F2";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            try
            {
                msgTran.Secondary().Item("VERSION").Putvalue(pInfo.All.SoftVersion);
                msgTran.Secondary().Item("SPEC_CODE").Putvalue(pInfo.All.SpecCode);     //2012.09.19 Kim Youngsik for SMD A3,V1
                msgTran.Secondary().Item("MODULEID").Putvalue(pInfo.Unit(3).SubUnit(0).ModuleID);
                msgTran.Secondary().Item("MCMD").Putvalue(pInfo.All.ControlState);

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
                if (pInfo.All.WantControlState == "")
                {
                    return;
                }
                pInfo.All.ONLINEModeChange = false;
                pInfo.All.ControlStateOLD = pInfo.All.ControlState;     //현재의 ControlState를 백업
                pInfo.All.ControlState = pInfo.All.WantControlState;

                pInfo.subOPCall_Set(InfoAct.clsInfo.OPCall.FormClose, 0, "", "");      //Mode Change Form을 닫는다

                //pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, pInfo.All.ControlState);

                if (pInfo.All.ControlState == "2")
                {
                    //HOST로 Online 변경 보고를 한다.(S6F11, CEID=73(Remote) 보고)
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 72, 0, 0);   //뒤에 0은 전체장비를 의미
                }
                else if (pInfo.All.ControlState == "3")
                {
                    //HOST로 Online 변경 보고를 한다.(S6F11, CEID=73(Remote) 보고)
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 73, 3, 0);   //뒤에 0은 전체장비를 의미
                    //SEM Data Send Time 초기화 - 12.04.04 ksh
                    pInfo.All.FDCDataSendTime = DateTime.Now;
                }

                pInfo.All.WantControlState = "";   //S6F11 보고 후 초기화
            }
            catch (Exception error)
            {
                funSetLog(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion
    }
}
