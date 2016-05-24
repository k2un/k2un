namespace DisplayAct
{
    partial class tabfrmSetupPPID
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabHostPPID = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdHostInfo = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtHOST_MappingEQPPPID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreate_Hostppid = new System.Windows.Forms.Button();
            this.btnModify_Hostppid = new System.Windows.Forms.Button();
            this.btnDelete_Hostppid = new System.Windows.Forms.Button();
            this.txtHost_HOSTPPID = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabHostPPID.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHostInfo)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(928, 550);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabHostPPID);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(928, 550);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            this.tabControl1.TabIndexChanged += new System.EventHandler(this.tabControl1_TabIndexChanged);
            this.tabControl1.VisibleChanged += new System.EventHandler(this.tabControl1_VisibleChanged);
            // 
            // tabHostPPID
            // 
            this.tabHostPPID.Controls.Add(this.tableLayoutPanel1);
            this.tabHostPPID.Location = new System.Drawing.Point(4, 22);
            this.tabHostPPID.Name = "tabHostPPID";
            this.tabHostPPID.Padding = new System.Windows.Forms.Padding(3);
            this.tabHostPPID.Size = new System.Drawing.Size(920, 524);
            this.tabHostPPID.TabIndex = 0;
            this.tabHostPPID.Text = "HostPPID";
            this.tabHostPPID.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 510F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdHostInfo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 518F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 518F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(914, 518);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // grdHostInfo
            // 
            this.grdHostInfo.AllowUserToAddRows = false;
            this.grdHostInfo.AllowUserToDeleteRows = false;
            this.grdHostInfo.AllowUserToResizeColumns = false;
            this.grdHostInfo.AllowUserToResizeRows = false;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdHostInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.grdHostInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdHostInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.grdHostInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdHostInfo.Location = new System.Drawing.Point(3, 3);
            this.grdHostInfo.MultiSelect = false;
            this.grdHostInfo.Name = "grdHostInfo";
            this.grdHostInfo.ReadOnly = true;
            this.grdHostInfo.RowHeadersVisible = false;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdHostInfo.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.grdHostInfo.RowTemplate.Height = 23;
            this.grdHostInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdHostInfo.Size = new System.Drawing.Size(504, 512);
            this.grdHostInfo.TabIndex = 0;
            this.grdHostInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdHostInfo_CellClick);
            this.grdHostInfo.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdHostInfo_DataError);
            this.grdHostInfo.SelectionChanged += new System.EventHandler(this.grdHostInfo_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "No";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 50;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.FillWeight = 225F;
            this.Column2.Frozen = true;
            this.Column2.HeaderText = "Host PPID";
            this.Column2.MinimumWidth = 20;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.Width = 225;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column3.FillWeight = 225F;
            this.Column3.HeaderText = "Mapping EQP PPID";
            this.Column3.MinimumWidth = 20;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.Width = 225;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.txtHOST_MappingEQPPPID, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCreate_Hostppid, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.btnModify_Hostppid, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.btnDelete_Hostppid, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.txtHost_HOSTPPID, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(513, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(398, 512);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // txtHOST_MappingEQPPPID
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.txtHOST_MappingEQPPPID, 2);
            this.txtHOST_MappingEQPPPID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHOST_MappingEQPPPID.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHOST_MappingEQPPPID.Location = new System.Drawing.Point(123, 103);
            this.txtHOST_MappingEQPPPID.Multiline = true;
            this.txtHOST_MappingEQPPPID.Name = "txtHOST_MappingEQPPPID";
            this.txtHOST_MappingEQPPPID.ReadOnly = true;
            this.txtHOST_MappingEQPPPID.Size = new System.Drawing.Size(234, 44);
            this.txtHOST_MappingEQPPPID.TabIndex = 16;
            this.txtHOST_MappingEQPPPID.DoubleClick += new System.EventHandler(this.txtHOST_MappingEQPPPID_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 50);
            this.label2.TabIndex = 14;
            this.label2.Text = "EQP PPID";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host PPID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCreate_Hostppid
            // 
            this.btnCreate_Hostppid.BackColor = System.Drawing.Color.SkyBlue;
            this.btnCreate_Hostppid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreate_Hostppid.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCreate_Hostppid.Location = new System.Drawing.Point(3, 445);
            this.btnCreate_Hostppid.Name = "btnCreate_Hostppid";
            this.btnCreate_Hostppid.Size = new System.Drawing.Size(114, 44);
            this.btnCreate_Hostppid.TabIndex = 2;
            this.btnCreate_Hostppid.Text = "Create";
            this.btnCreate_Hostppid.UseVisualStyleBackColor = false;
            this.btnCreate_Hostppid.Click += new System.EventHandler(this.btnCreate_Hostppid_Click);
            // 
            // btnModify_Hostppid
            // 
            this.btnModify_Hostppid.BackColor = System.Drawing.Color.Khaki;
            this.btnModify_Hostppid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnModify_Hostppid.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnModify_Hostppid.Location = new System.Drawing.Point(123, 445);
            this.btnModify_Hostppid.Name = "btnModify_Hostppid";
            this.btnModify_Hostppid.Size = new System.Drawing.Size(114, 44);
            this.btnModify_Hostppid.TabIndex = 3;
            this.btnModify_Hostppid.Text = "Modify";
            this.btnModify_Hostppid.UseVisualStyleBackColor = false;
            this.btnModify_Hostppid.Click += new System.EventHandler(this.btnModify_Hostppid_Click);
            // 
            // btnDelete_Hostppid
            // 
            this.btnDelete_Hostppid.BackColor = System.Drawing.Color.Tomato;
            this.btnDelete_Hostppid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete_Hostppid.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete_Hostppid.Location = new System.Drawing.Point(243, 445);
            this.btnDelete_Hostppid.Name = "btnDelete_Hostppid";
            this.btnDelete_Hostppid.Size = new System.Drawing.Size(114, 44);
            this.btnDelete_Hostppid.TabIndex = 4;
            this.btnDelete_Hostppid.Text = "Delete";
            this.btnDelete_Hostppid.UseVisualStyleBackColor = false;
            this.btnDelete_Hostppid.Click += new System.EventHandler(this.btnDelete_Hostppid_Click);
            // 
            // txtHost_HOSTPPID
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.txtHost_HOSTPPID, 2);
            this.txtHost_HOSTPPID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHost_HOSTPPID.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHost_HOSTPPID.Location = new System.Drawing.Point(123, 3);
            this.txtHost_HOSTPPID.Multiline = true;
            this.txtHost_HOSTPPID.Name = "txtHost_HOSTPPID";
            this.txtHost_HOSTPPID.ReadOnly = true;
            this.txtHost_HOSTPPID.Size = new System.Drawing.Size(234, 44);
            this.txtHost_HOSTPPID.TabIndex = 15;
            this.txtHost_HOSTPPID.DoubleClick += new System.EventHandler(this.txtHost_HOSTPPID_DoubleClick);
            // 
            // tabfrmSetupPPID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel1);
            this.Name = "tabfrmSetupPPID";
            this.Size = new System.Drawing.Size(928, 550);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabHostPPID.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdHostInfo)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabHostPPID;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView grdHostInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCreate_Hostppid;
        private System.Windows.Forms.Button btnModify_Hostppid;
        private System.Windows.Forms.Button btnDelete_Hostppid;
        private System.Windows.Forms.TextBox txtHOST_MappingEQPPPID;
        private System.Windows.Forms.TextBox txtHost_HOSTPPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;


    }
}
