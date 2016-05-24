using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using InfoAct;

namespace DisplayAct
{
    public partial class subfrmCommandButton_Main : UserControl
    {
        #region Fields
        public clsInfo PInfo = clsInfo.Instance;
        public bool TestCheckFlag = false;
        public frmSimulationTest frmTest = new frmSimulationTest();
        #endregion

        #region Properties
        public string Version
        {
            get { return "Samsung SMD HCLN V1.0"; }
        }
        #endregion

        #region Events
        public delegate void eventClickCommandButton_Main(object sender, EventArgs e);

        public event eventClickCommandButton_Main ClickModeChangeButton;
        public event eventClickCommandButton_Main ClickEQPStateButton;
        public event eventClickCommandButton_Main ClickProcessStateButton;
        public event eventClickCommandButton_Main ClickBuzzerOffButton;
        public event eventClickCommandButton_Main ClickMessageClearButton;
        #endregion

        #region Constructor
        public subfrmCommandButton_Main()
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

        private void subfrmControlButton_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.PInfo == null) return;

                if (this.PInfo.EQP("Main").DummyPLC == true)
                {
                    this.btnTest.Visible = true;
                }
                else
                {
                    this.btnTest.Visible = false;
                }

                
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void funInitializeForm()
        {
            if (this.PInfo.EQP("Main").DummyPLC == true)
            {
                this.btnTest.Visible = true;
            }
            else
            {
                this.btnTest.Visible = false;
            }
        }

        private void rboFalse_CheckedChanged(object sender, EventArgs e)
        {
            this.PInfo.EQP("Main").RecipeCheck = false;
        }

        private void rboTrue_CheckedChanged(object sender, EventArgs e)
        {
            this.PInfo.EQP("Main").RecipeCheck = true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (frmTest.Visible == true)
                {
                    //this.PInfo.subSendSF_Set(clsInfo.SFName.S1F1AreYouThereRequest);
                }
                else
                {
                    frmTest.Show();
                }

                //pfrmLotinformationDisplay = new frmLotInformationDisplay();
                //pfrmLotinformationDisplay.subFormLoad();

            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        //public void subClose()
        //{
        //    if (this.frmTest != null && !this.frmTest.IsDisposed)
        //    {
        //        this.frmTest.subClose();
        //    }
        //}
        #endregion

        #region Button Events
        private void btnModeChange_Click(object sender, EventArgs e)
        {
            if (ClickModeChangeButton != null) ClickModeChangeButton(this, e);
        }

        private void btnEQPState_Click(object sender, EventArgs e)
        {
            if (ClickEQPStateButton != null) ClickEQPStateButton(this, e);
        }

        private void btnProcessState_Click(object sender, EventArgs e)
        {
            if (ClickProcessStateButton != null) ClickProcessStateButton(this, e);
        }

        private void btnBuzzerOff_Click(object sender, EventArgs e)
        {
            if (ClickBuzzerOffButton != null) ClickBuzzerOffButton(this, e);
        }

        private void btnMessageClear_Click(object sender, EventArgs e)
        {
            if (ClickMessageClearButton != null) ClickMessageClearButton(this, e);
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            PInfo.subPLCCommand_Set(InfoAct.clsInfo.PLCCommand.Recovery);
        }
    }
}
