namespace DisplayAct
{
    partial class frmHostPPIDModify
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblHostPPID = new System.Windows.Forms.Label();
            this.txtHOSTPPID = new System.Windows.Forms.TextBox();
            this.txtEQPPPID_Change = new System.Windows.Forms.TextBox();
            this.lblEQPPPID = new System.Windows.Forms.Label();
            this.txtEQPPPID_Current = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnCancel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(330, 213);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(122, 48);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(440, 43);
            this.label1.TabIndex = 4;
            this.label1.Text = "HostPPID Modify Display";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnSave.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(204, 213);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(122, 48);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblHostPPID
            // 
            this.lblHostPPID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHostPPID.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblHostPPID.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHostPPID.Location = new System.Drawing.Point(18, 63);
            this.lblHostPPID.Name = "lblHostPPID";
            this.lblHostPPID.Size = new System.Drawing.Size(180, 41);
            this.lblHostPPID.TabIndex = 6;
            this.lblHostPPID.Text = "HOST PPID";
            this.lblHostPPID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtHOSTPPID
            // 
            this.txtHOSTPPID.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHOSTPPID.Location = new System.Drawing.Point(204, 63);
            this.txtHOSTPPID.Multiline = true;
            this.txtHOSTPPID.Name = "txtHOSTPPID";
            this.txtHOSTPPID.ReadOnly = true;
            this.txtHOSTPPID.Size = new System.Drawing.Size(248, 44);
            this.txtHOSTPPID.TabIndex = 16;
            // 
            // txtEQPPPID_Change
            // 
            this.txtEQPPPID_Change.BackColor = System.Drawing.SystemColors.Window;
            this.txtEQPPPID_Change.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEQPPPID_Change.Location = new System.Drawing.Point(204, 163);
            this.txtEQPPPID_Change.Multiline = true;
            this.txtEQPPPID_Change.Name = "txtEQPPPID_Change";
            this.txtEQPPPID_Change.ReadOnly = true;
            this.txtEQPPPID_Change.Size = new System.Drawing.Size(248, 44);
            this.txtEQPPPID_Change.TabIndex = 18;
            this.txtEQPPPID_Change.DoubleClick += new System.EventHandler(this.txtEQPPPID_DoubleClick);
            // 
            // lblEQPPPID
            // 
            this.lblEQPPPID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEQPPPID.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblEQPPPID.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEQPPPID.Location = new System.Drawing.Point(18, 165);
            this.lblEQPPPID.Name = "lblEQPPPID";
            this.lblEQPPPID.Size = new System.Drawing.Size(180, 41);
            this.lblEQPPPID.TabIndex = 17;
            this.lblEQPPPID.Text = "Change EQP PPID";
            this.lblEQPPPID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEQPPPID_Current
            // 
            this.txtEQPPPID_Current.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtEQPPPID_Current.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEQPPPID_Current.Location = new System.Drawing.Point(204, 113);
            this.txtEQPPPID_Current.Multiline = true;
            this.txtEQPPPID_Current.Name = "txtEQPPPID_Current";
            this.txtEQPPPID_Current.ReadOnly = true;
            this.txtEQPPPID_Current.Size = new System.Drawing.Size(248, 44);
            this.txtEQPPPID_Current.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 41);
            this.label2.TabIndex = 19;
            this.label2.Text = "Current EQP PPID";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmHostPPIDModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(466, 273);
            this.ControlBox = false;
            this.Controls.Add(this.txtEQPPPID_Current);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEQPPPID_Change);
            this.Controls.Add(this.lblEQPPPID);
            this.Controls.Add(this.txtHOSTPPID);
            this.Controls.Add(this.lblHostPPID);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHostPPIDModify";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error Message";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblHostPPID;
        private System.Windows.Forms.TextBox txtHOSTPPID;
        private System.Windows.Forms.TextBox txtEQPPPID_Change;
        private System.Windows.Forms.Label lblEQPPPID;
        private System.Windows.Forms.TextBox txtEQPPPID_Current;
        private System.Windows.Forms.Label label2;

    }
}