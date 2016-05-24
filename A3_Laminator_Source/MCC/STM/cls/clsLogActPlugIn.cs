using System;
using System.Collections.Generic;
using System.Text;

namespace STM
{
    class clsLogActPlugIn
    {
        public InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;
        public LogAct.clsLogAct PLogAct;

        public clsLogActPlugIn() {}

        //*******************************************************************************
        //  Function Name : subInitialLog()
        //  Description   : LogAct를 초기화한다.
        //  Parameters    : None
        //  Return Value  : None
        //  Special Notes : None
        //*******************************************************************************
        //  2006/11/02          어 경태             [L 00]
        //*******************************************************************************
        public void subInitialLog()
        {
            try
            {
                this.PLogAct = new LogAct.clsLogAct();

                this.PLogAct.subInitialLog();

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "Program Start");
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subClose()
        {
            PLogAct.subClose();
        }
    }
}
