using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_RelatedProcessStatusEvent : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_RelatedProcessStatusEvent Instance = new clsS6F11_RelatedProcessStatusEvent();
        #endregion

        #region Constructors
        public clsS6F11_RelatedProcessStatusEvent()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11RelatedProcessStatusEvent";
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

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                int dintCEID = Convert.ToInt32(arrayEvent[1]);                       //CEID
                if (dintCEID != 23) return null;

                int dintUnitID = Convert.ToInt32(arrayEvent[2]);                     //UnitID
                string dstrLOTID = arrayEvent[3];                                    //LOTID
                int dintSlotID = Convert.ToInt32(arrayEvent[4]);                     //SlotID

                InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);  //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);

                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState);

                pMsgTran.Primary().Item("STEPNO").Putvalue(currentSlot.StepNo);
                pMsgTran.Primary().Item("PREV_STEPNO").Putvalue(currentSlot.StepNo_OLD);

                pMsgTran.Primary().Item("L2_3").Putvalue(0); // Fixed Value For CEID=23;

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
