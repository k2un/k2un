using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InfoAct;

namespace DisplayAct
{
    public partial class frmExit : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        private long plngCheckTime;     //프로그램 종료시 3초간 기다림
        int dintCnt = 0;
        public frmExit()
        {
            InitializeComponent();
        }

        public void subFormLoad()
        {
            string dstrMsg = "";

            try
            {
                plngCheckTime = 0;  //종료시간 체크 초기화

                dstrMsg = "frmExit(subFormLoad)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //로그출력

                this.CenterToScreen();
                this.Show();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnYES_Click(object sender, EventArgs e)
        {
            string dstrMsg = "";

            try
            {
                this.btnYES.Enabled = false;   //버튼을 2번이상 누르지 못하게 한다.
                this.lblTitle.Text = "Ending...Wait 3 sec";

                //프로그램 종료시 Offline(CEID=71)보고한다.
                //PInfo.All.ControlstateChangeBYWHO = "2";    //By Operator

                //PInfo.All.ControlStateOLD =this.PInfo.All.ControlState;     //현재의 ControlState를 백업
                //PInfo.All.WantControlState = "";
                //PInfo.All.ControlState = "1";        // Offline 으로 변경일 경우엔 Host에 상관없이 Offline 변경을 한다.
                //this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 71, 0); //뒤에 0은 전체장비 

                //this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState,this.PInfo.All.ControlState);

                //SuperVisor Logout
                //this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SupervisorLoginOut, false);

                dstrMsg = "frmExit(btnYES_Click)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //로그출력

                plngCheckTime = DateTime.Now.Ticks;
                this.tmrMain.Enabled = true;        //Timer를 기동시켜 3초간만 기다린다.
                dintCnt = 0;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnNO_Click(object sender, EventArgs e)
        {
            string dstrMsg = "";

            try
            {
                dstrMsg = "frmExit(btnNO_Click)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //로그출력

                this.Close();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            long dintSec = 0;
            
            try
            {   
                dintSec = DateTime.Now.Ticks - plngCheckTime;
                dintCnt++;
                if (dintSec < 0)
                {
                    plngCheckTime = 0;
                }
                else if (dintSec > 3 * 10000000)    //프로그램 종료전 3초간 기다린다.
                {
                    this.tmrMain.Enabled = false;   //Timer 중지
                    this.PInfo.All.ProgramEnd = true; //프로그램 종료
                    this.Close();
                }

                this.lblTitle.Text = string.Format("Ending...Wait {0} sec", (40 - dintCnt).ToString().Substring(0,1));

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                this.tmrMain.Enabled = false;   //Timer 중지
                this.PInfo.All.ProgramEnd = true; //프로그램 종료
                this.Close();
            }
        }
    }
}