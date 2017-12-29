using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenFileDialog=Microsoft.Win32.OpenFileDialog;
using Path=System.IO.Path;

namespace WpfPdf2Epub
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
	  public ConversionOptions Options;

		public MainWindow()
		{
		  App applic = (App)(App.Current);
		  String source = string.Empty;
		  string dest = string.Empty;

		  if ( applic.Arguments.Length > 0 )
		  {
		    source = applic.Arguments[ 0 ];
		    dest = Path.ChangeExtension( source, ".epub" );
		  }
			this.InitializeComponent();
		  Options = new ConversionOptions()
		            {
                  SourceFilename = source,
                  DestinationFilename = dest,
                  MaxSplitSizeInBytes = 100 * 1024,
		              StripHeader = new Pattern()
		                            {
		                              Enable = true,
		                              LineCount = 2,
                                  RegEx = EncodeString("(<b>[IVX0-9]*?</b><br>\r\n[^<]*?<br>\r\n)|([^<]*?<br>\r\n<b>[IVX0-9]*?</b><br>\r\n)")
		                            },
		              StripFooter = new Pattern()
		                            {
		                              Enable = true,
		                              LineCount = 1,
                                  RegEx = EncodeString( "<b>[IVX0-9]*?</b><br>" )
		                            }
		            };
		  DataContext = Options;
    }

    private string EncodeString( string s )
    {
      s = s.Replace( "\n", "\\n" );
      s = s.Replace( "\r", "\\r" );
      s = s.Replace( "\t", "\\t" );
      return s;
    }
    private string DecodeString( string s )
    {
      s = s.Replace( "\n", "" );
      s = s.Replace( "\r", "" );
      s = s.Replace( "\t", "" );
      s = s.Replace( "\\n", "\n" );
      s = s.Replace( "\\r", "\r" );
      s = s.Replace( "\\t", "\t" );
      return s;
    }

    private void ButtonSource_Click( object sender, RoutedEventArgs e )
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "Portable Document Files|*.pdf";
        bool? result = dialog.ShowDialog();
        if ( result == true )
        {
          Options.SourceFilename = dialog.FileName;
          Options.DestinationFilename = Path.ChangeExtension( Options.SourceFilename, ".epub" );
        }
    }

    private void ButtonDestination_Click( object sender, RoutedEventArgs e )
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Filter = "Ebook Files|*.epub";
      bool? result = dialog.ShowDialog();
      if ( result == true )
      {
        Options.DestinationFilename = dialog.FileName;
      }
    }


    private void cbStripHeader_Checked( object sender, RoutedEventArgs e )
    {
      GroupHeader.IsEnabled = true;
    }

    private void cbStripHeader_Unchecked( object sender, RoutedEventArgs e )
    {
      GroupHeader.IsEnabled = false;
    }

    private void cbStripFooter_Checked( object sender, RoutedEventArgs e )
    {
      GroupFooter.IsEnabled = true;
    }

    private void cbStripFooter_Unchecked( object sender, RoutedEventArgs e )
    {
      GroupFooter.IsEnabled = false;
    }

    private void btCancel_Click( object sender, RoutedEventArgs e )
    {
        Window.Close();
    }

    private void btOk_Click( object sender, RoutedEventArgs e )
    {
      Options.StripHeader.RegEx = DecodeString( Options.StripHeader.RegEx );
      Options.StripFooter.RegEx = DecodeString( Options.StripFooter.RegEx );

      Options.HtmlFilename = HtmlFromPdf.CreateHtml( Options.SourceFilename );
      string workingDir = GetWorkingDir( Options.HtmlFilename );
 
      HtmlSplitter splitter = new HtmlSplitter();
      splitter.SplitFile( Options );

      MetaData metaData = new MetaData();
      string projectName = GetProjectName( Options.SourceFilename );
      metaData.Parse( Path.Combine( workingDir, projectName + ".html" ) );

      IDictionary<string, string> parameters = Meta2Parameters( metaData );
      AddCoverToParameters( parameters, workingDir );

      string rootDir = Path.GetDirectoryName( workingDir );
      TemplateFiles.AddAllTemplateFiles( workingDir, metaData, projectName, parameters );

      List<string> files = ZipModule.ScanFiles( rootDir );
      ZipModule.BuildZipFile( Options.DestinationFilename, files, rootDir );
      Window.Close();
    }

    private static void AddCoverToParameters( IDictionary<string, string> dictionary, string workingDirectory )
    {
      string[] filenames = Directory.GetFiles( workingDirectory, "*-1_1.*" );
      if ( filenames.Length == 1 )
      {
        string imageTag = string.Format( "<img src=\"{0}\" alt=\"cover\" style=\"height: 100%\"/>", Path.GetFileName( filenames[ 0 ] ) );
        dictionary.Add( "##Cover##", imageTag );
      }
      else
      {
        string msg = "            <div id=\"titlepage\">\n" +
                    "               <h1 class=\"part-title\">{0}</h1>\n" +
                    "               <h3 class=\"title-break\">***</h3>\n" +
                    "               <h3 class=\"author\">{1}</h3>\n" +
                    "            </div>";
        string cover = string.Format( msg, dictionary[ "##Title##" ], dictionary[ "##Author##" ] );
        dictionary.Add( "##Cover##", cover );
      }
    }

    private static IDictionary<string, string> Meta2Parameters( MetaData data )
    {
      IDictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add( "##Author##", data.Author );
      dictionary.Add( "##Title##", data.Title );
      dictionary.Add( "##Date##", data.Date );
      dictionary.Add( "##UUID##", Guid.NewGuid().ToString() );
      return dictionary;
    }

    private static string GetWorkingDir( string fileName )
    {
      string fullPath = Path.GetFullPath( fileName );
      return Path.GetDirectoryName( fullPath );
    }

    private static string GetProjectName( string file )
    {
      return Path.GetFileNameWithoutExtension( file );
    }

	}
}