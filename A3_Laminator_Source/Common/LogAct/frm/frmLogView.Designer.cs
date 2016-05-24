namespace LogAct
{
    partial class frmLogView
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
            this.tabLogControl = new System.Windows.Forms.TabControl();
            this.tabCIMLog = new System.Windows.Forms.TabPage();
            this.txtCIMLog = new System.Windows.Forms.TextBox();
            this.tabPLCLog = new System.Windows.Forms.TabPage();
            this.txtPLCLog = new System.Windows.Forms.TextBox();
            this.tabPLCErrLog = new System.Windows.Forms.TabPage();
            this.txtPLCErrLog = new System.Windows.Forms.TextBox();
            this.btnAllLogClear = new System.Windows.Forms.Button();
            this.btnLogClear = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabLogControl.SuspendLayout();
            this.tabCIMLog.SuspendLayout();
            this.tabPLCLog.SuspendLayout();
            this.tabPLCErrLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabLogControl
            // 
            this.tabLogControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabLogControl.Controls.Add(this.tabCIMLog);
            this.tabLogControl.Controls.Add(this.tabPLCLog);
            this.tabLogControl.Controls.Add(this.tabPLCErrLog);
            this.tabLogControl.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabLogControl.Location = new System.Drawing.Point(12, 8);
            this.tabLogControl.Name = "tabLogControl";
            this.tabLogControl.SelectedIndex = 0;
            this.tabLogControl.Size = new System.Drawing.Size(674, 554);
            this.tabLogControl.TabIndex = 0;
            // 
            // tabCIMLog
            // 
            this.tabCIMLog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabCIMLog.Controls.Add(this.txtCIMLog);
            this.tabCIMLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabCIMLog.Location = new System.Drawing.Point(4, 31);
            this.tabCIMLog.Name = "tabCIMLog";
            this.tabCIMLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabCIMLog.Size = new System.Drawing.Size(666, 519);
            this.tabCIMLog.TabIndex = 1;
            this.tabCIMLog.Text = "CIM Log";
            this.tabCIMLog.UseVisualStyleBackColor = true;
            // 
            // txtCIMLog
            // 
            this.txtCIMLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtCIMLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCIMLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCIMLog.Location = new System.Drawing.Point(0, 0);
            this.txtCIMLog.MaxLength = 20000;
            this.txtCIMLog.Multiline = true;
            this.txtCIMLog.Name = "txtCIMLog";
            this.txtCIMLog.ReadOnly = true;
            this.txtCIMLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCIMLog.Size = new System.Drawing.Size(661, 516);
            this.txtCIMLog.TabIndex = 0;
            // 
            // tabPLCLog
            // 
            this.tabPLCLog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPLCLog.Controls.Add(this.txtPLCLog);
            this.tabPLCLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.tabPLCLog.Location = new System.Drawing.Point(4, 31);
            this.tabPLCLog.Name = "tabPLCLog";
            this.tabPLCLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPLCLog.Size = new System.Drawing.Size(666, 519);
            this.tabPLCLog.TabIndex = 0;
            this.tabPLCLog.Text = "PLC Log";
            this.tabPLCLog.UseVisualStyleBackColor = true;
            // 
            // txtPLCLog
            // 
            this.txtPLCLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtPLCLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPLCLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPLCLog.Location = new System.Drawing.Point(0, 0);
            this.txtPLCLog.MaxLength = 20000;
            this.txtPLCLog.Multiline = true;
            this.txtPLCLog.Name = "txtPLCLog";
            this.txtPLCLog.ReadOnly = true;
            this.txtPLCLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPLCLog.Size = new System.Drawing.Size(661, 516);
            this.txtPLCLog.TabIndex = 8;
            // 
            // tabPLCErrLog
            // 
            this.tabPLCErrLog.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPLCErrLog.Controls.Add(this.txtPLCErrLog);
            this.tabPLCErrLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.tabPLCErrLog.Location = new System.Drawing.Point(4, 31);
            this.tabPLCErrLog.Name = "tabPLCErrLog";
            this.tabPLCErrLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPLCErrLog.Size = new System.Drawing.Size(666, 519);
            this.tabPLCErrLog.TabIndex = 2;
            this.tabPLCErrLog.Text = "Error Log";
            this.tabPLCErrLog.UseVisualStyleBackColor = true;
            // 
            // txtPLCErrLog
            // 
            this.txtPLCErrLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtPLCErrLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPLCErrLog.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPLCErrLog.Location = new System.Drawing.Point(0, 0);
            this.txtPLCErrLog.MaxLength = 20000;
            this.txtPLCErrLog.Multiline = true;
            this.txtPLCErrLog.Name = "txtPLCErrLog";
            this.txtPLCErrLog.ReadOnly = true;
            this.txtPLCErrLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPLCErrLog.Size = new System.Drawing.Size(661, 516);
            this.txtPLCErrLog.TabIndex = 9;
            // 
            // btnAllLogClear
            // 
            this.btnAllLogClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnAllLogClear.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllLogClear.Location = new System.Drawing.Point(209, 569);
            this.btnAllLogClear.Name = "btnAllLogClear";
            this.btnAllLogClear.Size = new System.Drawing.Size(151, 48);
            this.btnAllLogClear.TabIndex = 1;
            this.btnAllLogClear.Text = "All Log Clear";
            this.btnAllLogClear.UseVisualStyleBackColor = false;
            this.btnAllLogClear.Click += new System.EventHandler(this.btnAllLogClear_Click);
            // 
            // btnLogClear
            // 
            this.btnLogClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnLogClear.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogClear.Location = new System.Drawing.Point(370, 569);
            this.btnLogClear.Name = "btnLogClear";
            this.btnLogClear.Size = new System.Drawing.Size(151, 48);
            this.btnLogClear.TabIndex = 2;
            this.btnLogClear.Text = "Log Clear";
            this.btnLogClear.UseVisualStyleBackColor = false;
            this.btnLogClear.Click += new System.EventHandler(this.btnLogClear_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(531, 569);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(151, 48);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmLogView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(698, 627);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnLogClear);
            this.Controls.Add(this.btnAllLogClear);
            this.Controls.Add(this.tabLogControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogView";
            this.Text = "Log View";
            this.TopMost = true;
            this.tabLogControl.ResumeLayout(false);
            this.tabCIMLog.ResumeLayout(false);
            this.tabCIMLog.PerformLayout();
            this.tabPLCLog.ResumeLayout(false);
            this.tabPLCLog.PerformLayout();
            this.tabPLCErrLog.ResumeLayout(false);
            this.tabPLCErrLog.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabLogControl;
        private System.Windows.Forms.TabPage tabPLCLog;
        private System.Windows.Forms.TabPage tabCIMLog;
        private System.Windows.Forms.TextBox txtCIMLog;
        private System.Windows.Forms.Button btnAllLogClear;
        private System.Windows.Forms.Button btnLogClear;
        private System.Windows.Forms.Button btnClose;
        public System.Windows.Forms.TextBox txtPLCLog;
        private System.Windows.Forms.TabPage tabPLCErrLog;
        public System.Windows.Forms.TextBox txtPLCErrLog;
    }
}