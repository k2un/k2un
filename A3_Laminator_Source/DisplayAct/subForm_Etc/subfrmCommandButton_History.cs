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
    public partial class subfrmCommandButton_History : UserControl
    {
        #region Fields
        #endregion

        #region Events
        public delegate void eventClickCommandButton_History(object sender, EventArgs e);

        public event eventClickCommandButton_History ClickClearButton;
        public event eventClickCommandButton_History ClickSaveButton;
        #endregion

        #region Constructors
        public subfrmCommandButton_History()
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

        private void subfrmCommandButton_History_Load(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (ClickClearButton != null) ClickClearButton(this, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ClickSaveButton != null) ClickSaveButton(this, e);
        }
        #endregion
    }
}
