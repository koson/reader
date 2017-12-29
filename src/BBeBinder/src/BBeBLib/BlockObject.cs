using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using BBeBLib.Serializer;


namespace BBeBLib
{
	public class BlockObject : BBeBObject
	{
		BBeBObject m_LinkObj = null;

		public BlockObject(ushort id)
			: base(id)
		{
            Type = ObjectType.Block;
		}

		public BBeBObject LinkObj
		{
			get { return m_LinkObj; }
			set { m_LinkObj = value; }
		}

		public uint BlockLink
		{
			set
			{
				UInt32Tag tag = (UInt32Tag)FindFirstTag(TagId.Link);
				if (tag == null)
				{
					tag = new UInt32Tag(TagId.Link, 0);
					this.Tags.Add(tag);
				}
				tag.Value = value;
			}
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

		public ushort BlockHeight
		{
			get
			{
				UInt16Tag tag = (UInt16Tag)FindFirstTag(TagId.BlockHeight);
				if (tag != null)
				{
					return tag.Value;
				}
				else
				{
					return k_wTagNotFound16;
				}
			}
			set
			{
				UInt16Tag tag = (UInt16Tag)FindFirstTag(TagId.BlockHeight);
				if (tag == null)
				{
					tag = new UInt16Tag(TagId.BlockHeight, 0);
					this.Tags.Add(tag);
				}
				tag.Value = value;
			}
		}

		public ushort BlockRule
		{
			set
			{
				UInt16Tag tag = (UInt16Tag)FindFirstTag(TagId.BlockRule);
				if (tag == null)
				{
					tag = new UInt16Tag(TagId.BlockRule, 0);
					this.Tags.Add(tag);
				}
				tag.Value = value;
			}
			get
			{
				UInt16Tag tag = (UInt16Tag)FindFirstTag(TagId.BlockRule);
				if (tag == null)
				{
					return k_wTagNotFound16;
				}
				else
				{
					return tag.Value;
				}
			}
		}

		public ushort BlockWidth
		{
			set
			{
				UInt16Tag tag = (UInt16Tag)FindFirstTag(TagId.BlockWidth);
				if (tag == null)
				{
					tag = new UInt16Tag(TagId.BlockWidth, 0);
					this.Tags.Add(tag);
				}
				tag.Value = value;
			}
			get
			{
				UInt16Tag tag = (UInt16Tag)FindFirstTag(TagId.BlockWidth);
				if (tag == null)
				{
					return k_wTagNotFound16;
				}
				else
				{
					return tag.Value;
				}
			}
		}

		public uint ObjectLink
		{
			set
			{
				// 2 bytes for the tag and 4 for the id (which is stored as an int
				uint objId = value;

				byte[] data = {
					(byte)((ushort)TagId.Link & 0x00ff),
					(byte)0xF5,
					(byte)(objId & 0x00ff),
					(byte)((objId >> 8) & 0x00ff),
					(byte)((objId >> 16) & 0x00ff),
					(byte)((objId >> 24) & 0x00ff) 
				};

				Tags.Add(new UInt16Tag(TagId.StreamFlags, 0));
				Tags.Add(new ByteArrayTag(TagId.StreamStart, data));
			}
			get
			{
				StreamTagGroup data = (StreamTagGroup)FindFirstTag(TagId.StreamGroup);
				if (data == null)
				{
					return k_wTagNotFound32;
				}
				else
				{
					BBeBinaryReader reader = new BBeBinaryReader(data.Data);

					TagId tid = reader.ReadTag();

					if (tid != TagId.Link)
					{
						throw new UnexpectedTagException("Wasn't a link tag");
					}

					return reader.ReadUInt32();
				}
			}
		}

		public override void WriteDebugInfo(TextWriter writer)
		{
			writer.WriteLine("{0}/{1}, ID={2}", this.GetType().Name, Type.ToString(), ID);

			writer.WriteLine(ToString());
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			if (LinkObj != null)
			{
				sb.AppendFormat("   Link->{0} ({1})", LinkObj.ID, LinkObj.Type.ToString());
				sb.AppendLine();
			}
			else
			{
				sb.AppendLine("   Link: None");
			}

			ushort height = BlockHeight;
			if (height == k_wTagNotFound16)
				sb.AppendLine("   Height: <none>");
			else
				sb.AppendLine("   Height: " + height);

			ushort width = BlockWidth;
			if (width == k_wTagNotFound16)
				sb.AppendLine("   Width: <none>");
			else
				sb.AppendLine("   Width: " + width);

			ushort rule = BlockRule;
			if (rule == k_wTagNotFound16)
				sb.AppendLine("   Rule: <none>");
			else
				sb.AppendLine("   Rule: " + rule);

			uint link = ObjectLink;
			if (link == k_wTagNotFound32)
				sb.AppendLine("   ObjLink: <none>");
			else
				sb.AppendLine("   ObjLink: " + link);

			return sb.ToString();
		}
	}
}
