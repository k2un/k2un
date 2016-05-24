namespace LogAct
{
    partial class frmCommListView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnAlarmDisplay = new System.Windows.Forms.Button();
            this.lblNext = new System.Windows.Forms.Label();
            this.lblNum10 = new System.Windows.Forms.Label();
            this.lblNum9 = new System.Windows.Forms.Label();
            this.lblNum8 = new System.Windows.Forms.Label();
            this.lblNum7 = new System.Windows.Forms.Label();
            this.lblNum6 = new System.Windows.Forms.Label();
            this.lblNum5 = new System.Windows.Forms.Label();
            this.lblNum4 = new System.Windows.Forms.Label();
            this.lblNum3 = new System.Windows.Forms.Label();
            this.lblNum2 = new System.Windows.Forms.Label();
            this.lblPrevious = new System.Windows.Forms.Label();
            this.lblAlarmDateTo = new System.Windows.Forms.Label();
            this.lblNum1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.calAlarmTo = new System.Windows.Forms.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAlarmDateFrom = new System.Windows.Forms.Label();
            this.calAlarmFrom = new System.Windows.Forms.MonthCalendar();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.grbSearch = new System.Windows.Forms.GroupBox();
            this.comUnitNum = new System.Windows.Forms.ComboBox();
            this.grdAlarmList = new System.Windows.Forms.DataGridView();
            this.grpLabel = new System.Windows.Forms.GroupBox();
            this.grbSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAlarmList)).BeginInit();
            this.grpLabel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAlarmDisplay
            // 
            this.btnAlarmDisplay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAlarmDisplay.Location = new System.Drawing.Point(646, 9);
            this.btnAlarmDisplay.Name = "btnAlarmDisplay";
            this.btnAlarmDisplay.Size = new System.Drawing.Size(113, 38);
            this.btnAlarmDisplay.TabIndex = 5;
            this.btnAlarmDisplay.Text = "DISPLAY";
            this.btnAlarmDisplay.UseVisualStyleBackColor = true;
            this.btnAlarmDisplay.Click += new System.EventHandler(this.btnAlarmDisplay_Click);
            // 
            // lblNext
            // 
            this.lblNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNext.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNext.ForeColor = System.Drawing.Color.Black;
            this.lblNext.Location = new System.Drawing.Point(376, 17);
            this.lblNext.Name = "lblNext";
            this.lblNext.Size = new System.Drawing.Size(67, 21);
            this.lblNext.TabIndex = 20;
            this.lblNext.Text = "> Next";
            this.lblNext.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNext.MouseLeave += new System.EventHandler(this.lblNext_MouseLeave);
            this.lblNext.Click += new System.EventHandler(this.lblNext_Click);
            this.lblNext.MouseEnter += new System.EventHandler(this.lblNext_MouseEnter);
            // 
            // lblNum10
            // 
            this.lblNum10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum10.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum10.ForeColor = System.Drawing.Color.Black;
            this.lblNum10.Location = new System.Drawing.Point(350, 17);
            this.lblNum10.Name = "lblNum10";
            this.lblNum10.Size = new System.Drawing.Size(32, 15);
            this.lblNum10.TabIndex = 22;
            this.lblNum10.Tag = "10";
            this.lblNum10.Text = "10";
            this.lblNum10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum10.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum10.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum10.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblNum9
            // 
            this.lblNum9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum9.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum9.ForeColor = System.Drawing.Color.Black;
            this.lblNum9.Location = new System.Drawing.Point(321, 17);
            this.lblNum9.Name = "lblNum9";
            this.lblNum9.Size = new System.Drawing.Size(32, 15);
            this.lblNum9.TabIndex = 17;
            this.lblNum9.Tag = "9";
            this.lblNum9.Text = "9";
            this.lblNum9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum9.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum9.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum9.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblNum8
            // 
            this.lblNum8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum8.ForeColor = System.Drawing.Color.Black;
            this.lblNum8.Location = new System.Drawing.Point(294, 17);
            this.lblNum8.Name = "lblNum8";
            this.lblNum8.Size = new System.Drawing.Size(32, 15);
            this.lblNum8.TabIndex = 25;
            this.lblNum8.Tag = "8";
            this.lblNum8.Text = "8";
            this.lblNum8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum8.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum8.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum8.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblNum7
            // 
            this.lblNum7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum7.ForeColor = System.Drawing.Color.Black;
            this.lblNum7.Location = new System.Drawing.Point(266, 17);
            this.lblNum7.Name = "lblNum7";
            this.lblNum7.Size = new System.Drawing.Size(32, 15);
            this.lblNum7.TabIndex = 26;
            this.lblNum7.Tag = "7";
            this.lblNum7.Text = "7";
            this.lblNum7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum7.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum7.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum7.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblNum6
            // 
            this.lblNum6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum6.ForeColor = System.Drawing.Color.Black;
            this.lblNum6.Location = new System.Drawing.Point(240, 17);
            this.lblNum6.Name = "lblNum6";
            this.lblNum6.Size = new System.Drawing.Size(32, 15);
            this.lblNum6.TabIndex = 23;
            this.lblNum6.Tag = "6";
            this.lblNum6.Text = "6";
            this.lblNum6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum6.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum6.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum6.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblNum5
            // 
            this.lblNum5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum5.ForeColor = System.Drawing.Color.Black;
            this.lblNum5.Location = new System.Drawing.Point(214, 17);
            this.lblNum5.Name = "lblNum5";
            this.lblNum5.Size = new System.Drawing.Size(32, 15);
            this.lblNum5.TabIndex = 24;
            this.lblNum5.Tag = "5";
            this.lblNum5.Text = "5";
            this.lblNum5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum5.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum5.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum5.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblNum4
            // 
            this.lblNum4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum4.ForeColor = System.Drawing.Color.Black;
            this.lblNum4.Location = new System.Drawing.Point(187, 17);
            this.lblNum4.Name = "lblNum4";
            this.lblNum4.Size = new System.Drawing.Size(32, 15);
            this.lblNum4.TabIndex = 29;
            this.lblNum4.Tag = "4";
            this.lblNum4.Text = "4";
            this.lblNum4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum4.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum4.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum4.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblNum3
            // 
            this.lblNum3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum3.ForeColor = System.Drawing.Color.Black;
            this.lblNum3.Location = new System.Drawing.Point(159, 17);
            this.lblNum3.Name = "lblNum3";
            this.lblNum3.Size = new System.Drawing.Size(32, 15);
            this.lblNum3.TabIndex = 30;
            this.lblNum3.Tag = "3";
            this.lblNum3.Text = "3";
            this.lblNum3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum3.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum3.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum3.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblNum2
            // 
            this.lblNum2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum2.ForeColor = System.Drawing.Color.Black;
            this.lblNum2.Location = new System.Drawing.Point(133, 17);
            this.lblNum2.Name = "lblNum2";
            this.lblNum2.Size = new System.Drawing.Size(32, 15);
            this.lblNum2.TabIndex = 27;
            this.lblNum2.Tag = "2";
            this.lblNum2.Text = "2";
            this.lblNum2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum2.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum2.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum2.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // lblPrevious
            // 
            this.lblPrevious.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPrevious.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrevious.ForeColor = System.Drawing.Color.Black;
            this.lblPrevious.Location = new System.Drawing.Point(24, 17);
            this.lblPrevious.Name = "lblPrevious";
            this.lblPrevious.Size = new System.Drawing.Size(89, 20);
            this.lblPrevious.TabIndex = 28;
            this.lblPrevious.Text = "Previous <";
            this.lblPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPrevious.MouseLeave += new System.EventHandler(this.lblPrevious_MouseLeave);
            this.lblPrevious.Click += new System.EventHandler(this.lblPrevious_Click);
            this.lblPrevious.MouseEnter += new System.EventHandler(this.lblPrevious_MouseEnter);
            // 
            // lblAlarmDateTo
            // 
            this.lblAlarmDateTo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblAlarmDateTo.BackColor = System.Drawing.Color.White;
            this.lblAlarmDateTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAlarmDateTo.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarmDateTo.Location = new System.Drawing.Point(473, 15);
            this.lblAlarmDateTo.Name = "lblAlarmDateTo";
            this.lblAlarmDateTo.Size = new System.Drawing.Size(120, 27);
            this.lblAlarmDateTo.TabIndex = 4;
            this.lblAlarmDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAlarmDateTo.Click += new System.EventHandler(this.lblAlarmDateTo_Click);
            // 
            // lblNum1
            // 
            this.lblNum1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblNum1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNum1.ForeColor = System.Drawing.Color.Black;
            this.lblNum1.Location = new System.Drawing.Point(106, 17);
            this.lblNum1.Name = "lblNum1";
            this.lblNum1.Size = new System.Drawing.Size(32, 15);
            this.lblNum1.TabIndex = 18;
            this.lblNum1.Tag = "1";
            this.lblNum1.Text = "1";
            this.lblNum1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNum1.MouseLeave += new System.EventHandler(this.lblNum_MouseLeave);
            this.lblNum1.Click += new System.EventHandler(this.lblNum_Click);
            this.lblNum1.MouseEnter += new System.EventHandler(this.lblNum_MouseEnter);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(433, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 26);
            this.label2.TabIndex = 3;
            this.label2.Text = "To";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // calAlarmTo
            // 
            this.calAlarmTo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calAlarmTo.Location = new System.Drawing.Point(485, 58);
            this.calAlarmTo.Name = "calAlarmTo";
            this.calAlarmTo.TabIndex = 12;
            this.calAlarmTo.Visible = false;
            this.calAlarmTo.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calAlarmTo_DateSelected);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(136, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Date:  From";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAlarmDateFrom
            // 
            this.lblAlarmDateFrom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblAlarmDateFrom.BackColor = System.Drawing.Color.White;
            this.lblAlarmDateFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAlarmDateFrom.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarmDateFrom.Location = new System.Drawing.Point(297, 15);
            this.lblAlarmDateFrom.Name = "lblAlarmDateFrom";
            this.lblAlarmDateFrom.Size = new System.Drawing.Size(120, 27);
            this.lblAlarmDateFrom.TabIndex = 1;
            this.lblAlarmDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAlarmDateFrom.Click += new System.EventHandler(this.lblAlarmDateFrom_Click);
            // 
            // calAlarmFrom
            // 
            this.calAlarmFrom.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calAlarmFrom.Location = new System.Drawing.Point(309, 58);
            this.calAlarmFrom.Name = "calAlarmFrom";
            this.calAlarmFrom.TabIndex = 11;
            this.calAlarmFrom.Visible = false;
            this.calAlarmFrom.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calAlarmFrom_DateSelected);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(662, 493);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(114, 40);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnClear.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(534, 494);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(114, 40);
            this.btnClear.TabIndex = 15;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // grbSearch
            // 
            this.grbSearch.Controls.Add(this.comUnitNum);
            this.grbSearch.Controls.Add(this.btnAlarmDisplay);
            this.grbSearch.Controls.Add(this.lblAlarmDateTo);
            this.grbSearch.Controls.Add(this.label2);
            this.grbSearch.Controls.Add(this.label1);
            this.grbSearch.Controls.Add(this.lblAlarmDateFrom);
            this.grbSearch.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbSearch.Location = new System.Drawing.Point(12, 4);
            this.grbSearch.Name = "grbSearch";
            this.grbSearch.Size = new System.Drawing.Size(765, 50);
            this.grbSearch.TabIndex = 13;
            this.grbSearch.TabStop = false;
            this.grbSearch.Text = "Search";
            // 
            // comUnitNum
            // 
            this.comUnitNum.BackColor = System.Drawing.Color.Silver;
            this.comUnitNum.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comUnitNum.FormattingEnabled = true;
            this.comUnitNum.Location = new System.Drawing.Point(9, 19);
            this.comUnitNum.Name = "comUnitNum";
            this.comUnitNum.Size = new System.Drawing.Size(78, 24);
            this.comUnitNum.TabIndex = 6;
            this.comUnitNum.Text = "UnitNum";
            this.comUnitNum.SelectedIndexChanged += new System.EventHandler(this.comUnitNum_SelectedIndexChanged);
            // 
            // grdAlarmList
            // 
            this.grdAlarmList.AllowUserToAddRows = false;
            this.grdAlarmList.AllowUserToDeleteRows = false;
            this.grdAlarmList.AllowUserToResizeColumns = false;
            this.grdAlarmList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdAlarmList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdAlarmList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grdAlarmList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdAlarmList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdAlarmList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdAlarmList.ColumnHeadersHeight = 28;
            this.grdAlarmList.Location = new System.Drawing.Point(12, 58);
            this.grdAlarmList.Name = "grdAlarmList";
            this.grdAlarmList.ReadOnly = true;
            this.grdAlarmList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.grdAlarmList.RowTemplate.Height = 19;
            this.grdAlarmList.RowTemplate.ReadOnly = true;
            this.grdAlarmList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdAlarmList.ShowCellErrors = false;
            this.grdAlarmList.ShowCellToolTips = false;
            this.grdAlarmList.ShowEditingIcon = false;
            this.grdAlarmList.ShowRowErrors = false;
            this.grdAlarmList.Size = new System.Drawing.Size(764, 430);
            this.grdAlarmList.TabIndex = 14;
            this.grdAlarmList.TabStop = false;
            this.grdAlarmList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.Grid_DataError);
            // 
            // grpLabel
            // 
            this.grpLabel.Controls.Add(this.lblPrevious);
            this.grpLabel.Controls.Add(this.lblNext);
            this.grpLabel.Controls.Add(this.lblNum1);
            this.grpLabel.Controls.Add(this.lblNum10);
            this.grpLabel.Controls.Add(this.lblNum2);
            this.grpLabel.Controls.Add(this.lblNum9);
            this.grpLabel.Controls.Add(this.lblNum3);
            this.grpLabel.Controls.Add(this.lblNum8);
            this.grpLabel.Controls.Add(this.lblNum4);
            this.grpLabel.Controls.Add(this.lblNum7);
            this.grpLabel.Controls.Add(this.lblNum5);
            this.grpLabel.Controls.Add(this.lblNum6);
            this.grpLabel.Location = new System.Drawing.Point(49, 491);
            this.grpLabel.Name = "grpLabel";
            this.grpLabel.Size = new System.Drawing.Size(469, 40);
            this.grpLabel.TabIndex = 0;
            this.grpLabel.TabStop = false;
            // 
            // frmCommListView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(791, 538);
            this.ControlBox = false;
            this.Controls.Add(this.grpLabel);
            this.Controls.Add(this.calAlarmTo);
            this.Controls.Add(this.calAlarmFrom);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.grbSearch);
            this.Controls.Add(this.grdAlarmList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmCommListView";
            this.Text = "frmCommListView";
            this.TopMost = true;
            this.grbSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAlarmList)).EndInit();
            this.grpLabel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAlarmDisplay;
        private System.Windows.Forms.Label lblNext;
        private System.Windows.Forms.Label lblNum10;
        private System.Windows.Forms.Label lblNum9;
        private System.Windows.Forms.Label lblNum8;
        private System.Windows.Forms.Label lblNum7;
        private System.Windows.Forms.Label lblNum6;
        private System.Windows.Forms.Label lblNum5;
        private System.Windows.Forms.Label lblNum4;
        private System.Windows.Forms.Label lblNum3;
        private System.Windows.Forms.Label lblNum2;
        private System.Windows.Forms.Label lblPrevious;
        private System.Windows.Forms.Label lblAlarmDateTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MonthCalendar calAlarmTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAlarmDateFrom;
        private System.Windows.Forms.MonthCalendar calAlarmFrom;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox grbSearch;
        protected System.Windows.Forms.DataGridView grdAlarmList;
        private System.Windows.Forms.GroupBox grpLabel;
        private System.Windows.Forms.Label lblNum1;
        internal System.Windows.Forms.ComboBox comUnitNum;
    }
}