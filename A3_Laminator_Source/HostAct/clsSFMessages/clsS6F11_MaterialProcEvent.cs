using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;

namespace HostAct
{
    public class clsS6F11_MaterialProcEvent : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS6F11_MaterialProcEvent Instance = new clsS6F11_MaterialProcEvent();
        #endregion

        #region Constructors
        public clsS6F11_MaterialProcEvent()
        {
            this.IntStream = 6;
            this.IntFunction = 11;
            this.StrPrimaryMsgName = "S6F11MaterialProcEvent";
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
            int dintSubUnitID = 0;
			//[2015/07/07](Add by HS)
            string dstrJuge = string.Empty;
            try
            {
                arrayEvent = strParameters.Split(',');

                int dintCEID = Convert.ToInt32(arrayEvent[1]);   //CEID
                dintUnitID = Convert.ToInt32(arrayEvent[5]);    //UnitID

                dstrFilmID = arrayEvent[3];
                dintSlotID = Convert.ToInt32(arrayEvent[4]);
                dintSubUnitID = Convert.ToInt32(arrayEvent[6]);
                //[2015/07/07](Add by HS)
                if (arrayEvent.Length > 7 && arrayEvent[7] != "" && arrayEvent[7]!= null)
                {
                    dstrJuge = arrayEvent[7];
                }
            

                pMsgTran = pSecsDrv.MakeTransaction(this.StrPrimaryMsgName);

                pMsgTran.Primary().Item("DATAID").Putvalue(0);  //Fixed Value
                pMsgTran.Primary().Item("CEID").Putvalue(dintCEID);
                pMsgTran.Primary().Item("RPTID").Putvalue(1);   //Fixed Value
                pMsgTran.Primary().Item("MODULEID").Putvalue(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID);
                pMsgTran.Primary().Item("MCMD").Putvalue(pInfo.All.ControlState);
                pMsgTran.Primary().Item("MODULE_STATE").Putvalue(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPState);
                pMsgTran.Primary().Item("PROC_STATE").Putvalue(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).EQPProcessState);
                if (dintCEID == 1018 || dintCEID == 1019)
                {
                    pMsgTran.Primary().Item("BYWHO").Putvalue(2);
                }
                else
                {
                    pMsgTran.Primary().Item("BYWHO").Putvalue(3);
                }
                pMsgTran.Primary().Item("OPERID").Putvalue(this.pInfo.All.UserID);

                pMsgTran.Primary().Item("RPTID2").Putvalue(13);     //Fixed Value
                pMsgTran.Primary().Item("GLASSCOUNT").Putvalue(1);  //임시 20101015 어우수

                InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrFilmID).Slot(dintSlotID);

