﻿using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventCIMTest : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventCIMTest(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "CIMTest";
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
            string dstrHGLSID = "";
            string dstrWordAddress = "";
            string dstrBitAddress = "";
            bool dbolItemCheck = false;
            string strAPCEQPPPID = "";
            string dstrValue = "";
            int APCItemIndex = 0;
            string JobRecipe = "";
            try
            {
                dstrHGLSID = parameters[0].ToString();
                dstrWordAddress = "W1440";
                dstrBitAddress = "B1018";
                strAPCEQPPPID = pInfo.APC(dstrHGLSID).EQPPPID;

                //if (pInfo.All.GlassUpperJobFlag)
                //{
                //    JobRecipe = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(strAPCEQPPPID).UP_EQPPPID;
                //}
                //else
                //{
                //    JobRecipe = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(strAPCEQPPPID).LOW_EQPPPID;
                //}

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

                    JobRecipe += pEqpAct.funWordWriteString(2, dstrValue, EnuEQP.PLCRWType.Int_Data).Substring(4, 4) + pEqpAct.funWordWriteString(2, dstrValue, EnuEQP.PLCRWType.Int_Data).Substring(0, 4);
                }

                pEqpAct.funWordWrite(dstrWordAddress, JobRecipe, EnuEQP.PLCRWType.Hex_Data);        //변경할 ECID 데이터

                //pEqpAct.funBitWrite(dstrBitAddress, "1");

                //pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S16F111APCStart, dstrHGLSID);


            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
