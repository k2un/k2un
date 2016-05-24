using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using InfoAct;

namespace DisplayAct
{
    public partial class subfrmTitlePanel : UserControl
    {
        #region Fields
        public clsInfo PInfo = clsInfo.Instance;
        private frmDataView pfrmDataView = new frmDataView();
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public subfrmTitlePanel()
        {
            InitializeComponent();
            this.lblViewName.Text = "View Name";
        }
        #endregion

        #region Methods
        private void subfrmTitlePanel_Load(object sender, EventArgs e)
        {
           
        }

        public void funInitializeForm()
        {
            try
            {
                if (PInfo != null)
                {
                    this.lblName.Text = this.PInfo.EQP("Main").EQPName;
                }
                else
                {
                    this.lblName.Text = "EQP Name is null";
                }

                this.tmrTitlePanel.Enabled = true;
            }
            catch (Exception error)
            {
                if (PInfo != null)
                {
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
                }
            }
        }

        private void tmrTitlePanel_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tmrTitlePanel.Enabled = false;

                this.lblMainDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception error)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
            finally
            {
                this.tmrTitlePanel.Enabled = true;
            }
        }

        public void SetViewName(string strCurrentViewName)
        {
            try
            {
                if (lblViewName != null)
                {
                    this.lblViewName.Text = strCurrentViewName;
                }
                else
                {
                    this.lblViewName.Text = "Null Name";
                }
            }
            catch (Exception error)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, error.ToString());
            }
        }
        #endregion

        public void DataviewClickEvent(EventHandler e)
        {
            pictureBox1.Click += new EventHandler(e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (pfrmDataView != null)
                {
                    pfrmDataView.Close();
                }

                pfrmDataView = new frmDataView();
                pfrmDataView.subFormLoad();
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
