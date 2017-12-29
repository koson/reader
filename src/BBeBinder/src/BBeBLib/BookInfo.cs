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
    /// The BookInfo portion of the BBeB metadata. This is part of the
    /// BBeB schema so don't just go a change it or else the Sony Reader
    /// may not be able to understand the document.
    /// </summary>
    [Serializable]
    public class BookInfo
    {
        string m_strTitle;
        string m_strAuthor;
        string m_strBookID;
        string m_strPublisher;
        string m_strLabel;
        string m_strCategory;
        string m_strClassification = "text";
        string m_strFreeText;
        

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

        public string BookID
        {
            get { return m_strBookID; }
            set { m_strBookID = value; }
        }

        public string Publisher
        {
            get { return m_strPublisher; }
            set { m_strPublisher = value; }
        }

        public string Label
        {
            get { return m_strLabel; }
            set { m_strLabel = value; }
        }

        public string Category
        {
            get { return m_strCategory; }
            set { m_strCategory = value; }
        }

        public string Classification
        {
            get { return m_strClassification; }
            set { m_strClassification = value; }
        }

        public string FreeText
        {
            get { return m_strFreeText; }
            set { m_strFreeText = value; }
        }
    }
}
