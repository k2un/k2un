using System;
using System.Collections.Generic;
using System.Text;

namespace STM
{
    class clsNewDisplayActPlugIn
    {
        public InfoAct.clsInfo PInfo;

        public DisplayMainAct.frmMainDisplay_3th pfrmMain_3th;
        public DisplayMainAct.frmMainDisplay_4th pfrmMain;

        public void subInitialDisplay()
        {
            try
            {
                //3기는 폼이 다름(사이즈)
                if (this.PInfo.EQP("Main").EQPType == "3")
                {
                    this.pfrmMain_3th = new DisplayMainAct.frmMainDisplay_3th();
                    this.pfrmMain_3th.PInfo = clsConstant.gInfo;    //EQPACT.DLL에 구조체정보를 공유할수 있도록 넘긴다.

                    pfrmMain_3th.subformShow();
                }
                else
                {
                    this.pfrmMain = new DisplayMainAct.frmMainDisplay_4th();
                    this.pfrmMain.PInfo = clsConstant.gInfo;    //EQPACT.DLL에 구조체정보를 공유할수 있도록 넘긴다.

                    pfrmMain.subformShow();
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subClose()
        {
            try
            {
                this.pfrmMain.subFormClose();

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
