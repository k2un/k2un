using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F13_GLSAPDMea : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F13_GLSAPDMea Instance = new clsS6F13_GLSAPDMea();
        #endregion

        #region Constructors
        public clsS6F13_GLSAPDMea()
        {
            this.IntStream = 6;
            this.IntFunction = 13;
            this.StrPrimaryMsgName = "S6F13GLSAPDMea";
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
                pMsgTran.Primary().Item("CEID").Putvalue(82);       //Fixed Value
                pMsgTran.Primary().Item("RPTID").Putvalue(10);      //Fixed Value
                pMsgTran.Primary().Item("GLASSCOUNT").Putvalue(1);  //이시 20101018 어우수

                InfoAct.clsSlot currentSlot = this.pInfo.LOTID(dstrLOTID).Slot(dintSlotID);

                pMsgTran.Primary().Item("H_GLASSID" + 0).Putvalue(currentSlot.H_PANELID);
                pMsgTran.Primary().Item("E_GLASSID" + 0).Putvalue(currentSlot.E_PANELID);
                pMsgTran.Primary().Item("LOTID" + 0).Putvalue(currentSlot.LOTID);
                pMsgTran.Primary().Item("BATCHID" + 0).Putvalue(currentSlot.BATCHID);
                pMsgTran.Primary().Item("JOBID" + 0).Putvalue(currentSlot.JOBID);
                pMsgTran.Primary().Item("PORTID" + 0).Putvalue(currentSlot.PORTID);
                pMsgTran.Primary().Item("SLOTNO" + 0).Putvalue(currentSlot.SLOTNO);
                pMsgTran.Primary().Item("PRODUCT_TYPE" + 0).Putvalue(currentSlot.PRODUCT_TYPE);
                pMsgTran.Primary().Item("PRODUCT_KIND" + 0).Putvalue(currentSlot.PRODUCT_KIND);
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
                pMsgTran.Primary().Item("ENC_FLAG" + 0).Putvalue(currentSlot.ENC_FLAG);
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

                //GLS APD 추가
                pMsgTran.Primary().Item("RPTID1").Putvalue(9);          //Fixed Value
                pMsgTran.Primary().Item("MODULECNT").Putvalue(1);       //임시 20101018 어우수
                pMsgTran.Primary().Item("MODULEID" + 0).Putvalue(this.pInfo.Unit(0).SubUnit(0).ModuleID);

                pMsgTran.Primary().Item("RAWPATH").Putvalue("RAWPATH");
                pMsgTran.Primary().Item("RAWPATH_Value").Putvalue(pInfo.All.RAWPATH);
                pMsgTran.Primary().Item("SUMPATH").Putvalue("SUMPATH");
                pMsgTran.Primary().Item("SUMPATH_VALUE").Putvalue(pInfo.All.SUMPATH);
                pMsgTran.Primary().Item("IMGPATH").Putvalue("IMGPATH");
                pMsgTran.Primary().Item("IMGPATH_VALUE").Putvalue(pInfo.All.IMGPATH);
                pMsgTran.Primary().Item("DISK").Putvalue("DISK");
                pMsgTran.Primary().Item("DISK_VALUE").Putvalue(pInfo.All.DISK);
                
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
