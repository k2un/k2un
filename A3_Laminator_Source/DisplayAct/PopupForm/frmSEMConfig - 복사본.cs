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
                //BaudRate�� AlarmTime�� �����ο��� ����

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
                //COM Port ������ Combo� �Է��Ѵ�.
                this.cboPort.Items.Clear();

                foreach (string dstrCOMPort in SerialPort.GetPortNames())    //���� ��ǻ���� ��Ʈ�̸��� �����´�.
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
                        //��Ʈ ���� �׽�Ʈ�� �Ѵ�.======================================================
                        dstrComPort = this.cboPort.Text.Trim();
                        spPort = new SerialPort(dstrComPort, 115200, Parity.Even, 8, StopBits.One);
                        spPort.Open();
                        if (spPort.IsOpen) spPort.Close();
                        //==============================================================================

                        this.PInfo.All.CommPort = this.cboPort.Text.Trim();
                        dstrBaudRate = this.cboBaudRate.Text.Trim();
                        this.PInfo.All.CommSetting = dstrBaudRate + ",e,8,1";
                        this.PInfo.All.SEMAlarmTime = Convert.ToInt32(this.cboAlarmTime.Text.Trim());

                        //��Ʈ����õ����Ѵ�.
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SerialPortOpen);
                        //SEM Controller�� Start����� ������.
                        this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.SEMControllerStart);

                        FunINIMethod.subINIWriteValue("SEM", "SEM_Port", dstrComPort, this.PInfo.All.SystemINIFilePath);
                        FunINIMethod.subINIWriteValue("SEM", "SEM_BaudRate", dstrBaudRate, this.PInfo.All.SystemINIFilePath);
                        FunINIMethod.subINIWriteValue("SEM", "SEM_AlarmTime", this.PInfo.All.SEMAlarmTime.ToString(), this.PInfo.All.SystemINIFilePath);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        //����Ҽ� ���� ��Ʈ
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                        this.btnSave.Enabled = true;
                    }
                }
                else
                {
                    //SEM Controller�� End����� ������ ��Ʈ�� �ݴ´�.
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
                    if (MessageBox.Show("SEM Trace Disable�ÿ��� ���� ȯ����� �μ��� �������� ȹ���� Disable �ٶ��ϴ�.", "SEM Trace Enable or Disable",
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