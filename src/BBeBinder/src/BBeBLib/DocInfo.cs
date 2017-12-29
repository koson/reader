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

namespace BBeBLib
{
    /// <summary>
    /// The DocInfo portion of the BBeB metadata. This is part of the
    /// BBeB schema so don't just go a change it or else the Sony Reader
    /// may not be able to understand the document.
    /// </summary>
    [Serializable]
    public class DocInfo
    {
        string m_strLanguage = "en";
        string m_strCreator;
        string m_strCreationDate;
        string m_strProducer;
        string m_strNumPages;

        public string Language
        {
            get { return m_strLanguage; }
            set { m_strLanguage = value; }
        }

        public string Creator
        {
            get { return m_strCreator; }
            set { m_strCreator = value; }
        }

        public string CreationDate
        {
            get { return m_strCreationDate; }
            set { m_strCreationDate = value; }
        }

        public string Producer
        {
            get { return m_strProducer; }
            set { m_strProducer = value; }
        }

		/// <summary>
		/// This used to be an int, but some books have no value for this
		/// which causes XmlSerializer to throw an exception (apparently it
		/// needs a value).
		/// </summary>
        public string Page
        {
            get { return m_strNumPages; }
            set { m_strNumPages = value; }
        }
    }
}
