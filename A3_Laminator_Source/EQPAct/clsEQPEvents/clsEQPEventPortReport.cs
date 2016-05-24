using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Threading;
using System.Collections;

namespace EQPAct
{
    public class clsEQPEventPortReport : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventPortReport(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actPortReport";
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
            string strAddress = "W3100";
            int dintCEID = 0;
            try
            {
                int intUnitID = Convert.ToInt32(parameters[2]);
                int intPortNo = Convert.ToInt32(parameters[3]);
                int intBitVal = Convert.ToInt32(parameters[4]);
                int intACTVal = Convert.ToInt32(parameters[1]);

                strAddress = FunTypeConversion.funAddressAdd(strAddress, (intPortNo - 1) * 16);

                if (intBitVal != 1) return;

                string strMappingData1  = "";
                string strMappingData2  = "";
                string strMappingData = "";

                pInfo.Port(intPortNo).CSTID = m_pEqpAct.funWordRead(strAddress, 10, EnuEQP.PLCRWType.ASCII_Data).Trim();
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);
                strMappingData1 = m_pEqpAct.funWordRead(strAddress, 1, EnuEQP.PLCRWType.Binary_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);
                strMappingData2 = m_pEqpAct.funWordRead(strAddress, 1, EnuEQP.PLCRWType.Binary_Data);
                strMappingData = strMappingData2 + strMappingData1;

                string strReversMappingData = "";
                for (int dintLoop = strMappingData.Length; dintLoop > 0; dintLoop--)
                {
                    strReversMappingData += strMappingData.Substring(dintLoop - 1, 1);
                }

                if (intACTVal != 4 && intACTVal != 5)
                {
                    pInfo.Port(intPortNo).PTST = intACTVal.ToString();
                }

                switch (intACTVal)
                {
                    case 0: //Port Load Request
                        ArrayList arrCon = new ArrayList();
                        foreach (string strLotID in pInfo.LOT())
                        {
                            if (pInfo.LOTID(strLotID).InPortID == intPortNo)
                            {
                                arrCon.Add(strLotID);
                            }
                        }

                        for (int dintLoop = 0; dintLoop < arrCon.Count; dintLoop++)
                        {
                            pInfo.RemoveLotID(arrCon[dintLoop].ToString());
                        }

                        pInfo.Port(intPortNo).PortMapping = "E".PadRight(25, 'E');
                        pInfo.Port(intPortNo).LCTime = "";
                        dintCEID = 200;
                        pInfo.Port(intPortNo).SLOTINFO = "E".PadLeft(25, 'E');
                        pInfo.Port(intPortNo).SLOTMAP = "X".PadLeft(25, 'X');
                        pInfo.Port(intPortNo).S9F13MSGSendFlag = false;
                        break;

                    case 1: //Port Load Complete
                        //Pre Load 보고
                        //pInfo.Port(intPortNo).PortStatus = "";
                        pInfo.Port(intPortNo).S9F13MSGSendFlag = false;
                        pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_PortOccupationStatusChange, 201, intPortNo);

                        pInfo.Port(intPortNo).PortMapping = strReversMappingData.Substring(0,25);
                        
                        for (int dintLoop = 0; dintLoop < pInfo.Port(intPortNo).PortMapping.Length; dintLoop++)
                        {
                            if (pInfo.Port(intPortNo).PortMapping.Substring(dintLoop, 1) == "1")
                            {
                                pInfo.Port(intPortNo).Slot(dintLoop + 1).SLOTMAP = "O";
                                pInfo.Port(intPortNo).Slot(dintLoop + 1).SLOTINFO = "W";
                            }
                            else
                            {
                                pInfo.Port(intPortNo).Slot(dintLoop + 1).SLOTMAP = "X";
                                pInfo.Port(intPortNo).Slot(dintLoop + 1).SLOTINFO = "E";

                            }
                        }

                        pInfo.Port(intPortNo).LCTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        dintCEID = 202;

                        if (pInfo.All.ControlState == "0")
                        {
                            //Lot Information 창을 띄운다.
                            pInfo.subOPCall_Set(InfoAct.clsInfo.OPCall.LOTInformation, intPortNo, "", "");
                        }
                        else
                        {
                            pInfo.Port(intPortNo).bolLotInfoDownloadCheck = true;
                        }

                        break;

                    case 2: //Port Unload Requset
                        pInfo.Port(intPortNo).PortMapping = strReversMappingData.Substring(0, 25);
                        dintCEID = 203;

                        pInfo.RemoveLotID(pInfo.Port(intPortNo).LOTID);
                        break;

                    case 3: //Port Unload Complete
                        dintCEID = 204;
                        pInfo.Port(intPortNo).PortMapping = "E".PadRight(25, '0');
                        pInfo.Port(intPortNo).UCTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        break;

                    case 4:
                        //pInfo.Port(intPortNo).PTST = "4";
                        //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_PortStatusChange, 213, intPortNo);
                        break;

                    case 5:
                        //pInfo.Port(intPortNo).PTST = "5";
                        //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_PortStatusChange, 213, intPortNo);
                        break;

                }

                if (intACTVal != 4 && intACTVal != 5)
                {
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11_PortOccupationStatusChange, dintCEID, intPortNo);
                }

                //pInfo.subPortDataRecovery();
             
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }

        #endregion
    }
}
