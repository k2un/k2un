using System;
using System.Text;

using System.Collections;

namespace InfoAct
{
    public class clsUnit : clsUnitMethod 
    {
        public int UnitID = 0;                      //UnitID

        
        //Alarm 시나리오 변경 후 두번째 알람을 저장할 변수 들
        public int subUnitID = 0;                   //해당 Module에 대한 UnitID
        public string ModuleID = "";                //ModuleID
        public int AlarmID = 0;                     //Alarm ID
        public int AlarmCode = 0;                   //Alarm Code
        public string AlarmType = "";               //Alarm Level(H:High, L:Low)
        public string AlarmDesc = "";               //Alarm의 Description
        public string AlarmOCCTime = "";            //Alarm 발생시간
        public string AlarmEventType = "";          //Alarm Event Type(S: Alarm Occur, R:Alarm Reset)
        public Boolean AlarmReport = true;          //설비에서 Alarm발생시 HOST로 보고할지 여부
        /// <summary>
        /// //발생한 Alarm을 연결해서 저장한다 (1,2,3,4,...)
        /// </summary>
        public string SetAlarmID = "";
        

        //Constructor
        public clsUnit(int intUnitID)
        {
            this.UnitID = intUnitID;
        }
    }
}
