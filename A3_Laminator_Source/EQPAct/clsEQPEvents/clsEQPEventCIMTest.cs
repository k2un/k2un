using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsEQPEventCIMTest : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventCIMTest(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "CIMTEST";
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
            string dstrHGLSID = "";
            string dstrWordAddress = "W1440";
            string dstrBitAddress = "";
            bool dbolItemCheck = false;
            string strAPCEQPPPID = "";
            string dstrValue = "";
            int APCItemIndex = 0;
            string JobRecipe = "";
            string strWordWriteData = "";

            try
            {
                #region 임시

                dstrHGLSID = "TEST";
                dstrWordAddress = "W1440";
                dstrBitAddress = "B1018";

                for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).EQPPPID(strAPCEQPPPID).PPIDBodyCount; dintLoop++)
                {
                    dbolItemCheck = false;
                    for (int dintLoop2 = 0; dintLoop2 < pInfo.APC(dstrHGLSID).ParameterName.Length; dintLoop2++)
                    {
                        if (pInfo.APC(dstrHGLSID).ParameterName[dintLoop2] == pInfo.Unit(0).SubUnit(0).EQPPPID(strAPCEQPPPID).PPIDBody(dintLoop).Name)
                        {
                            APCItemIndex = dintLoop2;
                            dbolItemCheck = true;
                            break;
                        }
                    }

                    if (dbolItemCheck)
                    {
                        dstrValue = Convert.ToInt32(FunStringH.funMakePLCData(pInfo.APC(dstrHGLSID).ParameterValue[APCItemIndex].ToString())).ToString();
                    }
                    else
                    {
                        dstrValue = FunStringH.funMakePLCData(pInfo.Unit(0).SubUnit(0).EQPPPID(strAPCEQPPPID).PPIDBody(dintLoop).Value);
                    }

                    //if (pInfo.Unit(0).SubUnit(0).EQPPPID(strAPCEQPPPID).PPIDBody(dintLoop).Length > 1)
                    //{
                    //    pEqpAct.funDoubleWordWrite(dstrWordAddress, dstrValue, EnuEQP.PLCRWType.Int_Data);
                    //}
                    //else
                    //{
                    //    pEqpAct.funWordWrite(dstrWordAddress, dstrValue, EnuEQP.PLCRWType.Int_Data);
                    //}

                    //dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length);
                    if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length == 2)
                    {
                        if (dstrValue.Contains("-"))
                        {
                            strWordWriteData = m_pEqpAct.funWordWriteString(pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, dstrValue.ToString(), EnuEQP.PLCRWType.Int_Data);
                            //m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData, EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터

                        }
                        else
                        {
                            strWordWriteData = m_pEqpAct.funWordWriteString(pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, dstrValue.ToString(), EnuEQP.PLCRWType.Int_Data).Substring(4, 4) + m_pEqpAct.funWordWriteString(2, dstrValue.ToString(), EnuEQP.PLCRWType.Int_Data).Substring(0, 4);
                        }

                        m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData.Substring(4, 4), EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터
                        dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, 1);
                        m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData.Substring(0, 4), EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터
                        //m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData, EnuEQP.PLCRWType.Hex_Data);
                        dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length - 1);
                    }
                    else
                    {
                        strWordWriteData = m_pEqpAct.funWordWriteString(pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, dstrValue, EnuEQP.PLCRWType.ASCII_Data);
                        dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length);

                    }
                }


                //pEqpAct.funBitWrite(dstrBitAddress, "1");

                //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F111APCStart, dstrHGLSID);

                #endregion 

                //int dintTest = 124;
                //for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                //{
                //    if (pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length == 2)
                //    {
                //        dintTest -= 1;

                //        if (dintTest < 0)
                //        {
                //            strWordWriteData = m_pEqpAct.funWordWriteString(pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, dintTest.ToString(), EnuEQP.PLCRWType.Int_Data);
                //            //m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData, EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터

                //        }
                //        else
                //        {
                //            strWordWriteData = m_pEqpAct.funWordWriteString(pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, dintTest.ToString(), EnuEQP.PLCRWType.Int_Data).Substring(4, 4) + m_pEqpAct.funWordWriteString(2, dintTest.ToString(), EnuEQP.PLCRWType.Int_Data).Substring(0, 4);
                //        }

                //        m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData.Substring(4, 4), EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터
                //        dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, 1);
                //        m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData.Substring(0, 4), EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터
                //        //m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData, EnuEQP.PLCRWType.Hex_Data);
                //        dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length - 1);
                //    }
                //    else
                //    {
                //        strWordWriteData = m_pEqpAct.funWordWriteString(pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length, dintTest.ToString(), EnuEQP.PLCRWType.ASCII_Data);
                //        dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length);

                //    }

                //    //m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData.Substring(4, 4), EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터
                //    //dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, 1);

                //    //m_pEqpAct.funWordWrite(dstrWordAddress, strWordWriteData.Substring(0, 4), EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터
                //    //dstrWordAddress = FunTypeConversion.funAddressAdd(dstrWordAddress, pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length - 1);
                //}

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
