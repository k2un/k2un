using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using InfoAct;

namespace EQPAct
{
    public class clsCimEventECIDChange : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventECIDChange(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "ECIDChange";
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
            string dstrBitAddress = "B1020";
            string[] dstrValue;
            string dstrECID = FunStringH.funMakeLengthStringFirst("0", 48);
            int dintECID = 0;
            int dintIndex = 0;
            string strConvertData = "";
            try
            {
                if (pInfo.All.ECIDChange.Count > 0)
                {
                    for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).ECIDCount; dintLoop++)
                    {
                        dintIndex = pInfo.All.ECIDChange.IndexOfKey(dintLoop);         //Key 값을 가지고 SortedList의 Index값을 가져온다.

                        if (pInfo.All.ECIDChange.ContainsKey(dintLoop))
                        {
                            dintECID = Convert.ToInt32(pInfo.All.ECIDChange.GetKey(dintIndex));                            //Index값을 가지고 Key값을 알아온다,
                            dstrValue = pInfo.All.ECIDChange.GetByIndex(dintIndex).ToString().Split(new char[] { ',' });   //Index값을 가지고 value값을 알아온다.

                            //Word영역에 변경할 ECID를 Write한다.
                            dstrWordData += pEqpAct.funWordWriteString(1, dintECID.ToString(), EnuEQP.PLCRWType.Int_Data);
                            //dstrWordData += pEqpAct.funWordWriteString(2, dstrValue[0], EnuEQP.PLCRWType.Int_Data);
                            
                            if( !dstrValue[1].Contains("-"))
                            {
                                dstrWordData += pEqpAct.funWordWriteString(2, dstrValue[1], EnuEQP.PLCRWType.Int_Data);
                            }
                            else
                            {
                                strConvertData = pEqpAct.funWordWriteString(2, dstrValue[1], EnuEQP.PLCRWType.Int_Data);
                                dstrWordData += strConvertData.Substring(4, 4) + strConvertData.Substring(0, 4);
                            }

                            if (!dstrValue[2].Contains("-"))
                            {
                                dstrWordData += pEqpAct.funWordWriteString(2, dstrValue[2], EnuEQP.PLCRWType.Int_Data);
                            }
                            else
                            {
                                strConvertData = pEqpAct.funWordWriteString(2, dstrValue[2], EnuEQP.PLCRWType.Int_Data);
                                dstrWordData += strConvertData.Substring(4, 4) + strConvertData.Substring(0, 4);
                            }

                            if (!dstrValue[3].Contains("-"))
                            {
                                dstrWordData += pEqpAct.funWordWriteString(2, dstrValue[3], EnuEQP.PLCRWType.Int_Data);
                            }
                            else
                            {
                                strConvertData = pEqpAct.funWordWriteString(2, dstrValue[2], EnuEQP.PLCRWType.Int_Data);
                                dstrWordData += strConvertData.Substring(4, 4) + strConvertData.Substring(0, 4);
                            }

                            //dstrWordData += pEqpAct.funWordWriteString(2, dstrValue[4], EnuEQP.PLCRWType.Int_Data);
                        }
                        else
                        {
                            clsECID CurrentECID = pInfo.Unit(0).SubUnit(0).ECID(dintLoop);
                            dstrWordData += pEqpAct.funWordWriteString(1, CurrentECID.Index.ToString(), EnuEQP.PLCRWType.Int_Data);
                            //dstrWordData += pEqpAct.funWordWriteString(2, FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentECID.ECSLL, CurrentECID.Format)), EnuEQP.PLCRWType.Int_Data);
                            if (!CurrentECID.ECWLL.Contains("-"))
                            {
                                dstrWordData += pEqpAct.funWordWriteString(2, FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentECID.ECWLL, CurrentECID.Format)), EnuEQP.PLCRWType.Int_Data);
                            }
                            else
                            {
                                strConvertData = pEqpAct.funWordWriteString(2, FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentECID.ECWLL, CurrentECID.Format)), EnuEQP.PLCRWType.Int_Data);
                                dstrWordData += strConvertData.Substring(4, 4) + strConvertData.Substring(0, 4);
                            }

                            if (!CurrentECID.ECDEF.Contains("-"))
                            {
                                dstrWordData += pEqpAct.funWordWriteString(2, FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentECID.ECDEF, CurrentECID.Format)), EnuEQP.PLCRWType.Int_Data);
                            }
                            else
                            {
                                strConvertData = pEqpAct.funWordWriteString(2, FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentECID.ECDEF, CurrentECID.Format)), EnuEQP.PLCRWType.Int_Data);
                                dstrWordData += strConvertData.Substring(4, 4) + strConvertData.Substring(0, 4);
                            }

                            if (!CurrentECID.ECWUL.Contains("-"))
                            {
                                dstrWordData += pEqpAct.funWordWriteString(2, FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentECID.ECWUL, CurrentECID.Format)), EnuEQP.PLCRWType.Int_Data);
                            }
                            else
                            {
                                strConvertData = pEqpAct.funWordWriteString(2, FunStringH.funMakePLCData(FunStringH.funMakeRound(CurrentECID.ECWUL, CurrentECID.Format)), EnuEQP.PLCRWType.Int_Data);
                                dstrWordData += strConvertData.Substring(4, 4) + strConvertData.Substring(0, 4);
                            }
                        }
                    }

                    //변경할 ECID 값을 써준다.
                    dstrWordAddress = "W16C0";
                    pEqpAct.funWordWrite(dstrWordAddress, dstrWordData, EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터

                    ////ECID 변경 지시를 내린다.                                 
                    pEqpAct.funBitWrite(dstrBitAddress, "1");

                    //if (pInfo.EQP("Main").DummyPLC)
                    //{
                    //pEqpAct.funWordWrite("W2A00", dstrWordData, EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터
                    //pEqpAct.funBitWrite("B1121", "1");

                    //}


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
