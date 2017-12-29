using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace WpfPdf2Epub
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
	  public string[] Arguments;

    protected override void OnStartup( StartupEventArgs e )
    {
      base.OnStartup( e );
      Arguments = e.Args;
    }
	}
}