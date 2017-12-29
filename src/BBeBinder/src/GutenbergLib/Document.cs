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
using System.Web;



namespace Gutenberg
{
    [Serializable]
    public class Document
    {
        List<Chapter> m_Chapters = new List<Chapter>();
        string m_strAuthor = "Unknown";
        string m_strTitle = "Unknown";

        public Document()
        {
        }

        public string Title
        {
            get { return m_strTitle; }
            set { m_strTitle = value; }
        }

        public string Author
        {
            get { return m_strAuthor; }
            set { m_strAuthor = value; }
        }

        public List<Chapter> Chapters
        {
            get { return m_Chapters; }
            set { m_Chapters = value; }
        }

		/// <summary>
		/// Serialize this document as an HTML string. 
		/// </summary>
		/// <returns></returns>
		public string SerializeToHtml()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<html>");
			sb.Append("<head>");
			sb.Append("<title>");
			sb.Append(HttpUtility.HtmlEncode(Title));
			sb.Append("</title>");
			sb.Append("</head>");

			sb.Append("<body>");
			foreach (Gutenberg.Chapter chapter in Chapters)
			{
				sb.Append("<h2>");
				if (!string.IsNullOrEmpty(chapter.Prefix))
				{
					sb.Append(HttpUtility.HtmlEncode(chapter.Prefix));
					sb.Append(" ");
				}
				sb.Append(HttpUtility.HtmlEncode(chapter.Number));
				sb.Append("</h2>");
				if (!string.IsNullOrEmpty(chapter.SubHeading))
				{
					sb.Append("<b>");
					sb.Append(HttpUtility.HtmlEncode(chapter.SubHeading));
					sb.Append("</b>");
				}

				foreach (Gutenberg.Paragraph paragraph in chapter.Paragraphs)
				{
					sb.Append("<p>");
					sb.Append(HttpUtility.HtmlEncode(paragraph.Text));
					sb.Append("</p>");
				}
			}
			sb.Append("</body>");
			sb.Append("</html>");

			return sb.ToString();
		}
    }
}
