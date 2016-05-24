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
    public partial class tabfrmMainView_B2 : UserControl
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
        public tabfrmMainView_B2()
        {
            InitializeComponent();
            subInit();

            funSetDoubleBuffer();
            subStateinit();
            if (!subSetLayout()) throw new Exception("DisplayAct initialize failure.");


        }

        private void subStateinit()
        {
            try
            {
                for (int dintloop = 1; dintloop <= PInfo.UnitCount; dintloop++)
                {
                    for (int dintloop2 = 1; dintloop2 <= PInfo.Unit(dintloop).SubUnitCount; dintloop2++)
                    {
                        string strName = PInfo.Unit(dintloop).SubUnit(dintloop2).ModuleID.Substring(PInfo.Unit(dintloop).SubUnit(dintloop2).ModuleID.Length - 4);
                        if (dintloop == 3)
                        {
                            dgv_UnitState.Columns.Add(strName, strName);
                        }
                        else
                        {
                            //dgv_PortState.Columns.Add(strName, strName);
                        }
                    }
                }

                foreach (DataGridViewColumn col in dgv_UnitState.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }


                dgv_UnitState.Rows.Add(2);

                dgv_UnitState.Rows[0].HeaderCell.Value = "EQP STATE";
                dgv_UnitState.Rows[1].HeaderCell.Value = "Process STATE";
                
                for (int dintloop = 1; dintloop <= PInfo.UnitCount; dintloop++)
                {
                    for (int dintloop2 = 1; dintloop2 <= PInfo.Unit(dintloop).SubUnitCount; dintloop2++)
                    {
                        string strName = PInfo.Unit(dintloop).SubUnit(dintloop2).ModuleID.Substring(PInfo.Unit(dintloop).SubUnit(dintloop2).ModuleID.Length - 4);
                        if (dintloop == 3)
                        {
                            eqpState(dintloop2, PInfo.Unit(dintloop).SubUnit(dintloop2).EQPState);
                            eqpProcessState(dintloop2, PInfo.Unit(dintloop).SubUnit(dintloop2).EQPProcessState);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subInit()
        {
            try
            {
                pbPO02.Parent = pbMain;
                pbPO02.Location = new Point(339, 148);
                pbPO02.BringToFront();

                pbDM01.Parent = pbMain;
                pbDM01.Location = new Point(480, 148);
                pbDM01.BringToFront();

                pbLM01.Parent = pbMain;
                pbLM01.Location = new Point(621, 148);
                pbLM01.BringToFront();

                pbIS01.Parent = pbMain;
                pbIS01.Location = new Point(762, 148);
                pbIS01.BringToFront();

                pbST01.Parent = pbMain;
                pbST01.Location = new Point(904, 148);
                pbST01.BringToFront();

                pbPI01.Parent = pbMain;
                pbPI01.Location = new Point(303, 258);
                pbPI01.BringToFront();

                pbFT02.Parent = pbMain;
                pbFT02.Location = new Point(553, 281);
                pbFT02.BringToFront();

                pbST02.Parent = pbMain;
                pbST02.Location = new Point(866, 258);
                pbST02.BringToFront();


                pbFI01.Parent = pbMain;
                pbFI01.Location = new Point(69, 408);
                pbFI01.BringToFront();

                pbFI02.Parent = pbMain;
                pbFI02.Location = new Point(214, 408);
                pbFI02.BringToFront();

                pbFT01.Parent = pbMain;
                pbFT01.Location = new Point(355, 408);
                pbFT01.BringToFront();

                pbAL01.Parent = pbMain;
                pbAL01.Location = new Point(496, 408);
                pbAL01.BringToFront();

                pbFO04.Parent = pbMain;
                pbFO04.Location = new Point(27, 520);
                pbFO04.BringToFront();

                pbFO03.Parent = pbMain;
                pbFO03.Location = new Point(175, 520);
                pbFO03.BringToFront();

                //[2015/01/16]GL01(Add by HS)
                pbGL01.Parent = pbMain;
                pbGL01.Location = new Point(945, 42);
                pbGL01.BringToFront();

                lblAL01.Parent = pbAL01;
                lblAL01.Dock = DockStyle.Fill;
                lblAL01.BringToFront();

                lblDM01.Parent = pbDM01;
                lblDM01.Dock = DockStyle.Fill;
                lblDM01.BringToFront();

                lblFI01.Parent = pbFI01;
                lblFI01.Dock = DockStyle.Fill;
                lblFI01.BringToFront();

                lblFI02.Parent = pbFI02;
                lblFI02.Dock = DockStyle.Fill;
                lblFI02.BringToFront();

                lblFO03.Parent = pbFO03;
                lblFO03.Dock = DockStyle.Fill;
                lblFO03.BringToFront();

                lblFO04.Parent = pbFO04;
                lblFO04.Dock = DockStyle.Fill;
                lblFO04.BringToFront();

                lblFT01.Parent = pbFT01;
                lblFT01.Dock = DockStyle.Fill;
                lblFT01.BringToFront();

                lblFT02.Parent = pbFT02;
                lblFT02.Dock = DockStyle.Fill;
                lblFT02.BringToFront();

                lblIS01.Parent = pbIS01;
                lblIS01.Dock = DockStyle.Fill;
                lblIS01.BringToFront();

                lblLM01.Parent = pbLM01;
                lblLM01.Dock = DockStyle.Fill;
                lblLM01.BringToFront();

                lblPI01.Parent = pbPI01;
                lblPI01.Dock = DockStyle.Fill;
                lblPI01.BringToFront();

                lblPO02.Parent = pbPO02;
                lblPO02.Dock = DockStyle.Fill;
                lblPO02.BringToFront();

                lblST01.Parent = pbST01;
                lblST01.Dock = DockStyle.Fill;
                lblST01.BringToFront();

                lblST02.Parent = pbST02;
                lblST02.Dock = DockStyle.Fill;
                lblST02.BringToFront();

                lblGL01.Parent = pbGL01;
                lblGL01.Dock = DockStyle.Fill;
                lblGL01.BringToFront();

            }
            catch (Exception ex)
            {
                if (PInfo != null) PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
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
                for (int dintLoop = 1; dintLoop <= PInfo.Unit(3).SubUnitCount; dintLoop++)
                {
                    PInfo.Unit(3).SubUnit(dintLoop).eqpState += new clsSubUnit.EQPStateChange(eqpState);
                    PInfo.Unit(3).SubUnit(dintLoop).eqpProcessState += new clsSubUnit.EQPProcessStateChange(eqpProcessState);
                    //PInfo.Unit(3).SubUnit(dintLoop).filmexist += new clsSubUnit.FilmExistChange(filmexist);
                    PInfo.Unit(3).SubUnit(dintLoop).glsexist += new clsSubUnit.GLSExistChange(glsexist);
                    PInfo.Unit(3).SubUnit(dintLoop).GLSIDChange += new clsSubUnit.GlassIDChange(GlassIDChange);
                    glsexist(dintLoop, PInfo.Unit(3).SubUnit(dintLoop).GLSExist);
                    if (dintLoop > 6)
                    {
                        GlassIDChange(dintLoop, PInfo.Unit(3).SubUnit(dintLoop).HGLSID);
                    }
                    else
                    {
                        GlassIDChange(dintLoop, PInfo.Unit(3).SubUnit(dintLoop).FilmID);
                    }
                }

                for (int dintLoop = 1; dintLoop <= PInfo.Unit(1).SubUnitCount; dintLoop++)
                {
                    PInfo.Unit(3).SubUnit(dintLoop).eqpState += new clsSubUnit.EQPStateChange(eqpState);
                    PInfo.Unit(3).SubUnit(dintLoop).eqpProcessState += new clsSubUnit.EQPProcessStateChange(eqpProcessState);
                    PInfo.Unit(1).SubUnit(dintLoop).filmcaseexist += new clsSubUnit.FilmCaseExistChange(filmcaseexist);

                    PInfo.Unit(1).SubUnit(dintLoop).PortNo = dintLoop;
                    filmcaseexist(dintLoop, PInfo.Unit(1).SubUnit(dintLoop).FilmCaseExist);
                }

                for (int dintLoop = 1; dintLoop <= PInfo.Unit(2).SubUnitCount; dintLoop++)
                {
                    PInfo.Unit(3).SubUnit(dintLoop).eqpState += new clsSubUnit.EQPStateChange(eqpState);
                    PInfo.Unit(3).SubUnit(dintLoop).eqpProcessState += new clsSubUnit.EQPProcessStateChange(eqpProcessState);
                    PInfo.Unit(2).SubUnit(dintLoop).filmcaseexist += new clsSubUnit.FilmCaseExistChange(filmcaseexist);

                    PInfo.Unit(2).SubUnit(dintLoop).PortNo = dintLoop + 4;
                    filmcaseexist(dintLoop + 4, PInfo.Unit(2).SubUnit(dintLoop).FilmCaseExist);
                }

                for (int dintLoop = 1; dintLoop <= PInfo.PortCount; dintLoop++)
                {
                    PInfo.Port(dintLoop).cstidchange += new clsPort.CSTIDChange(cstidchange);
                }
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void cstidchange(int dintPortID, string strCSTID)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    PictureBox pb = new PictureBox();
                    Label lb = new Label();
                    //if (GLSExist)
                    {
                        switch (dintPortID)
                        {
                            case 1:
                                pb = pbFI01;
                                lb = lblFI01;
                                break;

                            case 2:
                                pb = pbFI02;
                                lb = lblFI02;
                                break;

                            case 3:
                                pb = pbFO03;
                                lb = lblFO03;
                                break;

                            case 4:
                                pb = pbFO04;
                                lb = lblFO04;
                                break;

                            case 5:
                                pb = pbPI01;
                                lb = lblPI01;
                                break;

                            case 6:
                                pb = pbPO02;
                                lb = lblPO02;
                                break;

                            default:
                                pb = null;
                                lb = null;
                                break;
                        }
                    }
                    lb.Text = string.Format("{0}", PInfo.Port(dintPortID).CSTID);
                }));
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());                
            }
        }


        void GlassIDChange(int subUnitID, string strGlassID)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    Label lb = new Label();
                    //if (GLSExist)
                    {
                        switch (subUnitID)
                        {
                            case 1://FT01
                                lb = lblFT01;
                                break;

                            case 2://FT02
                                lb = lblFT02;
                                break;

                            case 3://AL01
                                lb = lblAL01;
                                break;

                            case 4://LM01
                                lb = lblLM01;
                                break;

                            case 5://DM01
                                lb = lblDM01;
                                break;

                            case 6://IS01
                                lb = lblIS01;
                                break;

                            case 7://ST01
                                lb = lblST01;
                                break;

                            case 8://ST02
                                lb = lblST02;
                                break;

                            case 9://GL01
                                lb = lblGL01;
                                break;

                            default:
                                break;
                        }
                    }


                    lb.Text = "";
                    if (subUnitID > 6)
                    {
                        clsGLS CurrentGLS = PInfo.GLSID(PInfo.Unit(3).SubUnit(subUnitID).HGLSID);

                        if (CurrentGLS != null)
                        {
                            lb.Text = string.Format("{0}\r\n{1}", CurrentGLS.H_PANELID, CurrentGLS.SlotID);
                        }
                        else
                        {
                            lb.Text = "";
                        }
                    }
                    else
                    {
                        string strLotID = PInfo.Unit(3).SubUnit(subUnitID).FilmID;
                        int dintNo = PInfo.Unit(3).SubUnit(subUnitID).FilmCount;

                        lb.Text = string.Format("{0}\r\n{1}", strLotID, dintNo);

                    }
                }));
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void filmcaseexist(int PortNo, bool FilmCaseExist)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    PictureBox pb = new PictureBox();
                    Label lb = new Label();
                    //if (GLSExist)
                    {
                        switch (PortNo)
                        {
                            case 1:
                                pb = pbFI01;
                                lb = lblFI01;
                                break;

                            case 2:
                                pb = pbFI02;
                                lb = lblFI02;
                                break;

                            case 3:
                                pb = pbFO03;
                                lb = lblFO03;
                                break;

                            case 4:
                                pb = pbFO04;
                                lb = lblFO04;
                                break;

                            case 5:
                                pb = pbPI01;
                                lb = lblPI01;
                                break;

                            case 6:
                                pb = pbPO02;
                                lb = lblPO02;
                                break;

                            default:
                                pb = null;
                                lb = null;
                                break;
                        }
                    }

                    if (pb != null) pb.Visible = FilmCaseExist;

                    lb.Text = "";
                    lb.Text = string.Format("{0}", PInfo.Port(PortNo).CSTID);

                }));
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void glsexist(int subUnitID, bool GLSExist)
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    PictureBox pb = new PictureBox();
                    Label lb = new Label();
                    //if (GLSExist)
                    {
                        switch (subUnitID)
                        {
                            case 1://FT01
                                pb = pbFT01;
                                lb = lblFT01;
                                break;

                            case 2://FT02
                                pb = pbFT02;
                                lb = lblFT02;
                                break;

                            case 3://AL01
                                pb = pbAL01;
                                lb = lblAL01;
                                break;

                            case 4://LM01
                                pb = pbLM01;
                                lb = lblLM01;
                                break;

                            case 5://DM01
                                pb = pbDM01;
                                lb = lblDM01;
                                break;

                            case 6://IS01
                                pb = pbIS01;
                                lb = lblIS01;
                                break;

                            case 7://ST01
                                pb = pbST01;
                                lb = lblST01;
                                break;

                            case 8://ST02
                                pb = pbST02;
                                lb = lblST02;
                                break;

                            case 9://GL01
                               pb = pbGL01;
                                lb = lblGL01;
                                break;

                            default:
                                pb = null;
                                break;
                        }
                    }

                    if (pb != null) pb.Visible = GLSExist;

                    lb.Text = "";
                    if (subUnitID > 6)
                    {
                        clsGLS CurrentGLS = PInfo.GLSID(PInfo.Unit(3).SubUnit(subUnitID).HGLSID);

                        if (CurrentGLS != null)
                        {
                            lb.Text = string.Format("{0}\r\n{1}", CurrentGLS.H_PANELID, CurrentGLS.SlotID);
                        }
                        else
                        {
                            lb.Text = "";
                        }
                    }
                    else
                    {
                        string strLotID = PInfo.Unit(3).SubUnit(subUnitID).FilmID;
                        int dintNo = PInfo.Unit(3).SubUnit(subUnitID).FilmCount;

                        lb.Text = string.Format("{0}\r\n{1}", strLotID, dintNo);

                    }
                }));
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
                dgv_UnitState[UnitID-1, 0].Value = PInfo.funEQPState(strEqpState);



                switch (strEqpState)
                {
                    case "1": //Normal
                        dgv_UnitState[UnitID - 1, 0].Style.BackColor = Color.Yellow;
                        dgv_UnitState[UnitID - 1, 0].Style.ForeColor = Color.Black;
                        break;

                    case "2"://Fault
                        dgv_UnitState[UnitID - 1, 0].Style.BackColor = Color.Red;
                        dgv_UnitState[UnitID - 1, 0].Style.ForeColor = Color.White;
                        break;

                    case "3"://PM
                        dgv_UnitState[UnitID - 1, 0].Style.BackColor = Color.Gray;
                        dgv_UnitState[UnitID - 1, 0].Style.ForeColor = Color.White;
                        break;
                }

                #region
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
                #endregion
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        void eqpProcessState(int UnitID, string strEQPProcessState)
        {
            //Label lblTemp = new Label();

            try
            {
                dgv_UnitState[UnitID - 1, 1].Value = PInfo.funEQPProcessState(strEQPProcessState);

                switch (strEQPProcessState)
                {
                    case "1": //idle
                        dgv_UnitState[UnitID-1, 1].Style.BackColor = Color.Yellow;
                        dgv_UnitState[UnitID-1, 1].Style.ForeColor = Color.Black;
                        break;

                    case "2"://setup
                        dgv_UnitState[UnitID-1, 1].Style.BackColor = Color.LightGreen;
                        dgv_UnitState[UnitID-1, 1].Style.ForeColor = Color.White;
                        break;

                    case "3"://execute
                        dgv_UnitState[UnitID-1, 1].Style.BackColor = Color.Blue;
                        dgv_UnitState[UnitID-1, 1].Style.ForeColor = Color.White;
                        break;

                    case "4"://pause
                        dgv_UnitState[UnitID-1, 1].Style.BackColor = Color.LightPink;
                        dgv_UnitState[UnitID-1, 1].Style.ForeColor = Color.Black;
                        break;

                    case "5"://DISABLE
                        dgv_UnitState[UnitID-1, 1].Style.BackColor = Color.Red;
                        dgv_UnitState[UnitID-1, 1].Style.ForeColor = Color.Black;
                        break;
                }

                #region
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
                #endregion
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

        private void tabfrmMainView_T_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.Visible)
                {
                    dgv_UnitState.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void dgv_UnitState_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgv_UnitState.Visible)
                {
                    dgv_UnitState.ClearSelection();
                }

            }
            catch (Exception ex)
            {
                PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
      
      
    }
}



