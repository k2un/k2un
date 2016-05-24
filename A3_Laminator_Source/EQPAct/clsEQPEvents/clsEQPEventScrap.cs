using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventScrap : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventScrap(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actScrap";
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
            string dstrLOTID = "";
            int dintSlotID = 0;
            string dstrGLSID = "";
            string[] dstrValue;
            string dstrLogMsg = "";
            int dintUnitID = 0;
            int dintSubUnitID = 0;
            int dintModuleNo = 0;
            string strMCCData = "";
            try
            {
                dstrWordAddress = "W2020";
                //Scrap되었는 LOTID, SlotID 정보를 읽어온다.
                m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);       //LOTID
                m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                    //SlotID
                m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                      //UnitID

                dstrValue = m_pEqpAct.funWordReadAction(true);    //Word영역을 Block으로 읽어서 결과값을 DataType에 맞게 배열로 넘겨준다.

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);

                dstrLOTID = dstrValue[0];
                dintSlotID = Convert.ToInt32(dstrValue[1]);
                dintModuleNo = Convert.ToInt32(dstrValue[2]);

                switch (dintModuleNo)
                {
                    case 7:
                        dintUnitID = 3;
                        dintSubUnitID = 1;
                        break;
                    case 8:
                        dintUnitID = 3;
                        dintSubUnitID = 2;
                        break;
                    case 9:
                        dintUnitID = 3;
                        dintSubUnitID = 3;
                        break;
                    case 10:
                        dintUnitID = 3;
                        dintSubUnitID = 4;
                        break;
                    case 11:
                        dintUnitID = 3;
                        dintSubUnitID = 5;
                        break;
                    case 12:
                        dintUnitID = 3;
                        dintSubUnitID = 6;
                        break;
                    case 13:
                        dintUnitID = 3;
                        dintSubUnitID = 7;
                        break;
                    case 14:
                        dintUnitID = 3;
                        dintSubUnitID = 8;
                        break;
                }
                //[2015/05/08] GL01 조건 추가(Add by HS)
                if (dintModuleNo == 13 || dintModuleNo == 14 || dintModuleNo == 15)
                {
                      pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).RemoveCurrGLS(dstrLOTID);
                    pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).GLSExist = false;

                    //pInfo.Unit(0).SubUnit(0).RemoveCurrGLS(dstrLOTID);
                    //pInfo.Unit(0).SubUnit(0).GLSExist = false;


                    pInfo.Unit(dintUnitID).SubUnit(0).RemoveCurrGLS(dstrLOTID);

                    if(pInfo.Unit(3).SubUnit(7).GLSExist == false && pInfo.Unit(3).SubUnit(8).GLSExist == false)
                    {
                        pInfo.Unit(dintUnitID).SubUnit(0).GLSExist = false;
                    }

                    if (pInfo.GLSID(dstrLOTID) != null)
                    {
                        pInfo.GLSID(dstrLOTID).Scrap = true;
                        //S6F11(CEID=14, Scrap 보고)
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 18, dintUnitID, dintSubUnitID, dstrLOTID, dintSlotID);

                        //[2015/05/15]MCC Event Log(Add by HS)
                        InfoAct.clsGLS CurrentGLS = pInfo.GLSID(dstrLOTID);
                        strMCCData = "EVENT;";
                        strMCCData += "CEID_18" + ",";
                        strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                        strMCCData += CurrentGLS.STEPID + ",";
                        strMCCData += CurrentGLS.H_PANELID + ",";
                        strMCCData += CurrentGLS.LOTID + ",";
                        strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                        pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                    }
                }
                else
                {
                    //Scrap 로그 출력
                    if (pInfo.LOTID(dstrLOTID) == null)
                    {
                        dstrLogMsg = "LOTID:" + dstrLOTID + "(NULL)" + "\r\n" + "SlotID:" + dintSlotID.ToString() + "\r\n" + "GLSID:" + dstrGLSID + "\r\n" + "ModuleID:" + pInfo.Unit(dintUnitID).SubUnit(0).ModuleID;
                    }
                    else
                    {
                        pInfo.LOTID(dstrLOTID).Slot(dintSlotID).Scrap = true;    //해당 Slot이 Scrap되었음을 저장
                    }
                    pInfo.Unit(dintUnitID).SubUnit(0).FilmExist = false;
                    
                    //S6F11(CEID=14, Scrap 보고)
                    //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1018, dintModuleNo, dstrLOTID, dintSlotID, dintUnitID, dintSubUnitID);

                    //[2015/05/08]Scrap처리를 호스트에서 하지못하여 CEID 1015로 판단하기에 추가(Add by HS)
                    //[2015/06/30] Juge 구분 추가(Add by HS)
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1015, dintModuleNo, dstrLOTID, dintSlotID, dintUnitID, dintSubUnitID, "SCRAP");

                    //[2015/05/15]MCC Event Log(Add by HS)
                    InfoAct.clsSlot CurrentSlot = pInfo.LOTID(dstrGLSID).Slot(dintSlotID);

                    strMCCData = "EVENT;";
                    strMCCData += "CEID_1015" + ",";
                    strMCCData += pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).ModuleID + ",";
                    strMCCData += CurrentSlot.STEPID + ",";
                    strMCCData += CurrentSlot.H_PANELID + ",";
                    strMCCData += CurrentSlot.LOTID + ",";
                    strMCCData += pInfo.All.CurrentHOSTPPID + "=" + pInfo.All.CurrentEQPPPID + ";";

                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MCCDataSend, strMCCData);
                }
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString() + ", dstrLOTID:" + dstrLOTID + ", dintSlotID: " + dintSlotID.ToString());
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
