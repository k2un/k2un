using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using CommonAct;
using InfoAct;
using System.Net;
using System.Net.Sockets;

namespace DisplayAct
{
    public partial class frmSEMConfig : Form
    {
        public clsInfo PInfo = clsInfo.Instance;

        public frmSEMConfig()
        {
            InitializeComponent();
        }

        public void subFormLoad()
        {
            try
            {
                //BaudRate와 AlarmTime은 디자인에서 수정

                subSEMEnableDisable();          //SEM Trace Display
                subPortComboDisplay();          //Port ComboBox Display
                subSaveButtonCheck();

                this.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSaveButtonCheck()
        {
            try
            {
                if (PInfo.EQP("Main").UDPStart) //20141106 이원규 (SEM_UDP)
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSEMEnableDisable()
        {
            try
            {
                if (PInfo.EQP("Main").UDPStart == true) //20141106 이원규 (SEM_UDP)
                {
                    this.rdoEnable.Checked = true;
                    this.rdoDisable.Checked = false;
                }
                else
                {
                    this.rdoEnable.Checked = false;
                    this.rdoDisable.Checked = true;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subPortComboDisplay()
        {
            try
            {

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

        //20141106 이원규 (SEM_UDP)
        private void btnSave_Click(object sender, EventArgs e)
        {
            string dstrUDPIP = "";
            string dstrUDPPort = "";

            try
            {
                this.btnSave.Enabled = false;
                if (this.rdoEnable.Checked == true)
                {
                    try
                    {
                        if (PInfo.All.UDPRecvPort == null)
                        {
                            this.PInfo.All.UDP_IP = IPAddress.Parse(this.tbxIP.Text);
                            this.PInfo.All.UDP_PORT = Convert.ToInt32(this.tbxPort.Text);
                            this.PInfo.All.SEMAlarmTime = Convert.ToInt32(this.cboAlarmTime.Text.Trim());

                            dstrUDPIP = this.PInfo.All.UDP_IP.ToString();
                            dstrUDPPort = this.PInfo.All.UDP_PORT.ToString();

                            //포트연결시도를한다.
                            this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.UDPPortOpen);
                            //SEM Controller에 Start명령을 내린다.
                            this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerStart);

                            FunINIMethod.subINIWriteValue("SEM", "UDP_PORT", dstrUDPPort, this.PInfo.All.SystemINIFilePath);
                            FunINIMethod.subINIWriteValue("SEM", "UDP_IP", dstrUDPIP, this.PInfo.All.SystemINIFilePath);
                            FunINIMethod.subINIWriteValue("SEM", "SEM_AlarmTime", this.PInfo.All.SEMAlarmTime.ToString(), this.PInfo.All.SystemINIFilePath);
                        }
                        else
                        {
                            MessageBox.Show("UDP Port가 Open되어 있습니다.");
                        }
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        //사용할수 없는 포트
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                        this.btnSave.Enabled = true;
                        return;
                    }
                }
                else
                {
                    //SEM Controller에 End명령을 내리고 포트를 닫는다.
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerEnd);

                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.UDPPortClose);
                }

                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void rdoDisable_Click(object sender, EventArgs e)
        {
            try
            {
                if (PInfo.EQP("Main").UDPStart == true)
                {
                    if (MessageBox.Show("SEM Trace Disable시에는 필히 환경안전 부서의 사전승인 획득후 Disable 바랍니다.", "SEM Trace Enable or Disable",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.rdoDisable.Checked = true;
                    }
                    else
                    {
                        this.rdoEnable.Checked = true;
                    }

                    btnSave.Enabled = true;
                }
                else
                {
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void rdoEnable_Click(object sender, EventArgs e)
        {
            try
            {
                if (PInfo.EQP("Main").UDPStart == true)
                {
                    btnSave.Enabled = false;
                }
                else
                {
                    btnSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}