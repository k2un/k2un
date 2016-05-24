using System;
using System.Collections.Generic;
using System.Text;
using CommonAct;
using System.IO.Ports;

namespace EQPAct
{
    public class clsCimEventSerialPortOpen : clsCIMEvent, ICIMEvent
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public clsCimEventSerialPortOpen(string strUnitID)
        {
            strUnitID = strUnitID;
            strEventName = "SerialPortOpen";
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
            string dstrPort = string.Empty;             //Port이름
            int dintBaudRate = 0;                       //전송 속도
            Parity dParity = Parity.None;               //흐름제어
            int dintDataBit = 8;                        //송/수신 데이터 비트
            StopBits dStopBits = StopBits.One;          //Stop 비트

            string[] dstrVal = null;

            try
            {
                dstrVal = pInfo.All.CommSetting.Split(',');

                //Port이름
                dstrPort = pInfo.All.CommPort;

                //전송 속도
                dintBaudRate = Convert.ToInt32(dstrVal[0]);

                //흐름제어
                switch (dstrVal[1])
                {
                    case "n":
                        dParity = Parity.None;
                        break;

                    case "e":
                        dParity = Parity.Even;
                        break;
                    default:
                        break;
                }

                //Stop 비트
                dintDataBit = Convert.ToInt32(dstrVal[2]);

                //송/수신 데이터 비트
                switch (dstrVal[3])
                {
                    case "1":
                        dStopBits = StopBits.One;
                        break;
                    default:
                        break;
                }

                //현재 포트가 열여잇으면 우선 닫는다.
                if (pInfo.All.spSerialPort != null)
                {
                    if (pInfo.All.spSerialPort.IsOpen) pInfo.All.spSerialPort.Close();
                    pInfo.All.spSerialPort.Dispose();
                    pInfo.All.spSerialPort = null;
                }

                //시리얼포트 
                pInfo.All.spSerialPort = new SerialPort(dstrPort, dintBaudRate, dParity, dintDataBit, dStopBits);

                //12.04.02 ksh추가
                pInfo.All.spSerialPort.NewLine = "\r\n";
                pInfo.All.spSerialPort.ReadTimeout = 1000;

                try
                {
                    //시리얼 포트 연결
                    pInfo.All.spSerialPort.Open();
                }
                catch (Exception ex)
                {
                    //시리얼 포트 연결 상태를 가져온다.
                    pInfo.EQP("Main").RS232Connect = false;
                    pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                    return;
                }
                

                //시리얼 포트 연결 상태를 가져온다.
                pInfo.EQP("Main").RS232Connect = pInfo.All.spSerialPort.IsOpen;
                //SEM Controller에 Start명령을 내림
                //subSEMStart();

                if (pInfo.All.spSerialPort.IsOpen) pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "SerialPort : " + dstrPort + " OPEN");
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());

            }
        }
        #endregion
    }
}
