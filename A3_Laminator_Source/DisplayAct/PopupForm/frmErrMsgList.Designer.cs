namespace DisplayAct
{
    partial class frmErrMsgList
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
            this.btnClear = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.btnBuzerOff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnClear.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(169, 563);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(156, 48);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtMessage.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.Black;
            this.txtMessage.Location = new System.Drawing.Point(3, 3);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(644, 229);
            this.txtMessage.TabIndex = 2;
            // 
            // lstMessage
            // 
            this.lstMessage.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.HorizontalScrollbar = true;
            this.lstMessage.ItemHeight = 15;
            this.lstMessage.Location = new System.Drawing.Point(3, 238);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(644, 319);
            this.lstMessage.TabIndex = 4;
            this.lstMessage.SelectedIndexChanged += new System.EventHandler(this.lstMessage_SelectedIndexChanged);
            // 
            // btnBuzerOff
            // 
            this.btnBuzerOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnBuzerOff.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuzerOff.Location = new System.Drawing.Point(327, 563);
            this.btnBuzerOff.Name = "btnBuzerOff";
            this.btnBuzerOff.Size = new System.Drawing.Size(156, 48);
            this.btnBuzerOff.TabIndex = 5;
            this.btnBuzerOff.Text = "BuzzerOff";
            this.btnBuzerOff.UseVisualStyleBackColor = false;
            this.btnBuzerOff.Click += new System.EventHandler(this.btnBuzzerOff_Click);
            // 
            // frmErrMsgList
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(651, 618);
            this.ControlBox = false;
            this.Controls.Add(this.btnBuzerOff);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lstMessage);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmErrMsgList";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "frmErrMsgList";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        public System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.ListBox lstMessage;
        private System.Windows.Forms.Button btnBuzerOff;
    }
}