using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F13_LOTAPD : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F13_LOTAPD Instance = new clsS6F13_LOTAPD();
        #endregion

        #region Constructors
        public clsS6F13_LOTAPD()
        {
            this.IntStream = 6;
            this.IntFunction = 13;
            this.StrPrimaryMsgName = "S6F13LOTAPD";
            this.StrSecondaryMsgName = "S6F14";
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

                string dstrLOTID = arrayEvent[1].Trim();                                       //LOTID
                int dintSlotID = Convert.ToInt32(arrayEvent[2]);                     //SlotID
                string dstrHGLSID = arrayEvent[3].Trim();

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);      //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(91);       //Fixed Value
                pMsgTran.Primary().Item("RPTID").Putvalue(10);      //Fixed Value
                pMsgTran.Primary().Item("GLASSCOUNT").Putvalue(1);  //임시 20101018 어우수

                InfoAct.clsSlot currentSlot = this.pInfo.LOTID(dstrLOTID).Slot(dintSlotID);

                pMsgTran.Primary().Item("H_GLASSID" + 0).Putvalue(currentSlot.H_PANELID);
                pMsgTran.Primary().Item("E_GLASSID" + 0).Putvalue(currentSlot.E_PANELID);
                pMsgTran.Primary().Item("LOTID" + 0).Putvalue(currentSlot.LOTID);
                pMsgTran.Primary().Item("BATCHID" + 0).Putvalue(currentSlot.BATCHID);
                pMsgTran.Primary().Item("JOBID" + 0).Putvalue(currentSlot.JOBID);
                pMsgTran.Primary().Item("PORTID" + 0).Putvalue(currentSlot.PORTID);
                pMsgTran.Primary().Item("SLOTNO" + 0).Putvalue(currentSlot.SLOTNO);
                pMsgTran.Primary().Item("PROD_TYPE" + 0).Putvalue(currentSlot.PRODUCT_TYPE);
                pMsgTran.Primary().Item("PROD_KIND" + 0).Putvalue(currentSlot.PRODUCT_KIND);
                pMsgTran.Primary().Item("PRODUCTID" + 0).Putvalue(currentSlot.PRODUCTID);
                pMsgTran.Primary().Item("RUNSPECID" + 0).Putvalue(currentSlot.RUNSPECID);
                pMsgTran.Primary().Item("LAYERID" + 0).Putvalue(currentSlot.LAYERID);
                pMsgTran.Primary().Item("STEPID" + 0).Putvalue(currentSlot.STEPID);
                pMsgTran.Primary().Item("PPID" + 0).Putvalue(currentSlot.HOSTPPID);
                pMsgTran.Primary().Item("FLOWID" + 0).Putvalue(currentSlot.FLOWID);
                pMsgTran.Primary().Item("SIZE" + 0).Putvalue(currentSlot.SIZE);
                pMsgTran.Primary().Item("THICKNESS" + 0).Putvalue(currentSlot.THICKNESS);
                pMsgTran.Primary().Item("STATE" + 0).Putvalue(currentSlot.GLASS_STATE);
                pMsgTran.Primary().Item("ORDER" + 0).Putvalue(currentSlot.GLASS_ORDER);
                pMsgTran.Primary().Item("COMMENT" + 0).Putvalue(currentSlot.COMMENT);

                //LOT APD 추가
                pMsgTran.Primary().Item("RPTID1").Putvalue(9);      //Fixed Value
                pMsgTran.Primary().Item("MODULECNT").Putvalue(1);   //임시 20101018 어우수
                pMsgTran.Primary().Item("MODULEID" + 0).Putvalue(this.pInfo.Unit(0).SubUnit(0).ModuleID);

                int dintLotApdCount = this.pInfo.Unit(0).SubUnit(0).LOTAPDCount;
                pMsgTran.Primary().Item("DATACOUNT" + 0).Putvalue(dintLotApdCount);

                for (int dintLoop = 1; dintLoop <= dintLotApdCount; dintLoop++)
                {
                    InfoAct.clsLOTAPD lotApd = this.pInfo.LOTID(dstrLOTID).LOTAPD(dintLoop);
                    pMsgTran.Primary().Item("DATA_ITEM" + 0 + (dintLoop - 1)).Putvalue(lotApd.Name);
                    pMsgTran.Primary().Item("DATA_VALUE" + 0 + (dintLoop - 1)).Putvalue(lotApd.Value);
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
