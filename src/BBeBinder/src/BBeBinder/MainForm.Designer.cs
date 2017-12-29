namespace BBeBinder
{
    partial class MainForm
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

            // Close Plugins
            PluginServices.Instance.ClosePlugins();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.m_MainMenu = new System.Windows.Forms.MenuStrip();
            this.m_FileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.m_OpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ImportFromClipboardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_SaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_ToggleEditMode = new System.Windows.Forms.ToolStripMenuItem();
            this.m_PageToImage = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_TocTree = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.m_HtmlEditor = new BBeBinder.HtmlEditor();
            this.m_TabCtl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.m_PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.m_TocRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.StatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_NewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_MainMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.m_TabCtl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_MainMenu
            // 
            this.m_MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_FileMenu,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.m_MainMenu.Location = new System.Drawing.Point(0, 0);
            this.m_MainMenu.Name = "m_MainMenu";
            this.m_MainMenu.Size = new System.Drawing.Size(1019, 24);
            this.m_MainMenu.TabIndex = 1;
            this.m_MainMenu.Text = "Main Menu";
            // 
            // m_FileMenu
            // 
            this.m_FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_NewMenuItem,
            this.toolStripMenuItem1,
            this.m_OpenMenuItem,
            this.m_ImportFromClipboardMenuItem,
            this.toolStripSeparator1,
            this.m_SaveAsMenuItem,
            this.toolStripMenuItem2,
            this.m_ExitMenuItem});
            this.m_FileMenu.Name = "m_FileMenu";
            this.m_FileMenu.Size = new System.Drawing.Size(37, 20);
            this.m_FileMenu.Text = "&File";
            // 
            // m_OpenMenuItem
            // 
            this.m_OpenMenuItem.Name = "m_OpenMenuItem";
            this.m_OpenMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.m_OpenMenuItem.Size = new System.Drawing.Size(194, 22);
            this.m_OpenMenuItem.Text = "&Open";
            // 
            // m_ImportFromClipboardMenuItem
            // 
            this.m_ImportFromClipboardMenuItem.Name = "m_ImportFromClipboardMenuItem";
            this.m_ImportFromClipboardMenuItem.Size = new System.Drawing.Size(194, 22);
            this.m_ImportFromClipboardMenuItem.Text = "Import from Clipboard";
            this.m_ImportFromClipboardMenuItem.Click += new System.EventHandler(this.ImportFromClipboardMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(191, 6);
            // 
            // m_SaveAsMenuItem
            // 
            this.m_SaveAsMenuItem.Name = "m_SaveAsMenuItem";
            this.m_SaveAsMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.m_SaveAsMenuItem.Size = new System.Drawing.Size(194, 22);
            this.m_SaveAsMenuItem.Text = "&Save As";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(191, 6);
            // 
            // m_ExitMenuItem
            // 
            this.m_ExitMenuItem.Name = "m_ExitMenuItem";
            this.m_ExitMenuItem.Size = new System.Drawing.Size(194, 22);
            this.m_ExitMenuItem.Text = "E&xit";
            this.m_ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_ToggleEditMode,
            this.m_PageToImage});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // m_ToggleEditMode
            // 
            this.m_ToggleEditMode.Name = "m_ToggleEditMode";
            this.m_ToggleEditMode.Size = new System.Drawing.Size(195, 22);
            this.m_ToggleEditMode.Text = "&Design Mode";
            this.m_ToggleEditMode.Click += new System.EventHandler(this.m_ToggleEditMode_Click);
            // 
            // m_PageToImage
            // 
            this.m_PageToImage.Name = "m_PageToImage";
            this.m_PageToImage.Size = new System.Drawing.Size(195, 22);
            this.m_PageToImage.Text = "&Convert page to image";
            this.m_PageToImage.Click += new System.EventHandler(this.m_PageToImage_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // m_TocTree
            // 
            this.m_TocTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_TocTree.Location = new System.Drawing.Point(3, 3);
            this.m_TocTree.Name = "m_TocTree";
            this.m_TocTree.Size = new System.Drawing.Size(286, 410);
            this.m_TocTree.TabIndex = 4;
            this.m_TocTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TocTree_AfterSelect);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.m_HtmlEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.m_TabCtl);
            this.splitContainer1.Size = new System.Drawing.Size(1019, 442);
            this.splitContainer1.SplitterDistance = 715;
            this.splitContainer1.TabIndex = 5;
            // 
            // m_HtmlEditor
            // 
            this.m_HtmlEditor.BodyHtml = null;
            this.m_HtmlEditor.BodyText = null;
            this.m_HtmlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_HtmlEditor.DocumentText = "<HTML></HTML>\0";
            this.m_HtmlEditor.EditorBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.m_HtmlEditor.EditorForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.m_HtmlEditor.FontName = null;
            this.m_HtmlEditor.FontSize = BBeBinder.FontSize.NA;
            this.m_HtmlEditor.Location = new System.Drawing.Point(0, 0);
            this.m_HtmlEditor.Name = "m_HtmlEditor";
            this.m_HtmlEditor.SetDesignMode = false;
            this.m_HtmlEditor.Size = new System.Drawing.Size(715, 442);
            this.m_HtmlEditor.StyleName = null;
            this.m_HtmlEditor.TabIndex = 3;
            this.m_HtmlEditor.Load += new System.EventHandler(this.m_HtmlEditor_Load);
            // 
            // m_TabCtl
            // 
            this.m_TabCtl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.m_TabCtl.Controls.Add(this.tabPage1);
            this.m_TabCtl.Controls.Add(this.tabPage2);
            this.m_TabCtl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_TabCtl.Location = new System.Drawing.Point(0, 0);
            this.m_TabCtl.Name = "m_TabCtl";
            this.m_TabCtl.SelectedIndex = 0;
            this.m_TabCtl.Size = new System.Drawing.Size(300, 442);
            this.m_TabCtl.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.m_PropertyGrid);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(292, 416);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Properties";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // m_PropertyGrid
            // 
            this.m_PropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_PropertyGrid.Location = new System.Drawing.Point(3, 3);
            this.m_PropertyGrid.Name = "m_PropertyGrid";
            this.m_PropertyGrid.Size = new System.Drawing.Size(286, 410);
            this.m_PropertyGrid.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.m_TocTree);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(292, 416);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Table of Contents";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // m_TocRefreshTimer
            // 
            this.m_TocRefreshTimer.Interval = 500;
            this.m_TocRefreshTimer.Tick += new System.EventHandler(this.TocTimer_Tick);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusText});
            this.statusBar.Location = new System.Drawing.Point(0, 466);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1019, 22);
            this.statusBar.TabIndex = 6;
            this.statusBar.Text = "statusStrip1";
            // 
            // StatusText
            // 
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(0, 17);
            // 
            // m_NewMenuItem
            // 
            this.m_NewMenuItem.Name = "m_NewMenuItem";
            this.m_NewMenuItem.Size = new System.Drawing.Size(194, 22);
            this.m_NewMenuItem.Text = "&New";
            this.m_NewMenuItem.Click += new System.EventHandler(this.m_NewMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(191, 6);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 488);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.m_MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.m_MainMenu;
            this.Name = "MainForm";
            this.Text = "BBeBinder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.m_MainMenu.ResumeLayout(false);
            this.m_MainMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.m_TabCtl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip m_MainMenu;
        private System.Windows.Forms.ToolStripMenuItem m_FileMenu;
        private System.Windows.Forms.ToolStripMenuItem m_OpenMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem m_ExitMenuItem;
        private System.Windows.Forms.TreeView m_TocTree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private HtmlEditor m_HtmlEditor;
        private System.Windows.Forms.TabControl m_TabCtl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer m_TocRefreshTimer;
		private System.Windows.Forms.PropertyGrid m_PropertyGrid;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_SaveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_ToggleEditMode;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel StatusText;
        private System.Windows.Forms.ToolStripMenuItem m_ImportFromClipboardMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_PageToImage;
        private System.Windows.Forms.ToolStripMenuItem m_NewMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

