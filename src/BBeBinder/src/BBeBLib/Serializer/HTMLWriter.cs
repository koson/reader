using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Web;
using System.Diagnostics;
using BBeBLib;

namespace BBeBLib.Serializer
{
    public class HTMLWriter : BBeBWriter
    {
		TagType m_eLastHeadingTag = TagType.Unknown;

        public HTMLWriter(BBeB book) :
            base(book)
        {
        }

        protected override void start()
        {
            // Add HTML header (basic)
            data.AppendLine( "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" );
            data.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            data.AppendFormat("<head>\n   <title>{0}</title>\n", book.MetaData.BookInfo.Title);
            data.AppendFormat("   <meta name=\"author\" content=\"{0}\" />\n", book.MetaData.BookInfo.Author);
            data.AppendFormat("   <meta name=\"publisher\" content=\"{0}\" />\n", book.MetaData.BookInfo.Publisher );
            data.AppendFormat("   <meta name=\"content-language\" content=\"en\" />\n", book.MetaData.DocInfo.Language );
            data.AppendLine( "</head>\n<body>" );
        }

        protected override void startPage()
        {
        }

		protected static TagType ConvertFontSizeToHeading(ushort size)
		{
			switch (size)
			{
				case 200: return TagType.H1;
				case 160: return TagType.H2;
				case 140: return TagType.H3;
				case 120: return TagType.H4;
				case 115: return TagType.H5;
				case 110: return TagType.H6;
				default: return TagType.text;
			}
		}

		protected override void handleTag(BBeBTag tag)
		{
			switch (tag.Id)
			{
				case TagId.Text:
					string encoded = HttpUtility.HtmlEncode(((TextTag)tag).Value);
					StringBuilder utf8 = new StringBuilder();
					foreach (char ch in encoded)
					{
						if ((ushort)ch > 127)
						{
							utf8.AppendFormat("&#{0};", (ushort)ch);
						}
						else
						{
							utf8.Append(ch);
						}
					}
					data.Append( utf8.ToString() );
					break;
				case TagId.EOL:
					data.Append("<br />");
					break;
				case TagId.ItalicBegin:
					data.Append("<i>");
					break;
				case TagId.ItalicEnd:
					data.Append("</i>");
					break;
				case TagId.BeginSup:
					data.Append("<sup>");
					break;
				case TagId.EndSup:
					data.Append("</sup>");
					break;
				case TagId.BeginSub:
					data.Append("<sub>");
					break;
				case TagId.EndSub:
					data.Append("</sub>");
					break;
				case TagId.BeginButton:
					// TODO Can probably turn this into a hyperlink
					break;
				case TagId.FontSize:
					UInt16Tag fontSizeTag = (UInt16Tag)tag;
					if (fontSizeTag.Value == 100)
					{
						data.AppendFormat("</{0}>", m_eLastHeadingTag.ToString());
						m_eLastHeadingTag = TagType.Unknown;
					}
					TagType type = ConvertFontSizeToHeading(fontSizeTag.Value);
					switch (type)
					{
						case TagType.H1:
						case TagType.H2:
						case TagType.H3:
						case TagType.H4:
						case TagType.H5:
						case TagType.H6:
							data.AppendFormat("<{0}>", type.ToString());
							m_eLastHeadingTag = type;
							break;
					}
					break;
				case TagId.BeginPage:
					data.Append("<p>");
					break;
				case TagId.EndPage:
					data.Append("</p>");
					break;
				case TagId.FontWeight:
					break;
				case TagId.FontWidth:
					break;
				case TagId.EndButton:
				case TagId.KomaPlot:
					break;
				default:
					break;
			}
		}

        protected override void handleImage(ImageInfo info)
        {
            // Add image to data
            string tmp = String.Format("<img src=\"{0}.{1}\" alt=\"{2}\" width=\"{3}\" height=\"{4}\"/>",
                   info.name, info.type, info.name, info.position.Width, info.position.Height);
            data.Append( tmp );
        }

        protected override void endPage()
        {
        }

        protected override void finished()
        {
            data.Append("\n</body>\n</html>");
        }
    }
}
