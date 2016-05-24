using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_RelatedGlassProcess : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_RelatedGlassProcess Instance = new clsS6F11_RelatedGlassProcess();
        #endregion

        #region Constructors
        public clsS6F11_RelatedGlassProcess()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11RelatedGlassProcess";
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
            int dintBYWHO = 0;

            try
            {
                arrayEvent = strParameters.Split(',');

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                int dintCEID = Convert.ToInt32(arrayEvent[1]);                       //CEID
                int dintUnitID = Convert.ToInt32(arrayEvent[2]);                     //UnitID
                int dintSubUnitID = Convert.ToInt32(arrayEvent[3]);
                string dstrGLSID = arrayEvent[4];                                       //LOTID
                int dintSlotID = Convert.ToInt32(arrayEvent[5]);                     //SlotID
                //string dstrHGLSID = arrayEvent[5].Trim();                               //GLSID

                pMsgTran.Primary().Item("DATAID").Putvalue(0);  //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(1);   //Fixed Value
                
                    pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPProcessState);

                //각 CEID별로 BYWHO값을 설정한다.
                switch (dintCEID)
                {
                    case 10:
                        dintBYWHO = 3;
                        break;
                    case 13:
                        dintBYWHO = 3;
                        break;
                    case 18:
                        dintBYWHO = 2;
                        break;
                    case 19:
                        dintBYWHO = 2;
                        break;
                    case 21:
                        dintBYWHO = 3;
                        break;
                    case 23:
                        dintBYWHO = 3;
                        break;
                    default:
                        dintBYWHO = 3;
                        break;
                }

                pMsgTran.Primary().Item("BYWHO").Putvalue(dintBYWHO);
                pMsgTran.Primary().Item("OPERID").Putvalue(this.pInfo.All.UserID);

                pMsgTran.Primary().Item("RPTID2").Putvalue(10);     //Fixed Value
                pMsgTran.Primary().Item("GLASSCOUNT").Putvalue(1);  //임시 20101015 어우수


                if (dintCEID.ToString().Length > 2)
                {
                    InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrGLSID).Slot(dintSlotID);

                    pMsgTran.Primary().Item("H_GLASSID").Putvalue(currentSlot.H_PANELID);
                    pMsgTran.Primary().Item("E_GLASSID").Putvalue(currentSlot.E_PANELID);
                    pMsgTran.Primary().Item("LOTID").Putvalue(currentSlot.LOTID);
                    pMsgTran.Primary().Item("BATCHID").Putvalue(currentSlot.BATCHID);
                    pMsgTran.Primary().Item("JOBID").Putvalue(currentSlot.JOBID);
                    pMsgTran.Primary().Item("PORTID1").Putvalue(currentSlot.PORTID.Trim());
                    pMsgTran.Primary().Item("SLOTNO").Putvalue(currentSlot.SLOTNO);
                    pMsgTran.Primary().Item("PROD_TYPE").Putvalue(currentSlot.PRODUCT_TYPE);
                    pMsgTran.Primary().Item("PROD_KIND").Putvalue(currentSlot.PRODUCT_KIND);
                    pMsgTran.Primary().Item("PRODUCTID").Putvalue(currentSlot.PRODUCTID);
                    pMsgTran.Primary().Item("RUNSPECID").Putvalue(currentSlot.RUNSPECID);
                    pMsgTran.Primary().Item("LAYERID").Putvalue(currentSlot.LAYERID);
                    pMsgTran.Primary().Item("STEPID").Putvalue(currentSlot.STEPID);
                    pMsgTran.Primary().Item("PPID").Putvalue(currentSlot.HOSTPPID);
                    pMsgTran.Primary().Item("FLOWID").Putvalue(currentSlot.FLOWID);
                    string[] darrSize = currentSlot.SIZE.Split(' ');
                    pMsgTran.Primary().Item("SIZE").Putvalue(darrSize);
                    pMsgTran.Primary().Item("THICKNESS").Putvalue(currentSlot.THICKNESS);
                    pMsgTran.Primary().Item("STATE").Putvalue(currentSlot.GLASS_STATE);
                    pMsgTran.Primary().Item("ORDER").Putvalue(currentSlot.GLASS_ORDER);
                    pMsgTran.Primary().Item("COMMENT").Putvalue(currentSlot.COMMENT);

                    pMsgTran.Primary().Item("USE_COUNT").Putvalue(currentSlot.USE_COUNT);
                    pMsgTran.Primary().Item("JUDGEMENT").Putvalue(currentSlot.JUDGEMENT);
                    pMsgTran.Primary().Item("REASON_CODE").Putvalue(currentSlot.REASON_CODE);
                    pMsgTran.Primary().Item("INS_FLAG").Putvalue(currentSlot.INS_FLAG);
                    pMsgTran.Primary().Item("ENC_FLAG").Putvalue(currentSlot.ENC_FLAG);
                    pMsgTran.Primary().Item("PRERUN_FLAG").Putvalue(currentSlot.PRERUN_FLAG);
                    pMsgTran.Primary().Item("TURN_DIR").Putvalue(currentSlot.TURN_DIR);
                    pMsgTran.Primary().Item("FLIP_STATE").Putvalue(currentSlot.FLIP_STATE);
                    pMsgTran.Primary().Item("WORK_STATE").Putvalue(currentSlot.WORK_STATE);
                    pMsgTran.Primary().Item("MULTI_USE").Putvalue(currentSlot.MULTI_USE);

                    pMsgTran.Primary().Item("PAIR_GLASSID").Putvalue(currentSlot.PAIR_GLASSID);
                    pMsgTran.Primary().Item("PAIR_PPID").Putvalue(currentSlot.PAIR_PPID);

                    pMsgTran.Primary().Item("OPTION_NAME1").Putvalue(currentSlot.OPTION_NAME[0]);
                    pMsgTran.Primary().Item("OPTION_VALUE1").Putvalue(currentSlot.OPTION_VALUE[0]);
                    pMsgTran.Primary().Item("OPTION_NAME2").Putvalue(currentSlot.OPTION_NAME[1]);
                    pMsgTran.Primary().Item("OPTION_VALUE2").Putvalue(currentSlot.OPTION_VALUE[1]);
                    pMsgTran.Primary().Item("OPTION_NAME3").Putvalue(currentSlot.OPTION_NAME[2]);
                    pMsgTran.Primary().Item("OPTION_VALUE3").Putvalue(currentSlot.OPTION_VALUE[2]);
                    pMsgTran.Primary().Item("OPTION_NAME4").Putvalue(currentSlot.OPTION_NAME[3]);
                    pMsgTran.Primary().Item("OPTION_VALUE4").Putvalue(currentSlot.OPTION_VALUE[3]);
                    pMsgTran.Primary().Item("OPTION_NAME5").Putvalue(currentSlot.OPTION_NAME[4]);
                    pMsgTran.Primary().Item("OPTION_VALUE5").Putvalue(currentSlot.OPTION_VALUE[4]);

                }
                else
                {
                    //InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);
                    InfoAct.clsGLS CurrentGLS = pInfo.GLSID(dstrGLSID);

                    pMsgTran.Primary().Item("H_GLASSID").Putvalue(CurrentGLS.H_PANELID);
                    pMsgTran.Primary().Item("E_GLASSID").Putvalue(CurrentGLS.E_PANELID);
                    pMsgTran.Primary().Item("LOTID").Putvalue(CurrentGLS.LOTID);
                    pMsgTran.Primary().Item("BATCHID").Putvalue(CurrentGLS.BATCHID);
                    pMsgTran.Primary().Item("JOBID").Putvalue(CurrentGLS.JOBID);
                    pMsgTran.Primary().Item("PORTID1").Putvalue(CurrentGLS.PORTID.Trim());
                    pMsgTran.Primary().Item("SLOTNO").Putvalue(CurrentGLS.SLOTNO);
                    pMsgTran.Primary().Item("PROD_TYPE").Putvalue(CurrentGLS.PRODUCT_TYPE);
                    pMsgTran.Primary().Item("PROD_KIND").Putvalue(CurrentGLS.PRODUCT_KIND);
                    pMsgTran.Primary().Item("PRODUCTID").Putvalue(CurrentGLS.PRODUCTID);
                    pMsgTran.Primary().Item("RUNSPECID").Putvalue(CurrentGLS.RUNSPECID);
                    pMsgTran.Primary().Item("LAYERID").Putvalue(CurrentGLS.LAYERID);
                    pMsgTran.Primary().Item("STEPID").Putvalue(CurrentGLS.STEPID);
                    pMsgTran.Primary().Item("PPID").Putvalue(CurrentGLS.HOSTPPID);
                    pMsgTran.Primary().Item("FLOWID").Putvalue(CurrentGLS.FLOWID);
                    string[] darrSize = CurrentGLS.SIZE.Split(' ');
                    pMsgTran.Primary().Item("SIZE").Putvalue(darrSize);
                    pMsgTran.Primary().Item("THICKNESS").Putvalue(CurrentGLS.THICKNESS);
                    pMsgTran.Primary().Item("STATE").Putvalue(CurrentGLS.GLASS_STATE);
                    pMsgTran.Primary().Item("ORDER").Putvalue(CurrentGLS.GLASS_ORDER);
                    pMsgTran.Primary().Item("COMMENT").Putvalue(CurrentGLS.COMMENT);

                    pMsgTran.Primary().Item("USE_COUNT").Putvalue(CurrentGLS.USE_COUNT);
                    pMsgTran.Primary().Item("JUDGEMENT").Putvalue(CurrentGLS.JUDGEMENT);
                    pMsgTran.Primary().Item("REASON_CODE").Putvalue(CurrentGLS.REASON_CODE);
                    pMsgTran.Primary().Item("INS_FLAG").Putvalue(CurrentGLS.INS_FLAG);
                    pMsgTran.Primary().Item("ENC_FLAG").Putvalue(CurrentGLS.ENC_FLAG);
                    pMsgTran.Primary().Item("PRERUN_FLAG").Putvalue(CurrentGLS.PRERUN_FLAG);
                    pMsgTran.Primary().Item("TURN_DIR").Putvalue(CurrentGLS.TURN_DIR);
                    pMsgTran.Primary().Item("FLIP_STATE").Putvalue(CurrentGLS.FLIP_STATE);
                    pMsgTran.Primary().Item("WORK_STATE").Putvalue(CurrentGLS.WORK_STATE);
                    pMsgTran.Primary().Item("MULTI_USE").Putvalue(CurrentGLS.MULTI_USE);

                    pMsgTran.Primary().Item("PAIR_GLASSID").Putvalue(CurrentGLS.PAIR_GLASSID);
                    pMsgTran.Primary().Item("PAIR_PPID").Putvalue(CurrentGLS.PAIR_PPID);

                    pMsgTran.Primary().Item("OPTION_NAME1").Putvalue(CurrentGLS.OPTION_NAME[0]);
                    pMsgTran.Primary().Item("OPTION_VALUE1").Putvalue(CurrentGLS.OPTION_VALUE[0]);
                    pMsgTran.Primary().Item("OPTION_NAME2").Putvalue(CurrentGLS.OPTION_NAME[1]);
                    pMsgTran.Primary().Item("OPTION_VALUE2").Putvalue(CurrentGLS.OPTION_VALUE[1]);
                    pMsgTran.Primary().Item("OPTION_NAME3").Putvalue(CurrentGLS.OPTION_NAME[2]);
                    pMsgTran.Primary().Item("OPTION_VALUE3").Putvalue(CurrentGLS.OPTION_VALUE[2]);
                    pMsgTran.Primary().Item("OPTION_NAME4").Putvalue(CurrentGLS.OPTION_NAME[3]);
                    pMsgTran.Primary().Item("OPTION_VALUE4").Putvalue(CurrentGLS.OPTION_VALUE[3]);
                    pMsgTran.Primary().Item("OPTION_NAME5").Putvalue(CurrentGLS.OPTION_NAME[4]);
                    pMsgTran.Primary().Item("OPTION_VALUE5").Putvalue(CurrentGLS.OPTION_VALUE[4]);
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
