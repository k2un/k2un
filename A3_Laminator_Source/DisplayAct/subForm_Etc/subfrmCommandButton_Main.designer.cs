namespace DisplayAct
{
    partial class subfrmCommandButton_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(subfrmCommandButton_Main));
            this.btnTest = new System.Windows.Forms.Button();
            this.btnMessageClear = new System.Windows.Forms.Button();
            this.btnBuzzerOff = new System.Windows.Forms.Button();
            this.btnProcessState = new System.Windows.Forms.Button();
            this.btnEQPState = new System.Windows.Forms.Button();
            this.btnModeChange = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTest.Location = new System.Drawing.Point(0, 500);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(80, 37);
            this.btnTest.TabIndex = 16;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnMessageClear
            // 
            this.btnMessageClear.BackColor = System.Drawing.Color.Black;
            this.btnMessageClear.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMessageClear.BackgroundImage")));
            this.btnMessageClear.FlatAppearance.BorderSize = 0;
            this.btnMessageClear.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMessageClear.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnMessageClear.Image = ((System.Drawing.Image)(resources.GetObject("btnMessageClear.Image")));
            this.btnMessageClear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMessageClear.Location = new System.Drawing.Point(36, 234);
            this.btnMessageClear.Name = "btnMessageClear";
            this.btnMessageClear.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnMessageClear.Size = new System.Drawing.Size(80, 70);
            this.btnMessageClear.TabIndex = 42;
            this.btnMessageClear.Text = "Message Clear";
            this.btnMessageClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMessageClear.UseVisualStyleBackColor = false;
            this.btnMessageClear.Visible = false;
            this.btnMessageClear.Click += new System.EventHandler(this.btnMessageClear_Click);
            // 
            // btnBuzzerOff
            // 
            this.btnBuzzerOff.BackColor = System.Drawing.Color.Black;
            this.btnBuzzerOff.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBuzzerOff.BackgroundImage")));
            this.btnBuzzerOff.FlatAppearance.BorderSize = 0;
            this.btnBuzzerOff.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuzzerOff.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnBuzzerOff.Image = ((System.Drawing.Image)(resources.GetObject("btnBuzzerOff.Image")));
            this.btnBuzzerOff.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBuzzerOff.Location = new System.Drawing.Point(36, 386);
            this.btnBuzzerOff.Name = "btnBuzzerOff";
            this.btnBuzzerOff.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnBuzzerOff.Size = new System.Drawing.Size(80, 70);
            this.btnBuzzerOff.TabIndex = 42;
            this.btnBuzzerOff.Text = "Buzzer Off";
            this.btnBuzzerOff.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBuzzerOff.UseVisualStyleBackColor = false;
            this.btnBuzzerOff.Visible = false;
            this.btnBuzzerOff.Click += new System.EventHandler(this.btnBuzzerOff_Click);
            // 
            // btnProcessState
            // 
            this.btnProcessState.BackColor = System.Drawing.Color.Black;
            this.btnProcessState.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProcessState.BackgroundImage")));
            this.btnProcessState.FlatAppearance.BorderSize = 0;
            this.btnProcessState.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessState.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnProcessState.Image = ((System.Drawing.Image)(resources.GetObject("btnProcessState.Image")));
            this.btnProcessState.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnProcessState.Location = new System.Drawing.Point(36, 310);
            this.btnProcessState.Name = "btnProcessState";
            this.btnProcessState.Size = new System.Drawing.Size(80, 70);
            this.btnProcessState.TabIndex = 41;
            this.btnProcessState.Text = "Process State";
            this.btnProcessState.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnProcessState.UseVisualStyleBackColor = false;
            this.btnProcessState.Visible = false;
            this.btnProcessState.Click += new System.EventHandler(this.btnProcessState_Click);
            // 
            // btnEQPState
            // 
            this.btnEQPState.BackColor = System.Drawing.Color.Black;
            this.btnEQPState.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEQPState.BackgroundImage")));
            this.btnEQPState.FlatAppearance.BorderSize = 0;
            this.btnEQPState.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEQPState.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnEQPState.Image = ((System.Drawing.Image)(resources.GetObject("btnEQPState.Image")));
            this.btnEQPState.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEQPState.Location = new System.Drawing.Point(36, 158);
            this.btnEQPState.Name = "btnEQPState";
            this.btnEQPState.Padding = new System.Windows.Forms.Padding(0, 3, 0, 5);
            this.btnEQPState.Size = new System.Drawing.Size(80, 70);
            this.btnEQPState.TabIndex = 39;
            this.btnEQPState.Text = "EQP State";
            this.btnEQPState.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEQPState.UseVisualStyleBackColor = false;
            this.btnEQPState.Visible = false;
            this.btnEQPState.Click += new System.EventHandler(this.btnEQPState_Click);
            // 
            // btnModeChange
            // 
            this.btnModeChange.BackColor = System.Drawing.Color.Black;
            this.btnModeChange.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnModeChange.BackgroundImage")));
            this.btnModeChange.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnModeChange.FlatAppearance.BorderSize = 0;
            this.btnModeChange.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModeChange.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnModeChange.Image = ((System.Drawing.Image)(resources.GetObject("btnModeChange.Image")));
            this.btnModeChange.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnModeChange.Location = new System.Drawing.Point(3, 6);
            this.btnModeChange.Name = "btnModeChange";
            this.btnModeChange.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.btnModeChange.Size = new System.Drawing.Size(80, 70);
            this.btnModeChange.TabIndex = 38;
            this.btnModeChange.Text = "Mode Change";
            this.btnModeChange.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnModeChange.UseVisualStyleBackColor = false;
            this.btnModeChange.Click += new System.EventHandler(this.btnModeChange_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(4, 471);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 43;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // subfrmCommandButton_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnMessageClear);
            this.Controls.Add(this.btnBuzzerOff);
            this.Controls.Add(this.btnProcessState);
            this.Controls.Add(this.btnEQPState);
            this.Controls.Add(this.btnModeChange);
            this.Controls.Add(this.btnTest);
            this.Name = "subfrmCommandButton_Main";
            this.Size = new System.Drawing.Size(87, 550);
            this.Load += new System.EventHandler(this.subfrmControlButton_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnModeChange;
        private System.Windows.Forms.Button btnEQPState;
        private System.Windows.Forms.Button btnProcessState;
        private System.Windows.Forms.Button btnBuzzerOff;
        private System.Windows.Forms.Button btnMessageClear;
        private System.Windows.Forms.Button button1;
    }
}
