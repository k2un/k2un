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
    public partial class frmEQPPPIDCreate : Form
    {
        public clsInfo PInfo = clsInfo.Instance;
        private frmPPIDList pfrmPPIDList;
        private string strPPIDName = "";
        private int intType = 0;
        private string strType = "";



        public frmEQPPPIDCreate()
        {
            InitializeComponent();
        }

        private void txtEQPPPID_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TextBox txtTemp = (TextBox)sender;

                if (pfrmPPIDList != null) pfrmPPIDList.Close();

                pfrmPPIDList = new frmPPIDList();
                pfrmPPIDList.subFormLoad("EQP", 3);
                pfrmPPIDList.ppidselect += new frmPPIDList.PPIDSelect(pfrmPPIDList_ppidselect);

                if (pfrmPPIDList.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    txtTemp.Text = strPPIDName;
                }

                //switch (txtTemp.Name)
                //{
                //    case "txtEQPPPID_UP":
                //        if (pfrmPPIDList.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                //        {
                //            txtEQPPPID_UP.Text = strPPIDName;
                //        }
                //        break;

                //    case "txtEQPPPID_LOW":
                //        if (pfrmPPIDList.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                //        {
                //            txtEQPPPID_UP.Text = strPPIDName;
                //        }
                //        break;
                //}

               
               
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
                if (string.IsNullOrEmpty(txtEQPPPID.Text) == false && string.IsNullOrEmpty(txtEQPPPID_UP.Text) == false && string.IsNullOrEmpty(txtEQPPPID_LOW.Text) == false)
                {
                    if (PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text) == null)
                    {
                        string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        PInfo.Unit(0).SubUnit(0).AddMappingEQPPPID(txtEQPPPID.Text);
                        PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text);

                        PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text).UP_EQPPPID = txtEQPPPID_UP.Text;
                        PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text).LOW_EQPPPID = txtEQPPPID_LOW.Text;
                        PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text).DateTime = strDateTime;
                        PInfo.Unit(0).SubUnit(0).MappingEQPPPID(txtEQPPPID.Text).PPIDCommand = clsInfo.PPIDCMD.Create;
                        PInfo.SetPPIDCMD(txtEQPPPID.Text);

                        //PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, 1, txtEQPPPID_UP.Text, "");
                        //subWaitDuringReadFromPLC();
                        
                        //PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.OnePPIDRead, 1, txtEQPPPID_LOW.Text, "");
                        //subWaitDuringReadFromPLC();

                        string dstrSQL = string.Format("INSERT INTO `tbEQPPPID` values ('{0}','{1}','{2}', '{3}');", txtEQPPPID.Text, txtEQPPPID_UP.Text, txtEQPPPID_LOW.Text, strDateTime);
                        clsDBAct.funExecuteQuery(dstrSQL);

                        PInfo.All.CreatePPIDName = txtEQPPPID.Text;
                        PInfo.All.CreatePPIDType = 1;

                        //PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S7F107PPIDCreatDeleteAndPPBodyChangedReport, "1", "1","", txtEQPPPID.Text);
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
