/* 
 * Copyright (C) 2006, Chris Mumford cmumford@cmumford.com
 * 
 * This code originated in a sample program posted to codeproject.com
 * by kevin delafield in the article titled "A Windows Forms based text
 * editor with HTML output". http://www.codeproject.com/cs/miscctrl/editor_in_windows_forms.asp
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BBeBinder
{
    public partial class LinkDialog : Form
    {
        private bool _accepted = false;

        public LinkDialog()
        {
            InitializeComponent();
            LoadUrls();
            linkEdit.TextChanged += new EventHandler(linkEdit_TextChanged);
        }

        void linkEdit_TextChanged(object sender, EventArgs e)
        {
            label1.Text = URL;
        }

        public string URL
        {
            get
            {
                return comboBox1.Text + linkEdit.Text.Trim();
            }
        }

        public string URI
        {
            get
            {
                return linkEdit.Text.Trim();
            }
        }

        public bool Accepted
        {
            get
            {
                return _accepted;
            }
        }

        private void LinkDialog_Load(object sender, EventArgs e)
        {
            label1.Text = URL;
            BeginInvoke((MethodInvoker)delegate
            {
                linkEdit.Focus();
            });
        }

        private void LoadUrls()
        {
            string glob = Properties.Settings.Default.LinkDialogURLs;
            string[] urls = glob.Split(null);
            if (urls != null)
            {
                foreach (string url in urls)
                {
                    linkEdit.Items.Add(url);
                }
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            string url = linkEdit.Text;
            string glob = Properties.Settings.Default.LinkDialogURLs;
            if (glob == null) glob = "";
            if (!glob.Contains(url))
            {
                if (glob.Length > 0)
                    glob += "\n";
                glob += url;
            }
            Properties.Settings.Default.LinkDialogURLs = glob;
            Properties.Settings.Default.Save();
            _accepted = true;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _accepted = false;
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = URL;
        }
    }
}