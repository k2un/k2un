namespace Simulator.Forms
{
    partial class PpidType1
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
            this.buttonSave = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxId = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDefaultSave = new System.Windows.Forms.Button();
            this.buttonDefaultLoad = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(509, 107);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "저장";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnValue});
            this.dataGridView1.Location = new System.Drawing.Point(13, 107);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(345, 427);
            this.dataGridView1.TabIndex = 1;
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.Width = 200;
            // 
            // ColumnValue
            // 
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            // 
            // textBoxId
            // 
            this.textBoxId.Location = new System.Drawing.Point(196, 56);
            this.textBoxId.Name = "textBoxId";
            this.textBoxId.Size = new System.Drawing.Size(162, 21);
            this.textBoxId.TabIndex = 2;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(509, 137);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "취소";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDefaultSave
            // 
            this.buttonDefaultSave.Location = new System.Drawing.Point(509, 167);
            this.buttonDefaultSave.Name = "buttonDefaultSave";
            this.buttonDefaultSave.Size = new System.Drawing.Size(130, 23);
            this.buttonDefaultSave.TabIndex = 4;
            this.buttonDefaultSave.Text = "기본값으로 저장하기";
            this.buttonDefaultSave.UseVisualStyleBackColor = true;
            this.buttonDefaultSave.Click += new System.EventHandler(this.buttonDefaultSave_Click);
            // 
            // buttonDefaultLoad
            // 
            this.buttonDefaultLoad.Location = new System.Drawing.Point(509, 197);
            this.buttonDefaultLoad.Name = "buttonDefaultLoad";
            this.buttonDefaultLoad.Size = new System.Drawing.Size(130, 23);
            this.buttonDefaultLoad.TabIndex = 5;
            this.buttonDefaultLoad.Text = "기본값 불러오기";
            this.buttonDefaultLoad.UseVisualStyleBackColor = true;
            this.buttonDefaultLoad.Click += new System.EventHandler(this.buttonDefaultLoad_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(137, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "아이디 : ";
            // 
            // PpidType1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 570);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDefaultLoad);
            this.Controls.Add(this.buttonDefaultSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBoxId);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonSave);
            this.Name = "PpidType1";
            this.Text = "PpidType1";
            this.Load += new System.EventHandler(this.PpidType1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.TextBox textBoxId;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDefaultSave;
        private System.Windows.Forms.Button buttonDefaultLoad;
        private System.Windows.Forms.Label label1;
    }
}