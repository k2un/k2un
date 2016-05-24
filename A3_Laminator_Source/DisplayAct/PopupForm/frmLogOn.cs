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
    public partial class frmLogOn : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        //public frmVCRReadingMode pfrmVCRReadingMode;
        public string pstrVCRorSYstemSetup = "";

        public delegate void eventLogonDialog(object sender, EventArgs e);

        public event eventLogonDialog LogonSuccess;

        public frmLogOn()
        {
            InitializeComponent();
        }

        public void subFormLoad()
        {
            try
            {
                this.txtUserID.Text = "";
                this.txtPassword.Text = "";
                
                this.CenterToScreen();
                this.Show();

                this.txtUserID.Focus();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnLogOn_Click(object sender, EventArgs e)
        {
            string dstrUserID = "";
            string dstrUserPassword = "";

            try
            {
                dstrUserID = this.txtUserID.Text.Trim();
                dstrUserPassword = this.txtPassword.Text.Trim();

                if (this.PInfo.Unit(0).SubUnit(0).User(dstrUserID) != null)         //UserID가 틀린경우
                {
                    if (this.PInfo.Unit(0).SubUnit(0).User(dstrUserID).PassWord == dstrUserPassword)
                    {
                        //Login(CEID=161) Host로 보고
                        //this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11EquipmentSpecifiedControlEvent, 161, dstrUserID);

                        this.PInfo.All.UserLogInDuringTime = 0;       //로그인 지속시간 초기화
                        this.PInfo.All.UserID = dstrUserID;     //UserID 저장

                        if (this.pstrVCRorSYstemSetup == "SYSTEMSETUP")
                        {
                            //System Setup Window Load
                            //long longStartTick = DateTime.Now.Ticks;

                            //this.pfrmSystem = new frmSystem();
                            //this.pfrmSystem.PInfo = this.PInfo;
                            //this.pfrmSystem.subFormLoad(dstrUserID);

                            //long longEndTick = DateTime.Now.Ticks;

                            //long longTimeSpan = longEndTick - longStartTick;

                            //string strText = string.Format("frmSystemInitialTime : {0:0.000}", (double)longTimeSpan / 10000.0);
                            //this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, strText);
                            if (LogonSuccess != null) LogonSuccess(this, e);
                        }
                        else   //VCR로 로그인한 경우
                        {
                            //VCR Window Load
                            //this.pfrmVCRReadingMode = new frmVCRReadingMode();
                            //this.pfrmVCRReadingMode.PInfo = this.PInfo;
                            //this.pfrmVCRReadingMode.subFormLoad();
                        }

                        this.Hide();
                    }
                    else       //UserID는 맞는데 Password가 틀린경우
                    {
                        MessageBox.Show("Your UserID, Password Is Incorrect!.", "UserID, Password Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Your UserID, Password Is Incorrect!.", "UserID, Password Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((Keys)e.KeyChar == Keys.Enter)
                {
                    this.btnLogOn_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}