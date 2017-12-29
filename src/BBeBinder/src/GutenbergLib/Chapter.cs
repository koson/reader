/* 
 * Copyright (C) 2006, Chris Mumford cmumford@cmumford.com
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
using System.Text;

namespace Gutenberg
{
    [Serializable]
    public class Chapter
    {
		string m_strChapterPrefix = "Chapter";
        string m_strSubHeading = "";
        string m_strNumber;    // Number (roman, decimal, etc.)
        List<Paragraph> m_Paragraphs = new List<Paragraph>();

        public Chapter()
        {
        }

        public Chapter(string number)
        {
            m_strNumber = number;
        }

        public string Number
        {
            get { return m_strNumber; }
            set { m_strNumber = value; }
        }

        public string SubHeading
        {
            get { return m_strSubHeading; }
            set { m_strSubHeading = value; }
        }

		public string Prefix
		{
			get { return m_strChapterPrefix; }
			set { m_strChapterPrefix = value; }
		}

        public List<Paragraph> Paragraphs
        {
            get { return m_Paragraphs; }
        }
    }
}
