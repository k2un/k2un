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
    public partial class frmEQPState : Form
    {
        public clsInfo PInfo = clsInfo.Instance;

        public frmEQPState()
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

                dstrMsg = "frmEQPState(subFormLoad)";
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

                if (PInfo.EQP("Main").PLCConnect)
                {
                    string EQPState = PInfo.Unit(0).SubUnit(0).EQPState;

                    if (EQPState != null && EQPState != "")
                    {
                        if (EQPState == "1")
                        {
                            this.btnNORMAL.Enabled = false;
                            this.btnPM.Enabled = true;
                            this.btnNORMAL.BackColor = Color.WhiteSmoke;
                            this.btnNORMAL.ForeColor = Color.Black;
                        }
                        else if (EQPState == "3")
                        {
                            this.btnNORMAL.Enabled = true;
                            this.btnPM.Enabled = false;
                            this.btnPM.BackColor = Color.WhiteSmoke;
                            this.btnPM.ForeColor = Color.Black;
                        }
                    }
                }
                else
                {
                    btnNORMAL.Enabled = false;
                    btnPM.Enabled = false;

                    this.btnNORMAL.BackColor = Color.WhiteSmoke;
                    this.btnNORMAL.ForeColor = Color.Black;
                    this.btnPM.BackColor = Color.WhiteSmoke;
                    this.btnPM.ForeColor = Color.Black;
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
                this.btnNORMAL.Enabled = false;
                this.btnPM.Enabled = false;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }   

        }
        #endregion

        #region "Button Click"

        private void btnNORMAL_Click(object sender, EventArgs e)
        {
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.EQPState, "1");

                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

        }

        private void btnPM_Click(object sender, EventArgs e)
        {
            try
            {
                PInfo.subPLCCommand_Set(clsInfo.PLCCommand.EQPState, "3");

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



        #region "Mouse Button ����"


        private void btnOffline_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this.btnNORMAL.BackColor = Color.BlueViolet;
                this.btnNORMAL.ForeColor = Color.White;
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
                this.btnPM.BackColor = Color.BlueViolet;
                this.btnPM.ForeColor = Color.White;
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
                this.btnNORMAL.BackColor = Color.WhiteSmoke;
                this.btnNORMAL.ForeColor = Color.Black;
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
                this.btnPM.BackColor = Color.WhiteSmoke;
                this.btnPM.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion

       
    }
}