using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using DBAct;

namespace DisplayAct
{
    public partial class subfrmSetupView : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;

        private tabfrmSetupAlarm tabAlarm;
        private tabfrmSetupECID tabECID;
        private tabfrmSetupEQP tabEQP;
        private tabfrmSetupUser tabUser;
        //private tabfrmSetupSVID tabSVID;
        private tabfrmSetupPPID tabPPID;
        private tabfrmSetupEOID tabEOID;

        //private tabfrmSetupGLSAPD tabGLSAPD;
        //private tabfrmSetupLOTAPD tabLOTAPD;


        private TabPage pageAlarm = new TabPage();
        private TabPage pageECID = new TabPage();
        private TabPage pageEQP = new TabPage();
        private TabPage pageMCC = new TabPage();
        private TabPage pageSEM = new TabPage();
        private TabPage pageUser = new TabPage();
        //private TabPage pageSVID = new TabPage();

        private TabPage pageGLSAPD = new TabPage();
        private TabPage pageLOTAPD = new TabPage();
        private TabPage pagePPID = new TabPage();
        private TabPage pageEOID = new TabPage();
        #endregion

        #region Properties
        /// <summary>
        /// 현재 선택된 Tab의 TEXT를 가져 옵니다.
        /// </summary>
        public string SelectedTab   // 20121129 이상창 추가
        {
            get
            {
                return this.tabControl1.SelectedTab.Text;
            }
        }
        #endregion

        #region Events
        public delegate void eventSystemSetup(object sender, EventArgs e);

        public event eventSystemSetup SelectSaveEnableTab;
        public event eventSystemSetup SelectSaveDisableTab;

        public event eventSystemSetup SelectReloadEnableTab;
        public event eventSystemSetup SelectReloadDisableTab;


        public event eventSystemSetup SelectEditModeEnableTab;
        public event eventSystemSetup SelectEditModeDisableTab;
        #endregion

