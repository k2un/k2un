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
                subOnlinePossibleCheck();       // Online ��ȯ�� ������ �������� üũ

                this.PInfo.All.ModeChangeFormVisible = true;

                dstrMsg = "frmModeChange(subFormLoad)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //�α����

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
                //HOST�� ������ ������ �����̸� ��� ��ư ��Ȱ��ȭ
                //���� �߰� : HOST���� ������ ONLINE���¿��� �������� �� ���¸� �����ϰ� �ֱ� ������
                //OFFLINE���� ��ȯ�ؾ� �� �ʿ䰡 �ִ�. ��Ȱ��ȭ�� HOST���� ������ ��������, �������� �� ���°� OFFLINE�� ��쿡 �����Ѵ�.
                //20100412 ����
                if (PInfo.All.HostConnect == false && this.PInfo.All.ControlStateOLD == "1")
                {
                    this.lblMessage.Text = "Now Host Disconnect State!";
                    subButtonDisable();

                    return;
                }

                //CIM<->��� Manual Mode�̸� Online ��ȯ�� ���ϰ� �Ѵ�.
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
                //        ////CIM<->PLC�� Manual ����ε� Online�� ��� Offline ��ư�� Ȱ��ȭ�Ѵ�.
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

                this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;     //������ ControlState�� ���
                this.PInfo.All.WantControlState = "";
                this.PInfo.All.ControlState = "1";        // Offline ���� ������ ��쿣 Host�� ������� Offline ������ �Ѵ�.
                this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 71, 3, 0); //�ڿ� 0�� ��ü��� 

                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.PInfo.All.ControlState);

                //OFFLine ���� ��ȯ�Ǹ� Trace�� �ʱ�ȭ �Ѵ�. 20120507 ����
                this.PInfo.Unit(0).SubUnit(0).RemoveTRID();

                dstrMsg = "frmModeChange(btnOffline_Click)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //�α����

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
                
                this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;     //������ ControlState�� ���
                this.PInfo.All.ControlState = "2";
                this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 112, 0);  //HOST�� ����, �ڿ� 0�� ��ü��� 
                if (PInfo.All.ControlStateOLD == "0")
                {
                }
                this.PInfo.All.WantControlState = "";

                //Offline���� ��ȯ �� �ٷ� ���� �޴´�.
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.PInfo.All.ControlState); //PLC���� �˸���.
                this.Hide();

                dstrMsg = "frmModeChange(btnOnlineLocal_Click)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //�α����
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

                //PInfo.All.ControlStateOLD =this.PInfo.All.ControlState;     //������ ControlState�� ���
                this.PInfo.All.WantControlState = "3";

                if (PInfo.All.ControlState == "1")
                {
                    this.PInfo.All.ONLINEModeChange = true;
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S1F1AreYouThereRequest);
                }
                else
                {
                    //Offline���� ��ȯ �� �ٷ� ���� �޴´�.
                    this.PInfo.All.ControlStateOLD = this.PInfo.All.ControlState;     //������ ControlState�� ���
                    this.PInfo.All.ControlState = "3";
                    this.PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11RelatedEquipmentEvent, 73, 3, 0);     //HOST�� ����, �ڿ� 0�� ��ü��� 
                    this.PInfo.All.WantControlState = "";

                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.ControlState, this.PInfo.All.ControlState); //PLC���� �˸���.

                    this.PInfo.All.ModeChangeFormVisible = false;
                    this.Hide();
                }

                dstrMsg = "frmModeChange(btnOnlineRemote_Click)";
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, dstrMsg);     //�α����
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

        #region "Mouse Button ����"

        #endregion

       
    }
}