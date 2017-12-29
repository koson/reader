using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Windows.Forms;


namespace BBeBinder
{
	public partial class AboutForm : Form
	{
		public AboutForm()
		{
			InitializeComponent();
		}

		private void AboutForm_Load(object sender, EventArgs e)
		{
			m_VersionStr.Text = string.Format(m_VersionStr.Text, 
				Assembly.GetExecutingAssembly().GetName().Version.ToString());
		}

		private void HomeUrlLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(m_HomeUrlLinkLabel.Text);
		}
	}
}