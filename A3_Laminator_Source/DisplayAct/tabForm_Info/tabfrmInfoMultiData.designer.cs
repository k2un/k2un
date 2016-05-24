namespace DisplayAct
{
    partial class tabfrmInfoMultiData
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DATATYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEMNAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ITEMVALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.REFERENCE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnSort = new System.Windows.Forms.Panel();
            this.cbSort2 = new System.Windows.Forms.CheckBox();
            this.cbSort1 = new System.Windows.Forms.CheckBox();
            this.lbSort = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnSort.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.DATATYPE,
            this.ITEMNAME,
            this.ITEMVALUE,
            this.REFERENCE});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 29);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(791, 519);
            this.dataGridView1.TabIndex = 1;
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Index.FillWeight = 65F;
            this.Index.Frozen = true;
            this.Index.HeaderText = "No";
            this.Index.MaxInputLength = 10;
            this.Index.MinimumWidth = 65;
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Index.Width = 65;
            // 
            // DATATYPE
            // 
            this.DATATYPE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DATATYPE.Frozen = true;
            this.DATATYPE.HeaderText = "DATA_TYPE";
            this.DATATYPE.MaxInputLength = 100;
            this.DATATYPE.MinimumWidth = 126;
            this.DATATYPE.Name = "DATATYPE";
            this.DATATYPE.ReadOnly = true;
            this.DATATYPE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DATATYPE.Width = 126;
            // 
            // ITEMNAME
            // 
            this.ITEMNAME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ITEMNAME.HeaderText = "ITEM_NAME";
            this.ITEMNAME.MaxInputLength = 100;
            this.ITEMNAME.MinimumWidth = 210;
            this.ITEMNAME.Name = "ITEMNAME";
            this.ITEMNAME.ReadOnly = true;
            this.ITEMNAME.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ITEMNAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ITEMNAME.Width = 210;
            // 
            // ITEMVALUE
            // 
            this.ITEMVALUE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ITEMVALUE.HeaderText = "ITEM_VALUE";
            this.ITEMVALUE.MaxInputLength = 100;
            this.ITEMVALUE.Name = "ITEMVALUE";
            this.ITEMVALUE.ReadOnly = true;
            this.ITEMVALUE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // REFERENCE
            // 
            this.REFERENCE.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.REFERENCE.HeaderText = "REFERENCE";
            this.REFERENCE.MaxInputLength = 100;
            this.REFERENCE.MinimumWidth = 126;
            this.REFERENCE.Name = "REFERENCE";
            this.REFERENCE.ReadOnly = true;
            this.REFERENCE.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.REFERENCE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.REFERENCE.Width = 126;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnSort, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(797, 551);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // pnSort
            // 
            this.pnSort.BackColor = System.Drawing.Color.Wheat;
            this.pnSort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnSort.Controls.Add(this.cbSort2);
            this.pnSort.Controls.Add(this.cbSort1);
            this.pnSort.Controls.Add(this.lbSort);
            this.pnSort.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnSort.Font = new System.Drawing.Font("Arial Black", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnSort.Location = new System.Drawing.Point(534, 0);
            this.pnSort.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.pnSort.Name = "pnSort";
            this.pnSort.Size = new System.Drawing.Size(260, 26);
            this.pnSort.TabIndex = 2;
            // 
            // cbSort2
            // 
            this.cbSort2.AutoSize = true;
            this.cbSort2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbSort2.Font = new System.Drawing.Font("Arial Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSort2.Location = new System.Drawing.Point(146, 6);
            this.cbSort2.Name = "cbSort2";
            this.cbSort2.Size = new System.Drawing.Size(98, 15);
            this.cbSort2.TabIndex = 2;
            this.cbSort2.Text = "DATA TYPE";
            this.cbSort2.UseVisualStyleBackColor = true;
            this.cbSort2.Click += new System.EventHandler(this.cbSort_Click);
            // 
            // cbSort1
            // 
            this.cbSort1.AutoSize = true;
            this.cbSort1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbSort1.Checked = true;
            this.cbSort1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSort1.Font = new System.Drawing.Font("Arial Black", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSort1.Location = new System.Drawing.Point(86, 6);
            this.cbSort1.Name = "cbSort1";
            this.cbSort1.Size = new System.Drawing.Size(40, 15);
            this.cbSort1.TabIndex = 1;
            this.cbSort1.Text = "No";
            this.cbSort1.UseVisualStyleBackColor = true;
            this.cbSort1.Click += new System.EventHandler(this.cbSort_Click);
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
            // tabfrmInfoMultiData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "tabfrmInfoMultiData";
            this.Size = new System.Drawing.Size(797, 551);
            this.Load += new System.EventHandler(this.tabfrmSEM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnSort.ResumeLayout(false);
            this.pnSort.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn DATATYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEMNAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn ITEMVALUE;
        private System.Windows.Forms.DataGridViewTextBoxColumn REFERENCE;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnSort;
        private System.Windows.Forms.CheckBox cbSort2;
        private System.Windows.Forms.CheckBox cbSort1;
        private System.Windows.Forms.Label lbSort;

    }
}
