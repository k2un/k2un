using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsINI 
    {
        //Unit
        public Boolean RecipeBody = false;
        public int UnitCount = 0;                          //Unit 총갯수

        //Robot
        public int RobotCount = 0;                         //Robot 총갯수

        //Port
        public int PortCount = 0;                          //Port의 총 갯수
        public int SlotCount = 0;                          //Slot의 총 갯수

        //PLC
        public string PLCIP = "";
        public string PLCPort = "";
        public Boolean DummyPLC = true;
        public string WordStart = "";
        public string WordEnd = "";
        public string BitScanCount = "";
        public string[] BitScanStart = new string[11];
        public string[] BitScanEnd = new string[11];
        public Boolean[] BitScanEnabled = new Boolean[11];

        //Host
        public string RemoteIP = "";
        public string RemotePort = "";
        public int T3 = 0;
        public int T5 = 0;
        public int T6 = 0;
        public int T7 = 0;
        public int T8 = 0;
        public int T9 = 0;   //CT ==> System.INI파일에 있음, 나머지 TimeOut은 모두 SECom.EXP파일에 있음.

        //Unit
        public string EQPID = "";
        public string SOFTREV = "";                             //Software Revision Code(S1F2의 송신 값)
        public string MDLN = "";                                //Equipment Model Type(S1F2의 송신 값)
        public string EQPType = "";                             //장비 Type(I Type(I), M Type(M), C Type(C), U Type(U))
        public int CurrentLOTIndex = 0;                         //현재까지 발번한 LOTIndex
    }
}
