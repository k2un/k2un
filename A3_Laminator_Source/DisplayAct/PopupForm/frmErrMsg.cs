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
        /// Form Load시 필요한 작업을 수행
        /// </summary>
        /// <param name="strMessage">Textbox에 표시할 메시지</param>
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
