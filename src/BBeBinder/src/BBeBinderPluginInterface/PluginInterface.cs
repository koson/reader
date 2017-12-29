using System;
using System.Windows.Forms;
using BBeBLib;
using BBeBLib.Serializer;

namespace BBeBinderPluginInterface
{
    public enum PluginType { Input, Output };

    public interface BBeBinderPlugin
    {
        PluginType TypeOfPlugin { get; }
        IPluginHost Host { get;set;}
        string Name { get;}
        string Description { get;}
        string Author { get;}
        string Version { get;}
        string[] SupportedFiles { get; }

        // Init and cleanup
        void Initialize();
        void Dispose();
    }

    public interface BBeBinderInputPlugin : BBeBinderPlugin
	{
        bool LoadDocument();
	}

    public interface BBeBinderOutputPlugin : BBeBinderPlugin
    {
        bool SaveDocument(BindingParams bindingParameters, TocEntry tocEntries, 
			BBeBLib.Serializer.CharacterMapper charMapper, HtmlDocument document);
    }

    public interface IPluginHost
    {
        void SetFileDetails(string filename);
        void SetDocumentURI(string URI);
        void SetDocument(string htmlData);
        void SetDocument(System.IO.Stream stream);
        void Status(string statusMsg);
    }

}
