using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Collections;

namespace EQPAct
{
    public class clsEQPEventEqpPPIDBodyChange : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventEqpPPIDBodyChange(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actEQPPPIDBodyChange";
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

            StringBuilder dstrLog = new StringBuilder();
            string dstrEQPPPID = "";
            int dintPPIDType = 0;
            string dstrTemp = "";
            string strWordAddress = "";

            try
            {
                //m_pEqpAct.subWordReadSave("W2040", 10, EnuEQP.PLCRWType.ASCII_Data);    //HOST PPID
                dstrEQPPPID = m_pEqpAct.funWordRead("W2540", 1, EnuEQP.PLCRWType.Int_Data);
                dintPPIDType = 1;
                #region 수정 전
                //pInfo.Unit(0).SubUnit(0).RemoveEQPPPID(dstrEQPPPID);
                //strWordAddress = "W254C";
                //if (pInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrEQPPPID) == true)
                //{
                //    for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                //    {
                //        //EQPPPID에 Body 저장
                //        if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).AddPPIDBody(dintLoop) == true)
                //        {
                //            pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).DESC = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).DESC;
                //            pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Format = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format;
                //            pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Length = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length;
                //            pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).ModuleID = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).ModuleID;
                //            pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Name = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Name;
                //            pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Range = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Range;
                //            pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Unit = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Unit;
                //            pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).UseMode = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).UseMode;
                //            if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length == 2)
                //            {
                //                dstrTemp = FunStringH.funPoint(m_pEqpAct.funWordRead(strWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, EnuEQP.PLCRWType.Int_Data), pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format);
                //                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value = dstrTemp;
                //            }
                //            else
                //            {
                //                dstrTemp = m_pEqpAct.funWordRead(strWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, EnuEQP.PLCRWType.ASCII_Data);
                //                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value = dstrTemp;
                //            }
                //            strWordAddress = CommonAct.FunTypeConversion.funAddressAdd(strWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length);
                //        }
                //    }
                //}
                # endregion
                #region 수정 후
                int dintReadcount = (pInfo.Unit(0).SubUnit(0).PPIDBodyCount - 5) * 2;
                string dstrReadData = "";
                int dintLength = 0;
                string strValue = "";
                int dintStartIndex = 0;
                strWordAddress = "W254C";
                if (pInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrEQPPPID) == true)
                {
                    //if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBodyCount == 0)
                    //{
                        //[2015/04/20] PPID Body 값 Block으로 한번에(Add by HS)
                        dstrReadData = m_pEqpAct.funWordRead("W254C", dintReadcount, EnuEQP.PLCRWType.Hex_Data, false).Trim();
                        #region EQPPPID에 Body 저장
                        for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount - 5; dintLoop++)
                        {
                            if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).AddPPIDBody(dintLoop) == true)
                            {
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).DESC = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).DESC;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Format = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Length = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).ModuleID = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).ModuleID;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Name = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Name;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Range = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Range;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Unit = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Unit;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).UseMode = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).UseMode;

                                dintLength = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length * 4;   //Length에 맞게 자를 문자열 개수를 가져온다.
                                dstrTemp = dstrReadData.Substring(dintStartIndex, dintLength);

                                if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length > 1)
                                {
                                    string dstrTemp1 = dstrTemp.Substring(0, 4);
                                    string dstrTemp2 = dstrTemp.Substring(4, 4);
                                    dstrTemp = dstrTemp2 + dstrTemp1;

                                    strValue = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.Decimal);
                                    dstrTemp = FunStringH.funPoint(strValue, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format);
                                    pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value = dstrTemp;
                                }
                                else
                                {
                                    strValue = FunTypeConversion.funHexConvert(dstrTemp, EnuEQP.StringType.ASCString);
                                    pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value = strValue;
                                }

                                dintStartIndex += dintLength;

                                #region 변경전 [2015.04.20] Modify by HS
                                //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).DESC = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).DESC;
                                //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Format = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format;
                                //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Length = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length;
                                //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).ModuleID = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).ModuleID;
                                //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Name = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Name;
                                //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Range = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Range;
                                //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Unit = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Unit;
                                //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).UseMode = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).UseMode;

                                //if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length == 2)
                                //{
                                //    dstrTemp = FunStringH.funPoint(m_pEqpAct.funWordRead(strWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, EnuEQP.PLCRWType.Int_Data), pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format);
                                //    pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value = dstrTemp;
                                //}
                                //else
                                //{
                                //    dstrTemp = m_pEqpAct.funWordRead(strWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, EnuEQP.PLCRWType.ASCII_Data);
                                //    pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value = dstrTemp;
                                //}
                                //strWordAddress = CommonAct.FunTypeConversion.funAddressAdd(strWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length);
                                #endregion

                            }

                        }
                        #endregion

                        strWordAddress = CommonAct.FunTypeConversion.funAddressAdd(strWordAddress, dintReadcount);

                        dstrReadData = m_pEqpAct.funWordRead(strWordAddress, 80, EnuEQP.PLCRWType.ASCII_Data, false);
                        dintStartIndex = 0;
                        #region EQPPPID에 Film Code 저장
                        for (int dintLoop = 251; dintLoop <= 255; dintLoop++)
                        {
                            if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).AddPPIDBody(dintLoop) == true)
                            {
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).DESC = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).DESC;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Format = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Length = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).ModuleID = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).ModuleID;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Name = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Name;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Range = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Range;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Unit = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Unit;
                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).UseMode = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).UseMode;

                                dintLength = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length * 2;   //Length에 맞게 자를 문자열 개수를 가져온다.
                                dstrTemp = dstrReadData.Substring(dintStartIndex, dintLength);


                                pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value = dstrTemp.Trim();
                            }

                            dintStartIndex += dintLength;
                        }
                        #endregion
                    //}
                }
                #endregion

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                m_pEqpAct.subSetConfirmBit(strCompBit);

                switch (dintPPIDType)
                {
                    case 1:         //EQP PPID
                        {
                            pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, "3", "1", "", dstrEQPPPID);

                            ArrayList arrCon = new ArrayList();
                            InfoAct.clsHOSTPPID CurrentPPID;
                            foreach (string strPPID in pInfo.Unit(0).SubUnit(0).HOSTPPID())
                            {
                                CurrentPPID = pInfo.Unit(0).SubUnit(0).HOSTPPID(strPPID);
                                if (CurrentPPID.EQPPPID == dstrEQPPPID)
                                {
                                    arrCon.Add(strPPID);
                                }
                            }

                            string dstrSQL = "";
                            for (int dintLoop = 0; dintLoop < arrCon.Count; dintLoop++)
                            {
                                pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, "3", "2", arrCon[dintLoop].ToString(), dstrEQPPPID);

                                //pInfo.Unit(0).SubUnit(0).RemoveHOSTPPID(arrCon[dintLoop].ToString());

                                dstrSQL = string.Format("Update `tbHOSTPPID` set EQPPPID='{0}', DTime='{1}' Where HOSTPPID='{2}';", dstrEQPPPID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), arrCon[dintLoop].ToString());
                                DBAct.clsDBAct.funExecuteQuery(dstrSQL);
                            }
                        }
                        break;
                }

                //최종 수정된 날짜 Ini에 변경
                FunINIMethod.subINIWriteValue("ETCInfo", "RECIPELastModified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), pInfo.All.SystemINIFilePath);
                
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
