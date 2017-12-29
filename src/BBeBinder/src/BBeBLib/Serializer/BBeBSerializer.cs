using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using BBeBLib;
using ICSharpCode.SharpZipLib.Core;


namespace BBeBLib.Serializer
{
	public class BBeBSerializer
	{
		static bool s_bDebugMode = false;

		long m_dwIndexOffsetVal = 0x0;

		private BBeBHeader DeserializeHeader(BinaryReader reader)
		{
			Debug.WriteLineIf(s_bDebugMode, "Reading header");
			
			BBeBHeader header = new BBeBHeader();

			header.signature = reader.ReadChars(4);
			header.wVersion = reader.ReadUInt16();
			header.wPseudoEncByte = reader.ReadUInt16();
			header.dwRootObjectId = reader.ReadUInt32();
			header.NumberOfObjects = reader.ReadUInt64();
			header.ObjectIndexOffset = reader.ReadUInt64();
			header.dwUnknown1 = reader.ReadUInt32();
			header.byBindingDir = reader.ReadByte();
			header.byPadding1 = reader.ReadByte();
			header.wDPI = reader.ReadUInt16();
			header.wPadding2 = reader.ReadUInt16();
			header.wScreenWidth = reader.ReadUInt16();
			header.wScreenHeight = reader.ReadUInt16();
			header.byColorDepth = reader.ReadByte();
			header.byPadding3 = reader.ReadByte();
			header.byUnkonwn2 = reader.ReadBytes(0x14);
			header.dwTocObjectId = reader.ReadUInt32();
			header.dwTocObjectOffset = reader.ReadUInt32();
			header.wDocInfoCompSize = reader.ReadUInt16();
			
			Debug.Assert(reader.BaseStream.Position == 0x4E);

			if (header.wVersion >= 800)
			{
				header.wThumbnailFlags = reader.ReadUInt16();
				header.dwThumbSize = reader.ReadUInt32();
				Debug.Assert(reader.BaseStream.Position == 0x54);
			}

            // Set the TextObject EncodingBytes (temporary hack)
			StreamTagSerializer.EncodingByte = header.wPseudoEncByte;

			return header;
		}

		private void SerializeHeader(BBeB book, BBeBinaryWriter writer)
		{
			BBeBHeader header = book.Header;

			TocObject tocObject = (TocObject)book.FindObject((ushort)header.dwTocObjectId);
			if ( tocObject == null )
			{
				throw new InvalidBookException( "Can't find the TOC object" );
			}

			writer.Write(header.signature);
			writer.Write(header.wVersion);
			writer.Write(header.wPseudoEncByte);
			writer.Write(header.dwRootObjectId);
			writer.Write(header.NumberOfObjects);
			m_dwIndexOffsetVal = writer.Position;
			writer.Write(header.ObjectIndexOffset);
			writer.Write(header.dwUnknown1);
			writer.Write(header.byBindingDir);
			writer.Write(header.byPadding1);
			writer.Write(header.wDPI);
			writer.Write(header.wPadding2);
			writer.Write(header.wScreenWidth);
			writer.Write(header.wScreenHeight);
			writer.Write(header.byColorDepth);
			writer.Write(header.byPadding3);
			writer.Write(header.byUnkonwn2);
			writer.Write(header.dwTocObjectId);
			writer.WriteStreamOffsetReference(tocObject);

            // This may seem a little wasteful as we actually serialize the MetaData object twice
            // Once here and onces when we actualy write it to the stream, however we need to set
            // the compressed size in the header and we can only do it by compressing it first.

            // Really we should store the compressed byte stream and just save that out
            // but its not very big so the overhead is quite small
            {
                MemoryStream mem = new MemoryStream();
                BinaryWriter sw = new BinaryWriter(mem);
                header.wDocInfoCompSize = (ushort)(SerializeMetaData(book.MetaData, sw) + 4);
            }
			writer.Write(header.wDocInfoCompSize);

			Debug.Assert(writer.Position == 0x4E);

			if (header.wVersion >= 800)
			{
				writer.Write(header.wThumbnailFlags);
				writer.Write(header.dwThumbSize);
				Debug.Assert(writer.Position == 0x54);
			}
		}


