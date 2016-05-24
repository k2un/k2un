namespace DisplayAct
{
    partial class frmEQPPPIDModify
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtChange_LOW = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtChange_UP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCurrent_LOW = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCurrent_UP = new System.Windows.Forms.TextBox();
            this.lblEQPPPID_UP = new System.Windows.Forms.Label();
            this.txtEQPPPID = new System.Windows.Forms.TextBox();
            this.lblEQPPPID_ = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.85715F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.txtCurrent_LOW, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtChange_UP, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtEQPPPID, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtChange_LOW, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtCurrent_UP, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblEQPPPID_, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblEQPPPID_UP, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28572F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(525, 374);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtChange_LOW
            // 
            this.txtChange_LOW.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.txtChange_LOW, 2);
            this.txtChange_LOW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChange_LOW.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChange_LOW.Location = new System.Drawing.Point(229, 263);
            this.txtChange_LOW.Multiline = true;
            this.txtChange_LOW.Name = "txtChange_LOW";
            this.txtChange_LOW.ReadOnly = true;
            this.txtChange_LOW.Size = new System.Drawing.Size(282, 44);
            this.txtChange_LOW.TabIndex = 37;
            this.txtChange_LOW.Tag = "LOW";
            this.txtChange_LOW.DoubleClick += new System.EventHandler(this.txtEQPPPID_DoubleClick);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label4.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(210, 50);
            this.label4.TabIndex = 36;
            this.label4.Text = "Change Lower Recipe";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtChange_UP
            // 
            this.txtChange_UP.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.txtChange_UP, 2);
            this.txtChange_UP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChange_UP.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChange_UP.Location = new System.Drawing.Point(229, 163);
            this.txtChange_UP.Multiline = true;
            this.txtChange_UP.Name = "txtChange_UP";
            this.txtChange_UP.ReadOnly = true;
            this.txtChange_UP.Size = new System.Drawing.Size(282, 44);
            this.txtChange_UP.TabIndex = 35;
            this.txtChange_UP.Tag = "UP";
            this.txtChange_UP.DoubleClick += new System.EventHandler(this.txtEQPPPID_DoubleClick);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label3.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(210, 50);
            this.label3.TabIndex = 34;
            this.label3.Text = "Change Upper Recipe";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCurrent_LOW
            // 
            this.txtCurrent_LOW.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.txtCurrent_LOW, 2);
            this.txtCurrent_LOW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCurrent_LOW.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrent_LOW.Location = new System.Drawing.Point(229, 213);
            this.txtCurrent_LOW.Multiline = true;
            this.txtCurrent_LOW.Name = "txtCurrent_LOW";
            this.txtCurrent_LOW.ReadOnly = true;
            this.txtCurrent_LOW.Size = new System.Drawing.Size(282, 44);
            this.txtCurrent_LOW.TabIndex = 33;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 50);
            this.label2.TabIndex = 32;
            this.label2.Text = "Current Lower Recipe";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCurrent_UP
            // 
            this.txtCurrent_UP.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.txtCurrent_UP, 2);
            this.txtCurrent_UP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCurrent_UP.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrent_UP.Location = new System.Drawing.Point(229, 113);
            this.txtCurrent_UP.Multiline = true;
            this.txtCurrent_UP.Name = "txtCurrent_UP";
            this.txtCurrent_UP.ReadOnly = true;
            this.txtCurrent_UP.Size = new System.Drawing.Size(282, 44);
            this.txtCurrent_UP.TabIndex = 31;
            // 
            // lblEQPPPID_UP
            // 
            this.lblEQPPPID_UP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEQPPPID_UP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEQPPPID_UP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblEQPPPID_UP.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEQPPPID_UP.Location = new System.Drawing.Point(13, 110);
            this.lblEQPPPID_UP.Name = "lblEQPPPID_UP";
            this.lblEQPPPID_UP.Size = new System.Drawing.Size(210, 50);
            this.lblEQPPPID_UP.TabIndex = 30;
            this.lblEQPPPID_UP.Text = "Current Upper Recipe";
            this.lblEQPPPID_UP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEQPPPID
            // 
            this.txtEQPPPID.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.txtEQPPPID, 2);
            this.txtEQPPPID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEQPPPID.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEQPPPID.Location = new System.Drawing.Point(229, 63);
            this.txtEQPPPID.Multiline = true;
            this.txtEQPPPID.Name = "txtEQPPPID";
            this.txtEQPPPID.Size = new System.Drawing.Size(282, 44);
            this.txtEQPPPID.TabIndex = 29;
            // 
            // lblEQPPPID_
            // 
            this.lblEQPPPID_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEQPPPID_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEQPPPID_.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblEQPPPID_.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEQPPPID_.Location = new System.Drawing.Point(13, 60);
            this.lblEQPPPID_.Name = "lblEQPPPID_";
            this.lblEQPPPID_.Size = new System.Drawing.Size(210, 50);
            this.lblEQPPPID_.TabIndex = 28;
            this.lblEQPPPID_.Text = "EQP PPID";
            this.lblEQPPPID_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(229, 313);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 44);
            this.btnSave.TabIndex = 27;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 3);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(498, 50);
            this.label1.TabIndex = 26;
            this.label1.Text = "EQPPPID Modify Display";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(373, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 44);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmEQPPPIDModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(525, 374);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEQPPPIDModify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error Message";
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtCurrent_LOW;
        private System.Windows.Forms.TextBox txtChange_UP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtEQPPPID;
        private System.Windows.Forms.TextBox txtChange_LOW;
        private System.Windows.Forms.TextBox txtCurrent_UP;
        private System.Windows.Forms.Label lblEQPPPID_;
        private System.Windows.Forms.Label lblEQPPPID_UP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;


    }
}