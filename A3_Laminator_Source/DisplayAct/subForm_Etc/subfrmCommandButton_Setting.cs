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
    public partial class subfrmCommandButton_Setting : UserControl
    {
        #region Fields
        #endregion

        #region Events
        public delegate void eventClickCommandButton_Setting(object sender, EventArgs e);

        public event eventClickCommandButton_Setting ClickSaveButton;
        public event eventClickCommandButton_Setting ClickReloadButton;
        public event eventClickCommandButton_Setting ClickEditMode;
        #endregion

        #region Constructors
        public subfrmCommandButton_Setting()
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

        private void subfrmCommandButton_Setting_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ClickSaveButton != null) ClickSaveButton(this, e);
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (ClickReloadButton != null) ClickReloadButton(this, e);
        }

        private void cbListEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (ClickEditMode != null) ClickEditMode(this, e);
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

        public void HideReloadButton()
        {
            btnReload.Visible = false;
        }

        public void ShowReloadButton()
        {
            btnReload.Visible = true;
        }
        public void ShowListEditCheckBox()
        {
            cbListEdit.Visible = true;
        }
        public void HideListEditCheckBox()
        {
            cbListEdit.Visible = false;
        }

        


        #endregion

        
    }
}
