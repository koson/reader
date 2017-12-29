namespace BBeBinder
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.m_SplashPicture = new System.Windows.Forms.PictureBox();
			this.m_VersionStr = new System.Windows.Forms.Label();
			this.m_HomeUrlLinkLabel = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.m_SplashPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// m_SplashPicture
			// 
			this.m_SplashPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.m_SplashPicture.Image = ((System.Drawing.Image)(resources.GetObject("m_SplashPicture.Image")));
			this.m_SplashPicture.Location = new System.Drawing.Point(12, 28);
			this.m_SplashPicture.Name = "m_SplashPicture";
			this.m_SplashPicture.Size = new System.Drawing.Size(356, 67);
			this.m_SplashPicture.TabIndex = 0;
			this.m_SplashPicture.TabStop = false;
			// 
			// m_VersionStr
			// 
			this.m_VersionStr.AutoSize = true;
			this.m_VersionStr.Location = new System.Drawing.Point(150, 118);
			this.m_VersionStr.Name = "m_VersionStr";
			this.m_VersionStr.Size = new System.Drawing.Size(62, 13);
			this.m_VersionStr.TabIndex = 1;
			this.m_VersionStr.Text = "Version: {0}";
			// 
			// m_HomeUrlLinkLabel
			// 
			this.m_HomeUrlLinkLabel.AutoSize = true;
			this.m_HomeUrlLinkLabel.Location = new System.Drawing.Point(95, 138);
			this.m_HomeUrlLinkLabel.Name = "m_HomeUrlLinkLabel";
			this.m_HomeUrlLinkLabel.Size = new System.Drawing.Size(188, 13);
			this.m_HomeUrlLinkLabel.TabIndex = 2;
			this.m_HomeUrlLinkLabel.TabStop = true;
			this.m_HomeUrlLinkLabel.Tag = "http://code.google.com/p/bbebinder/";
			this.m_HomeUrlLinkLabel.Text = "http://code.google.com/p/bbebinder/";
			this.m_HomeUrlLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HomeUrlLinkLabel_LinkClicked);
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(380, 179);
			this.Controls.Add(this.m_HomeUrlLinkLabel);
			this.Controls.Add(this.m_VersionStr);
			this.Controls.Add(this.m_SplashPicture);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About BBeB Binder";
			this.Load += new System.EventHandler(this.AboutForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.m_SplashPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox m_SplashPicture;
		private System.Windows.Forms.Label m_VersionStr;
		private System.Windows.Forms.LinkLabel m_HomeUrlLinkLabel;
	}
}