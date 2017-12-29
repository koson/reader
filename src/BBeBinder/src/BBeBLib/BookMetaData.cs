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
using System.Xml.Serialization;


namespace BBeBLib
{
    /// <summary>
    /// The Sony BBeB (*.lrf) metadata. This is part of the
    /// BBeB schema so don't just go a change it or else the Sony Reader
    /// may not be able to understand the document.
    /// </summary>
    [Serializable]
    [XmlRoot("Info")]
    public class BookMetaData
    {
        string m_strVersion = "1.0";
        BookInfo m_BookInfo = new BookInfo();
        DocInfo m_DocInfo = new DocInfo();


		/// <summary>
		/// The version of this metadata.
		/// </summary>
        [XmlAttribute("version")]
        public string Version
        {
            get { return m_strVersion; }
            set { m_strVersion = value; }
        }
        
		/// <summary>
		/// The book information.
		/// </summary>
        public BookInfo BookInfo
        {
            get { return m_BookInfo; }
            set { m_BookInfo = value; }
        }

		/// <summary>
		/// The document information.
		/// </summary>
        public DocInfo DocInfo
        {
            get { return m_DocInfo; }
            set { m_DocInfo = value; }
        }

		/// <summary>
		/// The maximum length (in characters) of the BookID property string.
		/// </summary>
		public static int MaxBookIdLength
		{
			get { return 16; }
		}

		/// <summary>
		/// The maximum length (in characters) of the CreationDate property string.
		/// </summary>
		public static int MaxDateLength
		{
			get { return 15; }
		}

		/// <summary>
		/// Validate (and fixup if necessary) this metadata so that it is OK to 
		/// write to the book.
		/// </summary>
		public void Fixup()
		{
			if (string.IsNullOrEmpty(BookInfo.Title))
			{
				BookInfo.Title = "UNKNOWN";
			}
			if (string.IsNullOrEmpty(BookInfo.BookID))
			{
				Random rand = new Random(DateTime.Now.Millisecond);
				BookInfo.BookID = "FB" + rand.Next().ToString();
			}
			if (BookInfo.BookID.Length > MaxBookIdLength)
			{
				BookInfo.BookID = BookInfo.BookID.Substring(0, MaxBookIdLength);
			}
			if (string.IsNullOrEmpty(BookInfo.Label))
			{
				BookInfo.Label = BookInfo.Title;
			}
			if (string.IsNullOrEmpty(DocInfo.CreationDate))
			{
				DocInfo.CreationDate = DateTime.Now.Date.ToShortDateString();
			}
			if (DocInfo.CreationDate.Length > MaxDateLength)
			{
				DocInfo.CreationDate = DocInfo.CreationDate.Substring(0, MaxDateLength);
			}
			if (string.IsNullOrEmpty(DocInfo.Creator))
			{
				DocInfo.Creator = Environment.UserName;
			}
			if (string.IsNullOrEmpty(DocInfo.Producer))
			{
				DocInfo.Producer = Environment.UserName;
			}
		}
    }
}
