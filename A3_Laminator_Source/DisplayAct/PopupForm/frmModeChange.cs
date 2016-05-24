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
    public partial class frmModeChange : Form
    {
        public clsInfo PInfo = clsInfo.Instance;

        public frmModeChange()
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
                subOnlinePossibleCheck();       // Online 전환이 가능한 구간인지 체크

                this.PInfo.All.ModeChangeFormVisible = true;

                dstrMsg = "frmModeChange(subFormLoad)";
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
                if (PInfo.All.ControlState == "1")           // Offline Mode
                {
                    this.lblMessage.Text = "Now Is Offline Mode!";
                    this.btnOffline.Enabled = false;

                    if (PInfo.All.HostConnect == true)
                    {
                        this.btnOnlineLocal.Enabled = true;
                        this.btnOnlineRemote.Enabled = true;
                    }
                    else
                    {
                        this.btnOnlineLocal.Enabled = false;
                        this.btnOnlineRemote.Enabled = false;
                    }

                    this.btnOffline.BackColor = Color.WhiteSmoke;
                    this.btnOffline.ForeColor = Color.Black;
                }
                else if (PInfo.All.ControlState == "2")       // Online-Local Mode
                {
                    this.lblMessage.Text = "Now Is Online-Local Mode!";

                    this.btnOffline.Enabled = true;
                    this.btnOnlineLocal.Enabled = false;
                    this.btnOnlineRemote.Enabled = true;

                    this.btnOnlineLocal.BackColor = Color.WhiteSmoke;
                    this.btnOnlineLocal.ForeColor = Color.Black;
                }
                else if (PInfo.All.ControlState == "3")       // Online-Remote Mode
                {
                    this.lblMessage.Text = "Now Is Online-Remote Mode!";

                    this.btnOffline.Enabled = true;
                    this.btnOnlineLocal.Enabled = true;
                    this.btnOnlineRemote.Enabled = false;

                    this.btnOnlineRemote.BackColor = Color.WhiteSmoke;
                    this.btnOnlineRemote.ForeColor = Color.Black;
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

                //CIM<->장비간 Manual Mode이면 Online 전환을 못하게 한다.
                //if (PInfo.All.AutoMode == false)
                //{
                //    if (PInfo.All.ControlState == "1")
                //    {
                //        this.lblMessage.Text = "Now CIM<->EQP Manual Mode!";
                //        subButtonDisable();

                //        return;
                //    }
                //    else
                //    {
                //        ////CIM<->PLC간 Manual 모드인데 Online인 경우 Offline 버튼만 활성화한다.
                //        //this.lblMessage.Text = "Only Online -> Offline Enable!";
                //        //this.btnOffline.Enabled = true;
                //        //this.btnOnlineLocal.Enabled = false;
                //        //this.btnOnlineRemote.Enabled = false;

                //        //return;
                //    }
                //}
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
                this.btnOffline.Enabled = false;
                this.btnOnlineLocal.Enabled = false;
                this.btnOnlineRemote.Enabled = false;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }   

        }
        #endregion

        #region "Button Click"
        private void btnOffline_Click(object sender, EventArgs e)
        {
            string dstrMsg = "";

            try
            {
                subButtonDisable();
                this.lblMessage.Text = "Now Is Changing To Offline Mode.";

                this.PInfo.All.ControlstateChangeBYWHO = "2";    //By Operator

                this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;     //현재의 ControlState를 백업
                this.PInfo.All.WantControlState = "";
                this.PInfo.All.ControlState = "1";        // Offline 으로 변경일 경우엔 Host에 상관없이 Offline 변경을 한다.
                this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 71, 3, 0); //뒤에 0은 전체장비 

                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.PInfo.All.ControlState);

                //OFFLine 으로 전환되면 Trace를 초기화 한다. 20120507 어우수
                this.PInfo.Unit(0).SubUnit(0).RemoveTRID();

                dstrMsg = "frmModeChange(btnOffline_Click)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //로그출력

                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnOnlineLocal_Click(object sender, EventArgs e)
        {
            string dstrMsg = "";

            try
            {
                subButtonDisable();
                this.lblMessage.Text = "Now Is Changing To Online-Local Mode..";

                this.PInfo.All.ControlstateChangeBYWHO = "2";    //By Operator
                
                this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;     //현재의 ControlState를 백업
                this.PInfo.All.ControlState = "2";
                this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 112, 0);  //HOST로 보고, 뒤에 0은 전체장비 
                if (PInfo.All.ControlStateOLD == "0")
                {
                }
                this.PInfo.All.WantControlState = "";

                //Offline으로 전환 후 바로 폼을 받는다.
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.PInfo.All.ControlState); //PLC에게 알린다.
                this.Hide();

                dstrMsg = "frmModeChange(btnOnlineLocal_Click)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //로그출력
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnOnlineRemote_Click(object sender, EventArgs e)
        {
            string dstrMsg = "";

            try
            {
                subButtonDisable();
                this.lblMessage.Text = "Now Is Changing To Online-Remote Mode..";

                this.PInfo.All.ControlstateChangeBYWHO = "2";    //By Operator

                //PInfo.All.ControlStateOLD =this.PInfo.All.ControlState;     //현재의 ControlState를 백업
                this.PInfo.All.WantControlState = "3";

                if (PInfo.All.ControlState == "1")
                {
                    this.PInfo.All.ONLINEModeChange = true;
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);
                }
                else
                {
                    //Offline으로 전환 후 바로 폼을 받는다.
                    this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;     //현재의 ControlState를 백업
                    this.PInfo.All.ControlState = "3";
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 73, 3, 0);     //HOST로 보고, 뒤에 0은 전체장비 
                    this.PInfo.All.WantControlState = "";

                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.PInfo.All.ControlState); //PLC에게 알린다.

                    this.PInfo.All.ModeChangeFormVisible = false;
                    this.Hide();
                }

                dstrMsg = "frmModeChange(btnOnlineRemote_Click)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //로그출력
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

        private void btnOffline_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.btnOffline.BackColor = Color.BlueViolet;
                this.btnOffline.ForeColor = Color.White;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }      
        }

        private void btnOnlineLocal_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.btnOnlineLocal.BackColor = Color.BlueViolet;
                this.btnOnlineLocal.ForeColor = Color.White;
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
                this.btnOnlineRemote.BackColor = Color.BlueViolet;
                this.btnOnlineRemote.ForeColor = Color.White;
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
                this.btnOffline.BackColor = Color.WhiteSmoke;
                this.btnOffline.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnOnlineLocal_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                this.btnOnlineLocal.BackColor = Color.WhiteSmoke;
                this.btnOnlineLocal.ForeColor = Color.Black;
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
                this.btnOnlineRemote.BackColor = Color.WhiteSmoke;
                this.btnOnlineRemote.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        } 

        #region "Mouse Button 영역"

        #endregion

       
    }
}