		/// <summary>
		/// Read the metadata using the supplied reader.
		/// </summary>
		/// <param name="reader">The reader to use to read the metadata with. It is
		/// already positioned at the first byte of metadata.</param>
		/// <param name="nCompressedLen">The length of the compressed metadata. The first
		/// four bytes is the size of the uncompressed data.</param>
        public BookMetaData DeserializeMetaData(BinaryReader reader, int nCompressedLen)
		{
			Debug.WriteLineIf(s_bDebugMode, "Reading metadata");

			byte[] byUncompressedData = ZLib.Decompress(reader, nCompressedLen);

			Debug.WriteLineIf(s_bDebugMode, "Parsing metadata");
			MemoryStream xmlStream = new MemoryStream(byUncompressedData);

            XmlSerializer serializer = new XmlSerializer(typeof(BookMetaData));
            XmlTextReader xreader = new XmlTextReader(xmlStream);
            BookMetaData metaData = null;
            try
            {
                metaData = (BookMetaData)serializer.Deserialize(xreader);
            }
            catch (Exception e)
            {
                Debug.WriteLineIf(s_bDebugMode, e.ToString());
            }
            xreader.Close();

            return metaData;
		}

		
		/// <summary>
		/// Write the metadata using the supplied writer..
		/// </summary>
		/// <param name="metaData">The metadata to write</param>
		/// <param name="writer">The writer to write the serialized meta with.</param>
        private int SerializeMetaData(BookMetaData metaData, BinaryWriter writer)
		{
            XmlSerializer serializer = new XmlSerializer(typeof(BookMetaData));
            MemoryStream uncompressedStream = new MemoryStream();
            StreamWriter swriter = new StreamWriter(uncompressedStream, Encoding.Unicode);
            swriter.NewLine = "\n";
            serializer.Serialize(swriter, metaData);
            writer.Flush();

//			File.WriteAllBytes("C:\\MetaData.xml", uncompressedStream.GetBuffer());

			int size = ZLib.Compress(uncompressedStream.GetBuffer(), (int)uncompressedStream.Position, writer);
            return size;
		}
		

		private void ConnectPageTree(BBeB book)
		{
			Debug.WriteLineIf(s_bDebugMode, "Connecting page tree");
			
			PageTreeObject pageTree = (PageTreeObject)book.FindFirstObject(typeof(PageTreeObject));
			if (pageTree == null)
			{
				throw new InvalidBookException("Couldn't find the required PageTree object");
			}

			UInt32ArrayTag pageListTag = (UInt32ArrayTag)pageTree.FindFirstTag(TagId.PageList);
			if (pageListTag == null)
			{
				throw new InvalidBookException("Couldn't find the required PageList tag");
			}

			foreach (uint id in pageListTag.Value)
			{
				PageObject page = (PageObject)book.FindObject((ushort)id);
				if (page == null)
				{
					throw new InvalidBookException("Can't find page id " + id);
				}
				pageTree.Pages.Add(page);
			}
		}

		private void ConnectPage(PageObject page, BBeB book)
		{
			Debug.WriteLineIf(s_bDebugMode, "Connecting page: " + page.ID);

			UInt32ArrayTag childIDs = (UInt32ArrayTag)page.FindFirstTag(TagId.PageObjectIds);
			if (childIDs != null)
			{
				foreach (uint id in childIDs.Value)
				{
					BBeBObject child = book.FindObject((ushort)id);
					if (child == null)
					{
						throw new InvalidBookException("Can't find object id " + id);
					}

					Debug.Assert(child.ID == id);

					Debug.WriteLineIf(s_bDebugMode, "   Child: " + child.GetType().ToString() + " - id:" + child.ID);
					page.Children.Add(child);
				}
			}

			UInt32Tag objInfoTag = (UInt32Tag)page.FindFirstTag(TagId.ObjectInfoLink);
			if (objInfoTag != null)
			{
				page.InfoObj = book.FindObject((ushort)objInfoTag.Value);
				if (page.InfoObj == null)
				{
					throw new InvalidBookException("Can't find info object id " + objInfoTag.Value);
				}
			}

			StreamTagGroup stream = (StreamTagGroup)page.FindFirstTag(TagId.StreamGroup);
			if (stream != null)
			{
				BBeBObject tempObj = new BBeBObject(0x0);

				page.StreamTags = BBeBTagFactory.ParseAllTags(tempObj, stream.Data);
			}

			UInt32Tag linkTag = (UInt32Tag)page.FindFirstTag(TagId.Link);
			if (linkTag != null)
			{
				BBeBObject linkedObj = book.FindObject((ushort)linkTag.Value);
				if (linkedObj == null)
				{
					throw new InvalidBookException("Can't find object id " + linkTag.Value);
				}
				page.LinkObj = linkedObj;
			}
		}

