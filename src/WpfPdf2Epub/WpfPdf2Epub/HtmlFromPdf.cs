﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfPdf2Epub
{
  static class HtmlFromPdf
  {
    public static string CreateHtml( string pdfFile )
    {
      string workPath;
      BuidEpubOutputDir( pdfFile, out workPath );
      string appDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
      string hmltFilename = CallPdfToHtml( appDir + "\\pdftohtml.exe", pdfFile, workPath );
      return hmltFilename;
    }

    private static void BuidEpubOutputDir( string pdfFile, out string workPath )
    {
      string workingDir = GetWorkingDir( pdfFile );
      string baseName = Path.GetFileNameWithoutExtension( pdfFile );
      workPath = BuildSubDirName( workingDir, baseName );
      Directory.CreateDirectory( workPath );
      workPath += "\\OEBPS";
      Directory.CreateDirectory( workPath );
    }

    private static string GetWorkingDir( string fileName )
    {
      string fullPath = Path.GetFullPath( fileName );
      return Path.GetDirectoryName( fullPath );

    }

    private static string BuildSubDirName( string dir, string name )
    {
      string subDir = Path.Combine( dir, name );
      for ( int number = 0; ( Directory.Exists( subDir )) && (number < 20000); number++ )
      {
        subDir = Path.Combine( dir, string.Format("{0}({1})",name, number) );  
      }
      if ( Directory.Exists( subDir ) )
      {
        throw new ApplicationException("Unable to find new subdirectory for output!");
      }
      return subDir;
    }

    private static string CallPdfToHtml( string app, string pdfFile, string outputPath )
    {
      string baseFilename = Path.GetFileNameWithoutExtension( pdfFile );
      string html = Path.Combine( outputPath, baseFilename ); 
      Process scriptProc = new Process();
      scriptProc.StartInfo.FileName = app;
      scriptProc.StartInfo.Arguments = string.Format( "-p \"{0}\" \"{1}\"", pdfFile, html );
      scriptProc.Start();
      scriptProc.WaitForExit();
      scriptProc.Close();
      return html+ "s.html";
    }

  
  }
}