        #region Constructors
        public subfrmSetupView()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        public void funInitializeForm()
        {
            try
            {
                tabAlarm = new tabfrmSetupAlarm();
                tabECID = new tabfrmSetupECID();
                tabEQP = new tabfrmSetupEQP();
                tabUser = new tabfrmSetupUser();
                //tabSVID = new tabfrmSetupSVID();
                tabPPID = new tabfrmSetupPPID();
                tabEOID = new tabfrmSetupEOID();

                //tabGLSAPD = new tabfrmSetupGLSAPD();
                //tabLOTAPD = new tabfrmSetupLOTAPD();

                tabAlarm.Dock = DockStyle.Fill;
                tabECID.Dock = DockStyle.Fill;
                tabEQP.Dock = DockStyle.Fill;
                tabUser.Dock = DockStyle.Fill;
                //tabSVID.Dock = DockStyle.Fill;
                tabPPID.Dock = DockStyle.Fill;
                //tabGLSAPD.Dock = DockStyle.Fill;
                //tabLOTAPD.Dock = DockStyle.Fill;
                tabEOID.Dock = DockStyle.Fill;

                this.tabControl1.Controls.Clear();
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
        private void subfrmSystemSetup_Load(object sender, EventArgs e)
        {
            try
            {

                pageEQP.Text = "EQP";
                pageEQP.Controls.Add(tabEQP);

                pageUser.Text = "USER";
                pageUser.Controls.Add(tabUser);

                pageECID.Text = "ECID";
                pageECID.Controls.Add(tabECID);

                //pageSVID.Text = "SVID";
                //pageSVID.Controls.Add(tabSVID);

                //pageGLSAPD.Text = "GLSAPD";
                //pageGLSAPD.Controls.Add(tabGLSAPD);

                //pageLOTAPD.Text = "LOTAPD";
                //pageLOTAPD.Controls.Add(tabLOTAPD);

                pageAlarm.Text = "ALARM";
                pageAlarm.Controls.Add(tabAlarm);

                pagePPID.Text = "PPID";
                pagePPID.Controls.Add(tabPPID);

                pageEOID.Text = "EOID";
                pageEOID.Controls.Add(tabEOID);

                this.tabControl1.Controls.Add(pageEQP);
                this.tabControl1.Controls.Add(pageUser);
                this.tabControl1.Controls.Add(pageECID);
                //this.tabControl1.Controls.Add(pageSVID);
                //this.tabControl1.Controls.Add(pageGLSAPD);
                //this.tabControl1.Controls.Add(pageLOTAPD);
                this.tabControl1.Controls.Add(pageAlarm);
                tabControl1.Controls.Add(pagePPID);
                tabControl1.Controls.Add(pageEOID);
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
        public void EditModeSetting()
        {
            try
            {
                switch (this.tabControl1.SelectedTab.Text)
                {
                    case "ALARM":
                        break;
                    case "ECID":
                        tabECID.EditMode();
                        break;
                    case "EQP":
                        break;
                    case "USER":
                        tabUser.EditMode();
                        break;
                    case "SVID":
                        break;
                    case "GLSAPD":
                        break;
                    case "LOTAPD":
                        break;
                    case "EOID":
                        tabEOID.EditMode();
                        break;
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Setup Command Button의 Save를 Click하면 frmMainView에서 이 Method를 호출한다.
        /// 현재 선택된 Setup Tab을 확인하여 해당 Tab-Form의 Save Method를 호출한다.
        /// </summary>
        public void SaveSetting()
        {
            try
            {
                switch (this.tabControl1.SelectedTab.Text)
                {
                    case "EQP":
                        tabEQP.Save();
                        break;
                    case "USER":
                        tabUser.Save();
                        break;
                    case "ECID":
                        tabECID.Save();
                        break;
                    case "SVID":
                        //tabSVID.Save();
                        break;
                    //case "GLSAPD":
                    //    tabGLSAPD.Save();
                    //    break;
                    //case "LOTAPD":
                    //    tabLOTAPD.Save();
                    //    break;
                    case "ALARM":
                        tabAlarm.Save();
                        break;

                    case "EOID":
                        tabEOID.Save();
                        break;

                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Setup Tab 선택에 따라 Command Button의 Save Button을 보여줄지 숨길지 판단.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);

                if (SelectReloadEnableTab != null)
                {
                    if (this.tabControl1.SelectedTab.Text == "EQP" || this.tabControl1.SelectedTab.Text == "MCC" || this.tabControl1.SelectedTab.Text == "SEM") SelectReloadDisableTab(this, e);
                    else SelectReloadEnableTab(this, e);
                }
                if (SelectEditModeDisableTab != null)
                {
                    if (this.tabControl1.SelectedTab.Text == "ECID" || this.tabControl1.SelectedTab.Text == "USER" || this.tabControl1.SelectedTab.Text == "EOID") SelectEditModeEnableTab(this, e);
                    else SelectEditModeDisableTab(this, e);
                }

                if (tabControl1.SelectedTab.Text == "PPID")
                {
                    if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);
                    if (SelectSaveEnableTab != null) SelectSaveDisableTab(this, e);
                    if (SelectReloadEnableTab != null) SelectReloadDisableTab(this, e);
                }


                switch (this.tabControl1.SelectedTab.Text)
                {
                    case "ALARM":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);
                        }
                        break;
                    case "ECID":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeEnableTab != null) SelectEditModeEnableTab(this, e);
                        }
                        break;
                    case "EOID":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeEnableTab != null) SelectEditModeEnableTab(this, e);
                        }
                        break;
                    case "EQP":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);
                        }
                        break;
                    case "HOST":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);

                        }
                        break;
                    case "MCC":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);

                        }
                        break;
                    case "SEM":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);

                        }
                        break;
                    case "SVID":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);

                        }
                        break;
                    case "USER":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);

                        }
                        break;
                    case "GLSAPD":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);
                        }
                        break;

                    case "LOTAPD":
                        {
                            if (SelectSaveEnableTab != null) SelectSaveEnableTab(this, e);
                            if (SelectReloadEnableTab != null) SelectReloadEnableTab(this, e);
                            if (SelectEditModeDisableTab != null) SelectEditModeDisableTab(this, e);
                        }
                        break;

                }

            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subUpdateSettingData(clsInfo.ViewEvent evtView)
        {
            try
            {
                switch (evtView)
                {
                    case clsInfo.ViewEvent.ECIDUpdate:
                        tabECID.subUpdateTable();
                        break;
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void Reload()
        {
            try
            {
                switch (this.tabControl1.SelectedTab.Text)
                {
                    case "EQP":
                        //tabEQP.Reload();
                        break;
                    case "USER":
                        tabUser.Reload();
                        break;
                    case "ECID":
                        tabECID.Reload();
                        break;
                    case "SVID":
                        //tabSVID.Reload();
                        break;
                    //case "GLSAPD":
                    //    tabGLSAPD.Reload();
                    //    break;
                    //case "LOTAPD":
                    //    tabLOTAPD.Reload();
                    //    break;
                    case "ALARM":
                        tabAlarm.Reload();
                        break;

                    case "EOID":
                        tabEOID.Reload();
                        break;
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #endregion
    }
}
