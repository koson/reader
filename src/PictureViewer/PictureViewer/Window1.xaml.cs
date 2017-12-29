using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;

namespace PictureViewer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class Window1 : System.Windows.Window
    {
        public Window1()
        {
            InitializeComponent();  
        }
    
        public void OnSelctionChanged(Object source, RoutedEventArgs args)
        {
            ListBox lb = args.Source as ListBox;
            string simplestr;
            XmlDataProvider provider = MyListBox.DataContext as XmlDataProvider;
            if (provider != null)
            {
                simplestr = lb.SelectedValue.ToString().Replace(" ", "");
                simplestr = simplestr.Replace(",", "");
                provider.Source = new Uri(@"http://picasaweb.google.com/data/feed/api/user/rohits79/album/" + simplestr  + "?kind=photo");
            }
        }
    }

}