namespace DisplayAct
{
    partial class tabfrmHistoryGeneral
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpBase = new System.Windows.Forms.TableLayoutPanel();
            this.tlpLeftBase = new System.Windows.Forms.TableLayoutPanel();
            this.tvDate = new System.Windows.Forms.TreeView();
            this.lbDate = new System.Windows.Forms.Label();
            this.tplRightBase = new System.Windows.Forms.TableLayoutPanel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.tlpBottom = new System.Windows.Forms.TableLayoutPanel();
            this.lblGLSID = new System.Windows.Forms.Label();
            this.lbPage = new System.Windows.Forms.Label();
            this.cbSubType = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnLast = new System.Windows.Forms.Button();
            this.lblUnitID = new System.Windows.Forms.Label();
            this.txtGLSID = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.tlpBase.SuspendLayout();
            this.tlpLeftBase.SuspendLayout();
            this.tplRightBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.tlpBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpBase
            // 
            this.tlpBase.ColumnCount = 2;
            this.tlpBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tlpBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpBase.Controls.Add(this.tlpLeftBase, 0, 0);
            this.tlpBase.Controls.Add(this.tplRightBase, 1, 0);
            this.tlpBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBase.Location = new System.Drawing.Point(0, 0);
            this.tlpBase.Name = "tlpBase";
            this.tlpBase.RowCount = 1;
            this.tlpBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBase.Size = new System.Drawing.Size(904, 569);
            this.tlpBase.TabIndex = 0;
            // 
            // tlpLeftBase
            // 
            this.tlpLeftBase.ColumnCount = 1;
            this.tlpLeftBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeftBase.Controls.Add(this.tvDate, 0, 1);
            this.tlpLeftBase.Controls.Add(this.lbDate, 0, 0);
            this.tlpLeftBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLeftBase.Location = new System.Drawing.Point(0, 0);
            this.tlpLeftBase.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLeftBase.Name = "tlpLeftBase";
            this.tlpLeftBase.RowCount = 2;
            this.tlpLeftBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tlpLeftBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLeftBase.Size = new System.Drawing.Size(125, 569);
            this.tlpLeftBase.TabIndex = 0;
            // 
            // tvDate
            // 
            this.tvDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvDate.HideSelection = false;
            this.tvDate.LineColor = System.Drawing.Color.DimGray;
            this.tvDate.Location = new System.Drawing.Point(3, 27);
            this.tvDate.Margin = new System.Windows.Forms.Padding(3, 3, 1, 1);
            this.tvDate.Name = "tvDate";
            this.tvDate.PathSeparator = "/";
            this.tvDate.Size = new System.Drawing.Size(121, 541);
            this.tvDate.TabIndex = 0;
            this.tvDate.Visible = false;
            this.tvDate.VisibleChanged += new System.EventHandler(this.tvDate_VisibleChanged);
            this.tvDate.DoubleClick += new System.EventHandler(this.tvDate_DoubleClick);
            // 
            // lbDate
            // 
            this.lbDate.AutoSize = true;
            this.lbDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDate.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDate.Location = new System.Drawing.Point(3, 0);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(119, 24);
            this.lbDate.TabIndex = 1;
            this.lbDate.Text = "Date Select";
            this.lbDate.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tplRightBase
            // 
            this.tplRightBase.ColumnCount = 1;
            this.tplRightBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tplRightBase.Controls.Add(this.dgv, 0, 1);
            this.tplRightBase.Controls.Add(this.tlpBottom, 0, 0);
            this.tplRightBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tplRightBase.Location = new System.Drawing.Point(125, 0);
            this.tplRightBase.Margin = new System.Windows.Forms.Padding(0);
            this.tplRightBase.Name = "tplRightBase";
            this.tplRightBase.RowCount = 2;
            this.tplRightBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tplRightBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tplRightBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tplRightBase.Size = new System.Drawing.Size(779, 569);
            this.tplRightBase.TabIndex = 1;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv.ColumnHeadersHeight = 21;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv.Location = new System.Drawing.Point(1, 27);
            this.dgv.Margin = new System.Windows.Forms.Padding(1, 3, 3, 1);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.RowTemplate.ReadOnly = true;
            this.dgv.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.ShowCellErrors = false;
            this.dgv.ShowCellToolTips = false;
            this.dgv.ShowEditingIcon = false;
            this.dgv.ShowRowErrors = false;
            this.dgv.Size = new System.Drawing.Size(775, 541);
            this.dgv.TabIndex = 1;
            this.dgv.VirtualMode = true;
            this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellDoubleClick);
            this.dgv.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgv_CellValueNeeded);
            // 
            // tlpBottom
            // 
            this.tlpBottom.BackColor = System.Drawing.SystemColors.Control;
            this.tlpBottom.ColumnCount = 12;
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tlpBottom.Controls.Add(this.lblGLSID, 3, 0);
            this.tlpBottom.Controls.Add(this.lbPage, 7, 0);
            this.tlpBottom.Controls.Add(this.cbSubType, 1, 0);
            this.tlpBottom.Controls.Add(this.btnNext, 10, 0);
            this.tlpBottom.Controls.Add(this.btnPrev, 9, 0);
            this.tlpBottom.Controls.Add(this.btnFirst, 8, 0);
            this.tlpBottom.Controls.Add(this.btnLast, 11, 0);
            this.tlpBottom.Controls.Add(this.lblUnitID, 0, 0);
            this.tlpBottom.Controls.Add(this.txtGLSID, 4, 0);
            this.tlpBottom.Controls.Add(this.btnSearch, 5, 0);
            this.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBottom.Location = new System.Drawing.Point(0, 0);
            this.tlpBottom.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.tlpBottom.Name = "tlpBottom";
            this.tlpBottom.RowCount = 1;
            this.tlpBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBottom.Size = new System.Drawing.Size(776, 24);
            this.tlpBottom.TabIndex = 3;
            // 
            // lblGLSID
            // 
            this.lblGLSID.AutoSize = true;
            this.lblGLSID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGLSID.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblGLSID.Location = new System.Drawing.Point(183, 0);
            this.lblGLSID.Name = "lblGLSID";
            this.lblGLSID.Size = new System.Drawing.Size(54, 24);
            this.lblGLSID.TabIndex = 9;
            this.lblGLSID.Text = "GLSID";
            this.lblGLSID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lbPage
            // 
            this.lbPage.AutoSize = true;
            this.lbPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPage.ForeColor = System.Drawing.Color.DimGray;
            this.lbPage.Location = new System.Drawing.Point(535, 0);
            this.lbPage.Name = "lbPage";
            this.lbPage.Size = new System.Drawing.Size(94, 24);
            this.lbPage.TabIndex = 0;
            this.lbPage.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // cbSubType
            // 
            this.cbSubType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubType.Location = new System.Drawing.Point(61, 0);
            this.cbSubType.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.cbSubType.Name = "cbSubType";
            this.cbSubType.Size = new System.Drawing.Size(99, 23);
            this.cbSubType.TabIndex = 3;
            this.cbSubType.Visible = false;
            this.cbSubType.SelectedValueChanged += new System.EventHandler(this.cbSubType_SelectedValueChanged);
            // 
            // btnNext
            // 
            this.btnNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(704, 0);
            this.btnNext.Margin = new System.Windows.Forms.Padding(0);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(36, 24);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "▶";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPrev.Enabled = false;
            this.btnPrev.Location = new System.Drawing.Point(668, 0);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(36, 24);
            this.btnPrev.TabIndex = 1;
            this.btnPrev.Text = "◀";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnFirst
            // 
            this.btnFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFirst.Enabled = false;
            this.btnFirst.Location = new System.Drawing.Point(632, 0);
            this.btnFirst.Margin = new System.Windows.Forms.Padding(0);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(36, 24);
            this.btnFirst.TabIndex = 4;
            this.btnFirst.Text = "|◀";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
            // 
            // btnLast
            // 
            this.btnLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLast.Enabled = false;
            this.btnLast.Location = new System.Drawing.Point(740, 0);
            this.btnLast.Margin = new System.Windows.Forms.Padding(0);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(36, 24);
            this.btnLast.TabIndex = 5;
            this.btnLast.Text = "▶|";
            this.btnLast.UseVisualStyleBackColor = true;
            this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
            // 
            // lblUnitID
            // 
            this.lblUnitID.AutoSize = true;
            this.lblUnitID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUnitID.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblUnitID.Location = new System.Drawing.Point(3, 0);
            this.lblUnitID.Name = "lblUnitID";
            this.lblUnitID.Size = new System.Drawing.Size(54, 24);
            this.lblUnitID.TabIndex = 6;
            this.lblUnitID.Text = "UnitID";
            this.lblUnitID.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtGLSID
            // 
            this.txtGLSID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGLSID.Location = new System.Drawing.Point(243, 3);
            this.txtGLSID.Name = "txtGLSID";
            this.txtGLSID.Size = new System.Drawing.Size(194, 21);
            this.txtGLSID.TabIndex = 8;
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnSearch.Location = new System.Drawing.Point(440, 0);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(70, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // tabfrmHistoryGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpBase);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "tabfrmHistoryGeneral";
            this.Size = new System.Drawing.Size(904, 569);
            this.Tag = "Alarm History,GLSPDC History,LOTPDC History,Scrap History,Param History";
            this.tlpBase.ResumeLayout(false);
            this.tlpLeftBase.ResumeLayout(false);
            this.tlpLeftBase.PerformLayout();
            this.tplRightBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.tlpBottom.ResumeLayout(false);
            this.tlpBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpBase;
        private System.Windows.Forms.TableLayoutPanel tlpLeftBase;
        private System.Windows.Forms.TreeView tvDate;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TableLayoutPanel tplRightBase;
        private System.Windows.Forms.TableLayoutPanel tlpBottom;
        private System.Windows.Forms.Label lbPage;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.ComponentModel.BackgroundWorker bgWorker;
        //private System.Windows.Forms.DataGridViewTextBoxColumn cTime;
        //private System.Windows.Forms.DataGridViewTextBoxColumn cModuleID;
        //private System.Windows.Forms.DataGridViewTextBoxColumn cIsSet;
        //private System.Windows.Forms.DataGridViewTextBoxColumn cAlarmCode;
        //private System.Windows.Forms.DataGridViewTextBoxColumn cCode;
        //private System.Windows.Forms.DataGridViewTextBoxColumn cIsHeavy;
        //private System.Windows.Forms.DataGridViewTextBoxColumn cDescription;
        private System.Windows.Forms.ComboBox cbSubType;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Label lblGLSID;
        private System.Windows.Forms.Label lblUnitID;
        private System.Windows.Forms.TextBox txtGLSID;
        private System.Windows.Forms.Button btnSearch;
        
    }
}
