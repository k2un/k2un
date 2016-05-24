using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InfoAct;

namespace DisplayAct
{
    public partial class subfrmNavigationButton : UserControl
    {
        #region Fields
        public clsInfo PInfo = clsInfo.Instance;
        #endregion

        #region Events
        public delegate void eventClickNavigationButton(object sender, EventArgs e);

        public event eventClickNavigationButton ClickMainViewButton;
        public event eventClickNavigationButton ClickHistoryViewButton;
        public event eventClickNavigationButton ClickLogViewButton;
        public event eventClickNavigationButton ClickSetupViewButton;
        public event eventClickNavigationButton ClickSEMViewButton;
        public event eventClickNavigationButton ClickInfoButton;
        public event eventClickNavigationButton ClickSubDisplayButton;
        
        #endregion

        #region Properties
        #endregion

        #region Constructor
        public subfrmNavigationButton()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void subfrmOperationButton_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                if (PInfo != null)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void funInitializeForm()
        {
            try
            {
                this.Show();
            }
            catch (Exception ex)
            {
                if(PInfo != null)
                    this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        ///// <summary>
        ///// Loading VCR Reading Mode Changing Form
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnVCRReadingMode_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.pfrmLogOn.pstrVCRorSYstemSetup = "VCR";
        //        this.pfrmLogOn.subFormLoad();

        //        //this.pfrmVCRReadingMode.subFormLoad();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
        //    }
        //}
        #endregion

        #region Button Events
        /// <summary>
        /// MainView Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMainView_Click(object sender, EventArgs e)
        {
            if (ClickMainViewButton != null) ClickMainViewButton(this, e);
        }

        /// <summary>
        /// Alarm History Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHistoryView_Click(object sender, EventArgs e)
        {
            if (ClickHistoryViewButton != null) ClickHistoryViewButton(this, e);
        }

        /// <summary>
        /// LOG Manager Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogView_Click(object sender, EventArgs e)
        {
            if (ClickLogViewButton != null) ClickLogViewButton(this, e);
        }

        /// <summary>
        /// System Setup Button Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetupView_Click(object sender, EventArgs e)
        {
            if (ClickSetupViewButton != null) ClickSetupViewButton(this, e);
        }

        private void btnSEMInfo_Click(object sender, EventArgs e)
        {
            if (ClickSEMViewButton != null) ClickSEMViewButton(this, e);
        }
      
        #endregion

        private void btnSubDisplayView_Click(object sender, EventArgs e)
        {
            if(ClickSubDisplayButton != null) ClickSubDisplayButton(sender, e);
        }

        private void btnInfoView_Click(object sender, EventArgs e)
        {
            if (ClickInfoButton != null) ClickInfoButton(this, e);
        }
    }
}
