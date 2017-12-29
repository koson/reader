using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using BBeBLib;
using BBeBLib.Serializer;
using mshtml;
using MiscUtil;
using BBeBinderPluginInterface;

namespace BBeBinder
{
    public partial class MainForm : Form, BBeBinderPluginInterface.IPluginHost
    {
		TocEntry m_TocEntryRoot = null;
		BindingParams m_BindingParams = new BindingParams();
		BindingParamsProperties m_PropBindingParams;
		private RegistrySettings m_Registry = new RegistrySettings("BBeBinder", "BBeBinder");
		BBeBLib.Serializer.CharacterMapper m_CharMapper;

        Regex m_rexHeading = new Regex("^H(?<number>[1-6])$", 
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public MainForm()
        {
            InitializeComponent();

            // Load Plugins
			try
			{
				PluginServices.Instance.LoadPlugins(this, Application.StartupPath);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			LoadCharacterMap();

            // Build open and save menu
            foreach (AvailablePlugin plugin in PluginServices.Instance.AvailablePlugins)
            {
                if (plugin.Instance.TypeOfPlugin == PluginType.Input)
                {
                    foreach (string filetype in ((BBeBinderInputPlugin)plugin.Instance).SupportedFiles)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(filetype);
                        item.Tag = plugin;
                        item.Click += new EventHandler(OpenMenu_Click);
                        m_OpenMenuItem.DropDownItems.Add(item);
                    }
                }
                if (plugin.Instance.TypeOfPlugin == PluginType.Output)
                {
                    foreach (string filetype in ((BBeBinderOutputPlugin)plugin.Instance).SupportedFiles)
                    {
                        ToolStripMenuItem item = new ToolStripMenuItem(filetype);
                        item.Tag = plugin;
                        item.Click += new EventHandler(SaveMenu_Click);
                        m_SaveAsMenuItem.DropDownItems.Add(item);
                    }
                }
            }

			m_PropBindingParams = new BindingParamsProperties(m_BindingParams);
        }

		public void LoadCharacterMap()
		{
			m_CharMapper = new CharacterMapper();

            string filename = GetCharMapFile();
            if (filename != null)
            {
                StreamReader reader = new StreamReader(filename, Encoding.Unicode);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length == 3 && line[0] != '#' && line[1] == ':')
                    {
                        m_CharMapper.Add(line[0], line[2]);
                    }
                }
            }
            else
                MessageBox.Show("The Character Mapping file (charmap.txt) is missing!\n");
		}

        // IPlugInHost Implementation
        public void SetFileDetails(string filename)
        {
            Text = "BBeBinder - " + filename;
        }

        public void SetDocumentURI(string uri)
        {
            m_HtmlEditor.Navigate(uri);
        }

        public void SetDocument(string htmlData)
        {
            m_HtmlEditor.setDocumentText(htmlData);
        }

        public void SetDocument(System.IO.Stream stream)
        {
            m_HtmlEditor.setDocumentStream(stream);
        }
        public void Status(string statusMsg)
        {
            StatusText.Text = statusMsg;
            Application.DoEvents();
        }
        // End of IPlugInHost Implementation

