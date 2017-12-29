using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using BBeBinderPluginInterface;

namespace BBeBinderPlugins
{
    public class BBeBLoaderPlugin : BBeBinderInputPlugin
    {
        IPluginHost myPluginHost = null;

        public BBeBLoaderPlugin()
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


        public string Name { get { return "BBeBLoaderPlugin"; } }
        public string Description { get { return "Loads BBeB/LFR files from disk"; } }
        public string Author { get { return "Andy Qua"; } }
        public string Version { get { return "1.0"; } }

        public string[] SupportedFiles { get { return new string[] { "BBeB/LRF Files" }; } }

        public bool LoadDocument()
        {
            bool ret = false;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "BBeB/LRF Files (*.lrf)|*.LRF";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Host.SetFileDetails(dialog.FileName);
                FileStream stream = File.OpenRead(dialog.FileName);
                BBeBLib.BBeB book;
                try
                {
                    BBeBLib.Serializer.BBeBSerializer serializer = new BBeBLib.Serializer.BBeBSerializer();
                    book = serializer.Deserialize(stream);
                    //Debug.WriteLine(book.ToString());

                    BBeBLib.Serializer.HTMLWriter convertor = new BBeBLib.Serializer.HTMLWriter(book);
                    if (!convertor.HasImages)
                        Host.SetDocument(convertor.Data);
                    else
                    {
                        MessageBox.Show("This book contains images.\nYou will be asked to select a location to save the converted html and images to.", "Book contains Images", MessageBoxButtons.OK);
                        // HTML file has images which means we need to save the file and
                        // Associated images somewhere and then load it from there.
                        string filename = dialog.FileName;
                        FileInfo fi = new FileInfo(filename);
                        filename = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + ".html";

                        SaveFileDialog sdialog = new SaveFileDialog();
                        sdialog.FileName = filename;
                        sdialog.Title = "Save converted HTML and Images";
                        if (sdialog.ShowDialog() == DialogResult.OK)
                        {
                            // save html file and associated images
                            string path = sdialog.FileName;
                            filename = path.Substring(path.LastIndexOf("\\") + 1);
                            path = path.Substring(0, path.LastIndexOf("\\"));
                            convertor.save(path, filename);
                            Host.SetDocumentURI(sdialog.FileName);
                        }
                    }
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
