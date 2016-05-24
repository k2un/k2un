using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11CSTMovingEvent : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11CSTMovingEvent Instance = new clsS6F11CSTMovingEvent();
        #endregion

        #region Constructors
        public clsS6F11CSTMovingEvent()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11CSTMovingEvent";
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
            int dintPortID = 0;
            string strType = "";
            string dstrCaseID = "";
            int dintUnitID = 0;
            int dintSubUnitID = 0;

            try
            {
                arrayEvent = strParameters.Split(',');

                int dintCEID = Convert.ToInt32(arrayEvent[1]);   //CEID
                dintPortID = Convert.ToInt32(arrayEvent[3]);    //UnitID
                strType = arrayEvent[2];
                dstrCaseID = arrayEvent[4];
                dintUnitID = Convert.ToInt32(arrayEvent[5]);
                dintSubUnitID = Convert.ToInt32(arrayEvent[6]);

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);  //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(1);
                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(0).SubUnit(0).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(0).SubUnit(0).EQPProcessState);
                pMsgTran.Primary().Item("BYWHO").Putvalue("3");
                pMsgTran.Primary().Item("OPERID").Putvalue(pInfo.All.UserID);

                pMsgTran.Primary().Item("RPTID1").Putvalue(2);      //Fixed Value;
                pMsgTran.Primary().Item("PORTID").Putvalue(pInfo.Port(dintPortID).HostReportPortID);
                switch (dintCEID)
                {
                    case 45:
                        pInfo.Port(dintPortID).PortState = "5";
                        break;
                }
                pMsgTran.Primary().Item("PORT_STATE").Putvalue(pInfo.Port(dintPortID).PortState);
                pMsgTran.Primary().Item("PORT_TYPE").Putvalue("2");
                pMsgTran.Primary().Item("PORT_MODE").Putvalue("OK");
                pMsgTran.Primary().Item("SORT_TYPE").Putvalue("0");

                pMsgTran.Primary().Item("RPTID2").Putvalue(3);      //Fixed Value;
                pMsgTran.Primary().Item("CSTID").Putvalue(pInfo.Port(dintPortID).CSTID);
                pMsgTran.Primary().Item("CSTTYPE").Putvalue(12);
                pMsgTran.Primary().Item("MAP_STIF").Putvalue(pInfo.Port(dintPortID-1).HostReportPortID);
                pMsgTran.Primary().Item("CUR_STIF").Putvalue("");
                pMsgTran.Primary().Item("BATCH_ORDER").Putvalue("");

            
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
