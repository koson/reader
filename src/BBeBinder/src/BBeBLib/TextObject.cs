using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using BBeBLib.Serializer;


namespace BBeBLib
{
	public class TextObject : StreamObject
	{
		BBeBTag[] m_Tags = null;

		public TextObject(ushort id)
			: base(id)
		{
            Type = ObjectType.Text;
		}

		public uint LinkObj
		{
			get
			{
				UInt32Tag tag = (UInt32Tag)FindFirstTag(TagId.Link);
				if (tag == null)
				{
					return k_wTagNotFound32;
				}
				else
				{
					return tag.Value;
				}
			}
		}

		public BBeBTag[] TextTags
		{
			get
			{
				if (m_Tags != null)
				{
					return m_Tags;
				}

				long len = 0x0;
				if (Data != null)
				{
					len = Data.Length;
				}

				List<BBeBTag> tags = new List<BBeBTag>();

				StringBuilder sb = null;

				if (len > 0)
				{
					BBeBinaryReader reader = new BBeBinaryReader(Data);
					while ( reader.BaseStream.Position < reader.BaseStream.Length )
					{
						if (reader.IsNextCharTag())
						{
							if (sb != null)
							{
								tags.Add( new TextTag(sb.ToString()) );
								sb = null;
							}
							BBeBTagFactory.ReadTag(reader.ReadTag(), reader, ref tags, Type);
						}
						else
						{
							char ch = reader.ReadChar();
							if ((ch & 0xff00) == 0xf5)
							{
								Debug.Assert(false, "We didn't interpret a previous tag correctly");
							}

							if (sb == null)
							{
								sb = new StringBuilder();
							}
							sb.Append(ch);
						}
					}
				}

				if (sb != null)
				{
					tags.Add(new TextTag(sb.ToString()));
				}

				m_Tags = tags.ToArray();

				return m_Tags;
			}
		}

		public override void WriteDebugInfo(TextWriter writer)
		{
			writer.WriteLine("{0}/{1}, ID={2}", this.GetType().Name, Type.ToString(), ID);

			writer.WriteLine(ToString());
		}

        public override string ToString()
        {
			long len = 0x0;
			if (Data != null)
			{
				len = Data.Length;
			}

            StringBuilder ret = new StringBuilder();

			uint linkObj = LinkObj;
			if (linkObj == k_wTagNotFound32)
				ret.AppendLine("  LinkObj: <none>");
			else
				ret.AppendLine("  LinkObj: " + linkObj);

			ret.AppendLine("  Text Stream:");
			ret.AppendLine("  ============");
			BBeBTag[] tags = TextTags;
			foreach (BBeBTag tag in tags)
			{
				ret.AppendLine("    " + tag.ToString());
			}

            return ret.ToString();
        }
	}
}
