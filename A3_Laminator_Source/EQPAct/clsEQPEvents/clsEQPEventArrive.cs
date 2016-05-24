using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Threading;

namespace EQPAct
{
    public class clsEQPEventArrive : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventArrive(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actArrive";
        }
        #endregion


        #region Methods
        /// <summary>
        /// 설비에서 CIM으로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : strCompBit
        /// parameters[1] : dstrACTVal
        /// parameters[2] : dintActFrom
        /// parameters[3] : dstrACTFromSub
        /// parameters[4] : intBitVal
        /// parameters[5] : Special Parameter
        /// </remarks>
        public void funProcessEQPEvent(string[] parameters)
        {
             string strCompBit = parameters[0];
            int intUnitID = Convert.ToInt32(parameters[2]);
            int intSubUnitID = Convert.ToInt32(parameters[3]);
            int intModuleNo = Convert.ToInt32(parameters[1]);
            StringBuilder dstrLog = new StringBuilder();
            string[] dstrValue = null;
            string dstrHGLSID = "";
            string dstrLOTID = "";
            int dintSlotID = 0;
            string dstrGlassSize1 = "";
            string dstrGlassSize2 = "";
            InfoAct.clsSlot dslot = new InfoAct.clsSlot(0);
            string[] dstrDataValue = new string[4];     //MCC Log Data
            int dintIndex = 0;
            int dintPortID = 0;     //LD/UD 작업 Port
            int dintJobStart = 0;
            int dintJobEnd = 0;
            bool dbolXPCStart = false;
            bool dbolProcChanged = false;

            InfoAct.clsGLS CurrentGLS;
            string strMCCData = "";
			//[2015/05/18](Add by HS)
            string dstrWordAddress = "";
            int nIndex = 0;
            try
            {


                #region Unit1

                if (intModuleNo == 13 || intModuleNo == 14 || intModuleNo == 15) //ST01
                {
                    try
                    {
                        pInfo.All.RPCPPID = "";
                        if (intModuleNo != 15)
                        {
                            m_pEqpAct.subWordReadSave("W2040", 8, EnuEQP.PLCRWType.ASCII_Data);               //H-Glass(=panel)-ID
                        }
                        else
                        {
                            m_pEqpAct.subWordReadSave("W2380", 8, EnuEQP.PLCRWType.ASCII_Data);               //H-Glass(=panel)-ID

                        }
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //E-Glass(=panel)-ID
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Lot-ID
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Batch-ID
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Job-ID
                        m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Port-ID
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Slot-NO
                        m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Prod-Type
                        m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Prod-Kind
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //ProductID
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Runspec-ID
                        m_pEqpAct.subWordReadSave("", 4, EnuEQP.PLCRWType.ASCII_Data);                            //Layer-ID
                        m_pEqpAct.subWordReadSave("", 4, EnuEQP.PLCRWType.ASCII_Data);                            //Step-ID
                        m_pEqpAct.subWordReadSave("", 10, EnuEQP.PLCRWType.ASCII_Data);                           //HOST PPID
                        m_pEqpAct.subWordReadSave("", 10, EnuEQP.PLCRWType.ASCII_Data);                            //Flow-ID

                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Glass(=panel)-Size(하위)         ---->U2 2임.(하위 + Space(1) + 상위)
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Glass(=panel)-Size(상위)         ---->U2 2임.(하위 + Space(1) + 상위)
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Glass-thickness(thickness)
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Glass-State(빈값)

                        m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Glass-Order
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Comment

                        m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Use-Count
                        m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Judgement
                        m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Reason-Code
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Inspection-Flag
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Enc-Flag
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Prerun-Flag
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Turn-Dir
                        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);                            //Flip-State
                        m_pEqpAct.subWordReadSave("", 2, EnuEQP.PLCRWType.ASCII_Data);                            //Work-State
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Multi-Use
                        m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);                            //Pair Glass-ID
                        m_pEqpAct.subWordReadSave("", 10, EnuEQP.PLCRWType.ASCII_Data);                           //Pair PPID

                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name1
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value1
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name2
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value2
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name3
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value3
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name4
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value4
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                           //Option Name5
                        m_pEqpAct.subWordReadSave("", 20, EnuEQP.PLCRWType.ASCII_Data);                              //Option Value5


                        //m_pEqpAct.subWordReadSave("", 11, EnuEQP.PLCRWType.Hex_Data);                              //Spare
                        //m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //상부 / 하부 작업 진행여부 확인
                        //m_pEqpAct.subWordReadSave("", 156, EnuEQP.PLCRWType.Hex_Data);                              //Spare

                        //m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Job Start
                        //m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                              //Job End



                        //장비에서 GLS Data정보를 한꺼번에 읽는다.
                        dstrValue = m_pEqpAct.funWordReadAction(true);


                        //읽은 GLS Data를 Parsing해서 변수에 저장
                        dslot.H_PANELID = dstrValue[0].Trim();
                        dslot.E_PANELID = dstrValue[1].Trim();
                        dslot.LOTID = dstrValue[2].Trim();
                        dslot.BATCHID = dstrValue[3].Trim();
                        dslot.JOBID = dstrValue[4].Trim();
                        dslot.PORTID = dstrValue[5].Trim();
                        dintPortID = Convert.ToInt32(FunStringH.funMakeLengthStringFirst(dslot.PORTID, 4).Substring(2, 2));
                        dslot.SlotID = Convert.ToInt32(dstrValue[6]);
                        dslot.SLOTNO = dstrValue[6];
                        dslot.PRODUCT_TYPE = dstrValue[7].Trim();
                        dslot.PRODUCT_KIND = dstrValue[8].Trim();
                        dslot.PRODUCTID = dstrValue[9].Trim();
                        dslot.RUNSPECID = dstrValue[10].Trim();
                        dslot.LAYERID = dstrValue[11].Trim();
                        dslot.STEPID = dstrValue[12].Trim();
                        dslot.HOSTPPID = dstrValue[13].Trim();
                        dslot.FLOWID = dstrValue[14].Trim();
                        dstrGlassSize1 = dstrValue[15].Trim();
                        dstrGlassSize2 = dstrValue[16].Trim();
                        dslot.SIZE = dstrGlassSize1 + " " + dstrGlassSize2;
                        dslot.THICKNESS = Convert.ToInt32(dstrValue[17]);
                        //dstrValue[18]은 Reserved임. Glass State
                        dslot.GLASS_ORDER = dstrValue[19].Trim();
                        dslot.COMMENT = dstrValue[20].Trim();
                        dslot.USE_COUNT = dstrValue[21].Trim();
                        dslot.JUDGEMENT = dstrValue[22].Trim();
                        dslot.REASON_CODE = dstrValue[23].Trim();
                        dslot.INS_FLAG = dstrValue[24].Trim();
                        dslot.ENC_FLAG = dstrValue[25].Trim();
                        dslot.PRERUN_FLAG = dstrValue[26].Trim();
                        dslot.TURN_DIR = dstrValue[27].Trim();
                        dslot.FLIP_STATE = dstrValue[28].Trim();
                        dslot.WORK_STATE = dstrValue[29].Trim();
                        dslot.MULTI_USE = dstrValue[30].Trim();

                        dslot.PAIR_GLASSID = dstrValue[31].Trim();
                        dslot.PAIR_PPID = dstrValue[32].Trim();

                        for (int dintLoop = 0; dintLoop <= dslot.OPTION_NAME.Length - 1; dintLoop++)
                        {
                            dslot.OPTION_NAME[dintLoop] = dstrValue[dintIndex + 33].Trim();
                            dslot.OPTION_VALUE[dintLoop] = dstrValue[dintIndex + 34].Trim();
                            //dslot.OPTION_NAME[dintLoop] = "0";
                            //dslot.OPTION_VALUE[dintLoop] = "0";
                            dintIndex = dintIndex + 2;
                        }
                        
                        dintIndex = 0;


                        //상부, 하부 확인 - ksh  //A3 사용안함
                        //if (dstrValue[44] == "0")
                        //{
                        //    pInfo.All.GlassUpperJobFlag = true;
                        //}
                        //else
                        //{
                        //    pInfo.All.GlassUpperJobFlag = false;
                        //}

                        //dintJobStart = Convert.ToInt32(dstrValue[46]);
                        //dintJobEnd = Convert.ToInt32(dstrValue[47]);

                        //if (dintJobStart == 1) dslot.JOBStart = true;
                        //else dslot.JOBStart = false;

                        //if (dintJobEnd == 1) dslot.JOBEnd = true;
                        //else dslot.JOBEnd = false;

                        dstrLOTID = dslot.LOTID;
                        dintSlotID = dslot.SlotID;
                        dstrHGLSID = dslot.H_PANELID;

                        if (intModuleNo == 13)
                        {
                            m_pEqpAct.funWordWrite("W1FC0", dslot.H_PANELID.PadRight(16, ' '), EnuEQP.PLCRWType.ASCII_Data);
                        }
                        else if (intModuleNo == 14)
                        {
                            m_pEqpAct.funWordWrite("W1FD0", dslot.H_PANELID.PadRight(16, ' '), EnuEQP.PLCRWType.ASCII_Data);
                        }
                        else
                        {
                            m_pEqpAct.funWordWrite("W1FE0", dslot.H_PANELID.PadRight(16, ' '), EnuEQP.PLCRWType.ASCII_Data);
                        }
                    }
                    catch (Exception ex)
                    {
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    }
                    finally
                    {
                        //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                        m_pEqpAct.subSetConfirmBit(strCompBit);
                    }

                    //장비에 GLS In시 읽은 Data를 로그를 남긴다.
                    dstrLog.Append("LOTID:" + dslot.LOTID + ",");
                    dstrLog.Append("SlotID:" + dslot.SlotID + ",");
                    dstrLog.Append("GLS Arrive   -> UnitID:" + intUnitID.ToString() + ",");
                    dstrLog.Append(intUnitID.ToString()+",");
                    dstrLog.Append("HGLSID:" + dslot.H_PANELID + ",");
                    dstrLog.Append("EGLSID:" + dslot.E_PANELID + ",");
                    dstrLog.Append("BatchID:" + dslot.BATCHID + ",");
                    dstrLog.Append("JobID:" + dslot.JOBID + ",");
                    dstrLog.Append("PortID:" + dslot.PORTID + ",");
                    dstrLog.Append("ProdType:" + dslot.PRODUCT_TYPE + ",");
                    dstrLog.Append("ProdKind:" + dslot.PRODUCT_KIND + ",");
                    dstrLog.Append("ProdID:" + dslot.PRODUCTID + ",");
                    dstrLog.Append("RunspecID:" + dslot.RUNSPECID + ",");
                    dstrLog.Append("LayerID:" + dslot.LAYERID + ",");
                    dstrLog.Append("StepID:" + dslot.STEPID + ",");
                    dstrLog.Append("HOSTPPID:" + dslot.HOSTPPID + ",");
                    dstrLog.Append("FlowID:" + dslot.FLOWID + ",");
                    dstrLog.Append("GlassSize1:" + dstrGlassSize1 + ",");
                    dstrLog.Append("GlassSize2:" + dstrGlassSize2 + ",");
                    dstrLog.Append("Glassthickness:" + dslot.THICKNESS.ToString() + ",");
                    dstrLog.Append("GlassOrder:" + dslot.GLASS_ORDER + ",");
                    dstrLog.Append("Comment:" + dslot.COMMENT + ",");

                    dstrLog.Append("UseCount:" + dslot.USE_COUNT + ",");
                    dstrLog.Append("Judgement:" + dslot.JUDGEMENT + ",");
                    dstrLog.Append("ReasonCode:" + dslot.REASON_CODE + ",");
                    dstrLog.Append("InspectionFlag:" + dslot.INS_FLAG + ",");
                    dstrLog.Append("EncFlag:" + dslot.ENC_FLAG + ",");
                    dstrLog.Append("PrerunFlag:" + dslot.PRERUN_FLAG + ",");
                    dstrLog.Append("TurnDir:" + dslot.TURN_DIR + ",");
                    dstrLog.Append("FlipState:" + dslot.FLIP_STATE + ",");
                    dstrLog.Append("WorkState:" + dslot.WORK_STATE + ",");
                    dstrLog.Append("MultiUse:" + dslot.MULTI_USE + ",");

                    dstrLog.Append("PairGlassID:" + dslot.PAIR_GLASSID + ",");
                    dstrLog.Append("PairPPID:" + dslot.PAIR_PPID + ",");

                    for (int dintLoop = 0; dintLoop <= dslot.OPTION_NAME.Length - 1; dintLoop++)
                    {
                        dstrLog.Append("OptionName" + Convert.ToString(dintLoop + 1) + ":" + dslot.OPTION_NAME[dintLoop] + ",");
                        dstrLog.Append("OptionValue" + Convert.ToString(dintLoop + 1) + ":" + dslot.OPTION_VALUE[dintLoop] + ",");
                    }

                    dstrLog.Append("JobStart:" + dintJobStart.ToString() + ",");
                    dstrLog.Append("JobEnd:" + dintJobEnd.ToString());

                    //pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, dstrLog.ToString());

                    dstrLog.Clear();
                    dstrLog.Append(dslot.GlassID + ",");
                    dstrLog.Append(dslot.SLOTNO + ",");

                    switch (intModuleNo)
                    {
                        case 13:
                            dstrLog.Append("ST01 Glass IN");
                            break;
                        case 14:
                            dstrLog.Append("ST02 Glass IN");
                            break;
                        case 15:
                            dstrLog.Append("GL01 Glass IN");
                            break;
                    }
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, dstrLog.ToString());

