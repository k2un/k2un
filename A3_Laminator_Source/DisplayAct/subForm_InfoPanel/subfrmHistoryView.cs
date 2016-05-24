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
    public enum HistoryType
    {
        GLSIN_OUT = 0,
        ALARM = 1,
        GLS_APD = 2,
        LOT_APD = 3,
        SCRAP = 4,
        PARAMETER = 5,
        HOSTMSG = 6,

        APC = 10,
        RPC = 11,
        PPC = 12,

        Cleaner_DV =13,
        Oven_DV = 14,

        NONE = 100,
    }

    public partial class subfrmHistoryView : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;

        private tabfrmHistoryGeneral tabfrmAlarmHistory;
        //private tabfrmHistoryGeneral tabfrmGlassApdHistory;
        //private tabfrmHistoryGeneral tabfrmLotApdHistory;
        //private tabfrmHistoryGeneral tabfrmScrapHistory;
        private tabfrmHistoryGeneral tabfrmMessageHistory;
        private tabfrmHistoryGeneral tabfrmGLSIn_Out;
        private tabfrmHistoryGeneral tabfrmCleaner_DV;
        private tabfrmHistoryGeneral tabfrmOven_DV;

        private TabPage tabAlarmHistory = new TabPage();
        private TabPage tabGlassApdHistory = new TabPage();
        private TabPage tabLotApdHistory = new TabPage();
        private TabPage tabScrapHistory = new TabPage();
        private TabPage tabMessageHistory = new TabPage();
        private TabPage tabGLSIn_Out = new TabPage();
        private TabPage tabCleaner_DV = new TabPage();
        private TabPage tabOven_DV = new TabPage();

        #endregion

        #region Properties
        //public string SetTabPage
        //{
        //    set
        //    {
        //        try
        //        {
                    
        //        }
        //        catch (Exception ex)
        //        {
        //            if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
        //        }
        //    }
        //}
        #endregion

        #region Constructors
        public subfrmHistoryView()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        public void funInitializeForm()
        {
            try
            {
                tabfrmAlarmHistory = new tabfrmHistoryGeneral();
                tabfrmAlarmHistory.funInitializeForm();
                tabfrmAlarmHistory.Dock = DockStyle.Fill;

                //tabfrmGlassApdHistory = new tabfrmHistoryGeneral();
                //tabfrmGlassApdHistory.funInitializeForm();
                //tabfrmGlassApdHistory.Dock = DockStyle.Fill;

                //tabfrmLotApdHistory = new tabfrmHistoryGeneral();
                //tabfrmLotApdHistory.funInitializeForm();
                //tabfrmLotApdHistory.Dock = DockStyle.Fill;

                //tabfrmScrapHistory = new tabfrmHistoryGeneral();
                //tabfrmScrapHistory.funInitializeForm();
                //tabfrmScrapHistory.Dock = DockStyle.Fill;

                tabfrmMessageHistory = new tabfrmHistoryGeneral();
                tabfrmMessageHistory.funInitializeForm();
                tabfrmMessageHistory.Dock = DockStyle.Fill;

                tabfrmGLSIn_Out = new tabfrmHistoryGeneral();
                tabfrmGLSIn_Out.funInitializeForm();
                tabfrmGLSIn_Out.Dock = DockStyle.Fill;

                //tabfrmCleaner_DV = new tabfrmHistoryGeneral();
                //tabfrmCleaner_DV.funInitializeForm();
                //tabfrmCleaner_DV.Dock = DockStyle.Fill;

                //tabfrmOven_DV = new tabfrmHistoryGeneral();
                //tabfrmOven_DV.funInitializeForm();
                //tabfrmOven_DV.Dock = DockStyle.Fill;

            }
            catch(Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());

            }
        }

        private void subfrmHistoryView_Load(object sender, EventArgs e)
        {
            try
            {
                this.tabControl1.Controls.Clear();

                tabAlarmHistory.Text = HistoryType.ALARM.ToString();
                tabAlarmHistory.Controls.Add(tabfrmAlarmHistory);

                //tabGlassApdHistory.Text = HistoryType.GLS_APD.ToString();
                //tabGlassApdHistory.Controls.Add(tabfrmGlassApdHistory);
                //tabLotApdHistory.Text = HistoryType.LOT_APD.ToString();
                //tabLotApdHistory.Controls.Add(tabfrmLotApdHistory);
                //tabScrapHistory.Text = HistoryType.SCRAP.ToString();
                //tabScrapHistory.Controls.Add(tabfrmScrapHistory);

                tabMessageHistory.Text = HistoryType.HOSTMSG.ToString();
                tabMessageHistory.Controls.Add(tabfrmMessageHistory);

                tabGLSIn_Out.Text = HistoryType.GLSIN_OUT.ToString();
                tabGLSIn_Out.Controls.Add(tabfrmGLSIn_Out);

                //tabCleaner_DV.Text = HistoryType.Cleaner_DV.ToString();
                //tabCleaner_DV.Controls.Add(tabfrmCleaner_DV);

                //tabOven_DV.Text = HistoryType.Oven_DV.ToString();
                //tabOven_DV.Controls.Add(tabfrmOven_DV);
                
                this.tabControl1.Controls.Add(tabAlarmHistory);
                //this.tabControl1.Controls.Add(tabGlassApdHistory);
                //this.tabControl1.Controls.Add(tabLotApdHistory);
                //this.tabControl1.Controls.Add(tabScrapHistory);
                this.tabControl1.Controls.Add(tabMessageHistory);
                tabControl1.Controls.Add(tabGLSIn_Out);
                //tabControl1.Controls.Add(tabCleaner_DV);
                //tabControl1.Controls.Add(tabOven_DV);


                //tabfrmHistoryGeneral을 사용하는 tab은 초기 로딩시 원하는 History 유형에 따라 Form을 initialize 해 준다.
                tabfrmAlarmHistory.subInitForm(HistoryType.ALARM);
                tabfrmMessageHistory.subInitForm(HistoryType.HOSTMSG);
                //tabfrmGlassApdHistory.subInitForm(HistoryType.GLS_APD);
                //tabfrmLotApdHistory.subInitForm(HistoryType.LOT_APD);
                //tabfrmScrapHistory.subInitForm(HistoryType.SCRAP);
                tabfrmGLSIn_Out.subInitForm(HistoryType.GLSIN_OUT);
                //tabfrmCleaner_DV.subInitForm(HistoryType.Cleaner_DV);
                //tabfrmOven_DV.subInitForm(HistoryType.Oven_DV);
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Command Button 중 Save Button의 Event가 발생하면 frmMainView에서 이 Method를 호출한다.
        /// History Tab중 어떤 Tab이 선택되었는지를 확인하여 해당 Tab-Form의 Save Method를 호출.
        /// </summary>
        public void Save()
        {
            try
            {

                foreach (Control ctl in this.tabControl1.Controls)
                {
                    if (((tabfrmHistoryGeneral)ctl.Controls[0]).CurrentHistoryType.ToString() == this.tabControl1.SelectedTab.Text)
                    {
                        ((tabfrmHistoryGeneral)ctl.Controls[0]).Save();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Command Button 중 Clear Button의 Event가 발생하면 frmMainView에서 이 Method를 호출한다.
        /// History Tab중 어떤 Tab이 선택되었는지를 확인하여 해당 Tab-Form의 Clear Method를 호출.
        /// </summary>
        public void Clear()
        {
            try
            {
                foreach (Control ctl in this.tabControl1.Controls)
                {
                    if (ctl.Controls[0] is tabfrmHistoryGeneral)
                    {
                        if (((tabfrmHistoryGeneral)ctl.Controls[0]).CurrentHistoryType.ToString() == this.tabControl1.SelectedTab.Text)
                        {
                            ((tabfrmHistoryGeneral)ctl.Controls[0]).Clear();
                            break;
                        }
                    }
                    else if (ctl.Controls[0] is tabfrmHistoryGroup)
                    {
                        if (((tabfrmHistoryGroup)ctl.Controls[0]).CurrentHistoryType.ToString() == this.tabControl1.SelectedTab.Text)
                        {
                            ((tabfrmHistoryGroup)ctl.Controls[0]).Clear();
                            break;
                        }
                    }
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

