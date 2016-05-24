using System;
using System.Collections.Generic;
using System.Text;
using InfoAct;
using LogAct;

namespace STM
{
    class clsLogActPlugIn
    {
        public clsInfo PInfo = clsInfo.Instance;
        public clsLogAct PLogAct = clsLogAct.Instance;

        public clsLogActPlugIn() {}

        /// <summary>
        /// LogAct를 초기화한다.
        /// </summary>
        public void subInitialLog()
        {
            try
            {
                PLogAct.subInitialLog();

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Program Start");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// LogAct를 종료 시킨다.
        /// </summary>
        public void subClose()
        {
            try
            {
                PLogAct.subCloseLog();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
