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
    public partial class subfrmMainView : UserControl
    {
        #region Fields
        private clsInfo pInfo = clsInfo.Instance;

        private tabfrmMainView_T pfrmMainView_T;
        private tabfrmMainView_B pfrmMainView_B;
        private tabfrmMainView_T2 pfrmMainView_T2;
        private tabfrmMainView_B2 pfrmMainView_B2;

        private TabPage tabMain = new TabPage();

        #endregion

        #region Properties
        #endregion

        #region Constructors
        public subfrmMainView()
        {
            InitializeComponent();
        }
        #endregion

        #region event
        public delegate void eventClickMainEQPButton(object sender, EventArgs e);

        public event eventClickMainEQPButton ClickOven01State;
        public event eventClickMainEQPButton ClickOven02State;
        public event eventClickMainEQPButton ClickPort01State;
        public event eventClickMainEQPButton ClickPort02State;
        public event eventClickMainEQPButton ClickPort03State;
        public event eventClickMainEQPButton ClickPort04State;

        public delegate void eventOvenEQPStateChange(int UnitID, int SubUnitID, string EqpState);
        public event eventOvenEQPStateChange Oven01EQPState;
        public event eventOvenEQPStateChange Oven02EQPState;

        public delegate void eventOvenGlassExist(int UnitID, int SubUnitID, bool ExistFlag);
        public event eventOvenGlassExist Oven01GlassExist;
        public event eventOvenGlassExist Oven02GlassExist;

        #endregion


        #region Methods
        public void funInitializeForm()
        {
            try
            {
                if (pInfo.All.EQPID == "A3TLM02S")
                {
                    //pfrmMainView_T = new tabfrmMainView_T();
                    ////pfrmMainView.PInfo = this.pInfo;
                    //pfrmMainView_T.Dock = DockStyle.Fill;

                    pfrmMainView_T2 = new tabfrmMainView_T2();
                    pfrmMainView_T2.Dock = DockStyle.Fill;

                }
                else
                {
                    //pfrmMainView_B = new tabfrmMainView_B();
                    ////pfrmMainView.PInfo = this.pInfo;
                    //pfrmMainView_B.Dock = DockStyle.Fill;

                    pfrmMainView_B2 = new tabfrmMainView_B2();
                    pfrmMainView_B2.Dock = DockStyle.Fill;

                    this.panel1.Controls.Clear();
                }
                this.panel1.Controls.Clear();
                
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

     
        private void subfrmMainView_Load(object sender, EventArgs e)
        {
            tabMain.Text = "Main";
            if (pInfo.All.EQPID == "A3TLM02S")
            {
                //tabMain.Controls.Add(pfrmMainView_T);
                //this.panel1.Controls.Add(pfrmMainView_T);
                tabMain.Controls.Add(pfrmMainView_T2);
                panel1.Controls.Add(pfrmMainView_T2);
            }
            else
            {
                //tabMain.Controls.Add(pfrmMainView_B);
                //this.panel1.Controls.Add(pfrmMainView_B);
                tabMain.Controls.Add(pfrmMainView_B2);
                this.panel1.Controls.Add(pfrmMainView_B2);
            }
        }

     
        /// <summary>
        /// MainView 영역의 모든 UserControl의 Timer Disable
        /// Navigation Button으로 MainView 외 다른 View가 선택되면
        /// MainView의 Timer를 정지.
        /// </summary>
        public void DisableUpdate()
        {
            try
            {

                if (pInfo.All.EQPID == "A3TLM02S")
                {
                    pfrmMainView_T2.TimerControl(false);
                }
                else
                {
                    pfrmMainView_B2.TimerControl(false);
                }
            }
            catch (Exception ex)
            {
                if (pInfo != null) pInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subClose()
        {

            if (pInfo.All.EQPID == "A3TLM02S")
            {
                pfrmMainView_T2.subClose();
            }
            else
            {
                pfrmMainView_B2.subClose();
            }
        }
    

        #endregion
    }
}
