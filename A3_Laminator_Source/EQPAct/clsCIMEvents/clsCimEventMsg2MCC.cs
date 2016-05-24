using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EQPAct
{
    public class clsCimEventMsg2MCC : clsCIMEvent, ICIMEvent
    {
        #region Variables

        private Win32 Win32API = new Win32();

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventMsg2MCC(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "Msg2MCC";
        }
        #endregion

        #region Methods
        /// <summary>
        /// CIM에서 설비로 발생하는 Event에 대한 처리
        /// </summary>
        /// <param name="parameters">Parameter Array</param>
        /// <remarks>
        /// parameters[0] : Message Type
        /// parameters[1] : Message String
        /// parameters[2] : 2nd parameter       ==> 
        /// parameters[3] : 3rd parameter       ==>
        /// parameters[4] : 4th parameter       ==>
        /// parameters[5] : 5th Parameter       ==>
        /// </remarks>
        public void funProcessCIMEvent(object[] parameters)
        {
            try
            {
                if (parameters.Length < 1 || parameters[0] == null) return;

                string dstrMsg = parameters[0].ToString();

                if (pInfo.All.MCCprocess != null && !pInfo.All.MCCprocess.HasExited)
                {
                    // 여서 보낸다.
                    if (pInfo.All.MCCprocess.StandardInput != null)
                    {
                        //System.Diagnostics.Debug.WriteLine(dstrMsg);
                        pInfo.All.MCCprocess.StandardInput.WriteLine(dstrMsg);
                    }
                }
                
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
            }
        }
        #endregion
    }
}
