using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EQPAct
{
    public class clsCimEventProcessStartReqNormal : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventProcessStartReqNormal(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "NormalStart";
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
                string dstrStartBitAddress = "B1016";
                string EQPPPID = pInfo.Unit(0).SubUnit(0).HOSTPPID(pInfo.All.HostPPID).EQPPPID;

                //if(pInfo.All.GlassUpperJobFlag)
                //{
                //    EQPPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(EQPPPID).UP_EQPPPID;
                //}
                //else
                //{
                //    EQPPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(EQPPPID).LOW_EQPPPID;
                //}

                pEqpAct.funWordWrite("W100D", EQPPPID, CommonAct.EnuEQP.PLCRWType.Int_Data);

                pEqpAct.funBitWrite(dstrStartBitAddress, "1");
                //[2015/03/27] Normal작업지시 Bit가 꺼지지 않는 현상으로 인하여..(Add by HS)
                pInfo.All.B1016Flag = true;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
