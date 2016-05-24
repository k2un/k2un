using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventDeparture : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventDeparture(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actDeparture";
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
            string dstrWordAddress = "";
            int dintSlotID = 0;
            StringBuilder dstrLog = new StringBuilder();
            string[] dstrValue = null;
            string dstrGLSID = "";
            //int dintTemp = 0;
            //string dstrModuleID = "";
            //string dstrStepID = "";
            //string dstrHOSTPPID = "";
            string[] dstrDataValue = new string[4];     //MCC Log Data
            string dstrJudge = "";

            bool dbolProcChanged = false;
            string strMCCData = "";
            try
            {
                int intUnitID = Convert.ToInt32(parameters[2]);
                int intSubUnitID = Convert.ToInt32(parameters[3]);
                int intModuleNo = Convert.ToInt32(parameters[1]);

                switch (intModuleNo)
                {
                    case 7: //FT01
                        dstrWordAddress = "W22D0";
                        break;
                    case 9: //AL01
                        dstrWordAddress = "W2310";
                        break;
                    case 10: //LM01
                        dstrWordAddress = "W2330";
                        break;
                    case 11: //DM01
                        dstrWordAddress = "W2350";
                        break;
                    case 12: //IS01
                        dstrWordAddress = "W2370";
                        break;
                    case 8: //FT02
                        dstrWordAddress = "W22F0";
                        break;
                    case 13:
                    case 14:
                        dstrWordAddress = "W21E0";
                        break;
                    case 15:
                        dstrWordAddress = "W24F0";
                        break;
                }

                if (intModuleNo == 13 || intModuleNo == 14)
                {
                    m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);
                    m_pEqpAct.subWordReadSave("", 8, EnuEQP.PLCRWType.ASCII_Data);
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data); //150122 고석현

                }
                else
                {
                    m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);
                    //m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.ASCII_Data);
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);//150429 고석현 수정 SlotNo/FilmNo 숫자로 쓰여짐.

                }
                dstrValue = m_pEqpAct.funWordReadAction(true);

                m_pEqpAct.subSetConfirmBit(strCompBit);

                if ((intModuleNo != 13 && intModuleNo != 14 && intModuleNo != 15) && dstrValue[0].Length > 13)
                {
                    dstrValue[0] = dstrValue[0].Substring(0, 13);
                }

                dstrWordAddress = "W1680";
                string dstrFilmID = m_pEqpAct.funWordRead(dstrWordAddress, 50, EnuEQP.PLCRWType.ASCII_Data);//1605023 keun strLotID -> strFilmID로 수정.

                dstrGLSID = dstrValue[0].Trim();
                dintSlotID = Convert.ToInt32(dstrValue[1].Trim());

                if (intModuleNo == 13 || intModuleNo == 14 || intModuleNo == 15)
                {
                    if (pInfo.GLSID(dstrGLSID) == null)
                    {
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "GlassID null(subACTDepature) -> GlassID: " + dstrFilmID.Trim() + ", SlotID:" +
                                                dintSlotID.ToString() + ", UnitID: " + intUnitID.ToString()); //1605023 keun strLotID -> strFilmID로 수정.
                    }
                }

                //장비에 GLS Out시 읽은 Data를 로그를 남긴다.

                //dstrLog.Append("FilmID:" + dstrGLSID + ",");
                //dstrLog.Append("SlotID:" + dintSlotID + ",");
                //dstrLog.Append("Film Departure-> UnitID:" + intUnitID.ToString() + ",");
                //dstrLog.Append(intUnitID.ToString());
                
                //pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, dstrLog.ToString());


                //dstrGLSID = pInfo.LOTID(dstrLOTID).Slot(dintSlotID).H_PANELID;

                // 20130220 이상창.. 여기가 위치가맞나..?
                // 상태 변경 후에 보고하도록 변경해야하나..?
                #region "Process Step - 생략"
                //string dstrModuleID = pInfo.Unit(intUnitID).SubUnit(0).ModuleID;

                //InfoAct.clsSlot currentSlot = pInfo.LOTID(dstrLOTID).Slot(dintSlotID);

                //foreach (InfoAct.clsProcessStep tmpProcStep in this.pInfo.Unit(0).SubUnit(0).ProcessStepValues())
                //{
                //    if (dstrModuleID.Equals(tmpProcStep.StartModuleID) || dstrModuleID.Equals(tmpProcStep.EndModuleID))
                //    {
                //        if (tmpProcStep.ProcessEvent.ToUpper().Equals("END"))
                //        {
                //            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 27, 0, currentSlot.LOTID, currentSlot.SlotID);
                //        }
                //    }

                //    if (!dbolProcChanged && dstrModuleID.Equals(tmpProcStep.EndModuleID))
                //    {
                //        if (pInfo.Unit(0).SubUnit(0).CurrGLS(dstrGLSID) != null)
                //        {
                //            currentSlot.StepNo_OLD = currentSlot.StepNo;
                //            currentSlot.StepNo = tmpProcStep.StepNO;
                //        }

                //        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedProcessStatusEvent, 23, 0, currentSlot.LOTID, currentSlot.SlotID);

                //        dbolProcChanged = true;
                //    }
                //}

                #endregion

                //현재 장비에서 GLS가 나갔음을 저장(S1F6(SFCD=3) 보고시 사용)
                pInfo.Unit(intUnitID).SubUnit(0).RemoveCurrGLS(dstrGLSID);

                //unit별로 현재 Glass ID 초기화
                pInfo.Unit(intUnitID).SubUnit(0).HGLSID = "";
                pInfo.Unit(intUnitID).SubUnit(0).FilmCount = 0;
                pInfo.Unit(intUnitID).SubUnit(0).FilmExist = false;
                pInfo.Unit(intUnitID).SubUnit(0).FilmID = "";
                

                // 마지막 유닛이다..
                if (intModuleNo == 13 || intModuleNo == 14)
                {

                    pInfo.All.ST01GLSID = dstrGLSID; 
                    //ProcessData 확인
                    if (pInfo.RPC(dstrGLSID) != null)
                    {
                        this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F133RPCEnd, dstrGLSID);
                        if (pInfo.APC(dstrGLSID) != null)
                        {
                            pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ProcessDataDel, InfoAct.clsInfo.ProcessDataType.APC, "2", "2!" + dstrGLSID, false);
                            this.pInfo.All.APCDBUpdateCheck = true;
                        }
                    }
                    else if (pInfo.APC(dstrGLSID) != null)
                    {
                        this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F113APCEnd, dstrGLSID);

                    }

                    InfoAct.clsGLS CurrentGLS1 = pInfo.GLSID(dstrGLSID);
                    if (!string.IsNullOrEmpty(dstrValue[2]))
                    {
                        if (pInfo.LOTID(dstrValue[2]) != null)
                        {
                            if (pInfo.LOTID(dstrValue[2]).Slot(Convert.ToInt32(dstrValue[3])) != null)
                            {
                                CurrentGLS1.FilmID = pInfo.LOTID(dstrValue[2]).Slot(Convert.ToInt32(dstrValue[3])).GlassID;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(dstrValue[3])) CurrentGLS1.USE_COUNT = dstrValue[3].PadLeft(3, '0'); //150122 고석현

                    string dstrMCCWordAddress = "W1D34";
                    dstrMCCWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrMCCWordAddress, 64 * (intModuleNo - 12));
                    string strGlassData = "";
                    strGlassData += m_pEqpAct.funWordWriteString(1, "0", EnuEQP.PLCRWType.Int_Data);

                    m_pEqpAct.funWordWrite(dstrMCCWordAddress, strGlassData, EnuEQP.PLCRWType.Hex_Data);

                dstrLog.Clear();
                dstrLog.Append(dstrGLSID + ",");
                dstrLog.Append(dintSlotID + ",");
                    string strTemp = "";
                switch (intModuleNo)
                {
                    case 13:
                        if (string.IsNullOrEmpty(dstrValue[2]) == false)
                        {
                            strTemp = dstrValue[2];
                        }
                        dstrLog.Append(string.Format("ST01 Glass OUT / {0} / {1} / {2} ", CurrentGLS1.FilmID, CurrentGLS1.USE_COUNT, strTemp));

                        break;
                    case 14:
                        if (string.IsNullOrEmpty(dstrValue[2]) == false)
                        {
                            strTemp = dstrValue[2];
                        }
                        dstrLog.Append(string.Format("ST02 Glass OUT / {0} / {1} / {2} ", CurrentGLS1.FilmID, CurrentGLS1.USE_COUNT, strTemp));
                        break;

                    case 15:
                        dstrLog.Append("GL01 Glass OUT");
                        break;
                }
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, dstrLog.ToString());

                    //Glass Process End
                    //pInfo.LOTID(dstrLOTID).Slot(dintSlotID).EndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //pInfo.LOTID(dstrLOTID).OutCount += 1;

                    //pInfo.GLSID(dstrGLSID).EndTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    

                    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    //sw.Start();

                    subGLSAPDRead(dstrGLSID, dintSlotID);

                    //sw.Stop();
                    //pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "GLSAPD READ Elapsed : " + sw.ElapsedMilliseconds + " ms");

                    // LOT END
                    //if (pInfo.LOTID(dstrLOTID).Slot(dintSlotID).JOBEnd)
                    //{
                    //    pInfo.LOTID(dstrLOTID).EndTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                    //    subLOTAPDRead(dstrLOTID, dintSlotID);
                    //}

                    if (pInfo.Unit(0).SubUnit(0).CurrGLS(dstrGLSID) != null)
                    {
                        pInfo.Unit(0).SubUnit(0).RemoveCurrGLS(dstrGLSID);
                        pInfo.Unit(0).SubUnit(0).GLSExist = false;
                    }
                    if (pInfo.Unit(intUnitID).SubUnit(0).CurrGLS(dstrGLSID) != null)
                    {
                        pInfo.Unit(intUnitID).SubUnit(0).RemoveCurrGLS(dstrGLSID);
                        pInfo.Unit(intUnitID).SubUnit(0).GLSExist = false;

                    }

                    if (pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(dstrGLSID) != null)
                    {
                        pInfo.Unit(intUnitID).SubUnit(intSubUnitID).RemoveCurrGLS(dstrGLSID);
                        pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSExist = false;

                    }
                    if (intUnitID == 1 && intSubUnitID == 2)
                    {
                        //this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.FilmJobCommand, 1004, 2);     //Lot Start 지시                    
                    }

                    //CEID 1025 추가 - 150122 고석현
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcessStatusEvent, 1025, dstrGLSID,intUnitID, intSubUnitID);


                    //Layer2 보고(CEID=16, PANEL PROVESS START for MUDULE)
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 27, intUnitID, intSubUnitID, dstrGLSID, dintSlotID);
                    InfoAct.clsGLS CurrentGLS = pInfo.GLSID(dstrGLSID);

                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 17, intUnitID, intSubUnitID, dstrGLSID, dintSlotID);
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 17, intUnitID, 0, dstrGLSID, dintSlotID);

                    //[2015/04/23]MCC Event Log(Add by HS)
                    strMCCData = "EVENT;";
                    strMCCData += "CEID_17" + ",";
                    strMCCData += pInfo.Unit(intUnitID).SubUnit(intSubUnitID).ModuleID + ",";
                    strMCCData += CurrentGLS.STEPID + ",";
                    strMCCData += CurrentGLS.H_PANELID + ",";
                    strMCCData += CurrentGLS.LOTID + ",";
                    strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                }
                else if(intModuleNo == 15)
                {
                    dstrLog.Clear();
                    dstrLog.Append(dstrGLSID + ",");
                    dstrLog.Append(dintSlotID + ",");
                    switch (intModuleNo)
                    {
                        case 15:
                            dstrLog.Append("GL01 Glass OUT");
                            break;
                    }
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, dstrLog.ToString());


                    if (pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(dstrGLSID) != null)
                    {
                        pInfo.Unit(intUnitID).SubUnit(intSubUnitID).RemoveCurrGLS(dstrGLSID);
                        pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSExist = false;
                    }
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 17, intUnitID, intSubUnitID, dstrGLSID, dintSlotID);
                }
                else
                {
                    InfoAct.clsSlot CurrentSlot = pInfo.LOTID(dstrGLSID).Slot(dintSlotID);
                    string strGlassID = "";
                    if (intModuleNo == 12)
                    {
                        try
                        {
                            string strFilmID = pInfo.Unit(3).SubUnit(6).FilmID;
                            int dintFilmNo = pInfo.Unit(3).SubUnit(6).FilmCount;
                            int dintJudge = Convert.ToInt32(m_pEqpAct.funWordRead("W2012", 1, EnuEQP.PLCRWType.Int_Data));

                            if (m_pEqpAct.funBitRead("B160E", 1) == "1")
                            {
                                strGlassID = pInfo.Unit(3).SubUnit(7).HGLSID;
                            }
                            else
                            {
                                strGlassID = pInfo.Unit(3).SubUnit(8).HGLSID;
                            }

                            if (string.IsNullOrEmpty(strGlassID))
                            {
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Judge GlssID Null, B160E Read Value :" + m_pEqpAct.funBitRead("B160E", 2));
                            }
                            else
                            {
                                pInfo.GLSID(strGlassID).JUDGEMENT = (dintJudge == 1) ? "OK" : "NG";
                                ////pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 15, intUnitID, 0, strGlassID, dintSlotID);
                                ////pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 15, intUnitID, intSubUnitID, strGlassID, dintSlotID);
                                //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1015, intModuleNo, dstrGLSID, dintSlotID, intUnitID, intSubUnitID);
                            }
                            if (pInfo.LOTID(strFilmID) != null && pInfo.LOTID(strFilmID).Slot(dintFilmNo) != null)
                            {
                                pInfo.LOTID(strFilmID).Slot(dintFilmNo).JUDGEMENT = (dintJudge == 1) ? "OK" : "NG";
                            }

                            //[2015/06/30] Juge 추가(Add by HS)
                            if (dintJudge == 1)//OK
                            {
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1015, intModuleNo, dstrGLSID, dintSlotID, intUnitID, intSubUnitID, "OK");
                            }
                            else
                            {
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1015, intModuleNo, dstrGLSID, dintSlotID, intUnitID, intSubUnitID, "NG");
                            }
							
                            //[2015/05/15]MCC Event Log(Add by HS)
                            strMCCData = "EVENT;";
                            strMCCData += "CEID_15" + ",";
                            strMCCData += pInfo.Unit(intUnitID).SubUnit(intSubUnitID).ModuleID + ",";
                            strMCCData += CurrentSlot.STEPID + ",";
                            strMCCData += CurrentSlot.H_PANELID + ",";
                            strMCCData += CurrentSlot.LOTID + ",";
                            strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                            pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);

                        }
                        catch (Exception ex1) 
                        {
                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex1.ToString());
                        }
                    }

                    if (intModuleNo == 11)
                    {
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedJobProcess, 1007, 6, 2, 2, dstrGLSID, dintSlotID);
                    }

                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1017, intModuleNo, dstrGLSID, dintSlotID, intUnitID, intSubUnitID);

                    //[2015/05/15]MCC Event Log(Add by HS)
                    strMCCData = "EVENT;";
                    strMCCData += "CEID_1017" + ",";
                    strMCCData += pInfo.Unit(intUnitID).SubUnit(intSubUnitID).ModuleID + ",";
                    strMCCData += CurrentSlot.STEPID + ",";
                    strMCCData += CurrentSlot.H_PANELID + ",";
                    strMCCData += CurrentSlot.LOTID + ",";
                    strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);

                    if (intModuleNo == 10)
                    {
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1017, intModuleNo, dstrGLSID, dintSlotID, intUnitID, 0);
                    }



                   
                }
            }

            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        

        /// <summary>
        /// Glass APD를 읽어서 파일/구조체에 저장
        /// </summary>
        /// <param name="strLOTID"></param>
        /// <param name="intSlotID"></param>
        private void subGLSAPDRead(string strGLSID, int intSlotID)
        {
            string dstrWriteData = "";
            string dstrTemp = "";
            string dstrWordAddress = "W2780";
            string dstrValue = "";
            string dstrDateTime = "";
            string dstrReadData = "";
            int dintLength = 0;
            int dintStartIndex = 0;
            

            try
            {
                //LOT이 존재하지 않거나 SlotID가 0이면 에러 로그를 출력하고 빠져나간다.
                if (pInfo.GLSID(strGLSID) == null || intSlotID == 0)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subGLSAPDRead: strGlassID:" + strGLSID + ", intSlotID:" + intSlotID.ToString());
                    return;
                }

                ////기본 Data를 구성한다.(6개)
                //dstrTemp = strGLSID;                                                           //LOTID
                //pInfo.LOTID(strLOTID).Slot(intSlotID).GLSAPD(1).Value = dstrTemp;

                //dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                //dstrTemp = pInfo.LOTID(strLOTID).Slot(intSlotID).HOSTPPID;                    //PPID
                //pInfo.LOTID(strLOTID).Slot(intSlotID).GLSAPD(2).Value = dstrTemp;
                //dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                //dstrTemp = pInfo.LOTID(strLOTID).Slot(intSlotID).H_PANELID;               //H_PANELID
                //pInfo.LOTID(strLOTID).Slot(intSlotID).GLSAPD(3).Value = dstrTemp;
                //dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                //dstrTemp = FunStringH.funMakeLengthStringFirst(intSlotID.ToString(), 2);       //SLOTNO
                //pInfo.LOTID(strLOTID).Slot(intSlotID).GLSAPD(4).Value = dstrTemp;
                //dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                //dstrTemp = pInfo.LOTID(strLOTID).Slot(intSlotID).STEPID;                  //STEPID
                //pInfo.LOTID(strLOTID).Slot(intSlotID).GLSAPD(5).Value = dstrTemp;
                //dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                //dstrTemp = pInfo.Unit(0).SubUnit(0).EQPState;
                //pInfo.LOTID(strLOTID).Slot(intSlotID).GLSAPD(6).Value = dstrTemp;
                //dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                //Word를 Block으로 한번에 읽는다.
                dstrReadData = m_pEqpAct.funWordRead(dstrWordAddress, pInfo.All.GLSAPDPLCReadLength * 4, EnuEQP.PLCRWType.Hex_Data).Trim();

                for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).GLSAPDCount; dintLoop++)
                {
                    try
                    {
                        dintLength = pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Length * 4;   //Length에 맞게 자를 문자열 개수를 가져온다.
                        dstrTemp = dstrReadData.Substring(dintStartIndex, dintLength);          //문자열을 자른다.

                        //if (pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).HaveMinusValue == false)
                        //{
                            //날짜 데이터일 경우 읽은 값을 4자리로 끊어서 구조체에 저장
                        if (pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Format.ToUpper() == "YY/MM/DD/HH/MM/SS")
                            {
                                dstrValue = dstrTemp;

                                for (int dintStep = 1; dintStep <= dintLength / 4; dintStep++)
                                {
                                    dstrTemp = dstrValue.Substring((dintStep - 1) * 4, 4);    //문자열을 잘라온다.
                                    dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);  //PLC로 읽은 Hex를 10진수(Int)로 변환한다.
                                    if (dintStep != 1)
                                    {
                                        dstrTemp = FunStringH.funMakeLengthStringFirst(dstrTemp, 2); //월, 일, 시, 분, 초를 2자리로 맞춘다.
                                    }
                                    dstrDateTime = dstrDateTime + dstrTemp + "/";
                                }
                                dstrDateTime = dstrDateTime.Substring(0, dstrDateTime.Length - 1);

                                dstrValue = dstrDateTime;    //최종 변환한 값
                                dstrDateTime = "";          //초기화
                            }
                            else
                            {
                                //2 Word일때 처리
                                if (pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Length > 1)
                                {
                                    if (pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Length == 8)
                                    {
                                        string strTemp = "";

                                        for (int dintLoop2 = 0; dintLoop2 < 7; dintLoop2++)
                                        {
                                            string strTemp2 = dstrTemp.Substring(dintLoop2 * 4, 4);
                                            strTemp2 = strTemp2.Substring(2, 2) + strTemp2.Substring(0, 2);
                                            strTemp += FunTypeConversion.funHexConvert(strTemp2, EnuEQP.StringType.ASCString);
                                        }
                                        dstrTemp = strTemp;
                                        //dstrTemp = FunTypeConversion.funHexConvert(strTemp, EnuEQP.StringType.ASCString);
                                    }
                                    else
                                    {
                                        dstrTemp = dstrTemp.Substring(4, 4) + dstrTemp.Substring(0, 4);
                                        dstrTemp = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
                                    }

                                    dstrValue = dstrTemp;       //최종 변환된 값
                                }
                                else
                                {
                                    //1 Word임.
                                    dstrValue = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);  //PLC로 읽은 Hex를 10진수(Int)로 변환한다.
                                }
                            }
                        //}
                        //else
                        //{
                        //    //항목의 값에 음수값이 있는 경우 음수 값 처리
                        //    dstrValue = FunTypeConversion.funPlusMinusAPDCalc(dstrTemp);
                        //}

                        //소수점을 붙인다.
                        dstrValue = CommonAct.FunStringH.funPoint(dstrValue, pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Format);

                    }
                    catch
                    {
                        dstrValue = "0";
                    }
                    finally
                    {
                        
                        pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Value = dstrValue;   //구조체에 저장
                        dstrWriteData = dstrWriteData + dstrValue + ",";                                 //파일로 쓰기 위해 결합

                        //읽을 Index 증가
                        dintStartIndex = dintStartIndex + dintLength;

                        #region LOTAPD
                        //LOT APD에 data를 저장한다.(후에 LOT APD 보고시 사용)
                        //switch (pInfo.Unit(0).SubUnit(0).LOTAPD(dintLoop).Type)
                        //{
                        //    case "DateTime":
                        //        break;

                        //    case "Other":       //Overwrite함.
                        //        pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = dstrValue;
                        //        break;

                        //    case "Boolean":     //하나라도 1인 값이 있으면 1로 설정한다.(0 혹은 1의 값 경우)
                        //        if (pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value == "1")
                        //        {
                        //        }
                        //        else
                        //        {
                        //            pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = dstrValue;
                        //        }
                        //        break;

                        //    case "Numeric":     //값을 누적한다.

                        //        dstrTemp = pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Name.Substring(pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Name.LastIndexOf("_") + 1).ToUpper();

                        //        if (dstrTemp == "MIN")
                        //        {
                        //            if (pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value == "")        //첫번째 값이 들어올때는 그 값을 Min으로 한다.
                        //            {
                        //                pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = dstrValue;
                        //            }
                        //            else
                        //            {
                        //                //기존의 값 > 새로 들어온값 ---> 새로들오온 값을 Min으로 저장
                        //                if (Convert.ToSingle(pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value) > Convert.ToSingle(dstrValue))
                        //                {
                        //                    pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = dstrValue;
                        //                }
                        //            }
                        //        }
                        //        else if (dstrTemp == "MAX")
                        //        {
                        //            if (pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value == "")        //첫번째 값이 들어올때는 그 값을 Max으로 한다.
                        //            {
                        //                pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = dstrValue;
                        //            }
                        //            else
                        //            {
                        //                //기존의 값 < 새로 들어온값 ---> 새로들오온 값을 Max으로 저장
                        //                if (Convert.ToSingle(pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value) < Convert.ToSingle(dstrValue))
                        //                {
                        //                    pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = dstrValue;
                        //                }
                        //            }
                        //        }
                        //        else if (dstrTemp == "AVG")
                        //        {
                        //            if (pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value == "") pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = "0";

                        //            //기존의 값 + 새로 들어온 값을 계속 더한다.(평균은 subLOTAPDRead()에서 한다.)
                        //            pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = Convert.ToString(Convert.ToSingle(pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value) + Convert.ToSingle(dstrValue));
                        //        }
                        //        else
                        //        {
                        //            if (pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value == "") pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = "0";

                        //            //기존의 값 + 새로 들어온 값을 계속 더한다.(평균은 subLOTAPDRead()에서 한다.)
                        //            pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = Convert.ToString(Convert.ToSingle(pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value) + Convert.ToSingle(dstrValue));
                        //        }

                        //        break;

                        //    //case "Numeric":     //값을 누적한다.
                        //    //    if (m_pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value == "") m_pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = "0";

                        //    //    m_pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = Convert.ToString(Convert.ToSingle(m_pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value) + Convert.ToSingle(dstrValue));

                        //    //    break;
                        //}
                        #endregion
                    }
                }

                //마지막의 콤마는 제거
                dstrWriteData = dstrWriteData.Remove(dstrWriteData.Length - 1);

                //파일에 저장
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSPDC, dstrWriteData);

                //HOST로 보고(S6F11, CEID=81, Process Data(GLS))
                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F13GLSAPD, strGLSID, intSlotID, strGLSID);

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strGLSID:" + strGLSID + ", intSlotID:" + intSlotID.ToString());
            }
        }

        /// <summary>
        /// LOT APD를 읽어서 파일/구조체에 저장
        /// </summary>
        /// <param name="strLOTID"></param>
        /// <param name="intSlotID"></param>
        private void subLOTAPDRead(string strLOTID, int intSlotID)
        {
            string dstrWriteData = "";
            string dstrTemp = "";

            try
            {
                //LOT이 존재하지 않거나 SlotID가 0이면 에러 로그를 출력하고 빠져나간다.
                if (pInfo.LOTID(strLOTID) == null)
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "subLOTAPDRead: strLOTID:" + strLOTID);
                    return;
                }

                //기본 Data를 구성한다.(6개)
                dstrTemp = strLOTID;
                pInfo.LOTID(strLOTID).LOTAPD(1).Value = dstrTemp;                       //LOTID
                dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                dstrTemp = pInfo.LOTID(strLOTID).Slot(intSlotID).HOSTPPID;                    //PPID
                pInfo.LOTID(strLOTID).LOTAPD(2).Value = dstrTemp;
                dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                dstrTemp = "";
                pInfo.LOTID(strLOTID).LOTAPD(3).Value = dstrTemp;                       //H_PANELID
                dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                dstrTemp = "";
                pInfo.LOTID(strLOTID).LOTAPD(4).Value = dstrTemp;                       //SLOTNO
                dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                dstrTemp = pInfo.LOTID(strLOTID).Slot(intSlotID).STEPID;                  //STEPID
                pInfo.LOTID(strLOTID).LOTAPD(5).Value = dstrTemp;
                dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합

                dstrTemp = pInfo.Unit(0).SubUnit(0).EQPState;
                pInfo.LOTID(strLOTID).LOTAPD(6).Value = dstrTemp;
                dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합


                ////LOT Start Time
                //m_pInfo.LOTID(strLOTID).LOTAPD(1).Value = m_pInfo.LOTID(strLOTID).StartTime;

                ////LOT End Time
                //m_pInfo.LOTID(strLOTID).LOTAPD(2).Value = m_pInfo.LOTID(strLOTID).EndTime;


                //LOT Start, End Time을 제외한 나머지 항목은 CIM에서 평균값을 계산한다.
                for (int dintLoop = 7; dintLoop <= pInfo.Unit(0).SubUnit(0).LOTAPDCount; dintLoop++)
                {
                    if (pInfo.Unit(0).SubUnit(0).LOTAPD(dintLoop).Type == "Numeric")
                    {
                        dstrTemp = pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Name.Substring(pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Name.LastIndexOf("_") + 1).ToUpper();
                        if (dstrTemp == "AVG")
                        {
                            //평균값을 계산한다.
                            dstrTemp = Convert.ToString(Convert.ToSingle(pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value) / pInfo.LOTID(strLOTID).OutCount);
                            //dstrTemp = Convert.ToString(Convert.ToInt32(m_pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value) / m_pInfo.LOTID(strLOTID).OutCount);

                            //숫자형 Data일 경우 소수점을 붙인다. //subGLSAPDRead에서 Data Format을 맞추고 누적저장하기 때문에 사용안함. 101028 김중권
                            //dstrTemp = FunStringH.funPoint(dstrTemp, m_pInfo.Unit(0).SubUnit(0).LOTAPD(dintLoop).Format);     
                            pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value = dstrTemp;   //구조체에 저장
                        }
                        else
                        {
                            dstrTemp = pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value;
                        }
                    }
                    else
                    {
                        dstrTemp = pInfo.LOTID(strLOTID).LOTAPD(dintLoop).Value;
                    }

                    dstrWriteData = dstrWriteData + dstrTemp + ",";                                 //파일로 쓰기 위해 결합
                }

                //마지막의 콤마는 제거
                dstrWriteData = dstrWriteData.Remove(dstrWriteData.Length - 1);

                //파일에 저장
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.LOTPDC, dstrWriteData);

                //HOST로 보고(S6F11, CEID=91, Process Data(LOT))
                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F13LOTAPD, strLOTID, intSlotID);

                pInfo.LOTID(strLOTID).OutCount = 0;        //초기화

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", strLOTID:" + strLOTID);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
