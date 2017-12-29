using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using BBeBinderPluginInterface;

namespace BBeBinderPlugins
{
    public class RTFLoaderPlugin : BBeBinderInputPlugin
    {
        IPluginHost myPluginHost = null;

        public RTFLoaderPlugin()
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


        public string Name { get { return "RTFLoaderPlugin"; } }
        public string Description { get { return "Loads RTF files from disk"; } }
        public string Author { get { return "Andy Qua"; } }
        public string Version { get { return "1.0"; } }

        public string[] SupportedFiles { get { return new string[] { "RTF Files" }; } }

        public bool LoadDocument()
        {
            bool ret = false;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "RTF Files (*.rtf)|*.rtf";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Blank the current document otherwise if you have a large document 
                // already loaded then it takes frickin ages and memory usage goes
                // through the roof!
                Host.SetDocumentURI("about:blank");
                Application.DoEvents();

                RTF2HTML convertor = new RTF2HTML(Host);
                string html = convertor.convert( dialog.FileName, false, false);
                convertor = null;

                using (StreamWriter sw = new StreamWriter("tmp.html"))
                {
                    sw.WriteLine(html);
                }

                //do a quick GC 
                GC.Collect();

                Host.SetDocument(html);
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