		private void ConnectPages(BBeB book)
		{
			foreach (BBeBObject obj in book.Objects)
			{
				if (obj.GetType() == typeof(PageObject))
				{
					ConnectPage((PageObject)obj, book);
				}
			}
		}

		private void ConnectBlock(BlockObject block, BBeB book)
		{
			UInt32Tag linkTag = (UInt32Tag)block.FindFirstTag(TagId.Link);
			if (linkTag == null)
			{
				throw new InvalidBookException("Can't find link for block " + block.ID);
			}

			block.LinkObj = book.FindObject((ushort)linkTag.Value);
			if (block.LinkObj == null)
			{
				throw new InvalidBookException("Can't find object id " + linkTag.Value);
			}
		}

		private void ConnectBlocks(BBeB book)
		{
			foreach (BBeBObject obj in book.Objects)
			{
				if (obj.GetType() == typeof(BlockObject))
				{
					ConnectBlock((BlockObject)obj, book);
				}
			}
		}

		private void ConnectText(TextObject text, BBeB book)
		{
			UInt32Tag linkTag = (UInt32Tag)text.FindFirstTag(TagId.Link);
			if (linkTag == null)
			{
				throw new InvalidBookException("Can't find link for block " + text.ID);
			}

			BBeBObject LinkObj = book.FindObject((ushort)linkTag.Value);
			if (LinkObj == null)
			{
				throw new InvalidBookException("Can't find object id " + linkTag.Value);
			}
		}

		private void ConnectTexts(BBeB book)
		{
			foreach (BBeBObject obj in book.Objects)
			{
				if (obj.GetType() == typeof(TextObject))
				{
					ConnectText((TextObject)obj, book);
				}
			}
		}

		/// <summary>
		/// Resolve the object ID's that are used to specify references to 
		/// the actual references.
		/// </summary>
		private void ConnectObjects(BBeB book)
		{
			Debug.WriteLineIf(s_bDebugMode, "Connecting objects");

			ConnectPageTree(book);
			ConnectPages(book);
			ConnectBlocks(book);
			ConnectTexts(book);
		}

		public BBeB Deserialize( Stream bbebStream )
		{
			BinaryReader reader = new BinaryReader(bbebStream, Encoding.Unicode);

			BBeB book = new BBeB();
			BBeBHeader header = DeserializeHeader(reader);
			header.Validate();

			book.Header = header;

			book.MetaData = DeserializeMetaData(reader, header.wDocInfoCompSize);

			if (header.wVersion >= 800)
			{
				book.ThumbnailData = reader.ReadBytes((int)header.dwThumbSize);
			}

			// See http://www.sven.de/librie/Librie/LrfFormat for more information on this table.
			const ulong k_dwObjTableElementSize = 4 * sizeof(uint);

			ulong nTotalSize = 0;

			BBeBObjectFactory objFactory = new BBeBObjectFactory();

			Debug.WriteLineIf(s_bDebugMode, "File contains " + header.NumberOfObjects + " objects");
			for (ulong idxObj = 0; idxObj < header.NumberOfObjects; idxObj++)
			{
				reader.BaseStream.Seek((long)(header.ObjectIndexOffset + idxObj * k_dwObjTableElementSize), 
					SeekOrigin.Begin);

				uint id = reader.ReadUInt32();
				uint offset = reader.ReadUInt32();
				uint size = reader.ReadUInt32();

				nTotalSize += size;

				reader.BaseStream.Seek(offset, SeekOrigin.Begin);
				book.Objects.Add(objFactory.CreateObject(reader, id, size));
			}

			Debug.WriteLineIf(s_bDebugMode, "Total size: " + nTotalSize);

			ConnectObjects(book);

			return book;
		}

