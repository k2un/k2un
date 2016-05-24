using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfoAct;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventProcessStartReqRPC : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventProcessStartReqRPC(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "RPCStart";
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
                string dstrStartBitAddress = "B1017";
                string dstrRPCWordAddress = "W100D";
                string dstrGlassID = parameters[0].ToString();

                //RPC Word 정보 Write
                InfoAct.clsRPC pRpcInfo = pInfo.RPC(dstrGlassID);
                string EQPPPID = pInfo.Unit(0).SubUnit(0).HOSTPPID(pRpcInfo.RPC_PPID).EQPPPID;

                //if (pInfo.All.GlassUpperJobFlag)
                //{
                //    EQPPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(EQPPPID).UP_EQPPPID;
                //}
                //else
                //{
                //    EQPPPID = pInfo.Unit(0).SubUnit(0).MappingEQPPPID(EQPPPID).LOW_EQPPPID;
                //}

                pEqpAct.funWordWrite("W100D", EQPPPID, CommonAct.EnuEQP.PLCRWType.Int_Data);

                pEqpAct.funBitWrite(dstrStartBitAddress, "1");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
