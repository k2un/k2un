namespace DisplayAct
{
    partial class frmEQPPPIDCreate
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
            this.lblEQPPPID_ = new System.Windows.Forms.Label();
            this.txtEQPPPID = new System.Windows.Forms.TextBox();
            this.txtEQPPPID_UP = new System.Windows.Forms.TextBox();
            this.lblEQPPPID_UP = new System.Windows.Forms.Label();
            this.txtEQPPPID_LOW = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnCancel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(287, 212);
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
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(391, 43);
            this.label1.TabIndex = 4;
            this.label1.Text = "EQPPPID Create Display";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnSave.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(161, 212);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(122, 48);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblEQPPPID_
            // 
            this.lblEQPPPID_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEQPPPID_.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblEQPPPID_.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEQPPPID_.Location = new System.Drawing.Point(18, 63);
            this.lblEQPPPID_.Name = "lblEQPPPID_";
            this.lblEQPPPID_.Size = new System.Drawing.Size(137, 41);
            this.lblEQPPPID_.TabIndex = 6;
            this.lblEQPPPID_.Text = "EQP PPID";
            this.lblEQPPPID_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEQPPPID
            // 
            this.txtEQPPPID.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEQPPPID.Location = new System.Drawing.Point(161, 62);
            this.txtEQPPPID.Multiline = true;
            this.txtEQPPPID.Name = "txtEQPPPID";
            this.txtEQPPPID.Size = new System.Drawing.Size(248, 44);
            this.txtEQPPPID.TabIndex = 16;
            // 
            // txtEQPPPID_UP
            // 
            this.txtEQPPPID_UP.BackColor = System.Drawing.SystemColors.Window;
            this.txtEQPPPID_UP.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEQPPPID_UP.Location = new System.Drawing.Point(161, 112);
            this.txtEQPPPID_UP.Multiline = true;
            this.txtEQPPPID_UP.Name = "txtEQPPPID_UP";
            this.txtEQPPPID_UP.ReadOnly = true;
            this.txtEQPPPID_UP.Size = new System.Drawing.Size(248, 44);
            this.txtEQPPPID_UP.TabIndex = 18;
            this.txtEQPPPID_UP.DoubleClick += new System.EventHandler(this.txtEQPPPID_DoubleClick);
            // 
            // lblEQPPPID_UP
            // 
            this.lblEQPPPID_UP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEQPPPID_UP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblEQPPPID_UP.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEQPPPID_UP.Location = new System.Drawing.Point(18, 115);
            this.lblEQPPPID_UP.Name = "lblEQPPPID_UP";
            this.lblEQPPPID_UP.Size = new System.Drawing.Size(137, 41);
            this.lblEQPPPID_UP.TabIndex = 17;
            this.lblEQPPPID_UP.Text = "Upper Recipe";
            this.lblEQPPPID_UP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEQPPPID_LOW
            // 
            this.txtEQPPPID_LOW.BackColor = System.Drawing.SystemColors.Window;
            this.txtEQPPPID_LOW.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEQPPPID_LOW.Location = new System.Drawing.Point(161, 162);
            this.txtEQPPPID_LOW.Multiline = true;
            this.txtEQPPPID_LOW.Name = "txtEQPPPID_LOW";
            this.txtEQPPPID_LOW.ReadOnly = true;
            this.txtEQPPPID_LOW.Size = new System.Drawing.Size(248, 44);
            this.txtEQPPPID_LOW.TabIndex = 20;
            this.txtEQPPPID_LOW.DoubleClick += new System.EventHandler(this.txtEQPPPID_DoubleClick);
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 41);
            this.label2.TabIndex = 19;
            this.label2.Text = "Lower Recipe";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmEQPPPIDCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(426, 277);
            this.ControlBox = false;
            this.Controls.Add(this.txtEQPPPID_LOW);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEQPPPID_UP);
            this.Controls.Add(this.lblEQPPPID_UP);
            this.Controls.Add(this.txtEQPPPID);
            this.Controls.Add(this.lblEQPPPID_);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEQPPPIDCreate";
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
        private System.Windows.Forms.Label lblEQPPPID_;
        private System.Windows.Forms.TextBox txtEQPPPID;
        private System.Windows.Forms.TextBox txtEQPPPID_UP;
        private System.Windows.Forms.Label lblEQPPPID_UP;
        private System.Windows.Forms.TextBox txtEQPPPID_LOW;
        private System.Windows.Forms.Label label2;

    }
}