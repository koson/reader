using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Gutenberg;


namespace BBeBLib
{
    public class GutenbergReader
    {
        LegacyBBeB m_Book;

        public GutenbergReader(LegacyBBeB book)
        {
            m_Book = book;
        }

        public void ReadFile(String aFileName, BookInfo bookInfo)
        {
            Gutenberg.TextFormatter gbFormatter = new TextFormatter();

            Document doc;
            FileStream stream = File.OpenRead(aFileName);
            try
            {
                doc = gbFormatter.ProcessTextBook(stream);
            }
            finally
            {
                stream.Close();
            }

            if (!string.IsNullOrEmpty(doc.Author))
            {
                bookInfo.Author = doc.Author;
            }
            if (!string.IsNullOrEmpty(doc.Title))
            {
                bookInfo.Title = doc.Title;
            }

            foreach (Chapter chapter in doc.Chapters)
            {
                TextBlockBuilder pageBlock = new TextBlockBuilder(m_Book);

                bool bPrintedPara = false;
                foreach (Paragraph para in chapter.Paragraphs)
                {
                    if (bPrintedPara)
                    {
                        pageBlock.Append(TagId.EOL);
                        pageBlock.Append(TagId.EOL);
                    }

                    foreach (char c in para.Text)
                    {
                        pageBlock.AppendChar(c);
                    }

                    bPrintedPara = true;
                }

                m_Book.AddTextPage(pageBlock.CreateLegacyTextObject());
            }
        }
    }
}
