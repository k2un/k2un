using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_RelatedJobProcess : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_RelatedJobProcess Instance = new clsS6F11_RelatedJobProcess();
        #endregion

        #region Constructors
        public clsS6F11_RelatedJobProcess()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11RelatedJobProcess";
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
            string dstrLOTID = "";
            int dintSlotID = 0;
            try
            {
                arrayEvent = strParameters.Split(',');

                int dintBYWHO = 0;

                int dintCEID = Convert.ToInt32(arrayEvent[1]);                       //CEID
                int dintPortID = Convert.ToInt32(arrayEvent[2]);
                int dintUnitID = Convert.ToInt32(arrayEvent[3]);
                int dintSubUnitID = Convert.ToInt32(arrayEvent[4]);
                if (dintCEID == 1006 || dintCEID == 1007 || dintCEID == 1001 || dintCEID == 1004)
                {
                    //if (arrayEvent.Length == 6)
                    //{
                        dstrLOTID = arrayEvent[5].ToString();
                        dintSlotID = Convert.ToInt32(arrayEvent[6].ToString());
                    //}
                }

                InfoAct.clsPort CurrentPort = pInfo.Port(dintPortID);

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);      //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);

                pMsgTran.Primary().Item("RPTID").Putvalue(1);       //Fixed Value
                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(0).ModuleID);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(0).SubUnit(0).EQPState);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(0).SubUnit(0).EQPProcessState);

                //각 CEID별로 BYWHO값을 설정한다.
                switch (dintCEID)
                {
                    case 111:
                        dintBYWHO = 3;
                        break;
                    case 113:
                        dintBYWHO = 3;
                        break;
                    case 114:
                        dintBYWHO = 4;
                        break;
                    default:
                        dintBYWHO = 3;
                        break;
                }

                //InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);

                pMsgTran.Primary().Item("BYWHO").Putvalue(dintBYWHO);
                pMsgTran.Primary().Item("OPERID").Putvalue(pInfo.All.UserID);

                //면취기는 자체 PORT가 없어 PORTID 이외의 정보를 가지고 있지 않으므로, 협의후 확정될때 까지 무시한다. 20101018 어우수
                pMsgTran.Primary().Item("RPTID1").Putvalue(2);       //Fixed Value
                pMsgTran.Primary().Item("PORTID").Putvalue(CurrentPort.HostReportPortID);
                pMsgTran.Primary().Item("PORT_STATE").Putvalue(CurrentPort.PortState);
                pMsgTran.Primary().Item("PORT_TYPE").Putvalue(CurrentPort.PortType);
                pMsgTran.Primary().Item("PORT_MODE").Putvalue("OK");
                pMsgTran.Primary().Item("SORT_TYPE").Putvalue("0");

                //면취기는 자체 PORT가 없어 CST 정보를 가지고 있지 않으므로, 협의후 확정될때까지 무시한다. 20101018 어우수
                pMsgTran.Primary().Item("RPTID2").Putvalue(3);      //Fixed Value
                pMsgTran.Primary().Item("CSTID").Putvalue(CurrentPort.CSTID);
                if (dintUnitID == 1)
                {
                    pMsgTran.Primary().Item("CST_TYPE").Putvalue("12");
                }
                else
                {
                    pMsgTran.Primary().Item("CST_TYPE").Putvalue("13");
                }
                pMsgTran.Primary().Item("MAP_STIF").Putvalue(CurrentPort.GLSHostMapping10);
                pMsgTran.Primary().Item("CUR_STIF").Putvalue(CurrentPort.GLSRealMapping10);
                pMsgTran.Primary().Item("BATCH_ORDER").Putvalue(CurrentPort.BATCH_ORDER);

                pMsgTran.Primary().Item("RPTID3").Putvalue(10);     //Fixed Value
                pMsgTran.Primary().Item("GLASSCOUNT").Putvalue(1);  //임시 20101018 어우수
                if (dintCEID == 1006 || dintCEID == 1007)
                {
                    //InfoAct.clsSlot currentSlot = new InfoAct.clsSlot(1);
                    InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);

                    pMsgTran.Primary().Item("H_GLASSID" + 0).Putvalue(currentSlot.GlassID);
                    pMsgTran.Primary().Item("E_GLASSID" + 0).Putvalue(currentSlot.E_PANELID);
                    pMsgTran.Primary().Item("LOTID" + 0).Putvalue(currentSlot.LOTID);
                    pMsgTran.Primary().Item("BATCHID" + 0).Putvalue(currentSlot.BATCHID);
                    pMsgTran.Primary().Item("JOBID" + 0).Putvalue(currentSlot.JOBID);
                    pMsgTran.Primary().Item("PORTID1" + 0).Putvalue(currentSlot.PORTID.Trim());
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

                    pMsgTran.Primary().Item("USE_COUNT" + 0).Putvalue(currentSlot.USE_COUNT);
                    pMsgTran.Primary().Item("JUDGEMENT" + 0).Putvalue(currentSlot.JUDGEMENT);
                    pMsgTran.Primary().Item("REASON_CODE" + 0).Putvalue(currentSlot.REASON_CODE);
                    pMsgTran.Primary().Item("INS_FLAG" + 0).Putvalue(currentSlot.INS_FLAG);
                    pMsgTran.Primary().Item("ENC_FALG" + 0).Putvalue(currentSlot.ENC_FLAG);
                    pMsgTran.Primary().Item("PRERUN_FLAG" + 0).Putvalue(currentSlot.PRERUN_FLAG);
                    pMsgTran.Primary().Item("TURN_DIR" + 0).Putvalue(currentSlot.TURN_DIR);
                    pMsgTran.Primary().Item("FLIP_STATE" + 0).Putvalue(currentSlot.FLIP_STATE);
                    pMsgTran.Primary().Item("WORK_STATE" + 0).Putvalue(currentSlot.WORK_STATE);
                    pMsgTran.Primary().Item("MULTI_USE" + 0).Putvalue(currentSlot.MULTI_USE);

                    pMsgTran.Primary().Item("PAIR_GLASSID" + 0).Putvalue(currentSlot.PAIR_GLASSID);
                    pMsgTran.Primary().Item("PAIR_PPID" + 0).Putvalue(currentSlot.PAIR_PPID);

                    pMsgTran.Primary().Item("OPTION_NAME1" + 0).Putvalue(currentSlot.OPTION_NAME[0]);
                    pMsgTran.Primary().Item("OPTION_VALUE1" + 0).Putvalue(currentSlot.OPTION_VALUE[0]);
                    pMsgTran.Primary().Item("OPTION_NAME2" + 0).Putvalue(currentSlot.OPTION_NAME[1]);
                    pMsgTran.Primary().Item("OPTION_VALUE2" + 0).Putvalue(currentSlot.OPTION_VALUE[1]);
                    pMsgTran.Primary().Item("OPTION_NAME3" + 0).Putvalue(currentSlot.OPTION_NAME[2]);
                    pMsgTran.Primary().Item("OPTION_VALUE3" + 0).Putvalue(currentSlot.OPTION_VALUE[2]);
                    pMsgTran.Primary().Item("OPTION_NAME4" + 0).Putvalue(currentSlot.OPTION_NAME[3]);
                    pMsgTran.Primary().Item("OPTION_VALUE4" + 0).Putvalue(currentSlot.OPTION_VALUE[3]);
                    pMsgTran.Primary().Item("OPTION_NAME5" + 0).Putvalue(currentSlot.OPTION_NAME[4]);
                    pMsgTran.Primary().Item("OPTION_VALUE5" + 0).Putvalue(currentSlot.OPTION_VALUE[4]);

                    pMsgTran.Primary().Item("SUBMATERIALCOUNT" + 0).Putvalue(0);
                }
                else if (dintCEID == 1001 || dintCEID == 1004)
                {
                    InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);


                    if (currentSlot == null)
                    {
                        pInfo.LOTID(dstrLOTID).AddSlot(dintSlotID);
                        currentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);
                        currentSlot.USE_COUNT = dintSlotID.ToString();
                    }

                    pMsgTran.Primary().Item("H_GLASSID" + 0).Putvalue(currentSlot.GlassID);
                    pMsgTran.Primary().Item("E_GLASSID" + 0).Putvalue(currentSlot.E_PANELID);
                    pMsgTran.Primary().Item("LOTID" + 0).Putvalue(currentSlot.LOTID);
                    pMsgTran.Primary().Item("BATCHID" + 0).Putvalue(currentSlot.BATCHID);
                    pMsgTran.Primary().Item("JOBID" + 0).Putvalue(currentSlot.JOBID);
                    pMsgTran.Primary().Item("PORTID1" + 0).Putvalue(currentSlot.PORTID.Trim());
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

                    pMsgTran.Primary().Item("USE_COUNT" + 0).Putvalue(currentSlot.USE_COUNT);
                    pMsgTran.Primary().Item("JUDGEMENT" + 0).Putvalue(currentSlot.JUDGEMENT);
                    pMsgTran.Primary().Item("REASON_CODE" + 0).Putvalue(currentSlot.REASON_CODE);
                    pMsgTran.Primary().Item("INS_FLAG" + 0).Putvalue(currentSlot.INS_FLAG);
                    pMsgTran.Primary().Item("ENC_FALG" + 0).Putvalue(currentSlot.ENC_FLAG);
                    pMsgTran.Primary().Item("PRERUN_FLAG" + 0).Putvalue(currentSlot.PRERUN_FLAG);
                    pMsgTran.Primary().Item("TURN_DIR" + 0).Putvalue(currentSlot.TURN_DIR);
                    pMsgTran.Primary().Item("FLIP_STATE" + 0).Putvalue(currentSlot.FLIP_STATE);
                    pMsgTran.Primary().Item("WORK_STATE" + 0).Putvalue(currentSlot.WORK_STATE);
                    pMsgTran.Primary().Item("MULTI_USE" + 0).Putvalue(currentSlot.MULTI_USE);

                    pMsgTran.Primary().Item("PAIR_GLASSID" + 0).Putvalue(currentSlot.PAIR_GLASSID);
                    pMsgTran.Primary().Item("PAIR_PPID" + 0).Putvalue(currentSlot.PAIR_PPID);

                    pMsgTran.Primary().Item("OPTION_NAME1" + 0).Putvalue(currentSlot.OPTION_NAME[0]);
                    pMsgTran.Primary().Item("OPTION_VALUE1" + 0).Putvalue(currentSlot.OPTION_VALUE[0]);
                    pMsgTran.Primary().Item("OPTION_NAME2" + 0).Putvalue(currentSlot.OPTION_NAME[1]);
                    pMsgTran.Primary().Item("OPTION_VALUE2" + 0).Putvalue(currentSlot.OPTION_VALUE[1]);
                    pMsgTran.Primary().Item("OPTION_NAME3" + 0).Putvalue(currentSlot.OPTION_NAME[2]);
                    pMsgTran.Primary().Item("OPTION_VALUE3" + 0).Putvalue(currentSlot.OPTION_VALUE[2]);
                    pMsgTran.Primary().Item("OPTION_NAME4" + 0).Putvalue(currentSlot.OPTION_NAME[3]);
                    pMsgTran.Primary().Item("OPTION_VALUE4" + 0).Putvalue(currentSlot.OPTION_VALUE[3]);
                    pMsgTran.Primary().Item("OPTION_NAME5" + 0).Putvalue(currentSlot.OPTION_NAME[4]);
                    pMsgTran.Primary().Item("OPTION_VALUE5" + 0).Putvalue(currentSlot.OPTION_VALUE[4]);

                    pMsgTran.Primary().Item("SUBMATERIALCOUNT" + 0).Putvalue(0);
                }
                else
                {
                    InfoAct.clsSlot currentSlot = CurrentPort.Slot(1);

                    pMsgTran.Primary().Item("H_GLASSID" + 0).Putvalue(currentSlot.GlassID);
                    pMsgTran.Primary().Item("E_GLASSID" + 0).Putvalue(currentSlot.E_PANELID);
                    pMsgTran.Primary().Item("LOTID" + 0).Putvalue(currentSlot.LOTID);
                    pMsgTran.Primary().Item("BATCHID" + 0).Putvalue(currentSlot.BATCHID);
                    pMsgTran.Primary().Item("JOBID" + 0).Putvalue(currentSlot.JOBID);
                    pMsgTran.Primary().Item("PORTID1" + 0).Putvalue(currentSlot.PORTID.Trim());
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

                    pMsgTran.Primary().Item("USE_COUNT" + 0).Putvalue(currentSlot.USE_COUNT);
                    pMsgTran.Primary().Item("JUDGEMENT" + 0).Putvalue(currentSlot.JUDGEMENT);
                    pMsgTran.Primary().Item("REASON_CODE" + 0).Putvalue(currentSlot.REASON_CODE);
                    pMsgTran.Primary().Item("INS_FLAG" + 0).Putvalue(currentSlot.INS_FLAG);
                    pMsgTran.Primary().Item("ENC_FALG" + 0).Putvalue(currentSlot.ENC_FLAG);
                    pMsgTran.Primary().Item("PRERUN_FLAG" + 0).Putvalue(currentSlot.PRERUN_FLAG);
                    pMsgTran.Primary().Item("TURN_DIR" + 0).Putvalue(currentSlot.TURN_DIR);
                    pMsgTran.Primary().Item("FLIP_STATE" + 0).Putvalue(currentSlot.FLIP_STATE);
                    pMsgTran.Primary().Item("WORK_STATE" + 0).Putvalue(currentSlot.WORK_STATE);
                    pMsgTran.Primary().Item("MULTI_USE" + 0).Putvalue(currentSlot.MULTI_USE);

                    pMsgTran.Primary().Item("PAIR_GLASSID" + 0).Putvalue(currentSlot.PAIR_GLASSID);
                    pMsgTran.Primary().Item("PAIR_PPID" + 0).Putvalue(currentSlot.PAIR_PPID);

                    pMsgTran.Primary().Item("OPTION_NAME1" + 0).Putvalue(currentSlot.OPTION_NAME[0]);
                    pMsgTran.Primary().Item("OPTION_VALUE1" + 0).Putvalue(currentSlot.OPTION_VALUE[0]);
                    pMsgTran.Primary().Item("OPTION_NAME2" + 0).Putvalue(currentSlot.OPTION_NAME[1]);
                    pMsgTran.Primary().Item("OPTION_VALUE2" + 0).Putvalue(currentSlot.OPTION_VALUE[1]);
                    pMsgTran.Primary().Item("OPTION_NAME3" + 0).Putvalue(currentSlot.OPTION_NAME[2]);
                    pMsgTran.Primary().Item("OPTION_VALUE3" + 0).Putvalue(currentSlot.OPTION_VALUE[2]);
                    pMsgTran.Primary().Item("OPTION_NAME4" + 0).Putvalue(currentSlot.OPTION_NAME[3]);
                    pMsgTran.Primary().Item("OPTION_VALUE4" + 0).Putvalue(currentSlot.OPTION_VALUE[3]);
                    pMsgTran.Primary().Item("OPTION_NAME5" + 0).Putvalue(currentSlot.OPTION_NAME[4]);
                    pMsgTran.Primary().Item("OPTION_VALUE5" + 0).Putvalue(currentSlot.OPTION_VALUE[4]);

                    pMsgTran.Primary().Item("SUBMATERIALCOUNT" + 0).Putvalue(0);

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
