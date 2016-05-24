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
        public InfoAct.clsInfo PInfo = InfoAct.clsInfo.Instance;                   //구조체
        private static string pstrPLCLogTime = "";          //PLCLOG Time을 임시 저장해 새로들어온 로그 시간과 비교할 변수
        private static string pstrCIMLogTime = "";          //CIMLOG Time을 임시 저장해 새로들어온 로그 시간과 비교할 변수
        private static string pstrPLCErrorLogTime = "";            //PLCErrorLOG Time을 임시 저장해 새로들어온 로그 시간과 비교할 변수

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
        //  2007/01/11          구 정환         [L 00] 
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
        //  Description   : CIM, PLC 로그를 폼에 출력한다.
        //  Parameters    : 
        //  Return Value  : 
        //  Special Notes : PLC로그와 PLCERROR로그는 분리하지 않고 같이 출력함
        //*******************************************************************************
        //  2007/01/11          구 정환         [L 00] 
        //  2007/03/12          박 근태
        //*******************************************************************************
        public void subLogFormWrite(InfoAct.clsInfo.LogType dLogType, string strMessage)
        {
            string dstrTmp = "";
            string dstrLogTime = "";                        //로그 시간을 뽑아 저장할 변수
            string[] dstrArrayLogData = { "", };            //로그 시간이 같을 경우 "\n"으로 Split하여 시간을 제외한 값만 뽑아낼 배열.
            Point CursorPoint;

            try
            {
                
                switch (dLogType)
                {
                    case InfoAct.clsInfo.LogType.CIM:
                        //인자로 들어온 strMessage에서 시간부분만 추려냄.
                        dstrLogTime = strMessage.Substring(12, 8);

                        //이전 로그의 시간과 현재 들어온 로그의 시간이 같을 경우 '\n'으로 구분해서 시간을 제외한 데이타만 strMessage에 다시 넣음.
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
                        this.txtCIMLog.Text = dstrTmp;       //PLC Log 내용 출력
                        this.txtCIMLog.ScrollToCaret();
                        pstrCIMLogTime = dstrLogTime;
                        break;

                    case InfoAct.clsInfo.LogType.PLC:

                        //인자로 들어온 strMessage에서 시간부분만 추려냄.
                        dstrLogTime = strMessage.Substring(12, 8);

                        //이전 로그의 시간과 현재 들어온 로그의 시간이 같을 경우 '\n'으로 구분해서 시간을 제외한 데이타만 strMessage에 다시 넣음.
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
                        this.txtPLCLog.Text = dstrTmp;   //PLC Log 내용 출력
                        this.txtPLCLog.ScrollToCaret();
                        if (this.Visible == true)
                        {
                            txtPLCLog.Focus();
                            SendKeys.Send("^{END}");    // Focus를 맨 아랫줄로 보낸다.
                            txtPLCLog.Focus();
                            
                             CursorPoint = this.txtPLCLog.AutoScrollOffset;
                            this.txtPLCLog.PointToClient(CursorPoint);
                        }
                        pstrPLCLogTime = dstrLogTime;
                        break;

                    case InfoAct.clsInfo.LogType.PLCError:
                        //인자로 들어온 strMessage에서 시간부분만 추려냄.
                        dstrLogTime = strMessage.Substring(12, 8);

                        //이전 로그의 시간과 현재 들어온 로그의 시간이 같을 경우 '\n'으로 구분해서 시간을 제외한 데이타만 strMessage에 다시 넣음.
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
                        this.txtPLCErrLog.Text = dstrTmp;            //PLC Error 내용 출력
                        
                        pstrPLCErrorLogTime = dstrLogTime;             
                        break;
                      
                    default:
                        break;
                }

                //이전 로그 시간과 현재 로그 시간을 비교하기 위해 임시 저장변수(static string dstrLogTimeTmp)에 저장해놈.
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
                        this.txtCIMLog.Text = "";   //CIM Log내용을 Clear
                        break;
                    case 1:
                        this.txtPLCLog.Text = "";   //PLC Log내용을 Clear
                        break;
                    case 2:
                        this.txtPLCErrLog.Text = "";//PLC Error Log내용을 Clear
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