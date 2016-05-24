using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.IO.Ports;
using System.Net.Sockets;
using System.Net;
using InfoAct;
using CommonAct;
using System.IO;
using System.Collections;

namespace EQPAct
{
    public class clsCimEventRecovery : clsCIMEvent, ICIMEvent //[2015/03/10](Add by HS)
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventRecovery(string strUnitID)
        {
            //strUnitID = strUnitID;
            strEventName = "Recovery";
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
            try
            {
                string dstrWordAddress = "";
                string strFilmID = "";
                string strProductID = "";
                string strFilmCount = "";
                string strCSTID = "";

                dstrWordAddress = "W1680";
                strFilmID = pEqpAct.funWordRead(dstrWordAddress, 50, EnuEQP.PLCRWType.ASCII_Data);
                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 50);
                strFilmCount = pEqpAct.funWordRead(dstrWordAddress, 1, EnuEQP.PLCRWType.Int_Data);
                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 1);
                strProductID = pEqpAct.funWordRead(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data);
                
                dstrWordAddress = "W2220";
                strCSTID = pEqpAct.funWordRead(dstrWordAddress, 8, EnuEQP.PLCRWType.ASCII_Data).Trim();
                if (string.IsNullOrEmpty(strCSTID.Trim()) == false)
                {
                    if (pInfo.LOTID(strCSTID) == null)
                    {
                        pInfo.AddLOT(strCSTID);
                    }

                    int dUsed_Count = Convert.ToInt32(strFilmCount);

                    pInfo.Port(2).CSTID = strCSTID;
                    for (int dintloop = 1; dintloop <= dUsed_Count; dintloop++)
                    {
                        if (pInfo.LOTID(strCSTID).Slot(dintloop) == null)
                        {
                            pInfo.LOTID(strCSTID).AddSlot(dintloop);
                        }
                        pInfo.LOTID(strCSTID).Slot(dintloop).GlassID = strFilmID;
                        pInfo.LOTID(strCSTID).Slot(dintloop).USE_COUNT = strFilmCount;
                        pInfo.LOTID(strCSTID).Slot(dintloop).PRODUCTID = strProductID;
                    }
                }


