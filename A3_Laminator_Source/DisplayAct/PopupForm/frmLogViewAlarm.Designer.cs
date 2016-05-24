namespace DisplayAct
{
    partial class frmLogViewAlarm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdGlassInfo = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lvwAlarmInfo = new System.Windows.Forms.ListView();
            this.ITEM = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CONTENTS = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdGlassInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // grdGlassInfo
            // 
            this.grdGlassInfo.AllowUserToAddRows = false;
            this.grdGlassInfo.AllowUserToDeleteRows = false;
            this.grdGlassInfo.AllowUserToResizeColumns = false;
            this.grdGlassInfo.AllowUserToResizeRows = false;
            this.grdGlassInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdGlassInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.grdGlassInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdGlassInfo.DefaultCellStyle = dataGridViewCellStyle4;
            this.grdGlassInfo.Location = new System.Drawing.Point(12, 114);
            this.grdGlassInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdGlassInfo.MultiSelect = false;
            this.grdGlassInfo.Name = "grdGlassInfo";
            this.grdGlassInfo.ReadOnly = true;
            this.grdGlassInfo.RowHeadersVisible = false;
            this.grdGlassInfo.RowHeadersWidth = 25;
            this.grdGlassInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdGlassInfo.RowTemplate.Height = 23;
            this.grdGlassInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdGlassInfo.Size = new System.Drawing.Size(598, 370);
            this.grdGlassInfo.TabIndex = 7;
            // 
            // Column1
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.HeaderText = "POSITION";
            this.Column1.MaxInputLength = 100;
            this.Column1.MinimumWidth = 80;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "GLASS ID";
            this.Column2.MaxInputLength = 30;
            this.Column2.MinimumWidth = 150;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "LOT ID";
            this.Column3.MaxInputLength = 30;
            this.Column3.MinimumWidth = 150;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 150;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Slot ID";
            this.Column4.MaxInputLength = 30;
            this.Column4.MinimumWidth = 60;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.HeaderText = "PPID";
            this.Column5.MaxInputLength = 30;
            this.Column5.MinimumWidth = 150;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lvwAlarmInfo
            // 
            this.lvwAlarmInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvwAlarmInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ITEM,
            this.CONTENTS});
            this.lvwAlarmInfo.Font = new System.Drawing.Font("Arial", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvwAlarmInfo.GridLines = true;
            this.lvwAlarmInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwAlarmInfo.Location = new System.Drawing.Point(12, 14);
            this.lvwAlarmInfo.Name = "lvwAlarmInfo";
            this.lvwAlarmInfo.Scrollable = false;
            this.lvwAlarmInfo.Size = new System.Drawing.Size(597, 86);
            this.lvwAlarmInfo.TabIndex = 6;
            this.lvwAlarmInfo.UseCompatibleStateImageBehavior = false;
            this.lvwAlarmInfo.View = System.Windows.Forms.View.Details;
            // 
            // ITEM
            // 
            this.ITEM.Text = "ITEM";
            this.ITEM.Width = 140;
            // 
            // CONTENTS
            // 
            this.CONTENTS.Text = "CONTENTS";
            this.CONTENTS.Width = 646;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(487, 494);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(123, 41);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmLogViewAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 545);
            this.ControlBox = false;
            this.Controls.Add(this.grdGlassInfo);
            this.Controls.Add(this.lvwAlarmInfo);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogViewAlarm";
            this.Text = "Alarm Glass Info";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLogView_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.grdGlassInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdGlassInfo;
        private System.Windows.Forms.ListView lvwAlarmInfo;
        private System.Windows.Forms.ColumnHeader ITEM;
        private System.Windows.Forms.ColumnHeader CONTENTS;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}