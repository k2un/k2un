using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventCaseIn : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventCaseIn(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actCaseIn";
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
            string dstrCaseID = "";
            int dintFilmCount = 0;
            string[] dstrValue = null ;
            StringBuilder dstrLog = new StringBuilder();
            int dintUnitID = 0;
            int dintSubUnitID = 0;
            int dintPortID = 0;
            string strMCCData = "";
            try
            {
                dintPortID = Convert.ToInt32(parameters[1]);
                dintUnitID = Convert.ToInt32(parameters[2]);
                dintSubUnitID = Convert.ToInt32(parameters[3]);
                if (dintUnitID == 1)
                {
                    dstrWordAddress = "W2200";
                    dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 32 * (dintSubUnitID - 1));
                    dstrCaseID = m_pEqpAct.funWordRead(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);
                }
                else
                {
                    dstrWordAddress = "W2280";
                    dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 32 * (dintSubUnitID - 1));
                    dstrCaseID = m_pEqpAct.funWordRead(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);
                }
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM , string.Format ( "CaseIN ==> Case ID : {0}", dstrCaseID));

                pInfo.Port(dintPortID).Initial();
                pInfo.Port(dintPortID).CSTID = dstrCaseID.Trim();
                pInfo.Port(dintPortID).PortState = "2";
                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);

                //wordwrite시점은 MCC로 정보를 보내기전으로...
                //MCC로 메세지 전송후 보내면 MCC에서 Data 읽을때 이전Data를 읽을 수 있음.
                string dstrMCCWordAddress = "W1A00";
                dstrMCCWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrMCCWordAddress, 64 * (dintPortID - 1));
                string strGlassData = "";
                strGlassData += m_pEqpAct.funWordWriteString(4, "0000", EnuEQP.PLCRWType.ASCII_Data);//임시
                strGlassData += m_pEqpAct.funWordWriteString(28, dstrCaseID.PadRight(28, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strGlassData += m_pEqpAct.funWordWriteString(8, "".PadRight(8, ' '), EnuEQP.PLCRWType.ASCII_Data);//CASE 로딩시 정보없음
                strGlassData += m_pEqpAct.funWordWriteString(10, pInfo.All.CurrentHOSTPPID.PadRight(10, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strGlassData += m_pEqpAct.funWordWriteString(1, "0", EnuEQP.PLCRWType.Int_Data);
                strGlassData += m_pEqpAct.funWordWriteString(1, "0", EnuEQP.PLCRWType.Int_Data);
                strGlassData += m_pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);

                m_pEqpAct.funWordWrite(dstrMCCWordAddress, strGlassData, EnuEQP.PLCRWType.Hex_Data);


                //this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialLoadEvent, 1133, "IN", dintPortID, dstrCaseID, dintUnitID, dintSubUnitID);
                if (dintPortID == 1 || dintPortID == 5)
                {
                    //[2015/04/15] Host요청으로 인하여 위치변경(Modify by HS)
                    //this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPortEvent, 31, "IN", dintPortID, dstrCaseID, dintUnitID, dintSubUnitID);
                    this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPortEvent, 32, "IN", dintPortID, dstrCaseID, dintUnitID, dintSubUnitID);
                    this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPortEvent, 33, "IN", dintPortID, dstrCaseID, dintUnitID, dintSubUnitID);

                    //[2015/05/15]MCC Event Log(Add by HS)
                    //[2015/07/31]CEID_31 위치 변경(Add by HS)
                    //strMCCData = "EVENT;";
                    //strMCCData += "CEID_31" + ",";
                    //strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                    //strMCCData += ",";
                    //strMCCData += dstrCaseID + ",";
                    //strMCCData +=  ",";
                    //strMCCData += "=" + ";";
                    //pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
					
                    strMCCData = "EVENT;";
                    strMCCData += "CEID_32" + ",";
                    strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                    strMCCData += ",";
                    strMCCData += dstrCaseID + ",";
                    strMCCData +=  ",";
                    strMCCData +=  "=" +  ";";
                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
					
                    strMCCData = "EVENT;";
                    strMCCData += "CEID_33" + ",";
                    strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                    strMCCData += ",";
                    strMCCData += dstrCaseID + ",";
                    strMCCData += ",";
                    strMCCData +=  "=" + ";";
                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);

                }
                else if (dintPortID == 2)
                {
                    this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPortEvent, 33, "IN", dintPortID, dstrCaseID, dintUnitID, dintSubUnitID);

                    //[2015/05/15]MCC Event Log(Add by HS)
                    strMCCData = "EVENT;";
                    strMCCData += "CEID_33" + ",";
                    strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                    strMCCData += ",";
                    strMCCData += dstrCaseID + ",";
                    strMCCData += ",";
                    strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                }
                else
                {
                    this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11CSTMovingEvent, 45, "IN", dintPortID, dstrCaseID, dintUnitID, dintSubUnitID);

                    //[2015/05/15]MCC Event Log(Add by HS)
                    strMCCData = "EVENT;";
                    strMCCData += "CEID_45" + ",";
                    strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                    strMCCData += ",";
                    strMCCData += dstrCaseID + ",";
                    strMCCData += ",";
                    strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                }

                
            }
            catch (Exception ex)
            {
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
