﻿namespace sgsubdotnet
{
    partial class SGSMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SGSMainForm));
            this.hSpliter = new System.Windows.Forms.SplitContainer();
            this.vSpliter = new System.Windows.Forms.SplitContainer();
            this.axWMP = new AxWMPLib.AxWindowsMediaPlayer();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.smallSpliter = new System.Windows.Forms.SplitContainer();
            this.subLabel = new System.Windows.Forms.Label();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.subtitleGrid = new System.Windows.Forms.DataGridView();
            this.subToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenSubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenVideoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveSubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsSubToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.toolStripPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripPlay = new System.Windows.Forms.ToolStripButton();
            this.toolStripDuplicate = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripJumpto = new System.Windows.Forms.ToolStripButton();
            this.hSpliter.Panel1.SuspendLayout();
            this.hSpliter.Panel2.SuspendLayout();
            this.hSpliter.SuspendLayout();
            this.vSpliter.Panel1.SuspendLayout();
            this.vSpliter.Panel2.SuspendLayout();
            this.vSpliter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).BeginInit();
            this.smallSpliter.Panel1.SuspendLayout();
            this.smallSpliter.Panel2.SuspendLayout();
            this.smallSpliter.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.LeftToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subtitleGrid)).BeginInit();
            this.subToolStrip.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // hSpliter
            // 
            this.hSpliter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hSpliter.Location = new System.Drawing.Point(0, 28);
            this.hSpliter.Margin = new System.Windows.Forms.Padding(4);
            this.hSpliter.Name = "hSpliter";
            this.hSpliter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // hSpliter.Panel1
            // 
            this.hSpliter.Panel1.Controls.Add(this.vSpliter);
            // 
            // hSpliter.Panel2
            // 
            this.hSpliter.Panel2.Controls.Add(this.smallSpliter);
            this.hSpliter.Size = new System.Drawing.Size(979, 701);
            this.hSpliter.SplitterDistance = 410;
            this.hSpliter.SplitterWidth = 5;
            this.hSpliter.TabIndex = 0;
            // 
            // vSpliter
            // 
            this.vSpliter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vSpliter.Location = new System.Drawing.Point(0, 0);
            this.vSpliter.Margin = new System.Windows.Forms.Padding(4);
            this.vSpliter.Name = "vSpliter";
            // 
            // vSpliter.Panel1
            // 
            this.vSpliter.Panel1.Controls.Add(this.axWMP);
            // 
            // vSpliter.Panel2
            // 
            this.vSpliter.Panel2.Controls.Add(this.button4);
            this.vSpliter.Panel2.Controls.Add(this.button2);
            this.vSpliter.Panel2.Controls.Add(this.button1);
            this.vSpliter.Size = new System.Drawing.Size(979, 410);
            this.vSpliter.SplitterDistance = 508;
            this.vSpliter.SplitterWidth = 5;
            this.vSpliter.TabIndex = 0;
            // 
            // axWMP
            // 
            this.axWMP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWMP.Enabled = true;
            this.axWMP.Location = new System.Drawing.Point(0, 0);
            this.axWMP.Margin = new System.Windows.Forms.Padding(4);
            this.axWMP.Name = "axWMP";
            this.axWMP.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWMP.OcxState")));
            this.axWMP.Size = new System.Drawing.Size(508, 410);
            this.axWMP.TabIndex = 0;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(32, 96);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 29);
            this.button4.TabIndex = 3;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(32, 60);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 29);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 24);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // smallSpliter
            // 
            this.smallSpliter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.smallSpliter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.smallSpliter.IsSplitterFixed = true;
            this.smallSpliter.Location = new System.Drawing.Point(0, 0);
            this.smallSpliter.Margin = new System.Windows.Forms.Padding(4);
            this.smallSpliter.Name = "smallSpliter";
            this.smallSpliter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // smallSpliter.Panel1
            // 
            this.smallSpliter.Panel1.Controls.Add(this.subLabel);
            // 
            // smallSpliter.Panel2
            // 
            this.smallSpliter.Panel2.Controls.Add(this.toolStripContainer1);
            this.smallSpliter.Size = new System.Drawing.Size(979, 286);
            this.smallSpliter.SplitterDistance = 53;
            this.smallSpliter.SplitterWidth = 5;
            this.smallSpliter.TabIndex = 1;
            // 
            // subLabel
            // 
            this.subLabel.AutoSize = true;
            this.subLabel.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.subLabel.Location = new System.Drawing.Point(67, 11);
            this.subLabel.Name = "subLabel";
            this.subLabel.Size = new System.Drawing.Size(319, 20);
            this.subLabel.TabIndex = 0;
            this.subLabel.Text = "Subtitle will be diplayed here.";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.subtitleGrid);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(955, 203);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.Controls.Add(this.subToolStrip);
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(979, 228);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // subtitleGrid
            // 
            this.subtitleGrid.AllowUserToAddRows = false;
            this.subtitleGrid.AllowUserToDeleteRows = false;
            this.subtitleGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.subtitleGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subtitleGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.subtitleGrid.Location = new System.Drawing.Point(0, 0);
            this.subtitleGrid.Margin = new System.Windows.Forms.Padding(4);
            this.subtitleGrid.MultiSelect = false;
            this.subtitleGrid.Name = "subtitleGrid";
            this.subtitleGrid.RowTemplate.Height = 23;
            this.subtitleGrid.Size = new System.Drawing.Size(955, 203);
            this.subtitleGrid.TabIndex = 0;
            this.subtitleGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.subtitleGrid_CellBeginEdit);
            this.subtitleGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.subtitleGrid_CellEndEdit);
            this.subtitleGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.subtitleGrid_KeyDown);
            this.subtitleGrid.KeyUp += new System.Windows.Forms.KeyEventHandler(this.subtitleGrid_KeyUp);
            // 
            // subToolStrip
            // 
            this.subToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.subToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripPause,
            this.toolStripPlay,
            this.toolStripSeparator1,
            this.toolStripDuplicate,
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripJumpto});
            this.subToolStrip.Location = new System.Drawing.Point(0, 3);
            this.subToolStrip.Name = "subToolStrip";
            this.subToolStrip.Size = new System.Drawing.Size(24, 138);
            this.subToolStrip.TabIndex = 0;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(30, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(30, 6);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.设置ToolStripMenuItem,
            this.关于ToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.mainMenu.Size = new System.Drawing.Size(979, 28);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenSubToolStripMenuItem,
            this.OpenVideoToolStripMenuItem,
            this.SaveSubToolStripMenuItem,
            this.SaveAsSubToolStripMenuItem1});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.FileToolStripMenuItem.Text = "文件";
            // 
            // OpenSubToolStripMenuItem
            // 
            this.OpenSubToolStripMenuItem.Name = "OpenSubToolStripMenuItem";
            this.OpenSubToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.OpenSubToolStripMenuItem.Text = "打开时间轴";
            this.OpenSubToolStripMenuItem.Click += new System.EventHandler(this.OpenSubToolStripMenuItem_Click);
            // 
            // OpenVideoToolStripMenuItem
            // 
            this.OpenVideoToolStripMenuItem.Name = "OpenVideoToolStripMenuItem";
            this.OpenVideoToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.OpenVideoToolStripMenuItem.Text = "打开动画";
            this.OpenVideoToolStripMenuItem.Click += new System.EventHandler(this.OpenVideoToolStripMenuItem_Click);
            // 
            // SaveSubToolStripMenuItem
            // 
            this.SaveSubToolStripMenuItem.Name = "SaveSubToolStripMenuItem";
            this.SaveSubToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.SaveSubToolStripMenuItem.Text = "保存时间轴";
            // 
            // SaveAsSubToolStripMenuItem1
            // 
            this.SaveAsSubToolStripMenuItem1.Name = "SaveAsSubToolStripMenuItem1";
            this.SaveAsSubToolStripMenuItem1.Size = new System.Drawing.Size(168, 24);
            this.SaveAsSubToolStripMenuItem1.Text = "另存为时间轴";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.设置ToolStripMenuItem.Text = "设置";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.关于ToolStripMenuItem.Text = "关于";
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 707);
            this.statusBar.Name = "statusBar";
            this.statusBar.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusBar.Size = new System.Drawing.Size(979, 22);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "Status";
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // toolStripPause
            // 
            this.toolStripPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPause.Image = global::sgsubdotnet.Properties.Resources.Pause;
            this.toolStripPause.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPause.Name = "toolStripPause";
            this.toolStripPause.Size = new System.Drawing.Size(30, 20);
            this.toolStripPause.Text = "Pause";
            this.toolStripPause.Click += new System.EventHandler(this.toolStripPause_Click);
            // 
            // toolStripPlay
            // 
            this.toolStripPlay.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPlay.Image = global::sgsubdotnet.Properties.Resources.Run;
            this.toolStripPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPlay.Name = "toolStripPlay";
            this.toolStripPlay.Size = new System.Drawing.Size(30, 20);
            this.toolStripPlay.Text = "Play";
            this.toolStripPlay.Click += new System.EventHandler(this.toolStripPlay_Click);
            // 
            // toolStripDuplicate
            // 
            this.toolStripDuplicate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDuplicate.Image = global::sgsubdotnet.Properties.Resources.copy;
            this.toolStripDuplicate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDuplicate.Name = "toolStripDuplicate";
            this.toolStripDuplicate.Size = new System.Drawing.Size(30, 20);
            this.toolStripDuplicate.Text = "Duplicate";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::sgsubdotnet.Properties.Resources.delete;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(30, 20);
            this.toolStripButton1.Text = "Delete";
            // 
            // toolStripJumpto
            // 
            this.toolStripJumpto.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripJumpto.Image = global::sgsubdotnet.Properties.Resources.jumpto;
            this.toolStripJumpto.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripJumpto.Name = "toolStripJumpto";
            this.toolStripJumpto.Size = new System.Drawing.Size(30, 20);
            this.toolStripJumpto.Text = "Jump to";
            this.toolStripJumpto.Click += new System.EventHandler(this.toolStripJumpto_Click);
            // 
            // SGSMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 729);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.hSpliter);
            this.Controls.Add(this.mainMenu);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SGSMainForm";
            this.Text = "SGSUB.Net";
            this.hSpliter.Panel1.ResumeLayout(false);
            this.hSpliter.Panel2.ResumeLayout(false);
            this.hSpliter.ResumeLayout(false);
            this.vSpliter.Panel1.ResumeLayout(false);
            this.vSpliter.Panel2.ResumeLayout(false);
            this.vSpliter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).EndInit();
            this.smallSpliter.Panel1.ResumeLayout(false);
            this.smallSpliter.Panel1.PerformLayout();
            this.smallSpliter.Panel2.ResumeLayout(false);
            this.smallSpliter.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.LeftToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.LeftToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subtitleGrid)).EndInit();
            this.subToolStrip.ResumeLayout(false);
            this.subToolStrip.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer hSpliter;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.SplitContainer vSpliter;
        private AxWMPLib.AxWindowsMediaPlayer axWMP;
        private System.Windows.Forms.DataGridView subtitleGrid;
        private System.Windows.Forms.SplitContainer smallSpliter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label subLabel;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenSubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenVideoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveSubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsSubToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip subToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripPause;
        private System.Windows.Forms.ToolStripButton toolStripPlay;
        private System.Windows.Forms.ToolStripButton toolStripDuplicate;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripJumpto;
    }
}

