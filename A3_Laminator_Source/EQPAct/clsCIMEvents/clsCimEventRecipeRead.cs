using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Collections;

namespace EQPAct
{
    public class clsCimEventRecipeRead : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventRecipeRead(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "RecipeRead";
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
            string dstrBitAddress = pInfo.pPLCAddressInfo.bCIM_BuzzerOff;
            string[] arrCon = new string[64];
            char chrTemp;
            try
            {
                string strReadData = pEqpAct.funWordRead("W40C0", 64, EnuEQP.PLCRWType.Binary_Data);
                if (string.IsNullOrEmpty(strReadData))
                {
                    pInfo.All.RecipeReadFlag = false;
                    return;
                }

                for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                {
                    StringBuilder sb = new StringBuilder(strReadData.Substring((dintLoop * 16), 16));
                    for (int dintLoop2 = 0; dintLoop2 < sb.Length / 2; dintLoop2++)
                    {
                        chrTemp = sb[dintLoop2];
                        sb[dintLoop2] = sb[(sb.Length - dintLoop2) - 1];
                        sb[(sb.Length - dintLoop2) - 1] = chrTemp;
                    }
                    arrCon[dintLoop] = sb.ToString();
                }
                strReadData = string.Join("", arrCon);

                for (int dintLoop = 0; dintLoop < strReadData.Length; dintLoop++)
                {
                    if (strReadData[dintLoop] == '1')
                    {
                        if (pInfo.Unit(2).SubUnit(0).EQPPPID((dintLoop + 1).ToString()) == null)
                        {
                            pInfo.Unit(2).SubUnit(0).AddEQPPPID((dintLoop + 1).ToString());
                        }
                    }
                    else
                    {
                        pInfo.Unit(2).SubUnit(0).RemoveEQPPPID((dintLoop + 1).ToString());
                    }
                }

                //strReadData = pEqpAct.funWordRead("W50C0", 7, EnuEQP.PLCRWType.Binary_Data);
                //for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                //{
                //    StringBuilder sb = new StringBuilder(strReadData.Substring((dintLoop * 16), 16));
                //    for (int dintLoop2 = 0; dintLoop2 < sb.Length / 2; dintLoop2++)
                //    {
                //        chrTemp = sb[dintLoop2];
                //        sb[dintLoop2] = sb[(sb.Length - dintLoop2) - 1];
                //        sb[(sb.Length - dintLoop2) - 1] = chrTemp;
                //    }
                //    arrCon[dintLoop] = sb.ToString();
                //}
                //strReadData = string.Join("", arrCon);

                //for (int dintLoop = 0; dintLoop < strReadData.Length; dintLoop++)
                //{
                //    if (strReadData[dintLoop] == '1')
                //    {
                //        if (pInfo.Unit(3).SubUnit(0).EQPPPID((dintLoop + 1).ToString()) == null)
                //        {
                //            pInfo.Unit(3).SubUnit(0).AddEQPPPID((dintLoop + 1).ToString());
                //        }
                //    }
                //    else
                //    {
                //        pInfo.Unit(3).SubUnit(0).RemoveEQPPPID((dintLoop + 1).ToString());
                //    }
                //}


                strReadData = pEqpAct.funWordRead("W6080", 64, EnuEQP.PLCRWType.Binary_Data);
                for (int dintLoop = 0; dintLoop < arrCon.Length; dintLoop++)
                {
                    StringBuilder sb = new StringBuilder(strReadData.Substring((dintLoop * 16), 16));
                    for (int dintLoop2 = 0; dintLoop2 < sb.Length / 2; dintLoop2++)
                    {
                        chrTemp = sb[dintLoop2];
                        sb[dintLoop2] = sb[(sb.Length - dintLoop2) - 1];
                        sb[(sb.Length - dintLoop2) - 1] = chrTemp;
                    }
                    arrCon[dintLoop] = sb.ToString();
                }
                strReadData = string.Join("", arrCon);

                for (int dintLoop = 0; dintLoop < strReadData.Length; dintLoop++)
                {
                    if (strReadData[dintLoop] == '1')
                    {
                        if (pInfo.Unit(4).SubUnit(0).EQPPPID((dintLoop + 1).ToString()) == null)
                        {
                            pInfo.Unit(4).SubUnit(0).AddEQPPPID((dintLoop + 1).ToString());
                        }
                    }
                    else
                    {
                        pInfo.Unit(4).SubUnit(0).RemoveEQPPPID((dintLoop + 1).ToString());
                    }
                }
                
                pInfo.All.RecipeReadFlag = false;

            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //private void subLotInfomationData(int intPortID, string strCSTID, string strLOTID)
        //{
        //    string strAddress = "W9000";
        //    try
        //    {
        //        InfoAct.clsPort CurrentPort = pInfo.Port(intPortID);
        //        InfoAct.clsLOT CurrentLot = pInfo.LOTID(strLOTID);

        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 1024 * (intPortID - 1));

        //        pEqpAct.funWordWrite(strAddress, ("P0" + intPortID).PadRight(4, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 2);

        //        pEqpAct.funWordWrite(strAddress, strLOTID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

        //        pEqpAct.funWordWrite(strAddress, strCSTID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

        //        pEqpAct.funWordWrite(strAddress, CurrentLot.LOTJudge.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

        //        pEqpAct.funWordWrite(strAddress, CurrentLot.LSORTTYPE.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

        //        pEqpAct.funWordWrite(strAddress, CurrentLot.OPERID.PadRight(10, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 5);

        //        pEqpAct.funWordWrite(strAddress, CurrentLot.PRODID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

        //        pEqpAct.funWordWrite(strAddress, CurrentLot.QTY.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);
        //        strAddress = FunTypeConversion.funAddressAdd(strAddress, 24);

        //        string[] arrData = new string[25];
        //        for (int dintLoop = 0; dintLoop < pInfo.Port(intPortID).SlotCount; dintLoop++)
        //        {
        //            if (string.IsNullOrEmpty(CurrentPort.Slot(dintLoop + 1).GLSID))
        //            {
        //                pEqpAct.funWordWrite(strAddress, "0".PadLeft(128, '0'), EnuEQP.PLCRWType.Hex_Data);
        //                pEqpAct.funWordWrite(strAddress, (dintLoop + 1).ToString(), EnuEQP.PLCRWType.Int_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 32);
        //            }
        //            else
        //            {
        //                pEqpAct.funWordWrite(strAddress, (dintLoop + 1).ToString(), EnuEQP.PLCRWType.Int_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

        //                pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop+1).GLSID).GLSID.PadRight(20, ' ') , EnuEQP.PLCRWType.ASCII_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

        //                pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).HostPPID.PadRight(20, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 10);

        //                pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).GLSJudge.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

        //                pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).SMPLFLAG.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

        //                pEqpAct.funWordWrite(strAddress, CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).RWKCNT.PadRight(2, ' '), EnuEQP.PLCRWType.ASCII_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

        //                string strPPID = CurrentLot.GLSID(CurrentPort.Slot(dintLoop + 1).GLSID).HostPPID;
        //                pEqpAct.funWordWrite(strAddress, pInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID).CLEANER_EQPPPID, EnuEQP.PLCRWType.Int_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

        //                pEqpAct.funWordWrite(strAddress, pInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID).Oven1_EQPPPID, EnuEQP.PLCRWType.Int_Data);
        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 1);

        //                strAddress = FunTypeConversion.funAddressAdd(strAddress, 5);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
        //    }
        //}
        #endregion
    }
}