        private void OpenMenu_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            AvailablePlugin plugin = (AvailablePlugin)item.Tag;
            ((BBeBinderInputPlugin)plugin.Instance).LoadDocument();
        }

        private void SaveMenu_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            AvailablePlugin plugin = (AvailablePlugin)item.Tag;
			((BBeBinderOutputPlugin)plugin.Instance).SaveDocument(m_BindingParams,
				m_TocEntryRoot, m_CharMapper, m_HtmlEditor.Document);
        }

        private int GetHeadingNumber(string tagName)
        {
            Match m = m_rexHeading.Match(tagName);
            if (m.Success)
            {
                return int.Parse(m.Result("${number}"));
            }
            else
            {
                return 0;
            }
        }

		private static string GetHeadingInnerText(HtmlElement element)
		{
			return element.InnerText;
		}

		private TocEntry CreateTableOfContents()
		{
			List<TocEntry>[] headings = new List<TocEntry>[10];

			TocEntry rootEntry = new TocEntry("temp root");

			Array.Clear(headings, 0, headings.Length);
			headings[0] = rootEntry.Children;

			foreach (HtmlElement element in m_HtmlEditor.Document.All)
			{
				int nHeadingNum = GetHeadingNumber(element.TagName);
				if (nHeadingNum > 0)
				{
					int d;
					int nParentIdx = 0;

					for (d = nHeadingNum - 1; d > 0; d--)
					{
						if (headings[d] != null)
						{
							nParentIdx = d;
							break;
						}
					}

					Array.Clear(headings, nParentIdx + 1, headings.Length - (nParentIdx + 1));

					TocEntry tocEntry = new TocEntry(GetHeadingInnerText(element));
					tocEntry.Tag = element;

					headings[nParentIdx].Add(tocEntry);
					headings[nHeadingNum] = tocEntry.Children;
				}
			}

			return rootEntry;
		}

		private void AddTocViewNodes(TocEntry tocEntry, TreeNodeCollection nodes)
		{
			TreeNode node = new TreeNode(tocEntry.Title);
			node.Tag = tocEntry;
			nodes.Add(node);

			foreach (TocEntry child in tocEntry.Children)
			{
				AddTocViewNodes(child, node.Nodes);
			}
		}


		/// <summary>
		/// Fill the table of contents tree view.
		/// </summary>
        private void FillTocView()
        {
			TocEntry toc = CreateTableOfContents();

			// The top entry is a fake entry to contain the others. 

			if (m_TocEntryRoot == null || !m_TocEntryRoot.Equals(toc))
			{
				m_TocTree.Nodes.Clear();
				foreach (TocEntry entry in toc.Children)
				{
					AddTocViewNodes(entry, m_TocTree.Nodes);
				}
				m_TocTree.ExpandAll();

				m_TocEntryRoot = toc;
			}
        }


		/// <summary>
		/// Called after this form has finished loading.
		/// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
			FormPersister persister = new FormPersister(m_Registry);
			persister.LoadSize(this);
			
			m_HtmlEditor.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Browser_DocumentCompleted);

			m_BindingParams.IconFile = GetDefaultThumbnailImage();
        }


		/// <summary>
		/// Called after the browser has completed loading its document.
		/// </summary>
        private void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
				m_TocEntryRoot = null;
				FillTocView();

				ExtractBookMetaData(m_HtmlEditor.Document, 
									m_BindingParams.MetaData);
				
				m_BindingParams.MetaData.Fixup();

				m_PropertyGrid.SelectedObject = m_PropBindingParams;

				m_TocRefreshTimer.Enabled = true;
            }
            catch ( Exception ex )
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }

            Cursor.Current = Cursors.Default;
            StatusText.Text = "Finished.";
        }

        public void ExtractBookMetaData(HtmlDocument doc, BookMetaData data)
        {
            data.BookInfo.Author = null;
            data.BookInfo.Title = null;
            data.BookInfo.Publisher = null;
            data.DocInfo.Language = null;
            IHTMLDocument2 dom = (IHTMLDocument2)doc.DomDocument;

            foreach (IHTMLDOMNode node in dom.all)
            {
                if (string.Compare(node.nodeName, "meta", true) == 0)
                {
                    IHTMLMetaElement meta = (IHTMLMetaElement)node;
                    if (string.Compare(meta.name, "author") == 0)
                    {
                        data.BookInfo.Author = meta.content;
                    }
                    else if (string.Compare(meta.name, "publisher") == 0)
                    {
                        data.BookInfo.Publisher = meta.content;
                    }
                    else if (string.Compare(meta.name, "content-language") == 0)
                    {
                        data.DocInfo.Language = meta.content;
                    }
                }
                else if (string.Compare(node.nodeName, "body", true) == 0)
                {
                    // No more meta tags.
                    break;
                }
            }

            // Get the title
            data.BookInfo.Title = GetDocumentTitle(doc);

            // If the title or the auther is blank then try to extract it from the
            // filename
            if (data.BookInfo.Title == null || data.BookInfo.Author == null )
            {
                string filename = doc.Url.LocalPath;
				try
				{
					FileInfo finfo = new FileInfo(filename);
					if (finfo.Extension.Length > 0)
					{
						filename = finfo.Name.Substring(0, finfo.Name.Length - finfo.Extension.Length);
					}
					else
					{
						filename = finfo.Name;
					}

					string author = null;
					string title = null;
					if (filename.IndexOf(" - ") != -1)
					{
						author = filename.Substring(0, filename.IndexOf(" - "));
						title = filename.Substring(filename.IndexOf(" - ") + 3);
					}
					else
						title = filename;

					if (data.BookInfo.Title == null && title != null)
						data.BookInfo.Title = title;
					if (data.BookInfo.Author == null && author != null)
						data.BookInfo.Author = author;
				}
				catch (NotSupportedException)
				{
					// If the URL doesn't map to a filename like "about:blank"
					// then this exception can be thrown. We just ignore it.
				}
            }
        }
        
        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string GetDocumentTitle(HtmlDocument doc)
        {
            if (!string.IsNullOrEmpty(doc.Title))
            {
                return doc.Title;
            }

            // Title wasn't specified in the header, so get one of the H tags
            foreach (HtmlElement element in m_HtmlEditor.Document.All)
            {
                if (string.Compare(element.TagName, "H1", true) == 0 ||
                    string.Compare(element.TagName, "H2", true) == 0)
                {
                    return GetHeadingInnerText(element);
                }
            }

            return null;
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


		private string GetCharMapFile()
		{
			const string strCharMap = "charmap.txt";

			DirectoryInfo dirInfo = new FileInfo(GetType().Assembly.Location).Directory;

			// Going up two directories is necessary in order for this program to run
			// in place from a developers machine. On the customers machine it will be 
			// beside the executable.
			for (int i = 0; i <= 2; i++)
			{
				FileInfo finfo = new FileInfo(Path.Combine(dirInfo.FullName, strCharMap));
				if (File.Exists(finfo.FullName))
				{
					return finfo.FullName;
				}
				dirInfo = dirInfo.Parent;
			}

			return null;
		}

        private void TocTimer_Tick(object sender, EventArgs e)
        {
			FillTocView();
        }

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			FormPersister persister = new FormPersister(m_Registry);
			persister.SaveSize(this);
			
			m_TocRefreshTimer.Enabled = false;
		}

		private void TocTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (m_TocTree.SelectedNode == null)
			{
				return;
			}

			TocEntry itemTocEntry = (TocEntry)m_TocTree.SelectedNode.Tag;
			HtmlElement selectedElement = (HtmlElement)itemTocEntry.Tag;

			selectedElement.ScrollIntoView(true);
		}

		private void m_HtmlEditor_Load(object sender, EventArgs e)
		{

		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm aboutDialog = new AboutForm();

			aboutDialog.ShowDialog();
        }

        private void m_ToggleEditMode_Click(object sender, EventArgs e)
        {
            m_ToggleEditMode.Checked = !m_ToggleEditMode.Checked;
            if (m_ToggleEditMode.Checked)
            {
                m_HtmlEditor.SetDesignMode = true;
            }
            else
                m_HtmlEditor.SetDesignMode = false;

        }

		private void ImportFromClipboardMenuItem_Click(object sender, EventArgs e)
		{
			if (Clipboard.ContainsText(TextDataFormat.Text))
			{
				MemoryStream stream = new MemoryStream();
				BinaryWriter writer = new BinaryWriter(stream);
				writer.Write(Clipboard.GetText(TextDataFormat.Text));
				writer.Flush();

				stream.Position = 0;
				Gutenberg.TextFormatter formatter = new Gutenberg.TextFormatter();

				Gutenberg.Document doc = formatter.ProcessTextBook(stream);
				m_HtmlEditor.DocumentText = doc.SerializeToHtml();
			}
			else if (Clipboard.ContainsText(TextDataFormat.Html))
			{
				m_HtmlEditor.DocumentText = Clipboard.GetText(TextDataFormat.Html);
			}
			else
			{
				Debug.WriteLine("Unknown clipboard data format");
			}
		}

        private void m_PageToImage_Click(object sender, EventArgs e)
        {
            Bitmap bmp = m_HtmlEditor.GetPageAsImage();
            if (bmp == null)
                return;

            string strWriteDir = "C:\\lrfdecomp";
            Directory.CreateDirectory(strWriteDir);

            Status("Splitting images and creating new Html file.....");

			Bitmap[] bmps = ImageUtils.SplitImage(bmp, BBeB.ReaderPageWidth, BBeB.ReaderPageHeight-50);

            StringBuilder html = new StringBuilder();
            html.AppendLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 3.2 Final//EN\">");
            html.AppendLine("<HTML>");
            html.AppendLine("<HEAD>");
            html.AppendLine("<TITLE></TITLE>");
            html.AppendLine("</HEAD>");
            html.AppendLine("<BODY>");

            for (int i = 0; i < bmps.Length; ++i)
            {
                string file = "image_" + i + ".png";
                html.AppendLine("<IMG SRC=\"" + file + "\">");

                bmps[i].Save(Path.Combine(strWriteDir, file), System.Drawing.Imaging.ImageFormat.Png);
            }
            html.AppendLine("</BODY>");
            html.AppendLine("</HTML>");

            string outputFile = Path.Combine(strWriteDir, "test.html");
            using (StreamWriter sw = new StreamWriter(outputFile))
            {
                sw.Write(html.ToString());
            }
            m_HtmlEditor.Navigate( "file://" + outputFile );
        }

        private void m_NewMenuItem_Click(object sender, EventArgs e)
        {
            m_HtmlEditor.Navigate("about:blank");
        }

    }
}
