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
    public partial class frmErrMSG : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        private int pintpOPCallType;

        public frmErrMSG()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Form Load�� �ʿ��� �۾��� ����
        /// </summary>
        /// <param name="strMessage">Textbox�� ǥ���� �޽���</param>
        /// <param name="intOPCallType">OP Call Type</param>
        public void subFormLoad(string strMessage)
        {
            try
            {
                txtMessage.Text = strMessage;
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
    }
}
