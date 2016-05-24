using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Collections;

namespace EQPAct
{
    public class clsCimEventPortCMD : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventPortCMD(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "PortCMD";
        }
        #endregion

        #region Methods
        /// <summary>
        /// CIM에서 설비로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : cmdName
        /// parameters[1] : 1st parameter
        /// parameters[2] : 2nd parameter
        /// parameters[3] : 3rd parameter
        /// parameters[4] : 4th parameter
        /// parameters[5] : 5th Parameter
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string dstrBitAddress = "B0010";

            try
            {
                int intPortNo = Convert.ToInt32(parameters[0].ToString().Substring(1));
                string strCSTID = parameters[1].ToString().Trim();
                string strLOTID = parameters[2].ToString().Trim();
                string strRCMD = parameters[3].ToString();
                StringBuilder sbData = new StringBuilder("0".PadLeft(32, '0'));
                
                string strGLSID = "";

                //for (int dintLoop = 0; dintLoop < pInfo.PortCount(); dintLoop++)
                //{
                
                    if (pInfo.Port(intPortNo).CSTID == strCSTID)
                    {
                        switch (strRCMD)
                        {
                            case "1":
                                if (pInfo.Port(intPortNo).LOTID == strLOTID)
                                {
                                    subLotInfomationData(intPortNo, strCSTID, strLOTID);
                                    pEqpAct.funWordWrite("W00C0", intPortNo.ToString(), EnuEQP.PLCRWType.Int_Data);
                                    for (int dintLoop2 = 0; dintLoop2 < pInfo.Port(intPortNo).SlotCount; dintLoop2++)
                                    {
                                        if (pInfo.LOTID(strLOTID) != null && pInfo.LOTID(strLOTID).GLSID(pInfo.Port(intPortNo).Slot(dintLoop2 + 1).GLSID) != null)
                                        {
                                            if (pInfo.Port(intPortNo).Slot(dintLoop2 + 1).SLOTINFO == "W")
                                            {
                                                if (pInfo.All.ControlState == "1")
                                                {
                                                    if (pInfo.LOTID(strLOTID).GLSID(pInfo.Port(intPortNo).Slot(dintLoop2 + 1).GLSID).SkipFalg)
                                                    {
                                                        sbData[dintLoop2] = '0';
                                                    }
                                                    else
                                                    {
                                                        sbData[dintLoop2] = '1';
                                                    }
                                                }
                                                else
                                                {
                                                    sbData[dintLoop2] = '1';
                                                }
                                            }
                                        }
                                    }
                                    string strData = sbData[15].ToString() + sbData[14].ToString() + sbData[13].ToString() + sbData[12].ToString() + sbData[11].ToString()
                                                    + sbData[10].ToString() + sbData[9].ToString() + sbData[8].ToString() + sbData[7].ToString() + sbData[6].ToString()
                                                    + sbData[5].ToString() + sbData[4].ToString() + sbData[3].ToString() + sbData[2].ToString() + sbData[1].ToString() + sbData[0].ToString();

                                    pEqpAct.funWordWrite("W00C1", strData, EnuEQP.PLCRWType.Binary_Data);

                                    strData = sbData[31].ToString() + sbData[30].ToString() + sbData[29].ToString() + sbData[28].ToString() + sbData[27].ToString()
                                                    + sbData[26].ToString() + sbData[25].ToString() + sbData[24].ToString() + sbData[23].ToString() + sbData[22].ToString()
                                                    + sbData[21].ToString() + sbData[20].ToString() + sbData[19].ToString() + sbData[18].ToString() + sbData[17].ToString() + sbData[16].ToString();
                                    pEqpAct.funWordWrite("W00C2", strData, EnuEQP.PLCRWType.Binary_Data);


                                    if (pEqpAct.funBitRead(dstrBitAddress, 1) == "1")
                                    {
                                        pEqpAct.funBitWrite(dstrBitAddress, "0");
                                    }

                                    pEqpAct.funBitWrite(dstrBitAddress, "1");

                                    if (pInfo.All.ControlState != "2")
                                    {
                                        intPortNo = pInfo.LOTID(strLOTID).InPortID;
                                        for (int dintIndex = 0; dintIndex < pInfo.Port(intPortNo).SlotCount; dintIndex++)
                                        {
                                            strGLSID = pInfo.Port(intPortNo).Slot(dintIndex + 1).GLSID;


                                            if (string.IsNullOrEmpty(strGLSID) == false && pInfo.LOTID(strLOTID) != null && pInfo.LOTID(strLOTID).GLSID(strGLSID) != null)
                                            {
                                                if (pInfo.LOTID(strLOTID).GLSID(strGLSID).SkipFalg)
                                                {
                                                    pInfo.Port(intPortNo).Slot(dintIndex + 1).SLOTINFO = "S";
                                                }
                                                else
                                                {
                                                    pInfo.Port(intPortNo).Slot(dintIndex + 1).SLOTINFO = "W";
                                                }
                                            }
                                            else
                                            {
                                                if (pInfo.Port(intPortNo).Slot(dintIndex + 1).SLOTMAP == "O")
                                                {
                                                    pInfo.Port(intPortNo).Slot(dintIndex + 1).SLOTINFO = "S";
                                                }
                                                else
                                                {
                                                    pInfo.Port(intPortNo).Slot(dintIndex + 1).SLOTINFO = "E";
                                                }
                                            }
                                        }
                                    }

                                    pInfo.All.StartCMDFLAG = true;
                                    pInfo.Port(intPortNo).LOTST = "2";
                                }
                                break;

                            case "2":
                                pEqpAct.funWordWrite("W00D0", intPortNo.ToString(), EnuEQP.PLCRWType.Int_Data);
                                dstrBitAddress = "B0011";
                                pEqpAct.funBitWrite(dstrBitAddress, "1");
                                pInfo.All.CancelCMDFLAG = true;
                                //pInfo.Port(intPortNo).LOTST = "5";

                                break;

                            case "3":
                                pEqpAct.funWordWrite("W00E0", intPortNo.ToString(), EnuEQP.PLCRWType.Int_Data);
                                dstrBitAddress = "B0012";
                                pEqpAct.funBitWrite(dstrBitAddress, "1");
                                //pInfo.Port(intPortNo).LOTST = "6";
                                pInfo.All.AbortCMDFlag = true;
                                break;
                        }
                    }
                
                //}

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subLotInfomationData(int intPortID, string strCSTID, string strLOTID)
        {
            string strAddress = "W9000";
            try
            {
                InfoAct.clsPort CurrentPort = pInfo.Port(intPortID);
                InfoAct.clsLOT CurrentLot = pInfo.LOTID(strLOTID);

                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1024 * (intPortID - 1));

                pEqpAct.funWordWrite(strAddress, ("P0" + intPortID).PadRight(4, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 2);

                pEqpAct.funWordWrite(strAddress, strLOTID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

                pEqpAct.funWordWrite(strAddress, strCSTID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

                pEqpAct.funWordWrite(strAddress, CurrentLot.LOTJudge.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                pEqpAct.funWordWrite(strAddress, CurrentLot.LSORTTYPE.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                pEqpAct.funWordWrite(strAddress, CurrentLot.OPERID.PadRight(10, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 5);

                pEqpAct.funWordWrite(strAddress, CurrentLot.PRODID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

                pEqpAct.funWordWrite(strAddress, CurrentLot.QTY.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);
                strAddress = FunTypeConversion.funAddressAdd(strAddress, 24);

                string[] arrData = new string[25];
                for (int dintLoop = 0; dintLoop < pInfo.Port(intPortID).SlotCount; dintLoop++)
                {
                    if (string.IsNullOrEmpty(CurrentPort.Slot(dintLoop + 1).GLSID))
                    {
                        pEqpAct.funWordWrite(strAddress, "0".PadLeft(128, '0'), EnuEQP.PLCRWType.Hex_Data);
                        pEqpAct.funWordWrite(strAddress, (dintLoop + 1).ToString(), EnuEQP.PLCRWType.Int_Data);
                        strAddress = FunTypeConversion.funAddressAdd(strAddress, 32);
                    }
                    else
                    {
                        if(CurrentLot.GLSID(CurrentPort.Slot(dintLoop+1).GLSID) == null)
                        {
                            pEqpAct.funWordWrite(strAddress, "0".PadLeft(128, '0'), EnuEQP.PLCRWType.Hex_Data);
                            pEqpAct.funWordWrite(strAddress, (dintLoop + 1).ToString(), EnuEQP.PLCRWType.Int_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 32);
                        }
                        else
                        {
                            pEqpAct.funWordWrite(strAddress, (dintLoop + 1).ToString(), EnuEQP.PLCRWType.Int_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                            pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).GLSID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

                            pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).HostPPID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

                            pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).GLSJudge.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                            pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).SMPLFLAG.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                            pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).RWKCNT.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                            string strPPID = CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).HostPPID;
                            pEqpAct.funWordWrite(strAddress, pInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID).CLEANER_EQPPPID, EnuEQP.PLCRWType.Int_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                            pEqpAct.funWordWrite(strAddress, pInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID).Oven1_EQPPPID, EnuEQP.PLCRWType.Int_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                            pEqpAct.funWordWrite(strAddress, (Convert.ToSingle( pInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID).Tickness)*100).ToString(), EnuEQP.PLCRWType.Int_Data);
                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

                            strAddress = FunTypeConversion.funAddressAdd(strAddress, 5);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}

