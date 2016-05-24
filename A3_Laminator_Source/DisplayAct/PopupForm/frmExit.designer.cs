namespace DisplayAct
{
    partial class frmExit
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
            if(disposing && (components != null))
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
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnYES = new System.Windows.Forms.Button();
            this.btnNO = new System.Windows.Forms.Button();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Black;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(29, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(282, 50);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Do you want to exit?";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnYES
            // 
            this.btnYES.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnYES.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYES.Location = new System.Drawing.Point(29, 83);
            this.btnYES.Name = "btnYES";
            this.btnYES.Size = new System.Drawing.Size(139, 74);
            this.btnYES.TabIndex = 1;
            this.btnYES.Text = "YES";
            this.btnYES.UseVisualStyleBackColor = false;
            this.btnYES.Click += new System.EventHandler(this.btnYES_Click);
            // 
            // btnNO
            // 
            this.btnNO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnNO.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNO.Location = new System.Drawing.Point(172, 83);
            this.btnNO.Name = "btnNO";
            this.btnNO.Size = new System.Drawing.Size(139, 74);
            this.btnNO.TabIndex = 2;
            this.btnNO.Text = "NO";
            this.btnNO.UseVisualStyleBackColor = false;
            this.btnNO.Click += new System.EventHandler(this.btnNO_Click);
            // 
            // tmrMain
            // 
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // frmExit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(340, 183);
            this.ControlBox = false;
            this.Controls.Add(this.btnNO);
            this.Controls.Add(this.btnYES);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EXIT";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnYES;
        private System.Windows.Forms.Button btnNO;
        private System.Windows.Forms.Timer tmrMain;
    }
}