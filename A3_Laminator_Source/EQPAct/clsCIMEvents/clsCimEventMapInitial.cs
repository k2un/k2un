using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventMapInitial : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventMapInitial(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "MapInitial";
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
            string dstrWordAddress = "";
            string dstrWordData = "";
            string dstrHOSTPPID = "";
            int dintHOSTPPIDCount = 0;
            int dintEQPPPIDCount = 0;
            string[] dstrValue = null;
            int dintTemp = 0;
            string dstrTemp = "";
            //string dstrSQL = "";
            //int dintUserLevel = 0;
            //string dstrUserID = "";

            try
            {
                if (pInfo.EQP("Main").DummyPLC == true)
                {
                    #region 사용안함
                    ///////////////////////////////////Bit영역///////////////////////////////////////////
                    //pEqpAct.funBitWrite("B1600", "100");    //EQP State(NORMAL)
                    //pEqpAct.funBitWrite("B1604", "1000000");    //EQP Process State(Idle)

                    //pEqpAct.funBitWrite("B160C", "1");    //Auto Mode 여부(On: Auto 모드, Off: Manual 모드)

                    //Unit Normal
                    dstrWordAddress = "B1600";
                    for (int dintUnit = 0; dintUnit <= 15; dintUnit++)
                    {
                        string strTemp = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 16* dintUnit);
                        pEqpAct.funBitWrite(strTemp, "10010000");
                    }

                    dstrWordData = "";
                    //ECID 값 Write
                    //]2016/05/03]미사용
                    //dstrWordAddress = "W22C0";
                    //string strData = "";
                    //for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop++)
                    //{

                    //    //Word영역에 변경할 ECID를 Write한다.
                    //    pEqpAct.funWordWrite(dstrWordAddress, dintLoop.ToString(), EnuEQP.PLCRWType.Int_Data);
                    //    dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 1);

                    //    pEqpAct.funDoubleWordWrite(dstrWordAddress, Convert.ToString(dintLoop + 1), EnuEQP.PLCRWType.Int_Data);
                    //    dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 2);
                    //    pEqpAct.funDoubleWordWrite(dstrWordAddress, Convert.ToString(dintLoop + 2), EnuEQP.PLCRWType.Int_Data);
                    //    dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 2);
                    //    pEqpAct.funDoubleWordWrite(dstrWordAddress, Convert.ToString(dintLoop + 3), EnuEQP.PLCRWType.Int_Data);
                    //    dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 2);

                       
                    //}
                    dstrWordData = "";   //초기화


                    ///////////////////////////////Word영역(CIM영역)/////////////////////////////////////////
                    ////시간
                    //dstrWordAddress = this.pInfo.pPLCAddressInfo.wCIM_DateTimeSet;// "W1000";
                    //dstrTemp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(0, 4), EnuEQP.PLCRWType.Int_Data);
                    //dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(4, 2), EnuEQP.PLCRWType.Int_Data);
                    //dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(6, 2), EnuEQP.PLCRWType.Int_Data);
                    //dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(8, 2), EnuEQP.PLCRWType.Int_Data);
                    //dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(10, 2), EnuEQP.PLCRWType.Int_Data);
                    //dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(12, 2), EnuEQP.PLCRWType.Int_Data);

                    //pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    //dstrWordData = "";   //초기화



                    ////////////////////////////////Word영역(장비영역)//////////////////////////////////////////////////
                    ////dstrWordAddress = "W1500";
                    ////dstrWordData += pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);          //Alarm 발생 코드

                    ////pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화


                    ////dstrWordAddress = "W1502";
                    ////dstrWordData += pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);          //Alarm 해제 코드

                    ////pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화




                    ////장비 전체(Scrap GLS 정보)
                    //dstrWordAddress = this.pInfo.pPLCAddressInfo.wEQP_ScrapGlass;// "W1700";
                    //dstrWordData += pEqpAct.funWordWriteString(8, "LOTID01", EnuEQP.PLCRWType.ASCII_Data);  //LOTID
                    //dstrWordData += pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.ASCII_Data);          //SlotID

                    //pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    //dstrWordData = "";   //초기화


                    ////장비 전체(UnScrap GLS 정보)
                    //dstrWordAddress = this.pInfo.pPLCAddressInfo.wEQP_UnscrapGlass; // "W1710";
                    //dstrWordData += pEqpAct.funWordWriteString(8, "LOTID01", EnuEQP.PLCRWType.ASCII_Data);  //LOTID
                    //dstrWordData += pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.ASCII_Data);          //SlotID

                    //pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    //dstrWordData = "";   //초기화


                    ////장비 전체(Abort GLS 정보)
                    ////dstrWordAddress = "W1720";
                    ////dstrWordData += m_pEqpAct.funWordWriteString(7, "LOTID01", EnuEQP.PLCRWType.ASCII_Data);  //LOTID
                    ////dstrWordData += m_pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);          //SlotID
                    ////dstrWordData += m_pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);          //Abort된 UnitID

                    ////m_pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화


                    ////장비 전체(장비 전체(Move GLS 정보))
                    ////dstrWordAddress = "W1730";
                    ////dstrWordData += m_pEqpAct.funWordWriteString(7, "LOTID01", EnuEQP.PLCRWType.ASCII_Data);  //LOTID
                    ////dstrWordData += m_pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);          //SlotID
                    ////dstrWordData += m_pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);          //Move 전 UNIT-NO
                    ////dstrWordData += m_pEqpAct.funWordWriteString(1, "3", EnuEQP.PLCRWType.Int_Data);          //Move 후 UNIT-NO

                    ////m_pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화


                    ////장비 전체(User Login, out 정보)
                    ////dstrWordAddress = "W1520";
                    ////dstrWordData += m_pEqpAct.funWordWriteString(1, "2", EnuEQP.PLCRWType.Int_Data);          //Level
                    ////dstrWordData += m_pEqpAct.funWordWriteString(20, "USERID01", EnuEQP.PLCRWType.ASCII_Data);    //USERID

                    ////m_pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화


                    ////현재 운영중인 HOST_PPID
                    //dstrWordAddress = this.pInfo.pPLCAddressInfo.wEQP_CurrentHostPPID;// "W1504";
                    //dstrWordData += pEqpAct.funWordWriteString(10, "A", EnuEQP.PLCRWType.ASCII_Data);    //현재 운영중인 HOST_PPID

                    //pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    //dstrWordData = "";   //초기화

                    ////현재 운영중인 EQP_PPID
                    //dstrWordAddress = this.pInfo.pPLCAddressInfo.wEQP_CurrentEQPPPID;// "W1520";
                    //dstrWordData += pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);    //현재 운영중인 EQP_PPID

                    //pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    //dstrWordData = "";   //초기화

                    ////현재 장비에 등록되어 있는 HOSTPPID, EQPPPID의 개수
                    //dstrWordAddress = this.pInfo.pPLCAddressInfo.wEQP_CurrentHostPPIDCount;// "W150E";
                    //dstrWordData += pEqpAct.funWordWriteString(1, "4", EnuEQP.PLCRWType.Int_Data);    //HOSTPPID 개수
                    //dstrWordData += pEqpAct.funWordWriteString(1, "3", EnuEQP.PLCRWType.Int_Data);    //EQPPPID 개수

                    //pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    //dstrWordData = "";   //초기화


                    ////SVID값
                    //dstrWordAddress = this.pInfo.pPLCAddressInfo.wEQP_SVIDSet;// "W1B00";
                    ////for (int dintLoop = 7; dintLoop <= m_pInfo.Unit(0).SubUnit(0).SVIDCount; dintLoop++)
                    //for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).SVIDCount - pInfo.All.SVIDPLCNotReadLength; dintLoop++)
                    //{
                    //    if (pInfo.Unit(0).SubUnit(0).SVID(dintLoop) != null)
                    //    {
                    //        dintTemp = pInfo.Unit(0).SubUnit(0).SVID(dintLoop).Length;  //Write할 길이(Length)

                    //        if (pInfo.Unit(0).SubUnit(0).SVID(dintLoop).Format.ToUpper() == "YYYYMMDDHHMMSS")
                    //        {
                    //            dstrTemp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    //            dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(0, 4), EnuEQP.PLCRWType.Int_Data);
                    //            dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(4, 2), EnuEQP.PLCRWType.Int_Data);
                    //            dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(6, 2), EnuEQP.PLCRWType.Int_Data);
                    //            dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(8, 2), EnuEQP.PLCRWType.Int_Data);
                    //            dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(10, 2), EnuEQP.PLCRWType.Int_Data);
                    //            dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(12, 2), EnuEQP.PLCRWType.Int_Data);
                    //        }
                    //        else
                    //        {
                    //            dstrWordData += pEqpAct.funWordWriteString(dintTemp, dintLoop.ToString(), EnuEQP.PLCRWType.Int_Data);
                    //        }
                    //    }
                    //}
                    //pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    //dstrWordData = "";   //초기화


                    //////GLS APD값
                    ////dstrWordAddress = "W1D00";
                    ////for (int dintLoop = 7; dintLoop <= pInfo.Unit(0).SubUnit(0).GLSAPDCount; dintLoop++)
                    ////{
                    ////    dintTemp = pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Length;  //Write할 길이(Length)

                    ////    if (pInfo.Unit(0).SubUnit(0).GLSAPD(dintLoop).Format.ToUpper() == "YYYYMMDDHHMMSS")
                    ////    {
                    ////        dstrTemp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    ////        dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(0, 4), EnuEQP.PLCRWType.Int_Data);
                    ////        dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(4, 2), EnuEQP.PLCRWType.Int_Data);
                    ////        dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(6, 2), EnuEQP.PLCRWType.Int_Data);
                    ////        dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(8, 2), EnuEQP.PLCRWType.Int_Data);
                    ////        dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(10, 2), EnuEQP.PLCRWType.Int_Data);
                    ////        dstrWordData += pEqpAct.funWordWriteString(1, dstrTemp.Substring(12, 2), EnuEQP.PLCRWType.Int_Data);
                    ////    }
                    ////    else
                    ////    {
                    ////        dstrWordData += pEqpAct.funWordWriteString(dintTemp, dintLoop.ToString(), EnuEQP.PLCRWType.Int_Data);
                    ////    }
                    ////}
                    ////pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화


                    

                    //////장비에서 PM 변경시 PM Code
                    ////dstrWordAddress = "W1510";
                    ////dstrWordData += pEqpAct.funWordWriteString(1, "1234", EnuEQP.PLCRWType.Int_Data);          //Alarm 발생 코드

                    ////pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화

                    //////VCR Reading Mode(1: VCR ON-SKIP MODE)
                    ////dstrWordAddress = "W1511";
                    ////dstrWordData += pEqpAct.funWordWriteString(1, "1", EnuEQP.PLCRWType.Int_Data);

                    ////pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화

                    //////Wait time for glassid key-input(At reading fail)
                    ////dstrWordAddress = "W1512";
                    ////dstrWordData += pEqpAct.funWordWriteString(1, "0", EnuEQP.PLCRWType.Int_Data);     //10초

                    ////pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);
                    ////dstrWordData = "";   //초기화
                    #endregion
                }

                //EventBit 초기화
                pEqpAct.subEventBitInitialCmd();

                //현재 운영중인 HOST PPID, EQPPPID정보 저장
                //pInfo.All.CurrentHOSTPPID = pEqpAct.funWordRead(this.pInfo.pPLCAddressInfo.wEQP_CurrentHostPPID, 10, EnuEQP.PLCRWType.ASCII_Data);
                //pInfo.All.CurrentEQPPPID = pEqpAct.funWordRead(this.pInfo.pPLCAddressInfo.wEQP_CurrentEQPPPID, 1, EnuEQP.PLCRWType.Int_Data);
                              
                //SuperVisor Logout
                pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SupervisorLoginOut, false);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
