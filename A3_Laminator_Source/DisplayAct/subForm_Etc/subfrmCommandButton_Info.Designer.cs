namespace DisplayAct
{
    partial class subfrmCommandButton_Info
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(subfrmCommandButton_Info));
            this.btnSVReload = new System.Windows.Forms.Button();
            this.btnSEMInfo = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnCreateNewSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSVReload
            // 
            this.btnSVReload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSVReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSVReload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSVReload.BackgroundImage")));
            this.btnSVReload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSVReload.FlatAppearance.BorderSize = 0;
            this.btnSVReload.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSVReload.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSVReload.Image = ((System.Drawing.Image)(resources.GetObject("btnSVReload.Image")));
            this.btnSVReload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSVReload.Location = new System.Drawing.Point(1, 77);
            this.btnSVReload.Name = "btnSVReload";
            this.btnSVReload.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnSVReload.Size = new System.Drawing.Size(82, 70);
            this.btnSVReload.TabIndex = 49;
            this.btnSVReload.Text = "RELOAD";
            this.btnSVReload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSVReload.UseVisualStyleBackColor = false;
            this.btnSVReload.Visible = false;
            this.btnSVReload.Click += new System.EventHandler(this.btnSVReload_Click);
            // 
            // btnSEMInfo
            // 
            this.btnSEMInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSEMInfo.BackColor = System.Drawing.Color.Yellow;
            this.btnSEMInfo.FlatAppearance.BorderSize = 0;
            this.btnSEMInfo.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSEMInfo.ForeColor = System.Drawing.Color.Blue;
            this.btnSEMInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnSEMInfo.Image")));
            this.btnSEMInfo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSEMInfo.Location = new System.Drawing.Point(1, 1);
            this.btnSEMInfo.Name = "btnSEMInfo";
            this.btnSEMInfo.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.btnSEMInfo.Size = new System.Drawing.Size(82, 70);
            this.btnSEMInfo.TabIndex = 48;
            this.btnSEMInfo.Text = "CONFIG";
            this.btnSEMInfo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSEMInfo.UseVisualStyleBackColor = false;
            this.btnSEMInfo.Click += new System.EventHandler(this.btnSEMInfo_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnHistory.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHistory.BackgroundImage")));
            this.btnHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistory.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnHistory.Image")));
            this.btnHistory.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHistory.Location = new System.Drawing.Point(0, 153);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnHistory.Size = new System.Drawing.Size(82, 70);
            this.btnHistory.TabIndex = 50;
            this.btnHistory.Text = "HISTORY";
            this.btnHistory.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Visible = false;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnCreateNewSet
            // 
            this.btnCreateNewSet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateNewSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnCreateNewSet.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreateNewSet.BackgroundImage")));
            this.btnCreateNewSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCreateNewSet.FlatAppearance.BorderSize = 0;
            this.btnCreateNewSet.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateNewSet.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCreateNewSet.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateNewSet.Image")));
            this.btnCreateNewSet.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCreateNewSet.Location = new System.Drawing.Point(0, 229);
            this.btnCreateNewSet.Name = "btnCreateNewSet";
            this.btnCreateNewSet.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnCreateNewSet.Size = new System.Drawing.Size(82, 70);
            this.btnCreateNewSet.TabIndex = 51;
            this.btnCreateNewSet.Text = "CREATE NEW SET";
            this.btnCreateNewSet.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCreateNewSet.UseVisualStyleBackColor = false;
            this.btnCreateNewSet.Visible = false;
            this.btnCreateNewSet.Click += new System.EventHandler(this.btnCreateNewSet_Click);
            // 
            // subfrmCommandButton_Info
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.btnCreateNewSet);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnSVReload);
            this.Controls.Add(this.btnSEMInfo);
            this.Name = "subfrmCommandButton_Info";
            this.Size = new System.Drawing.Size(87, 550);
            this.Load += new System.EventHandler(this.subfrmCommandButton_SEM_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSEMInfo;
        private System.Windows.Forms.Button btnSVReload;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnCreateNewSet;
    }
}
