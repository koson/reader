/* 
 * Copyright (C) 2006, Chris Mumford cmumford@cmumford.com
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
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Text.RegularExpressions;


namespace Gutenberg
{
    /// <summary>
    /// Convert a text stream to HTML.
    /// </summary>
    public class TextFormatter
    {
        Regex m_regChapterNumber = new Regex("^chapter\\s+(?<number>\\w+)$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Regex m_regTitle = new Regex("^title\\s*:\\s+(?<title>\\w.+)$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
        Regex m_regAuthor = new Regex("^author\\s*:\\s+(?<author>\\w.+)$",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);
        
        public void SerializeAsXml(Document doc, FileStream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Document));
            serializer.Serialize(stream, doc);
        }

        public void SerializeAsHtml(Document doc, FileStream stream)
        {   
        }

        private string[] ReadAllLines(Stream input)
        {
            char[] trimchars = { ' ', '\t' };

            StreamReader reader = new StreamReader(input);

            List<string> allLines = new List<string>();

            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    allLines.Add(line.TrimEnd(trimchars));
                }
            }
            finally
            {
                reader.Close();
            }

            return allLines.ToArray();
        }

        private string GetTitle(ref string[] lines)
        {
            int max = Math.Min(1000, lines.Length);
            for (int i = 0; i < max; i++)
            {
                Match m = m_regTitle.Match(lines[i]);
                if (m.Success)
                {
                    return m.Result("${title}");
                }
            }
            return null;
        }

        private string GetAuthor(ref string[] lines)
        {
            int max = Math.Min(1000, lines.Length);
            for (int i = 0; i < max; i++)
            {
                Match m = m_regAuthor.Match(lines[i]);
                if (m.Success)
                {
                    return m.Result("${author}");
                }
            }
            return null;
        }

        public Document ProcessTextBook(Stream input)
        {
            string[] lines = ReadAllLines(input);

            Document doc = new Document();
            Chapter chapter = new Chapter("Forward");
			chapter.Prefix = string.Empty;
            doc.Chapters.Add(chapter);

            doc.Author = GetAuthor(ref lines);
            doc.Title = GetTitle(ref lines);

            StringBuilder paraText = new StringBuilder();
            for (int i = 0; i < lines.Length; i++)
            {
                Match m = m_regChapterNumber.Match(lines[i]);
                if (m.Success)
                {
                    chapter = new Chapter(m.Result("${number}"));
                    doc.Chapters.Add(chapter);

                    if (i < lines.Length - 3)
                    {
                        if ( lines[i+1] != string.Empty && 
                            lines[i+2] == string.Empty ) {
                            // Next line has chapter title, but following line is empty.
                            chapter.SubHeading = lines[i + 1];
                            i += 2;
                        }
                        else if (lines[i + 1] == string.Empty && 
                            lines[i + 2] != string.Empty && 
                            lines[i + 3] == string.Empty)
                        {
                            // separated by one line
                            chapter.SubHeading = lines[i + 2];
                            i += 3;
                        }
                    }
                }
                else
                {
                    if (lines[i] != string.Empty)
                    {
                        if (paraText.Length > 0)
                        {
                            paraText.Append(" ");
                        }
                        paraText.Append(lines[i]);
                    }
                    else
                    {
                        if (paraText.Length > 0)
                        {
                            Paragraph para = new Paragraph();
                            para.Text = paraText.ToString();
                            chapter.Paragraphs.Add(para);
                            paraText = new StringBuilder();
                        }
                    }
                }
            }

            if (paraText.Length > 0)
            {
                Paragraph para = new Paragraph();
                para.Text = paraText.ToString();
                chapter.Paragraphs.Add(para);
            }

            return doc;
        }

        public void Convert(string input, string output)
        {
            File.Delete(output);

            Document doc = ProcessTextBook(File.OpenRead(input));

            FileInfo finfo = new FileInfo(output);
            if (string.Compare(finfo.Extension, ".xml", true) == 0)
            {
                SerializeAsXml(doc, File.OpenWrite(output));
            }
            else if (string.Compare(finfo.Extension, ".htm", true) == 0)
            {
                SerializeAsHtml(doc, File.OpenWrite(output));
            }
            else {
                throw new ApplicationException("Unknown output extension: " + output);
            }
        }
    }
}
