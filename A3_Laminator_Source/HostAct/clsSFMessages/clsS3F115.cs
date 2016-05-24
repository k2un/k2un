using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using NSECS;
using CommonAct;
using System.Threading;
using InfoAct;

namespace HostAct
{
    public class clsS3F115 : clsStreamFunction, IStreamFunction
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Singleton
        public static readonly clsS3F115 Instance = new clsS3F115();
        #endregion

        #region Constructors
        public clsS3F115()
        {
            this.IntStream = 3;
            this.IntFunction = 115;
            this.StrPrimaryMsgName = "S3F115MaterialInfo";
            this.StrSecondaryMsgName = "S3F116";
        }
        #endregion

        private enum LOTInfoValidation
        {
            Accepted = 0,                                   //LOT Information : 정상적 수신
            Busy = 1,                                       //LOT Information : busy, try again
            AlreadyReceived = 2,                            //LOT Information : The data already received.
            NoCST = 3,                                      //LOT Information : No cassette
            CSTIDMismatch = 4,                              //LOT Information : Cassette ID is not match
            ParameterError = 5,                             //LOT Information : At least one parameter invalid
            PortStateNotWaiting = 6,                        //LOT Information : Port State is not waiting
            PPIDError = 7,                                  //LOT Information : PPID is not match
            ProductTypeMismatch = 8,                        //LOT Information : Product Type is not match
            GLSMappingMismatch = 9,                         //LOT Information : Glass Mapping Mismatch * (Glass Mapping Mismatch or Panel Thickness Mismatch)
            //>9 = Reserved
        }


