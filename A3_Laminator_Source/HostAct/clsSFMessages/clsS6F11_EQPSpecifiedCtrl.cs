using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_EQPSpecifiedCtrl : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_EQPSpecifiedCtrl Instance = new clsS6F11_EQPSpecifiedCtrl();
        #endregion

        #region Constructors
        public clsS6F11_EQPSpecifiedCtrl()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11EQPSpecifiedCtrl";
            this.StrSecondaryMsgName = "S6F12";
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

                int dintCEID = Convert.ToInt32(arrayEvent[1]);   //CEID

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);  //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(1);
                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(3).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(3).SubUnit(0).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(3).SubUnit(0).EQPProcessState);

                pMsgTran.Primary().Item("OPERID").Putvalue(pInfo.All.UserID);
                pMsgTran.Primary().Item("RPTID1").Putvalue(7);      //Fixed Value;


                switch (dintCEID)
                {
                    case 131:
                        {
                            pMsgTran.Primary().Item("BYWHO").Putvalue(pInfo.All.EQPSpecifiedCtrlBYWHO);

                            string dstrOLDHOSTPPID = arrayEvent[2];                   //OLD HOSTPPID임.
                            string dstrNEWHOSTPPID = arrayEvent[3];                   //NEW HOSTPPID임.

                            pMsgTran.Primary().Item("COUNT").Putvalue(2);
                            pMsgTran.Primary().Item("ITEM_NAME" + 0).Putvalue("OLDPPID");
                            pMsgTran.Primary().Item("ITEM_VALUE" + 0).Putvalue(dstrOLDHOSTPPID);
                            pMsgTran.Primary().Item("ITEM_NAME" + 1).Putvalue("NEWPPID");
                            pMsgTran.Primary().Item("ITEM_VALUE" + 1).Putvalue(dstrNEWHOSTPPID);
                        }
                        break;
                }

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
