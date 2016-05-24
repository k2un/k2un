using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventFilmCommand : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventFilmCommand(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "FilmJobCommand";
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
        /// (string strHOSTPPID, string strEQPPPID, string strTime, string strPPIDRev, string dstrPPIDType)
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            string strEQPPPID = parameters[0].ToString();

            try
            {
                int dintCEID = Convert.ToInt32(parameters[0]);
                int dintPortID = Convert.ToInt32(parameters[1]);
                int dintUnitID = 0;
                int dintSubUnitID = 0;

                switch (dintPortID)
                {
                    case 1:
                        dintUnitID = 1;
                        dintSubUnitID = 1;
                        break;
                    case 2:
                        dintUnitID = 1;
                        dintSubUnitID = 2;
                        break;
                    case 3:
                        dintUnitID = 1;
                        dintSubUnitID = 3;
                        break;
                    case 4:
                        dintUnitID = 1;
                        dintSubUnitID = 4;
                        break;
                    case 5:
                        dintUnitID = 2;
                        dintSubUnitID = 1;
                        break;
                    case 6:
                        dintUnitID = 2;
                        dintSubUnitID = 2;
                        break;
                }

                if (dintCEID == 1001)
                {
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedJobProcess, dintCEID, dintPortID, dintUnitID, dintSubUnitID, pInfo.Port(dintPortID).CSTID, pInfo.Port(dintPortID).SlotCount);
                }
                else
                {
                    pInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedJobProcess, dintCEID, dintPortID, dintUnitID, dintSubUnitID);
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
