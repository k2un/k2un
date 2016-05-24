using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using InfoAct;
using System.Diagnostics;
using System.IO;

namespace STM
{
    class clsEqpActPlugIn
    {
        private string pAppPath = Application.StartupPath;

        public EQPAct.clsEQPAct pEQPAct;   //EQPAct DLL 정의
        private clsInfo pInfo = clsInfo.Instance;      //공용 구조체

        private enum CommunicationType
        {
            RS232 = 1,
            TCPIP = 2,
            NET10 = 3,
        }        

        public clsEqpActPlugIn() {}

        /// <summary>
        /// INI로부터 값을 읽어들여 각 PLC의 각 값을 설정하고 구조체에 저장후 PLC를 OPEN한다
        /// </summary>
        /// <returns>성공 => True, 실패 => False</returns>
        public Boolean funOpenPLC()
        {
            Boolean bolOpenPort = false;

            try
            {
                this.pEQPAct = new EQPAct.clsEQPAct();                                  //EQPAct DLL 정의

                for (int dintLoop = 1; dintLoop <= 10; dintLoop++)
                {
                    pEQPAct.PstrBitScanStart[dintLoop] = pInfo.EQP("Main").BitScanStart[dintLoop];
                    pEQPAct.PstrBitScanEnd[dintLoop] = pInfo.EQP("Main").BitScanEnd[dintLoop];
                    pEQPAct.PbolBitScanEnabled[dintLoop] = pInfo.EQP("Main").BitScanEnabled[dintLoop];
                }

                pEQPAct.PstrWAreaStart = pInfo.EQP("Main").WordStart;
                pEQPAct.PstrWAreaEnd = pInfo.EQP("Main").WordEnd;

                pEQPAct.PbolDummyPLC = pInfo.EQP("Main").DummyPLC;

                pEQPAct.PstrAddressPath = pAppPath + @"\system";

                pEQPAct.PintScanTime = this.pInfo.EQP("Main").ScanTime;                     //어경태 20071119
                pEQPAct.PintWorkingSizeMin = this.pInfo.EQP("Main").WorkingSizeMin;
                pEQPAct.PintWorkingSizeMax = this.pInfo.EQP("Main").WorkingSizeMax;

                pEQPAct.PstrTCPIPAddress = "192.168.1.88";
                pEQPAct.PstrTCPIPPort = "2048";

                //Open을 시도한다.
                bolOpenPort = this.pEQPAct.funOpenPLC(CommonAct.EnuEQP.CommunicationType.NET10);

                //리턴값이 True 이면 성공
                if (bolOpenPort == true)
                {
                    this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PLC Port Open Success");
                    this.pInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.MapInitial);
                    //this.pInfo.subPLCCommand_Set(clsInfo.PLCCommand.ECIDRead);
                }
                else
                {                                        //Open이 되지 않아 에러가 발생한 경우
                    this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PLC Port Open Fail");
                }
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return bolOpenPort;
        }

        /// <summary>
        /// EQPAct를 종료시킨다.
        /// </summary>
        public void subClosePLC()
        {
            try
            {
                pEQPAct.funClosePLC();
            }
            catch (Exception ex)
            {
                this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

    }
}
