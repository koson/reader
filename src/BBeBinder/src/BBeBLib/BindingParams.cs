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
	/// The binding parameters used to bind (which really means create)
	/// a BBeB.
	/// </summary>
    [Serializable]
    public class BindingParams
    {
        string m_strIcon;
        string m_strInputFile;
        string m_strOutputFile;
        BookMetaData m_MetaData = new BookMetaData();

		/// <summary>
		/// The icon thumbnail image to use.
		/// </summary>
        public string IconFile
        {
            get { return m_strIcon; }
            set { m_strIcon = value; }
        }

		/// <summary>
		/// The input file (HTML, TEXT)
		/// </summary>
        public string InputFile
        {
            get { return m_strInputFile; }
            set { m_strInputFile = value; }
        }

		/// <summary>
		/// The output LRF file. 
		/// </summary>
        public string OutputFile
        {
            get { return m_strOutputFile; }
            set { m_strOutputFile = value; }
        }

		/// <summary>
		/// The book's metadata. This data gets written to the BBeB
		/// file (*.lrf) when it is created.
		/// </summary>
		[XmlElement("Info")]
        public BookMetaData MetaData
        {
            get { return m_MetaData; }
            set { m_MetaData = value; }
        }
    }
}
