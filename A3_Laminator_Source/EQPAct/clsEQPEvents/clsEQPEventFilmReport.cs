using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventFilmReport : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventFilmReport(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actFilmReport";
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
            string dstrFilmID = "";
            int dintFilmCount = 0;
            string[] dstrValue = null ;
            int dintUnitID = 0;
            StringBuilder dstrLog = new StringBuilder();

            try
            {
                dintUnitID = Convert.ToInt32(parameters[2]);

                if (dintUnitID == 1) //FI01
                {
                   //switch(Convert.ToInt32(parameters[3]))
                   //{
                   //    case 1: //Stock In
                   //        break;
                   //    case 2: //Stock Out
                   //        break;
                   //    case 3: //Dock In
                   //        dstrWordAddress = "W20C0";
                   //        m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);       //LOTID
                   //        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                    //SlotID
                   //        dstrValue = m_pEqpAct.funWordReadAction(true);    //Word영역을 Block으로 읽어서 결과값을 DataType에 맞게 배열로 넘겨준다.
                   //        break;
                   //    case 4: //Dock Out
                   //        dstrWordAddress = "W20C0";
                   //        m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);       //LOTID
                   //        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                    //SlotID
                   //        dstrValue = m_pEqpAct.funWordReadAction(true);    //Word영역을 Block으로 읽어서 결과값을 DataType에 맞게 배열로 넘겨준다.
                   //        break;
                   //}

                   dstrWordAddress = "W20C0";
                   m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);       //LOTID
                   m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                    //SlotID
                   dstrValue = m_pEqpAct.funWordReadAction(true);

                }
                else //FI02
                {
                    //switch (Convert.ToInt32(parameters[3]))
                    //{
                    //    case 1: //Stock In
                    //        break;
                    //    case 2: //Stock Out
                    //        break;
                    //    case 3: //Dock In
                    //        dstrWordAddress = "W20D0";
                    //        m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);       //LOTID
                    //        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                    //SlotID
                    //        dstrValue = m_pEqpAct.funWordReadAction(true);    //Word영역을 Block으로 읽어서 결과값을 DataType에 맞게 배열로 넘겨준다.
                    //        break;
                    //    case 4: //Dock Out
                    //        dstrWordAddress = "W20D0";
                    //        m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);       //LOTID
                    //        m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                    //SlotID
                    //        dstrValue = m_pEqpAct.funWordReadAction(true);    //Word영역을 Block으로 읽어서 결과값을 DataType에 맞게 배열로 넘겨준다.
                    //        break;
                    //}

                    dstrWordAddress = "W20D0";
                    m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);       //LOTID
                    m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                    //SlotID
                    dstrValue = m_pEqpAct.funWordReadAction(true);    //Word영역을 Block으로 읽어서 결과값을 DataType에 맞게 배열로 넘겨준다.

                }
                
                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);

                if (dstrValue.Length != 0)
                {

                    if (dstrValue[0].Length > 13)
                    {
                        dstrValue[0] = dstrValue[0].Substring(0, 13);
                    }
                    dstrFilmID = dstrValue[0];
                    dintFilmCount = Convert.ToInt32(dstrValue[1].Trim());

                    if (pInfo.LOTID(dstrFilmID) == null)
                    {
                        subCreateLOTInfo(dstrFilmID, 0);
                    }

                    if (dintUnitID == 1) //FI01
                    {
                        switch (Convert.ToInt32(parameters[3]))
                        {
                            case 1: //Stock In
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = false;
                                break;
                            case 2: //Stock Out
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = false;
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialStockEvent, 202, dintUnitID, dstrFilmID, dintFilmCount);
                                this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialLoadEvent, 1135, dintUnitID, dstrFilmID);

                                break;
                            case 3: //Dock In
                                this.pInfo.LOTID(dstrFilmID).FilmCount = dintFilmCount;
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmID = dstrFilmID;
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmCount = dintFilmCount;
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = true;

                                //PortID 항목 추가 20120404 어우수
                                this.pInfo.LOTID(dstrFilmID).PortID = this.pInfo.Unit(dintUnitID).SubUnit(0).ModuleID.Substring(14).Trim();

                                //장비에 GLS Out시 읽은 Data를 로그를 남긴다.
                                dstrLog.Append("====== CST " + "DOCK IN" + ", ModuleID: " + pInfo.Unit(dintUnitID).SubUnit(0).ModuleID + " ======,");
                                dstrLog.Append("FilmID:" + dstrFilmID);
                                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrLog.ToString());
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialStockEvent, 201, dintUnitID, dstrFilmID, dintFilmCount);
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialStockEvent, 203, dintUnitID, dstrFilmID, dintFilmCount);
                                //this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialLoadEvent, 1133, dintUnitID, dstrFilmID);
                                //this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPortEvent, 33, dintUnitID, dstrFilmID);

                                for (int dintLoop = 0; dintLoop < dintFilmCount; dintLoop++)
                                {
                                    pInfo.LOTID(dstrFilmID).Slot(dintLoop + 1).H_PANELID = dstrFilmID + (dintLoop + 1).ToString().PadRight(3, '0');
                                }

                                break;
                            case 4: //Dock Out
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = false;
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialStockEvent, 204, dintUnitID, dstrFilmID, dintFilmCount);

                                break;
                        }
                    }
                    else
                    {
                        switch (Convert.ToInt32(parameters[3]))
                        {
                            case 1: //Stock In
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = false;
                                
                                break;

                            case 2: //Stock Out
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = false;
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialStockEvent, 202, dintUnitID, dstrFilmID, dintFilmCount);
                                this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialLoadEvent, 1135, dintUnitID, dstrFilmID);
                                break;

                            case 3: //Dock In
                                this.pInfo.LOTID(dstrFilmID).FilmCount = dintFilmCount;
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmID = dstrFilmID;
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmCount = dintFilmCount;
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = true;

                                //PortID 항목 추가 20120404 어우수
                                this.pInfo.LOTID(dstrFilmID).PortID = this.pInfo.Unit(dintUnitID).SubUnit(0).ModuleID.Substring(14).Trim();

                                //장비에 GLS Out시 읽은 Data를 로그를 남긴다.
                                dstrLog.Append("====== CST " + "DOCK IN" + ", ModuleID: " + pInfo.Unit(dintUnitID).SubUnit(0).ModuleID + " ======,");
                                dstrLog.Append("FilmID:" + dstrFilmID);
                                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrLog.ToString());
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialStockEvent, 201, dintUnitID, dstrFilmID, dintFilmCount);
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialStockEvent, 203, dintUnitID, dstrFilmID, dintFilmCount);
                                //this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialLoadEvent, 1133, dintUnitID, dstrFilmID);

                                for (int dintLoop = 0; dintLoop < dintFilmCount; dintLoop++)
                                {
                                    pInfo.LOTID(dstrFilmID).Slot(dintLoop + 1).H_PANELID = dstrFilmID + (dintLoop + 1).ToString().PadRight(3, '0');
                                }
                                break;
                            case 4: //Dock Out
                                this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = false;
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialStockEvent, 204, dintUnitID, dstrFilmID, dintFilmCount);
                                
                                break;
                        }
                    }
                }

                //    if (Convert.ToInt32(parameters[3]) == 3)
                //    {
                //        this.pInfo.LOTID(dstrFilmID).FilmCount = dintFilmCount;
                //        this.pInfo.Unit(dintUnitID).SubUnit(0).FilmID = dstrFilmID;
                //        this.pInfo.Unit(dintUnitID).SubUnit(0).FilmCount = dintFilmCount;
                //        this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = true;

                //        //PortID 항목 추가 20120404 어우수
                //        this.pInfo.LOTID(dstrFilmID).PortID = this.pInfo.Unit(dintUnitID).SubUnit(0).ModuleID.Substring(14).Trim();

                //        //장비에 GLS Out시 읽은 Data를 로그를 남긴다.
                //        dstrLog.Append("====== CST " + "DOCK IN" + ", ModuleID: " + pInfo.Unit(dintUnitID).SubUnit(0).ModuleID + " ======,");
                //        dstrLog.Append("FilmID:" + dstrFilmID);
                //        this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrLog.ToString());

                //        this.pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialLoadEvent, 33, dintUnitID, dstrFilmID);

                //        for (int dintLoop = 0; dintLoop < dintFilmCount; dintLoop++)
                //        {
                //            pInfo.LOTID(dstrFilmID).Slot(dintLoop + 1).H_PANELID = dstrFilmID + (dintLoop + 1).ToString().PadRight(3, '0');
                //        }
                //    }
                //    else
                //    {
                //        this.pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = false;
                //    }
                //}

                
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
