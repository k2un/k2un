using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using InfoAct;

namespace STM
{
    class clsHostActPlugIn
    {
        public clsInfo PInfo = clsInfo.Instance;
        private HostAct.clsHostAct pHostAct;                                   //외부 HostAct DLL 정의

        public clsHostActPlugIn() {}
        
        /// <summary>
        /// HostAct DLL의 인스턴스를 생성시키고 오픈한다.
        /// </summary>
        /// <param name="strEqpId">SECS에서 쓰이는 EQP ID</param>
        /// <returns>성공 => True, 실패 => False</returns>
        public Boolean funOpenSecs(string strEqpId)
        {
            Boolean dbolReturn = false;

            try
            {
                this.pHostAct = new HostAct.clsHostAct();                          //HostAct 인스턴스 생성

                //Open을 시도한다.
                int dintReturn = this.pHostAct.funOpenSecsDrv(strEqpId);
                //리턴값이 1006이면 정상
                if (dintReturn == 0)
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

        /// <summary>
        /// HostAct를 종료시킨다.
        /// </summary>
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
