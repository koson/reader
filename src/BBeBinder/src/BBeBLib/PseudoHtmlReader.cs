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
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
    public class PseudoHtmlReader
    {
        LegacyBBeB m_Book;
        byte[] m_TextBuffer = null;
        int m_TextBufferOffset = 0;


        public PseudoHtmlReader(LegacyBBeB book)
        {
            m_Book = book;
        }

        LegacyBBeBObject getNextPage()
        {
            if (m_TextBufferOffset >= m_TextBuffer.Length)
            {
                // All done
                return null;
            }

            TextBlockBuilder pageBlock = new TextBlockBuilder(m_Book);

            // Convert ASCII to UTF-16LE and along the way we strip \r and null
            // and convert \n to 0xf5d2
            int lineCount = 0;
            int nls = 0;
            while ((m_TextBufferOffset < m_TextBuffer.Length) && (lineCount < 200)
                    && !((nls == 6) && (lineCount > 30)))
            {
                int b = m_TextBuffer[m_TextBufferOffset++] & 0x00ff;
                if ((b == 13) || (b == 0))
                {
                    // Skip \r and null values
                    continue;
                }
                else if (b == 10)
                {
                    pageBlock.Append(TagId.EOL);
                    lineCount++;
                    nls++;
                }
                else if (b == '<')
                {
                    if (((m_TextBufferOffset + 1) < m_TextBuffer.Length)
                            && ((m_TextBuffer[m_TextBufferOffset + 1] & 0x00ff) == '>'))
                    {
                        if (((m_TextBuffer[m_TextBufferOffset] & 0x00ff) == 'i')
                                || ((m_TextBuffer[m_TextBufferOffset] & 0x00ff) == 'I'))
                        {
                            pageBlock.Append(TagId.ItalicBegin);
                            m_TextBufferOffset += 2;
                        }
                        else if (((m_TextBuffer[m_TextBufferOffset] & 0x00ff) == 'b')
                                || ((m_TextBuffer[m_TextBufferOffset] & 0x00ff) == 'B'))
                        {
                            pageBlock.Append(TagId.FontWeight, LegacyBBeB.k_BoldFontWeight);
                            m_TextBufferOffset += 2;
                        }
                    }
                    else if (((m_TextBufferOffset + 2) < m_TextBuffer.Length)
                            && ((m_TextBuffer[m_TextBufferOffset] & 0x00ff) == '/')
                            && ((m_TextBuffer[m_TextBufferOffset + 2] & 0x00ff) == '>'))
                    {
                        if (((m_TextBuffer[m_TextBufferOffset + 1] & 0x00ff) == 'i')
                                || ((m_TextBuffer[m_TextBufferOffset + 1] & 0x00ff) == 'I'))
                        {
                            pageBlock.Append(TagId.ItalicEnd);
                            m_TextBufferOffset += 3;
                        }
                        else if (((m_TextBuffer[m_TextBufferOffset + 1] & 0x00ff) == 'b')
                                || ((m_TextBuffer[m_TextBufferOffset + 1] & 0x00ff) == 'B'))
                        {
                            pageBlock.Append(TagId.FontWeight, LegacyBBeB.k_NormalFontWeight);
                            m_TextBufferOffset += 3;
                        }
                    }
                    else
                    {
                        pageBlock.AppendChar(b);
                    }
                }
                else
                {
                    pageBlock.AppendChar(b);
                    nls = 0;
                }
            }

            return pageBlock.CreateLegacyTextObject();
        }

        public void ReadFile(String aFileName)
        {
            // Load text for book
            m_TextBuffer = File.ReadAllBytes(aFileName);

            while (true)
            {
                LegacyBBeBObject text = getNextPage();
                if (text == null)
                {
                    // All done
                    break;
                }
                m_Book.AddTextPage(text);
            }
        }
    }
}
