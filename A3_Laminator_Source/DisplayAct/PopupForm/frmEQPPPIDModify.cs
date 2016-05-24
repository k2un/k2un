using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using DBAct;
using System.Threading;

namespace DisplayAct
{
    public partial class frmEQPPPIDModify : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        private frmPPIDList pfrmPPIDList;
        private string strPPIDName = "";
        private int intType = 0;
        private string strType = "";

        private string strClickType = "";


        public frmEQPPPIDModify()
        {
            InitializeComponent();
        }

        public void subFormLoad(string strEQPPPID)
        {
            try
            {
                txtEQPPPID.Text = strEQPPPID;
                txtCurrent_UP.Text = PInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID).UP_EQPPPID;
                txtCurrent_LOW.Text = PInfo.Unit(0).SubUnit(0).MappingEQPPPID(strEQPPPID).LOW_EQPPPID;
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
                TextBox txtTemp = (TextBox)sender;
                strClickType = txtTemp.Tag.ToString();

                if (pfrmPPIDList != null) pfrmPPIDList.Close();

                pfrmPPIDList = new frmPPIDList();
                pfrmPPIDList.subFormLoad("EQP", 3);
                pfrmPPIDList.ppidselect += new frmPPIDList.PPIDSelect(pfrmPPIDList_ppidselect);

                if (pfrmPPIDList.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    txtTemp.Text = strPPIDName;
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
                if (string.IsNullOrEmpty(txtEQPPPID.Text) == false && string.IsNullOrEmpty(txtChange_UP.Text) == false && string.IsNullOrEmpty(txtChange_LOW.Text) == false)
                {
                    PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text).UP_EQPPPID = txtCurrent_UP.Text;
                    PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text).LOW_EQPPPID = txtCurrent_LOW.Text;
                    PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text).DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text).PPIDCommand = clsInfo.PPIDCMD.Modify;
                    PInfo.SetPPIDCMD(txtEQPPPID.Text);

                    string dstrSQL = string.Format("update `tbEQPPPID` set UP_PPID='{0}', LOW_PPID='{1}' where EQPPPID='{2}';", txtChange_UP.Text, txtChange_LOW.Text, txtEQPPPID.Text);
                    clsDBAct.funExecuteQuery(dstrSQL);

                    DialogResult = System.Windows.Forms.DialogResult.Yes;
                    this.Hide();
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
