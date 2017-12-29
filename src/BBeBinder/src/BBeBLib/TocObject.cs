using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using BBeBLib.Serializer;


namespace BBeBLib
{
	public class TocObject : BBeBObject
	{
		public class TocLabelEntry
		{
			uint refpage;
			uint refobj;
			string labelText;

			public TocLabelEntry(uint page, uint obj, string text)
			{
				refpage = page;
				refobj = obj;
				labelText = text;
			}

			public uint RefPage
			{
				get { return refpage; }
			}

			public uint RefObj
			{
				get { return refobj; }
			}

			public string Text
			{
				get { return labelText; }
			}
		}

		List<TocLabelEntry> m_Entries = new List<TocLabelEntry>();
		bool m_bEntriesParsed = false;

		public TocObject(ushort id)
			: base(id)
		{
            Type = ObjectType.TOC;
		}


		StreamTagGroup StreamData
		{
			get
			{
				return (StreamTagGroup)FindFirstTag(TagId.StreamGroup);
			}
		}

		public void AddEntry(uint refPage, uint refObj, string labelText)
		{
			m_Entries.Add(new TocLabelEntry(refPage, refObj, labelText));

			// We only really parse when we're reading an LRF file. If we're
			// creating this object and it's not coming from a LRF file then
			// either way we want to set this flag.
			m_bEntriesParsed = true;
		}

		private void Deserialize()
		{
			Debug.Assert(!m_bEntriesParsed);

			StreamTagGroup data = (StreamTagGroup)FindFirstTag(TagId.StreamGroup);
			if (data == null)
			{
				return;
			}

			BBeBinaryReader reader = new BBeBinaryReader(new MemoryStream(data.Data));

			uint dwLabelCount = reader.ReadUInt32();
			uint i;

			// The offsets are the byte offset to the start of the TOC label entry
			// relative to the first TOC label entry (AKA the end of this table)
			uint[] offsets = new uint[dwLabelCount];
			for (i = 0; i < dwLabelCount; i++)
			{
				offsets[i] = reader.ReadUInt32();
			}

			long dwEndOfTablePos = reader.BaseStream.Position;

			for (i = 0; i < dwLabelCount; i++)
			{
				if (offsets[i] != reader.BaseStream.Position - dwEndOfTablePos)
				{
					throw new InvalidDataException("TOC entry didn't start at specified offset");
				}

				uint refpage = reader.ReadUInt32();
				uint refobj = reader.ReadUInt32();

				string labelText = reader.ReadString();

				AddEntry(refpage, refobj, labelText);
			}
		}

		/// <summary>
		/// Serialize the list of TOC entries into a stream, and add that stream
		/// as a tag (really 3 tags) to this objects tags list.
		/// </summary>
		public void Serialize()
		{
			Tags.Add(new UInt16Tag(TagId.StreamFlags, (ushort)StreamContents.TableOfContents));

			Debug.Assert(FindFirstTag(TagId.StreamStart) == null);

			MemoryStream stream = new MemoryStream();
			BBeBinaryWriter writer = new BBeBinaryWriter(stream);

			writer.Write((uint)m_Entries.Count);

			uint[] offsets = new uint[m_Entries.Count];

			// Now write out the entry offsets which we don't know yet
			// We'll come back later and overwrite
			foreach (uint offset in offsets)
			{
				writer.Write(offset);
			}

			writer.Flush();
			long dwEndOfTablePos = writer.BaseStream.Position;

			uint idx = 0;
			foreach (TocLabelEntry entry in m_Entries)
			{
				writer.Flush();
				offsets[idx] = (uint)(writer.Position - dwEndOfTablePos);

				writer.Write(entry.RefPage);
				writer.Write(entry.RefObj);
				writer.Write(entry.Text);

				idx++;
			}

			writer.Flush();
			long endPos = writer.Position;

			// Now go back and overwrite the offset table
			writer.Position = sizeof(uint);

			foreach (uint offset in offsets)
			{
				writer.Write(offset);
			}

			writer.Close();

			byte[] data = new byte[endPos];
			Array.Copy(stream.GetBuffer(), data, endPos);

			Tags.Add(new UInt32Tag(TagId.StreamSize, (uint)endPos));
			Tags.Add(new ByteArrayTag(TagId.StreamStart, data));
			Tags.Add(new IDOnlyTag(TagId.StreamEnd));
		}

		public List<TocLabelEntry> Entries
		{
			get
			{
				if (!m_bEntriesParsed)
				{
					Deserialize();
					m_bEntriesParsed = true;
				}
				return m_Entries;
			}
		}

		public override void WriteDebugInfo(TextWriter writer)
		{
			if (!m_bEntriesParsed)
			{
				Deserialize();
			}

			writer.WriteLine("{0} (Id = {1})\n", this.GetType().Name, ID);
			int i = 0;
			foreach (TocLabelEntry entry in m_Entries)
			{
				writer.WriteLine("  [{0}] refpage={1}, refobj={2}, label={3}",
					i++, entry.RefPage, entry.RefObj, entry.Text);
			}
			writer.WriteLine();

		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("{0} (Id = {1})\n", this.GetType().Name, ID);

			List<TocLabelEntry> entries = Entries;
			sb.AppendFormat("    Entries ({0}):", entries.Count);
			sb.AppendLine();

			uint idx = 0;
			foreach (TocLabelEntry entry in entries)
			{
				sb.AppendFormat("      Label[{0}]: refpage={1}, refobj={2}, text=\"{3}\"",
					idx++, entry.RefPage, entry.RefObj, entry.Text);
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
