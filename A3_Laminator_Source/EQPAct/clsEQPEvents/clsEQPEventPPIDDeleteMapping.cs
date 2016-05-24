using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Collections;

namespace EQPAct
{
    public class clsEQPEventPPIDDeleteMapping : clsEQPEvent, IEQPEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsEQPEventPPIDDeleteMapping(string strUnitID)
        {
            m_strUnitID = strUnitID;
            strEventName = "actPPIDDeleteMapping";
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

            string[] dstrValue;
            int dintPPID = 0;
            ArrayList arrCon = new ArrayList();
            string dstrAddress = "";
            try
            {
                m_pEqpAct.subWordReadSave("W2040", 1, EnuEQP.PLCRWType.Int_Data);            //EQP PPID
                m_pEqpAct.subWordReadSave("", 10, EnuEQP.PLCRWType.Int_Data);           //Spare
                m_pEqpAct.subWordReadSave("", 1, EnuEQP.PLCRWType.Int_Data);            //PPID Type
                //for (int dintLoop = 1; dintLoop <= pInfo.Unit(0).SubUnit(0).PPIDBodyCount; dintLoop++)
                //{
                //    dintLength = pInfo.Unit(0).SubUnit(0).PPIDBody(dintLoop).Length;
                //    m_pEqpAct.subWordReadSave("", dintLength, EnuEQP.PLCRWType.Int_Data);   //Body
                //}

                dstrValue = m_pEqpAct.funWordReadAction(true);

                //PLC Req에 대한 CIM의 Confirm Bit를 준다.
                dintPPID =Convert.ToInt32(dstrValue[0]);

                foreach (string strEQPPPID in pInfo.Unit(0).SubUnit(0).MappingEQPPPID())
                {
                    InfoAct.clsMappingEQPPPID CurrentPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID);
                    if (CurrentPPID.LOW_EQPPPID == dstrValue[0] || CurrentPPID.UP_EQPPPID == dstrValue[0])
                    {
                        if (arrCon.Contains(CurrentPPID) == false)
                        {
                            arrCon.Add(CurrentPPID);
                        }
                    }
                }

                dstrAddress = "W1800";
                m_pEqpAct.funWordWrite(dstrAddress, arrCon.Count.ToString(), EnuEQP.PLCRWType.Int_Data);
                InfoAct.clsMappingEQPPPID CurrentMappingPPID;
                dstrAddress = FunTypeConversion.funAddressAdd(dstrAddress, 1);

                for (int dintLoop = 0; dintLoop < arrCon.Count; dintLoop++)
                {
                    CurrentMappingPPID = (InfoAct.clsMappingEQPPPID)arrCon[dintLoop];

                    m_pEqpAct.funWordWrite(dstrAddress, CurrentMappingPPID.EQPPPID.PadRight(16), EnuEQP.PLCRWType.ASCII_Data);
                    dstrAddress = FunTypeConversion.funAddressAdd(dstrAddress, 8);

                    m_pEqpAct.funWordWrite(dstrAddress, CurrentMappingPPID.UP_EQPPPID, EnuEQP.PLCRWType.Int_Data);
                    dstrAddress = FunTypeConversion.funAddressAdd(dstrAddress, 1);
                    m_pEqpAct.funWordWrite(dstrAddress, CurrentMappingPPID.LOW_EQPPPID, EnuEQP.PLCRWType.Int_Data);
                    dstrAddress = FunTypeConversion.funAddressAdd(dstrAddress, 1);
                }

                m_pEqpAct.subSetConfirmBit(strCompBit);

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
