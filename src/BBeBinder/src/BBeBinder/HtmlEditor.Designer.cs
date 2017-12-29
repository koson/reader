namespace BBeBinder
{
    partial class HtmlEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HtmlEditor));
			this.m_BrowserToolstrip = new System.Windows.Forms.ToolStrip();
			this.m_StyleComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.m_FontFaceComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.m_FontSizeComboBox = new System.Windows.Forms.ToolStripComboBox();
			this.m_BoldBtn = new System.Windows.Forms.ToolStripButton();
			this.m_ItalicBtn = new System.Windows.Forms.ToolStripButton();
			this.m_UnderlineBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.m_TextColorBtn = new System.Windows.Forms.ToolStripButton();
			this.m_BkgndColorBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.m_HyperlinkBtn = new System.Windows.Forms.ToolStripButton();
			this.m_ImageBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.m_LeftJustBtn = new System.Windows.Forms.ToolStripButton();
			this.m_CenterJustBtn = new System.Windows.Forms.ToolStripButton();
			this.m_RightJustBtn = new System.Windows.Forms.ToolStripButton();
			this.m_FullJustBtn = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.m_OrderedListBtn = new System.Windows.Forms.ToolStripButton();
			this.m_UnorderedListBtn = new System.Windows.Forms.ToolStripButton();
			this.m_OutdentBtn = new System.Windows.Forms.ToolStripButton();
			this.m_IndentBtn = new System.Windows.Forms.ToolStripButton();
			this.m_Browser = new System.Windows.Forms.WebBrowser();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.m_BrowserToolstrip.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_BrowserToolstrip
			// 
			this.m_BrowserToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_StyleComboBox,
            this.m_FontFaceComboBox,
            this.m_FontSizeComboBox,
            this.m_BoldBtn,
            this.m_ItalicBtn,
            this.m_UnderlineBtn,
            this.toolStripSeparator1,
            this.m_TextColorBtn,
            this.m_BkgndColorBtn,
            this.toolStripSeparator2,
            this.m_HyperlinkBtn,
            this.m_ImageBtn,
            this.toolStripSeparator3,
            this.m_LeftJustBtn,
            this.m_CenterJustBtn,
            this.m_RightJustBtn,
            this.m_FullJustBtn,
            this.toolStripSeparator4,
            this.m_OrderedListBtn,
            this.m_UnorderedListBtn,
            this.m_OutdentBtn,
            this.m_IndentBtn});
			this.m_BrowserToolstrip.Location = new System.Drawing.Point(0, 0);
			this.m_BrowserToolstrip.Name = "m_BrowserToolstrip";
			this.m_BrowserToolstrip.Size = new System.Drawing.Size(725, 25);
			this.m_BrowserToolstrip.TabIndex = 0;
			this.m_BrowserToolstrip.Text = "toolStrip1";
			// 
			// m_StyleComboBox
			// 
			this.m_StyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.m_StyleComboBox.Items.AddRange(new object[] {
            "Normal",
            "Heading 1",
            "Heading 2",
            "Heading 3",
            "Heading 4",
            "Heading 5",
            "Heading 6",
            "Formatted"});
			this.m_StyleComboBox.Name = "m_StyleComboBox";
			this.m_StyleComboBox.Size = new System.Drawing.Size(121, 25);
			// 
			// m_FontFaceComboBox
			// 
			this.m_FontFaceComboBox.Name = "m_FontFaceComboBox";
			this.m_FontFaceComboBox.Size = new System.Drawing.Size(121, 25);
			// 
			// m_FontSizeComboBox
			// 
			this.m_FontSizeComboBox.Name = "m_FontSizeComboBox";
			this.m_FontSizeComboBox.Size = new System.Drawing.Size(75, 25);
			// 
			// m_BoldBtn
			// 
			this.m_BoldBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_BoldBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_BoldBtn.Image")));
			this.m_BoldBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_BoldBtn.Name = "m_BoldBtn";
			this.m_BoldBtn.Size = new System.Drawing.Size(23, 22);
			this.m_BoldBtn.Text = "toolStripButton1";
			this.m_BoldBtn.Click += new System.EventHandler(this.boldButton_Click);
			// 
			// m_ItalicBtn
			// 
			this.m_ItalicBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_ItalicBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_ItalicBtn.Image")));
			this.m_ItalicBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ItalicBtn.Name = "m_ItalicBtn";
			this.m_ItalicBtn.Size = new System.Drawing.Size(23, 22);
			this.m_ItalicBtn.Text = "toolStripButton2";
			this.m_ItalicBtn.Click += new System.EventHandler(this.italicButton_Click);
			// 
			// m_UnderlineBtn
			// 
			this.m_UnderlineBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_UnderlineBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_UnderlineBtn.Image")));
			this.m_UnderlineBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_UnderlineBtn.Name = "m_UnderlineBtn";
			this.m_UnderlineBtn.Size = new System.Drawing.Size(23, 22);
			this.m_UnderlineBtn.Text = "toolStripButton3";
			this.m_UnderlineBtn.Click += new System.EventHandler(this.underlineButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// m_TextColorBtn
			// 
			this.m_TextColorBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_TextColorBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_TextColorBtn.Image")));
			this.m_TextColorBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_TextColorBtn.Name = "m_TextColorBtn";
			this.m_TextColorBtn.Size = new System.Drawing.Size(23, 22);
			this.m_TextColorBtn.Text = "toolStripButton4";
			this.m_TextColorBtn.Click += new System.EventHandler(this.colorButton_Click);
			// 
			// m_BkgndColorBtn
			// 
			this.m_BkgndColorBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_BkgndColorBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_BkgndColorBtn.Image")));
			this.m_BkgndColorBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_BkgndColorBtn.Name = "m_BkgndColorBtn";
			this.m_BkgndColorBtn.Size = new System.Drawing.Size(23, 22);
			this.m_BkgndColorBtn.Text = "toolStripButton5";
			this.m_BkgndColorBtn.Click += new System.EventHandler(this.backColorButton_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// m_HyperlinkBtn
			// 
			this.m_HyperlinkBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_HyperlinkBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_HyperlinkBtn.Image")));
			this.m_HyperlinkBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_HyperlinkBtn.Name = "m_HyperlinkBtn";
			this.m_HyperlinkBtn.Size = new System.Drawing.Size(23, 22);
			this.m_HyperlinkBtn.Text = "toolStripButton6";
			this.m_HyperlinkBtn.Click += new System.EventHandler(this.linkButton_Click);
			// 
			// m_ImageBtn
			// 
			this.m_ImageBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_ImageBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_ImageBtn.Image")));
			this.m_ImageBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_ImageBtn.Name = "m_ImageBtn";
			this.m_ImageBtn.Size = new System.Drawing.Size(23, 22);
			this.m_ImageBtn.Text = "toolStripButton7";
			this.m_ImageBtn.Click += new System.EventHandler(this.imageButton_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// m_LeftJustBtn
			// 
			this.m_LeftJustBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_LeftJustBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_LeftJustBtn.Image")));
			this.m_LeftJustBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_LeftJustBtn.Name = "m_LeftJustBtn";
			this.m_LeftJustBtn.Size = new System.Drawing.Size(23, 22);
			this.m_LeftJustBtn.Text = "toolStripButton8";
			this.m_LeftJustBtn.Click += new System.EventHandler(this.justifyLeftButton_Click);
			// 
			// m_CenterJustBtn
			// 
			this.m_CenterJustBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_CenterJustBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_CenterJustBtn.Image")));
			this.m_CenterJustBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_CenterJustBtn.Name = "m_CenterJustBtn";
			this.m_CenterJustBtn.Size = new System.Drawing.Size(23, 22);
			this.m_CenterJustBtn.Text = "toolStripButton9";
			this.m_CenterJustBtn.Click += new System.EventHandler(this.justifyCenterButton_Click);
			// 
			// m_RightJustBtn
			// 
			this.m_RightJustBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_RightJustBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_RightJustBtn.Image")));
			this.m_RightJustBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_RightJustBtn.Name = "m_RightJustBtn";
			this.m_RightJustBtn.Size = new System.Drawing.Size(23, 22);
			this.m_RightJustBtn.Text = "toolStripButton10";
			this.m_RightJustBtn.Click += new System.EventHandler(this.justifyRightButton_Click);
			// 
			// m_FullJustBtn
			// 
			this.m_FullJustBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_FullJustBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_FullJustBtn.Image")));
			this.m_FullJustBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_FullJustBtn.Name = "m_FullJustBtn";
			this.m_FullJustBtn.Size = new System.Drawing.Size(23, 22);
			this.m_FullJustBtn.Text = "toolStripButton11";
			this.m_FullJustBtn.Click += new System.EventHandler(this.justifyFullButton_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// m_OrderedListBtn
			// 
			this.m_OrderedListBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_OrderedListBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_OrderedListBtn.Image")));
			this.m_OrderedListBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_OrderedListBtn.Name = "m_OrderedListBtn";
			this.m_OrderedListBtn.Size = new System.Drawing.Size(23, 22);
			this.m_OrderedListBtn.Text = "toolStripButton12";
			this.m_OrderedListBtn.Click += new System.EventHandler(this.orderedListButton_Click);
			// 
			// m_UnorderedListBtn
			// 
			this.m_UnorderedListBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_UnorderedListBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_UnorderedListBtn.Image")));
			this.m_UnorderedListBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_UnorderedListBtn.Name = "m_UnorderedListBtn";
			this.m_UnorderedListBtn.Size = new System.Drawing.Size(23, 22);
			this.m_UnorderedListBtn.Text = "toolStripButton13";
			this.m_UnorderedListBtn.Click += new System.EventHandler(this.unorderedListButton_Click);
			// 
			// m_OutdentBtn
			// 
			this.m_OutdentBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_OutdentBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_OutdentBtn.Image")));
			this.m_OutdentBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_OutdentBtn.Name = "m_OutdentBtn";
			this.m_OutdentBtn.Size = new System.Drawing.Size(23, 22);
			this.m_OutdentBtn.Text = "toolStripButton14";
			this.m_OutdentBtn.Click += new System.EventHandler(this.outdentButton_Click);
			// 
			// m_IndentBtn
			// 
			this.m_IndentBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_IndentBtn.Image = ((System.Drawing.Image)(resources.GetObject("m_IndentBtn.Image")));
			this.m_IndentBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.m_IndentBtn.Name = "m_IndentBtn";
			this.m_IndentBtn.Size = new System.Drawing.Size(23, 22);
			this.m_IndentBtn.Text = "toolStripButton15";
			this.m_IndentBtn.Click += new System.EventHandler(this.indentButton_Click);
			// 
			// m_Browser
			// 
			this.m_Browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_Browser.Location = new System.Drawing.Point(0, 25);
			this.m_Browser.MinimumSize = new System.Drawing.Size(20, 20);
			this.m_Browser.Name = "m_Browser";
			this.m_Browser.Size = new System.Drawing.Size(725, 228);
			this.m_Browser.TabIndex = 1;
			this.m_Browser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.m_Browser_Navigated);
			this.m_Browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Browser_DocumentCompleted);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(106, 92);
			// 
			// cutToolStripMenuItem
			// 
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.cutToolStripMenuItem.Text = "Cut";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(105, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			// 
			// HtmlEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_Browser);
			this.Controls.Add(this.m_BrowserToolstrip);
			this.Name = "HtmlEditor";
			this.Size = new System.Drawing.Size(725, 253);
			this.m_BrowserToolstrip.ResumeLayout(false);
			this.m_BrowserToolstrip.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip m_BrowserToolstrip;
        private System.Windows.Forms.WebBrowser m_Browser;
        private System.Windows.Forms.ToolStripComboBox m_FontFaceComboBox;
        private System.Windows.Forms.ToolStripComboBox m_FontSizeComboBox;
        private System.Windows.Forms.ToolStripButton m_BoldBtn;
        private System.Windows.Forms.ToolStripButton m_ItalicBtn;
        private System.Windows.Forms.ToolStripButton m_UnderlineBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton m_TextColorBtn;
        private System.Windows.Forms.ToolStripButton m_BkgndColorBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton m_HyperlinkBtn;
        private System.Windows.Forms.ToolStripButton m_ImageBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton m_LeftJustBtn;
        private System.Windows.Forms.ToolStripButton m_CenterJustBtn;
        private System.Windows.Forms.ToolStripButton m_RightJustBtn;
        private System.Windows.Forms.ToolStripButton m_FullJustBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton m_OrderedListBtn;
        private System.Windows.Forms.ToolStripButton m_UnorderedListBtn;
        private System.Windows.Forms.ToolStripButton m_OutdentBtn;
        private System.Windows.Forms.ToolStripButton m_IndentBtn;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripComboBox m_StyleComboBox;
    }
}