                    //LOT의 첫번째 GLS이면 LOT정보를 생성한다.
                    //if (pInfo.LOTID(dslot.LOTID) == null) subCreateLOTInfo(dslot.LOTID, dintJobStart);
                    if (pInfo.GLSID(dslot.H_PANELID) == null)
                    {
                        pInfo.AddGLS(dslot.H_PANELID);
                    }

                    pInfo.GLSID(dslot.H_PANELID).CopyFrom(dslot);

                    CurrentGLS = pInfo.GLSID(dslot.H_PANELID);
                    
                    ////GLS Data를 구조체에 저장
                    //InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);
                    //currentSlot.CopyFrom(dslot);

                    CurrentGLS.StartTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    CurrentGLS.GLASS_STATE = 3;      //Processing

                    //pInfo.LOTID(dstrLOTID).InCount += 1;   //LOT의 GLS개수 증가

                    if (pInfo.Unit(0).SubUnit(0).AddCurrGLS(CurrentGLS.H_PANELID) == true)
                    {
                        InfoAct.clsGLS curGLS = pInfo.Unit(0).SubUnit(0).CurrGLS(CurrentGLS.H_PANELID);
                        curGLS.H_PANELID = CurrentGLS.H_PANELID;
                        curGLS.SlotID = CurrentGLS.SlotID;
                        pInfo.Unit(0).SubUnit(0).GLSExist = true;
                    }