        #region Methods
        /// <summary>
        /// Primary Message에 대해 처리한 후 Secondary Message를 Biuld하여 Driver에 전송한다.
        /// </summary>
        /// <param name="msgTran">Primary Message의 Transaction</param>
        public void funPrimaryReceive(Transaction msgTran)
        {
            int dintAck3 = (int)LOTInfoValidation.Accepted;
            int dintLOTIndex = 0;
            string dstrHostGLSMapping10 = "";
            string dstrProcessGLSMapping10 = "";
            string dstrGLSLoaderMapping10 = "";
            string dstrPortModuleID = "";
            int dintPortID = 0;
            int dintSlotID = 0;
            int dintPortType = 0;
            string dstrHPanelID = "";
            string dstrHOSTPPID = "";
            string dstrProductType = "";
            string dstrCSTID = "";
            int dintCSTType = 0;
            string dstrMapStif = "";
            string dstrCurStif = "";
            int dintOrder = 0;
            string SlotArray = "";
            string strGLSID = "";

            //subSetLog(InfoAct.clsInfo.LogType.ReceivedPrimaryMessage, "funPrimaryReceive() S" + msgTran.Primary().Stream + "F" + msgTran.Primary().Function + "Start");   // 수신메시지추적 [6/3/2013 Hojun])

            int dintUnitID = 0;
            int dintSubUnitID = 0;

            try
            {
                dstrPortModuleID = msgTran.Primary().Item("PORTID").Getvalue().ToString();
                for (int dintLoop = 0; dintLoop < 6; dintLoop++)
                {
                    if (pInfo.Port(dintLoop + 1).HostReportPortID == dstrPortModuleID)
                    {
                        dintPortID = dintLoop + 1;
                        switch (dintLoop + 1)
                        {
                            case 1:
                                dintUnitID = 1;
                                dintSubUnitID = 1;
                                break;

                            case 2:
                                dintUnitID = 1;
                                dintSubUnitID = 2;
                                break;

                            case 3:
                                dintUnitID = 1;
                                dintSubUnitID = 3;
                                break;

                            case 4:
                                dintUnitID = 1;
                                dintSubUnitID = 4;
                                break;

                            case 5:
                                dintUnitID = 2;
                                dintSubUnitID = 1;
                                break;

                            case 6:
                                dintUnitID = 2;
                                dintSubUnitID = 2;
                                break;
                        }
                        break;
                    }
                }


                dstrCSTID = msgTran.Primary().Item("CSTID").Getvalue().ToString();
                dintPortType = Convert.ToInt32(msgTran.Primary().Item("PORT_TYPE").Getvalue());

                if (dintPortID == 0 || dintPortID > 6)
                {
                    dintAck3 = (int)LOTInfoValidation.ParameterError;
                }
                else
                {
                    if (pInfo.Port(dintPortID).S3F115Received)
                    {
                        dintAck3 = (int)LOTInfoValidation.AlreadyReceived;
                    }
                    else if (0 != dintPortType)
                    {
                        dintAck3 = (int)LOTInfoValidation.ParameterError;
                    }
                }



                if (dintAck3 == 0)
                {
                    //if (dintPortType == 0)
                    //{
                    //    dintAck3 = (int)LOTInfoValidation.ParameterError;
                    //}
                    //else
                    if (pInfo.Port(dintPortID).PortState == "4")
                    {
                        dintAck3 = (int)LOTInfoValidation.Busy;
                    }
                    else if ((pInfo.Port(dintPortID).PortState == "0") || (string.IsNullOrEmpty(pInfo.Port(dintPortID).CSTID)))
                    {
                        if (dintPortID == 2)
                        {
                            dintAck3 = (int)LOTInfoValidation.NoCST;
                        }
                    }
                    else if (pInfo.Port(dintPortID).PortState != "2")
                    {
                        dintAck3 = (int)LOTInfoValidation.PortStateNotWaiting;
                    }
                    else if (string.IsNullOrEmpty(pInfo.Port(dintPortID).CSTID) == false)
                    {
                        if (pInfo.Port(dintPortID).CSTID != dstrCSTID.Trim())
                        {
                            dintAck3 = (int)LOTInfoValidation.CSTIDMismatch;
                        }
                        else
                        {
                            //[2015/05/20]FI01에서도 저장되게 조건식 추가(Add by HS)
                            if (dintUnitID == 1 && dintSubUnitID == 2 || dintUnitID == 1 && dintSubUnitID == 1)
                            {
                                dstrHostGLSMapping10 = FunStringH.funPaddingStringData("0", 56, '0');
                                dstrProcessGLSMapping10 = FunStringH.funPaddingStringData("0", 56, '0');
                                dstrGLSLoaderMapping10 = FunStringH.funPaddingStringData("0", 56, '0');

                                dstrCSTID = msgTran.Primary().Item("CSTID").Getvalue().ToString();
                                dintCSTType = Convert.ToInt32(msgTran.Primary().Item("CST_TYPE").Getvalue());
                                dstrMapStif = msgTran.Primary().Item("MAP_STIF").Getvalue().ToString();
                                dstrCurStif = msgTran.Primary().Item("CUR_STIF").Getvalue().ToString();

                                int dintGlassCount = Convert.ToInt32(msgTran.Primary().Item("M_COUNT").Getvalue());

                                //pInfo.Port(dintPortID).SlotCount = dintGlassCount;
                                pInfo.Port(dintPortID).GLSHostMapping10 = dstrMapStif;
                                pInfo.Port(dintPortID).GLSLoaderMapping10 = dstrMapStif;
                                pInfo.Port(dintPortID).GLSProcessMapping10_HOST = dstrCurStif;
                                pInfo.Port(dintPortID).GLSRealMapping10 = dstrCurStif;
                                pInfo.Port(dintPortID).CSTType = dintCSTType.ToString();
                                pInfo.Port(dintPortID).arrSlotNo.Clear();
                                //SlotArray += dstrCSTID + ",";
                                for (int dintIndex = 0; dintIndex < dintGlassCount; dintIndex++)
                                {
                                    int.TryParse(msgTran.Primary().Item("USE_COUNT" + dintIndex).Getvalue().ToString(), out dintSlotID);

                                    SlotArray += dintSlotID + ",";
                                    if (pInfo.Port(dintPortID).arrSlotNo.Contains(dintSlotID) == false)
                                    {
                                        pInfo.Port(dintPortID).arrSlotNo.Add(dintSlotID);
                                    }

                                    for (int dintLoop2 = 1; dintLoop2 <= dintSlotID; dintLoop2++)
                                    {
                                        if (pInfo.Port(dintPortID).Slot(dintLoop2) == null)
                                        {
                                            if (pInfo.Port(dintPortID).AddSlot(dintLoop2) == false)
                                            {
                                                pInfo.subLog_Set(clsInfo.LogType.CIM, "Port Slot Add Error!!");
                                                //continue;
                                            }
                                        }
                                    }

                                    clsSlot dpSlot = pInfo.Port(dintPortID).Slot(1);
                                    if (dpSlot == null)
                                    {
                                        pInfo.Port(dintPortID).AddSlot(1);
                                        dpSlot = pInfo.Port(dintPortID).Slot(1);
                                    }


                                    //dpSlot.H_PANELID = msgTran.Primary().Item("H_GLASSID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.GlassID = msgTran.Primary().Item("MATERIALID" + dintIndex).Getvalue().ToString().Trim();
                                    strGLSID = dpSlot.GlassID; //150122 고석현
                                    dpSlot.E_PANELID = msgTran.Primary().Item("M_SETID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.LOTID = msgTran.Primary().Item("LOTID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.BATCHID = msgTran.Primary().Item("BATCHID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.JOBID = msgTran.Primary().Item("JOBID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.PORTID = msgTran.Primary().Item("PORTID1" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.SLOTNO = msgTran.Primary().Item("SLOTNO" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.PRODUCT_TYPE = msgTran.Primary().Item("PROD_TYPE" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.PRODUCT_KIND = msgTran.Primary().Item("PROD_KIND" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.PRODUCTID = msgTran.Primary().Item("PRODUCTID" + dintIndex).Getvalue().ToString().Trim();
                                    //strGLSID = dpSlot.PRODUCTID; //150122 고석현
                                    dpSlot.RUNSPECID = msgTran.Primary().Item("RUNSPECID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.LAYERID = msgTran.Primary().Item("LAYERID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.STEPID = msgTran.Primary().Item("STEPID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.HOSTPPID = msgTran.Primary().Item("PPID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.FLOWID = msgTran.Primary().Item("FLOWID" + dintIndex).Getvalue().ToString().Trim();
                                    int[] arrSIZE = { 0, 0 };
                                    arrSIZE = (int[])msgTran.Primary().Item("M_SIZE" + dintIndex).Getvalue();
                                    //dpSlot.SIZE = msgTran.Primary().Item("SIZE" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.SIZE1 = arrSIZE[0];
                                    dpSlot.SIZE2 = arrSIZE[1];
                                    dpSlot.THICKNESS = Convert.ToInt32(msgTran.Primary().Item("M_THICKNESS" + dintIndex).Getvalue().ToString().Trim());
                                    dpSlot.GLASS_STATE = Convert.ToInt32(msgTran.Primary().Item("M_STATE" + dintIndex).Getvalue().ToString().Trim());
                                    dpSlot.GLASS_ORDER = msgTran.Primary().Item("M_ORDER" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.COMMENT = msgTran.Primary().Item("COMMENT" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.USE_COUNT = msgTran.Primary().Item("USE_COUNT" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.JUDGEMENT = msgTran.Primary().Item("JUDGEMENT" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.REASON_CODE = msgTran.Primary().Item("REASON_CODE" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.INS_FLAG = msgTran.Primary().Item("INS_FLAG" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.ENC_FLAG = msgTran.Primary().Item("LIBRARYID" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.PRERUN_FLAG = msgTran.Primary().Item("PRERUN_FLAG" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.TURN_DIR = msgTran.Primary().Item("TURN_DIR" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.FLIP_STATE = msgTran.Primary().Item("FLIP_STATE" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.WORK_STATE = msgTran.Primary().Item("WORK_STATE" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.MULTI_USE = msgTran.Primary().Item("MULTI_USE" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.PAIR_GLASSID = msgTran.Primary().Item("STAGE_STATE" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.PAIR_PPID = msgTran.Primary().Item("LOCATION" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_NAME[0] = msgTran.Primary().Item("OPTION_NAME1" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_VALUE[0] = msgTran.Primary().Item("OPTION_VALUE1" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_NAME[1] = msgTran.Primary().Item("OPTION_NAME2" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_VALUE[1] = msgTran.Primary().Item("OPTION_VALUE2" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_NAME[2] = msgTran.Primary().Item("OPTION_NAME3" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_VALUE[2] = msgTran.Primary().Item("OPTION_VALUE3" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_NAME[3] = msgTran.Primary().Item("OPTION_NAME4" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_VALUE[3] = msgTran.Primary().Item("OPTION_VALUE4" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_NAME[4] = msgTran.Primary().Item("OPTION_NAME5" + dintIndex).Getvalue().ToString().Trim();
                                    dpSlot.OPTION_VALUE[4] = msgTran.Primary().Item("OPTION_VALUE5" + dintIndex).Getvalue().ToString().Trim();
                                }

                                if (dintAck3 == (int)LOTInfoValidation.Accepted)
                                {
                                    pInfo.Port(dintPortID).HostPPID = dstrHOSTPPID;
                                }
                            }
                        }
                    }
                }

                msgTran.Secondary().Item("ACKC3").Putvalue(dintAck3);
                funSendReply(msgTran);


                //pInfo.subPLCCommand_Set(clsInfo.PLCCommand.LotInfoSet, dintPortID, dintSlotID);

                if (dintAck3 == (int)LOTInfoValidation.Accepted)
                {
                    //[2015/05/20]FI01에서도 저장되게 조건식 추가(Add by HS)
                    if (dintUnitID == 1 && dintSubUnitID == 2 || dintUnitID == 1 && dintSubUnitID == 1)
                    {
                        pInfo.Port(dintPortID).S3F115Received = true;

                        clsSlot dpSlot = pInfo.Port(dintPortID).Slot(1);

                        if (dpSlot != null)
                        {
                            int dintFilmCount = Convert.ToInt32(dpSlot.USE_COUNT);

                            //[2015/06/23]FI02에서만 저장되게 조건식 추가(Add by HS)
                            if (dintUnitID == 1 && dintSubUnitID == 2)
                            {
                                pInfo.subPLCCommand_Set(clsInfo.PLCCommand.LotinformationSend, strGLSID, dintFilmCount, dpSlot.PRODUCTID);
                            }

                            if (pInfo.LOTID(pInfo.Port(dintPortID).CSTID) == null)
                            {
                                pInfo.AddLOT(pInfo.Port(dintPortID).CSTID);
                            }

                            for (int dintLoop = 1; dintLoop <= dintFilmCount; dintLoop++)
                            {
                                if (pInfo.LOTID(pInfo.Port(dintPortID).CSTID).Slot(dintLoop) == null)
                                {
                                    pInfo.LOTID(pInfo.Port(dintPortID).CSTID).AddSlot(dintLoop);
                                }
                                //pInfo.LOTID(strGLSID).Slot(dintLoop).GlassID = strGLSID + dintLoop.ToString().PadLeft(3, '0');
                                pInfo.LOTID(pInfo.Port(dintPortID).CSTID).Slot(dintLoop).GlassID = strGLSID;
                                pInfo.LOTID(pInfo.Port(dintPortID).CSTID).Slot(dintLoop).USE_COUNT = dintLoop.ToString();
                                pInfo.LOTID(pInfo.Port(dintPortID).CSTID).Slot(dintLoop).PRODUCTID = dpSlot.PRODUCTID;
                            }
                        }
                        if (dintPortID == 2)
                        {
                            //this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.FilmJobCommand, 1001, dintPortID);     //Lot Start 지시                    
                        }
                    }
                    //150122 고석현
                    if (dintUnitID == 1 && dintSubUnitID == 1)
                    {
                        pInfo.subPLCCommand_Set(clsInfo.PLCCommand.FI01_Check, "1");
                    }
                }
                else
                {
                    pInfo.Port(dintPortID).S3F115Received = false;
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
                return;
            }
            finally
            {

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
