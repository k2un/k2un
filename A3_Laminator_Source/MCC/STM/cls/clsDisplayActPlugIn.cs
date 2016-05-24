using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace STM
{
    class clsDisplayActPlugIn
    {
        public InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;

        public clsDisplayActPlugIn() { }

        //*******************************************************************************
        //  Function Name : subInitial()
        //  Description   : DisplayAct 초기화
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/10/31          어 경태         [L 00] 
        //*******************************************************************************
        public void subInitial()
        {
            try 
            {           
                //this.pfrmMain = new DisplayAct.frmMain();
                //this.pfrmMain.PInfo = clsConstant.gInfo;    //EQPACT.DLL에 구조체정보를 공유할수 있도록 넘긴다.

                //pfrmMain.Show();     
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
                //this.pfrmMain.subClose();
                
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
