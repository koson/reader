/* 
 * Copyright (C) 2006, Chris Mumford cmumford@cmumford.com
 * 
 * The BBeB and BBeBObject classes are derived from the BBeBook class
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
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Core;

namespace BBeBLib
{
	/// <summary>
	/// This class represents a Broadband eBook (BBeB) - both the data and the
	/// serializer.
	/// </summary>
	public class LegacyBBeB
	{
		public const UInt16 k_NormalFontWeight = 400;
		public const UInt16 k_BoldFontWeight = 800;

		const UInt16 m_wParagraphIndent = 0;
		const UInt16 m_wParagraphSkip = 0;

		/* Primary LRF header offsets */
		public const int LRF_SIGNATURE1 = 0x00;	// UTF-16LE "LRF\000"
		public const int LRF_VERSION = 0x08;	    // file version (ushort)
		public const int LRF_OBJECT_COUNT = 0x10;	// (long) count of objects
		public const int LRF_PSUEDOKEY = 0x0A;	    // XOR key (ushort)
		public const int LRF_ROOT_ID = 0x0C;	    // (int)
		public const int LRF_OBJECTTREE_OFFSET = 0x18;// (long) offset to object table

		/* Secondary LRF header offsets */
		public const int LRF_UNK0 = 0x20;		    // (int)
		public const int LRF_DIRECTION = 0x24;	    // (byte)
		public const int LRF_DIRECTION_FORWARDS = 0x01;
		public const int LRF_DIRECTION_BACKWARDS = 0x10;
		public const int LRF_UNK1 = 0x26;		    // (ushort)
		public const int LRF_UNK2 = 0x2A;		    // (ushort)
		public const int LRF_UNK3 = 0x2C;		    // (ushort)
		public const int LRF_UNK4 = 0x2E;		    // (byte)
		public const int LRF_SIGNATURE2 = 0x30;	// 5 int's (some kind of signature)
		public const int LRF_TOC_ID = 0x44;		// (int)
		public const int LRF_TOC_OFFSET = 0x48;	// (int)
		public const int LRF_INFOLEN = 0x4C;	    // Compressed metaData size (ushort)

		// Next two only valid if version >= 800
		public const int LRF_UNK7 = 0x4E;		    // (ushort)
		public const int LRF_THUMBNAIL_LENGTH = 0x50;  // GIF thumbnail size (int)

		public const int LRF_UNK6 = 0x4E;

		/// <summary>
		/// The file data that will be written to the disk.
		/// </summary>
		BBeByteBuffer m_FileData = new BBeByteBuffer();

		/// <summary>
		/// The object ID used by the next BBeBObject instance.
		/// </summary>
		private ushort m_wNextObjectId;

		/// <summary>
		/// This book's metadata
		/// </summary>
		BookMetaData m_MetaData;

		/// <summary>
		/// List of BBeBObject instances added to this BBeBook
		/// </summary>
		private List<LegacyBBeBObject> m_BookObjects = new List<LegacyBBeBObject>();

		int m_nNumPages = 0;
		LegacyBBeBObject m_BookFile = null;
		int m_nMarginsId = 0;
		int m_nPageBoxId = 0;
		int m_nFontRecordId = 0;
		private byte[] m_ThumbnailData = null;

		/// <summary>
		/// The text objects of the current page.
		/// </summary>
		private List<LegacyBBeBObject> m_PageTextObjects = new List<LegacyBBeBObject>();

		/// <summary>
		/// The bounding box objects of the current page.
		/// </summary>
		private List<LegacyBBeBObject> m_PageBoxObjects = new List<LegacyBBeBObject>();


		/// <summary>
		/// Constructor.
		/// </summary>
		public LegacyBBeB(BookMetaData metaData)
		{
			m_wNextObjectId = 0x32; // Magic.. Ask Sony why all LRF files start here

			m_MetaData = metaData;
		}

		public static UInt16 DefaultFontSize
		{
			get { return 100; }
		}

		public ushort GetNextObjId()
		{
			return m_wNextObjectId++;
		}

		public void BeginBook()
		{
			m_nNumPages = 0;

			LegacyBBeBObject head = new LegacyBBeBObject(this, ObjectType.BookAtr); // Root Object
			head.AddTagShort(TagId.RubyAlign, 0x0002);
			head.AddTagShort(TagId.RubyOverhang, 0x0000);
			head.AddTagShort(TagId.EmpDotsPosition, 0x0001);

			byte[] tmp78 = { 0, 0, 0, 0, 0x16, (byte)0xf5, 0, 0, 0x1, 0x30 };
			head.AddTag(TagId.EmpDotsCode, tmp78);

			/*
			 * head.addTagInt(0x78, 0); head.addTagInt(0x16, 0x30010000);
			 */
			head.AddTagShort(TagId.EmpLinePosition, 0x0002);
			head.AddTagShort(TagId.EmpLineMode, 0x0010);
			head.AddTagShort(TagId.SetWaitProp, 0x0002);

			byte[] empty = { 0x00, 0x00, 0x00, 0x00 };
			LegacyBBeBObject temp = new LegacyBBeBObject(this, ObjectType.TOC, ObjectFlags.TOC_51, empty); // special
			// ?

			// UTF-16LE string containing font name
			byte[] fontname = { 22, 0, // 11 chars
				    (byte)'I', 0, (byte)'W', 0, (byte)'A', 0, 0x0E, 0x66, (byte)'-', 0, 0x2D, 0x4E, 0x30,
				    0x7D, (byte)'N', 0, (byte)'-', 0, (byte)'e', 0, (byte)'b', 0 };

			// Create Global font record for the file
			LegacyBBeBObject fontRecord = new LegacyBBeBObject(this, ObjectType.TextAtr);
			fontRecord.AddTagShort(TagId.RubyOverhang, 0x0000);
			fontRecord.AddTagShort(TagId.EmpDotsPosition, 0x0001);
			fontRecord.AddTagShort(TagId.EmpLinePosition, 0x0001);
			fontRecord.AddTagShort(TagId.EmpLineMode, 0x0000);
			fontRecord.AddTagShort(TagId.FontSize, LegacyBBeB.DefaultFontSize);
			fontRecord.AddTagShort(TagId.FontWidth, 0xfff6);
			fontRecord.AddTagShort(TagId.FontEscapement, 0x0000);
			fontRecord.AddTagShort(TagId.FontOrientation, 0x0000);
			fontRecord.AddTagShort(TagId.FontWeight, k_NormalFontWeight);
			fontRecord.AddTag(TagId.FontFacename, fontname);
			fontRecord.AddTagInt(TagId.TextColor, 0);
			fontRecord.AddTagInt(TagId.TextBgColor, 0x00ff);
			fontRecord.AddTagShort(TagId.WordSpace, 0x0019);
			fontRecord.AddTagShort(TagId.LetterSpace, 0x0000);
			fontRecord.AddTagShort(TagId.BaseLineSkip, 0x008c);
			fontRecord.AddTagShort(TagId.LineSpace, 0x000a);
			fontRecord.AddTagShort(TagId.ParIndent, m_wParagraphIndent);
			fontRecord.AddTagShort(TagId.ParSkip, m_wParagraphSkip);
			fontRecord.AddTagShort(TagId.LineWidth, 0x0002);
			fontRecord.AddTagInt(TagId.LineColor, 0);
			fontRecord.AddTagShort(TagId.BlockAlignment, (ushort)BlockAlignment.Left);
			fontRecord.AddTagShort(TagId.FontUnknownTwo, 0x0001);
			fontRecord.AddTagShort(TagId.FontUnknownThree, 0x0000);
			fontRecord.AddTagShort(TagId.RubyAlign, 0x0001);

			m_nFontRecordId = fontRecord.m_Id;

			// Fill this one in later need id now for head
			m_BookFile = new LegacyBBeBObject(this, ObjectType.PageTree);

			head.AddTagInt(TagId.ChildPageTree, m_BookFile.m_Id);

			// Margins
			LegacyBBeBObject margins = new LegacyBBeBObject(this, ObjectType.PageAtr);
            margins.AddTagShort(TagId.TopMargin, 0x0005); // 5
            margins.AddTagShort(TagId.HeadHeight, 0x0035); // 53
            margins.AddTagShort(TagId.HeadSep, 0x0005); // 5
            margins.AddTagShort(TagId.OddSideMargin, 0x002a); // 42
            margins.AddTagShort(TagId.EvenSideMargin, 0x002a); // 42
            margins.AddTagShort(TagId.TextHeight, 0x02a2); // 674
            margins.AddTagShort(TagId.TextWidth, 0x0204); // 516
            margins.AddTagShort(TagId.FootSpace, 0x003a); // 58
            margins.AddTagShort(TagId.FootHeight, 0x0035); // 53
            margins.AddTagShort(TagId.Layout, 0x0034); // 52
            margins.AddTagShort(TagId.PagePosition, 0x0000);
            margins.AddTagShort(TagId.SetEmptyView, 0x0001);
            margins.AddTagShort(TagId.SetWaitProp, 0x0002);

			byte[] sixBytes = { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
            margins.AddTag(TagId.BGImageName, sixBytes);

			m_nMarginsId = margins.m_Id;

			LegacyBBeBObject pageBox = new LegacyBBeBObject(this, ObjectType.BlockAtr);
			pageBox.AddTagShort(TagId.BlockWidth, 600);
			pageBox.AddTagShort(TagId.BlockHeight, 800);
			pageBox.AddTagShort(TagId.BlockRule, 0x0012);
			pageBox.AddTagInt(TagId.BlockAttrUnknown1, 0x00ff);
            pageBox.AddTagShort(TagId.Layout, 0x0034);
			pageBox.AddTagShort(TagId.BlockAttrUnknown3, 0x0000);
			pageBox.AddTagInt(TagId.BlockAttrUnknown4, 0);
			pageBox.AddTagShort(TagId.BlockAttrUnknown0, 0x0001);
			pageBox.AddTagShort(TagId.BlockAttrUnknown5, 0x0000);
			pageBox.AddTagShort(TagId.BlockAttrUnknown6, 0x0000);
            pageBox.AddTag(TagId.BGImageName, sixBytes);

			m_nPageBoxId = pageBox.m_Id;
		}


		public void FinalizeBook(String strThumbnailFile)
		{
			m_ThumbnailData = File.ReadAllBytes(strThumbnailFile);

			// Update "Page" tag in metaData with value of pages
			/*
			 * sprintf(pagetext,"%d",pages); newmeta = update_tag(metadata, "Page",
			 * pagetext); free(metadata); metadata = newmeta;
			 */

			byte[] pageNumberData = new byte[(6 * m_nNumPages) + 4];
			byte[] bookPagesList = new byte[(4 * m_nNumPages) + 2];
			pageNumberData[0] = bookPagesList[0] = (byte)(m_nNumPages & 0x00ff);
			pageNumberData[1] = bookPagesList[1] = (byte)((m_nNumPages >> 8) & 0x00ff);

			int pnIndex = 4;
			int bpIndex = 2;
			for (int i = 0; i < m_BookObjects.Count; i++)
			{
				LegacyBBeBObject subFile = m_BookObjects[i];
				if (subFile.m_eType == ObjectType.Page)
				{
					pageNumberData[pnIndex] = bookPagesList[bpIndex] = (byte)(subFile.m_Id & 0x00ff);
					pageNumberData[pnIndex + 1] = bookPagesList[bpIndex + 1] = (byte)((subFile.m_Id >> 8) & 0x00ff);

					// Set to say 1 page per page (a kludge for now)
					pageNumberData[pnIndex + 4] = 1;
					pageNumberData[pnIndex + 5] = 0;

					pnIndex += 6;
					bpIndex += 4;
				}
			}

			LegacyBBeBObject pageNumbers = new LegacyBBeBObject(this, ObjectType.ObjectInfo,
					ObjectFlags.PAGE_NUMBERS, pageNumberData);

			m_BookFile.AddTagInt(TagId.ObjectInfoLink, pageNumbers.m_Id);
			m_BookFile.AddTag(TagId.PageList, bookPagesList);

			AddHeader(m_ThumbnailData);
		}


		void AddHeader(byte[] thumbnail)
		{
			// Put magic marker at front
			m_FileData.putChar(0, 'L');
			m_FileData.putChar(2, 'R');
			m_FileData.putChar(4, 'F');

			// Put LRF version in file
			m_FileData.putShort(LRF_VERSION, 999); // 0x3E7

			// Put in an encryption key (place holder for now)
			m_FileData.putShort(LRF_PSUEDOKEY, 0x30);

			// Set book direction to forwards
			m_FileData.put(LRF_DIRECTION, LRF_DIRECTION_FORWARDS);

			// Put Dimensions
			m_FileData.putShort(LRF_UNK1, 800 * 2);
			m_FileData.putShort(LRF_UNK2, 600);
			m_FileData.putShort(LRF_UNK3, 800);

			// Various unknown (but critical) values
			m_FileData.put(LRF_UNK4, 0x18);

			m_FileData.putShort(LRF_TOC_OFFSET, 0x1536); // XXX is this legit?

			m_FileData.put(LRF_UNK6, 0x14);

			// Set position to just past header
			m_FileData.Position = 0x54;

			// XML file containing metadata
			byte[] metaData = SerializeMetaData();
			int compressedLength = putCompressedBytes(metaData);
			m_FileData.putShort(LRF_INFOLEN, (ushort)(compressedLength + 4));

			// Thumbnail (60x80)
			m_FileData.putInt(LRF_THUMBNAIL_LENGTH, thumbnail.Length);
			m_FileData.put(thumbnail);

			// 16-byte align before placing first BBeBObject into file.
			m_FileData.AlignPosition(16);

			// Put BBeBObjects into file
			for (int i = 0; i < m_BookObjects.Count; i++)
			{
				putObjectData(m_BookObjects[i]);
			}

			// 16-byte align before placing BBeBObject tree into file
			m_FileData.AlignPosition(16);

			// Put BBEBObject info into LRF header
			// And yes, these are technically 64 bits each...
			m_FileData.putInt(LRF_OBJECT_COUNT, m_BookObjects.Count);
			m_FileData.putInt(LRF_OBJECTTREE_OFFSET, (int)m_FileData.Position);

			// Put BBeBObject tree into file
			for (int i = 0; i < m_BookObjects.Count; i++)
			{
				LegacyBBeBObject obj = m_BookObjects[i];
				m_FileData.putInt(obj.m_Id);
				m_FileData.putInt((int)obj.m_nFileLocation);
				m_FileData.putInt(obj.m_nSizeInFile);
				m_FileData.putInt(0);
			}
		}

		public void AddObject(LegacyBBeBObject obj)
		{
			m_BookObjects.Add(obj);
		}

		public void AddTextPage(LegacyBBeBObject text)
		{
			// font size?? para.addTag( 0x11, 80);
			text.AddTagInt(TagId.Link, m_nFontRecordId);

			/* The text goes into a bounding box */
			BBeByteBuffer boxData = new BBeByteBuffer();
			boxData.putTagInt(TagId.Link, text.m_Id);

			LegacyBBeBObject box = new LegacyBBeBObject(this, ObjectType.Block, ObjectFlags.NONE, boxData);
			box.AddTagInt(TagId.Link, m_nPageBoxId);

			// add_tag_to_subfile(box,new_tag(0x31,sizeof(WORD),0,600));
			// add_tag_to_subfile(box,new_tag(0x32,sizeof(WORD),0,800));
			// add_tag_to_subfile(box,new_tag(0x33,sizeof(WORD),0,0x22));

			BBeByteBuffer pageFiles = new BBeByteBuffer();
			pageFiles.putShort(3);
			pageFiles.putInt(m_nFontRecordId);
			pageFiles.putInt(text.m_Id);
			pageFiles.putInt(box.m_Id);

			BBeByteBuffer pageData = new BBeByteBuffer();
			pageData.putTagInt(TagId.Link, box.m_Id);

			LegacyBBeBObject page = new LegacyBBeBObject(this, ObjectType.Page, ObjectFlags.NONE, pageData);
			page.AddTagInt(TagId.ParentPageTree, m_BookFile.m_Id);
			page.AddTag(TagId.PageObjectIds, pageFiles.GetBuffer(), (int)pageFiles.Length);
			page.AddTagInt(TagId.Link, m_nMarginsId);

			// We make the layout big so paragraphs can break over display pages
			// We don't really understand the format of the PAGE_LAYOUT object info
			// object yet. 
			byte[] layoutData = new byte[24];
			ByteBuffer.PackInt(layoutData, 0, 1);
			ByteBuffer.PackInt(layoutData, 4, box.m_Id);
			LegacyBBeBObject physicalPages = new LegacyBBeBObject(this, ObjectType.ObjectInfo,
					ObjectFlags.PAGE_LAYOUT, layoutData);
			page.AddTagInt(TagId.ObjectInfoLink, physicalPages.m_Id);

			m_nNumPages++;
		}


		protected void BeginPage()
		{
			m_PageTextObjects.Clear();
			m_PageBoxObjects.Clear();
		}


		protected void EndPage()
		{
			//
			//
			//
			// NOTE - Right now it looks like nobody is callint this routine
			//
			//
			//


			BBeByteBuffer pageFiles = new BBeByteBuffer();
			pageFiles.putShort(3);
			pageFiles.putInt(m_nFontRecordId);

			BBeByteBuffer pageData = new BBeByteBuffer();
			pageData.putTag(TagId.Link);

			BBeByteBuffer layoutData = new BBeByteBuffer();
			layoutData.putInt(m_PageTextObjects.Count);

			List<LegacyBBeBObject>.Enumerator boxes = m_PageBoxObjects.GetEnumerator();
			foreach (LegacyBBeBObject text in m_PageTextObjects)
			{
				LegacyBBeBObject box = boxes.Current;
				boxes.MoveNext();

				pageFiles.putInt(text.m_Id);
				pageData.putInt(box.m_Id);
				layoutData.putInt(box.m_Id);
			}

			// We make the layout big so paragraphs can break over display pages
			LegacyBBeBObject physicalPages = new LegacyBBeBObject(this, ObjectType.ObjectInfo,
					ObjectFlags.PAGE_LAYOUT, layoutData);
			LegacyBBeBObject page = new LegacyBBeBObject(this, ObjectType.Page, ObjectFlags.NONE,
					pageData);
			page.AddTagInt(TagId.ObjectInfoLink, physicalPages.m_Id);
			page.AddTag(TagId.PageObjectIds, pageFiles.GetBuffer(), (int)pageFiles.Length);
			page.AddTagInt(TagId.Link, m_nMarginsId);

			// page.addTagInt(0x07, id);
			page.AddTagShort(TagId.HeadHeight, 0x34);
			page.AddTagShort(TagId.OddSideMargin, 0x37);
			page.AddTagShort(TagId.TextHeight, 800);
			page.AddTagShort(TagId.TextWidth, 600);
			page.AddTagShort(TagId.Layout, 0x34);

			page.AddTagInt(TagId.ParentPageTree, m_BookFile.m_Id);

			/*
			 * <Object ID="0x00000034" offset = "16830" size = "324" name =
			 * "ObjectType.Page"> <Tag ID= "0xf502" name= "*ObjectInfoLink" length=
			 * "4"> 0x00003a22 </Tag> <Tag ID= "0xf50b" name=
			 * "*ContainedObjectsList" length= "0"> <0b_5c count = "33"> 0x00000323
			 * 0x00001b74 0x0000032a 0x00001b78 0x00001b79 0x00000324 0x00001b77
			 * 0x00001b73 0x00000321 0x00001b72 0x00000326 0x00003a1e 0x00003a1d
			 * 0x00000329 0x000030f3 0x0000031f 0x000030f1 0x00001b7a 0x00001b7c
			 * 0x000030f2 0x00000322 0x00001b71 0x000039d4 0x00001b75 0x00000327
			 * 0x00001b76 0x00000320 0x00000328 0x00000325 0x0000032b 0x00001b7b
			 * 0x000030f0 0x00001b7d </0b_5c> </Tag> <Tag ID= "0xf503" name= "*Link"
			 * length= "4"> 0x00003a1c </Tag> <Tag ID= "0xf507" name= "Unknown_07"
			 * length= "4"> 0x000039d4 </Tag> <Tag ID= "0xf522" name= "Unknown_22"
			 * length= "2"> 0x0034 </Tag> <Tag ID= "0xf524" name= "Unknown_24"
			 * length= "2"> 0x0037 </Tag> <Tag ID= "0xf525" name= "*PageHeight"
			 * length= "2"> 0x0299 </Tag> <Tag ID= "0xf526" name= "*PageWidth"
			 * length= "2"> 0x01e0 </Tag> <Tag ID= "0xf535" name= "Unknown_35"
			 * length= "2"> 0x0034 </Tag> <Tag ID= "0xf57c" name= "*ParentPageTree"
			 * length= "4"> 0x0000004d </Tag> <Object ID="0x000039d4" offset =
			 * "2527901" size = "58" name = "ObjectType.Header"> <Tag ID= "0xf535"
			 * name= "Unknown_35" length= "2"> 0x0034 </Tag> <Tag ID= "0xf534" name=
			 * "Unknown_34" length= "4"> 0x000000ff </Tag> <Tag ID= "0xf536" name=
			 * "Unknown_36" length= "2"> 0x0000 </Tag> <Tag ID= "0xf537" name=
			 * "Unknown_37" length= "4"> 0x00000000 </Tag> <Tag ID= "0xf52e" name=
			 * "Unknown_2E" length= "2"> 0x0001 </Tag> <Stream flags="0x0000"/>
			 * <Stream length="10"/> 49f5000000001f030000 </Object>
			 */
			m_nNumPages++;
		}


		/// <summary>
		/// Serialize the metadata to a XML stream of bytes.
		/// </summary>
		/// <returns></returns>
		private byte[] SerializeMetaData()
		{
			m_MetaData.DocInfo.Page = m_nNumPages.ToString();
			XmlSerializer serializer = new XmlSerializer(typeof(BookMetaData));
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream, Encoding.Unicode);
			writer.NewLine = "\n";
			serializer.Serialize(writer, m_MetaData);
			writer.Close();

#if false   // Handy for debugging
            File.Delete("C:\\MetaData.xml");
            FileStream ostream = File.OpenWrite("C:\\MetaData.xml");
            ostream.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
            ostream.Close();
#endif

			return stream.GetBuffer();
		}

		public void WriteToFile(String name)
		{
			FileMode openMode = FileMode.OpenOrCreate;
			if (File.Exists(name))
			{
				openMode |= FileMode.Truncate;
			}
			FileStream fout = new FileStream(name, openMode);
			try
			{
				fout.Write(m_FileData.GetBuffer(), 0, (int)m_FileData.Length);
			}
			finally
			{
				fout.Close();
			}
		}

		private void putObjectData(LegacyBBeBObject obj)
		{
			// Record certain id's in LRF Header
			if (obj.m_eType == ObjectType.BookAtr)
			{
				m_FileData.putShort(LRF_ROOT_ID, obj.m_Id);
			}
			else if (obj.m_eType == ObjectType.TOC)
			{
				m_FileData.putShort(LRF_TOC_ID, obj.m_Id);
			}

			// Record position of sub file record in file
			obj.m_nFileLocation = m_FileData.Position;

			// Put in Object Type TAG
			m_FileData.putTagInt(TagId.ObjectStart, obj.m_Id);
			m_FileData.putShort((ushort)obj.m_eType);

			// Put in tags
			m_FileData.put(obj.TagData.GetBuffer(), 0, (int)obj.TagData.Length);

			// Write data block if we have one
			if (obj.StreamData != null)
			{
				m_FileData.put(obj.StreamData.GetBuffer(), 0, (int)obj.StreamData.Length);
			}

			// Write end of sub file marker
			m_FileData.putTag(TagId.ObjectEnd);

			// Record size of sub file data written
			obj.m_nSizeInFile = (int)(m_FileData.Position - obj.m_nFileLocation);
		}

		private int putCompressedBytes(byte[] aValue)
		{
			MemoryStream compressedOutStream = new MemoryStream();
			int nCompressedDataLen = ZLib.Compress(aValue, aValue.Length, compressedOutStream);

			m_FileData.put(compressedOutStream.GetBuffer(), 0, (int)compressedOutStream.Position);

			return nCompressedDataLen;
		}
	}
}
