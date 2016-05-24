using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.IO.Ports;
using System.Net.Sockets;
using System.Net;

namespace EQPAct
{
    public class clsCimEventUDPPortOpen : clsCIMEvent, ICIMEvent //20141106 이원규 (SEM_UDP)
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventUDPPortOpen(string strUnitID)
        {
            //strUnitID = strUnitID;
            strEventName = "UDPPortOpen";
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
                pInfo.All.UDPRemoteEndPoint = new IPEndPoint(this.pInfo.All.UDP_IP, this.pInfo.All.UDP_PORT);
                pInfo.All.UDPLocalEndPoint = new IPEndPoint(IPAddress.Parse("192.168.100.201"), this.pInfo.All.UDP_PORT);

                //this.PortClose();
                pInfo.All.UDPSendPort = new UdpClient();
                pInfo.All.UDPRecvPort = new UdpClient(pInfo.All.UDPLocalEndPoint);

                pInfo.EQP("Main").UDPConnect = true;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());

            }
        }
        #endregion
    }
}
