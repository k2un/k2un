using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_MaterialStockEvent : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_MaterialStockEvent Instance = new clsS6F11_MaterialStockEvent();
        #endregion

        #region Constructors
        public clsS6F11_MaterialStockEvent()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11MaterialStockEvent";
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
            int dintUnitID = 0;
            int dintCount = 0;
            string dstrFilmID = "";
            int dintSlotID = 0;

            try
            {
                arrayEvent = strParameters.Split(',');

                int dintCEID = Convert.ToInt32(arrayEvent[1]);   //CEID
                dintUnitID = Convert.ToInt32(arrayEvent[2]);    //UnitID
                dstrFilmID = arrayEvent[3];
                dintSlotID = Convert.ToInt32(arrayEvent[4]);

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);  //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(1);   //Fixed Value
                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).EQPProcessState);
                pMsgTran.Primary().Item("BYWHO").Putvalue(3);
                pMsgTran.Primary().Item("OPERID").Putvalue(this.pInfo.All.UserID);

                pMsgTran.Primary().Item("MODULEID2").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).ModuleID);     //Fixed Value
                pMsgTran.Primary().Item("COUNT").Putvalue(1);  //임시 20101015 어우수


                pMsgTran.Primary().Item("MATERIAL_ID" + 0).Putvalue(dstrFilmID);
                pMsgTran.Primary().Item("PROD_TYPE" + 0).Putvalue("");
                pMsgTran.Primary().Item("LIBRARYID" + 0).Putvalue("");
                pMsgTran.Primary().Item("STAGE_STATE" + 0).Putvalue("");
                if (dintCEID == 201 || dintCEID == 203)
                {
                    pMsgTran.Primary().Item("MATERIAL_STATE" + 0).Putvalue("1");
                }
                else
                {
                    pMsgTran.Primary().Item("MATERIAL_STATE" + 0).Putvalue("4");
                }

                pMsgTran.Primary().Item("LOCATION" + 0).Putvalue("");
                //string[] darrSize = currentSlot.SIZE.Split(' ');
                pMsgTran.Primary().Item("MATERIAL_SIZE" + 0).Putvalue("");

                pMsgTran.Primary().Item("L4" + 0).Putvalue(1);

                if (pInfo.Unit(8).SubUnit(0).CurrGLSCount > 0)
                {
                    InfoAct.clsGLS CurrentGLS = pInfo.Unit(8).SubUnit(0).CurrGLS(pInfo.Unit(8).SubUnit(0).HGLSID);

                    if (pInfo.LOTID(CurrentGLS.LOTID) != null)
                    {
                        if (pInfo.LOTID(CurrentGLS.LOTID).Slot(dintSlotID) != null)
                        {
                            InfoAct.clsSlot currentSlot = pInfo.LOTID(CurrentGLS.LOTID).Slot(dintSlotID);
                            pMsgTran.Primary().Item("PRODUCTID" + 0 + 0).Putvalue(currentSlot.PRODUCTID);
                            pMsgTran.Primary().Item("STEPID" + 0 + 0).Putvalue(currentSlot.STEPID);
                            pMsgTran.Primary().Item("PPID2" + 0 + 0).Putvalue(currentSlot.HOSTPPID);
                        }
                        else
                        {
                            pMsgTran.Primary().Item("PRODUCTID" + 0 + 0).Putvalue("");
                            pMsgTran.Primary().Item("STEPID" + 0 + 0).Putvalue("");
                            pMsgTran.Primary().Item("PPID2" + 0 + 0).Putvalue("");
                        }
                    }
                    else
                    {
                        pMsgTran.Primary().Item("PRODUCTID" + 0 + 0).Putvalue("");
                        pMsgTran.Primary().Item("STEPID" + 0 + 0).Putvalue("");
                        pMsgTran.Primary().Item("PPID2" + 0 + 0).Putvalue("");
                    }
                }
                else
                {
                    pMsgTran.Primary().Item("PRODUCTID" + 0 + 0).Putvalue("");
                    pMsgTran.Primary().Item("STEPID" + 0 + 0).Putvalue("");
                    pMsgTran.Primary().Item("PPID2" + 0 + 0).Putvalue("");
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
