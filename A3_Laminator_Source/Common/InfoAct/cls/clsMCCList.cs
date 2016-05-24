using System;
using System.Collections.Generic;
using System.Text;

namespace InfoAct
{
    /// <summary>
    /// MCC Log Data List를 저장할 객체
    /// </summary>
    public class clsMCCList
    {
        /// <summary>
        /// MCC Index No
        /// </summary>
        private int Index = 0;

        /// <summary>
        /// MCC List Name
        /// </summary>
        public string MCCListName = "";

        /// <summary>
        /// MCC List Name 에 해당하는 값
        /// </summary>
        public string MCCListValue = "";

        public string MCCType = "";
        public string MCCLogItemName = "";
        public string MCCModuleID = "";
        public string MCCFromPosition = "";
        public string MCCToPosition = "";

        public string GLSID = "";
        public string LOTID = "";
        public string STEPID = "";
        public string PPID = "";

        //Information LOG DATA Read를 위하여 추가 20120607 어우수
        public string MCCStartInfoIndex = "";       //Action LOG의 Start시 읽어와야 하는 Information LOG의 Index번호
        public string MCCEndInfoIndex = "";         //Action LOG의 End시 읽어와야 하는 Information LOG의 Index 번호
        public string MCCInfoLogAddress = "";       //Information LOG의 DATA를 읽어와야 하는 주소

        public int MCCReadLength = 0;
        public string Description = "";
        public string Format = "";
        public Boolean HaveMinusValue = false;
        public string InOut = "";
        public string InfoType = "";

        public Boolean MCCLogStartEnd = false;         //Start : True,  End : False


        //Constructor
        public clsMCCList(int intIndex)
        {
            this.Index = intIndex;
        }
    }
}
