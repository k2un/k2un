using System;
using System.Collections.Generic;
using System.Text;
using InfoAct;

/// <summary>
/// Base Class of Equipment Event Action Classes
/// </summary>

namespace EQPAct
{
    public class clsEQPEvent
    {
        #region Variables
        protected clsEQPAct m_pEqpAct = null;
        protected clsInfo pInfo = clsInfo.Instance;
        protected string strEventName = "";
        protected string m_strUnitID = "EQP";
        #endregion

        #region Properties
        public string StrEventName
        {
            get { return strEventName; }
        }
        #endregion

        #region Constructors
        public clsEQPEvent()
        {

        }
        #endregion

        #region Methods
        public void funSetEqpAct(EQPAct.clsEQPAct eqpAct)
        {
            m_pEqpAct = eqpAct;
        }

        protected void subMCCLogData(string strModuleID, string strLogType, string strStepID, string strGLSID, string strLotID, string strPPID, params string[] strDataValue)
        {
            string dstrMCCLogData = "";
            string dstrDataTime = "";

            try
            {
                dstrDataTime = DateTime.Now.ToString("MMdd_HHmm_ss.fff");
                dstrMCCLogData = dstrDataTime + "," + strModuleID + "," + strLogType + "," + strStepID + "," + strGLSID + "," + strLotID + "," + strPPID;

                if (strDataValue != null)
                {
                    switch (strLogType)
                    {
                        case "A":       //Action Log
                            //Action=FromPosition=ToPosition=Start_End, ex) Componet_In=LD01=UP01=End
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2] + "=" + strDataValue[3];
                            break;

                        case "I":       //Information Log
                            //Information=Value, ex) VaccumPressure=360.4
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1];
                            break;

                        case "E":
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0];
                            break;

                        case "W":
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0];
                            break;

                        case "S":
                            dstrMCCLogData = dstrMCCLogData + "," + strDataValue[0] + "=" + strDataValue[1] + "=" + strDataValue[2] + "=" + strDataValue[3];
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "LogType : " + strLogType + ", Step Definition or Information Data is Null !!");
                    return;
                }

                pInfo.subLog_Set(InfoAct.clsInfo.LogType.MCCLog, dstrMCCLogData);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion
    }
}
