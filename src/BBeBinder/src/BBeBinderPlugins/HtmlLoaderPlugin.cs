using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using BBeBinderPluginInterface;

namespace BBeBinderPlugins
{
    public class HtmlLoaderPlugin : BBeBinderInputPlugin
    {
        IPluginHost myPluginHost = null;

        public HtmlLoaderPlugin()
        {
        }

        public PluginType TypeOfPlugin { get { return PluginType.Input; } }

        public IPluginHost Host
        {
            get
            {
                return myPluginHost;
            }
            set
            {
                myPluginHost = value;
            }
        }


        public string Name { get { return "HTMLLoaderPlugin"; } }
        public string Description { get { return "Loads HTML files from disk or url"; } }
        public string Author { get { return "Andy Qua"; } }
        public string Version { get { return "1.0"; } }

        public string[] SupportedFiles { get { return new string[] { "HTML Files" }; } }

        public bool LoadDocument()
        {
            bool ret = false;

            InputBox box = InputBox.Show("Enter URL", "Enter URL to Load", "http://");
            if (box.ButtonClicked != InputBox.Buttons.CANCEL)
            {
                string url = box.getReturnValue;
                Host.SetFileDetails(url);

                if (!url.Equals(""))
                {
                    Host.SetDocumentURI(url);
                    ret = true;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            return ret;
        }

        // Init and cleanup
        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

    }
}
