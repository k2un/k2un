using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DisplayAct;
using InfoAct;

namespace STM
{
    class clsDisplayActPlugIn
    {
        public clsInfo PInfo = clsInfo.Instance;
        private frmMain pfrmMain;

        public clsDisplayActPlugIn() { }

        /// <summary>
        /// DisplayAct 초기화
        /// </summary>
        public void subInitial()
        {
            try 
            {           
                this.pfrmMain = new frmMain();
                pfrmMain.Show();
            }
            catch(Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subClose()
        {
            try
            {
                this.pfrmMain.subClose();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
