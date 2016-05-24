namespace Simulator.Forms
{
    partial class PpidManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageType0 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewType0 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBoxType0 = new System.Windows.Forms.ListBox();
            this.tabPageType1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridViewType1 = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxType1 = new System.Windows.Forms.ListBox();
            this.tabPageType2 = new System.Windows.Forms.TabPage();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonModify = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.listBoxType2 = new System.Windows.Forms.ListBox();
            this.dataGridViewType2 = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabPageType0.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewType0)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPageType1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewType1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tabPageType2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewType2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageType0);
            this.tabControl1.Controls.Add(this.tabPageType1);
            this.tabControl1.Controls.Add(this.tabPageType2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(769, 593);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageType0
            // 
            this.tabPageType0.Controls.Add(this.groupBox2);
            this.tabPageType0.Controls.Add(this.groupBox1);
            this.tabPageType0.Location = new System.Drawing.Point(4, 21);
            this.tabPageType0.Name = "tabPageType0";
            this.tabPageType0.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageType0.Size = new System.Drawing.Size(761, 568);
            this.tabPageType0.TabIndex = 0;
            this.tabPageType0.Text = "타입 0";
            this.tabPageType0.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewType0);
            this.groupBox2.Location = new System.Drawing.Point(191, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(479, 521);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Body";
            // 
            // dataGridViewType0
            // 
            this.dataGridViewType0.AllowUserToAddRows = false;
            this.dataGridViewType0.AllowUserToDeleteRows = false;
            this.dataGridViewType0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewType0.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.dataGridViewType0.Location = new System.Drawing.Point(7, 20);
            this.dataGridViewType0.Name = "dataGridViewType0";
            this.dataGridViewType0.ReadOnly = true;
            this.dataGridViewType0.RowTemplate.Height = 23;
            this.dataGridViewType0.Size = new System.Drawing.Size(410, 429);
            this.dataGridViewType0.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Name";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Value";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxType0);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 489);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PPID";
            // 
            // listBoxType0
            // 
            this.listBoxType0.FormattingEnabled = true;
            this.listBoxType0.ItemHeight = 12;
            this.listBoxType0.Location = new System.Drawing.Point(6, 20);
            this.listBoxType0.Name = "listBoxType0";
            this.listBoxType0.Size = new System.Drawing.Size(165, 460);
            this.listBoxType0.TabIndex = 1;
            // 
            // tabPageType1
            // 
            this.tabPageType1.Controls.Add(this.groupBox4);
            this.tabPageType1.Controls.Add(this.groupBox3);
            this.tabPageType1.Location = new System.Drawing.Point(4, 21);
            this.tabPageType1.Name = "tabPageType1";
            this.tabPageType1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageType1.Size = new System.Drawing.Size(761, 568);
            this.tabPageType1.TabIndex = 1;
            this.tabPageType1.Text = "타입 1";
            this.tabPageType1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dataGridViewType1);
            this.groupBox4.Location = new System.Drawing.Point(251, 56);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(421, 465);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Body";
            // 
            // dataGridViewType1
            // 
            this.dataGridViewType1.AllowUserToAddRows = false;
            this.dataGridViewType1.AllowUserToDeleteRows = false;
            this.dataGridViewType1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewType1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnValue});
            this.dataGridViewType1.Location = new System.Drawing.Point(17, 33);
            this.dataGridViewType1.Name = "dataGridViewType1";
            this.dataGridViewType1.ReadOnly = true;
            this.dataGridViewType1.RowHeadersVisible = false;
            this.dataGridViewType1.RowTemplate.Height = 23;
            this.dataGridViewType1.Size = new System.Drawing.Size(382, 405);
            this.dataGridViewType1.TabIndex = 0;
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.Width = 200;
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.ReadOnly = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxType1);
            this.groupBox3.Location = new System.Drawing.Point(33, 46);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(192, 475);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PPID";
            // 
            // listBoxType1
            // 
            this.listBoxType1.FormattingEnabled = true;
            this.listBoxType1.ItemHeight = 12;
            this.listBoxType1.Location = new System.Drawing.Point(6, 43);
            this.listBoxType1.Name = "listBoxType1";
            this.listBoxType1.Size = new System.Drawing.Size(167, 388);
            this.listBoxType1.TabIndex = 0;
            this.listBoxType1.SelectedIndexChanged += new System.EventHandler(this.listBoxType1_SelectedIndexChanged);
            // 
            // tabPageType2
            // 
            this.tabPageType2.Controls.Add(this.groupBox6);
            this.tabPageType2.Controls.Add(this.groupBox5);
            this.tabPageType2.Location = new System.Drawing.Point(4, 21);
            this.tabPageType2.Name = "tabPageType2";
            this.tabPageType2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageType2.Size = new System.Drawing.Size(761, 568);
            this.tabPageType2.TabIndex = 2;
            this.tabPageType2.Text = "타입 2";
            this.tabPageType2.UseVisualStyleBackColor = true;
            // 
            // buttonCreate
            // 
            this.buttonCreate.Location = new System.Drawing.Point(787, 33);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(75, 23);
            this.buttonCreate.TabIndex = 1;
            this.buttonCreate.Text = "생성";
            this.buttonCreate.UseVisualStyleBackColor = true;
            this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
            // 
            // buttonModify
            // 
            this.buttonModify.Location = new System.Drawing.Point(787, 60);
            this.buttonModify.Name = "buttonModify";
            this.buttonModify.Size = new System.Drawing.Size(75, 23);
            this.buttonModify.TabIndex = 2;
            this.buttonModify.Text = "수정";
            this.buttonModify.UseVisualStyleBackColor = true;
            this.buttonModify.Click += new System.EventHandler(this.buttonModify_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(787, 89);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "삭제";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(787, 119);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "닫기";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.listBoxType2);
            this.groupBox5.Location = new System.Drawing.Point(29, 56);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(218, 452);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "PPID";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dataGridViewType2);
            this.groupBox6.Location = new System.Drawing.Point(279, 56);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(464, 452);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Body";
            // 
            // listBoxType2
            // 
            this.listBoxType2.FormattingEnabled = true;
            this.listBoxType2.ItemHeight = 12;
            this.listBoxType2.Location = new System.Drawing.Point(16, 30);
            this.listBoxType2.Name = "listBoxType2";
            this.listBoxType2.Size = new System.Drawing.Size(180, 388);
            this.listBoxType2.TabIndex = 0;
            this.listBoxType2.SelectedIndexChanged += new System.EventHandler(this.listBoxType2_SelectedIndexChanged);
            // 
            // dataGridViewType2
            // 
            this.dataGridViewType2.AllowUserToAddRows = false;
            this.dataGridViewType2.AllowUserToDeleteRows = false;
            this.dataGridViewType2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewType2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4});
            this.dataGridViewType2.Location = new System.Drawing.Point(19, 30);
            this.dataGridViewType2.Name = "dataGridViewType2";
            this.dataGridViewType2.ReadOnly = true;
            this.dataGridViewType2.RowTemplate.Height = 23;
            this.dataGridViewType2.Size = new System.Drawing.Size(439, 405);
            this.dataGridViewType2.TabIndex = 0;
            // 
            // Column3
            // 
            this.Column3.Frozen = true;
            this.Column3.HeaderText = "Name";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 200;
            // 
            // Column4
            // 
            this.Column4.Frozen = true;
            this.Column4.HeaderText = "Value";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 200;
            // 
            // PpidManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 628);
            this.ControlBox = false;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonModify);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.tabControl1);
            this.Name = "PpidManager";
            this.Text = "PpidManager";
            this.tabControl1.ResumeLayout(false);
            this.tabPageType0.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewType0)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabPageType1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewType1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.tabPageType2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewType2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageType0;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewType0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxType0;
        private System.Windows.Forms.TabPage tabPageType1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridViewType1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox listBoxType1;
        private System.Windows.Forms.TabPage tabPageType2;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonModify;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DataGridView dataGridViewType2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListBox listBoxType2;
    }
}