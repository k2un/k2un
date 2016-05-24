namespace DisplayAct
{
    partial class Ngrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tLPbase = new System.Windows.Forms.TableLayoutPanel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.tLP_TOP = new System.Windows.Forms.TableLayoutPanel();
            this.tLPFindH_GLASSID = new System.Windows.Forms.TableLayoutPanel();
            this.btnFindHGLASSID = new System.Windows.Forms.Button();
            this.tbFindGlassID = new System.Windows.Forms.TextBox();
            this.pnSort = new System.Windows.Forms.Panel();
            this.cbSort2 = new System.Windows.Forms.CheckBox();
            this.cbSort1 = new System.Windows.Forms.CheckBox();
            this.lbSort = new System.Windows.Forms.Label();
            this.tlpTitle = new System.Windows.Forms.TableLayoutPanel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.tLPbase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.tLP_TOP.SuspendLayout();
            this.tLPFindH_GLASSID.SuspendLayout();
            this.pnSort.SuspendLayout();
            this.tlpTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // tLPbase
            // 
            this.tLPbase.ColumnCount = 1;
            this.tLPbase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLPbase.Controls.Add(this.dgv, 0, 2);
            this.tLPbase.Controls.Add(this.tLP_TOP, 0, 1);
            this.tLPbase.Controls.Add(this.tlpTitle, 0, 0);
            this.tLPbase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLPbase.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.tLPbase.Location = new System.Drawing.Point(0, 0);
            this.tLPbase.Margin = new System.Windows.Forms.Padding(0);
            this.tLPbase.Name = "tLPbase";
            this.tLPbase.Padding = new System.Windows.Forms.Padding(3);
            this.tLPbase.RowCount = 3;
            this.tLPbase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tLPbase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tLPbase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLPbase.Size = new System.Drawing.Size(649, 480);
            this.tLPbase.TabIndex = 0;
            // 
            // dgv
            // 
            this.dgv.AllowDrop = true;
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeColumns = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.dgv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5, 1, 5, 1);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv.Location = new System.Drawing.Point(3, 77);
            this.dgv.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv.RowTemplate.Height = 23;
            this.dgv.RowTemplate.ReadOnly = true;
            this.dgv.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.ShowCellErrors = false;
            this.dgv.ShowCellToolTips = false;
            this.dgv.ShowEditingIcon = false;
            this.dgv.ShowRowErrors = false;
            this.dgv.Size = new System.Drawing.Size(643, 400);
            this.dgv.TabIndex = 0;
            this.dgv.TabStop = false;
            this.dgv.VirtualMode = true;
            this.dgv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
            this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellDoubleClick);
            this.dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_CellFormatting);
            this.dgv.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgv_CellPainting);
            this.dgv.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgv_CellValueNeeded);
            this.dgv.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgv_Scroll);
            this.dgv.SelectionChanged += new System.EventHandler(this.dgv_SelectionChanged);
            // 
            // tLP_TOP
            // 
            this.tLP_TOP.ColumnCount = 3;
            this.tLP_TOP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 380F));
            this.tLP_TOP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_TOP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tLP_TOP.Controls.Add(this.tLPFindH_GLASSID, 0, 0);
            this.tLP_TOP.Controls.Add(this.pnSort, 2, 0);
            this.tLP_TOP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_TOP.Location = new System.Drawing.Point(3, 42);
            this.tLP_TOP.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.tLP_TOP.Name = "tLP_TOP";
            this.tLP_TOP.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.tLP_TOP.RowCount = 1;
            this.tLP_TOP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLP_TOP.Size = new System.Drawing.Size(643, 29);
            this.tLP_TOP.TabIndex = 1;
            // 
            // tLPFindH_GLASSID
            // 
            this.tLPFindH_GLASSID.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tLPFindH_GLASSID.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tLPFindH_GLASSID.ColumnCount = 2;
            this.tLPFindH_GLASSID.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tLPFindH_GLASSID.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLPFindH_GLASSID.Controls.Add(this.btnFindHGLASSID, 0, 0);
            this.tLPFindH_GLASSID.Controls.Add(this.tbFindGlassID, 1, 0);
            this.tLPFindH_GLASSID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLPFindH_GLASSID.Location = new System.Drawing.Point(0, 0);
            this.tLPFindH_GLASSID.Margin = new System.Windows.Forms.Padding(0);
            this.tLPFindH_GLASSID.Name = "tLPFindH_GLASSID";
            this.tLPFindH_GLASSID.RowCount = 1;
            this.tLPFindH_GLASSID.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tLPFindH_GLASSID.Size = new System.Drawing.Size(380, 27);
            this.tLPFindH_GLASSID.TabIndex = 0;
            // 
            // btnFindHGLASSID
            // 
            this.btnFindHGLASSID.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btnFindHGLASSID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFindHGLASSID.FlatAppearance.BorderSize = 0;
            this.btnFindHGLASSID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFindHGLASSID.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFindHGLASSID.Location = new System.Drawing.Point(1, 1);
            this.btnFindHGLASSID.Margin = new System.Windows.Forms.Padding(0);
            this.btnFindHGLASSID.Name = "btnFindHGLASSID";
            this.btnFindHGLASSID.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.btnFindHGLASSID.Size = new System.Drawing.Size(120, 25);
            this.btnFindHGLASSID.TabIndex = 0;
            this.btnFindHGLASSID.Text = "Find H_GLASSID";
            this.btnFindHGLASSID.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnFindHGLASSID.UseVisualStyleBackColor = false;
            this.btnFindHGLASSID.Click += new System.EventHandler(this.btnFindHGLASSID_Click);
            // 
            // tbFindGlassID
            // 
            this.tbFindGlassID.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tbFindGlassID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbFindGlassID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFindGlassID.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tbFindGlassID.Location = new System.Drawing.Point(126, 8);
            this.tbFindGlassID.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            this.tbFindGlassID.MaxLength = 60;
            this.tbFindGlassID.Name = "tbFindGlassID";
            this.tbFindGlassID.Size = new System.Drawing.Size(249, 17);
            this.tbFindGlassID.TabIndex = 1;
            this.tbFindGlassID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pnSort
            // 
            this.pnSort.BackColor = System.Drawing.Color.Wheat;
            this.pnSort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnSort.Controls.Add(this.cbSort2);
            this.pnSort.Controls.Add(this.cbSort1);
            this.pnSort.Controls.Add(this.lbSort);
            this.pnSort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSort.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnSort.Location = new System.Drawing.Point(383, 0);
            this.pnSort.Margin = new System.Windows.Forms.Padding(0);
            this.pnSort.Name = "pnSort";
            this.pnSort.Size = new System.Drawing.Size(260, 27);
            this.pnSort.TabIndex = 1;
            // 
            // cbSort2
            // 
            this.cbSort2.AutoSize = true;
            this.cbSort2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbSort2.Font = new System.Drawing.Font("Arial Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSort2.Location = new System.Drawing.Point(146, 6);
            this.cbSort2.Name = "cbSort2";
            this.cbSort2.Size = new System.Drawing.Size(100, 15);
            this.cbSort2.TabIndex = 2;
            this.cbSort2.Text = "APC_STATE";
            this.cbSort2.UseVisualStyleBackColor = true;
            this.cbSort2.Click += new System.EventHandler(this.cbSortBtn_Click);
            // 
            // cbSort1
            // 
            this.cbSort1.AutoSize = true;
            this.cbSort1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbSort1.Checked = true;
            this.cbSort1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSort1.Font = new System.Drawing.Font("Arial Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSort1.Location = new System.Drawing.Point(49, 6);
            this.cbSort1.Name = "cbSort1";
            this.cbSort1.Size = new System.Drawing.Size(86, 15);
            this.cbSort1.TabIndex = 1;
            this.cbSort1.Text = "SET_TIME";
            this.cbSort1.UseVisualStyleBackColor = true;
            this.cbSort1.Click += new System.EventHandler(this.cbSortBtn_Click);
            // 
            // lbSort
            // 
            this.lbSort.AutoSize = true;
            this.lbSort.Location = new System.Drawing.Point(8, 7);
            this.lbSort.Name = "lbSort";
            this.lbSort.Size = new System.Drawing.Size(36, 12);
            this.lbSort.TabIndex = 0;
            this.lbSort.Text = "Sort:";
            // 
            // tlpTitle
            // 
            this.tlpTitle.ColumnCount = 3;
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tlpTitle.Controls.Add(this.lbTitle, 1, 0);
            this.tlpTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTitle.Location = new System.Drawing.Point(3, 6);
            this.tlpTitle.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tlpTitle.Name = "tlpTitle";
            this.tlpTitle.RowCount = 1;
            this.tlpTitle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTitle.Size = new System.Drawing.Size(643, 30);
            this.tlpTitle.TabIndex = 2;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.BackColor = System.Drawing.Color.Yellow;
            this.lbTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTitle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lbTitle.Font = new System.Drawing.Font("Calibri", 13F, System.Drawing.FontStyle.Bold);
            this.lbTitle.ForeColor = System.Drawing.Color.Red;
            this.lbTitle.Location = new System.Drawing.Point(123, 0);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(397, 30);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "Advanced Process Control (APC)";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Ngrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tLPbase);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "Ngrid";
            this.Size = new System.Drawing.Size(649, 480);
            this.tLPbase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.tLP_TOP.ResumeLayout(false);
            this.tLPFindH_GLASSID.ResumeLayout(false);
            this.tLPFindH_GLASSID.PerformLayout();
            this.pnSort.ResumeLayout(false);
            this.pnSort.PerformLayout();
            this.tlpTitle.ResumeLayout(false);
            this.tlpTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tLPbase;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.TableLayoutPanel tLP_TOP;
        private System.Windows.Forms.TableLayoutPanel tLPFindH_GLASSID;
        private System.Windows.Forms.Button btnFindHGLASSID;
        private System.Windows.Forms.TextBox tbFindGlassID;
        private System.Windows.Forms.Panel pnSort;
        private System.Windows.Forms.CheckBox cbSort2;
        private System.Windows.Forms.CheckBox cbSort1;
        private System.Windows.Forms.Label lbSort;
        private System.Windows.Forms.TableLayoutPanel tlpTitle;
        private System.Windows.Forms.Label lbTitle;
    }
}
