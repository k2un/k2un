using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAct
{
    public class ConPLCCodeNET10
    {
        //PLC 디바이스 코드 (어드레스 영역별)
        public string DeviceB = "23";               //B 영역의 디바이스 코드
        public string DeviceW = "24";               //W 영역의 디바이스 코드
        public string DeviceR = "2200";             //R 영역의 디바이스 코드

        public string DeviceD = "13";               //ZR 영역의 디바이스 코드
        public string DeviceZ = "20";               //ZR 영역의 디바이스 코드
        public string DeviceM = "4";                //ZR 영역의 디바이스 코드
        public string DeviceTN = "11";              //ZR 영역의 디바이스 코드

        //디바이스 별 스테이션 넘버
        public string Network = "111";              //NetWork Number
        public string StationNo = "255";

        public string BStationNo = "255";           //Upper(network number) + Lower(B영역의 Station Number)
        public string WStationNo = "255";           //Upper(network number) + Lower(W영역의 Station Number)
        public string RStationNo = "255";           //Upper(network number) + Lower(R영역의 Station Number)

        public string DStationNo = "255";           //Upper(network number) + Lower(ZR영역의 Station Number)
        public string ZStationNo = "255";           //Upper(network number) + Lower(ZR영역의 Station Number)
        public string MStationNo = "255";           //Upper(network number) + Lower(ZR영역의 Station Number)
        public string TNStationNo = "255";          //Upper(network number) + Lower(ZR영역의 Station Number)
    }
}
