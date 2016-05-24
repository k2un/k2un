using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Collections;

namespace EQPAct
{
    public class clsCimEventScrap : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventScrap(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "Scrap";
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
            string dstrBitAddress = "B0020";
            string dstrWordAddress = "W0100";
            string dstrLOTID = "";
            string dstrGLSID = "";
            int dintUnitID = 0;

            try
            {
                dstrLOTID = parameters[0].ToString().Trim();
                dstrGLSID = parameters[1].ToString().Trim();
                dintUnitID = Convert.ToInt32(parameters[2].ToString());

                InfoAct.clsLOT CurrentLot = pInfo.LOTID(dstrLOTID);

                if(CurrentLot == null )
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Scrap Index Report Error!! Current Lot Error!! LOTID : {0}, GLSID : {1}, UnitNo : {2}", dstrLOTID, dstrGLSID, dintUnitID));
                    return;
                }

                InfoAct.clsGLS CurrentGLS = CurrentLot.GLSID(dstrGLSID);

                if(CurrentGLS == null)
                {
                     pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, string.Format("Scrap Index Report Error!! Current GLS Error!! LOTID : {0}, GLSID : {1}, UnitNo : {2}", dstrLOTID, dstrGLSID, dintUnitID));
                    return;
                }

                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 16 * (dintUnitID - 2));
                pEqpAct.funWordWrite(dstrWordAddress, pInfo.Port(CurrentLot.InPortID).HostReportPortID, EnuEQP.PLCRWType.ASCII_Data);
                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 2);
                pEqpAct.funWordWrite(dstrWordAddress, CurrentGLS.SlotID.ToString(), EnuEQP.PLCRWType.Int_Data);
                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 1);
                pEqpAct.funWordWrite(dstrWordAddress, dstrGLSID.PadRight(20), EnuEQP.PLCRWType.ASCII_Data);
                
                //LotStartFlag 처리
                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 10);
                //pEqpAct.funWordWrite(dstrWordAddress, , EnuEQP.PLCRWType.Int_Data);

                //LotEndFlag 처리
                dstrWordAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrWordAddress, 10);
                //pEqpAct.funWordWrite(dstrWordAddress, , EnuEQP.PLCRWType.Int_Data);

                dstrBitAddress = CommonAct.FunTypeConversion.funAddressAdd(dstrBitAddress, dintUnitID - 2);
                pEqpAct.funBitWrite(dstrBitAddress, "1");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}

