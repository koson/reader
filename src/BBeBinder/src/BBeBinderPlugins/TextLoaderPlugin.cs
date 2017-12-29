using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using BBeBinderPluginInterface;

namespace BBeBinderPlugins
{
    public class TextLoaderPlugin : BBeBinderInputPlugin
    {
        IPluginHost myPluginHost = null;

        public TextLoaderPlugin()
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


        public string Name { get { return "TextLoaderPlugin"; } }
        public string Description { get { return "Loads Text files from disk"; } }
        public string Author { get { return "Andy Qua"; } }
        public string Version { get { return "1.0"; } }

        public string[] SupportedFiles { get { return new string[] { "Text Files" }; } }

        public bool LoadDocument()
        {
            bool ret = false;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text Files (*.txt)|*.TXT";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Host.SetFileDetails(dialog.FileName);

                Gutenberg.TextFormatter formatter = new Gutenberg.TextFormatter();

                FileStream stream = File.OpenRead(dialog.FileName);
                try
                {
                    Gutenberg.Document doc = formatter.ProcessTextBook(stream);

                    Host.SetDocument(doc.SerializeToHtml());
                    ret = true;
                }
                finally
                {
                    stream.Close();
                }
            }

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
