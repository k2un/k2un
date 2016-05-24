namespace DisplayAct
{
    partial class tabfrmInfoSEM
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SVID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VALUE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tmrUpdateSEM = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
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
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SVID,
            this.NAME,
            this.VALUE});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(797, 551);
            this.dataGridView1.TabIndex = 1;
            // 
            // SVID
            // 
            this.SVID.Frozen = true;
            this.SVID.HeaderText = "SVID";
            this.SVID.MinimumWidth = 150;
            this.SVID.Name = "SVID";
            this.SVID.ReadOnly = true;
            this.SVID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.SVID.Width = 150;
            // 
            // NAME
            // 
            this.NAME.Frozen = true;
            this.NAME.HeaderText = "Name";
            this.NAME.MinimumWidth = 430;
            this.NAME.Name = "NAME";
            this.NAME.ReadOnly = true;
            this.NAME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.NAME.Width = 430;
            // 
            // VALUE
            // 
            this.VALUE.HeaderText = "Value";
            this.VALUE.MinimumWidth = 200;
            this.VALUE.Name = "VALUE";
            this.VALUE.ReadOnly = true;
            this.VALUE.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.VALUE.Width = 200;
            // 
            // tmrUpdateSEM
            // 
            this.tmrUpdateSEM.Interval = 1000;
            this.tmrUpdateSEM.Tick += new System.EventHandler(this.tmrUpdateSEM_Tick);
            // 
            // tabfrmInfoSEM
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.dataGridView1);
            this.Name = "tabfrmInfoSEM";
            this.Size = new System.Drawing.Size(797, 551);
            this.Load += new System.EventHandler(this.tabfrmSEM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer tmrUpdateSEM;
        private System.Windows.Forms.DataGridViewTextBoxColumn SVID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn VALUE;

    }
}