		private void FixupHeaderForWriting(BBeB book)
		{
			if (book.MetaData == null)
			{
				// TODO - This is just because we don't have any metadata yet
				Debug.WriteLineIf(s_bDebugMode, "Book has no metadata - Mocking some up for testing");

				MemoryStream metaStream = new MemoryStream();
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(BookMetaData));
				BookMetaData md = new BookMetaData();
				xmlSerializer.Serialize(metaStream, md);
				metaStream.Seek(0, SeekOrigin.Begin);
				book.MetaData = md; //.Load(metaStream);
			}

			if (book.Header.wVersion >= 800)
			{
				if (book.ThumbnailData == null)
				{
					throw new InvalidBookException("Don't have a thumbnail image");
				}

				book.Header.dwThumbSize = (uint)book.ThumbnailData.Length;
			}

			book.Header.NumberOfObjects = (ulong)book.Objects.Count;
		}


		/// <summary>
		/// Write the tag serialized data using the supplied writer.
		/// </summary>
		/// <remarks>
		/// This is a really KLUDGY routine. Even though it deals with primitive
		/// types it has knowledge of what those types mean. For example the UInt16ArrayTag
		/// doesn't have a length value written, but the UInt32ArrayTag does. This is
		/// a very brittle solution to this problem and needs to be fixed. The solution is
		/// to have a serialize method for each tag type (uggh) - so you see why I took
		/// this shortcut.
		/// </remarks>
		/// <param name="tag"></param>
		/// <param name="writer"></param>
		private static void SerializeTag(BBeBTag tag, BBeBinaryWriter writer)
		{
            ushort id = (ushort)(0xf500 + tag.Id);
			if (tag.GetType() == typeof(ByteTag))
			{
				ByteTag t = (ByteTag)tag;
                writer.Write(id);
				writer.Write(t.Value);
			}
			else if (tag.GetType() == typeof(StreamTagGroup))
			{
				StreamTagSerializer.Serialize(writer, (StreamTagGroup)tag);
			}
			else if (tag.GetType() == typeof(ByteArrayTag))
			{
				ByteArrayTag t = (ByteArrayTag)tag;

				if (t.Id == TagId.StreamStart)
				{
					byte[] data = t.Value;

					writer.Write(BBeBLib.TagId.StreamSize);
					writer.Write((uint)data.Length);
					writer.Write(id);
					writer.Write(data);
					writer.Write(BBeBLib.TagId.StreamEnd);
				}
				else
				{
					writer.Write(id);
					writer.Write(t.Value);
				}
			}
			else if (tag.GetType() == typeof(UInt16Tag))
			{
				UInt16Tag t = (UInt16Tag)tag;
				writer.Write(id);
				writer.Write(t.Value);
			}
			else if (tag.GetType() == typeof(UInt16ArrayTag))
			{
				UInt16ArrayTag t = (UInt16ArrayTag)tag;
				writer.Write(id);
				foreach (ushort val in t.Value)
				{
					writer.Write(val);
				}
			}
			else if (tag.GetType() == typeof(UInt32Tag))
			{
				UInt32Tag t = (UInt32Tag)tag;

				// StreamSize is written out by the StreamStart tag as the data may be compressed
				if (t.Id != TagId.StreamSize)
				{
					writer.Write(id);

					// Zero byte tags (but have hardcoded data associated with them
					if (t.Id != TagId.BaseButtonStart &&
						t.Id != TagId.FocusinButtonStart &&
						t.Id != TagId.PushButtonStart &&
						t.Id != TagId.UpButtonStart)
					{
						writer.Write(t.Value);
					}
				}
			}
			else if (tag.GetType() == typeof(UInt32ArrayTag))
			{
				UInt32ArrayTag t = (UInt32ArrayTag)tag;
				writer.Write(id);

				// JumpTo doesn't have a length set and is hardcoded to 2!
				if (t.Id != TagId.JumpTo)
					writer.Write((ushort)t.Value.Length);
				foreach (uint val in t.Value)
				{
					writer.Write(val);
				}
			}
			else if (tag.GetType() == typeof(StringTag))
			{
				StringTag t = (StringTag)tag;
				writer.Write(id);
				writer.Write(t.Value);
			}
			else if (tag.GetType() == typeof(MessageTag))
			{
				MessageTag t = (MessageTag)tag;
				writer.Write(id);
				writer.Write((ushort)t.parameters);
				byte[] data = System.Text.Encoding.Unicode.GetBytes(t.param1);
				writer.Write((ushort)data.Length);
				writer.Write(data);
				data = System.Text.Encoding.Unicode.GetBytes(t.param2);
				writer.Write((ushort)data.Length);
				writer.Write(data);
			}
			else if (tag.GetType() == typeof(EmpDotsCodeTag))
			{
				EmpDotsCodeTag t = (EmpDotsCodeTag)tag;
				writer.Write(id);
				writer.Write((uint)t.Value);

				SerializeTag(t.FontFace, writer);

				writer.Write(t.DotsCode);
			}
			else if (tag.GetType() == typeof(IDOnlyTag))
			{
				IDOnlyTag t = (IDOnlyTag)tag;
				if (t.Id != TagId.StreamEnd)
					writer.Write(id);
			}
			else if (tag.GetType() == typeof(BBeBTag))
			{
				BBeBTag t = (BBeBTag)tag;
				if (t.Id != TagId.StreamEnd)
					writer.Write(id);
			}
			else
			{
				Debug.Assert(false, "Unknown tag type: " + tag.GetType().ToString());
			}
		}

