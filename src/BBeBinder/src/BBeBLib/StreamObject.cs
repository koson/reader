using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BBeBLib
{
    abstract public class StreamObject : BBeBObject
    {
		StreamTagGroup m_StreamGroup = null;

        public StreamObject(ushort id)
			: base(id)
		{
		}


		public StreamTagGroup StreamTagGroup
		{
			get
			{
				if (m_StreamGroup == null)
				{
					m_StreamGroup = (StreamTagGroup)FindFirstTag(TagId.StreamGroup);
				}

				return m_StreamGroup;
			}
		}

		public StreamContents Contents
		{
			get
			{
				if (StreamTagGroup == null)
				{
					return StreamContents.Unknown;
				}
				else
				{
					return StreamTagGroup.Contents;
				}
			}
			set
			{
				StreamTagGroup.Contents = value;
			}
		}

		public byte[] Data
		{
			get
			{
				if (m_StreamGroup == null)
				{
					m_StreamGroup = (StreamTagGroup)FindFirstTag(TagId.StreamGroup);
				}
				if (m_StreamGroup == null)
				{
					return null;
				}
				else
				{
					return m_StreamGroup.Data;
				}
			}
			set
			{
				if (m_StreamGroup == null)
				{
					m_StreamGroup = (StreamTagGroup)FindFirstTag(TagId.StreamGroup);
				}

				if (m_StreamGroup == null)
				{
					m_StreamGroup = new StreamTagGroup();
					m_StreamGroup.Data = value;
					Tags.Add(m_StreamGroup);
				}
				else
				{
					m_StreamGroup.Data = value;
				}
			}
		}
    }
}
