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
    public partial class frmErrMsgOverWrite : Form
    {
        public clsInfo PInfo = clsInfo.Instance;

        public frmErrMsgOverWrite()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form Load�� �ʿ��� �۾��� ����
        /// </summary>
        /// <param name="strMessage">Textbox�� ǥ���� �޽���</param>
        /// <param name="intOPCallType">OP Call Type</param>
        /// <param name="intPort">Port Number</param>
        public void subFormLoad(string strMessage, int intOPCallType, int intPort)
        {
            string dstrDateTime;
            string dstrMessage;

            try
            {
                dstrDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                dstrMessage = "[ " + dstrDateTime + " ]" + "\r\n" + "\r\n" + strMessage;
                this.txtMessage.Text = dstrMessage;
                this.btnClose.Focus();

                this.CenterToScreen();
                this.Show();

                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, "frmErrMsgOverWrite(subFormLoad): " + dstrMessage + ", intOPCallType: "
                  + (InfoAct.clsInfo.OPCallOverWrite)intOPCallType + ", intPort: " + intPort.ToString());  //�α� ���
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
                this.PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.BuzzerOff);     //Buzzer Off
                this.Hide();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
