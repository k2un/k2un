using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.IO.Ports;

namespace EQPAct
{
    public class clsCimEventSerialPortClose : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventSerialPortClose(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "SerialPortClose";
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
            string dstrComportName = "";
            try
            {
               pInfo.All.spSerialPort.Close();                             //포트를 닫는다
               dstrComportName = pInfo.All.spSerialPort.PortName;
                //시리얼 포트 연결 상태를 가져온다.
               pInfo.EQP("Main").RS232Connect = pInfo.All.spSerialPort.IsOpen;

               if (pInfo.All.spSerialPort != null)
                {
                    pInfo.All.spSerialPort.Dispose();
                    pInfo.All.spSerialPort = null;
                }

               pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SerialPort : " + dstrComportName + " CLOSE");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());

            }
        }
        #endregion
    }
}
