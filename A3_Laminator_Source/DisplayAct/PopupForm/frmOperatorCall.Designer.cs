namespace DisplayAct
{
    partial class frmOperatorCall
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdLotInfo = new System.Windows.Forms.DataGridView();
            this.txtLotJudge = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtOPERID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtxGLSGRADE = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGLSJudge = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtGLSID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPRODID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLOTID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboRecipeID = new System.Windows.Forms.ComboBox();
            this.btnGLSAging = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtCSTID = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAllCancel = new System.Windows.Forms.Button();
            this.btnLOTCancel = new System.Windows.Forms.Button();
            this.btnLOTStart = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnLOTAbort = new System.Windows.Forms.Button();
            this.SlotNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LOTID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GLSID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdcboRecipe1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLotInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdLotInfo);
            this.groupBox1.Controls.Add(this.txtLotJudge);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtOPERID);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtxGLSGRADE);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtGLSJudge);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtGLSID);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtPRODID);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtLOTID);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboRecipeID);
            this.groupBox1.Controls.Add(this.btnGLSAging);
            this.groupBox1.Controls.Add(this.lblMessage);
            this.groupBox1.Controls.Add(this.txtCSTID);
            this.groupBox1.Controls.Add(this.lblPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(970, 779);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "LOT INFORMATION";
            // 
            // grdLotInfo
            // 
            this.grdLotInfo.AllowUserToAddRows = false;
            this.grdLotInfo.AllowUserToDeleteRows = false;
            this.grdLotInfo.AllowUserToResizeColumns = false;
            this.grdLotInfo.AllowUserToResizeRows = false;
            this.grdLotInfo.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdLotInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdLotInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdLotInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SlotNo,
            this.Column1,
            this.Column2,
            this.LOTID,
            this.GLSID,
            this.grdcboRecipe1,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdLotInfo.DefaultCellStyle = dataGridViewCellStyle3;
            this.grdLotInfo.Location = new System.Drawing.Point(164, 78);
            this.grdLotInfo.Name = "grdLotInfo";
            this.grdLotInfo.RowHeadersVisible = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdLotInfo.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.grdLotInfo.RowTemplate.Height = 23;
            this.grdLotInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdLotInfo.Size = new System.Drawing.Size(800, 694);
            this.grdLotInfo.StandardTab = true;
            this.grdLotInfo.TabIndex = 11;
            this.grdLotInfo.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdLotInfo_CellValueChanged);
            this.grdLotInfo.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdLotInfo_DataError);
            // 
            // txtLotJudge
            // 
            this.txtLotJudge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLotJudge.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLotJudge.Location = new System.Drawing.Point(9, 262);
            this.txtLotJudge.MaxLength = 16;
            this.txtLotJudge.Name = "txtLotJudge";
            this.txtLotJudge.Size = new System.Drawing.Size(149, 22);
            this.txtLotJudge.TabIndex = 2;
            this.txtLotJudge.Text = "G";
            this.txtLotJudge.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Navy;
            this.label10.Location = new System.Drawing.Point(9, 243);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 16);
            this.label10.TabIndex = 121;
            this.label10.Text = "LOT Judge";
            this.label10.Visible = false;
            // 
            // txtOPERID
            // 
            this.txtOPERID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOPERID.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOPERID.Location = new System.Drawing.Point(9, 392);
            this.txtOPERID.MaxLength = 16;
            this.txtOPERID.Name = "txtOPERID";
            this.txtOPERID.Size = new System.Drawing.Size(149, 22);
            this.txtOPERID.TabIndex = 120;
            this.txtOPERID.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Navy;
            this.label9.Location = new System.Drawing.Point(9, 373);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 16);
            this.label9.TabIndex = 119;
            this.label9.Text = "OPER ID";
            this.label9.Visible = false;
            // 
            // txtxGLSGRADE
            // 
            this.txtxGLSGRADE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtxGLSGRADE.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtxGLSGRADE.Location = new System.Drawing.Point(9, 537);
            this.txtxGLSGRADE.MaxLength = 16;
            this.txtxGLSGRADE.Name = "txtxGLSGRADE";
            this.txtxGLSGRADE.Size = new System.Drawing.Size(149, 22);
            this.txtxGLSGRADE.TabIndex = 118;
            this.txtxGLSGRADE.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Navy;
            this.label8.Location = new System.Drawing.Point(9, 518);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 16);
            this.label8.TabIndex = 117;
            this.label8.Text = "Glass GRADE";
            this.label8.Visible = false;
            // 
            // txtGLSJudge
            // 
            this.txtGLSJudge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGLSJudge.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGLSJudge.Location = new System.Drawing.Point(9, 493);
            this.txtGLSJudge.MaxLength = 16;
            this.txtGLSJudge.Name = "txtGLSJudge";
            this.txtGLSJudge.Size = new System.Drawing.Size(149, 22);
            this.txtGLSJudge.TabIndex = 116;
            this.txtGLSJudge.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Navy;
            this.label7.Location = new System.Drawing.Point(9, 474);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 16);
            this.label7.TabIndex = 115;
            this.label7.Text = "Glass Judge";
            this.label7.Visible = false;
            // 
            // txtGLSID
            // 
            this.txtGLSID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGLSID.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGLSID.Location = new System.Drawing.Point(9, 449);
            this.txtGLSID.MaxLength = 16;
            this.txtGLSID.Name = "txtGLSID";
            this.txtGLSID.Size = new System.Drawing.Size(149, 22);
            this.txtGLSID.TabIndex = 5;
            this.txtGLSID.Visible = false;
            this.txtGLSID.TextChanged += new System.EventHandler(this.txtGLSID_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(9, 430);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 16);
            this.label5.TabIndex = 113;
            this.label5.Text = "Glass ID";
            this.label5.Visible = false;
            // 
            // txtPRODID
            // 
            this.txtPRODID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPRODID.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPRODID.Location = new System.Drawing.Point(9, 348);
            this.txtPRODID.MaxLength = 16;
            this.txtPRODID.Name = "txtPRODID";
            this.txtPRODID.Size = new System.Drawing.Size(149, 22);
            this.txtPRODID.TabIndex = 4;
            this.txtPRODID.Text = "PRODID";
            this.txtPRODID.Visible = false;
            this.txtPRODID.TextChanged += new System.EventHandler(this.txtPRODID_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Navy;
            this.label6.Location = new System.Drawing.Point(9, 329);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 16);
            this.label6.TabIndex = 111;
            this.label6.Text = "PROD ID";
            this.label6.Visible = false;
            // 
            // txtLOTID
            // 
            this.txtLOTID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLOTID.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLOTID.Location = new System.Drawing.Point(9, 218);
            this.txtLOTID.MaxLength = 16;
            this.txtLOTID.Name = "txtLOTID";
            this.txtLOTID.Size = new System.Drawing.Size(149, 22);
            this.txtLOTID.TabIndex = 1;
            this.txtLOTID.TextChanged += new System.EventHandler(this.txtLOTID_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(9, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 16);
            this.label4.TabIndex = 107;
            this.label4.Text = "LOT ID";
            // 
            // cboRecipeID
            // 
            this.cboRecipeID.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRecipeID.FormattingEnabled = true;
            this.cboRecipeID.Location = new System.Drawing.Point(9, 304);
            this.cboRecipeID.Name = "cboRecipeID";
            this.cboRecipeID.Size = new System.Drawing.Size(149, 22);
            this.cboRecipeID.Sorted = true;
            this.cboRecipeID.TabIndex = 3;
            this.cboRecipeID.TextChanged += new System.EventHandler(this.cboRecipeID_TextChanged);
            // 
            // btnGLSAging
            // 
            this.btnGLSAging.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnGLSAging.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGLSAging.Location = new System.Drawing.Point(10, 723);
            this.btnGLSAging.Name = "btnGLSAging";
            this.btnGLSAging.Size = new System.Drawing.Size(84, 50);
            this.btnGLSAging.TabIndex = 106;
            this.btnGLSAging.Text = "1000매\r\n반송모드";
            this.btnGLSAging.UseVisualStyleBackColor = false;
            this.btnGLSAging.Visible = false;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.Navy;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(10, 24);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(954, 46);
            this.lblMessage.TabIndex = 103;
            this.lblMessage.Text = "INFORM MESSAGE";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCSTID
            // 
            this.txtCSTID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCSTID.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCSTID.Location = new System.Drawing.Point(9, 174);
            this.txtCSTID.MaxLength = 12;
            this.txtCSTID.Name = "txtCSTID";
            this.txtCSTID.Size = new System.Drawing.Size(149, 22);
            this.txtCSTID.TabIndex = 0;
            // 
            // lblPort
            // 
            this.lblPort.BackColor = System.Drawing.Color.Black;
            this.lblPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPort.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.ForeColor = System.Drawing.Color.Yellow;
            this.lblPort.Location = new System.Drawing.Point(9, 101);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(149, 41);
            this.lblPort.TabIndex = 40;
            this.lblPort.Text = "2";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(9, 287);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 16);
            this.label1.TabIndex = 28;
            this.label1.Text = "CHANGE ALL PPID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(9, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "CASSETTE ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(9, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "PORT ID";
            // 
            // btnAllCancel
            // 
            this.btnAllCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnAllCancel.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAllCancel.Location = new System.Drawing.Point(12, 790);
            this.btnAllCancel.Name = "btnAllCancel";
            this.btnAllCancel.Size = new System.Drawing.Size(109, 49);
            this.btnAllCancel.TabIndex = 10;
            this.btnAllCancel.Text = "ALL CANCEL";
            this.btnAllCancel.UseVisualStyleBackColor = false;
            this.btnAllCancel.Click += new System.EventHandler(this.btnAllCancel_Click);
            // 
            // btnLOTCancel
            // 
            this.btnLOTCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnLOTCancel.Font = new System.Drawing.Font("Arial", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLOTCancel.Location = new System.Drawing.Point(735, 789);
            this.btnLOTCancel.Name = "btnLOTCancel";
            this.btnLOTCancel.Size = new System.Drawing.Size(118, 49);
            this.btnLOTCancel.TabIndex = 7;
            this.btnLOTCancel.Text = "LOT CANCEL";
            this.btnLOTCancel.UseVisualStyleBackColor = false;
            this.btnLOTCancel.Click += new System.EventHandler(this.btnLOTCancel_Click);
            // 
            // btnLOTStart
            // 
            this.btnLOTStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnLOTStart.Font = new System.Drawing.Font("Arial", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLOTStart.Location = new System.Drawing.Point(859, 790);
            this.btnLOTStart.Name = "btnLOTStart";
            this.btnLOTStart.Size = new System.Drawing.Size(123, 49);
            this.btnLOTStart.TabIndex = 6;
            this.btnLOTStart.Text = "LOT START";
            this.btnLOTStart.UseVisualStyleBackColor = false;
            this.btnLOTStart.Click += new System.EventHandler(this.btnLOTStart_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.Font = new System.Drawing.Font("Arial", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(176, 790);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(118, 49);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnLOTAbort
            // 
            this.btnLOTAbort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnLOTAbort.Font = new System.Drawing.Font("Arial", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLOTAbort.Location = new System.Drawing.Point(611, 789);
            this.btnLOTAbort.Name = "btnLOTAbort";
            this.btnLOTAbort.Size = new System.Drawing.Size(118, 49);
            this.btnLOTAbort.TabIndex = 8;
            this.btnLOTAbort.Text = "LOT ABORT";
            this.btnLOTAbort.UseVisualStyleBackColor = false;
            this.btnLOTAbort.Click += new System.EventHandler(this.btnLOTAbort_Click);
            // 
            // SlotNo
            // 
            this.SlotNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.SlotNo.HeaderText = "SlotNo";
            this.SlotNo.Name = "SlotNo";
            this.SlotNo.ReadOnly = true;
            this.SlotNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.SlotNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.SlotNo.Width = 56;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.HeaderText = "Port";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.Width = 39;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.HeaderText = "User";
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.Width = 41;
            // 
            // LOTID
            // 
            this.LOTID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LOTID.FillWeight = 150F;
            this.LOTID.HeaderText = "LOTID";
            this.LOTID.Name = "LOTID";
            this.LOTID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LOTID.Width = 150;
            // 
            // GLSID
            // 
            this.GLSID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.GLSID.FillWeight = 150F;
            this.GLSID.HeaderText = "GLSID";
            this.GLSID.Name = "GLSID";
            this.GLSID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.GLSID.Width = 150;
            // 
            // grdcboRecipe1
            // 
            this.grdcboRecipe1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdcboRecipe1.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdcboRecipe1.FillWeight = 150F;
            this.grdcboRecipe1.HeaderText = "PPID";
            this.grdcboRecipe1.Name = "grdcboRecipe1";
            this.grdcboRecipe1.ReadOnly = true;
            this.grdcboRecipe1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.grdcboRecipe1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.grdcboRecipe1.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "CLN_Recipe";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "OV_Recipe";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // frmOperatorCall
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(994, 843);
            this.ControlBox = false;
            this.Controls.Add(this.btnLOTAbort);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnLOTStart);
            this.Controls.Add(this.btnLOTCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAllCancel);
            this.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOperatorCall";
            this.Text = " LOT START WINDOW";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLotInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAllCancel;
        private System.Windows.Forms.Button btnLOTCancel;
        private System.Windows.Forms.Button btnLOTStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdLotInfo;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtCSTID;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ComboBox cboRecipeID;
        private System.Windows.Forms.Button btnGLSAging;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLOTID;
        private System.Windows.Forms.TextBox txtGLSID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPRODID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLotJudge;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtOPERID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtxGLSGRADE;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGLSJudge;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnLOTAbort;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlotNo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOTID;
        private System.Windows.Forms.DataGridViewTextBoxColumn GLSID;
        private System.Windows.Forms.DataGridViewTextBoxColumn grdcboRecipe1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}