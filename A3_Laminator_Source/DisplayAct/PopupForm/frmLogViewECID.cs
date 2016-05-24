using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using InfoAct;

namespace DisplayAct
{
    public partial class frmLogViewECID : Form
    {
        #region Fields
        public clsInfo PInfo = clsInfo.Instance;
        private string[] pstrData;
        #endregion

        #region Constructors
        public frmLogViewECID()
        {
            InitializeComponent();
        }
        #endregion


        #region Methods
        public void subFormLoad(string[] strData)
        {
            try
            {
                this.pstrData = strData;
                subSetInfoDisplay();
                subSetValueDisplay();
                this.CenterToScreen();
                this.Show();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSetInfoDisplay()
        {
            try
            {
                this.lvwInfo.Items.Clear();

                ListViewItem temp;

                DateTime dtTemp = DateTime.ParseExact(this.pstrData[1], "yyyyMMddHHmmss", null);


                temp = new ListViewItem(new string[] { "Time", dtTemp .ToString("yyyy-MM-dd HH:mm:ss")});
                this.lvwInfo.Items.Add(temp);

                temp = new ListViewItem(new string[] {"Operation", this.pstrData[2]});
                this.lvwInfo.Items.Add(temp);

                temp = new ListViewItem(new string[] {"User ID", this.pstrData[3]});
                this.lvwInfo.Items.Add(temp);

                temp = new ListViewItem(new string[] {"Description", this.pstrData[4]});
                this.lvwInfo.Items.Add(temp);    
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSetValueDisplay()
        {
            try
            {
                this.lvwValue.Items.Clear();

                ListViewItem temp;

                temp = new ListViewItem(new string[] { "ECSLL", this.pstrData[5], this.pstrData[10] });
                this.lvwValue.Items.Add(temp);

                temp = new ListViewItem(new string[] { "ECWLL", this.pstrData[6], this.pstrData[11] });
                this.lvwValue.Items.Add(temp);

                temp = new ListViewItem(new string[] { "ECDEF", this.pstrData[7], this.pstrData[12] });
                this.lvwValue.Items.Add(temp);

                temp = new ListViewItem(new string[] { "ECWUL", this.pstrData[8], this.pstrData[13] });
                this.lvwValue.Items.Add(temp);

                temp = new ListViewItem(new string[] { "ECSUL", this.pstrData[9], this.pstrData[14] });
                this.lvwValue.Items.Add(temp);
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
                this.Close();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }



        private void frmLogView_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.Dispose();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion
    }
}