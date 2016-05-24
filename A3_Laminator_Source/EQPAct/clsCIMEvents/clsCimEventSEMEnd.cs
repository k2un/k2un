using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using InfoAct;

namespace EQPAct
{
    public class clsCimEventSEMEnd : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventSEMEnd(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "SEMControllerEnd";
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
                //if (pInfo.All.SEMControllerConnect == true)
                //{
                //    pInfo.All.spSerialPort.WriteLine("$END");
                //    //확인필요
                //    pInfo.subPLCCommand_Set(clsInfo.PLCCommand.SerialPortClose);
                //}
                //else
                //{
                //    //확인필요
                //    pInfo.subPLCCommand_Set(clsInfo.PLCCommand.SerialPortClose);
                //}

                if (pInfo.All.USE_UDP)
                {
                    byte[] darrSendTemp = Encoding.ASCII.GetBytes("$END\r\n");

                    pInfo.All.UDPSendPort.Send(darrSendTemp, darrSendTemp.Length, pInfo.All.UDPRemoteEndPoint);
                    pInfo.EQP("Main").UDPStart = false;
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