                string strLotID = "";
                int dintSlotNo = 0;
                if (pEqpAct.funBitRead("B167B", 1, false) == "1")
                {
                    strLotID = pEqpAct.funWordRead("W22C0", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W22C8", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                    pInfo.Unit(3).SubUnit(1).FilmID = strFilmID.Trim();//160503 keun strLotID -> strFilmID로 수정.
                    pInfo.Unit(3).SubUnit(1).FilmCount = dintSlotNo;
                    if (this.pInfo.Unit(3).SubUnit(1).AddCurrGLS(strLotID) == true)
                    {
                        this.pInfo.Unit(3).SubUnit(1).CurrGLS(strLotID).FilmID = strFilmID;//160503 keun strLotID -> strFilmID로 수정.
                        this.pInfo.Unit(3).SubUnit(1).CurrGLS(strLotID).LOTID = strLotID;
                        this.pInfo.Unit(3).SubUnit(1).CurrGLS(strLotID).SlotID = dintSlotNo;
                        this.pInfo.Unit(3).SubUnit(1).CurrGLS(strLotID).PRODUCTID = strProductID;
                        this.pInfo.Unit(3).SubUnit(1).CurrGLS(strLotID).H_PANELID = strLotID;
                        if (string.IsNullOrEmpty(strLotID.Trim()) == false)
                        {
                            if (pInfo.LOTID(strLotID.Trim()) == null)
                            {
                                pInfo.AddLOT(strLotID.Trim());
                            }
                        }
                        this.pInfo.LOTID(strLotID.Trim()).Slot(dintSlotNo).PRODUCTID = strProductID;
                    }
                }

                if (pEqpAct.funBitRead("B168B", 1, false) == "1")
                {
                    strLotID = pEqpAct.funWordRead("W22E0", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W22E8", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                    pInfo.Unit(3).SubUnit(2).FilmID = strFilmID.Trim();
                    pInfo.Unit(3).SubUnit(2).FilmCount = dintSlotNo;
                    if (this.pInfo.Unit(3).SubUnit(2).AddCurrGLS(strLotID) == true)
                    {
                        this.pInfo.Unit(3).SubUnit(2).CurrGLS(strLotID).FilmID = strFilmID;
                        this.pInfo.Unit(3).SubUnit(2).CurrGLS(strLotID).LOTID = strLotID;
                        this.pInfo.Unit(3).SubUnit(2).CurrGLS(strLotID).SlotID = dintSlotNo;
                        this.pInfo.Unit(3).SubUnit(2).CurrGLS(strLotID).PRODUCTID = strProductID;
                        this.pInfo.Unit(3).SubUnit(2).CurrGLS(strLotID).H_PANELID = strLotID;
                        if (string.IsNullOrEmpty(strLotID.Trim()) == false)
                        {
                            if (pInfo.LOTID(strLotID) == null)
                            {
                                pInfo.AddLOT(strLotID);
                            }
                        }
                        this.pInfo.LOTID(strLotID).Slot(dintSlotNo).PRODUCTID = strProductID;
                    }
                }

                if (pEqpAct.funBitRead("B169B", 1, false) == "1")
                {
                    strLotID = pEqpAct.funWordRead("W2300", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W2308", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                    pInfo.Unit(3).SubUnit(3).FilmID = strFilmID.Trim();
                    pInfo.Unit(3).SubUnit(3).FilmCount = dintSlotNo;
                    if (this.pInfo.Unit(3).SubUnit(3).AddCurrGLS(strLotID) == true)
                    {
                        this.pInfo.Unit(3).SubUnit(3).CurrGLS(strLotID).FilmID = strFilmID;
                        this.pInfo.Unit(3).SubUnit(3).CurrGLS(strLotID).LOTID = strLotID;
                        this.pInfo.Unit(3).SubUnit(3).CurrGLS(strLotID).SlotID = dintSlotNo;
                        this.pInfo.Unit(3).SubUnit(3).CurrGLS(strLotID).PRODUCTID = strProductID;
                        this.pInfo.Unit(3).SubUnit(3).CurrGLS(strLotID).H_PANELID = strLotID;
                        if (string.IsNullOrEmpty(strLotID.Trim()) == false)
                        {
                            if (pInfo.LOTID(strLotID) == null)
                            {
                                pInfo.AddLOT(strLotID);
                            }
                        }
                        this.pInfo.LOTID(strLotID).Slot(dintSlotNo).PRODUCTID = strProductID;
                    }
                }

                if (pEqpAct.funBitRead("B16AB", 1, false) == "1")
                {
                    strLotID = pEqpAct.funWordRead("W2320", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W2328", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                    pInfo.Unit(3).SubUnit(4).FilmID = strFilmID.Trim();
                    pInfo.Unit(3).SubUnit(4).FilmCount = dintSlotNo;
                    if (this.pInfo.Unit(3).SubUnit(4).AddCurrGLS(strLotID) == true)
                    {
                        this.pInfo.Unit(3).SubUnit(4).CurrGLS(strLotID).FilmID = strFilmID;
                        this.pInfo.Unit(3).SubUnit(4).CurrGLS(strLotID).LOTID = strLotID;
                        this.pInfo.Unit(3).SubUnit(4).CurrGLS(strLotID).SlotID = dintSlotNo;
                        this.pInfo.Unit(3).SubUnit(4).CurrGLS(strLotID).PRODUCTID = strProductID;
                        this.pInfo.Unit(3).SubUnit(4).CurrGLS(strLotID).H_PANELID = strLotID;
                        if (string.IsNullOrEmpty(strLotID.Trim()) == false)
                        {
                            if (pInfo.LOTID(strLotID) == null)
                            {
                                pInfo.AddLOT(strLotID);
                            }
                        }
                        this.pInfo.LOTID(strLotID).Slot(dintSlotNo).PRODUCTID = strProductID;
                    }
                }

                if (pEqpAct.funBitRead("B16BB", 1, false) == "1")
                {
                    strLotID = pEqpAct.funWordRead("W2340", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W2348", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                    pInfo.Unit(3).SubUnit(5).FilmID = strFilmID.Trim();
                    pInfo.Unit(3).SubUnit(5).FilmCount = dintSlotNo;
                    if (this.pInfo.Unit(3).SubUnit(5).AddCurrGLS(strLotID) == true)
                    {
                        this.pInfo.Unit(3).SubUnit(5).CurrGLS(strLotID).FilmID = strFilmID;
                        this.pInfo.Unit(3).SubUnit(5).CurrGLS(strLotID).LOTID = strLotID;
                        this.pInfo.Unit(3).SubUnit(5).CurrGLS(strLotID).SlotID = dintSlotNo;
                        this.pInfo.Unit(3).SubUnit(5).CurrGLS(strLotID).PRODUCTID = strProductID;
                        this.pInfo.Unit(3).SubUnit(5).CurrGLS(strLotID).H_PANELID = strLotID;
                        if (string.IsNullOrEmpty(strLotID.Trim()) == false)
                        {
                            if (pInfo.LOTID(strLotID) == null)
                            {
                                pInfo.AddLOT(strLotID);
                            }
                        }
                        this.pInfo.LOTID(strLotID).Slot(dintSlotNo).PRODUCTID = strProductID;
                    }
                }

                if (pEqpAct.funBitRead("B16CB", 1, false) == "1")
                {
                    strLotID = pEqpAct.funWordRead("W2360", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W2368", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                    pInfo.Unit(3).SubUnit(6).FilmID = strFilmID.Trim();
                    pInfo.Unit(3).SubUnit(6).FilmCount = dintSlotNo;
                    if (this.pInfo.Unit(3).SubUnit(6).AddCurrGLS(strLotID) == true)
                    {
                        this.pInfo.Unit(3).SubUnit(6).CurrGLS(strLotID).FilmID = strFilmID;
                        this.pInfo.Unit(3).SubUnit(6).CurrGLS(strLotID).LOTID = strLotID;
                        this.pInfo.Unit(3).SubUnit(6).CurrGLS(strLotID).SlotID = dintSlotNo;
                        this.pInfo.Unit(3).SubUnit(6).CurrGLS(strLotID).PRODUCTID = strProductID;
                        this.pInfo.Unit(3).SubUnit(6).CurrGLS(strLotID).H_PANELID = strLotID;
                        if (string.IsNullOrEmpty(strLotID.Trim()) == false)
                        {
                            if (pInfo.LOTID(strLotID) == null)
                            {
                                pInfo.AddLOT(strLotID);
                            }
                        }
                        this.pInfo.LOTID(strLotID).Slot(dintSlotNo).PRODUCTID = strProductID;
                    }
                }

                //if (pEqpAct.funBitRead("B16DB", 1, false) == "1")
                //{
                //    strLotID = pEqpAct.funWordRead("W22C0", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                //    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W22C8", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                //    pInfo.Unit(3).SubUnit(1).FilmID = strLotID;
                //    pInfo.Unit(3).SubUnit(1).FilmCount = dintSlotNo;
                //}

                //if (pEqpAct.funBitRead("B16EB", 1, false) == "1")
                //{
                //    strLotID = pEqpAct.funWordRead("W22C0", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                //    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W22C8", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                //    pInfo.Unit(3).SubUnit(1).FilmID = strLotID;
                //    pInfo.Unit(3).SubUnit(1).FilmCount = dintSlotNo;
                //}

                //if (pEqpAct.funBitRead("B16FB", 1, false) == "1")
                //{
                //    strLotID = pEqpAct.funWordRead("W22C0", 8, CommonAct.EnuEQP.PLCRWType.ASCII_Data);
                //    dintSlotNo = Convert.ToInt32(pEqpAct.funWordRead("W22C8", 1, CommonAct.EnuEQP.PLCRWType.Int_Data));
                //    pInfo.Unit(3).SubUnit(1).FilmID = strLotID;
                //    pInfo.Unit(3).SubUnit(1).FilmCount = dintSlotNo;
                //}
                
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());

            }
        }
        #endregion
    }
}
