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
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
    public class LegacyBBeBObject
    {
        public ObjectType m_eType;      // Written as a ushort
        public ushort m_Id;             // The object ID
        public long m_nFileLocation = 0; // Where Object was placed in file
        public int m_nSizeInFile = 0;   // The size of this object when serialized.
        private BBeByteBuffer m_TagData = new BBeByteBuffer();
        private BBeByteBuffer m_StreamData = null;


        public LegacyBBeBObject(LegacyBBeB book, ObjectType aType)
        {
            m_Id = book.GetNextObjId();
            m_eType = aType;
            book.AddObject(this);
        }

        public LegacyBBeBObject(LegacyBBeB book, ObjectType aType, ObjectFlags aDataFlags, byte[] data)
        {
            m_Id = book.GetNextObjId();
            m_eType = aType;
            book.AddObject(this);

            AddStreamData(aDataFlags, data, data.Length);
        }

        public LegacyBBeBObject(LegacyBBeB book, ObjectType aType, ObjectFlags aDataFlags, byte[] data, long dataLen)
        {
            m_Id = book.GetNextObjId();
            m_eType = aType;
            book.AddObject(this);

            AddStreamData(aDataFlags, data, dataLen);
        }

        public LegacyBBeBObject(LegacyBBeB book, ObjectType aType, ObjectFlags aDataFlags, ByteBuffer data)
        {
            m_Id = book.GetNextObjId();
            m_eType = aType;
            book.AddObject(this);

            AddStreamData(aDataFlags, data.GetBuffer(), data.Length);
        }

        public BBeByteBuffer TagData
        {
            get { return m_TagData; }
        }

        public BBeByteBuffer StreamData
        {
            get { return m_StreamData; }
        }

        public void AddTagShort(TagId aID, int aValue)
        {
            m_TagData.putTagShort(aID, (ushort)aValue);
        }

        public void AddTagInt(TagId aID, int aValue)
        {
            m_TagData.putTagInt(aID, aValue);
        }

        public void AddTag(TagId aID, byte[] aValue)
        {
            m_TagData.putTagBytes(aID, aValue);
        }

        public void AddTag(TagId aID, byte[] aValue, int length)
        {
            m_TagData.putTagBytes(aID, aValue, length);
        }

        private void AddStreamData(ObjectFlags aDataFlags, byte[] aData, long aLength)
        {
            m_StreamData = new BBeByteBuffer();

            m_StreamData.putTagShort(TagId.StreamFlags, (ushort)aDataFlags);

            m_StreamData.putTagInt(TagId.StreamSize, (int)aLength);

            m_StreamData.putTagBytes(TagId.StreamStart, aData, (int)aLength);

            m_StreamData.putTag(TagId.StreamEnd);
        }
    }
}
