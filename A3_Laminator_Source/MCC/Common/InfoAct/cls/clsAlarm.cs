using System;
using System.Text;

namespace InfoAct
{
    public class clsAlarm
    {
        public int AlarmID = 0;                     //Alarm ID
        public int AlarmCode = 0;                   //Alarm Code
        public string AlarmType = "";               //Alarm Level(H:High, L:Low)
        public string AlarmDesc = "";               //Alarm의 Description
        public string AlarmOCCTime = "";            //Alarm 발생시간
        public string AlarmEventType = "";          //Alarm Event Type(S: Alarm Occur, R:Alarm Reset)
        public Boolean AlarmReport = true;          //설비에서 Alarm발생시 HOST로 보고할지 여부
        public int UnitID = 0;                      //해당 Module에 대한 UnitID
        public string ModuleID = "";                //ModuleID

        public int SETCODE = 0;                     //Alarm Set/Reset Code (0 = Alarm Reset(Alarm Cleared), 1 = Alarm Set (Alarm Occurred))

        //Constructor
        public clsAlarm(int intAlarmID)
        {
            this.AlarmID = intAlarmID;
        }
    }
}
