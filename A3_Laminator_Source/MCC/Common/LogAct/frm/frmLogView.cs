using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LogAct
{
    public partial class frmLogView : Form
    {
        public InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;                   //����ü
        private static string pstrPLCLogTime = "";          //PLCLOG Time�� �ӽ� ������ ���ε��� �α� �ð��� ���� ����
        private static string pstrCIMLogTime = "";          //CIMLOG Time�� �ӽ� ������ ���ε��� �α� �ð��� ���� ����
        private static string pstrPLCErrorLogTime = "";            //PLCErrorLOG Time�� �ӽ� ������ ���ε��� �α� �ð��� ���� ����

        public frmLogView()
        {
            InitializeComponent();
        }

        //*******************************************************************************
        //  Function Name : subFormLoad()
        //  Description   : 
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : 
        //*******************************************************************************
        //  2007/01/11          �� ��ȯ         [L 00] 
        //*******************************************************************************
        public void subFormLoad()
        {
            try
            {
                this.CenterToScreen();
                this.Show();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //*******************************************************************************
        //  Function Name : subLogFormWrite()
        //  Description   : CIM, PLC �α׸� ���� ����Ѵ�.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : PLC�α׿� PLCERROR�α״� �и����� �ʰ� ���� �����
        //*******************************************************************************
        //  2007/01/11          �� ��ȯ         [L 00] 
        //  2007/03/12          �� ����
        //*******************************************************************************
        public void subLogFormWrite(InfoAct.clsInfo.LogType dLogType, string strMessage)
        {
            string dstrTmp = "";
            string dstrLogTime = "";                        //�α� �ð��� �̾� ������ ����
            string[] dstrArrayLogData = { "", };            //�α� �ð��� ���� ��� "\n"���� Split�Ͽ� �ð��� ������ ���� �̾Ƴ� �迭.
            Point CursorPoint;

            try
            {
                
                switch (dLogType)
                {
                    case InfoAct.clsInfo.LogType.CIM:
                        //���ڷ� ���� strMessage���� �ð��κи� �߷���.
                        dstrLogTime = strMessage.Substring(12, 8);

                        //���� �α��� �ð��� ���� ���� �α��� �ð��� ���� ��� '\n'���� �����ؼ� �ð��� ������ ����Ÿ�� strMessage�� �ٽ� ����.
                        if (dstrLogTime.Equals(pstrCIMLogTime))
                        {
                            dstrArrayLogData = strMessage.Split('\n');
                            strMessage = dstrArrayLogData[1].ToString();

                        }
                        if (this.txtCIMLog.Text == "")
                        {
                            dstrTmp = this.txtCIMLog.Text + strMessage;
                        }
                        else
                        {
                            dstrTmp = this.txtCIMLog.Text + "\r\n" + strMessage;
                        }
                        if (dstrTmp.Length > 10000)
                            dstrTmp = dstrTmp.Substring(dstrTmp.Length - 10000);
                        this.txtCIMLog.Text = dstrTmp;       //PLC Log ���� ���
                        this.txtCIMLog.ScrollToCaret();
                        pstrCIMLogTime = dstrLogTime;
                        break;

                    case InfoAct.clsInfo.LogType.PLC:

                        //���ڷ� ���� strMessage���� �ð��κи� �߷���.
                        dstrLogTime = strMessage.Substring(12, 8);

                        //���� �α��� �ð��� ���� ���� �α��� �ð��� ���� ��� '\n'���� �����ؼ� �ð��� ������ ����Ÿ�� strMessage�� �ٽ� ����.
                        if (dstrLogTime.Equals(pstrPLCLogTime))
                        {
                            dstrArrayLogData = strMessage.Split('\n');
                            strMessage = dstrArrayLogData[1].ToString();

                        }
                        if (this.txtPLCLog.Text == "")
                        {
                            dstrTmp = this.txtPLCLog.Text + strMessage;
                        }
                        else
                        {
                            dstrTmp = this.txtPLCLog.Text + "\r\n" + strMessage;
                        }
                        
                        if (dstrTmp.Length > 10000)
                            dstrTmp = dstrTmp.Substring(dstrTmp.Length - 10000);
                        this.txtPLCLog.Text = dstrTmp;   //PLC Log ���� ���
                        this.txtPLCLog.ScrollToCaret();
                        if (this.Visible == true)
                        {
                            txtPLCLog.Focus();
                            SendKeys.Send("^{END}");    // Focus�� �� �Ʒ��ٷ� ������.
                            txtPLCLog.Focus();
                            
                             CursorPoint = this.txtPLCLog.AutoScrollOffset;
                            this.txtPLCLog.PointToClient(CursorPoint);
                        }
                        pstrPLCLogTime = dstrLogTime;
                        break;

                    case InfoAct.clsInfo.LogType.PLCError:
                        //���ڷ� ���� strMessage���� �ð��κи� �߷���.
                        dstrLogTime = strMessage.Substring(12, 8);

                        //���� �α��� �ð��� ���� ���� �α��� �ð��� ���� ��� '\n'���� �����ؼ� �ð��� ������ ����Ÿ�� strMessage�� �ٽ� ����.
                        if (dstrLogTime.Equals(pstrPLCErrorLogTime))
                        {
                            dstrArrayLogData = strMessage.Split('\n');
                            strMessage = dstrArrayLogData[1].ToString();

                        }

                        if (this.txtPLCErrLog.Text == "")
                        {
                            dstrTmp = this.txtPLCErrLog.Text + strMessage;
                        }
                        else
                        {
                            dstrTmp = this.txtPLCErrLog.Text + "\r\n" + strMessage;
                        }
                        if (dstrTmp.Length > 10000)
                            dstrTmp = dstrTmp.Substring(dstrTmp.Length - 10000);
                        this.txtPLCErrLog.Text = dstrTmp;            //PLC Error ���� ���
                        
                        pstrPLCErrorLogTime = dstrLogTime;             
                        break;
                      
                    default:
                        break;
                }

                //���� �α� �ð��� ���� �α� �ð��� ���ϱ� ���� �ӽ� ���庯��(static string dstrLogTimeTmp)�� �����س�.
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            
        }

        private void btnAllLogClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtCIMLog.Text = "";
                this.txtPLCLog.Text = "";
                this.txtPLCErrLog.Text = "";
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnLogClear_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.tabLogControl.SelectedIndex)
                {
                    case 0:
                        this.txtCIMLog.Text = "";   //CIM Log������ Clear
                        break;
                    case 1:
                        this.txtPLCLog.Text = "";   //PLC Log������ Clear
                        break;
                    case 2:
                        this.txtPLCErrLog.Text = "";//PLC Error Log������ Clear
                        break;
                    default:
                        break;
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
                this.PInfo.proLogActFormShowIndex = InfoAct.clsInfo.LogActFormShowType.None;
                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}