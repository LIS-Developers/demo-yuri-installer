namespace net_yuri_installer
{
    partial class _StartWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(_StartWindow));
            this.bottomBar = new System.Windows.Forms.Label();
            this.bottomButton1 = new System.Windows.Forms.Button();
            this.bottomButton2 = new System.Windows.Forms.Button();
            this.startVideo = new AxWMPLib.AxWindowsMediaPlayer();
            this.rightTopPanel = new System.Windows.Forms.Label();
            this.licensePanel = new System.Windows.Forms.Panel();
            this.license = new System.Windows.Forms.TextBox();
            this.pathBoxPanel = new System.Windows.Forms.Panel();
            this.pathBox = new System.Windows.Forms.TextBox();
            this.installerVer = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.contentTree = new System.Windows.Forms.TreeView();
            this.pathInfo = new System.Windows.Forms.Label();
            this.InstallSetting = new System.Windows.Forms.Label();
            this.credits = new System.Windows.Forms.Label();
            this.creditBtn = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.Label();
            this.menuUpdate = new AxWMPLib.AxWindowsMediaPlayer();
            this.progressDisplay = new System.Windows.Forms.Label();
            this._progressDisplay = new System.Windows.Forms.Label();
            this.selectTips = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.startVideo)).BeginInit();
            this.licensePanel.SuspendLayout();
            this.pathBoxPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.menuUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // bottomBar
            // 
            this.bottomBar.BackColor = System.Drawing.Color.Transparent;
            this.bottomBar.ForeColor = System.Drawing.Color.Yellow;
            this.bottomBar.Location = new System.Drawing.Point(12, 574);
            this.bottomBar.Name = "bottomBar";
            this.bottomBar.Size = new System.Drawing.Size(610, 24);
            this.bottomBar.TabIndex = 0;
            this.bottomBar.Text = "label1";
            this.bottomBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bottomButton1
            // 
            this.bottomButton1.FlatAppearance.BorderSize = 0;
            this.bottomButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bottomButton1.Location = new System.Drawing.Point(645, 535);
            this.bottomButton1.Name = "bottomButton1";
            this.bottomButton1.Size = new System.Drawing.Size(156, 42);
            this.bottomButton1.TabIndex = 4;
            this.bottomButton1.TabStop = false;
            this.bottomButton1.Text = "button1";
            this.bottomButton1.UseVisualStyleBackColor = true;
            this.bottomButton1.Enter += new System.EventHandler(this.Rbtn_Enter);
            this.bottomButton1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.底部按钮1_MouseClick);
            this.bottomButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Rbtn_MouseDown);
            this.bottomButton1.MouseEnter += new System.EventHandler(this.Rbtn_MouseEnter);
            this.bottomButton1.MouseLeave += new System.EventHandler(this.Rbtn_MouseLeave);
            this.bottomButton1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Rbtn_MouseUp);
            // 
            // bottomButton2
            // 
            this.bottomButton2.FlatAppearance.BorderSize = 0;
            this.bottomButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bottomButton2.Location = new System.Drawing.Point(645, 493);
            this.bottomButton2.Name = "bottomButton2";
            this.bottomButton2.Size = new System.Drawing.Size(156, 42);
            this.bottomButton2.TabIndex = 5;
            this.bottomButton2.TabStop = false;
            this.bottomButton2.Text = "button2";
            this.bottomButton2.UseVisualStyleBackColor = true;
            this.bottomButton2.Enter += new System.EventHandler(this.Rbtn_Enter);
            this.bottomButton2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.底部按钮2_MouseClick);
            this.bottomButton2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Rbtn_MouseDown);
            this.bottomButton2.MouseEnter += new System.EventHandler(this.Rbtn_MouseEnter);
            this.bottomButton2.MouseLeave += new System.EventHandler(this.Rbtn_MouseLeave);
            this.bottomButton2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Rbtn_MouseUp);
            // 
            // startVideo
            // 
            this.startVideo.Enabled = true;
            this.startVideo.Location = new System.Drawing.Point(0, 0);
            this.startVideo.Name = "startVideo";
            this.startVideo.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("startVideo.OcxState")));
            this.startVideo.Size = new System.Drawing.Size(100, 50);
            this.startVideo.TabIndex = 7;
            this.startVideo.TabStop = false;
            // 
            // rightTopPanel
            // 
            this.rightTopPanel.BackColor = System.Drawing.Color.Transparent;
            this.rightTopPanel.Location = new System.Drawing.Point(645, 39);
            this.rightTopPanel.Name = "rightTopPanel";
            this.rightTopPanel.Size = new System.Drawing.Size(144, 112);
            this.rightTopPanel.TabIndex = 10;
            this.rightTopPanel.Text = "label1";
            // 
            // licensePanel
            // 
            this.licensePanel.BackColor = System.Drawing.Color.Black;
            this.licensePanel.Controls.Add(this.license);
            this.licensePanel.ForeColor = System.Drawing.Color.Black;
            this.licensePanel.Location = new System.Drawing.Point(66, 55);
            this.licensePanel.Name = "licensePanel";
            this.licensePanel.Size = new System.Drawing.Size(122, 102);
            this.licensePanel.TabIndex = 12;
            this.licensePanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Painting);
            // 
            // license
            // 
            this.license.BackColor = System.Drawing.Color.Black;
            this.license.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.license.ForeColor = System.Drawing.Color.Red;
            this.license.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.license.Location = new System.Drawing.Point(2, 2);
            this.license.Multiline = true;
            this.license.Name = "license";
            this.license.ReadOnly = true;
            this.license.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.license.ShortcutsEnabled = false;
            this.license.Size = new System.Drawing.Size(100, 21);
            this.license.TabIndex = 0;
            this.license.TabStop = false;
            // 
            // pathBoxPanel
            // 
            this.pathBoxPanel.BackColor = System.Drawing.Color.Black;
            this.pathBoxPanel.Controls.Add(this.pathBox);
            this.pathBoxPanel.ForeColor = System.Drawing.Color.Black;
            this.pathBoxPanel.Location = new System.Drawing.Point(64, 330);
            this.pathBoxPanel.Name = "pathBoxPanel";
            this.pathBoxPanel.Size = new System.Drawing.Size(502, 28);
            this.pathBoxPanel.TabIndex = 15;
            this.pathBoxPanel.Visible = false;
            this.pathBoxPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Painting);
            // 
            // pathBox
            // 
            this.pathBox.BackColor = System.Drawing.Color.Black;
            this.pathBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pathBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathBox.ForeColor = System.Drawing.Color.Red;
            this.pathBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.pathBox.Location = new System.Drawing.Point(4, 2);
            this.pathBox.MaximumSize = new System.Drawing.Size(500, 24);
            this.pathBox.MinimumSize = new System.Drawing.Size(0, 24);
            this.pathBox.Name = "pathBox";
            this.pathBox.ReadOnly = true;
            this.pathBox.ShortcutsEnabled = false;
            this.pathBox.Size = new System.Drawing.Size(490, 17);
            this.pathBox.TabIndex = 0;
            this.pathBox.TabStop = false;
            this.pathBox.WordWrap = false;
            this.pathBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PathBox_MouseClick);
            this.pathBox.TextChanged += new System.EventHandler(this.PathBox_TextChanged);
            this.pathBox.Enter += new System.EventHandler(this.PathBox_Enter);
            this.pathBox.Leave += new System.EventHandler(this.PathBox_Leave);
            // 
            // installerVer
            // 
            this.installerVer.BackColor = System.Drawing.Color.Transparent;
            this.installerVer.Location = new System.Drawing.Point(639, 579);
            this.installerVer.Name = "installerVer";
            this.installerVer.Size = new System.Drawing.Size(160, 20);
            this.installerVer.TabIndex = 16;
            this.installerVer.Text = "label1";
            this.installerVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.installerVer.Visible = false;
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.Color.Transparent;
            this.contentPanel.Controls.Add(this.contentTree);
            this.contentPanel.Location = new System.Drawing.Point(64, 164);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(502, 267);
            this.contentPanel.TabIndex = 20;
            this.contentPanel.Visible = false;
            this.contentPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.Painting);
            // 
            // contentTree
            // 
            this.contentTree.BackColor = System.Drawing.Color.Black;
            this.contentTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.contentTree.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.contentTree.Indent = 18;
            this.contentTree.Location = new System.Drawing.Point(2, 2);
            this.contentTree.Name = "contentTree";
            this.contentTree.ShowNodeToolTips = true;
            this.contentTree.Size = new System.Drawing.Size(471, 263);
            this.contentTree.TabIndex = 20;
            this.contentTree.TabStop = false;
            this.contentTree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.DirectoryTree_BeforeExpand);
            this.contentTree.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.DirectoryTree_AfterExpand);
            this.contentTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DirectoryTree_AfterSelect);
            this.contentTree.Enter += new System.EventHandler(this.ContentTree_Enter);
            this.contentTree.Leave += new System.EventHandler(this.ContentTree_Leave);
            // 
            // pathInfo
            // 
            this.pathInfo.AutoSize = true;
            this.pathInfo.BackColor = System.Drawing.Color.Transparent;
            this.pathInfo.Location = new System.Drawing.Point(64, 120);
            this.pathInfo.MaximumSize = new System.Drawing.Size(500, 600);
            this.pathInfo.MinimumSize = new System.Drawing.Size(500, 0);
            this.pathInfo.Name = "pathInfo";
            this.pathInfo.Size = new System.Drawing.Size(500, 12);
            this.pathInfo.TabIndex = 21;
            this.pathInfo.Text = "label1";
            this.pathInfo.Visible = false;
            // 
            // InstallSetting
            // 
            this.InstallSetting.AutoSize = true;
            this.InstallSetting.BackColor = System.Drawing.Color.Transparent;
            this.InstallSetting.Location = new System.Drawing.Point(64, 55);
            this.InstallSetting.MaximumSize = new System.Drawing.Size(500, 65535);
            this.InstallSetting.MinimumSize = new System.Drawing.Size(500, 0);
            this.InstallSetting.Name = "InstallSetting";
            this.InstallSetting.Size = new System.Drawing.Size(500, 12);
            this.InstallSetting.TabIndex = 25;
            this.InstallSetting.Text = "label1";
            this.InstallSetting.Visible = false;
            // 
            // credits
            // 
            this.credits.AutoSize = true;
            this.credits.ForeColor = System.Drawing.Color.Black;
            this.credits.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.credits.Location = new System.Drawing.Point(0, 0);
            this.credits.Name = "credits";
            this.credits.Size = new System.Drawing.Size(47, 12);
            this.credits.TabIndex = 35;
            this.credits.Text = "credits";
            this.credits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.credits.Visible = false;
            this.credits.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Credits_MouseClick);
            // 
            // creditBtn
            // 
            this.creditBtn.Location = new System.Drawing.Point(657, 3);
            this.creditBtn.Name = "creditBtn";
            this.creditBtn.Size = new System.Drawing.Size(118, 15);
            this.creditBtn.TabIndex = 36;
            this.creditBtn.Text = "creditBtn";
            this.creditBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.creditBtn.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CreditBtn_MouseClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 577);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(0, 18);
            this.progressBar1.TabIndex = 27;
            this.progressBar1.Text = "TextBar";
            this.progressBar1.Visible = false;
            // 
            // menuUpdate
            // 
            this.menuUpdate.Enabled = true;
            this.menuUpdate.Location = new System.Drawing.Point(243, 537);
            this.menuUpdate.Name = "menuUpdate";
            this.menuUpdate.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("menuUpdate.OcxState")));
            this.menuUpdate.Size = new System.Drawing.Size(75, 23);
            this.menuUpdate.TabIndex = 29;
            this.menuUpdate.Visible = false;
            // 
            // progressDisplay
            // 
            this.progressDisplay.Location = new System.Drawing.Point(0, 0);
            this.progressDisplay.Name = "progressDisplay";
            this.progressDisplay.Size = new System.Drawing.Size(619, 18);
            this.progressDisplay.TabIndex = 31;
            this.progressDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.progressDisplay.Visible = false;
            // 
            // _progressDisplay
            // 
            this._progressDisplay.Location = new System.Drawing.Point(7, 577);
            this._progressDisplay.Name = "_progressDisplay";
            this._progressDisplay.Size = new System.Drawing.Size(619, 18);
            this._progressDisplay.TabIndex = 32;
            this._progressDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._progressDisplay.Visible = false;
            // 
            // selectTips
            // 
            this.selectTips.AutoSize = true;
            this.selectTips.BackColor = System.Drawing.Color.Transparent;
            this.selectTips.Location = new System.Drawing.Point(64, 120);
            this.selectTips.MaximumSize = new System.Drawing.Size(500, 600);
            this.selectTips.MinimumSize = new System.Drawing.Size(500, 0);
            this.selectTips.Name = "selectTips";
            this.selectTips.Size = new System.Drawing.Size(500, 12);
            this.selectTips.TabIndex = 37;
            this.selectTips.Text = "label1";
            this.selectTips.Visible = false;
            // 
            // _StartWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.ControlBox = false;
            this.Controls.Add(this.selectTips);
            this.Controls.Add(this.progressDisplay);
            this.Controls.Add(this.menuUpdate);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.startVideo);
            this.Controls.Add(this.installerVer);
            this.Controls.Add(this.pathBoxPanel);
            this.Controls.Add(this.licensePanel);
            this.Controls.Add(this.bottomButton2);
            this.Controls.Add(this.bottomButton1);
            this.Controls.Add(this.rightTopPanel);
            this.Controls.Add(this.pathInfo);
            this.Controls.Add(this.InstallSetting);
            this.Controls.Add(this.credits);
            this.Controls.Add(this.creditBtn);
            this.Controls.Add(this._progressDisplay);
            this.Controls.Add(this.bottomBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "_StartWindow";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this._StartWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this._StartWindow_FormClosed);
            this.Load += new System.EventHandler(this._StartWindow_Load);
            this.MouseEnter += new System.EventHandler(this.StartWindow_MouseEnter);
            ((System.ComponentModel.ISupportInitialize)(this.startVideo)).EndInit();
            this.licensePanel.ResumeLayout(false);
            this.licensePanel.PerformLayout();
            this.pathBoxPanel.ResumeLayout(false);
            this.pathBoxPanel.PerformLayout();
            this.contentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.menuUpdate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label bottomBar;
        private System.Windows.Forms.Button bottomButton1;
        private System.Windows.Forms.Button bottomButton2;
        private AxWMPLib.AxWindowsMediaPlayer startVideo;
        private System.Windows.Forms.Label rightTopPanel;
        private System.Windows.Forms.Panel licensePanel;
        private System.Windows.Forms.TextBox license;
        private System.Windows.Forms.Panel pathBoxPanel;
        private System.Windows.Forms.TextBox pathBox;
        private System.Windows.Forms.Label installerVer;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.TreeView contentTree;
        private System.Windows.Forms.Label pathInfo;
        private System.Windows.Forms.Label InstallSetting;
        private System.Windows.Forms.Label credits;
        private System.Windows.Forms.Label creditBtn;
        private System.Windows.Forms.Label progressBar1;
        private AxWMPLib.AxWindowsMediaPlayer menuUpdate;
        private System.Windows.Forms.Label progressDisplay;
        private System.Windows.Forms.Label _progressDisplay;
        private System.Windows.Forms.Label selectTips;
    }
}

