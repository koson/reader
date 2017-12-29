using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class ObjectInfoObject : BBeBObject
	{
		public ObjectInfoObject(ushort id)
			: base(id)
		{
            Type = ObjectType.ObjectInfo;
		}

		public ObjectFlags Flags
		{
			get
			{
				UInt16Tag flagTag = (UInt16Tag)FindFirstTag(TagId.StreamFlags);
				if (flagTag == null)
				{
					// This may need to be refactored since I added TagId.StreamGroup
					// I think I should now be finding a StreamTagGroup.
					throw new InvalidTagException("Can't find the flag tag in ObjectInfo object", 0x0);
				}
				return (ObjectFlags)flagTag.Value;
			}
		}

		private ByteArrayTag RefTableTag
		{
			get
			{
				ByteArrayTag refTable = (ByteArrayTag)FindFirstTag(TagId.StreamStart);
				if (refTable == null)
				{
					throw new InvalidTagException("Can't find the reference table in ObjectInfo object", 0x0);
				}

				return refTable;
			}
		}

		public override string ToString()
		{
			StringBuilder ret = new StringBuilder();
			ret.AppendLine(string.Format("{0} (Id = {1})", this.GetType().Name, this.ID));

			ByteArrayTag refTable = RefTableTag;
			MemoryStream tableStream = new MemoryStream(refTable.Value);
			BinaryReader tableReader = new BinaryReader(tableStream);

			ObjectFlags infoType = Flags;

			uint dwNumRectObjIds, i;

			switch (infoType)
			{
				case ObjectFlags.PAGE_LAYOUT:
					ret.AppendLine("   Flags: PAGE_LAYOUT");
					dwNumRectObjIds = tableReader.ReadUInt32();
					ret.AppendLine("   " + dwNumRectObjIds + " Rectangle object(s)");
					ret.AppendLine("   ------------------------------");
					for (i = 0; i < dwNumRectObjIds; i++)
					{
						ret.AppendLine(string.Format("   [{0}] ID: {1}",
							i, tableReader.ReadUInt32()));
						byte[] data = tableReader.ReadBytes(16);
					}
					long bytesLeft = tableReader.BaseStream.Length - tableReader.BaseStream.Position;
					if (bytesLeft != 0)
					{
						ret.AppendLine("  Left: " + bytesLeft + " bytes");
					}
					break;

				case ObjectFlags.PAGE_NUMBERS:
					ret.AppendLine("   Flags: PAGE_NUMBERS");
					dwNumRectObjIds = tableReader.ReadUInt32();
					ret.AppendLine("   " + dwNumRectObjIds + " Page object(s)");
					ret.AppendLine("   ------------------------------");
					for (i = 0; i < dwNumRectObjIds; i++)
					{
						ret.AppendLine(string.Format("   [{0}] ID = {1}, Count = {2}",
							i, tableReader.ReadUInt32(), tableReader.ReadUInt16()));
					}
					break;

				default:
					ret.AppendLine("   Flags: Unknown=" + infoType.ToString());
					while (tableReader.BaseStream.Position < tableReader.BaseStream.Length)
					{
						uint id = tableReader.ReadUInt16();
						ret.Append(id);
						ret.Append(",");
					}
					break;
			}

			return ret.ToString();
		}

        public void addPageNumbersTags(byte[] pageData)
        {
            Tags.Add(new UInt16Tag(TagId.StreamFlags, (ushort)ObjectFlags.PAGE_NUMBERS));

            Tags.Add(new ByteArrayTag(TagId.StreamStart, pageData));
        }

        public void addLayoutTags(uint blockId)
        {
            Tags.Add( new UInt16Tag( TagId.StreamFlags, (ushort)ObjectFlags.PAGE_LAYOUT ) );

            byte[] data = new byte[24];

            data[0] = (byte)(1 & 0x00ff);
            data[1] = (byte)((1 >> 8) & 0x00ff);
            data[2] = (byte)((1 >> 16) & 0x00ff);
            data[3] = (byte)((1 >> 24) & 0x00ff);

            data[4] = (byte)(blockId & 0x00ff);
            data[5] = (byte)((blockId >> 8) & 0x00ff);
            data[6] = (byte)((blockId >> 16) & 0x00ff);
            data[7] = (byte)((blockId >> 24) & 0x00ff);

            Tags.Add( new ByteArrayTag( TagId.StreamStart, data ) );
        }
	}
}
