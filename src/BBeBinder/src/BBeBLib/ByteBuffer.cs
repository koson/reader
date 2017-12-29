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

// Define this when you want to stop the program when it writes at
// a specific byte location.
#undef CATCH_WRITE

using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;


namespace BBeBLib
{
    public class ByteBuffer
    {

#if CATCH_WRITE
        const long k_dwDebugPos = 0x74;
#endif

        MemoryStream m_Stream = new MemoryStream();

        public void setData(byte[] buffer)
        {
            m_Stream = new MemoryStream(buffer);
        }

        public byte[] GetBuffer()
        {
            return m_Stream.GetBuffer();
        }

        public long Length
        {
            get { return m_Stream.Length; }
        }

        public long Position
        {
            get { return m_Stream.Position; }
            set { m_Stream.Position = value; }
        }

        public bool AtEnd()
        {
            return ( Position == Length );
        }

#if CATCH_WRITE
        private bool willWriteAtByte( long writeLen )
        {
            long dwLastByte = Position + writeLen;
            long dwFirstByte = Position;

            if (dwFirstByte <= k_dwDebugPos && dwLastByte >= k_dwDebugPos)
            {
                return true;
            }

            return false;
        }

        private void StopAtPos( long writeLen )
        {
            if (willWriteAtByte(writeLen))
            {
                int a = 5;
            }
        }
#endif

        private byte[] peek(int count)
        {
            long prevPos = m_Stream.Position;

            byte[] buffer = new byte[count];

            m_Stream.Read(buffer, 0, count);
            m_Stream.Seek(prevPos, SeekOrigin.Begin);

            return buffer;
        }


        public void put(byte[] data)
        {
#if CATCH_WRITE
            StopAtPos(data.Length);
#endif
            m_Stream.Write(data, 0, data.Length);
        }

        public byte[] get(long offset, int count)
        {
            long prevPos = m_Stream.Position;
            m_Stream.Seek(offset, SeekOrigin.Begin);
            
            byte[] buffer = new byte[count];

            m_Stream.Read(buffer, 0, count);
            m_Stream.Seek(prevPos, SeekOrigin.Begin);
            
            return buffer;
        }

        public byte[] get(int count)
        {
            byte[] buffer = new byte[count];

            m_Stream.Read(buffer, 0, count);
            return buffer;
        }

        public byte peekByte()
        {
            long prevPos = m_Stream.Position;

            byte b = (byte)m_Stream.ReadByte();

            m_Stream.Seek(prevPos, SeekOrigin.Begin);

            return b;
        }


        public byte getByte(long offset)
        {
            long prevPos = m_Stream.Position;
            m_Stream.Seek(offset, SeekOrigin.Begin);
            
            byte b = (byte)m_Stream.ReadByte();
            
            m_Stream.Seek(prevPos, SeekOrigin.Begin);
            
            return b;
        }

        private void put(long offset, byte[] data)
        {
            long prevPos = m_Stream.Position;
            m_Stream.Seek(offset, SeekOrigin.Begin);
#if CATCH_WRITE
            StopAtPos(data.Length);
#endif
            m_Stream.Write(data, 0, data.Length);
            m_Stream.Seek(prevPos, SeekOrigin.Begin);
        }

        public void put(byte[] data, int offset, int length)
        {
#if CATCH_WRITE
            StopAtPos(length); 
#endif
            m_Stream.Write(data, offset, length);
        }

        public void put(byte val)
        {
#if CATCH_WRITE
            StopAtPos(sizeof(byte)); 
#endif
            m_Stream.WriteByte(val);
        }

        public void put(long offset, byte val)
        {
            long prevPos = m_Stream.Position;
            m_Stream.Seek(offset, SeekOrigin.Begin);
#if CATCH_WRITE
            StopAtPos(sizeof(byte)); 
#endif
            m_Stream.WriteByte(val);
            m_Stream.Seek(prevPos, SeekOrigin.Begin);
        }

        public int peekInt()
        {
            byte[] bytes = peek(4);
            int i = (bytes[0]) + (bytes[1] << 8) + (bytes[2] << 16) + (bytes[3] << 24);

            return i;
        }

        public int getInt()
        {
            byte[] bytes = get(4);
            int i = (bytes[0]) + (bytes[1] << 8) + (bytes[2] << 16) + (bytes[3] << 24);

            return i;
        }

        public int getInt(long offset)
        {
            byte[] bytes = get(offset, 4);
            int i = (bytes[0]) + (bytes[1] << 8) + (bytes[2] << 16) + (bytes[3] << 24);

            return i;
        }

        public void putInt(int val)
        {
            byte[] intBytes = {
                (byte)(val & 0x00ff),
                (byte)((val >> 8) & 0x00ff),
                (byte)((val >> 16) & 0x00ff),
                (byte)((val >> 24) & 0x00ff)
            };

            put(intBytes);
        }

        public void putInt(long offset, int val)
        {
            byte[] intBytes = {
                (byte)(val & 0x00ff),
                (byte)((val >> 8) & 0x00ff),
                (byte)((val >> 16) & 0x00ff),
                (byte)((val >> 24) & 0x00ff)
            };

            put(offset, intBytes);
        }

        public ushort peekShort()
        {
            byte[] bytes = peek(2);
            ushort i = (ushort)((bytes[0]) + (bytes[1] << 8));

            return i;
        }

        public ushort getShort(long offset)
        {
            byte[] bytes = get(offset, 2);
            ushort i = (ushort)((bytes[0]) + (bytes[1] << 8));

            return i;
        }

        public ushort getShort()
        {
            byte[] bytes = get(2);
            ushort i = (ushort)((bytes[0]) + (bytes[1] << 8));

            return i;
        }

        public void putShort(ushort val)
        {
            byte[] shortData = {
                (byte)(val & 0x00ff),
                (byte)((val >> 8) & 0x00ff)
            };
            put(shortData);
        }

        public void putShort(long offset, ushort val)
        {
            byte[] shortData = {
                (byte)(val & 0x00ff),
                (byte)((val >> 8) & 0x00ff)
            };
            put(offset, shortData);
        }

        public char peekChar()
        {
            byte[] bytes = peek(2);
            char i = (char)((bytes[0]) + (bytes[1] << 8));

            return i;
        }

        public char getChar(long offset)
        {
            byte[] bytes = get(offset, 2);
            char i = (char)((bytes[0]) + (bytes[1] << 8));

            return i;
        }

        public char getChar()
        {
            byte[] bytes = get(2);
            char i = (char)((bytes[0]) + (bytes[1] << 8));

            return i;
        }

        public void putChar(char val)
        {
            byte[] charData = {
                (byte)(val & 0x00ff),
                (byte)((val >> 8) & 0x00ff)
            };
            
            put(charData);
        }

        public void putChar(long offset, char val)
        {
            byte[] charData = {
                (byte)(val & 0x00ff),
                (byte)((val >> 8) & 0x00ff)
            };
            put(offset, charData);
        }

        public static void PackInt(byte[] buf, int offset, int aValue)
        {
            buf[offset + 0] = (byte)(aValue & 0x00ff);
            buf[offset + 1] = (byte)((aValue >> 8) & 0x00ff);
            buf[offset + 2] = (byte)((aValue >> 16) & 0x00ff);
            buf[offset + 3] = (byte)((aValue >> 24) & 0x00ff);
        }
    }
}
