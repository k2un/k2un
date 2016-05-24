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
    public partial class tabfrmHistoryGroup : UserControl
    {
        #region Fields
        public clsInfo PInfo = clsInfo.Instance;
        
        
        private int pintFirstRowIndex = 0;
        private ArrayList pstrData = new ArrayList();

        private Form pfrmPopup;

        private string pstrSubTypeNow = string.Empty;
        private string pstrLogFileName = string.Empty;

        private HistoryType enmCurHistoryType = HistoryType.NONE;


        private System.Windows.Forms.Panel pnlFileOpen;
        private System.Windows.Forms.Label lbText;
        private System.Windows.Forms.ProgressBar pgb;


        #endregion

        #region Properties
        public HistoryType CurrentHistoryType
        {
            get { return enmCurHistoryType; }
        }
       
        #endregion

        #region Constructors
        public tabfrmHistoryGroup()
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
                    case HistoryType.APC:
                        pstrLogFileName = "\\APC.Log";
                        subSetForXPC();
                        break;

                    case HistoryType.PPC:
                        pstrLogFileName = "\\PPC.Log";
                        subSetForXPC();
                        break;

                    case HistoryType.RPC:
                        pstrLogFileName = "\\RPC.Log";
                        subSetForXPC();
                        break;
                    case HistoryType.PARAMETER:
                        pstrLogFileName = "\\Parameter.Log";
                        
                        break;
                    case HistoryType.NONE:
                        pstrLogFileName = "\\";
                        // 화면에 설정 잘못된거 알려주면 되나?
                        break;
                }

                this.tvDate.Visible = true;
            }
            catch (Exception ex)
            {
                if (this.PInfo != null) this.PInfo.subLog_Set(clsInfo.LogType.CIM, ex.ToString());
            }
        }

        /// <summary>
        /// XPC History View용 DataGridView Column Setting
        /// </summary>
        private void subSetForXPC()
        {
            try
            {
                if (this.dgv.Columns.Count > 0) this.dgv.Columns.Clear();

                System.Windows.Forms.DataGridViewTextBoxColumn cTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cType = new System.Windows.Forms.DataGridViewTextBoxColumn();
                System.Windows.Forms.DataGridViewTextBoxColumn cGlassID = new System.Windows.Forms.DataGridViewTextBoxColumn();

                

                cTime.FillWeight = 120F;
                cTime.HeaderText = "Time";
                cTime.MaxInputLength = 14;
                cTime.MinimumWidth = 130;
                cTime.Name = "cTime";
                cTime.ReadOnly = true;
                cTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cTime.Width = 130;

                cType.FillWeight = 120F;
                cType.HeaderText = "Type";
                cType.MaxInputLength = 12;
                cType.MinimumWidth = 100;
                cType.Name = "cType";
                cType.ReadOnly = true;
                cType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                cType.Width = 100;

                cGlassID.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                cGlassID.HeaderText = "Glass ID";
                cGlassID.MaxInputLength = 16;
                cGlassID.MinimumWidth = 120;
                cGlassID.Name = "cGlassID";
                cGlassID.ReadOnly = true;
                cGlassID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                cGlassID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

                if (this.enmCurHistoryType == HistoryType.APC)
                {
                    System.Windows.Forms.DataGridViewTextBoxColumn cAPCPPID = new System.Windows.Forms.DataGridViewTextBoxColumn();

                    cAPCPPID.FillWeight = 120F;
                    cAPCPPID.HeaderText = "APC PPID";
                    cAPCPPID.MaxInputLength = 28;
                    cAPCPPID.MinimumWidth = 120;
                    cAPCPPID.Name = "cAPCPPID";
                    cAPCPPID.ReadOnly = true;
                    cAPCPPID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                    cAPCPPID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    cAPCPPID.Width = 120;

                    this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cTime, cType, cGlassID, cAPCPPID });
                }
                else if (this.enmCurHistoryType == HistoryType.RPC)
                {
                    System.Windows.Forms.DataGridViewTextBoxColumn cBeforePPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    System.Windows.Forms.DataGridViewTextBoxColumn cAfterPPID = new System.Windows.Forms.DataGridViewTextBoxColumn();



                    cBeforePPID.FillWeight = 120F;
                    cBeforePPID.HeaderText = "BeforePPID";
                    cBeforePPID.MaxInputLength = 28;
                    cBeforePPID.MinimumWidth = 120;
                    cBeforePPID.Name = "cBeforePPID";
                    cBeforePPID.ReadOnly = true;
                    cBeforePPID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                    cBeforePPID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    cBeforePPID.Width = 120;

                    cAfterPPID.FillWeight = 120F;
                    cAfterPPID.HeaderText = "AfterPPID";
                    cAfterPPID.MaxInputLength = 28;
                    cAfterPPID.MinimumWidth = 120;
                    cAfterPPID.Name = "cAfterPPID";
                    cAfterPPID.ReadOnly = true;
                    cAfterPPID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                    cAfterPPID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                    cAfterPPID.Width = 120;

                    this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cTime, cType, cGlassID, cBeforePPID, cAfterPPID });

                }
                else
                {
                    this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { cTime, cType, cGlassID });
                }
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

                this.pintFirstRowIndex = (int)((this.pstrData.Count / 20) * 20);//+ (((this.pstrData.Count % 20) != 0) ? 1 : 0)));

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

                this.pintFirstRowIndex += 20;

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

                this.pintFirstRowIndex -= 20;

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
                    this.lbPage.Text = string.Format("{0}/{1}", (pintFirstRowIndex / 20) + 1, ((int)((this.pstrData.Count / 20) + (((this.pstrData.Count % 20) != 0) ? 1 : 0))));

                    if ((pintFirstRowIndex / 20) == 0)
                    {
                        this.btnPrev.Enabled = false;
                        this.btnFirst.Enabled = false;
                    }
                    else
                    {
                        this.btnPrev.Enabled = true;
                        this.btnFirst.Enabled = true;
                    }

                    if ((pintFirstRowIndex / 20) + 1 == ((int)((this.pstrData.Count / 20) + (((this.pstrData.Count % 20) != 0) ? 1 : 0))))
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
                dstrFilePath = this.PInfo.All.LogFilePath + "\\" + this.tvDate.SelectedNode.FullPath.Replace("/", "") + pstrLogFileName;


                //파일의 모든 라인을 가져와 스트링 배열에 저장한다(라인별 내용을 배열에 저장)
                if (File.Exists(dstrFilePath) == true)
                {
                    fs = new FileStream(dstrFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    sr = new StreamReader(fs);

                    string dstrDateTime = string.Empty;

                    while (!sr.EndOfStream)
                    {
                        string dstrTemp = sr.ReadLine();

                        if (!string.IsNullOrEmpty(dstrTemp))
                        {
                            if (dstrTemp.StartsWith("["))
                            {
                                // 시간 셋팅
                                dstrDateTime = dstrTemp.Substring(1, 19);
                            }
                            else
                            {
                                if (this.enmCurHistoryType == HistoryType.APC || this.enmCurHistoryType == HistoryType.PPC || this.enmCurHistoryType == HistoryType.RPC) //dstrTemp = dstrTemp.Replace(":", ",");
                                {
                                    this.pstrData.Add(dstrDateTime + "|" + dstrTemp);//.Split(','));
                                }
                                else
                                {
                                    // 어떤 형식이 필요할지..?
                                    // 기본은 이거다...
                                    this.pstrData.Add(dstrDateTime + "," + dstrTemp);
                                }
                            }
                        }
                    }

                    this.pstrData.Reverse();
                }
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


                    this.dgv.RowCount = 20;
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

                string strValue = ((string[])((string)this.pstrData[e.RowIndex + pintFirstRowIndex]).Split('|'))[e.ColumnIndex];

                if ((this.enmCurHistoryType == HistoryType.APC || this.enmCurHistoryType == HistoryType.PPC || this.enmCurHistoryType == HistoryType.RPC) && e.ColumnIndex == 1)
                {

                    // Creation :1, Deletion : 2, Expiration : 3, Override : 4
                    switch (strValue)
                    {
                        case "1":
                            strValue = "Creation";
                            break;
                        case "2":
                            strValue = "Deletion";
                            break;
                        case "3":
                            strValue = "Expiration";
                            break;
                        case "4":
                            strValue = "Override";
                            break;
                        default :
                            strValue = "Unknown";
                            break;
                    }
                }

                e.Value = strValue;
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
                string[] strTemp = (string[])((string)this.pstrData[e.RowIndex + this.pintFirstRowIndex]).Split('|');

                switch (this.enmCurHistoryType)
                {
                    case HistoryType.NONE:
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

        
    }
}
