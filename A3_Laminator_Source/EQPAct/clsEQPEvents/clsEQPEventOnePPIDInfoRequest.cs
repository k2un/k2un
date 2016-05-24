using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventOnePPIDInfoRequest : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventOnePPIDInfoRequest(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actOnePPIDInfoRequest";
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
            string[] dstrValue;
            string dstrWordAddress = "W2540";
            StringBuilder dstrLog = new StringBuilder();
            string dstrHOSTPPID = "";
            string dstrEQPPPID = "";
            int dintPPIDType = 0;
            string dstrTemp = "";
            int dintLength = 0;
            int dintBodyIndex = 3;     //PPID Body Index
            string APCType = "";
            string strWordAddress = "";
            //[2015/04/20](Add by HS)
            string dstrReadData = "";
            int dintStartIndex = 0;
            string strValue = "";
            int dintReadcount = 0;

            try
            {
                //[2015/04/20]임시add by HS)
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("actOnePPIDInfoRequest Start. Time = {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                dstrEQPPPID = m_pEqpAct.funWordRead("W2540", 1, EnuEQP.PLCRWType.Int_Data);
                dintPPIDType = 1;
                //150429 고석현 수정
                //매번 읽으면 늦어지므로  EQPPPID가 존재하면 그대로 보고 EQPPPID가 
                //pInfo.Unit(0).SubUnit(0).RemoveEQPPPID(dstrEQPPPID);
                
                    dintReadcount = (pInfo.Unit(0).SubUnit(0).PPIDBodyCount - 5) * 2;
                    strWordAddress = "W254C";
                    if (pInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrEQPPPID) == true)
                    {
                        if (pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBodyCount == 0)
                        {
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
                        }
                    }

               

                ////dstrHOSTPPID = dstrValue[0];
                //dstrEQPPPID = dstrValue[0];
                //dintPPIDType = Convert.ToInt32(dstrValue[2]);

                ////로그를 남긴다.
                //switch (dintPPIDType)
                //{
                //    case 1:
                //        dstrTemp = "EQP PPID 정보 요청(One)";
                //        break;

                //    case 2:
                //        dstrTemp = "HOST PPID 정보 요청(One)";
                //        break;

                //    default:        //여기로 들어오면 Error임.
                //        dstrTemp = "PPID 정보 요청(One)";
                //        break;
                //}

                //dstrLog.Append(FunStringH.funPaddingStringData("-", 30, '-') + "\r\n");
                //dstrLog.Append(dstrTemp + "\r\n");
                //dstrLog.Append("HOSTPPID:" + dstrHOSTPPID + ",");
                //dstrLog.Append("EQPPPID:" + dstrEQPPPID + ",");
                //dstrLog.Append("PPIDType:" + dintPPIDType.ToString() + "\r\n");
                //for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                //{
                //    dstrTemp = FunStringH.funPoint(dstrValue[dintBodyIndex + dintLoop - 1], pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format);
                //    dstrLog.Append(pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Name + ":" + dstrTemp + "\r\n");
                //}
                //dstrLog.Append(FunStringH.funPaddingStringData("-", 30, '-'));
                //pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrLog.ToString());

                switch (dintPPIDType)
                {
                    case 1:         //EQP PPID

                        //기존에 EQPPPID가 존재하면 지운다.
                        //pInfo.Unit(0).SubUnit(0).RemoveEQPPPID(dstrEQPPPID);

                        //if (pInfo.Unit(0).SubUnit(0).AddEQPPPID(dstrEQPPPID) == true)
                        //{
                        //    pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).EQPPPID = dstrEQPPPID;

                        //    for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                        //    {
                        //        //EQPPPID에 Body 저장
                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).AddPPIDBody(dintLoop);

                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).DESC = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).DESC;
                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Format = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format;
                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Length = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length;
                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).ModuleID = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).ModuleID;
                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Name = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Name;
                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Range = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Range;
                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Unit = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Unit;
                        //        dstrTemp = FunStringH.funPoint(dstrValue[dintBodyIndex + dintLoop - 1], pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Format);
                        //        pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBody(dintLoop).Value = dstrTemp;
                        //    }
                        //}
                        //pInfo.Unit(0).SubUnit(0).EQPPPID(dstrEQPPPID).PPIDBodyCheck = true;

                        break;

                    case 2:         //HOST PPID

                        if (pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID) == null)
                        {
                            //기존에 HOSTPPID가 존재하지 않으면 새로 생성한다.
                            pInfo.Unit(0).SubUnit(0).AddHOSTPPID(dstrHOSTPPID);
                        }

                        pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).EQPPPID = dstrEQPPPID;
                        pInfo.Unit(0).SubUnit(0).HOSTPPID(dstrHOSTPPID).HostPPID = dstrHOSTPPID;


                        break;

                    default:
                        break;
                }

                //if (pInfo.All.APCStartEQPPPIDCheck)
                //{
                //    pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.APCStart, pInfo.All.DataCheckGLSID);
                //}
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                pInfo.All.isReceivedFromHOST = false;  //초기화
                pInfo.All.PLCActionEnd = true;             //PLC로 부터 값을 읽었음을 저장
                pInfo.All.PLCActionEnd2 = true;
                pInfo.All.APCPPIDReadFlag = true;
                //[2015/04/20]임시add by HS)
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("actOnePPIDInfoRequest End. Time = {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
            }
        }

        public void funProcessEQPStatus(string[] parameters)
        {

        }
        #endregion
    }
}
