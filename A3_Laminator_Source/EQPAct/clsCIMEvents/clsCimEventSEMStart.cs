using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;

namespace EQPAct
{
    public class clsCimEventSEMStart : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventSEMStart(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "SEMControllerStart";
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
                //if (pInfo.EQP("Main").RS232Connect == true)
                //{
                //    pInfo.All.spSerialPort.WriteLine("$START");
                //    pInfo.All.SEMStartReplyCheck = true;       //ACK Reply Check설정
                //    pInfo.All.SEMStartReplyCheckTime = DateTime.Now.Ticks;
                //}
                if (pInfo.All.USE_UDP)
                {
                    byte[] darrSendTemp = Encoding.ASCII.GetBytes("$START\r\n");

                    System.Diagnostics.Debug.WriteLine(string.Format("[{0}] Send $Start", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
                    pInfo.All.UDPSendPort.Send(darrSendTemp, darrSendTemp.Length, pInfo.All.UDPRemoteEndPoint);
                    pInfo.EQP("Main").UDPStart = true;
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
