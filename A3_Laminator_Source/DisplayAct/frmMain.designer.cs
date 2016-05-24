namespace DisplayAct
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.panelCommand = new System.Windows.Forms.Panel();
            this.panelNavigation = new System.Windows.Forms.Panel();
            this.tmrMainView = new System.Windows.Forms.Timer(this.components);
            this.btnExit = new System.Windows.Forms.Button();
            this.subfrmTitlePanel = new DisplayAct.subfrmTitlePanel();
            this.subfrmCaption = new DisplayAct.subfrmCaption();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel1.Controls.Add(this.panelInfo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelCommand, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelNavigation, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.subfrmTitlePanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.subfrmCaption, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnExit, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1274, 996);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelInfo
            // 
            this.panelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInfo.Location = new System.Drawing.Point(1, 111);
            this.panelInfo.Margin = new System.Windows.Forms.Padding(1);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(1185, 834);
            this.panelInfo.TabIndex = 7;
            // 
            // panelCommand
            // 
            this.panelCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCommand.Location = new System.Drawing.Point(1188, 111);
            this.panelCommand.Margin = new System.Windows.Forms.Padding(1);
            this.panelCommand.Name = "panelCommand";
            this.panelCommand.Size = new System.Drawing.Size(85, 834);
            this.panelCommand.TabIndex = 8;
            // 
            // panelNavigation
            // 
            this.panelNavigation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelNavigation.Location = new System.Drawing.Point(1, 947);
            this.panelNavigation.Margin = new System.Windows.Forms.Padding(1);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(1185, 48);
            this.panelNavigation.TabIndex = 9;
            // 
            // tmrMainView
            // 
            this.tmrMainView.Enabled = true;
            this.tmrMainView.Interval = 1000;
            this.tmrMainView.Tick += new System.EventHandler(this.tmrMainView_Tick);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = global::DisplayAct.Properties.Resources.EXIT;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.Location = new System.Drawing.Point(1187, 946);
            this.btnExit.Margin = new System.Windows.Forms.Padding(0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(87, 50);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = " ";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // subfrmTitlePanel
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.subfrmTitlePanel, 2);
            this.subfrmTitlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subfrmTitlePanel.Location = new System.Drawing.Point(0, 0);
            this.subfrmTitlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.subfrmTitlePanel.Name = "subfrmTitlePanel";
            this.subfrmTitlePanel.Size = new System.Drawing.Size(1274, 30);
            this.subfrmTitlePanel.TabIndex = 10;
            // 
            // subfrmCaption
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.subfrmCaption, 2);
            this.subfrmCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subfrmCaption.Location = new System.Drawing.Point(0, 30);
            this.subfrmCaption.Margin = new System.Windows.Forms.Padding(0);
            this.subfrmCaption.Name = "subfrmCaption";
            this.subfrmCaption.Size = new System.Drawing.Size(1274, 80);
            this.subfrmCaption.TabIndex = 11;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(1274, 996);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CIM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMainNew_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMainNew_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer tmrMainView;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel panelCommand;
        private System.Windows.Forms.Panel panelNavigation;
        private System.Windows.Forms.Button btnExit;
        private subfrmTitlePanel subfrmTitlePanel;
        private subfrmCaption subfrmCaption;
    }
}