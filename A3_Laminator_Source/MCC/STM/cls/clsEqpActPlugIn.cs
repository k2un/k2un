using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace STM
{
    class clsEqpActPlugIn
    {
        private string pAppPath = Application.StartupPath;

        private EQPAct.clsEQPAct pEQPAct;   //EQPAct DLL 정의
        private InfoAct.clsInfo pInfo = InfoAct.clsInfo.Instance;      //공용 구조체

        private enum CommunicationType
        {
            RS232 = 1,
            TCPIP = 2,
            NET10 = 3,
        }        

        public clsEqpActPlugIn() {}

        //*******************************************************************************
        //  Function Name : funOpenPLC()
        //  Description   : INI로부터 값을 읽어들여 각 PLC의 각 값을 설정하고 구조체에 저장후 PLC를 OPEN한다
        //  Parameters    : 
        //  Return Value  : 성공 => True, 실패 => False
        //  Special Notes : 
        //*******************************************************************************
        //  2006/09/27          어 경태         [L 00] 
        //*******************************************************************************
        public Boolean funOpenPLC()
        {
            Boolean bolOpenPort = false;

            try
            {
                this.pEQPAct = new EQPAct.clsEQPAct();                                  //EQPAct DLL 정의
                //this.pEQPAct.PInfo = clsConstant.gInfo;                                     //EQPACT.DLL에 구조체정보를 공유할수 있도록 넘긴다.
                //this.pInfo = clsConstant.gInfo;

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

                pEQPAct.PstrTCPIPAddress = "127.0.0.1";
                pEQPAct.PstrTCPIPPort = "2048";

                //Open을 시도한다.
                bolOpenPort = this.pEQPAct.funOpenPLC(CommonAct.EnuEQP.CommunicationType.NET10);

                //리턴값이 True 이면 성공
                if (bolOpenPort == true)
                {
                    this.pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "PLC Port Open Success");
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

        //*******************************************************************************
        //  Function Name : subClosePLC()
        //  Description   : EQPAct를 종료시킨다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/02          어 경태         [L 00] 
        //*******************************************************************************
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
