/* 
 * Copyright (C) 2006-2007, Chris Mumford cmumford@cmumford.com
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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using BBeBLib;
using BBeBLib.Serializer;
using mshtml;

namespace BBeBinderPlugins
{
    class HTMLToBbebTransform
    {
        private BBeB m_Book;
        private ushort m_wNextObjectId = 0x32;
        private int m_nPageCount = 0;
		private TextAttrObject m_MainBodyTextAttr;
		private TextAttrObject m_CenteredTextAttr;
		private PageAttrObject m_PageAttr;
		private BlockAttrObject m_BlockAttr;
		private BlockObject m_StartReadingBlock;
		private PageObject m_StartReadingPage;
		private PageTreeObject m_PageTree;
		private TocObject m_TocObject;
		private PageObject m_CurrentPage = null;
		private List<PageObject> m_Pages = new List<PageObject>();
		private TagType m_eNewPageHeadingFilter = TagType.H2;
		private Dictionary<IHTMLDOMNode, ushort> m_HeadingNodePageId = new Dictionary<IHTMLDOMNode, ushort>();
		private Dictionary<IHTMLDOMNode, ushort> m_HeadingNodeTextBlockId = new Dictionary<IHTMLDOMNode, ushort>();
		private Dictionary<ushort, IHTMLDOMNode> m_TextObjectIdHeadingNode = new Dictionary<ushort, IHTMLDOMNode>();
		private BBeBLib.Serializer.CharacterMapper m_CharMapper;

        public	HTMLToBbebTransform( BBeBLib.Serializer.CharacterMapper charMapper )
        {
			m_CharMapper = charMapper;
        }

		/// <summary>
		/// If the TextBlockBuilder has any text then create a new text block and text object - 
		/// adding it to the page. If not then do nothing
		/// </summary>
		/// <param name="page"></param>
		/// <param name="tbBuilder"></param>
		/// <param name="attr"></param>
		/// <returns>The new text block object, or null if there was no text.</returns>
		private BlockObject FlushTextToBlock(PageObject page, TextBlockBuilder tbBuilder, TextAttrObject attr)
		{
			if (tbBuilder.HasText)
			{
				TextObject text = tbBuilder.CreateTextObject(attr.ID);
				BlockObject block = createTextBlock(page, text, attr);

				if (m_StartReadingBlock == null)
				{
					m_StartReadingBlock = block;
					m_StartReadingPage = page;
				}

				return block;
			}
			else
			{
				return null;
			}
		}

		public BBeB ParseHTML(HtmlDocument doc, BindingParams bindingParams, TocEntry tocEntries)
        {
            m_Book = new BBeB();

            byte[] thumb = File.ReadAllBytes(bindingParams.IconFile);
            setHeaderValues(thumb.Length);
			m_Book.MetaData = bindingParams.MetaData;
            m_Book.ThumbnailData = thumb;

            // Create our default Attribute objects
			createDefaultAttributeObjects(BBeB.ReaderPageWidth, BBeB.ReaderPageHeight);

			// cover page works, but it's ugly
			createCoverPage();

			m_CurrentPage = createPage();

			PageObject firstBookPage = m_CurrentPage;

			m_StartReadingBlock = null;
			m_StartReadingPage = null;

			addBookPage(m_CurrentPage);

            IHTMLDocument2 dom = (IHTMLDocument2)doc.DomDocument;
            IHTMLDOMNode domNode = (IHTMLDOMNode)dom.body;
            IHTMLDOMChildrenCollection children = (IHTMLDOMChildrenCollection)domNode.childNodes;

			TextBlockBuilder tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);
            foreach (IHTMLDOMNode child in children)
            {
				tbBuilder = ParseDomNode(child, tbBuilder);
            }

            PrintHTMLElementChildren(children);

            // If we have any text left then add it
			FlushTextToBlock(m_CurrentPage, tbBuilder, m_MainBodyTextAttr);

			finalizePage(m_CurrentPage);

			// Create the table of contents
			createTocPage(firstBookPage, tocEntries);

			m_TocObject.AddEntry(m_StartReadingPage.ID, m_StartReadingBlock.ID, "Start Reading");

			// Also serialize the table of contents object
			m_TocObject.Serialize();

			finalizeBook();

            return m_Book;
        }


        private string GetDefaultThumbnailImage()
        {
            const string strDefaultImage = "DefaultThumbnail.gif";

            DirectoryInfo dirInfo = new FileInfo(GetType().Assembly.Location).Directory;

            // Going up two directories is necessary in order for this program to run
            // in place from a developers machine. On the customers machine it will be 
            // beside the executable.
            for (int i = 0; i <= 2; i++)
            {
                FileInfo finfo = new FileInfo(Path.Combine(dirInfo.FullName, strDefaultImage));
                if (File.Exists(finfo.FullName))
                {
                    return finfo.FullName;
                }
                dirInfo = dirInfo.Parent;
            }

            return null;
        }

		private void addBookPage(PageObject page)
		{
			m_Book.Objects.Add(page);
			m_nPageCount++;
		}

		private void insertBookPage(PageObject page, PageObject pageBefore)
		{
			m_Book.Objects.Insert(m_Book.Objects.IndexOf(pageBefore), page);
			m_nPageCount++;
		}

        private PageObject createPage()
        {
            PageObject page = new PageObject(GetNextObjId());
			page.LinkObj = m_PageAttr;
			page.Tags.Add(new UInt32Tag(TagId.ParentPageTree, m_PageTree.ID));

			return page;
        }

        private void finalizePage( PageObject page )
        {
			page.Serialize();
        }

		/// <summary>
		/// Allocate and return the next available object ID.
		/// </summary>
		/// <returns>The next available object ID</returns>
        private ushort GetNextObjId()
        {
            return m_wNextObjectId++;
        }


        void setHeaderValues( int nGifThumbnailSize )
        {
			m_Book.Header = new BBeBHeader();
			m_Book.Header.wPseudoEncByte = 0x30;

			m_Book.Header.wDPI = 1600;

			m_Book.Header.dwTocObjectOffset = 0x1536;  // BUG is this legit?

	        // header.wDocInfoCompSize; - Metadata size
			m_Book.Header.ThumbnailType = StreamContents.GifImage;
			m_Book.Header.dwThumbSize = (uint)nGifThumbnailSize;
        }


        private void createDefaultAttributeObjects( ushort width, ushort height)
        {
            // Root Object
            BookAttrObject bookAttr = new BookAttrObject(GetNextObjId());
            bookAttr.createDefaultTags();
            m_Book.Objects.Add(bookAttr);
            m_Book.Header.dwRootObjectId = bookAttr.ID;

			// Create an empty table of contents.
			m_TocObject = new TocObject(GetNextObjId());
			m_Book.Objects.Add(m_TocObject);
			m_Book.Header.dwTocObjectId = m_TocObject.ID;

            m_MainBodyTextAttr = new TextAttrObject(GetNextObjId());
			m_MainBodyTextAttr.createDefaultTags(BlockAlignment.Left);
            m_Book.Objects.Add(m_MainBodyTextAttr);

			m_CenteredTextAttr = new TextAttrObject(GetNextObjId());
			m_CenteredTextAttr.createDefaultTags(BlockAlignment.Center);
			m_Book.Objects.Add(m_CenteredTextAttr);

            // Fill this one in later need id now for the BookAttrObject
            m_PageTree = new PageTreeObject(GetNextObjId());
            m_Book.Objects.Add(m_PageTree);
            bookAttr.Tags.Add(new UInt32Tag(TagId.ChildPageTree, m_PageTree.ID));

            m_PageAttr = new PageAttrObject(GetNextObjId());
            m_PageAttr.createDefaultTags();
            m_Book.Objects.Add(m_PageAttr);

            m_BlockAttr = new BlockAttrObject(GetNextObjId());
            m_BlockAttr.createDefaultTags( width, height );
            m_Book.Objects.Add(m_BlockAttr);
        }

		private void createCoverPage()
		{
			PageObject coverPage = createPage();

			if (!string.IsNullOrEmpty(m_Book.MetaData.BookInfo.Title))
			{
				TextBlockBuilder tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);
				tbBuilder.Append(TagId.EOL);
				tbBuilder.Append(TagId.EOL);
				tbBuilder.Append(m_Book.MetaData.BookInfo.Title);
				FlushTextToBlock(coverPage, tbBuilder, m_CenteredTextAttr);
			}

			if (!string.IsNullOrEmpty(m_Book.MetaData.BookInfo.Author))
			{
				TextBlockBuilder tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);
				tbBuilder.Append("By ");
				tbBuilder.Append(m_Book.MetaData.BookInfo.Author);
				FlushTextToBlock(coverPage, tbBuilder, m_CenteredTextAttr);
			}

			if (!string.IsNullOrEmpty(m_Book.MetaData.BookInfo.Publisher))
			{
				TextBlockBuilder tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);
				tbBuilder.Append("Publisher: ");
				tbBuilder.Append(m_Book.MetaData.BookInfo.Publisher);
				FlushTextToBlock(coverPage, tbBuilder, m_CenteredTextAttr);
			}

			if (!string.IsNullOrEmpty(m_Book.MetaData.BookInfo.FreeText))
			{
				TextBlockBuilder tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);
				tbBuilder.Append(TagId.EOL);
				tbBuilder.Append(TagId.EOL);
				tbBuilder.Append("Notes: ");
				tbBuilder.Append(m_Book.MetaData.BookInfo.FreeText);
				FlushTextToBlock(coverPage, tbBuilder, m_MainBodyTextAttr);
			}

			addBookPage(coverPage);

			finalizePage(coverPage);
		}


		private void AddTocEntry(TocEntry entry, int level, PageObject tocPage)
		{
			HtmlElement element = (HtmlElement)entry.Tag;
			IHTMLDOMNode entryNode = (IHTMLDOMNode)element.DomElement;

			Debug.WriteLine("Creating TOC entry \"" + entry.Title + "\"");

			TextBlockBuilder tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);

			if (m_HeadingNodePageId.ContainsKey(entryNode))
			{
				ushort wHeadingTextPageId = m_HeadingNodePageId[entryNode];
				ushort wHeadingTextBlockId = m_HeadingNodeTextBlockId[entryNode];

				// Create the one button object that points to the first block
				// in the document page.
				ButtonObject button = new ButtonObject(GetNextObjId());
				button.ButtonType = ButtonType.PushButton;
				button.SetObjectIds(wHeadingTextPageId, wHeadingTextBlockId);
				button.Serialize();
				m_Book.Objects.Add(button);

				// TODO Need to set a different block attribute to control the X
				// position instead of just adding spaces.
				for (int i = 0; i < level; i++)
				{
					tbBuilder.Append("    ");
				}

				tbBuilder.Append(TagId.BeginButton, (uint)button.ID);
				tbBuilder.Append(entry.Title);
				tbBuilder.Append(TagId.EndButton);

				FlushTextToBlock(tocPage, tbBuilder, m_MainBodyTextAttr);

				tocPage.Children.Add(button);
			}

			// Now for all of the children
			foreach (TocEntry childEntries in entry.Children)
			{
				AddTocEntry(childEntries, level + 1, tocPage);
			}
		}


		/// <summary>
		/// Create the table of contents page (not to be confused with the TOC object)
		/// which refers to this page. This page is the one that has the links to the
		/// other pages within this book.
		/// </summary>
		private void createTocPage( PageObject firstPage, TocEntry tocEntries )
		{
			// Now create a TOC page
			PageObject tocPage = createPage();

			TextBlockBuilder tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);

			tbBuilder.Append("Table of Contents");
			tbBuilder.Append(TagId.EOL);

			BlockObject headingBlock = FlushTextToBlock(tocPage, tbBuilder, m_CenteredTextAttr);

			foreach (TocEntry tocEntry in tocEntries.Children)
			{
				AddTocEntry(tocEntry, 0, tocPage);
			}

			insertBookPage(tocPage, firstPage);

			finalizePage(tocPage);

			m_TocObject.AddEntry(tocPage.ID, headingBlock.ID, "Table of Contents");
		}

		private void addPageText(PageObject page, TextObject text, BlockObject block, 
									TextAttrObject attr, uint wExtraTagId, uint width, uint height)
		{
			// Add our TextObject
			m_Book.Objects.Add(text);

			// Refer the block to the text object
			block.ObjectLink = text.ID;

			ObjectInfoObject objInfo = new ObjectInfoObject(GetNextObjId());
			objInfo.addLayoutTags(block.ID);
			if (wExtraTagId != 0x0)
			{
				objInfo.addLayoutTags(wExtraTagId);
			}
			m_Book.Objects.Add(objInfo);
			page.InfoObj = objInfo;

			// Now add out objects to the page
			page.Children.Add(attr);
			page.Children.Add(text);
			page.Children.Add(block);

			page.StreamTags.Add(new UInt32Tag(TagId.Link, block.ID));
		}

		/// <summary>
		/// Create a new text block object.
		/// </summary>
		/// <param name="page">The page to put the new text block into</param>
		/// <param name="text">The text object to place in the new block</param>
		/// <param name="attr">The text attributes.</param>
		/// <returns>The newly created text block.</returns>
		private BlockObject createTextBlock(PageObject page, TextObject text, TextAttrObject attr)
        {
            // Create a block Object
            BlockObject block = new BlockObject(GetNextObjId());
            block.BlockLink = m_BlockAttr.ID;
            m_Book.Objects.Add(block);

			addPageText(page, text, block, attr, 0x0, BBeB.ReaderPageWidth, BBeB.ReaderPageHeight);

			// Now that we've created the text block we can associate
			// it with the HTML DOM Node that created it. We need this 
			// association for TOC creation.
			if (m_TextObjectIdHeadingNode.ContainsKey(text.ID))
			{
				IHTMLDOMNode node = m_TextObjectIdHeadingNode[text.ID];
				m_HeadingNodeTextBlockId[node] = block.ID;
			}

			return block;
        }

        private void addPageImage(PageObject page, string src, ushort width, ushort height)
        {
            // Load image and then save it out as a Jpeg (just so we know what
            // format the image is - we could of course just check the extension
            // but the LRF format only supports a few image types
            // Also JPeg seems to be better supported
            byte[] data = null;
            string file = src;
            if (file.StartsWith("http"))
            {
                // Grab image
                file = "c:\\image.jpg";
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(src, file);
            }

            file = file.Replace("file:///", "").Replace("%20", " ");
            if (!File.Exists(file))
                return;
            System.Drawing.Bitmap img = new System.Drawing.Bitmap(file);
            MemoryStream mem = new MemoryStream();
            img.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
            data = mem.GetBuffer();
            mem.Close();
            img.Dispose();

            // Create a block Object
            BlockObject block = new BlockObject(GetNextObjId());
            block.BlockLink = m_BlockAttr.ID;
            block.BlockHeight = height;
            block.BlockWidth = width;
            block.BlockRule = 34;
            m_Book.Objects.Add(block);

            // Create our ImageObject
            ImageObject io = new ImageObject(GetNextObjId());
            io.setRect(0, 0, width, height);
            io.setSize(width, height);
            m_Book.Objects.Add(io);

			// Center the image block on the page
			int left = (BBeB.ReaderPageWidth - width) / 2;
			if (left < 0)
				left = 0;
			page.StreamTags.Add(new UInt16Tag(TagId.LocationX, (ushort)left));
			page.StreamTags.Add(new UInt32Tag(TagId.Link, block.ID));

            // Set the image id in the block
            block.ObjectLink = io.ID;

            ushort streamId = GetNextObjId();
            io.setImageStreamId(streamId);

            // Create our ImageStream
            ImageStreamObject istream = new ImageStreamObject(streamId);
			istream.Data = data;
			istream.Contents = StreamContents.JpegImage;
            m_Book.Objects.Add(istream);

            ObjectInfoObject objInfo = new ObjectInfoObject(GetNextObjId());
            objInfo.addLayoutTags(block.ID);
            m_Book.Objects.Add(objInfo);
			page.InfoObj = objInfo;

            // Now add out objects to the page
			page.Children.Add(block);
			page.Children.Add(io);
			page.Children.Add(m_BlockAttr);
			page.Children.Add(istream);
		}

        private void finalizeBook()
        {
            byte[] pageNumberData = new byte[(6 * m_nPageCount) + 4];
            byte[] bookPagesList = new byte[(4 * m_nPageCount) + 2];
            pageNumberData[0] = bookPagesList[0] = (byte)(m_nPageCount & 0x00ff);
            pageNumberData[1] = bookPagesList[1] = (byte)((m_nPageCount >> 8) & 0x00ff);

            int pnIndex = 4;
            int bpIndex = 2;
            for (int i = 0; i < m_Book.Objects.Count; i++)
            {
                BBeBObject subFile = m_Book.Objects[i];
                if (subFile.Type == ObjectType.Page)
                {
                    pageNumberData[pnIndex] = bookPagesList[bpIndex] = (byte)(subFile.ID & 0x00ff);
                    pageNumberData[pnIndex + 1] = bookPagesList[bpIndex + 1] = (byte)((subFile.ID >> 8) & 0x00ff);

                    // Set to say 1 page per page (a kludge for now)
                    pageNumberData[pnIndex + 4] = 1;
                    pageNumberData[pnIndex + 5] = 0;

                    pnIndex += 6;
                    bpIndex += 4;
                }
            }

            ObjectInfoObject objInfo = new ObjectInfoObject(GetNextObjId());
            objInfo.addPageNumbersTags(pageNumberData);
            m_Book.Objects.Add(objInfo);

            m_PageTree.setObjectInfoLink(objInfo.ID);
            m_PageTree.Tags.Add(new ByteArrayTag(TagId.PageList, bookPagesList));
        }


        private void PrintHTMLElementChildren(IHTMLDOMChildrenCollection nodes)
        {
            foreach (IHTMLDOMNode child in nodes)
            {
                //Debug.Write(child.nodeName + ": ");
                //Debug.WriteLine(child.nodeValue);
                if (child.hasChildNodes())
                {
                    PrintHTMLElementChildren((IHTMLDOMChildrenCollection)child.childNodes);
                }
            }
        }

		private static int GetHeadingLevel(TagType tagType)
		{
			switch (tagType)
			{
				case TagType.H1: return 1;
				case TagType.H2: return 2;
				case TagType.H3: return 3;
				case TagType.H4: return 4;
				case TagType.H5: return 5;
				case TagType.H6: return 6;
				default: return 0;
			}
		}

		private static ushort GetHeadingFontSize(TagType tagType)
		{
			switch (tagType)
			{
				case TagType.H1: return 200;
				case TagType.H2: return 160;
				case TagType.H3: return 140;
				case TagType.H4: return 120;
				case TagType.H5: return 115;
				case TagType.H6: return 110;
				default: return LegacyBBeB.DefaultFontSize;
			}
		}

		/// <summary>
		/// Walk the supplied HTML DOM node (recursively) and add its contents into the
		/// supplied page using the supplied TextBlockBuilder.
		/// </summary>
		/// <remarks>When this routine is done there may be some residual text still in
		/// tbBuilder. The caller is resonsible for checking this and adding it to the
		/// page if present.</remarks>
		/// <param name="node">The HTML DOM node to recursively walk.</param>
		/// <param name="tbBuilder">The TextBlockBuilder to put the text into.</param>
		private TextBlockBuilder ParseDomNode(IHTMLDOMNode node, TextBlockBuilder tbBuilder)
		{
			TagType tagType = GetTagType(node.nodeName);

			switch (tagType)
			{
				case TagType.IMG:

					// Before we add the image, see if we need to write the text object first
					if (tbBuilder.HasText)
					{
						// Yes it has
						tbBuilder.Append(TagId.EOL);
						FlushTextToBlock(m_CurrentPage, tbBuilder, m_MainBodyTextAttr);
						tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);
					}

					IHTMLAttributeCollection attribs = (IHTMLAttributeCollection)node.attributes;
					object name = "src";
					string src = ((IHTMLDOMAttribute)attribs.item(ref name)).nodeValue.ToString();
					name = "height";
					string height = ((IHTMLDOMAttribute)attribs.item(ref name)).nodeValue.ToString();
					name = "width";
					string width = ((IHTMLDOMAttribute)attribs.item(ref name)).nodeValue.ToString();

					addPageImage(m_CurrentPage, src, ushort.Parse(width), ushort.Parse(height));
					break;

				case TagType.text:
					AppendTextToBlock((string)node.nodeValue, tbBuilder);
					break;

				case TagType.I:
					tbBuilder.Append(TagId.ItalicBegin);
					break;
				case TagType.B:
					tbBuilder.Append(TagId.FontWeight, LegacyBBeB.k_BoldFontWeight);
					break;
				case TagType.SUP:
					tbBuilder.Append(TagId.BeginSup);
					break;
				case TagType.SUB:
					tbBuilder.Append(TagId.BeginSub);
					break;
				case TagType.H1:
				case TagType.H2:
				case TagType.H3:
				case TagType.H4:
				case TagType.H5:
				case TagType.H6:

					FlushTextToBlock(m_CurrentPage, tbBuilder, m_MainBodyTextAttr);
					tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);

					if (GetHeadingLevel(tagType) <= GetHeadingLevel(m_eNewPageHeadingFilter))
					{
						if (m_CurrentPage.Children.Count > 0)	// If current page not empty
						{
							// Start a new page
							finalizePage(m_CurrentPage);

							m_CurrentPage = createPage();

							addBookPage(m_CurrentPage);
						}
					}

					m_HeadingNodePageId[node] = m_CurrentPage.ID;
					m_TextObjectIdHeadingNode[tbBuilder.TextObjectId] = node;

					tbBuilder.Append(TagId.FontSize, GetHeadingFontSize(tagType));
					break;
			}

			if (node.hasChildNodes())
			{
                IHTMLDOMChildrenCollection childNodes = (IHTMLDOMChildrenCollection)node.childNodes;
                foreach (IHTMLDOMNode child in childNodes)
				{
					tbBuilder = ParseDomNode(child, tbBuilder);
				}
			}

			switch (tagType)
			{
				case TagType.I:
					tbBuilder.Append(TagId.ItalicEnd);
					break;
				case TagType.B:
					tbBuilder.Append(TagId.FontWeight, LegacyBBeB.k_NormalFontWeight);
					break;
				case TagType.SUP:
					tbBuilder.Append(TagId.EndSup);
					break;
				case TagType.SUB:
					tbBuilder.Append(TagId.EndSub);
					break;
				case TagType.P:
					tbBuilder.Append(TagId.EOL);
					tbBuilder.Append(TagId.EOL);
					break;
				case TagType.H1:
				case TagType.H2:
				case TagType.H3:
				case TagType.H4:
				case TagType.H5:
				case TagType.H6:
					tbBuilder.Append(TagId.FontSize, LegacyBBeB.DefaultFontSize);
					FlushTextToBlock(m_CurrentPage, tbBuilder, m_MainBodyTextAttr);
					tbBuilder = new TextBlockBuilder(GetNextObjId(), m_CharMapper);
					break;
				
				case TagType.BR:
					tbBuilder.Append(TagId.EOL);
					break;
			}

			return tbBuilder;
		}

		private TagType GetTagType(string tagName)
		{
			if (string.Compare(tagName, "#text", true) == 0)
			{
				return TagType.text;
			}
            else if (string.Compare(tagName, "IMG", true) == 0)
            {
                return TagType.IMG;
            }
            else if (string.Compare(tagName, "P", true) == 0)
            {
                return TagType.P;
            }
            else if (string.Compare(tagName, "I", true) == 0 || string.Compare(tagName, "EM", true) == 0)
			{
				return TagType.I;
			}
			else if (string.Compare(tagName, "B", true) == 0 || string.Compare(tagName, "STRONG", true) == 0)
			{
				return TagType.B;
			}
			else if (string.Compare(tagName, "BR", true) == 0)
			{
				return TagType.BR;
			}
			else if (string.Compare(tagName, "H1", true) == 0)
			{
				return TagType.H1;
			}
			else if (string.Compare(tagName, "H2", true) == 0)
			{
				return TagType.H2;
			}
			else if (string.Compare(tagName, "H3", true) == 0)
			{
				return TagType.H3;
			}
			else if (string.Compare(tagName, "H4", true) == 0)
			{
				return TagType.H4;
			}
			else if (string.Compare(tagName, "H5", true) == 0)
			{
				return TagType.H5;
			}
			else if (string.Compare(tagName, "H6", true) == 0)
			{
				return TagType.H6;
			}
			else if (string.Compare(tagName, "SUP", true) == 0)
			{
				return TagType.SUP;
			}
			else if (string.Compare(tagName, "SUB", true) == 0)
			{
				return TagType.SUB;
			}

//			Debug.WriteLine("Unknown tag type: " + tagName);
			return TagType.Unknown;
		}

        private static void AppendTextToBlock(string text, TextBlockBuilder builder)
        {
            if ( string.IsNullOrEmpty(text) )
            {
                return;
            }

            // Replace \r\n combinations with \n, and then replace \r with \n
            text = text.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");

            char lastchar = '\0';
            foreach (char ch in text)
            {
                if (ch == '\n' ) 
                {
                    builder.Append(TagId.EOL);
                }
                else
                {
					builder.AppendChar(ch);
                    lastchar = ch;
                }
            }
        }
    }
}
