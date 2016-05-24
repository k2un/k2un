using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using InfoAct;
using System.Threading;

namespace DisplayAct
{
    public partial class tabfrmMainView : UserControl
    {
        #region Fields
        public clsInfo PInfo = clsInfo.Instance;
        private string[] pstrUnitIndex;
        //public frmGLSInformation pfrmGLSInformation;
        #endregion

        #region Properties
        public string Version
        {
            get { return "Samsung SDI Wet Etch V1.3"; }
        }
        #endregion

        #region Constructor
        public tabfrmMainView()
        {
            InitializeComponent();
            funSetDoubleBuffer();

            if (!subSetLayout()) throw new Exception("DisplayAct initialize failure.");


        }
        #endregion

        #region event
        public delegate void eventClickMainEQPButton(object sender, EventArgs e);
        #endregion

        #region Methods
        private bool subSetLayout()
        {
            try
            {
                if (this.PInfo == null) return false;

                return true;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
                return false;
            }
        }

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

        private void tabfrmMainView_1024_Load(object sender, EventArgs e)
        {
            try
            {
                subMainGridViewInitial();   //Unit DataGridView 초기화
                subMainViewDisplay();       //Timer가 기동되기전 한번 호출한 후 기동한다.

                this.Show();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void subMainViewDisplay()
        {
            try
            {

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// DataGridView 초기화 (Port & Unit 정보)
        /// </summary>
        private void subMainGridViewInitial()
        {
            try
            {
                for (int dintLoop = 0; dintLoop < PInfo.UnitCount; dintLoop++)
                {
                    PInfo.Unit(dintLoop + 1).SubUnit(0).eqpState += new clsSubUnit.EQPStateChange(eqpState); 
                    PInfo.Unit(dintLoop + 1).SubUnit(0).eqpProcessState += new clsSubUnit.EQPProcessStateChange(eqpProcessState);
                    PInfo.Unit(dintLoop + 1).SubUnit(0).filmexist += new clsSubUnit.FilmExistChange(filmexist);
                    PInfo.Unit(dintLoop + 1).SubUnit(0).glsexist += new clsSubUnit.GLSExistChange(glsexist);


                    eqpState(dintLoop + 1, PInfo.Unit(dintLoop + 1).SubUnit(0).EQPState);
                    eqpProcessState(dintLoop + 1, PInfo.Unit(dintLoop + 1).SubUnit(0).EQPProcessState);
                    if (dintLoop == 7)
                    {
                        glsexist(8, PInfo.Unit(8).SubUnit(0).GLSExist);
                    }
                    else
                    {
                        filmexist(dintLoop + 1, PInfo.Unit(dintLoop + 1).SubUnit(0).FilmExist);

                    }

                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void glsexist(int UnitID, bool GLSExist)
        {
            try
            {
                //this.Invoke(new MethodInvoker(delegate()
                //{
                //    if (GLSExist)
                //    {
                //        string strGLSID = PInfo.Unit(UnitID).SubUnit(0).HGLSID;
                //        clsGLS CurrentGLS = PInfo.Unit(UnitID).SubUnit(0).CurrGLS(strGLSID);
                //        if (CurrentGLS != null)
                //            lblST01GLS.Text = string.Format("{0}\r\n{1}", strGLSID, PInfo.GLSID(strGLSID).HOSTPPID);
                //    }
                //    else
                //    {
                //        lblST01GLS.Text = "";
                //    }

                //    lblST01GLS.Visible = GLSExist;
                //}));
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void filmexist(int UnitID, bool FilmExist)
        {
            Label lblTemp = new Label();
            try
            {
                //this.Invoke(new MethodInvoker(delegate()
                //{
                //    switch (UnitID)
                //    {
                //        case 1: //FI01
                //            lblTemp = lblFL01GLS;
                //            break;

                //        case 2: //FI02
                //            lblTemp = lblFL02GLS;
                //            break;

                //        case 3: //FT01
                //            lblTemp = lblFT01GLS;
                //            break;

                //        case 4://AL01
                //            lblTemp = lblAL01GLS;
                //            break;

                //        case 5://LM01
                //            lblTemp = lblLM01GLS;
                //            break;

                //        case 6://DM01
                //            lblTemp = lblDM01GLS;
                //            break;

                //        case 7://IS01
                //            lblTemp = lblIS01GLS;
                //            break;

                //        case 8://ST01
                //            lblTemp = lblST01GLS;
                //            break;

                //        case 9://FT02
                //            lblTemp = lblFT02GLS;
                //            break;
                //    }

                //    if (FilmExist)
                //    {
                //        lblTemp.Text = string.Format("{0}\r\n{1}", PInfo.Unit(UnitID).SubUnit(0).FilmID, PInfo.Unit(UnitID).SubUnit(0).FilmCount);
                //    }
                //    else
                //    {
                //        lblTemp.Text = "";
                //    }

                //    lblTemp.Visible = FilmExist;
                //}));
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void eqpState(int UnitID, string strEqpState)
        {
            Label lblTemp = new Label();

            try
            {
                //switch (UnitID)
                //{
                //    case 1: //FI01
                //        lblTemp = lblFL01EQPState;
                //        break;

                //    case 2: //FI02
                //        lblTemp = lblFL02EQPState;
                //        break;

                //    case 3: //FT01
                //        lblTemp = lblFT01EQPState;
                //        break;

                //    case 4://AL01
                //        lblTemp = lblAL01EQPState;
                //        break;

                //    case 5://LM01
                //        lblTemp = lblLM01EQPState;
                //        break;

                //    case 6://DM01
                //        lblTemp = lblDM01EQPState;
                //        break;

                //    case 7://IS01
                //        lblTemp = lblIS01EQPState;
                //        break;

                //    case 8://ST01
                //        lblTemp = lblST01EQPState;
                //        break;

                //    case 9://FT02
                //        lblTemp = lblFT02EQPState;
                //        break;
                //}


                //switch (strEqpState)
                //{
                //    case "1": //Normal
                //        lblTemp.BackColor = Color.Yellow;
                //        lblTemp.ForeColor = Color.Black;
                //        break;

                //    case "2"://Fault
                //        lblTemp.BackColor = Color.Red;
                //        lblTemp.ForeColor = Color.White;
                //        break;

                //    case "3"://PM
                //        lblTemp.BackColor = Color.Gray;
                //        lblTemp.ForeColor = Color.White;
                //        break;
                //}
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void eqpProcessState(int UnitID, string strEQPProcessState)
        {
            Label lblTemp = new Label();

            try
            {
                //switch (UnitID)
                //{
                //    case 1: //FI01
                //        lblTemp = lblFL01EQPProcessState;
                //        break;

                //    case 2: //FI02
                //        lblTemp = lblFL02EQPProcessState;
                //        break;

                //    case 3: //FT01
                //        lblTemp = lblFT01EQPProcessState;
                //        break;

                //    case 4://AL01
                //        lblTemp = lblAL01EQPProcessState;
                //        break;

                //    case 5://LM01
                //        lblTemp = lblLM01EQPProcessState;
                //        break;

                //    case 6://DM01
                //        lblTemp = lblDM01EQPProcessState;
                //        break;

                //    case 7://IS01
                //        lblTemp = lblIS01EQPProcessState;
                //        break;

                //    case 8://ST01
                //        lblTemp = lblST01EQPProcessState;
                //        break;

                //    case 9://FT02
                //        lblTemp = lblFT02EQPProcessState;
                //        break;
                //}


                //switch (strEQPProcessState)
                //{
                //    case "1": //idle
                //            lblTemp.BackColor = Color.Yellow;
                //            lblTemp.ForeColor = Color.Black;
                //        break;

                //    case "2"://setup
                //         lblTemp.BackColor = Color.LightGreen;
                //            lblTemp.ForeColor = Color.White;
                //        break;

                //    case "3"://execute
                //         lblTemp.BackColor = Color.Blue;
                //            lblTemp.ForeColor = Color.White;
                //        break;

                //    case "4"://pause
                //         lblTemp.BackColor = Color.LightPink;
                //            lblTemp.ForeColor = Color.Black;
                //        break;

                //    case "5"://DISABLE
                //         lblTemp.BackColor = Color.Red;
                //            lblTemp.ForeColor = Color.Black;
                //        break;
                //}
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void tabfrmMainView_1024_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// DataGridView에서 다른 예외가 발생할 때 이곳으로 에러 루틴 발생. 지우지 말 것...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdMainUnitInfo_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        public void TimerControl(bool dbolEnable)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        public void subClose()
        {
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //PInfo.subPLCCommand_Set(clsInfo.PLCCommand.MCCDataSend, textBox2.Text.Trim());

                //this.PInfo.subSendSF_Set(clsInfo.SFName.S6F11EquipmentSpecifiedNetworkEvent, 171, this.PInfo.Unit(0).SubUnit(0).MultiData.TYPES("MCC").ITEMS("MCC_HOST_IP").VALUE);//this.PInfo.All.MCCNetworkPath);
                PInfo.All.CurrentHOSTPPID = "H2";
                PInfo.All.CurrentEQPPPID = "1";

                string dstrOLDHOSTPPID = PInfo.All.CurrentHOSTPPID;   //이전 PPID 백업
                string dstrNEWHOSTPPID = "H1";
                PInfo.All.CurrentHOSTPPID = dstrNEWHOSTPPID;                                                      //변경된 HOSTPPID를 입력
                PInfo.All.CurrentEQPPPID = PInfo.Unit(0).SubUnit(0).HOSTPPID(dstrNEWHOSTPPID).EQPPPID;

                PInfo.subSendSF_Set(InfoAct.clsInfo.SFName.S6F11EquipmentSpecifiedControlEvent, 131, dstrOLDHOSTPPID, dstrNEWHOSTPPID);


            }
            catch (Exception)
            {
                
                throw;
            }
        }
      
    }
}



