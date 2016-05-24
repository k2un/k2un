using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using CommonAct;
using InfoAct;
using DBAct;

namespace DisplayAct
{
    public partial class tabfrmSetupEQP : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;
        private BindingSource pAlarmListSource = new BindingSource();
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public tabfrmSetupEQP()
        {
            InitializeComponent();
        }
        #endregion

        private void tabfrmSetupEQPInfo_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblLastModified.Text = FunINIMethod.funINIReadValue("ETCInfo", "EQPLastModified", "", this.pInfo.All.SystemINIFilePath);
                this.txtEQPID.Text = pInfo.EQP("Main").EQPID;
                this.txtMDLN.Text = pInfo.All.MDLN;
                this.txtSlotCount.Text = Convert.ToString(pInfo.EQP("Main").SlotCount);
                this.txtUnitCount.Text = Convert.ToString(pInfo.UnitCount);
                this.txtKeepDays.Text = pInfo.All.ProcDataKeepDays.ToString();
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void Save()
        {
            try
            {
                FunINIMethod.subINIWriteValue("ETCInfo", "EQPID", this.txtEQPID.Text, pInfo.All.SystemINIFilePath);
                FunINIMethod.subINIWriteValue("ETCInfo", "MDLN", this.txtMDLN.Text, pInfo.All.SystemINIFilePath);
                //Process Data Days to Keep 관련 저장
                FunINIMethod.subINIWriteValue("ETCInfo", "KEEPDAYS", this.txtKeepDays.Text, pInfo.All.SystemINIFilePath);

                pInfo.EQP("Main").EQPID = this.txtEQPID.Text;
                pInfo.All.MDLN = this.txtMDLN.Text;
                pInfo.All.ProcDataKeepDays = Convert.ToInt32(txtKeepDays.Text);
                
                subSaveLastModified(); //최종 수정된 날짜를 Ini파일에 저장한다.

                MessageBox.Show("Data Save Success!", "Jobs Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSaveLastModified()
        {
            string dstrSection = "";
            string dstrDate = "";

            try
            {
                dstrSection = "EQPLastModified";
                dstrDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                FunINIMethod.subINIWriteValue("ETCInfo", dstrSection, dstrDate, pInfo.All.SystemINIFilePath);

                this.lblLastModified.Text = dstrDate;
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void Reload()
        {
            try
            {
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnCimBitInitial_Click(object sender, EventArgs e)
        {
            try
            {
                pInfo.subPLCCommand_Set(clsInfo.PLCCommand.EventBitInitialCmd);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void btnFTP_Click(object sender, EventArgs e)
        {
            try
            { 
                this.pInfo.subSendSF_Set(clsInfo.SFName.S6F11EquipmentSpecifiedNetworkEvent, 171, this.pInfo.Unit(0).SubUnit(0).MultiData.TYPES("MCC").ITEMS("MCC_HOST_IP").VALUE);//this.PInfo.All.MCCNetworkPath);
            }
            catch (Exception ex)
            {
                pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
    }
}
