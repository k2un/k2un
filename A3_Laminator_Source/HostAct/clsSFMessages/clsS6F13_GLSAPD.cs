using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F13_GLSAPD : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F13_GLSAPD Instance = new clsS6F13_GLSAPD();
        #endregion

        #region Constructors
        public clsS6F13_GLSAPD()
        {
            this.IntStream = 6;
            this.IntFunction = 13;
            this.StrPrimaryMsgName = "S6F13GLSAPD";
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

                string dstrLOTID = arrayEvent[1].Trim();                            //LOTID
                int dintSlotID = Convert.ToInt32(arrayEvent[2]);                    //SlotID
                string dstrHGLSID = arrayEvent[3].Trim();                           //GLSID 추가 20100320 어우수

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);      //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(81);       //Fixed Value
                pMsgTran.Primary().Item("RPTID").Putvalue(10);      //Fixed Value
                pMsgTran.Primary().Item("GLASSCOUNT").Putvalue(1);  //이시 20101018 어우수

                //InfoAct.clsSlot currentSlot = this.pInfo.LOTID(dstrLOTID).Slot(dintSlotID);
                InfoAct.clsGLS CurrentGLS = pInfo.GLSID(dstrHGLSID);

                pMsgTran.Primary().Item("H_GLASSID" + 0).Putvalue(CurrentGLS.H_PANELID);
                pMsgTran.Primary().Item("E_GLASSID" + 0).Putvalue(CurrentGLS.E_PANELID);
                pMsgTran.Primary().Item("LOTID" + 0).Putvalue(CurrentGLS.LOTID);
                pMsgTran.Primary().Item("BATCHID" + 0).Putvalue(CurrentGLS.BATCHID);
                pMsgTran.Primary().Item("JOBID" + 0).Putvalue(CurrentGLS.JOBID);
                pMsgTran.Primary().Item("PORTID" + 0).Putvalue(CurrentGLS.PORTID);
                pMsgTran.Primary().Item("SLOTNO" + 0).Putvalue(CurrentGLS.SLOTNO);
                pMsgTran.Primary().Item("PROD_TYPE" + 0).Putvalue(CurrentGLS.PRODUCT_TYPE);
                pMsgTran.Primary().Item("PROD_KIND" + 0).Putvalue(CurrentGLS.PRODUCT_KIND);
                pMsgTran.Primary().Item("PRODUCTID" + 0).Putvalue(CurrentGLS.PRODUCTID);
                pMsgTran.Primary().Item("RUNSPECID" + 0).Putvalue(CurrentGLS.RUNSPECID);
                pMsgTran.Primary().Item("LAYERID" + 0).Putvalue(CurrentGLS.LAYERID);
                pMsgTran.Primary().Item("STEPID" + 0).Putvalue(CurrentGLS.STEPID);
                if (CurrentGLS.IsRPCRunning)
                {

                    pMsgTran.Primary().Item("PPID" + 0).Putvalue(pInfo.All.RPCPPID);
                }
                else
                {
                    pMsgTran.Primary().Item("PPID" + 0).Putvalue(CurrentGLS.HOSTPPID);
                }
                pMsgTran.Primary().Item("FLOWID" + 0).Putvalue(CurrentGLS.FLOWID);
                pMsgTran.Primary().Item("SIZE" + 0).Putvalue(CurrentGLS.SIZE);
                pMsgTran.Primary().Item("THICKNESS" + 0).Putvalue(CurrentGLS.THICKNESS);
                pMsgTran.Primary().Item("STATE" + 0).Putvalue(CurrentGLS.GLASS_STATE);
                pMsgTran.Primary().Item("ORDER" + 0).Putvalue(CurrentGLS.GLASS_ORDER);
                pMsgTran.Primary().Item("COMMENT" + 0).Putvalue(CurrentGLS.COMMENT);

                pMsgTran.Primary().Item("USE_COUNT" + 0).Putvalue(CurrentGLS.USE_COUNT);
                pMsgTran.Primary().Item("JUDGEMENT" + 0).Putvalue(CurrentGLS.JUDGEMENT);
                pMsgTran.Primary().Item("REASON_CODE" + 0).Putvalue(CurrentGLS.REASON_CODE);
                pMsgTran.Primary().Item("INS_FLAG" + 0).Putvalue(CurrentGLS.INS_FLAG);
                pMsgTran.Primary().Item("ENC_FLAG" + 0).Putvalue(CurrentGLS.ENC_FLAG);
                pMsgTran.Primary().Item("PRERUN_FLAG" + 0).Putvalue(CurrentGLS.PRERUN_FLAG);
                pMsgTran.Primary().Item("TURN_DIR" + 0).Putvalue(CurrentGLS.TURN_DIR);
                pMsgTran.Primary().Item("FLIP_STATE" + 0).Putvalue(CurrentGLS.FLIP_STATE);
                pMsgTran.Primary().Item("WORK_STATE" + 0).Putvalue(CurrentGLS.WORK_STATE);
                pMsgTran.Primary().Item("MULTI_USE" + 0).Putvalue(CurrentGLS.MULTI_USE);

                pMsgTran.Primary().Item("PAIR_GLASSID" + 0).Putvalue(CurrentGLS.PAIR_GLASSID);
                pMsgTran.Primary().Item("PAIR_PPID" + 0).Putvalue(CurrentGLS.PAIR_PPID);

                pMsgTran.Primary().Item("OPTION_NAME1" + 0).Putvalue(CurrentGLS.OPTION_NAME[0]);
                pMsgTran.Primary().Item("OPTION_VALUE1" + 0).Putvalue(CurrentGLS.OPTION_VALUE[0]);
                pMsgTran.Primary().Item("OPTION_NAME2" + 0).Putvalue(CurrentGLS.OPTION_NAME[1]);
                pMsgTran.Primary().Item("OPTION_VALUE2" + 0).Putvalue(CurrentGLS.OPTION_VALUE[1]);
                pMsgTran.Primary().Item("OPTION_NAME3" + 0).Putvalue(CurrentGLS.OPTION_NAME[2]);
                pMsgTran.Primary().Item("OPTION_VALUE3" + 0).Putvalue(CurrentGLS.OPTION_VALUE[2]);
                pMsgTran.Primary().Item("OPTION_NAME4" + 0).Putvalue(CurrentGLS.OPTION_NAME[3]);
                pMsgTran.Primary().Item("OPTION_VALUE4" + 0).Putvalue(CurrentGLS.OPTION_VALUE[3]);
                pMsgTran.Primary().Item("OPTION_NAME5" + 0).Putvalue(CurrentGLS.OPTION_NAME[4]);
                pMsgTran.Primary().Item("OPTION_VALUE5" + 0).Putvalue(CurrentGLS.OPTION_VALUE[4]);

                //GLS APD 추가
                pMsgTran.Primary().Item("RPTID1").Putvalue(9);          //Fixed Value
                pMsgTran.Primary().Item("MODULECNT").Putvalue(1);       //임시 20101018 어우수
                pMsgTran.Primary().Item("MODULEID" + 0).Putvalue(this.pInfo.Unit(3).SubUnit(0).ModuleID);

                int dintGlassApdCount = pInfo.Unit(0).SubUnit(0).GLSAPDReportCount;

                pMsgTran.Primary().Item("DATACOUNT" + 0).Putvalue(dintGlassApdCount);

                int dintAPDIndex = 0;
                for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).GLSAPDCount; dintLoop++)
                {
                    InfoAct.clsGLSAPD currentGlassApd = this.pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop);
                    if (currentGlassApd.HostReportFlag)
                    {
                        pMsgTran.Primary().Item("DATA_ITEM" + 0 + dintAPDIndex).Putvalue(currentGlassApd.Name);
                        pMsgTran.Primary().Item("DATA_VALUE" + 0 + dintAPDIndex).Putvalue(currentGlassApd.Value);
                        dintAPDIndex++;
                    }
                }

                pMsgTran.Primary().Item("RPTID2").Putvalue(12);      //Fixed Value
                pMsgTran.Primary().Item("GLSCNT").Putvalue(1);      //임시 20101018 어우수

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
