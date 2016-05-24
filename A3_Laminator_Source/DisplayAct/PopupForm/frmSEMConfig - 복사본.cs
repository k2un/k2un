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
                subIntal();

                this.Show();
                this.Activate();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subIntal()
        {
            try
            {
                cboBaudRate.Text = PInfo.All.SEM_BaudRate;
                cboAlarmTime.Text = PInfo.All.SEM_ErrorDelayCheckTime.ToString();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSaveButtonCheck()
        {
            try
            {
                if (PInfo.EQP("Main").RS232Connect)
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
                if (PInfo.EQP("Main").RS232Connect == true)
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
                //COM Port 정보를 Combo등에 입력한다.
                this.cboPort.Items.Clear();

                foreach (string dstrCOMPort in SerialPort.GetPortNames())    //현재 컴퓨터의 포트이름을 가져온다.
                {
                    this.cboPort.Items.Add(dstrCOMPort);
                }

                if (PInfo.EQP("Main").RS232Connect)
                {
                    cboPort.Text = PInfo.All.spSerialPort.PortName;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            SerialPort spPort;
            string dstrBaudRate = "";
            string dstrComPort = "";

            try
            {
                this.btnSave.Enabled = false;
                if (this.rdoEnable.Checked == true)
                {
                    try
                    {
                        //포트 오픈 테스트를 한다.======================================================
                        dstrComPort = this.cboPort.Text.Trim();
                        spPort = new SerialPort(dstrComPort, 115200, Parity.Even, 8, StopBits.One);
                        spPort.Open();
                        if (spPort.IsOpen) spPort.Close();
                        //==============================================================================

                        this.PInfo.All.CommPort = this.cboPort.Text.Trim();
                        dstrBaudRate = this.cboBaudRate.Text.Trim();
                        this.PInfo.All.CommSetting = dstrBaudRate + ",e,8,1";
                        this.PInfo.All.SEMAlarmTime = Convert.ToInt32(this.cboAlarmTime.Text.Trim());

                        //포트연결시도를한다.
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SerialPortOpen);
                        //SEM Controller에 Start명령을 내린다.
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerStart);

                        FunINIMethod.subINIWriteValue("SEM", "SEM_Port", dstrComPort, this.PInfo.All.SystemINIFilePath);
                        FunINIMethod.subINIWriteValue("SEM", "SEM_BaudRate", dstrBaudRate, this.PInfo.All.SystemINIFilePath);
                        FunINIMethod.subINIWriteValue("SEM", "SEM_AlarmTime", this.PInfo.All.SEMAlarmTime.ToString(), this.PInfo.All.SystemINIFilePath);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        //사용할수 없는 포트
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                        this.btnSave.Enabled = true;
                    }
                }
                else
                {
                    //SEM Controller에 End명령을 내리고 포트를 닫는다.
                    this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerEnd);
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
                if (PInfo.EQP("Main").RS232Connect == true)
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
            if (PInfo.EQP("Main").RS232Connect == true)
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }
    }
}