		public static void SerializeTags(List<BBeBTag> tags, BBeBinaryWriter writer)
		{
			foreach (BBeBTag tag in tags)
			{
				SerializeTag(tag, writer);
			}
		}

		private void SerializeObject(BBeBObject obj, BBeBinaryWriter writer)
		{
			writer.WriteObjectStart(obj);

            writer.Write((ushort)obj.ID);
			writer.Write((ushort)0x0);	// All objects have this

            // Write object type
            writer.Write( (ushort)obj.Type );

			SerializeTags(obj.Tags, writer);

			writer.WriteObjectEnd(obj);
		}

		/// <summary>
		/// Serialize the object to the supplied stream.
		/// </summary>
		/// <param name="stream">Write the serialized bytes of the book to this stream.</param>
		/// <param name="book">The book to serialize</param>
		public void Serialize(Stream stream, BBeB book)
		{
			m_dwIndexOffsetVal = 0;

			FixupHeaderForWriting(book);

			BBeBinaryWriter writer = new BBeBinaryWriter(stream);

			book.Header.Validate();
			SerializeHeader(book, writer);
			SerializeMetaData(book.MetaData, writer);

			if (book.Header.wVersion >= 800)
			{
				writer.Write(book.ThumbnailData);
			}

			// Now go back and update the position of the object index now that we
			// know where its offset in the stream.
			long dwObjIdxStartOffset = writer.Position;
			writer.Position = m_dwIndexOffsetVal;
			writer.Write((ulong)dwObjIdxStartOffset);
			writer.Position = dwObjIdxStartOffset;

			// Write the object offset index table
			foreach (BBeBObject obj in book.Objects)
			{
				writer.Write((uint)obj.ID);
				writer.WriteStreamOffsetReference(obj);	// Will be fixed up later
				writer.WriteStreamSizeReference(obj);	// Will be fixed up later
				writer.Write((uint)0x0);				// Unused and reserved
			}

			// Now write out all the objects
			foreach (BBeBObject obj in book.Objects)
			{
				SerializeObject(obj, writer);
			}

			writer.Flush();
			writer.ResolveReferences();
		}
	}
}
