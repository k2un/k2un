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
    public partial class frmHostPPIDCreate : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        private frmPPIDList pfrmPPIDList;
        private string strPPIDName = "";
        private int intType = 0;
        private string strType = "";



        public frmHostPPIDCreate()
        {
            InitializeComponent();
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
                    txtEQPPPID.Text = strPPIDName;
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
                if (string.IsNullOrEmpty(txtHOSTPPID.Text) == false && string.IsNullOrEmpty(txtEQPPPID.Text) == false)
                {
                    if (PInfo.Unit(0).SubUnit(0).HOSTPPID(txtHOSTPPID.Text) == null)
                    {
                        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        PInfo.Unit(0).SubUnit(0).AddHOSTPPID(txtHOSTPPID.Text);
                        PInfo.Unit(0).SubUnit(0).HOSTPPID(txtHOSTPPID.Text);

                        PInfo.Unit(0).SubUnit(0).HOSTPPID(txtHOSTPPID.Text).EQPPPID = txtEQPPPID.Text;
                        PInfo.Unit(0).SubUnit(0).HOSTPPID(txtHOSTPPID.Text).DateTime = strDateTime;

                        string dstrSQL = string.Format("INSERT INTO `tbHOSTPPID` values ('{0}','{1}','{2}');", txtHOSTPPID.Text, txtEQPPPID.Text, strDateTime);
                        clsDBAct.funExecuteQuery(dstrSQL);

                        PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, "1", "2", txtHOSTPPID.Text, txtEQPPPID.Text);
                        DialogResult = System.Windows.Forms.DialogResult.Yes;
                        this.Hide();
                    }
                    else
                    {
                        //Error Message!!
                        MessageBox.Show("Error!! HostPPID Exist!!");
                    }
                }
                else
                {
                    MessageBox.Show("Input Data Error!!");
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
