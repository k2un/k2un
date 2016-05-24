namespace DisplayAct
{
    partial class frmEQPState
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnNORMAL = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPM = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblMessage.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(8, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(325, 51);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Select EQP State.";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNORMAL
            // 
            this.btnNORMAL.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNORMAL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNORMAL.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNORMAL.Location = new System.Drawing.Point(11, 25);
            this.btnNORMAL.Name = "btnNORMAL";
            this.btnNORMAL.Size = new System.Drawing.Size(145, 71);
            this.btnNORMAL.TabIndex = 1;
            this.btnNORMAL.Text = "NORMAL";
            this.btnNORMAL.UseVisualStyleBackColor = false;
            this.btnNORMAL.Click += new System.EventHandler(this.btnNORMAL_Click);
            this.btnNORMAL.MouseLeave += new System.EventHandler(this.btnOffline_MouseLeave);
            this.btnNORMAL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnOffline_MouseMove);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnNORMAL);
            this.groupBox1.Controls.Add(this.btnPM);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(9, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 108);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "EPQ STATE.";
            // 
            // btnPM
            // 
            this.btnPM.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPM.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPM.Location = new System.Drawing.Point(167, 25);
            this.btnPM.Name = "btnPM";
            this.btnPM.Size = new System.Drawing.Size(145, 71);
            this.btnPM.TabIndex = 3;
            this.btnPM.Text = "PM";
            this.btnPM.UseVisualStyleBackColor = false;
            this.btnPM.Click += new System.EventHandler(this.btnPM_Click);
            this.btnPM.MouseLeave += new System.EventHandler(this.btnOnlineRemote_MouseLeave);
            this.btnPM.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnOnlineRemote_MouseMove);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(86, 179);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(168, 56);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmEQPState
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(342, 241);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMessage);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEQPState";
            this.Text = "EPQ State Change";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnNORMAL;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPM;
        private System.Windows.Forms.Button btnClose;
    }
}