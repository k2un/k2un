using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using InfoAct;
using CommonAct;
using System.Collections;

namespace DisplayAct
{
    public partial class tabfrmHistoryGeneral : UserControl
    {
        #region Fields
        public clsInfo PInfo = clsInfo.Instance;
        
        
        private int pintFirstRowIndex = 0;
        private ArrayList pstrData = new ArrayList();

        private Form pfrmPopup;

        private string pstrSubTypeNow = string.Empty;
        private string pstrLogFileName = string.Empty;
        private string pstrDateString = string.Empty;
        private HistoryType enmCurHistoryType = HistoryType.NONE;


        private System.Windows.Forms.Panel pnlFileOpen;
        private System.Windows.Forms.Label lbText;
        private System.Windows.Forms.ProgressBar pgb;

        private int intDisplayRowCount = 32;


        #endregion

        #region Properties
        public HistoryType CurrentHistoryType
        {
            get { return enmCurHistoryType; }
        }
       
        #endregion

        #region Constructors
        public tabfrmHistoryGeneral()
        {
            InitializeComponent();
            subSetFileOpenPanel();
            funSetDoubleBuffered(this.tvDate);
            funSetDoubleBuffered(this.dgv);
        }

        #endregion

        #region Methods

        /// <summary>
        /// LOG Type에 따라 디스플레이 레이아웃 설정
        /// </summary>
        /// <param name="type"></param>
        public void subInitForm(HistoryType type)
        {
            try
            {
                this.enmCurHistoryType = type;

                switch (this.enmCurHistoryType)
                {
                    case HistoryType.ALARM:
                        pstrLogFileName = "\\Alarm.Log";
                        subSetForAlarm();
                        break;

                    case HistoryType.GLS_APD:
                        pstrLogFileName = "\\GLSPDC.Log";
                        subSetForGLSAPD();
                        break;

                    case HistoryType.LOT_APD:
                        pstrLogFileName = "\\LOTPDC.Log";
                        subSetForLOTAPD();
                        break;

                    case HistoryType.SCRAP:
                        pstrLogFileName = "\\Scrap.Log";
                        subSetForSCRAP();
                        break;

                    case HistoryType.HOSTMSG:
                        pstrLogFileName = "\\OPCallMSG.Log";
                        subSetForHOSTMSG();
                        break;

                    case HistoryType.GLSIN_OUT:
                        pstrLogFileName = "\\GLSInOut.Log";
                        subSetGLSInOut();
                        break;

                    case HistoryType.PARAMETER:
                        pstrLogFileName = "\\Parameter.Log";
                        subSetForPPID();
                        break;


                    case HistoryType.NONE:
                        pstrLogFileName = "\\";
                        // 화면에 설정 잘못된거 알려주면 되나?
                        break;
                }


                if (enmCurHistoryType == HistoryType.GLSIN_OUT)
                {
                    cbSubType.Items.Clear();
                    cbSubType.Items.Add("ALL");
                    //cbSubType.Items.Add("FI01");
                    //cbSubType.Items.Add("FI02");
                    //cbSubType.Items.Add("FT01");
                    //cbSubType.Items.Add("AL01");
                    //cbSubType.Items.Add("LM01");
                    //cbSubType.Items.Add("DM01");
                    //cbSubType.Items.Add("IS01");
                    if (PInfo.All.EQPID == "A3TLM02S")
                    {
                        cbSubType.Items.Add("ST01");
                        cbSubType.Items.Add("ST02");
                    }
                    else
                    {
                        cbSubType.Items.Add("ST01");
                        cbSubType.Items.Add("ST02");
                        cbSubType.Items.Add("GL01");
                    }
                    //cbSubType.Items.Add("FT01");

                    cbSubType.Text = "ALL";

                    lblUnitID.Visible = true;
                    lblGLSID.Visible = true;
                    cbSubType.Visible = true;
                    txtGLSID.Visible = true;
                    btnSearch.Visible = true;
                    dgv.ScrollBars = ScrollBars.None;
                }
                //else if(enmCurHistoryType == HistoryType.Oven_DV || enmCurHistoryType == HistoryType.Cleaner_DV)
                //{
                    //cbSubType.Items.Clear();
                    //cbSubType.Items.Add("ALL");
                    //cbSubType.Items.Add("U01");
                    //cbSubType.Items.Add("U03");
                    //cbSubType.Items.Add("U04");

                    //cbSubType.Text = "ALL";

                    //lblUnitID.Visible = false;
                    //lblGLSID.Visible = true;
                    //cbSubType.Visible = false;
                    //txtGLSID.Visible = true;
                    //btnSearch.Visible = true;

               //     dgv.ScrollBars = ScrollBars.Horizontal;
               //}
                else if (enmCurHistoryType == HistoryType.PARAMETER)
                {
                    cbSubType.Items.Clear();
                    cbSubType.Items.Add("PPID");
                    cbSubType.Items.Add("EOID");
                    cbSubType.Items.Add("ECID");

                    cbSubType.Text = "PPID";

                    lblUnitID.Visible = true;
                    lblGLSID.Visible = false;
                    cbSubType.Visible = true;
                    txtGLSID.Visible = false;
                    btnSearch.Visible = false;
                    dgv.ScrollBars = ScrollBars.Horizontal;

                }
                else
                {
                    lblUnitID.Visible = false;
                    lblGLSID.Visible = false;
                    cbSubType.Visible = false;
                    txtGLSID.Visible = false;
                    btnSearch.Visible = false;
                    dgv.ScrollBars = ScrollBars.None;
                }



                this.tvDate.Visible = true;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Parameter History View
        /// Sub Type 변경시 History View용 DataGridView Column Setting
        /// </summary>
        private void cbSubType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                // HistoryType.PARAMETER
                // -- EOID, ECID, PPID

                string dstrSubType ="";
                if(cbSubType.Visible)
                {
                    dstrSubType = this.cbSubType.SelectedItem.ToString();
                }
                else
                {
                    dstrSubType = "ALL";                    
                }

                //if (!dstrSubType.Equals(pstrSubTypeNow))
                //{
                    pstrSubTypeNow = dstrSubType;

                    //if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();
                    //if (this.pstrData.Count > 0) this.pstrData.Clear();

                    switch (dstrSubType)
                    {
                        case "GLSIn_Out":
                            subSetGLSInOut();
                            break;
                        
						case "PPID":
                            subSetForPPID();
                            break;
                        case "EOID":
                            subSetForEOID();
                            break;
                        case "ECID":
                            subSetForECID();
                            break;
                        
                    }

                    this.tvDate_DoubleClick(this.tvDate, e);

                    //if (this.pstrData.Count > 0) this.pstrData.Clear();
                    //this.subSetPageControl();
                    //this.tvDate_VisibleChanged(this, e);
                //}
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        #region subTypeSetting
        /// <summary>
        /// PPID History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForPPID()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                System.Windows.Forms.DataGridViewTextBoxColumn cType = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cTime = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cRev = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cComment = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cOperation = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cUserID = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cHostPPID = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cBeforeEQP = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cAfterEQP = new DataGridViewTextBoxColumn();

                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cType, cTime, cRev, cComment, cOperation, cUserID, cHostPPID, cBeforeEQP, cAfterEQP });


