/* 
 * Copyright (C) 2006, Chris Mumford cmumford@cmumford.com
 * 
 * This class is derived from the BBeBook class
 * that is part of the BBeBook application by Scott Turner.
 * 
 * Portions Copyright (C) 2005 and 2006, Scott Turner scotty1024@mac.com
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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
    public class TextBlockBuilder
    {
        LegacyBBeB m_Book;	// Soon to be depricated
		BBeByteBuffer m_PageBuffer = new BBeByteBuffer();
        const int k_MinUncompressedLen = 64;
		long m_nEndBeginPagePos;
		ushort m_wTextBlockId;	// The TextBlockId that this will crete
		Serializer.CharacterMapper m_CharMapper = new BBeBLib.Serializer.CharacterMapper();


		public TextBlockBuilder(ushort wTextBlockId, Serializer.CharacterMapper charMapper)
		{
			m_wTextBlockId = wTextBlockId;
			m_CharMapper = charMapper;

			Debug.Assert(wTextBlockId >= 32);

			AddBeginPage();
		}

        public bool HasText
        {
			get { return m_PageBuffer.Position > m_nEndBeginPagePos; }
        }

		public Serializer.CharacterMapper CharMapper
		{
			get { return m_CharMapper; }
			set { m_CharMapper = value; }
		}

		private void AddBeginPage()
		{
			m_PageBuffer.putTag(TagId.BeginPage);
			m_PageBuffer.putShort(0x0);
			m_PageBuffer.putShort(0x0);
			m_nEndBeginPagePos = m_PageBuffer.Position;
		}

        public TextBlockBuilder(LegacyBBeB book)
        {
            m_Book = book;

			AddBeginPage();
        }

        public void Append(TagId id)
        {
			m_PageBuffer.putTag(id);
        }

        public void Append(TagId id, ushort param)
        {
			m_PageBuffer.putTag(id);
			m_PageBuffer.putShort(param);
        }

		public void Append(TagId id, uint param)
		{
			m_PageBuffer.putTag(id);
			m_PageBuffer.putInt((int)param);
		}

		public void AppendChar(int c)
		{
			m_PageBuffer.putChar(m_CharMapper.GetMap((char)c));
		}

		public void Append(string text)
		{
			foreach (char ch in text)
			{
				AppendChar(ch);
			}
		}

		/// <summary>
		/// The ID of the text block that this instance will create when 
		/// the CreateTextObject method is called.
		/// </summary>
		public ushort TextObjectId
		{
			get { return m_wTextBlockId; }
		}

		public void Append(byte[] data)
		{
			m_PageBuffer.put(data);
		}
		
		public long Position
        {
			get { return m_PageBuffer.Position; }
        }

        public static long MaxTextBufferLength
        {
            /// TODO How long can this be?
            get { return 250000; }
        }

        public TextObject CreateTextObject( ushort textAttributeId )
        {
			m_PageBuffer.putTag(TagId.EndPage);
			
			int len = (int)m_PageBuffer.Position;
            byte[] output = new byte[len];

            Array.Copy(m_PageBuffer.GetBuffer(), output, len);

			TextObject obj = new TextObject(m_wTextBlockId);
            obj.Tags.Add(new UInt32Tag(TagId.Link, textAttributeId));
			obj.Data = output;

            return obj;
        }

        public LegacyBBeBObject CreateLegacyTextObject()
        {
            m_PageBuffer.putTag(TagId.EndPage);

            byte[] output = m_PageBuffer.GetBuffer();
            ObjectFlags flags = 0;
			int len = (int)m_PageBuffer.Position;
			if (len > k_MinUncompressedLen)
            {
                // Allocate a buffer to compress into
				output = new byte[len];

                // Stash uncompressed size
				ByteBuffer.PackInt(output, 0, len);

                // Deflate text
                ICSharpCode.SharpZipLib.Zip.Compression.Deflater compresser =
                    new ICSharpCode.SharpZipLib.Zip.Compression.Deflater();
				compresser.SetInput(m_PageBuffer.GetBuffer(), 0, len);
                compresser.Finish();
				len = compresser.Deflate(output, 4, output.Length - 4) + 4;
                flags = ObjectFlags.COMPRESSED;
            }

			return new LegacyBBeBObject(m_Book, ObjectType.Text, flags, output, len);
        }
    }
}
