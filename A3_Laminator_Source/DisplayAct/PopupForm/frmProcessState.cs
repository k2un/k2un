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
    public partial class frmProcessState : Form
    {
        public clsInfo PInfo = clsInfo.Instance;

        public frmProcessState()
        {
            InitializeComponent();
        }

        #region "Form Initial"
        public void subFormLoad()
        {
            string dstrMsg = "";

            try
            {
                subFormInitial();

                dstrMsg = "frmProcessState(subFormLoad)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //로그출력

                this.CenterToScreen();
                this.Show();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subFormInitial()
        {
            try
            {
                if (PInfo.EQP("Main").PLCConnect)
                {
                    if (PInfo.Unit(0).SubUnit(0).EQPProcessState != "4")
                    {
                        btnResume.Enabled = false;
                        btnPause.Enabled = true;

                        this.btnPause.BackColor = Color.WhiteSmoke;
                        this.btnPause.ForeColor = Color.Black;
                    }
                    else
                    {
                        btnPause.Enabled = false;
                        btnResume.Enabled = true;

                        this.btnResume.BackColor = Color.WhiteSmoke;
                        this.btnResume.ForeColor = Color.Black;
                    }
                }
                else
                {
                    btnPause.Enabled = false;
                    btnResume.Enabled = false;

                    this.btnPause.BackColor = Color.WhiteSmoke;
                    this.btnPause.ForeColor = Color.Black;
                    this.btnResume.BackColor = Color.WhiteSmoke;
                    this.btnResume.ForeColor = Color.Black;
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }   
        }

        public void subOnlinePossibleCheck()
        {
            try
            {
                //HOST와 연결이 끊어진 상태이면 모든 버튼 비활성화
                //조건 추가 : HOST와의 연결이 ONLINE상태에서 끊어지면 그 상태를 유지하고 있기 때문에
                //OFFLINE으로 전환해야 할 필요가 있다. 비활성화는 HOST와의 연결이 끊어지고, 끊어지기 전 상태가 OFFLINE인 경우에 한정한다.
                //20100412 어우수
                if (PInfo.All.HostConnect == false && this.PInfo.All.ControlStateOLD == "1")
                {
                    this.lblMessage.Text = "Now Host Disconnect State!";
                    subButtonDisable();

                    return;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }   
        }

        public void subButtonDisable()
        {
            try
            {
                this.btnPause.Enabled = false;
                this.btnResume.Enabled = false;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }   

        }
        #endregion

        #region "Button Click"

        private void btnPause_Click(object sender, EventArgs e)
        {

            try
            {
                this.PInfo.subPLCCommand_Set(clsInfo.PLCCommand.EQPProcessState, "4");

                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            try
            {
                this.PInfo.subPLCCommand_Set(clsInfo.PLCCommand.EQPProcessState, "8");

                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        } 
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.PInfo.All.ModeChangeFormVisible = false;
                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

        #region "Mouse Button 영역"
        private void btnOffline_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.btnPause.BackColor = Color.BlueViolet;
                this.btnPause.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }



        private void btnOnlineRemote_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.btnResume.BackColor = Color.BlueViolet;
                this.btnResume.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnOffline_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                this.btnPause.BackColor = Color.WhiteSmoke;
                this.btnPause.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnOnlineRemote_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                this.btnResume.BackColor = Color.WhiteSmoke;
                this.btnResume.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion

       
    }
}