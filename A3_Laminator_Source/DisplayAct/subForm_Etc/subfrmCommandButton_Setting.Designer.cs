namespace DisplayAct
{
    partial class subfrmCommandButton_Setting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(subfrmCommandButton_Setting));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.cbListEdit = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Black;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.btnSave.Size = new System.Drawing.Size(80, 70);
            this.btnSave.TabIndex = 41;
            this.btnSave.Text = "SAVE";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.BackColor = System.Drawing.Color.DarkRed;
            this.btnReload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReload.BackgroundImage")));
            this.btnReload.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReload.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnReload.Image = ((System.Drawing.Image)(resources.GetObject("btnReload.Image")));
            this.btnReload.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReload.Location = new System.Drawing.Point(3, 79);
            this.btnReload.Name = "btnReload";
            this.btnReload.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.btnReload.Size = new System.Drawing.Size(80, 70);
            this.btnReload.TabIndex = 42;
            this.btnReload.Text = "Reload";
            this.btnReload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.Visible = false;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // cbListEdit
            // 
            this.cbListEdit.AutoSize = true;
            this.cbListEdit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.cbListEdit.Location = new System.Drawing.Point(3, 531);
            this.cbListEdit.Name = "cbListEdit";
            this.cbListEdit.Size = new System.Drawing.Size(80, 17);
            this.cbListEdit.TabIndex = 43;
            this.cbListEdit.Text = "LIST EDIT";
            this.cbListEdit.UseVisualStyleBackColor = true;
            this.cbListEdit.Visible = false;
            this.cbListEdit.CheckedChanged += new System.EventHandler(this.cbListEdit_CheckedChanged);
            // 
            // subfrmCommandButton_Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.cbListEdit);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnSave);
            this.Name = "subfrmCommandButton_Setting";
            this.Size = new System.Drawing.Size(87, 550);
            this.Load += new System.EventHandler(this.subfrmCommandButton_Setting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.CheckBox cbListEdit;
    }
}