                cType.FillWeight = 120F;
                cType.Frozen = true;
                cType.HeaderText = "Type";
                cType.MaxInputLength = 14;
                cType.MinimumWidth = 110;
                cType.Name = "cType";
                cType.ReadOnly = true;
                cType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cType.Width = 110;
                cType.Visible = false;

                cTime.FillWeight = 120F;
                cTime.Frozen = true;
                cTime.HeaderText = "Time";
                cTime.MaxInputLength = 14;
                cTime.MinimumWidth = 110;
                cTime.Name = "cTime";
                cTime.ReadOnly = true;
                cTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cTime.Width = 110;

                cRev.FillWeight = 120F;
                //cRev.Frozen = true;
                cRev.HeaderText = "PPID Rev";
                cRev.MaxInputLength = 14;
                cRev.MinimumWidth = 110;
                cRev.Name = "cRev";
                cRev.ReadOnly = true;
                cRev.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cRev.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cRev.Width = 110;
                cRev.Visible = false;

                cComment.FillWeight = 120F;
                cComment.Frozen = true;
                cComment.HeaderText = "Comment";
                cComment.MaxInputLength = 100;
                cComment.MinimumWidth = 110;
                cComment.Name = "cComment";
                cComment.ReadOnly = true;
                cComment.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cComment.Width = 110;
                cComment.Visible = false;

                cOperation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cOperation.FillWeight = 120F;
                cOperation.HeaderText = "Operation";
                cOperation.MaxInputLength = 50;
                cOperation.MinimumWidth = 200;
                cOperation.Name = "cOperation";
                cOperation.ReadOnly = true;
                cOperation.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cOperation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

                cUserID.FillWeight = 120F;
                //cUserID.Frozen = true;
                cUserID.HeaderText = "UserID";
                cUserID.MaxInputLength = 25;
                cUserID.MinimumWidth = 110;
                cUserID.Name = "cUserID";
                cUserID.ReadOnly = true;
                cUserID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cUserID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cUserID.Width = 110;
                cUserID.Visible = false;

                cHostPPID.FillWeight = 120F;
                //cHostPPID.Frozen = true;
                cHostPPID.HeaderText = "HOST PPID";
                cHostPPID.MaxInputLength = 28;
                cHostPPID.MinimumWidth = 120;
                cHostPPID.Name = "cHostPPID";
                cHostPPID.ReadOnly = true;
                cHostPPID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cHostPPID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cHostPPID.Width = 120;

                cBeforeEQP.FillWeight = 120F;
                //cBeforeEQP.Frozen = true;
                cBeforeEQP.HeaderText = "BeforeEQP";
                cBeforeEQP.MaxInputLength = 28;
                cBeforeEQP.MinimumWidth = 120;
                cBeforeEQP.Name = "cBeforeEQP";
                cBeforeEQP.ReadOnly = true;
                cBeforeEQP.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cBeforeEQP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cBeforeEQP.Width = 120;

