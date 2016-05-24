using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    /// <summary>
    /// MCC Log Data List를 저장할 객체
    /// </summary>
    /// <remarks>
    /// 20101001            이상호          [L 00]
    /// </remarks>
    public class clsMCC
    {
        public int Index = 0;                       //Index No
        public string MCCType = "";                 //Type : A(Step Definition), I(Information)
        public string MCCName = "";                 //List Name
        public string ModuleID = "";                //ModuleID
        public string FromPosition = "";            //FromPosition
        public string ToPosition = "";              //ToPosition
        public string MCCDesc = "";                 //MCC ITEM Name의 Description
        public string MCCValue = "";                //Information Value
        public Boolean MCCOnlyItem = false;
        public Boolean MCCLogWrite = false;         //MCC Log를 기록했는지에 대한 여부
        public Boolean MCCLogStartEnd = false;         //Start : True,  End : False
        public string Unit = "";                    //단위

        public bool PLCReadFlag = false;
        public int SVIDIndex = 0;
        public int GroupNo = 0;

        //Constructor
        public clsMCC(int intIndex)
        {
            this.Index = intIndex;
        }
    }
}