                if (currentSlot != null)
                {
                    pMsgTran.Primary().Item("MATERIAL_ID").Putvalue(currentSlot.H_PANELID);
                    pMsgTran.Primary().Item("MATERIAL_SETID").Putvalue(currentSlot.E_PANELID);
                    pMsgTran.Primary().Item("LOTID").Putvalue(currentSlot.LOTID);
                    pMsgTran.Primary().Item("BATCHID").Putvalue(currentSlot.BATCHID);
                    pMsgTran.Primary().Item("JOBID").Putvalue(currentSlot.JOBID);
                    pMsgTran.Primary().Item("PORTID1").Putvalue(currentSlot.PORTID.Trim());
                    pMsgTran.Primary().Item("SLOTNO").Putvalue(currentSlot.SLOTNO);
                    pMsgTran.Primary().Item("PROD_TYPE").Putvalue(currentSlot.PRODUCT_TYPE);
                    if (dintCEID == 1015)
                    {
                        pMsgTran.Primary().Item("PROD_KIND").Putvalue("FILM");
                    }
                    else
                    {
                        pMsgTran.Primary().Item("PROD_KIND").Putvalue(currentSlot.PRODUCT_KIND);
                    }
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
                    
                    //[2015/06/30]Scrap과 NG처리를 구분하기위하여(Add by HS)
                    if (dintCEID == 1015)
                    {
                        if (dstrJuge == "OK")
                        {
                        }
                        else if (dstrJuge == "NG")
                        {
                            currentSlot.COMMENT = "C_00007";
                        }
                        else if (dstrJuge == "SCRAP")
                        {
                            currentSlot.COMMENT = "C_00008";
                        }
                    }
                    pMsgTran.Primary().Item("COMMENT").Putvalue(currentSlot.COMMENT);

                    pMsgTran.Primary().Item("USE_COUNT").Putvalue(currentSlot.USE_COUNT.PadLeft(3, '0'));
                    //[2015/05/08]Scrap처리를 호스트에서 하지못하여 CEID 1015로 판단하기에 추가(Add by HS)
                    //[2015/06/30]Scrap과 NG처리를 구분하기위하여(Add by HS)
                    if (dintCEID == 1015)
                    {
                        if (dstrJuge == "OK") //OK
                        {
                            currentSlot.JUDGEMENT = "OK";
                        }
                        else
                        {
                            currentSlot.JUDGEMENT = "NG";
                        }
                    }
                    pMsgTran.Primary().Item("JUDGEMENT").Putvalue(currentSlot.JUDGEMENT);
                    pMsgTran.Primary().Item("REASON_CODE").Putvalue(currentSlot.REASON_CODE);
                    pMsgTran.Primary().Item("INS_FLAG").Putvalue(currentSlot.INS_FLAG);
                    pMsgTran.Primary().Item("ENC_FLAG").Putvalue(currentSlot.ENC_FLAG);
                    pMsgTran.Primary().Item("PRERUN_FLAG").Putvalue(currentSlot.PRERUN_FLAG);
                    pMsgTran.Primary().Item("TURN_DIR").Putvalue(currentSlot.TURN_DIR);
                    pMsgTran.Primary().Item("FLIP_STATE").Putvalue(currentSlot.FLIP_STATE);
                    pMsgTran.Primary().Item("WORK_STATE").Putvalue(currentSlot.WORK_STATE);
                    pMsgTran.Primary().Item("MULTI_USE").Putvalue(currentSlot.MULTI_USE);

                    pMsgTran.Primary().Item("STAGE_STATE").Putvalue(currentSlot.PAIR_GLASSID);
                    pMsgTran.Primary().Item("LOCATION").Putvalue(currentSlot.PAIR_PPID);

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
                    currentSlot = new InfoAct.clsSlot(1);
                    pMsgTran.Primary().Item("MATERIAL_ID").Putvalue(currentSlot.H_PANELID);
                    pMsgTran.Primary().Item("MATERIAL_SETID").Putvalue(currentSlot.E_PANELID);
                    pMsgTran.Primary().Item("LOTID").Putvalue(currentSlot.LOTID);
                    pMsgTran.Primary().Item("BATCHID").Putvalue(currentSlot.BATCHID);
                    pMsgTran.Primary().Item("JOBID").Putvalue(currentSlot.JOBID);
                    pMsgTran.Primary().Item("PORTID1").Putvalue(currentSlot.PORTID.Trim());
                    pMsgTran.Primary().Item("SLOTNO").Putvalue(currentSlot.SLOTNO);
                    pMsgTran.Primary().Item("PROD_TYPE").Putvalue(currentSlot.PRODUCT_TYPE);
                    if (dintCEID == 1015)
                    {
                        pMsgTran.Primary().Item("PROD_KIND").Putvalue("FILM");
                    }
                    else
                    {
                        pMsgTran.Primary().Item("PROD_KIND").Putvalue(currentSlot.PRODUCT_KIND);
                    }
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
                    //[2015/06/30]Scrap과 NG처리를 구분하기위하여(Add by HS)
                    if (dintCEID == 1015)
                    {
                        if (dstrJuge == "OK")
                        {
                        }
                        else if (dstrJuge == "NG")
                        {
                            currentSlot.COMMENT = "C_00007";
                        }
                        else if (dstrJuge == "SCRAP")
                        {
                            currentSlot.COMMENT = "C_00008";
                        }
                    }
                    pMsgTran.Primary().Item("COMMENT").Putvalue(currentSlot.COMMENT);

                    pMsgTran.Primary().Item("USE_COUNT").Putvalue(currentSlot.USE_COUNT);
                    //[2015/05/08]Scrap처리를 호스트에서 하지못하여 CEID 1015로 판단하기에 추가(Add by HS)
                    //[2015/06/30]Scrap과 NG처리를 구분하기위하여(Add by HS)
                    if (dintCEID == 1015)
                    {
                        if (dstrJuge == "OK") //OK
                        {
                            currentSlot.JUDGEMENT = "OK";
                        }
                        else
                        {
                            currentSlot.JUDGEMENT = "NG";
                        }
                    }
                    pMsgTran.Primary().Item("JUDGEMENT").Putvalue(currentSlot.JUDGEMENT);
                    pMsgTran.Primary().Item("REASON_CODE").Putvalue(currentSlot.REASON_CODE);

                    pMsgTran.Primary().Item("INS_FLAG").Putvalue(currentSlot.INS_FLAG);
                    pMsgTran.Primary().Item("ENC_FLAG").Putvalue(currentSlot.ENC_FLAG);
                    pMsgTran.Primary().Item("PRERUN_FLAG").Putvalue(currentSlot.PRERUN_FLAG);
                    pMsgTran.Primary().Item("TURN_DIR").Putvalue(currentSlot.TURN_DIR);
                    pMsgTran.Primary().Item("FLIP_STATE").Putvalue(currentSlot.FLIP_STATE);
                    pMsgTran.Primary().Item("WORK_STATE").Putvalue(currentSlot.WORK_STATE);
                    pMsgTran.Primary().Item("MULTI_USE").Putvalue(currentSlot.MULTI_USE);

                    pMsgTran.Primary().Item("STAGE_STATE").Putvalue(currentSlot.PAIR_GLASSID);
                    pMsgTran.Primary().Item("LOCATION").Putvalue(currentSlot.PAIR_PPID);

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