                    if (pInfo.Unit(intUnitID).SubUnit(0).AddCurrGLS(CurrentGLS.H_PANELID) == true)
                    {
                        InfoAct.clsGLS curGLS = pInfo.Unit(intUnitID).SubUnit(0).CurrGLS(CurrentGLS.H_PANELID);
                        curGLS.H_PANELID = CurrentGLS.H_PANELID;
                        curGLS.SlotID = CurrentGLS.SlotID;
                        pInfo.Unit(intUnitID).SubUnit(0).GLSExist = true;
                    }

                    if (pInfo.Unit(intUnitID).SubUnit(intSubUnitID).AddCurrGLS(CurrentGLS.H_PANELID) == true)
                    {
                        InfoAct.clsGLS curGLS = pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(CurrentGLS.H_PANELID);
                        curGLS.H_PANELID = CurrentGLS.H_PANELID;
                        curGLS.SlotID = CurrentGLS.SlotID;
                        pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSExist = true;
                    }

                    string dstrOLDHOSTPPID = "";
                    string dstrNEWHOSTPPID = "";
                    if (pInfo.EQP("Main").DummyPLC == false)
                    {
                        if (pInfo.Unit(0).SubUnit(0).HOSTPPID(CurrentGLS.HOSTPPID) == null)
                        {
                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Arrive HostPPID Null!! HostPPID : " + CurrentGLS.HOSTPPID);
                        }

                        dstrOLDHOSTPPID = pInfo.All.CurrentHOSTPPID;   //이전 PPID 백업
                        dstrNEWHOSTPPID = CurrentGLS.HOSTPPID;
                        pInfo.All.CurrentHOSTPPID = dstrNEWHOSTPPID;                                                      //변경된 HOSTPPID를 입력
                        pInfo.All.CurrentEQPPPID = pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrNEWHOSTPPID).EQPPPID;
                    }
                    
                    if (pInfo.All.EQPSpecifiedCtrlBYWHO == "1")// || pInfo.All.EQPSpecifiedCtrlBYWHO == "2")
                    {
                        //HOST나 OP에서 발생한것임
                    }
                    else
                    {
                        pInfo.All.EQPSpecifiedCtrlBYWHO = "2"; //BY EQP
                    }

					
                    //wordwrite시점은 MCC로 정보를 보내기전으로...
                    //MCC로 메세지 전송후 보내면 MCC에서 Data 읽을때 이전Data를 읽을 수 있음.
                    string dstrMCCWordAddress = "W1D00";
                    dstrMCCWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrMCCWordAddress, 64 * (intModuleNo - 13));
                    string strGlassData = "";
                    strGlassData += m_pEqpAct.funWordWriteString(4, CurrentGLS.STEPID.PadRight(4, ' '), EnuEQP.PLCRWType.ASCII_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(28, CurrentGLS.H_PANELID.PadRight(28, ' '), EnuEQP.PLCRWType.ASCII_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(8, CurrentGLS.LOTID.PadRight(8, ' '), EnuEQP.PLCRWType.ASCII_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(10, CurrentGLS.HOSTPPID.PadRight(10, ' '), EnuEQP.PLCRWType.ASCII_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(1, CurrentGLS.SLOTNO, EnuEQP.PLCRWType.Int_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(1, "0", EnuEQP.PLCRWType.Int_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);

                    m_pEqpAct.funWordWrite(dstrMCCWordAddress, strGlassData, EnuEQP.PLCRWType.Hex_Data);



                    DateTime dt = DateTime.Now;
                    strMCCData = "";

                    if (dstrOLDHOSTPPID != dstrNEWHOSTPPID)
                    {
                        //CEID=131보고
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11EquipmentSpecifiedControlEvent, 131, dstrOLDHOSTPPID, dstrNEWHOSTPPID);

                        //[2015/04/23]MCC Event Log(Add by HS)
                        strMCCData = "EVENT;";
                        strMCCData += "CEID_131" + ",";
                        strMCCData += pInfo.Unit(intUnitID).SubUnit(intSubUnitID).ModuleID + ",";
                        strMCCData += CurrentGLS.STEPID + ",";
                        strMCCData += CurrentGLS.H_PANELID + ",";
                        strMCCData += CurrentGLS.LOTID + ",";
                        strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ",";
                        strMCCData += dstrOLDHOSTPPID + ",";
                        strMCCData += dstrNEWHOSTPPID + ";";

                        pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                    }

                    if (pInfo.EQP("Main").EQPID.Contains("A3GLM")) //하부 라미시나리오
                    {
                        if (intModuleNo == 15)
                        {
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 16, intUnitID, 0, dslot.H_PANELID, dslot.SlotID);

                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 16, intUnitID, intSubUnitID, dslot.H_PANELID, dslot.SlotID);
                            //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 26, intUnitID, intSubUnitID, dslot.H_PANELID, dslot.SlotID);
                        }
                        else
                        {
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 16, intUnitID, intSubUnitID, dslot.H_PANELID, dslot.SlotID);
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 26, intUnitID, intSubUnitID, dslot.H_PANELID, dslot.SlotID);
                    //[2015/04/23]MCC Event Log(Add by HS)
                    strMCCData = "EVENT;";
                    strMCCData += "CEID_16" + ",";
                    strMCCData += pInfo.Unit(intUnitID).SubUnit(intSubUnitID).ModuleID + ",";
                    strMCCData += CurrentGLS.STEPID + ",";
                    strMCCData += CurrentGLS.H_PANELID + ",";
                    strMCCData += CurrentGLS.LOTID + ",";
                    strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);

                        }
                    }
                    else//상부 라미 시나리오
                    {
                        //Layer1 보고(CEID=16, PANEL PROCESS START for MODULE)
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 16, intUnitID, 0, dslot.H_PANELID, dslot.SlotID);

                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 16, intUnitID, intSubUnitID, dslot.H_PANELID, dslot.SlotID);
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 26, intUnitID, intSubUnitID, dslot.H_PANELID, dslot.SlotID);
                        //[2015/04/23]MCC Event Log(Add by HS)
                        strMCCData += "EVENT;";
                        strMCCData += "CEID_16" + ",";
                        strMCCData += pInfo.Unit(intUnitID).SubUnit(intSubUnitID).ModuleID + ",";
                        strMCCData += CurrentGLS.STEPID + ",";
                        strMCCData += CurrentGLS.H_PANELID + ",";
                        strMCCData += CurrentGLS.LOTID + ",";
                        strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                        pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                    }
                                       
                    
                    // 20130220 이상창.. 여기가 위치가맞나..?
                    // 상태 변경 후에 보고하도록 변경해야하나..?
                    #region "Process Step"
                    string dstrModuleID = pInfo.Unit(intUnitID).SubUnit(0).ModuleID;

                    //foreach (InfoAct.clsProcessStep tmpProcStep in this.pInfo.Unit(0).SubUnit(0).ProcessStepValues())
                    //{
                    //    if (dstrModuleID.Equals(tmpProcStep.StartModuleID) || dstrModuleID.Equals(tmpProcStep.EndModuleID))
                    //    {
                    //        if (tmpProcStep.ProcessEvent.ToUpper().Equals("START"))
                    //        {
                    //            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 26, 0, dslot.LOTID, dslot.SlotID);
                    //        }
                    //    }

                    //    if (!dbolProcChanged && dstrModuleID.Equals(tmpProcStep.StartModuleID))
                    //    {
                    //        if (pInfo.Unit(0).SubUnit(0).CurrGLS(currentSlot.H_PANELID) != null)
                    //        {
                    //            currentSlot.StepNo_OLD = currentSlot.StepNo;
                    //            currentSlot.StepNo = tmpProcStep.StepNO;
                    //        }

                    //        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedProcessStatusEvent, 23, 0, dslot.LOTID, dslot.SlotID);
                    //        //break;

                    //        dbolProcChanged = true;
                    //    }
                    //}

                    #endregion

                    //2012.11.06 김영식...  Normal/APC/RPC/PPC Start 여부 판정하여 CIM Event 처리
                    // 20121212 lsc
                    if (this.pInfo.Unit(0).SubUnit(0).EOID(this.pInfo.funGetEOIDNameToIndex("RPC")).EOV == 1 && pInfo.RPC(dslot.H_PANELID) != null)
                    {
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F131RPCStart, dstrHGLSID);
                        pInfo.All.RPCDBUpdateCheck = true;
                        CurrentGLS.IsRPCRunning = true;
                        dbolXPCStart = true;
                        pInfo.All.RPCPPID = pInfo.RPC(dstrHGLSID).RPC_PPID;

                        if (pInfo.APC(dslot.H_PANELID) != null)
                        {
                            pInfo.APC(dslot.H_PANELID).State = "2";
                            pInfo.subProcessDataStatusSet(InfoAct.clsInfo.ProcessDataType.APC, dstrHGLSID);
                        }
                    }

                    else if (this.pInfo.Unit(0).SubUnit(0).EOID(this.pInfo.funGetEOIDNameToIndex("APC")).EOV == 1 && pInfo.APC(dslot.H_PANELID) != null)
                    {
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F111APCStart, dstrHGLSID);
                        CurrentGLS.IsAPCRunning = true;
                        dbolXPCStart = true;
                    }

                    if (dbolXPCStart == false)
                    {
                        //pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.NormalStart, dstrHGLSID);
                    }

                    //unit별로 현재 Glass ID 저장
                    pInfo.Unit(intUnitID).SubUnit(0).HGLSID = dslot.H_PANELID;
                    pInfo.Unit(intUnitID).SubUnit(intSubUnitID).HGLSID = dslot.H_PANELID;
                    pInfo.Unit(intUnitID).SubUnit(0).GLSExist = true;

                    
                }
                #endregion
                #region Film구간
                else
                {
                    int dintDEPUnitID = 0;
                    switch (intModuleNo)
                    {
                        case 7: //FT01
                            dstrWordAddress = "W22C0";
                            break;
                        case 9: //AL01
                            dstrWordAddress = "W2300";
                            break;
                        case 10: //LM01
                            dstrWordAddress = "W2320";
                            break;
                        case 11: //DM01
                            dstrWordAddress = "W2340";
                            break;
                        case 12: //IS01
                            dstrWordAddress = "W2360";
                            break;
                        case 8: //FT02
                            dstrWordAddress = "W22E0";
                            break;
                    }

                    if(string.IsNullOrEmpty(dstrWordAddress)) return;
                    
                    m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);         // LOTID
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                      // SlotID

                    string dstrproIDWordAddress = "W16B3";

                    string strPROID = m_pEqpAct.funWordRead(dstrproIDWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data); //20160407 KEUN

                    dstrWordAddress = "W1680";
                    string strFilmID = m_pEqpAct.funWordRead(dstrWordAddress, 50, EnuEQP.PLCRWType.ASCII_Data);//1605023 keun strLotID -> strFilmID로 수정.

                    dstrValue = m_pEqpAct.funWordReadAction(true);

                    m_pEqpAct.subSetConfirmBit(strCompBit);

                    if (dstrValue[0].Length > 13)
                    {
                        dstrValue[0] = dstrValue[0].Substring(0, 13);
                    }
                    dstrLOTID = dstrValue[0].Trim();
                    dintSlotID = Convert.ToInt32(dstrValue[1].Trim());

                    dstrLog.Append("FilmID:" + strFilmID + ",");//1605023 keun strLotID -> strFilmID로 수정.
                    dstrLog.Append("SlotID:" + dintSlotID + ",");
                    dstrLog.Append("Film Arrive   -> UnitID:" + intUnitID.ToString() +",");
                    dstrLog.Append(intUnitID.ToString());
                    //this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, dstrLog.ToString());

                    if (this.pInfo.LOTID(dstrLOTID) == null) subCreateLOTInfo(dstrLOTID, 0);

                    if (pInfo.LOTID(dstrLOTID).Slot(dintSlotID) == null)
                    {
                        pInfo.LOTID(dstrLOTID).AddSlot(dintSlotID);
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Slot No Error!! AddSlot!! LotID : {0}, SlotNo : {1}", dstrLOTID, dintSlotID));
                    }
                    if (string.IsNullOrEmpty(pInfo.LOTID(dstrLOTID).Slot(dintSlotID).GlassID))
                    {
                        pInfo.LOTID(dstrLOTID).Slot(dintSlotID).GlassID = strFilmID.Trim();
                        pInfo.LOTID(dstrLOTID).Slot(dintSlotID).USE_COUNT = dintSlotID.ToString().PadLeft(3, '0');
                        //pInfo.LOTID(dstrLOTID).Slot(dintSlotID).PRODUCTID = dstrLOTID;
                        pInfo.LOTID(dstrLOTID).Slot(dintSlotID).PRODUCTID = strPROID;  //160407 KEUN

                    }

                    //dstrHGLSID = this.pInfo.LOTID(dstrLOTID).Slot(dintSlotID).H_PANELID;
                    pInfo.Unit(intUnitID).SubUnit(0).FilmID = strFilmID; //1605023 keun strLotID -> strFilmID로 수정.
                    pInfo.Unit(intUnitID).SubUnit(0).FilmCount = dintSlotID;
                    pInfo.LOTID(dstrLOTID).Slot(dintSlotID).FilmExistUnitID = intUnitID;
                    pInfo.Unit(intUnitID).SubUnit(0).FilmExist = true;

                    pInfo.Unit(intUnitID).SubUnit(intSubUnitID).FilmID = strFilmID; //1605023 keun strLotID -> strFilmID로 수정.
                    pInfo.Unit(intUnitID).SubUnit(intSubUnitID).FilmCount = dintSlotID;
                    pInfo.LOTID(dstrLOTID).Slot(dintSlotID).FilmExistUnitID = intUnitID;
                    pInfo.Unit(intUnitID).SubUnit(intSubUnitID).FilmExist = true;

                    //wordwrite시점은 MCC로 정보를 보내기전으로...
                    //MCC로 메세지 전송후 보내면 MCC에서 Data 읽을때 이전Data를 읽을 수 있음.
                    string dstrMCCWordAddress = "W1B80";
                    dstrMCCWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrMCCWordAddress, 64 * (intModuleNo - 7));
                    string strGlassData = "";
                    strGlassData += m_pEqpAct.funWordWriteString(4, "0000", EnuEQP.PLCRWType.ASCII_Data);//임시
                    strGlassData += m_pEqpAct.funWordWriteString(28, pInfo.LOTID(dstrLOTID).Slot(dintSlotID).GlassID.PadRight(28, ' '), EnuEQP.PLCRWType.ASCII_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(8, dstrLOTID.PadRight(8, ' '), EnuEQP.PLCRWType.ASCII_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(10, pInfo.All.CurrentHOSTPPID.PadRight(10, ' '), EnuEQP.PLCRWType.ASCII_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(1, dintSlotID.ToString(), EnuEQP.PLCRWType.Int_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(1, "0", EnuEQP.PLCRWType.Int_Data);
                    strGlassData += m_pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);

                    m_pEqpAct.funWordWrite(dstrMCCWordAddress, strGlassData, EnuEQP.PLCRWType.Hex_Data);

                    if (intModuleNo == 7)
                    {
                        //this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.FilmJobCommand, 1006, dintPortID);     //Lot Start 지시
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedJobProcess, 1006, 2, 1, 2, dstrLOTID, dintSlotID);
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1016, intModuleNo, dstrLOTID, dintSlotID, intUnitID, 0);

                    }
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1016, intModuleNo, dstrLOTID, dintSlotID, intUnitID, intSubUnitID);

                    //[2015/05/15]MCC Event Log(Add by HS)
                    InfoAct.clsSlot CurrentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);

                    strMCCData = "EVENT;";
                    strMCCData += "CEID_1016" + ",";
                    strMCCData += pInfo.Unit(intUnitID).SubUnit(intSubUnitID).ModuleID + ",";
                    strMCCData += CurrentSlot.STEPID + ",";
                    strMCCData += CurrentSlot.H_PANELID + ",";
                    strMCCData += CurrentSlot.LOTID + ",";
                    strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                    
                    // 20130220 이상창.. 여기가 위치가맞나..?
                    // 상태 변경 후에 보고하도록 변경해야하나..?
                    #region "Process Step - 생략"
                    //string dstrModuleID = pInfo.Unit(intUnitID).SubUnit(0).ModuleID;

                    //foreach (InfoAct.clsProcessStep tmpProcStep in this.pInfo.Unit(0).SubUnit(0).ProcessStepValues())
                    //{
                    //    if(dstrModuleID.Equals(tmpProcStep.StartModuleID) || dstrModuleID.Equals(tmpProcStep.EndModuleID))
                    //    {
                    //        if (tmpProcStep.ProcessEvent.ToUpper().Equals("START"))
                    //        {
                    //            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 26, 0, currentSlot.LOTID, currentSlot.SlotID);
                    //        }
                    //    }

                    //    if (!dbolProcChanged && dstrModuleID.Equals(tmpProcStep.StartModuleID))
                    //    {
                    //        if (pInfo.Unit(0).SubUnit(0).CurrGLS(currentSlot.H_PANELID) != null)
                    //        {
                    //            currentSlot.StepNo_OLD = currentSlot.StepNo;
                    //            currentSlot.StepNo = tmpProcStep.StepNO;
                    //        }

                    //        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedProcessStatusEvent, 23, 0, currentSlot.LOTID, currentSlot.SlotID);
                    //        //break;

                    //        dbolProcChanged = true;
                    //    }
                    //}

                    #endregion

                }
                #endregion

            }
            catch (Exception ex)
            {
                pInfo.All.RPCPPID = "";
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }

        /// <summary>
        /// LOT 정보를 생성한다.
        /// </summary>
        /// <param name="strLOTID"></param>
        /// <param name="intJobStart"></param>
        /// <comment>
        /// 첫번째 Unit에 GLS 도착시만 체크하는게 아니라 모든 Unit에 GLS 도착시 LOT 정보가 있는가를 체크하여 LOT정보를 생성한다.
        /// CIM PC를 껏다켰을때도 적용하기 위함.
        /// </comment>
        private void subCreateLOTInfo(string strLOTID, int intJobStart)
        {
            try
            {
                //LOTID가 공백이면 나간다.
                if (strLOTID == "") return;


                //LOT의 첫번째 GLS이면 LOT정보를 생성한다.
                if (pInfo.LOTID(strLOTID) == null)
                {
                    if (pInfo.AddLOT(strLOTID) > 0)
                    {
                        //LOT APD를 생성한다.
                        int dintLotApdCount = pInfo.Unit(0).SubUnit(0).LOTAPDCount;

                        for (int dintIndex = 1; dintIndex <= dintLotApdCount; dintIndex++)
                        {
                            if (pInfo.LOTID(strLOTID).AddLOTAPD(dintIndex) == true)
                            {
                                //m_pInfo.LOTID(strLOTID).LOTAPD(dintIndex).Name = m_pInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Name;
                                //m_pInfo.LOTID(strLOTID).LOTAPD(dintIndex).Length = m_pInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Length;
                                //m_pInfo.LOTID(strLOTID).LOTAPD(dintIndex).Format = m_pInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Format;
                                //m_pInfo.LOTID(strLOTID).LOTAPD(dintIndex).Type = m_pInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).Type;
                                ////m_pInfo.LOTID(strLOTID).LOTAPD(dintIndex).UnitID = m_pInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).UnitID;
                                //m_pInfo.LOTID(strLOTID).LOTAPD(dintIndex).ModuleID = m_pInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex).ModuleID;
                                pInfo.LOTID(strLOTID).LOTAPD(dintIndex).CopyFrom(pInfo.Unit(0).SubUnit(0).LOTAPD(dintIndex));
                            }
                            else
                            {
                                //LOTAPD 생성 에러시 로그 출력
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "LOT APD Create Error -> LOTID: " + strLOTID + ", dintIndex: " + dintIndex.ToString());
                            }
                        }

                        //각 Slot별로 GLS APD를 생성한다.
                        for (int dintSlot = 1; dintSlot <= pInfo.EQP("Main").SlotCount; dintSlot++)
                        {
                            for (int dintIndex = 1; dintIndex <= pInfo.Unit(0).SubUnit(0).GLSAPDCount; dintIndex++)
                            {
                                if (pInfo.LOTID(strLOTID).Slot(dintSlot).AddGLSAPD(dintIndex) == true)
                                {
                                    //m_pInfo.LOTID(strLOTID).Slot(dintSlot).GLSAPD(dintIndex).Name = m_pInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Name;
                                    //m_pInfo.LOTID(strLOTID).Slot(dintSlot).GLSAPD(dintIndex).Length = m_pInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Length;
                                    //m_pInfo.LOTID(strLOTID).Slot(dintSlot).GLSAPD(dintIndex).Format = m_pInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).Format;
                                    ////m_pInfo.LOTID(strLOTID).Slot(dintSlot).GLSAPD(dintIndex).UnitID = m_pInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).UnitID;
                                    //m_pInfo.LOTID(strLOTID).Slot(dintSlot).GLSAPD(dintIndex).ModuleID = m_pInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex).ModuleID;
                                    pInfo.LOTID(strLOTID).Slot(dintSlot).GLSAPD(dintIndex).CopyFrom(pInfo.Unit(0).SubUnit(0).GLSAPD(dintIndex));
                                }
                                else
                                {
                                    //GLSAPD 생성 에러시 로그 출력
                                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "GLS APD Create Error -> LOTID: " + strLOTID + ", dintSlot: " +
                                                                                            dintSlot.ToString() + ", dintIndex: " + dintIndex.ToString());
                                }
                            }
                        }

                        //LOT 시작시간
                        pInfo.LOTID(strLOTID).StartTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    }
                    else
                    {
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "LOT Create Error -> LOTID: " + strLOTID);   //LOT정보 생성 에러시 로그 출력
                    }
                }
                else
                {
                    //LOT 정보가 기존에 있는데 Job Start=1인 GLS가 1번 Unit에 들어올때 LOT 정보를 초기화해서 사용
                    //1000매 테스트 같은 하나의 CST로 계속 돌리기 때문에 LOT정보가 같음
                    if (intJobStart == 1)
                    {
                        //LOT APD 값을 초기화한다.
                        for (int dintIndex = 1; dintIndex <= pInfo.Unit(0).SubUnit(0).LOTAPDCount; dintIndex++)
                        {
                            pInfo.LOTID(strLOTID).LOTAPD(dintIndex).Value = "";        //초기화
                        }

                        //각 Slot별로 GLS APD 값을 초기화한다.
                        for (int dintSlot = 1; dintSlot <= pInfo.EQP("Main").SlotCount; dintSlot++)
                        {
                            for (int dintIndex = 1; dintIndex <= pInfo.Unit(0).SubUnit(0).GLSAPDCount; dintIndex++)
                            {
                                pInfo.LOTID(strLOTID).Slot(dintSlot).GLSAPD(dintIndex).Value = ""; //초기화
                            }
                        }

                        //LOT 시작시간
                        pInfo.LOTID(strLOTID).StartTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "LOT Info Initial -> LOTID: " + strLOTID);   //로그 출력
                    }
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strLOTID:" + strLOTID);
            }
        }
        #endregion
    }
}

