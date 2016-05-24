using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DisplayAct
{
    public partial class subfrmCommandButton_Info : UserControl
    {
        #region Fields
        #endregion

        #region Events
        public delegate void eventClickCommandButton_Info(object sender, EventArgs e);

        public event eventClickCommandButton_Info ClickSEMConfigButton;
        public event eventClickCommandButton_Info ClickReloadButton;
        public event eventClickCommandButton_Info ClickHistoryButton;
        public event eventClickCommandButton_Info ClickCreateNewSetButton;

        #endregion

        #region Constructors
        public subfrmCommandButton_Info()
        {
            InitializeComponent();
            funSetDoubleBuffer();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 화면 깜빡임을 최소화 하기 위한 DoubleBuffer 사용.
        /// </summary>
        private void funSetDoubleBuffer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void subfrmCommandButton_SEM_Load(object sender, EventArgs e)
        {

        }

        private void btnSEMInfo_Click(object sender, EventArgs e)
        {
            if (ClickSEMConfigButton != null) ClickSEMConfigButton(this, e);
        }

        private void btnSVReload_Click(object sender, EventArgs e)
        {
            if (ClickReloadButton != null) ClickReloadButton(this, e);
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            if (ClickHistoryButton != null) ClickHistoryButton(this, e);
        }

        private void btnCreateNewSet_Click(object sender, EventArgs e)
        {
            if (ClickCreateNewSetButton != null) ClickCreateNewSetButton(this, e);
        }

        public void HideConfigButton()
        {
            try
            {
                this.btnSEMInfo.Visible = false;
            }
            catch
            { 
            }
        }

        public void ShowConfigButton()
        {
            try
            {
                this.btnSEMInfo.Visible = true;
            }
            catch
            {
            }
        }

        public void ShowReloadButton()
        {
            btnSVReload.Visible = true;
        }

        public void HideReloadButton()
        {
            btnSVReload.Visible = false;
        }

        public void HideHistoryButton()
        {
            btnHistory.Visible = false;
        }
        public void ShowHistoryButton()
        {
            btnHistory.Location = new Point(0, 0);
            btnHistory.Visible = true;
        }

        // 20121129 이상창
        public void ShowCreateNewButton()
        {
            btnCreateNewSet.Location = new Point(0, 76);
            btnCreateNewSet.Visible = true;
        }

        // 20121129 이상창
        public void HideCreateNewButton()
        {
            btnCreateNewSet.Visible = false;
        }

        
        #endregion

        

        
    }
}
