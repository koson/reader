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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using BBeBLib;

namespace makebbeb
{
    class Program
    {
        static void ConvertLrsFile(string inputLrs, string outputLrf)
        {
            if (!File.Exists(inputLrs))
            {
                throw new FileNotFoundException("Input file not found", inputLrs);
            }

#if false
            XylogParser.MSXML_InitInstance();
            try
            {
                XylogParser.XYP_Create();
                try
                {
                    int ver = XylogParser.XYP_GetVersion();

                    FileStream logStream = File.OpenWrite("BBeBinder.log");
                    XylogParser.XYP_SetOutputFD(logStream.SafeFileHandle.DangerousGetHandle().ToInt32());

                    XylogParser.XYP_SetLrfFileName(outputLrf);

//                    XylogParser.XYP_SetTextCompressionFlag(1);
//                    XylogParser.XYP_SetScrambleNumber(0xFE00);
                    XylogParser.XYP_SetProducerName("BBeBBinder");
                    XylogParser.XYP_ParseDocument(inputLrs);
                }
                finally
                {
                    XylogParser.XYP_Release();
                }
            }
            finally
            {
                XylogParser.MSXML_Release();
            }
#endif
        }


        static void BindBook(BindingParams config)
        {
            LegacyBBeB book = new LegacyBBeB(config.MetaData);
            
            book.BeginBook();

            FileInfo bookFileInfo = new FileInfo(config.InputFile);

            if (string.Compare(bookFileInfo.Extension, ".html", true) == 0 ||
                string.Compare(bookFileInfo.Extension, ".htm", true) == 0)
            {
                throw new ApplicationException("Can't parse HTML yet");
            }
            else if (string.Compare(bookFileInfo.Extension, ".pdf", true) == 0)
            {
                throw new ApplicationException("Can't parse a PDF yet");
            }
            else if (string.Compare(bookFileInfo.Extension, ".xml", true) == 0)
            {
                GutenbergReader reader = new GutenbergReader(book);

                reader.ReadFile(config.InputFile, config.MetaData.BookInfo);
            }
            else if (string.Compare(bookFileInfo.Extension, ".txt", true) == 0)
            {
                PseudoHtmlReader reader = new PseudoHtmlReader(book);

                reader.ReadFile(config.InputFile);
            }
            else
            {
                throw new ApplicationException("Can't handle file type: " + config.InputFile);
            }

            book.FinalizeBook(config.IconFile);

            book.WriteToFile(config.OutputFile);
        }


        static void BindBook(string strInputFile)
        {
            FileInfo finfo = new FileInfo(strInputFile);

            if (finfo.Extension == ".xml")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BindingParams));
                FileStream stream = File.OpenRead(strInputFile);
                try
                {
                    BindingParams config = (BindingParams)serializer.Deserialize(stream);
                    config.MetaData.Fixup();

                    BindBook(config);
                }
                finally
                {
                    stream.Close();
                }
            }
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    BindBook(args[0]);
                }
                else
                {
                    Console.Error.WriteLine("Need a file to process");
                    Environment.ExitCode = 1;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Exception: " + ex.Message);
                Environment.ExitCode = 2;
            }
        }
    }
}
