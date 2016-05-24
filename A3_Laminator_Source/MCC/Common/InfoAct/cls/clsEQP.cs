using System;
using System.Text;
using System.Windows.Forms;

namespace InfoAct
{
    public class clsEQP
    {     
        //Unit
        public int UnitCount = 0;                           //Unit 총갯수

        //Slot
        public int SlotCount = 0;  //Slot의 총 갯수

        //PC
        public int T3 = 0;
        public int RetryCount = 0;
        public string LocalPort = "";
        public string LocalIP = "";
        public Boolean DummyPC = false;
        public Boolean MainEQPConnect = false;

        //PLC
        public Boolean PLCConnect = false;                  //PLC 연결 상태(True:Connection, False:DisConnection)
        public Boolean PLCStartConnect = false;             //최초 프로그램 실행 후 PLC와 연결되면 True, 최초 시작시는 False임
        public string PLCIP = "";
        public string PLCPort = "";
        public Boolean DummyPLC = true;
        public string WordStart = "";
        public string WordEnd = "";
        public string BitScanCount = "";
        public string[] BitScanStart = new string[11];
        public string[] BitScanEnd = new string[11];
        public Boolean[] BitScanEnabled = new Boolean[11];

        public Boolean RS232Connect = false;                  //RS232 연결 상태(True:Connection, False:DisConnection)

        public int ScanTime = 200;
        public int WorkingSizeMin = 1;
        public int WorkingSizeMax = 3;

        //Type
        public string Type = "";    //"PLC" or "PC"

        //EQP
        public string EQPID = "";
        //public string Layer1ModuleID = "";                      //Layer1의 ModuleID
        public string EQPType = "";                             //장비 Type(I Type(I), M Type(M), C Type(C), U Type(U))
        public string EQPName = "";
        public Boolean RecipeCheck = false;                     //Dummy로 테스트시 HOST로 부터 내려온 메세지에서 Recipe Check 할지 여부

        //Constructor
        public clsEQP(string strEQPID)
        {
            this.EQPID = strEQPID;
        }
    }
}
