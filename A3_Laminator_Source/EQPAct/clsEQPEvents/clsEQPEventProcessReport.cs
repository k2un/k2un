using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Threading;

namespace EQPAct
{
    public class clsEQPEventProcessReport : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventProcessReport(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actProcessReport";
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
            try
            {
                int intUnitID = Convert.ToInt32(parameters[2]);
                int intSubUnitID = 0;
                int.TryParse(parameters[3].ToString(), out intSubUnitID);
                int dintACTVal = 0;
                int.TryParse(parameters[1], out dintACTVal);
                int intBitVal = 0;
                int.TryParse(parameters[4].ToString(), out intBitVal);
                int intPortID = 0;
                int intSlotNo = 0;
                string strAddress = "";
                string strLOTID = "";
                string strGLSID = "";
                string strJudgeID = "";
                int Arr_dintSlotNo = 0;
                int Dep_dintSlotNo = 0;
                bool Arrive_CheckFlag = false;
                bool Dep_CheckFlag = false;
                string strLogMSG = "";
                DateTime dtNow = DateTime.Now;
                string strPortID = "";
                string strLotStartFlag = "";
                string strLotEndFlag = "";
                
                DateTime dtDVTime = new DateTime();
                string strDVValue = "";

                switch (intUnitID)
                {
                    case 1:
                        switch (dintACTVal)
                        {
                            case 1: //Port In - Port에 Glass가 들어옴.

                                 strPortID = m_pEqpAct.funWordRead("W3040", 2, EnuEQP.PLCRWType.ASCII_Data);
                                intPortID = Convert.ToInt32(strPortID.Substring(1));
                                intSlotNo = Convert.ToInt32(m_pEqpAct.funWordRead("W3042", 1, EnuEQP.PLCRWType.Int_Data));

                                strGLSID = m_pEqpAct.funWordRead("W3043", 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                                strLOTID = pInfo.Port(intPortID).LOTID.Trim();


                                strLotStartFlag = m_pEqpAct.funWordRead("W304D", 1, EnuEQP.PLCRWType.Int_Data);
                                strLotEndFlag = m_pEqpAct.funWordRead("W304E", 1, EnuEQP.PLCRWType.Int_Data);

                                pInfo.LOTID(strLOTID).GLSID(strGLSID).RunState = "E";
                                pInfo.LOTID(strLOTID).EndGLSCount++;

                                //Component in by Indexer 보고
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 322, intUnitID, pInfo.LOTID(strLOTID).InPortID,  strLOTID, strGLSID);
                                pInfo.Port(intPortID).Slot(intSlotNo).SLOTINFO = "P";

                                bool dbolWaitGLSCheck = false;
                                for (int dintLoop = 0; dintLoop < pInfo.Port(intPortID).SlotCount; dintLoop++)
                                {
                                    if (pInfo.Port(intPortID).Slot(dintLoop + 1).SLOTINFO == "W" || pInfo.Port(intPortID).Slot(dintLoop + 1).SLOTINFO == "R")
                                    {
                                        dbolWaitGLSCheck = true;
                                        break;
                                    }
                                }

                                if (dbolWaitGLSCheck == false)
                                {
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F3_DVReport, 501, 0, 0, strLOTID, strGLSID);
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F103, strLOTID);
                                    if (pInfo.Port(intPortID).AbortFlag == true)
                                    {
                                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ProcessReport, 303, intUnitID, pInfo.LOTID(strLOTID).InPortID, strLOTID, strGLSID);
                                    }
                                    else if (pInfo.Port(intPortID).OffineChangeFlag == true)
                                    {
                                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ProcessReport, 303, intUnitID, pInfo.LOTID(strLOTID).InPortID, strLOTID, strGLSID);
                                    }
                                    else
                                    {
                                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ProcessReport, 302, intUnitID, pInfo.LOTID(strLOTID).InPortID, strLOTID, strGLSID);
                                    }

                                    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.UnloadReqCMD, intPortID);

                                    if (pInfo.Port(intPortID).LOTST == "3")
                                    {
                                        pInfo.Port(intPortID).LOTST = "4";
                                    }
                                }
                                
                                //bool dbolGLSCheckFlag = false;

                                //if (pInfo.Port(intPortID).AbortFlag == true)
                                //{
                                //    foreach (string str in pInfo.LOTID(strLOTID).GLS())
                                //    {
                                //        InfoAct.clsGLS CurrentGLS = pInfo.LOTID(strLOTID).GLSID(str);
                                //        if (CurrentGLS.RunState == "S")
                                //        {
                                //            dbolGLSCheckFlag = true;
                                //            break;
                                //        }
                                //    }

                                //    if (dbolGLSCheckFlag == false)
                                //    {
                                //        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F103, strLOTID);

                                //        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F3_DVReport, 501, 0, 0, strLOTID, strGLSID);

                                //        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ProcessReport, 303, intUnitID, pInfo.LOTID(strLOTID).InPortID, strLOTID, strGLSID);

                                //    }
                                //}
                                //else if (pInfo.LOTID(strLOTID).EndGLSCount == pInfo.LOTID(strLOTID).StartGLSCount + pInfo.LOTID(strLOTID).ScrapCount) //Lot의 마지막 Glass일때
                                //{
                                //    pInfo.LOTID(strLOTID).EndFlag = true;

                                //    //Lot Summary DV Data
                                //    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F3_DVReport, 501, 0, 0, strLOTID, strGLSID); 

                                //    //Lot Information Upload
                                //    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F103, strLOTID); 
                                    
                                //    //Process END 보고
                                //    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ProcessReport, 302, intUnitID, pInfo.LOTID(strLOTID).InPortID, strLOTID, strGLSID);
                                //}

                                //for (int dintLoop = 0; dintLoop < pInfo.PortCount(); dintLoop++)
                                //{
                                //    for (int dintLoop2 = 0; dintLoop2 < pInfo.Port(dintLoop + 1).SlotCount; dintLoop2++)
                                //    {
                                //        if (pInfo.Port(dintLoop + 1).Slot(dintLoop2 + 1).GLSID.Trim() == strGLSID.Trim())
                                //        {
                                //            Dep_CheckFlag = true;
                                //            Dep_dintSlotNo = dintLoop2 + 1;
                                //            break;
                                //        }
                                //    }

                                //    if (Dep_CheckFlag)
                                //    {
                                //        break;
                                //    }
                                //}

                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "Out", "Index -> P0" + intPortID);
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);

                                break;

                            case 2: //Port Out - Port에서 Glass를 꺼냄.
                                strPortID = m_pEqpAct.funWordRead("W3050", 2, EnuEQP.PLCRWType.ASCII_Data);
                                intPortID = Convert.ToInt32(strPortID.Substring(1));
                                intSlotNo = Convert.ToInt32(m_pEqpAct.funWordRead("W3052", 1, EnuEQP.PLCRWType.Int_Data));

                                strGLSID = m_pEqpAct.funWordRead("W3053", 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                                strLOTID = pInfo.Port(intPortID).LOTID.Trim();

                                pInfo.Port(intPortID).Slot(intSlotNo).SLOTINFO = "R";

                                strLotStartFlag = m_pEqpAct.funWordRead("W305D", 1, EnuEQP.PLCRWType.Int_Data);
                                strLotEndFlag = m_pEqpAct.funWordRead("W305E", 1, EnuEQP.PLCRWType.Int_Data);

                                if (strLotStartFlag == "1" || pInfo.LOTID(strLOTID).StartFlag)
                                {
                                    pInfo.LOTID(strLOTID).StartFlag = false;
                                    //Process Start 보고
                                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ProcessReport, 301, intUnitID, intPortID, strLOTID, strGLSID); //Process Start
                                }

                                //Component Out by Indexer 보고
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 321, intPortID, 0, strLOTID, strGLSID);

                                pInfo.LOTID(strLOTID).GLSID(strGLSID).RunState = "S";
                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "In", "P0" + intPortID + " -> Index");
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                             
                                break;

                            case 3://index in - sky C/V에서 Glass를 받음.
                                strPortID = m_pEqpAct.funWordRead("W3060", 2, EnuEQP.PLCRWType.ASCII_Data);
                                intPortID = Convert.ToInt32(strPortID.Substring(1));
                                intSlotNo = Convert.ToInt32(m_pEqpAct.funWordRead("W3062", 1, EnuEQP.PLCRWType.Int_Data));

                                strLOTID = pInfo.Port(intPortID).LOTID.Trim();
                                strGLSID = m_pEqpAct.funWordRead("W3063", 10, EnuEQP.PLCRWType.ASCII_Data).Trim();

                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "In", "Sky C/V -> Index");
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                                break;

                            case 4://index out - Index가 Cleaner에게 Glass를 줌.
                                 strPortID = m_pEqpAct.funWordRead("W3070", 2, EnuEQP.PLCRWType.ASCII_Data);
                                intPortID = Convert.ToInt32(strPortID.Substring(1));
                                intSlotNo = Convert.ToInt32(m_pEqpAct.funWordRead("W3072", 1, EnuEQP.PLCRWType.Int_Data));

                                strLOTID = pInfo.Port(intPortID).LOTID.Trim();
                                strGLSID = m_pEqpAct.funWordRead("W3073", 10, EnuEQP.PLCRWType.ASCII_Data).Trim();

                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "Out", "Index -> Cleaner");
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                                break;

                            default:    //다른 이벤트는 보고하지 않음.
                                break;
                        }
                        break;

                    case 2: // Cleaner
                        string strCleanerWordAddress = "W4400";
                        strCleanerWordAddress = FunTypeConversion.funAddressAdd(strCleanerWordAddress, 32 * (intSubUnitID - 1));
                        strLOTID = m_pEqpAct.funWordRead(strCleanerWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                        strCleanerWordAddress = FunTypeConversion.funAddressAdd(strCleanerWordAddress, 10);
                        strGLSID = m_pEqpAct.funWordRead(strCleanerWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                        strCleanerWordAddress = FunTypeConversion.funAddressAdd(strCleanerWordAddress, 10);
                        strJudgeID = m_pEqpAct.funWordRead(strCleanerWordAddress, 1, EnuEQP.PLCRWType.Int_Data);

                        if (intBitVal == 1)
                        {
                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = strGLSID;
                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = strLOTID;
                            if (pInfo.Unit(intUnitID).SubUnit(0).CurrGLS(strGLSID) == null)
                            {
                                pInfo.Unit(intUnitID).SubUnit(0).AddCurrGLS(strGLSID);
                                pInfo.Unit(intUnitID).SubUnit(0).CurrGLS(strGLSID).LOTID = strLOTID;

                            }

                            if (pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(strGLSID) == null)
                            {
                                pInfo.Unit(intUnitID).SubUnit(intSubUnitID).AddCurrGLS(strGLSID);
                                pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(strGLSID).LOTID = strLOTID;
                            }


                            if (intSubUnitID == 1)
                            {
                                //Component In By Cleaner
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 324, intUnitID, 0, strLOTID, strGLSID);

                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "In", "");
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                            }

                        }
                        else
                        {
                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = "";
                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = "";
                            
                            if (intSubUnitID == 10)
                            {
                                string address = "W4200";
                                string strCleanerDVData = "";
                                try
                                {
                                    dtDVTime = DateTime.Now;
                                    strDVValue = dtDVTime.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                                    strDVValue += strGLSID + ",";
                                    strDVValue += "U01,";
                                    //DVReport
                                    for (int dintLoop = 0; dintLoop < pInfo.Unit(2).SubUnit(0).GLSAPDCount; dintLoop++)
                                    {
                                        if (dintLoop != 0) address = CommonAct.FunTypeConversion.funAddressAdd(address, 2);

                                        //if (pInfo.Unit(2).SubUnit(0).GLSAPD(dintLoop + 1).Length == 1 || pInfo.Unit(2).SubUnit(0).GLSAPD(dintLoop + 1).Length == 2)
                                        //{
                                            strCleanerDVData = m_pEqpAct.funWordRead(address, 2, EnuEQP.PLCRWType.Int_Data);
                                            strCleanerDVData = CommonAct.FunStringH.funPoint(strCleanerDVData, pInfo.Unit(2).SubUnit(0).GLSAPD(dintLoop + 1).Format);
                                            pInfo.Unit(2).SubUnit(0).GLSAPD(dintLoop + 1).Value = Convert.ToSingle(strCleanerDVData);
                                            pInfo.LOTID(strLOTID).LOTAPD(dintLoop + 1).Value += pInfo.Unit(2).SubUnit(0).GLSAPD(dintLoop + 1).Value;
                                            strDVValue += pInfo.Unit(2).SubUnit(0).GLSAPD(dintLoop + 1).Value + ",";
                                        //}
                                    }
                                    strDVValue = strDVValue.Substring(0, strDVValue.Length - 1);
                                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.Clenaer_DV, strDVValue);
                                }
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        for (int dintLoop = 0; dintLoop < pInfo.Unit(2).SubUnit(0).GLSAPDCount; dintLoop++)
                                        {
                                            pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Cleaner DV Data List {0} : {1}", dintLoop + 1, m_pEqpAct.funWordRead("W4200", pInfo.Unit(2).SubUnit(0).GLSAPD(dintLoop + 1).Length, EnuEQP.PLCRWType.Int_Data)));
                                        }
                                    }
                                    catch (Exception ex2)
                                    {
                                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Cleaner DVData Read Error!!" );
                                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex2.ToString());
                                    }
                                    
                                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                                }
                               
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F3_DVReport, 500, intUnitID, 0, strLOTID, strGLSID);

                                //Component Out By Clenaer
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 323, intUnitID, 0, strLOTID, strGLSID);
                                
                                pInfo.Unit(intUnitID).SubUnit(0).RemoveCurrGLS(strGLSID);

                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "Out", "");
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                            }

                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).RemoveCurrGLS(strGLSID);
                        }
                        break;
                    case 3: //Sky C/V
                        intBitVal = Convert.ToInt32(parameters[4]);

                        string strOvenWordAddress = "W5400";
                        strOvenWordAddress = FunTypeConversion.funAddressAdd(strOvenWordAddress, 32 * (intSubUnitID - 1));
                        strLOTID = m_pEqpAct.funWordRead(strOvenWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                        strOvenWordAddress = FunTypeConversion.funAddressAdd(strOvenWordAddress, 10);
                        strGLSID = m_pEqpAct.funWordRead(strOvenWordAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                        strOvenWordAddress = FunTypeConversion.funAddressAdd(strOvenWordAddress, 10);
                        strJudgeID = m_pEqpAct.funWordRead(strOvenWordAddress, 1, EnuEQP.PLCRWType.Int_Data);

                        if (intBitVal == 1)
                        {
                            //strLOTID = m_pEqpAct.funWordRead("W5400", 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                            //strGLSID = m_pEqpAct.funWordRead("W540A", 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                            //strJudgeID = m_pEqpAct.funWordRead("W5414", 1, EnuEQP.PLCRWType.Int_Data);
                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = strGLSID;
                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = strLOTID;

                            if (pInfo.Unit(intUnitID).SubUnit(0).CurrGLS(strGLSID) == null)
                            {
                                pInfo.Unit(intUnitID).SubUnit(0).AddCurrGLS(strGLSID);
                                pInfo.Unit(intUnitID).SubUnit(0).CurrGLS(strGLSID).LOTID = strLOTID;

                            }

                            if (pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(strGLSID) == null)
                            {
                                pInfo.Unit(intUnitID).SubUnit(intSubUnitID).AddCurrGLS(strGLSID);
                                pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(strGLSID).LOTID = strLOTID;
                            }

                            if (intSubUnitID == 1)
                            {
                                //pInfo.Unit(intUnitID).SubUnit(intSubUnitID).AddCurrGLS(strGLSID);
                                //pInfo.Unit(intUnitID).SubUnit(0).AddCurrGLS(strGLSID);
                                //Component In By Cleaner
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 324, intUnitID, 0, strLOTID, strGLSID);
                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "In", "");
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                            }

                          
                        }
                        else
                        {
                            //strLOTID = m_pEqpAct.funWordRead("W5460", 10, EnuEQP.PLCRWType.ASCII_Data);
                            //strGLSID = m_pEqpAct.funWordRead("W546A", 10, EnuEQP.PLCRWType.ASCII_Data);
                            //strJudgeID = m_pEqpAct.funWordRead("W5474", 1, EnuEQP.PLCRWType.Int_Data);
                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = "";
                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = "";

                            if (intSubUnitID == 4)
                            {
                                //Component Out By Clenaer
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 323, intUnitID, 0, strLOTID, strGLSID);

                                pInfo.Unit(intUnitID).SubUnit(0).RemoveCurrGLS(strGLSID);

                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "Out", "");
                                pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                            }

                            pInfo.Unit(intUnitID).SubUnit(intSubUnitID).RemoveCurrGLS(strGLSID);

                        }
                        break;
                    case 4://Robot
                        strAddress = "W6200";
                        strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 32 * (dintACTVal - 1));

                        strLOTID = m_pEqpAct.funWordRead(strAddress, 10, EnuEQP.PLCRWType.ASCII_Data);
                        strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                        strGLSID = m_pEqpAct.funWordRead(strAddress, 10, EnuEQP.PLCRWType.ASCII_Data);


                        InfoAct.clsGLS CurrentGLS = null;

                        if (pInfo.Unit(4).SubUnit(0).CurrGLS(strGLSID) == null)
                        {
                            pInfo.Unit(4).SubUnit(0).AddCurrGLS(strGLSID);
                        }

                        switch(dintACTVal)
                        {
                            case 1:
                                CurrentGLS = pInfo.Unit(4).SubUnit(0).CurrGLS(strGLSID);
                                CurrentGLS.LOTID = strLOTID;
                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, "Oven Robot", "In", "Cleaner -> Oven Robot");
                                break;

                            case 2:
                                pInfo.Unit(4).SubUnit(0).RemoveCurrGLS(strGLSID);
                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, "Oven Robot", "Out", "Oven Robot -> Sky C/V");
                                break;

                            case 3:
                                pInfo.Unit(4).SubUnit(0).RemoveCurrGLS(strGLSID);
                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, "Oven Robot", "Out", "Oven Robot -> Oven1");
                                break;

                            case 4:
                                CurrentGLS = pInfo.Unit(4).SubUnit(0).CurrGLS(strGLSID);
                                CurrentGLS.LOTID = strLOTID;
                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, "Oven Robot", "In", "Oven1 -> Oven Robot");
                                break;

                            case 5:
                                pInfo.Unit(4).SubUnit(0).RemoveCurrGLS(strGLSID);
                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, "Oven Robot", "Out", "Oven Robot -> Oven2");
                                break;

                            case 6:
                                CurrentGLS = pInfo.Unit(4).SubUnit(0).CurrGLS(strGLSID);
                                CurrentGLS.LOTID = strLOTID;
                                strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, "Oven Robot", "In", "Oven2 -> Oven Robot");
                                break;
                        }

                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);


                        break;

                    case 5://OVEN#01
                         intBitVal = Convert.ToInt32(parameters[4]);
                        
                         strAddress = CommonAct.FunTypeConversion.funAddressAdd("W7500", (intSubUnitID - 1) * 32);
                         strLOTID = m_pEqpAct.funWordRead(strAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                        
                         strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                         strGLSID = m_pEqpAct.funWordRead(strAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                         
                         strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                         strJudgeID = m_pEqpAct.funWordRead(strAddress, 1, EnuEQP.PLCRWType.Int_Data);

                         if (intBitVal == 1)
                         {
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = strGLSID;
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = strLOTID;

                             if (pInfo.Unit(intUnitID).SubUnit(0).CurrGLS(strGLSID) == null)
                             {
                                 pInfo.Unit(intUnitID).SubUnit(0).AddCurrGLS(strGLSID);
                                 pInfo.Unit(intUnitID).SubUnit(0).CurrGLS(strGLSID).LOTID = strLOTID;
                             }

                             if (pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(strGLSID) == null)
                             {
                                 pInfo.Unit(intUnitID).SubUnit(intSubUnitID).AddCurrGLS(strGLSID);
                                 pInfo.Unit(intUnitID).SubUnit(intSubUnitID).CurrGLS(strGLSID).LOTID = strLOTID;
                             }

                             //Component In By Cleaner
                             pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 324, intUnitID, 0, strLOTID, strGLSID);

                             strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "In", "");
                             pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                         }
                         else
                         {
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = "";
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = "";
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).RemoveCurrGLS(strGLSID);
                             pInfo.Unit(intUnitID).SubUnit(0).RemoveCurrGLS(strGLSID);

                             try
                             {
                                 string address = "W7300";
                                 string strOvenDVData = "";


                                 dtDVTime = DateTime.Now;
                                 strDVValue = dtDVTime.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                                 strDVValue += strGLSID + ",";
                                 strDVValue += "U02,";

                                 int dintOven01UnitID = 5;

                                 //DVReport
                                 for (int dintLoop = 0; dintLoop < pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPDCount; dintLoop++)
                                 {
                                     if (dintLoop != 0) address = CommonAct.FunTypeConversion.funAddressAdd(address, pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop).Length);
                                     if (pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Length == 1)
                                     {
                                         strOvenDVData = m_pEqpAct.funWordRead(address, pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Length, EnuEQP.PLCRWType.Int_Data);
                                         strOvenDVData = CommonAct.FunStringH.funPoint(strOvenDVData, pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Format);
                                         pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Value = Convert.ToSingle(strOvenDVData);
                                         strDVValue += strOvenDVData + ",";
                                         pInfo.LOTID(strLOTID.Trim()).LOTAPD(pInfo.Unit(2).SubUnit(0).GLSAPDCount + dintLoop).Value += pInfo.Unit(intUnitID).SubUnit(0).GLSAPD(dintLoop + 1).Value;

                                     }
                                     else if (pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Length == 6)  
                                     {
                                         string Oven01_DateTime = "";
                                         Oven01_DateTime += m_pEqpAct.funWordRead(address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2,'0');
                                         address = CommonAct.FunTypeConversion.funAddressAdd(address, 1);
                                         Oven01_DateTime += m_pEqpAct.funWordRead(address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         address = CommonAct.FunTypeConversion.funAddressAdd(address, 1);
                                         Oven01_DateTime += m_pEqpAct.funWordRead(address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         address = CommonAct.FunTypeConversion.funAddressAdd(address, 1);
                                         Oven01_DateTime += m_pEqpAct.funWordRead(address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         address = CommonAct.FunTypeConversion.funAddressAdd(address, 1);
                                         Oven01_DateTime += m_pEqpAct.funWordRead(address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         address = CommonAct.FunTypeConversion.funAddressAdd(address, 1);
                                         Oven01_DateTime += m_pEqpAct.funWordRead(address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         address = CommonAct.FunTypeConversion.funAddressAdd(address, 1);
                                         //Oven01_DateTime += m_pEqpAct.funWordRead(address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         //address = CommonAct.FunTypeConversion.funAddressAdd(address, 1);


                                         if (pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Name == "Oven01_LOADING_TIME")
                                         {
                                             pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop + 1).LoadingTIME = Oven01_DateTime;
                                             strDVValue += Oven01_DateTime + ",";
                                         }
                                         else
                                         {
                                             pInfo.Unit(dintOven01UnitID).SubUnit(0).GLSAPD(dintLoop + 1).UnloadingTIME = Oven01_DateTime;
                                             strDVValue += Oven01_DateTime + ",";
                                         }
                                       
                                         address = CommonAct.FunTypeConversion.funAddressAdd(address, -6);
                                     }
                                 }
                                 strDVValue = strDVValue.Substring(0, strDVValue.Length - 1);

                                 pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F3_DVReport, 500, intUnitID, 0, strLOTID, strGLSID);
                                 pInfo.subLog_Set(InfoAct.clsInfo.LogType.Oven_DV, strDVValue);
                             }
                             catch (Exception ex)
                             {
                                 try
                                 {
                                     for (int dintLoop = 0; dintLoop < pInfo.Unit(intUnitID).SubUnit(0).GLSAPDCount; dintLoop++)
                                     {
                                         pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Oven01 DVData List {0} : {1}", dintLoop + 1, m_pEqpAct.funWordRead("W7300", pInfo.Unit(intUnitID).SubUnit(0).GLSAPD(dintLoop + 1).Length, EnuEQP.PLCRWType.Int_Data)));
                                     }
                                 }
                                 catch (Exception ex2)
                                 {
                                     pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Oven01 DVData Read Error!!");
                                     pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex2.ToString());
                                 }

                                 pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                             }

                            
                             //Component In By Cleaner
                             pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 323, intUnitID, 0, strLOTID, strGLSID);

                             strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "Out", "");
                             pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                         }
                        break;

                    case 6://OVEN#02
                         intBitVal = Convert.ToInt32(parameters[4]);
                        
                         strAddress = CommonAct.FunTypeConversion.funAddressAdd("W8500", (intSubUnitID - 1) * 32);
                         strLOTID = m_pEqpAct.funWordRead(strAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                        
                         strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                         strGLSID = m_pEqpAct.funWordRead(strAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                         
                         strAddress = CommonAct.FunTypeConversion.funAddressAdd(strAddress, 10);
                         strJudgeID = m_pEqpAct.funWordRead(strAddress, 1, EnuEQP.PLCRWType.Int_Data);

                         if (intBitVal == 1)
                         {
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = strGLSID;
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = strLOTID;
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).AddCurrGLS(strGLSID);
                             pInfo.Unit(intUnitID).SubUnit(0).AddCurrGLS(strGLSID);

                             //Component In By Cleaner
                             pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 324, intUnitID, 0, strLOTID, strGLSID);

                             strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "In", "");
                             pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                         }
                         else
                         {
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = "";
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = "";
                             pInfo.Unit(intUnitID).SubUnit(intSubUnitID).RemoveCurrGLS(strGLSID);
                             pInfo.Unit(intUnitID).SubUnit(0).RemoveCurrGLS(strGLSID);
                            
                             string Oven02_address = "W8300";
                             string strOven2DVData = "";
                             dtDVTime = DateTime.Now;
                             strDVValue = dtDVTime.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                             strDVValue += strGLSID + ",";
                             strDVValue += "U03,";

                             try
                             {
                                 int dintOven02UnitID = 5;

                                 //DVReport
                                 for (int dintLoop = 0; dintLoop < pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPDCount; dintLoop++)
                                 {
                                     if (dintLoop != 0) Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop).Length);
                                     if (pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Length == 1)
                                     {
                                         strOven2DVData = m_pEqpAct.funWordRead(Oven02_address, pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Length, EnuEQP.PLCRWType.Int_Data);
                                         strOven2DVData = CommonAct.FunStringH.funPoint(strOven2DVData, pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Format);
                                         pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Value = Convert.ToSingle(strOven2DVData);
                                         strDVValue += strOven2DVData + ",";

                                         //pInfo.LOTID(strLOTID.Trim()).LOTAPD(pInfo.Unit(2).SubUnit(0).GLSAPDCount + pInfo.Unit(5).SubUnit(0).GLSAPDCount + dintLoop).Value += pInfo.Unit(intUnitID).SubUnit(0).GLSAPD(dintLoop + 1).Value;
                                         pInfo.LOTID(strLOTID.Trim()).LOTAPD(pInfo.Unit(2).SubUnit(0).GLSAPDCount + dintLoop).Value += pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Value;
                                     }
                                     else if (pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Length == 6)
                                     {
                                         string Oven02_DateTime = "";
                                         Oven02_DateTime += m_pEqpAct.funWordRead(Oven02_address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, 1);
                                         Oven02_DateTime += m_pEqpAct.funWordRead(Oven02_address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, 1);
                                         Oven02_DateTime += m_pEqpAct.funWordRead(Oven02_address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, 1);
                                         Oven02_DateTime += m_pEqpAct.funWordRead(Oven02_address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, 1);
                                         Oven02_DateTime += m_pEqpAct.funWordRead(Oven02_address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, 1);
                                         Oven02_DateTime += m_pEqpAct.funWordRead(Oven02_address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, 1);
                                         //Oven02_DateTime += m_pEqpAct.funWordRead(Oven02_address, 1, EnuEQP.PLCRWType.Int_Data).PadLeft(2, '0');
                                         //Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, 1);


                                         if (pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).Name == "Oven02_LOADING_TIME")
                                         {
                                             pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).LoadingTIME = Oven02_DateTime;
                                             strDVValue += Oven02_DateTime + ",";

                                         }
                                         else
                                         {
                                             pInfo.Unit(dintOven02UnitID).SubUnit(0).GLSAPD(dintLoop + 1).UnloadingTIME = Oven02_DateTime;
                                             strDVValue += Oven02_DateTime + ",";

                                         }
                                         Oven02_address = CommonAct.FunTypeConversion.funAddressAdd(Oven02_address, -6);
                                     }
                                 }
                                 strDVValue = strDVValue.Substring(0, strDVValue.Length - 1);

                                 pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F3_DVReport, 500, intUnitID, 0, strLOTID, strGLSID);
                                 pInfo.subLog_Set(InfoAct.clsInfo.LogType.Oven_DV, strDVValue);
                             }
                             catch (Exception ex)
                             {
                                 try
                                 {
                                     for (int dintLoop = 0; dintLoop < pInfo.Unit(5).SubUnit(0).GLSAPDCount; dintLoop++)
                                     {
                                         pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Oven02 DVData List {0} : {1}", dintLoop + 1, m_pEqpAct.funWordRead("W8300", pInfo.Unit(5).SubUnit(0).GLSAPD(dintLoop + 1).Length, EnuEQP.PLCRWType.Int_Data)));
                                     }
                                 }
                                 catch (Exception ex2)
                                 {
                                     pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Oven02 DVData Read Error!!");
                                     pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex2.ToString());
                                 }

                                 pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                             }

                             //Component In By Cleaner
                             pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_ComponentReport, 323, intUnitID, 0, strLOTID, strGLSID);

                             strLogMSG = string.Format("{0},{1},{2},{3},{4},{5}", dtNow.ToString("HH:mm:ss.ff"), strLOTID, strGLSID, pInfo.Unit(intUnitID).SubUnit(0).ReportUnitID, "Out", "");
                             pInfo.subLog_Set(InfoAct.clsInfo.LogType.GLSInOut, strLogMSG);
                         }
                        break;

                }

                if (intUnitID == 1 || intUnitID ==4)
                {
                    intSubUnitID = 0;
                }
                if (intBitVal == 1)
                {
                    try
                    {
                        pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSExist = true;
                    }
                    catch (Exception ex)
                    {
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("IntUnitID : {0}, intSubUnitID : {1}", intUnitID, intSubUnitID));
                        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    }
                    
                    //pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSID = "";
                    //pInfo.Unit(intUnitID).SubUnit(intSubUnitID).LOTID = "";
                }
                else
                {
                    pInfo.Unit(intUnitID).SubUnit(intSubUnitID).GLSExist = false;
                }

                //if(strCompBit != null) m_pEqpAct.subSetConfirmBit(strCompBit);
             
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                //m_pEqpAct.subSetConfirmBit(strCompBit);
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }

        #endregion
    }
}

