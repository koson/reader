using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using BBeBinderPluginInterface;

namespace BBeBinderPlugins
{
    class RTF2HTML
    {
        IPluginHost host;
        public RTF2HTML(IPluginHost host)
        {
            this.host = host;

        }

        public string convert( string filename, bool par_is_p, bool no_small_footnotes)
        {
			host.Status( "Reading rtf..." );
            string input;
            using ( StreamReader sr = new StreamReader( filename ) )
            {
                input = sr.ReadToEnd();
            }

			host.Status( "rtf2html prep" );
            input = rtfToHtmlPrep( input );

			host.Status( "correcting images" );
            input = rtfimagesCorrect( input );

			host.Status( "handling images" );
            input = rtfImages( input );

			host.Status( "rtf to html" );
            input = rtfToHtml( input );

			host.Status( "parsing html" );
            HtmlParser parser = new HtmlParser(par_is_p, no_small_footnotes);
            input = parser.go(input);
            parser = null;

			host.Status( "doing small meta changes" );
            input = smallMeta( input );

			host.Status( "writing html" );
            string ret = null;
            using (StringWriter sw = new StringWriter())
            {
                sw.WriteLine( "<!DOCTYPE html public \"-//w3c//dtd html 4.0 transitional//cs\">" );
                sw.WriteLine( "<html>" );
                sw.WriteLine( "<head>" );
                sw.WriteLine( "<meta HTTP-Equiv=\"Content-Type\" CONTENT=\"text/html; charset=iso-8859-2\">" );
                sw.WriteLine( "</head>" );
                sw.WriteLine( "<body text=\"#000000\" bgcolor=\"#FFFFFF\">" );
                sw.WriteLine( "<font face=\"Verdana, Helvetica CE, Arial CE, Helvetica, Arial\">" );
                sw.WriteLine( "<font size=\"2\">" );

                sw.WriteLine( input );

                sw.WriteLine( "</body>" );
                sw.WriteLine( "</html>" );

                ret = sw.ToString();
            }

            return ret;
        }

        string rtfToHtmlPrep(string input)
        {
            string output = input;
            int pos = input.IndexOf("\\fonttbl");
            if (pos != -1)
            {
                int pos2 = input.IndexOf("\\info", pos);
				if ( pos2 == -1 )
					pos2 = 0;

                output = input.Substring(0, pos) + input.Substring(pos2, input.Length - pos2);
            }

            input = output;
            pos = input.IndexOf("\\par");
            if (pos != -1)
            {
                int pos2 = input.IndexOf(";}", pos);
                if ( pos2 == -1 )
                    pos2 = 0;
                char c = (char)2;
                output = input.Substring(0, pos2) + "<" + c + "!--BACKSLASHSEMICOLON-->" + c +
                         input.Substring(pos2 + 1, input.Length - pos2 - 1);
            }

            return output;
        }

        string rtfimagesCorrect(string input)
        {
            string[] pattern = new string[]{ "(\\\\bliptag[0-9]*)",
                                             "(\\\\blipuid[^}]*})" };
        
            string[] replace = new string[]{ "\\\\pict ",
                                             " " };

            for ( int i = 0 ; i < pattern.Length ; ++i )
            {
                input = Regex.Replace(input, pattern[i], replace[i] );
            }

            return input;
        }

        string rtfImages(string input)
        {
            int rtfimages = 0;
            
            int xmaxrtf = input.Length + 1000;;

            int rtfimage = rtfPictureSearch( input, xmaxrtf );
            while ( rtfimage > 0 ) 
            {
                rtfimages++;
                int rtfbin = input.IndexOf( "\\bin", rtfimage);
                int rtfbrace = input.IndexOf( "}", rtfimage );

                if ( rtfbrace == -1 )
                    rtfbrace = rtfbin+1;
                if ( rtfbin == -1 )
                    rtfbin = rtfbrace+1;

                if ( rtfbin < rtfbrace )
                {
                    int rtfbinspace = input.IndexOf( " ", rtfbin );
                    string rtfbinlength = input.Substring( rtfbin+4, rtfbinspace -rtfbin-4 );    
                    input = input.Substring( 0, rtfimage-1 ) + 
                             input.Substring( rtfbinspace + 1 );
                } 
                else 
                {
                    input = input.Substring( 0, rtfimage-1 ) + 
                        input.Substring( rtfbrace );
                }

                rtfimage = rtfPictureSearch( input, xmaxrtf);
            }

            return input;
        }

        int rtfPictureSearch( string input, int xmaxrtf )
        {
            int rtfimage1 = input.IndexOf( "\\pict " );
            int rtfimage2 = input.IndexOf( "\\pict\\" );
            int rtfimage3 = input.IndexOf( "\\pict{" );

            if ( rtfimage1 == -1 )
                rtfimage1 = xmaxrtf;
            if ( rtfimage2 == -1 )
                rtfimage2 = xmaxrtf;
            if ( rtfimage3 == -1 )
                rtfimage3 = xmaxrtf;

            int r = Math.Min( rtfimage1, Math.Min( rtfimage2, rtfimage3 ) );

            if ( r == xmaxrtf )
                r = 0;
				
            return r;
        }

        string rtfToHtml(string input)
        {
            for (int i = 0; i < XslPatterns.rtf2HtmlPatterns.Length; ++i)
            {
                host.Status("Parsing XSL pattern " + (i + 1) + "/" + XslPatterns.rtf2HtmlPatterns.Length);
                input = Regex.Replace(input, XslPatterns.rtf2HtmlPatterns[i], XslPatterns.rtf2HtmlPeplaces[i]);
            }

            string output = input;
            do 
            {
                input = output;
                output = Regex.Replace( input, 
                                        "(<a href=\"([a-zA-Z]*)://([^&]*)&amp;([^\"]*))",
                                        "<a href=\"$2://$3" + ((char)2) + "$4");
            } while ( !input.Equals( output ) );

            input = Regex.Replace(input, "(" + ((char)2) + ")", "&");
            return input;
        }

		string smallMeta( string input )
		{
			string[] pattern = new string[] { "(&AMP;)","(&GT;)","(&LT;)","(&NBSP;)" };
			string[] replace = new string[] { "&amp;","&gt;","&lt;","&nbsp;" };
			for ( int i = 0 ; i < pattern.Length ; ++i )
			{
				input = Regex.Replace(input, pattern[i], replace[i] );
			}
			return input;
		}
    }
}
