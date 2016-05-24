using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using DBAct;

namespace DisplayAct
{
    public partial class frmHostPPIDModify : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        private frmPPIDList pfrmPPIDList;
        private string strPPIDName = "";
        private int intType = 0;
        private string strType = "";



        public frmHostPPIDModify()
        {
            InitializeComponent();
        }

        public void subFormLoad(string strHostPPIDName)
        {
            try
            {
                txtHOSTPPID.Text = strHostPPIDName;
                this.txtEQPPPID_Current.Text = PInfo.Unit(0).SubUnit(0).HOSTPPID(strHostPPIDName).EQPPPID;
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void txtEQPPPID_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (pfrmPPIDList != null) pfrmPPIDList.Close();

                pfrmPPIDList = new frmPPIDList();
                pfrmPPIDList.subFormLoad("HOST", 1);
                pfrmPPIDList.ppidselect += new frmPPIDList.PPIDSelect(pfrmPPIDList_ppidselect);
                if (pfrmPPIDList.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    txtEQPPPID_Change.Text = strPPIDName;
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void pfrmPPIDList_ppidselect(string dstrType, int dintType, string dstrPPIDName)
        {
            try
            {
                strType = dstrType;
                intType = dintType;
                strPPIDName = dstrPPIDName;
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtEQPPPID_Change.Text) == false && PInfo.Unit(0).SubUnit(0).EQPPPID(this.txtEQPPPID_Change.Text) != null
                    && this.txtEQPPPID_Change.Text != txtEQPPPID_Current.Text)
                {
                    string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    PInfo.Unit(0).SubUnit(0).HOSTPPID(txtHOSTPPID.Text).EQPPPID = txtEQPPPID_Change.Text;
                    PInfo.Unit(0).SubUnit(0).HOSTPPID(txtHOSTPPID.Text).DateTime = strDateTime;

                    string dstrSQL = string.Format("Update `tbHOSTPPID` set EQPPPID='{1}', DTIME='{2}' Where HOSTPPID='{0}';", txtHOSTPPID.Text, txtEQPPPID_Change.Text, strDateTime);
                    clsDBAct.funExecuteQuery(dstrSQL);

                    PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, "3", "2", txtHOSTPPID.Text, txtEQPPPID_Change.Text);
                    DialogResult = System.Windows.Forms.DialogResult.Yes;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error!!");
                }
               
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }


       
    }
}
