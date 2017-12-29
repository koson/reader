using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using BBeBLib;

namespace BBeBLib
{
	public class BBeBObject
	{
		public ushort k_wTagNotFound16 = ushort.MaxValue;
		public uint k_wTagNotFound32 = uint.MaxValue;

        ObjectType objectType;
		ushort m_nId;
		List<BBeBTag> m_Tags;

		public BBeBObject(ushort id)
		{
			m_nId = id;
		}

        public ObjectType Type
        {
            get { return objectType; }
            set { objectType = value; }
        }

		public ushort ID
		{
			get { return m_nId; }
			set { m_nId = value; }
		}

		public List<BBeBTag> Tags
		{
			get 
            { 
                if ( m_Tags == null ) 
                    m_Tags = new List<BBeBTag>();
                return m_Tags; 
            }
		}

		public virtual void WriteDebugInfo(TextWriter writer)
		{
			writer.WriteLine("{0}/{1}, ID={2}", this.GetType().Name, Type.ToString(), m_nId);

			foreach ( BBeBTag tag in Tags )
			{
				tag.WriteDebugInfo(writer);
			}

			writer.WriteLine();
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.AppendFormat( "{0} (Id = {1})\n", this.GetType().Name, m_nId);

            foreach (BBeBTag tag in Tags)
                ret.Append("    " + tag.ToString() + "\n");

            return ret.ToString();
        }

		/// <summary>
		/// Finds the first tag with the specified ID.
		/// </summary>
		/// <param name="eId">The ID of the tag to find</param>
		/// <returns>The tag reference or null if none found.</returns>
		public BBeBTag FindFirstTag(BBeBLib.TagId eId)
		{
			foreach (BBeBTag tag in Tags)
			{
				if (tag.Id == eId)
				{
					return tag;
				}
			}

			return null;
		}
	}
}
