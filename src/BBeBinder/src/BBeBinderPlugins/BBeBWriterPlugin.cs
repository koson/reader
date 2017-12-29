using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using BBeBinderPluginInterface;
using BBeBLib;
using BBeBLib.Serializer;

namespace BBeBinderPlugins
{
    public class BBeBWriterPlugin : BBeBinderOutputPlugin
    {
        IPluginHost myPluginHost = null;

        public BBeBWriterPlugin()
        {
        }

        public PluginType TypeOfPlugin { get { return PluginType.Output; } }

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


        public string Name { get { return "BBeBWriterPlugin"; } }
        public string Description { get { return "Saves BBeB/LFR files to disk"; } }
        public string Author { get { return "Andy Qua"; } }
        public string Version { get { return "1.0"; } }
        public string[] SupportedFiles { get { return new string[] { "BBeB/LRF File" }; } }

        public bool SaveDocument(BindingParams bindingParameters, TocEntry tocEntries, 
									BBeBLib.Serializer.CharacterMapper charMapper, 
									HtmlDocument document)
        {
            bool ret = false;

            string filename = bindingParameters.MetaData.BookInfo.Author + " - " + bindingParameters.MetaData.BookInfo.Title; 

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = filename;
            dialog.Filter = "BBeB/LRF Files (*.lrf)|*.lrf";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Host.Status( "Converting...." );
                Application.DoEvents();
				HTMLToBbebTransform transformer = new HTMLToBbebTransform(charMapper);
				BBeB book = transformer.ParseHTML(document, bindingParameters, tocEntries);

                Host.Status("Saving....");
                Application.DoEvents();

                // Save book now
                File.Delete(dialog.FileName);
                BBeBLib.Serializer.BBeBSerializer serializer = new BBeBLib.Serializer.BBeBSerializer();
                FileStream newLrfStream = File.OpenWrite(dialog.FileName);
                try
                {
                    serializer.Serialize(newLrfStream, book);
                }
                catch (Exception ex)
                {
                    // TODO Need to get these strings from a resource for localization purposes
                    string msg = string.Format("Couldn't save file {0}\n{1}", dialog.FileName, ex.ToString());
                    MessageBox.Show( msg, "Error saving file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    newLrfStream.Close();
                }
                Host.Status("Finished.");
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
