using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventUnscrap : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventUnscrap(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actUnScrap";
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
            int dintLOTIndex = 0;
            int dintUnitID = 0;
            int dintSubUnitID = 0;
            int dintModuleNo = 0;

            try
            {
                dstrWordAddress = "W2030";
                //Unscrap되었는 LOTID, SlotID 정보를 읽어온다.
                m_pEqpAct.subWordReadSave(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);       //LOTID
                m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                    //SlotID
                m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);                      //Scrap된 UnitID

                dstrValue = m_pEqpAct.funWordReadAction(true);    //Word영역을 Block으로 읽어서 결과값을 DataType에 맞게 배열로 넘겨준다.

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);

                dstrLOTID = dstrValue[0];
                dintSlotID = Convert.ToInt32(dstrValue[1].Trim());
                dintModuleNo = Convert.ToInt32(dstrValue[2].Trim());
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

                if (dintModuleNo == 13 || dintModuleNo == 14)
                {
                    if (pInfo.GLSID(dstrLOTID) != null)
                    {
                        //현재 Unit에서 Unscrap되었기 때문에 GLS정보를 추가한다.
                        if (pInfo.Unit(0).SubUnit(0).AddCurrGLS(dstrLOTID) == true)
                        {
                            pInfo.Unit(0).SubUnit(0).CurrGLS(dstrLOTID).H_PANELID = dstrGLSID;
                            pInfo.Unit(0).SubUnit(0).CurrGLS(dstrLOTID).SlotID = dintSlotID;
                        }

                        if (pInfo.Unit(dintUnitID).SubUnit(0).AddCurrGLS(dstrLOTID) == true)
                        {
                            pInfo.Unit(dintUnitID).SubUnit(0).CurrGLS(dstrLOTID).H_PANELID = dstrGLSID;
                            pInfo.Unit(dintUnitID).SubUnit(0).CurrGLS(dstrLOTID).SlotID = dintSlotID;
                        }

                        if(pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).AddCurrGLS(dstrLOTID) )
                        {
                             pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).CurrGLS(dstrLOTID).H_PANELID = dstrGLSID;
                            pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).CurrGLS(dstrLOTID).SlotID = dintSlotID;
                        }

                        pInfo.Unit(0).SubUnit(0).GLSExist = true;
                        pInfo.Unit(dintUnitID).SubUnit(0).GLSExist = true;
                        pInfo.Unit(dintUnitID).SubUnit(dintSubUnitID).GLSExist = true;
                        pInfo.GLSID(dstrLOTID).Scrap = false;
                        //S6F11(CEID=15, Unscrap 보고)
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedPanelProcessEvent, 19, dintUnitID, dintSubUnitID, dstrLOTID, dintSlotID);
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
                        pInfo.LOTID(dstrLOTID).Slot(dintSlotID).Scrap = false;    //해당 Slot이 Scrap되었음을 저장
                        //S6F11(CEID=14, Scrap 보고)
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1019, dintModuleNo, dstrLOTID, dintSlotID, dintUnitID, dintSubUnitID);
                        //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11MaterialProcEvent, 1018, dintModuleNo, dstrLOTID, dintSlotID, dintUnitID, dintSubUnitID);
                    }
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
