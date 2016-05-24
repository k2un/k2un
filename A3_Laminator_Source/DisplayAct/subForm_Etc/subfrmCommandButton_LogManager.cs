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
    public partial class subfrmCommandButton_LogManager : UserControl
    {
        #region Fields
        #endregion

        #region Events
        public delegate void eventClickCommandButton_LogManager(object sender, EventArgs e);

        public event eventClickCommandButton_LogManager ClickSaveButton;
        #endregion

        #region Constructors
        public subfrmCommandButton_LogManager()
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

        private void subfrmCommandButton_LogManager_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ClickSaveButton != null) ClickSaveButton(this, e);
        }

        public void HideSaveButton()
        {
            try
            {
                this.btnSave.Visible = false;
            }
            catch
            {
            }
        }

        public void ShowSaveButton()
        {
            try
            {
                this.btnSave.Visible = true;
            }
            catch
            {
            }
        }
        #endregion
    }
}