                cAfterEQP.FillWeight = 120F;
                //cAfterEQP.Frozen = true;
                cAfterEQP.HeaderText = "AfterEQP";
                cAfterEQP.MaxInputLength = 28;
                cAfterEQP.MinimumWidth = 120;
                cAfterEQP.Name = "cAfterEQP";
                cAfterEQP.ReadOnly = true;
                cAfterEQP.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cAfterEQP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cAfterEQP.Width = 120;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// ECID History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForECID()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                System.Windows.Forms.DataGridViewTextBoxColumn cType = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cTime = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cOperation = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cUserID = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cItem = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cECWLL = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cECDEF = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cECWUL = new DataGridViewTextBoxColumn();


                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cType, cTime, cOperation, cUserID, cItem, cECWLL, cECDEF ,cECWUL });


                cType.FillWeight = 120F;
                cType.Frozen = true;
                cType.HeaderText = "Type";
                cType.MaxInputLength = 14;
                cType.MinimumWidth = 110;
                cType.Name = "cType";
                cType.ReadOnly = true;
                cType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cType.Width = 110;
                cType.Visible = false;

                cTime.FillWeight = 120F;
                cTime.Frozen = true;
                cTime.HeaderText = "Time";
                cTime.MaxInputLength = 14;
                cTime.MinimumWidth = 110;
                cTime.Name = "cTime";
                cTime.ReadOnly = true;
                cTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cTime.Width = 110;

                cOperation.FillWeight = 120F;
                cOperation.Frozen = true;
                cOperation.HeaderText = "Operation";
                cOperation.MaxInputLength = 14;
                cOperation.MinimumWidth = 110;
                cOperation.Name = "cOperation";
                cOperation.ReadOnly = true;
                cOperation.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cOperation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cOperation.Width = 110;

                cUserID.FillWeight = 120F;
                cUserID.Frozen = true;
                cUserID.HeaderText = "UserID";
                cUserID.MaxInputLength = 25;
                cUserID.MinimumWidth = 110;
                cUserID.Name = "cUserID";
                cUserID.ReadOnly = true;
                cUserID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cUserID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cUserID.Width = 110;

                cItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cItem.FillWeight = 120F;
                cItem.HeaderText = "Item(ECID)";
                cItem.MaxInputLength = 40;
                cItem.MinimumWidth = 400;
                cItem.Name = "cECID";
                cItem.ReadOnly = true;
                cItem.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cItem.Width = 160;

                cECWLL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cECWLL.FillWeight = 120F;
                cECWLL.HeaderText = "Item(ECWLL)";
                cECWLL.MaxInputLength = 16;
                cECWLL.MinimumWidth = 110;
                cECWLL.Name = "cECWLL";
                cECWLL.ReadOnly = true;
                cECWLL.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cECWLL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cECWLL.Width = 160;

                cECDEF.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cECDEF.FillWeight = 120F;
                cECDEF.HeaderText = "Item(ECDEF)";
                cECDEF.MaxInputLength = 16;
                cECDEF.MinimumWidth = 110;
                cECDEF.Name = "cECDEF";
                cECDEF.ReadOnly = true;
                cECDEF.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cECDEF.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cECDEF.Width = 160;

                cECWUL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cECWUL.FillWeight = 120F;
                cECWUL.HeaderText = "Item(ECWUL)";
                cECWUL.MaxInputLength = 16;
                cECWUL.MinimumWidth = 110;
                cECWUL.Name = "cECWUL";
                cECWUL.ReadOnly = true;
                cECWUL.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cECWUL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cECWUL.Width = 160;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// EOID History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForEOID()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                System.Windows.Forms.DataGridViewTextBoxColumn cType = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cTime = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cOperation = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cUserID = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cItem = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cBefore = new DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cAfter = new DataGridViewTextBoxColumn();

                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cType, cTime, cOperation, cUserID, cItem, cBefore, cAfter } );


                cType.FillWeight = 120F;
                cType.Frozen = true;
                cType.HeaderText = "Type";
                cType.MaxInputLength = 14;
                cType.MinimumWidth = 110;
                cType.Name = "cType";
                cType.ReadOnly = true;
                cType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cType.Width = 110;
                cType.Visible = false;

                cTime.FillWeight = 120F;
                cTime.Frozen = true;
                cTime.HeaderText = "Time";
                cTime.MaxInputLength = 14;
                cTime.MinimumWidth = 110;
                cTime.Name = "cTime";
                cTime.ReadOnly = true;
                cTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cTime.Width = 110;

                cOperation.FillWeight = 120F;
                cOperation.Frozen = true;
                cOperation.HeaderText = "Operation";
                cOperation.MaxInputLength = 14;
                cOperation.MinimumWidth = 110;
                cOperation.Name = "cOperation";
                cOperation.ReadOnly = true;
                cOperation.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cOperation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cOperation.Width = 110;
                cOperation.Visible = false;

                cUserID.FillWeight = 120F;
                cUserID.Frozen = true;
                cUserID.HeaderText = "UserID";
                cUserID.MaxInputLength = 25;
                cUserID.MinimumWidth = 110;
                cUserID.Name = "cUserID";
                cUserID.ReadOnly = true;
                cUserID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cUserID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cUserID.Width = 110;

                cItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cItem.FillWeight = 120F;
                cItem.HeaderText = "Item(EOID)";
                cItem.MaxInputLength = 14;
                cItem.MinimumWidth = 110;
                cItem.Name = "cEOID";
                cItem.ReadOnly = true;
                cItem.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cItem.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cItem.Width = 160;

                cBefore.FillWeight = 120F;
                cBefore.Frozen = true;
                cBefore.HeaderText = "BeforeParam";
                cBefore.MaxInputLength = 14;
                cBefore.MinimumWidth = 110;
                cBefore.Name = "cBefore";
                cBefore.ReadOnly = true;
                cBefore.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cBefore.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cBefore.Width = 110;

                cAfter.FillWeight = 120F;
                cAfter.Frozen = true;
                cAfter.HeaderText = "AfterParam";
                cAfter.MaxInputLength = 14;
                cAfter.MinimumWidth = 110;
                cAfter.Name = "cAfter";
                cAfter.ReadOnly = true;
                cAfter.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cAfter.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cAfter.Width = 110;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }
        #endregion

        /// <summary>
        /// Alarm History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForAlarm()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                System.Windows.Forms.DataGridViewTextBoxColumn cTime;
                System.Windows.Forms.DataGridViewTextBoxColumn cModuleID;
                System.Windows.Forms.DataGridViewTextBoxColumn cIsSet;
                System.Windows.Forms.DataGridViewTextBoxColumn cAlarmCode;
                System.Windows.Forms.DataGridViewTextBoxColumn cCode;
                System.Windows.Forms.DataGridViewTextBoxColumn cIsHeavy;
                System.Windows.Forms.DataGridViewTextBoxColumn cDescription;


                cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cModuleID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cIsSet = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cAlarmCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cIsHeavy = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();


                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cTime, cModuleID, cIsSet, cAlarmCode, cCode, cIsHeavy, cDescription });

                cTime.FillWeight = 120F;
                cTime.Frozen = true;
                cTime.HeaderText = "Time";
                cTime.MaxInputLength = 19 ;
                cTime.MinimumWidth = 140;
                cTime.Name = "cTime";
                cTime.ReadOnly = true;
                cTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cTime.Width = 140;

                cModuleID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
                cModuleID.FillWeight = 160F;
                cModuleID.Frozen = true;
                cModuleID.HeaderText = "UnitID";
                cModuleID.MaxInputLength = 20;
                cModuleID.MinimumWidth = 110;
                cModuleID.Name = "cModuleID";
                cModuleID.ReadOnly = true;
                cModuleID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cModuleID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cModuleID.Width = 110;

                cIsSet.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cIsSet.FillWeight = 70F;
                cIsSet.Frozen = true;
                cIsSet.HeaderText = "Set/Reset";
                cIsSet.MaxInputLength = 1;
                cIsSet.MinimumWidth = 70;
                cIsSet.Name = "cIsSet";
                cIsSet.ReadOnly = true;
                cIsSet.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cIsSet.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cIsSet.Width = 70;

                cAlarmCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cAlarmCode.FillWeight = 60F;
                cAlarmCode.Frozen = true;
                cAlarmCode.HeaderText = "AlarmID";
                cAlarmCode.MaxInputLength = 4;
                cAlarmCode.MinimumWidth = 60;
                cAlarmCode.Name = "cAlarmCode";
                cAlarmCode.ReadOnly = true;
                cAlarmCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cAlarmCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cAlarmCode.Width = 60;

                cCode.Frozen = true;
                cCode.HeaderText = "Code";
                cCode.MaxInputLength = 1;
                cCode.MinimumWidth = 60;
                cCode.Name = "cCode";
                cCode.ReadOnly = true;
                cCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cCode.Width = 60;

                cIsHeavy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cIsHeavy.FillWeight = 60F;
                cIsHeavy.Frozen = true;
                cIsHeavy.HeaderText = "Type";
                cIsHeavy.MaxInputLength = 1;
                cIsHeavy.MinimumWidth = 60;
                cIsHeavy.Name = "cIsHeavy";
                cIsHeavy.ReadOnly = true;
                cIsHeavy.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cIsHeavy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cIsHeavy.Width = 60;

                cDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cDescription.HeaderText = "Description";
                cDescription.MaxInputLength = 100;
                cDescription.MinimumWidth = 100;
                cDescription.Name = "cDescription";
                cDescription.ReadOnly = true;
                cDescription.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;



            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Host Message (OP Call, Terminal Message) 
        /// History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForHOSTMSG()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                System.Windows.Forms.DataGridViewTextBoxColumn cTime;
                System.Windows.Forms.DataGridViewTextBoxColumn cIsOPCall;
                System.Windows.Forms.DataGridViewTextBoxColumn cMessage;

                cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cIsOPCall = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();

                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cTime, cIsOPCall, cMessage });

                cTime.FillWeight = 120F;
                cTime.Frozen = true;
                cTime.HeaderText = "Time";
                cTime.MaxInputLength = 19;
                cTime.MinimumWidth = 130;
                cTime.Name = "cTime";
                cTime.ReadOnly = true;
                cTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cTime.Width = 130;

                cIsOPCall.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cIsOPCall.FillWeight = 60F;
                cIsOPCall.Frozen = true;
                cIsOPCall.HeaderText = "Type";
                cIsOPCall.MaxInputLength = 10;
                cIsOPCall.MinimumWidth = 80;
                cIsOPCall.Name = "cIsOPCall";
                cIsOPCall.ReadOnly = true;
                cIsOPCall.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cIsOPCall.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cIsOPCall.Width = 80;

                cMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cMessage.HeaderText = "Message";
                cMessage.MaxInputLength = 1000;
                cMessage.MinimumWidth = 100;
                cMessage.Name = "cMessage";
                cMessage.ReadOnly = true;
                cMessage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Parameter History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForPARAMETER()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                this.cbSubType.Items.AddRange(new object[] {"EOID", "ECID", "PPID"});
                this.cbSubType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Glass APD History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForGLSAPD()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                // 0    , 1   , 2      , 3     , 4     , 5       ,    6 Start =>
                // LOTID, PPID, GLASSID, SLOTNO, STEPID, EQPSTATE,    실 PDC Data
                
                System.Windows.Forms.DataGridViewTextBoxColumn cLotID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cPPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cGlassID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cSlotNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cStepID = new System.Windows.Forms.DataGridViewTextBoxColumn();


                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cLotID, cPPID, cGlassID, cSlotNO, cStepID });

                cLotID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cLotID.FillWeight = 120F;
                cLotID.Frozen = true;
                cLotID.HeaderText = "LOT ID";
                cLotID.MaxInputLength = 16;
                cLotID.MinimumWidth = 120;
                cLotID.Name = "cLotID";
                cLotID.ReadOnly = true;
                cLotID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cLotID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cLotID.Width = 120;

                cPPID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cPPID.FillWeight = 60F;
                cPPID.Frozen = true;
                cPPID.HeaderText = "PPID";
                cPPID.MaxInputLength = 20;
                cPPID.MinimumWidth = 135;
                cPPID.Name = "cPPID";
                cPPID.ReadOnly = true;
                cPPID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cPPID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cPPID.Width = 135;

                cGlassID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cGlassID.HeaderText = "Glass ID";
                cGlassID.MaxInputLength = 16;
                cGlassID.MinimumWidth = 120;
                cGlassID.Name = "cGlassID";
                cGlassID.ReadOnly = true;
                cGlassID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cGlassID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

                cSlotNO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cSlotNO.FillWeight = 120F;
                cSlotNO.HeaderText = "Slot NO";
                cSlotNO.MaxInputLength = 2;
                cSlotNO.MinimumWidth = 65;
                cSlotNO.Name = "cSlotNO";
                cSlotNO.ReadOnly = true;
                cSlotNO.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cSlotNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cSlotNO.Width = 65;

                cStepID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cStepID.FillWeight = 120F;
                cStepID.HeaderText = "Step ID";
                cStepID.MaxInputLength = 8;
                cStepID.MinimumWidth = 90;
                cStepID.Name = "cStepID";
                cStepID.ReadOnly = true;
                cStepID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cStepID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cStepID.Width = 90;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Lot APD History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForLOTAPD()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                // 0    , 1   , 2      , 3     , 4     , 5       ,    6 Start =>
                // LOTID, PPID, GLASSID, SLOTNO, STEPID, EQPSTATE,    실 PDC Data

                System.Windows.Forms.DataGridViewTextBoxColumn cLotID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cPPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cGlassID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cSlotNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cStepID = new System.Windows.Forms.DataGridViewTextBoxColumn();


                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cLotID, cPPID, cGlassID, cSlotNO, cStepID });

                //cLotID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cLotID.FillWeight = 120F;
                cLotID.Frozen = false;
                cLotID.HeaderText = "LOT ID";
                cLotID.MaxInputLength = 16;
                cLotID.MinimumWidth = 120;
                cLotID.Name = "cLotID";
                cLotID.ReadOnly = true;
                cLotID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cLotID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cLotID.Width = 120;

                cPPID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cPPID.FillWeight = 60F;
                cPPID.Frozen = false;
                cPPID.HeaderText = "PPID";
                cPPID.MaxInputLength = 20;
                cPPID.MinimumWidth = 135;
                cPPID.Name = "cPPID";
                cPPID.ReadOnly = true;
                cPPID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cPPID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cPPID.Width = 135;

                cGlassID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cGlassID.HeaderText = "Glass ID";
                cGlassID.MaxInputLength = 16;
                cGlassID.MinimumWidth = 120;
                cGlassID.Name = "cGlassID";
                cGlassID.ReadOnly = true;
                cGlassID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cGlassID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cGlassID.Visible = false;

                cSlotNO.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                cSlotNO.FillWeight = 120F;
                cSlotNO.HeaderText = "Slot NO";
                cSlotNO.MaxInputLength = 2;
                cSlotNO.MinimumWidth = 65;
                cSlotNO.Name = "cSlotNO";
                cSlotNO.ReadOnly = true;
                cSlotNO.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cSlotNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cSlotNO.Width = 65;
                cSlotNO.Visible = false;

                cStepID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                cStepID.FillWeight = 120F;
                cStepID.HeaderText = "Step ID";
                cStepID.MaxInputLength = 8;
                cStepID.MinimumWidth = 90;
                cStepID.Name = "cStepID";
                cStepID.ReadOnly = true;
                cStepID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cStepID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cStepID.Width = 90;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// SCRAP History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForSCRAP()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                System.Windows.Forms.DataGridViewTextBoxColumn cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cLotID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cSlotNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cGlassID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cModuleID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cComment = new System.Windows.Forms.DataGridViewTextBoxColumn();

                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cType, cTime, cLotID, cSlotNO, cGlassID, cModuleID, cComment });

                cType.FillWeight = 120F;
                cType.HeaderText = "Type";
                cType.MaxInputLength = 19;
                cType.MinimumWidth = 80;
                cType.Name = "cType";
                cType.ReadOnly = true;
                cType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cType.Width = 80;

                cTime.FillWeight = 120F;
                cTime.HeaderText = "Time";
                cTime.MaxInputLength = 14;
                cTime.MinimumWidth = 110;
                cTime.Name = "cTime";
                cTime.ReadOnly = true;
                cTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cTime.Width = 110;

                cLotID.FillWeight = 120F;
                cLotID.HeaderText = "LOT ID";
                cLotID.MaxInputLength = 16;
                cLotID.MinimumWidth = 120;
                cLotID.Name = "cLotID";
                cLotID.ReadOnly = true;
                cLotID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cLotID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cLotID.Width = 120;

                cSlotNO.FillWeight = 120F;
                cSlotNO.HeaderText = "Slot NO";
                cSlotNO.MaxInputLength = 2;
                cSlotNO.MinimumWidth = 65;
                cSlotNO.Name = "cSlotNO";
                cSlotNO.ReadOnly = true;
                cSlotNO.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cSlotNO.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cSlotNO.Width = 65;

                cGlassID.HeaderText = "Glass ID";
                cGlassID.MaxInputLength = 16;
                cGlassID.MinimumWidth = 120;
                cGlassID.Name = "cGlassID";
                cGlassID.ReadOnly = true;
                cGlassID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cGlassID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

                cModuleID.FillWeight = 160F;
                cModuleID.HeaderText = "UnitID";
                cModuleID.MaxInputLength = 28;
                cModuleID.MinimumWidth = 160;
                cModuleID.Name = "cModuleID";
                cModuleID.ReadOnly = true;
                cModuleID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cModuleID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cModuleID.Width = 160;

                cComment.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                cComment.FillWeight = 160F;
                cComment.Frozen = true;
                cComment.HeaderText = "Comment";
                cComment.MaxInputLength = 100;
                cComment.MinimumWidth = 160;
                cComment.Name = "cComment";
                cComment.ReadOnly = true;
                cComment.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cComment.Width = 160;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        private void subSetGLSInOut()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                System.Windows.Forms.DataGridViewTextBoxColumn cTime;
                System.Windows.Forms.DataGridViewTextBoxColumn cLOTID;
                System.Windows.Forms.DataGridViewTextBoxColumn cSlotID;
                System.Windows.Forms.DataGridViewTextBoxColumn cDESC;
                System.Windows.Forms.DataGridViewTextBoxColumn cUnitID;

                cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cLOTID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                cSlotID= new System.Windows.Forms.DataGridViewTextBoxColumn();
                cDESC = new DataGridViewTextBoxColumn();
                cUnitID = new DataGridViewTextBoxColumn();

                this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cTime, cLOTID, cSlotID, cDESC });
                
                cTime.FillWeight = 120F;
                cTime.Frozen = true;
                cTime.HeaderText = "Time";
                cTime.MaxInputLength = 19;
                cTime.MinimumWidth = 130;
                cTime.Name = "cTime";
                cTime.ReadOnly = true;
                cTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cTime.Width = 130;

                cLOTID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
                cLOTID.FillWeight = 120F;
                cLOTID.Frozen = true;
                cLOTID.HeaderText = "GlassID";
                cLOTID.MaxInputLength = 20;
                cLOTID.MinimumWidth = 180;
                cLOTID.Name = "cLOTID";
                cLOTID.ReadOnly = true;
                cLOTID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cLOTID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cLOTID.Width = 180;

                cSlotID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
                cSlotID.FillWeight = 80F;
                cSlotID.Frozen = true;
                cSlotID.HeaderText = "SlotNo";
                cSlotID.MaxInputLength = 10;
                cSlotID.MinimumWidth = 70;
                cSlotID.Name = "cSlotID";
                cSlotID.ReadOnly = true;
                cSlotID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cSlotID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cSlotID.Width = 70;

                cDESC.HeaderText = "DESC";
                cDESC.MaxInputLength = 100;
                cDESC.MinimumWidth = 300;
                cDESC.Name = "cDESC";
                cDESC.ReadOnly = true;
                cDESC.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cDESC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cDESC.Width = 300;
                cDESC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            }
            catch (Exception ex)
            {
                if (this.PInfo != null) PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }


        /// <summary>
        /// APC History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForAPC()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// PPC History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForPPC()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// RPC History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForRPC()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }


       

        /// <summary>
        /// 왜 있는거지?
        /// </summary>
        public void funInitializeForm()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// File Open Status Panel Setting
        /// </summary>
        private void subSetFileOpenPanel()
        {
            try
            {
                this.pnlFileOpen = new System.Windows.Forms.Panel();
                this.lbText = new System.Windows.Forms.Label();
                this.pgb = new System.Windows.Forms.ProgressBar();


                this.pnlFileOpen.SuspendLayout();

                // 
                // pnlFileOpen
                // 
                this.pnlFileOpen.BackColor = System.Drawing.SystemColors.Control;
                this.pnlFileOpen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.pnlFileOpen.Controls.Add(this.lbText);
                this.pnlFileOpen.Controls.Add(this.pgb);
                this.pnlFileOpen.MaximumSize = new System.Drawing.Size(280, 50);
                this.pnlFileOpen.MinimumSize = new System.Drawing.Size(280, 50);
                this.pnlFileOpen.Name = "pnlFileOpen";
                this.pnlFileOpen.Padding = new System.Windows.Forms.Padding(6);
                this.pnlFileOpen.Size = new System.Drawing.Size(280, 50);
                this.pnlFileOpen.Visible = false;
                // 
                // lbText
                // 
                this.lbText.AutoSize = true;
                this.lbText.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lbText.Location = new System.Drawing.Point(6, 3);
                this.lbText.Name = "lbText";
                this.lbText.Size = new System.Drawing.Size(100, 18);
                this.lbText.TabIndex = 1;
                this.lbText.Text = "Please wait...";
                // 
                // pgb
                // 
                this.pgb.BackColor = System.Drawing.SystemColors.ControlDark;
                this.pgb.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.pgb.Location = new System.Drawing.Point(6, 23);
                this.pgb.MarqueeAnimationSpeed = 20;
                this.pgb.Name = "pgb";
                this.pgb.Size = new System.Drawing.Size(266, 19);
                this.pgb.Step = 100;
                this.pgb.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
                this.pgb.TabIndex = 0;


                this.pnlFileOpen.ResumeLayout(false);
                this.pnlFileOpen.PerformLayout();

                this.dgv.Controls.Add(this.pnlFileOpen);

                this.pnlFileOpen.BringToFront();

                this.pnlFileOpen.Location = new Point((this.dgv.Width / 2) - (this.pnlFileOpen.Width / 2), (this.dgv.Height / 2) - (this.pnlFileOpen.Height / 2));
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Tree View Visible State 변경시 발생되는 이벤트
        /// 해당 LOG 타입의 파일이 있는 경우에만 Tree View에 날짜가 추가된다.
        /// </summary>
        private void tvDate_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(this.PInfo.All.LogFilePath)) return;

                string[] dstrDirectiory = Directory.GetDirectories(this.PInfo.All.LogFilePath);

                DateTime dtTemp;

                for (int dintLoop = 0; dintLoop < dstrDirectiory.Length; dintLoop++)
                {
                    string strTemp = dstrDirectiory[dintLoop].Substring(dstrDirectiory[dintLoop].LastIndexOf('\\') + 1);

                    if (DateTime.TryParseExact(strTemp, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out dtTemp))
                    {
                        string dstrYear = strTemp.Substring(0, 4);
                        string dstrMonth = strTemp.Substring(4, 2);
                        string dstrDay = strTemp.Substring(6, 2);


                        if (!this.tvDate.Nodes.ContainsKey(dstrYear)) this.tvDate.Nodes.Add(dstrYear, dstrYear);

                        TreeNode tnYearNode = this.tvDate.Nodes[dstrYear];

                        if (!tnYearNode.Nodes.ContainsKey(dstrMonth)) tnYearNode.Nodes.Add(dstrMonth, dstrMonth);

                        TreeNode tnMonthNode = tnYearNode.Nodes[dstrMonth];

                        if (File.Exists(dstrDirectiory[dintLoop] + pstrLogFileName) && !tnMonthNode.Nodes.ContainsKey(dstrDay)) tnMonthNode.Nodes.Add(dstrDay, dstrDay);

                        if (tnMonthNode.FirstNode == null) tnMonthNode.Remove();
                        if (tnYearNode.FirstNode == null) tnYearNode.Remove();
                    }
                }

            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }



        /// <summary>
        /// Tree View의 날짜 노드를 더블클릭했을때 발생하는 이벤트
        /// 해당 로그 파일의 데이터를 pstrData에 로드 한다.
        /// </summary>
        private void tvDate_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeNode tnNode = ((TreeView)sender).SelectedNode;

                if (tnNode == null)return;

                if (tnNode.Level == 2)
                {
                    if (!File.Exists(this.PInfo.All.LogFilePath + "\\" + tnNode.FullPath.Replace("/", "") + pstrLogFileName))
                    {
                        tnNode.Remove();
                        // 보관 기간 만료등의 이유로 해당 로그 파일이 삭제 되었습니다.
                        MessageBox.Show("보관 기간 만료등의 이유로 해당 로그 파일이 삭제 되었습니다.", "INFORM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    if (tnNode.FirstNode == null) tnNode.Remove();
                    return;
                }


                

                if (this.bgWorker.IsBusy)
                {
                    // 작업중 메시지
                    MessageBox.Show("파일을 열고 있습니다.\n잠시 후 다시 시도해 주세요.", "INFORM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    this.pnlFileOpen.Visible = true;

                    subGridClear();
                    this.pstrDateString = this.tvDate.SelectedNode.FullPath.Replace("/", "");
                    bgWorker.RunWorkerAsync();
                } 

            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Data Grid View, pstrData의 정보를 삭제 한다.
        /// </summary>
        private void subGridClear()
        {
            try
            {
                this.dgv.RowCount = 0;
                this.dgv.HorizontalScrollingOffset = 0;

                this.pstrData.Clear();
                this.pstrData.TrimToSize();
            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Command Button 중 Clear 버튼 클릭시 발생되는 이벤트
        /// Data Grid View, pstrData의 정보를 삭제 한다.
        /// </summary>
        public void Clear()
        {
            try
            {
                if (this.bgWorker.IsBusy)
                {
                    // 작업중 메시지
                    MessageBox.Show("파일을 열고 있습니다.\n잠시 후 다시 시도해 주세요.", "INFORM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (this.pfrmPopup != null && this.pfrmPopup.Visible) this.pfrmPopup.Close();

                    this.tvDate.CollapseAll();
                    this.tvDate.SelectedNode = null;
                    this.lbDate.Text = "Select Date";

                    subGridClear();

                    this.btnNext.Enabled = false;
                    this.btnPrev.Enabled = false;
                    this.lbPage.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }

        }

        /// <summary>
        /// Control의 DoubleBuffer 설정
        /// </summary>
        /// <param name="control">DoubleBuffer 설정할 Control</param>
        private void funSetDoubleBuffered(Control control)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                                          BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                                           null, control, new object[] { true });
        }

        /// <summary>
        /// 디스플레이 데이터의 페이지 변경 버튼 이벤트
        /// 디스플레이 기준점을 첫 페이지로 이동시키고 DataGridView를 Refresh 한다.
        /// </summary>
        private void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnNext.Enabled = false;
                this.btnPrev.Enabled = false;
                this.btnFirst.Enabled = false;
                this.btnLast.Enabled = false;

                this.pintFirstRowIndex = 0;

                this.dgv.Refresh();
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                subSetPageControl();
            }
        }

        /// <summary>
        /// 디스플레이 데이터의 페이지 변경 버튼 이벤트
        /// 디스플레이 기준점을 마지막 페이지로 이동시키고 DataGridView를 Refresh 한다.
        /// </summary>
        private void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnNext.Enabled = false;
                this.btnPrev.Enabled = false;
                this.btnFirst.Enabled = false;
                this.btnLast.Enabled = false;

                this.pintFirstRowIndex = (int)((this.pstrData.Count / intDisplayRowCount) * intDisplayRowCount);//+ (((this.pstrData.Count % 20) != 0) ? 1 : 0)));

                this.dgv.Refresh();
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                subSetPageControl();
            }
        }

        /// <summary>
        /// 디스플레이 데이터의 페이지 변경 버튼 이벤트
        /// 디스플레이 기준점을 증가 시키고 DataGridView를 Refresh 한다.
        /// </summary>
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnNext.Enabled = false;
                this.btnPrev.Enabled = false;
                this.btnFirst.Enabled = false;
                this.btnLast.Enabled = false;

                this.pintFirstRowIndex += intDisplayRowCount;

                this.dgv.Refresh();
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                subSetPageControl();
            }
        }

        /// <summary>
        /// 디스플레이 데이터의 페이지 변경 버튼 이벤트
        /// 디스플레이 기준점을 감소 시키고 DataGridView를 Refresh 한다.
        /// </summary>
        private void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnPrev.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnFirst.Enabled = false;
                this.btnLast.Enabled = false;

                this.pintFirstRowIndex -= intDisplayRowCount;

                this.dgv.Refresh();
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
            finally
            {
                subSetPageControl();
            }
        }

        /// <summary>
        /// 데이터 디스플레이 페이지 변경 후
        /// 이전페이지, 다음페이지 변경 버튼 및 현재 페이지 디스플레이 상태를 변경한다.
        /// </summary>
        private void subSetPageControl()
        {
            try
            {
                if (this.pstrData.Count == 0)
                {
                    this.lbPage.Text = string.Empty;
                    this.btnNext.Enabled = false;
                    this.btnPrev.Enabled = false;
                }
                else
                {
                    this.lbPage.Text = string.Format("{0}/{1}", (pintFirstRowIndex / intDisplayRowCount) + 1, ((int)((this.pstrData.Count / intDisplayRowCount) + (((this.pstrData.Count % intDisplayRowCount) != 0) ? 1 : 0))));

                    if ((pintFirstRowIndex / intDisplayRowCount) == 0)
                    {
                        this.btnPrev.Enabled = false;
                        this.btnFirst.Enabled = false;
                    }
                    else
                    {
                        this.btnPrev.Enabled = true;
                        this.btnFirst.Enabled = true;
                    }

                    if ((pintFirstRowIndex / intDisplayRowCount) + 1 == ((int)((this.pstrData.Count / intDisplayRowCount) + (((this.pstrData.Count % intDisplayRowCount) != 0) ? 1 : 0))))
                    {
                        this.btnNext.Enabled = false;
                        this.btnLast.Enabled = false;
                    }
                    else
                    {
                        this.btnNext.Enabled = true;
                        this.btnLast.Enabled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }

        }

        /// <summary>
        /// BackGround Worker의 작업
        /// 해당 날짜의 파일 내용을 pstrData에 로드한다.
        /// </summary>
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string dstrFilePath;                        //파일의 경로 저장
            System.IO.FileStream fs = null;
            System.IO.StreamReader sr = null;

            try
            {
                this.Invoke(new MethodInvoker(delegate()
                {
                    //dstrFilePath = this.PInfo.All.LogFilePath + "\\" + this.tvDate.SelectedNode.FullPath.Replace("/", "") + pstrLogFileName;
                    dstrFilePath = this.PInfo.All.LogFilePath + "\\" + this.pstrDateString + pstrLogFileName;   // 20130321 이상창님


                    //파일의 모든 라인을 가져와 스트링 배열에 저장한다(라인별 내용을 배열에 저장)
                    if (File.Exists(dstrFilePath) == true)
                    {
                        fs = new FileStream(dstrFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        sr = new StreamReader(fs);

                        while (!sr.EndOfStream)
                        {
                            string dstrTemp = sr.ReadLine();
                            if (HistoryType.Cleaner_DV == enmCurHistoryType || HistoryType.Oven_DV == enmCurHistoryType)
                            {
                                dstrTemp = dstrTemp.Substring(11);
                            }

                            if (!string.IsNullOrEmpty(dstrTemp))
                            {
                                if (this.enmCurHistoryType == HistoryType.PARAMETER)
                                {
                                    if (dstrTemp.StartsWith(this.pstrSubTypeNow)) this.pstrData.Add(dstrTemp);
                                }
                                else
                                {
                                    if (cbSubType.Visible)
                                    {
                                        string[] arrData = dstrTemp.Split(',');

                                        if (txtGLSID.Text == "" || txtGLSID.Text == null)
                                        {
                                            if (cbSubType.Text == "ALL")
                                            {
                                                this.pstrData.Add(dstrTemp);
                                            }
                                            else if (arrData[4] == cbSubType.SelectedIndex.ToString())
                                            {
                                                this.pstrData.Add(dstrTemp);
                                            }
                                        }
                                        else if (arrData[1].ToUpper().Trim().Contains(txtGLSID.Text.ToUpper().Trim()))
                                        {
                                            if (cbSubType.Text.Trim() == "ALL")
                                            {
                                                this.pstrData.Add(dstrTemp);
                                            }
                                            else if (arrData[4] == cbSubType.SelectedIndex.ToString())
                                            {
                                                this.pstrData.Add(dstrTemp);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (this.enmCurHistoryType == HistoryType.Oven_DV || this.enmCurHistoryType == HistoryType.Cleaner_DV)
                                        {
                                            if (string.IsNullOrEmpty(txtGLSID.Text))
                                            {
                                                this.pstrData.Add(dstrTemp);//.Split(','));
                                            }
                                            else
                                            {
                                                string[] arrData = dstrTemp.Split(',');
                                                if (arrData[1].Trim().Contains(txtGLSID.Text.Trim()))
                                                {
                                                    this.pstrData.Add(dstrTemp);//.Split(','));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            this.pstrData.Add(dstrTemp);//.Split(','));
                                        }
                                    }
                                }
                            }
                        }

                        this.pstrData.Reverse();
                    }
                }));
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
                this.bgWorker.CancelAsync();
                subGridClear();
                this.pnlFileOpen.Visible = false;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
            }
        }

        /// <summary>
        /// BackGround Worker의 작업이 완료되었을때 실행
        /// </summary>
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (this.pstrData != null)
                {
                    this.pintFirstRowIndex = 0;
                    this.pstrData.TrimToSize();
                    subSetPageControl();


                    this.dgv.RowCount = intDisplayRowCount;
                }

                this.pnlFileOpen.Visible = false;


                //검색결과가 없을 경우 메세지 출력
                if (this.pstrData != null && this.pstrData.Count == 0)
                {
                    MessageBox.Show("History not exist !!!", "INFORM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //this.lbDate.Text = "Select Date";
                }
                
                this.lbDate.Text = this.tvDate.SelectedNode.FullPath;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Data GridView의 내부 이벤트
        /// 해당 셀의 값을 다시 그려야 할 경우 발생됨
        /// 디스플레이 기준점에 따라 해당셀의 데이터를 디스플레이 한다.
        /// </summary>
        private void dgv_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            try
            {
                if (this.dgv.RowCount == 0 || this.pstrData.Count == 0) return;

                if (e.RowIndex + pintFirstRowIndex >= this.pstrData.Count) return;

                e.Value = ((string[])(this.pstrData[e.RowIndex + pintFirstRowIndex].ToString()).Split(','))[e.ColumnIndex];
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// 데이터 셀을 디스플레이 했을때 발생됨
        /// 데이터 상세 내용을 보여줄 Popup 창을 띄운다.
        /// </summary>
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.RowIndex + this.pintFirstRowIndex >= this.pstrData.Count) return;

                string[] strTemp = (string[])((string)this.pstrData[e.RowIndex + this.pintFirstRowIndex]).Split(',');

                switch (this.enmCurHistoryType)
                {
                    case HistoryType.ALARM:
                        //if (!strTemp[2].ToUpper().Equals("SET")) return;
                        //if (this.pfrmPopup == null || this.pfrmPopup.IsDisposed) this.pfrmPopup = new frmLogViewAlarm();
                        //((frmLogViewAlarm)this.pfrmPopup).subFormLoad(strTemp[0], Convert.ToInt32(strTemp[3]));
                        break;

                    //case HistoryType.HOSTMSG:
                    //    if (this.pfrmPopup == null || this.pfrmPopup.IsDisposed) this.pfrmPopup = new frmLogViewHostMSG();
                    //    ((frmLogViewHostMSG)this.pfrmPopup).subFormLoad(strTemp[1], strTemp[0], strTemp[2]);
                    //    break;

                    case HistoryType.PARAMETER:
                        //if (this.pstrSubTypeNow.Equals("EOID"))
                        //{
                        //    // 자세한 정보 필요 없어 보임
                        //}
                        //else if (this.pstrSubTypeNow.Equals("ECID"))
                        //{
                        //    if (this.pfrmPopup == null || this.pfrmPopup.IsDisposed) this.pfrmPopup = new frmLogViewECID();
                        //    ((frmLogViewECID)this.pfrmPopup).subFormLoad(strTemp);
                        //}
                        //else if (this.pstrSubTypeNow.Equals("PPID"))
                        //{
                        //    if (strTemp[4].StartsWith("HOST") || strTemp[4].Contains("삭제")) return;
                        //    if (this.pfrmPopup == null || this.pfrmPopup.IsDisposed) this.pfrmPopup = new frmLogViewPPID();
                        //    ((frmLogViewPPID)this.pfrmPopup).subFormLoad(strTemp);
                        //}
                        break;

                    //case HistoryType.GLS_APD:
                    //    if (this.pfrmPopup == null || this.pfrmPopup.IsDisposed) this.pfrmPopup = new frmLogViewAPD();
                    //    ((frmLogViewAPD)this.pfrmPopup).subFormLoad(true, strTemp);
                    //    break;

                    //case HistoryType.LOT_APD:
                    //    if (this.pfrmPopup == null || this.pfrmPopup.IsDisposed) this.pfrmPopup = new frmLogViewAPD();
                    //    ((frmLogViewAPD)this.pfrmPopup).subFormLoad(false, strTemp);
                    //    break;

                    case HistoryType.SCRAP:
                        // 자세한 정보 필요 없어 보임
                        break;

                    default:

                        break;

                }
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(InfoAct.clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// Command Button 중 Save 버튼 클릭시 발생되는 이벤트
        /// </summary>
        public void Save()
        {
            try
            {
                // CSV로 저장할까..?
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }

        }

        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            cbSubType_SelectedValueChanged(sender, e);
        }
        
    }
}

