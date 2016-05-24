using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    public class clsSubUnit : clsSubUnitMethod
    {
        public int UnitID = 0;
        public string ModuleID = "";                //각 Unit의 ModuleID
        public string Index = "";
        public Boolean GLSExist = false;            //GLS 존재유무      

        public string EQPState = "1";               //EQP(Module) State(1: NORMAL, 2: FAULT, 3: PM)
        public string EQPStateOLD = "1";            //EQP(Module) Old State(1: NORMAL, 2: FAULT, 3: PM)

        public string EQPProcessState = "1";        //EQP(Module) Process State(1: IDLE, 2: SETUP, 3: EXECUTE, 4: PAUSE, 8: Resume)
        public string EQPProcessStateOLD = "1";     //EQP(Module) Old Process State(1: IDLE, 2: SETUP, 3: EXECUTE, 4: PAUSE, 8: Resume)

        public string EQPStateChangeBYWHO = "";             //Who Initiated the event(1: By Host, 2: By Operator, 3: By Equipment itself)
        public string EQPStateLastCommand = "";             //CIM이나 HOST로 부터 마지막으로 받은 명령

        public string EQPProcessStateChangeBYWHO = "";      //Who Initiated the event(1: By Host, 2: By Operator, 3: By Equipment itself)
        public string EQPProcessStateLastCommand = "";      //CIM이나 HOST로 부터 마지막으로 받은 명령(1: IDLE, 2: SETUP, 3: EXECUTE, 4: PAUSE, 8: Resume)

        public string HGLSID = "";

        //알람 누락 수정 - 12.07.26 KSH
        public int AlarmID = 0;                     //헤비 알람 발생된 ID 저장

        //Constructor
        public clsSubUnit(int intUnitID)
        {
            this.UnitID = intUnitID;
        }
    }
}
