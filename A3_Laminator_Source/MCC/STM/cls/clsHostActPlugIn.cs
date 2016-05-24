using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace STM
{
    class clsHostActPlugIn
    {
        public InfoAct.clsInfo PInfo;
        private HostAct.clsHostAct pHostAct;                                   //외부 HostAct DLL 정의

        public clsHostActPlugIn() {}
        
        //*******************************************************************************
        //   Function Name : funOpenSecs()
        //   Description   : HostAct DLL의 인스턴스를 생성시키고 오픈한다.
        //   Parameters    : strEQPID => SECS에서 쓰이는 EQP ID
        //   Return Value  : 성공 => True, 실패 => False
        //   Special Notes : 
        //*******************************************************************************
        //   2006/09/22          어 경태          [L 00] 
        //*******************************************************************************
        public Boolean funOpenSecs(string strEqpId)
        {
            Boolean dbolReturn = false;

            try
            {
                this.pHostAct = new HostAct.clsHostAct();                          //HostAct 인스턴스 생성
                this.pHostAct.PInfo = clsConstant.gInfo;

                //Open을 시도한다.
                int dintReturn = this.pHostAct.funOpenSecsDrv(strEqpId);
                //리턴값이 1006이면 정상
                if (dintReturn == 1006)
                {
                    this.pHostAct.funThreadInitial();      //Secs Driver가 성공적으로 Open되면 HOST 자체 송신 Thread를 실행한다.
                    dbolReturn = true;
                }
                else
                {                                        //Open이 되지 않아 에러가 발생한 경우
                    dbolReturn = false;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

            return dbolReturn;
        }

        //*******************************************************************************
        //  Function Name : subClose()
        //  Description   : HostAct를 종료시킨다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2006/11/02          어 경태         [L 00] 
        //*******************************************************************************
        public void subClose()
        {
            try
            {
                this.pHostAct.funSecsDrvClose();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
