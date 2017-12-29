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
    public partial class SearchDialog : Form
    {
        public SearchDialog()
        {
            InitializeComponent();
        }
    }

    public interface SearchableBrowser
    {
        bool Search(string text, bool forward, bool matchWholeWord, bool matchCase);
    }
}