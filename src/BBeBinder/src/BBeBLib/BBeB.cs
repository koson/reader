using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace BBeBLib
{
	/// <summary>
	/// The data for a Broadband eBook (BBeB).
	/// 
	/// This class will eventually replace the one by the same name in
	/// the BBeBLib assembly.
	/// </summary>
	public class BBeB
	{
		BBeBHeader m_Header = new BBeBHeader();
		byte[] m_ThumbnailData = null;
        BookMetaData m_MetaData = new BookMetaData();
		List<BBeBObject> m_Objects = new List<BBeBObject>();


		public static ushort ReaderPageWidth
		{
			get { return 600; }
		}

		public static ushort ReaderPageHeight
		{
			get { return 800; }
		}

		public BBeBHeader Header
		{
			get { return m_Header; }
			set { m_Header = value; }
		}

		public byte[] ThumbnailData
		{
			get { return m_ThumbnailData; }
			set { m_ThumbnailData = value; }
		}

		public BookMetaData MetaData
		{
			get { return m_MetaData; }
			set { m_MetaData = value; }
		}

		public List<BBeBObject> Objects
		{
			get { return m_Objects; }
			set { m_Objects = value; }
		}

		public void WriteDebugInfo(TextWriter writer)
		{
			m_Header.WriteDebugInfo(writer);

			writer.WriteLine("Metadata:");
			writer.WriteLine("======================");

			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			settings.IndentChars = ("   ");
			XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
			XmlSerializer serializer = new XmlSerializer(typeof(BookMetaData));
			serializer.Serialize(xmlWriter, m_MetaData);

			writer.WriteLine();
			writer.WriteLine();

			writer.WriteLine("Objects ({0}):", Objects.Count);
			writer.WriteLine("======================");
			foreach (BBeBObject obj in Objects)
			{
				obj.WriteDebugInfo(writer);
			}
			writer.WriteLine("[/Objects]");
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder( "[Header]\n" );
            ret.Append(m_Header.ToString() + "\n");
            ret.Append("[/Header]\n");
            ret.Append("[MetaData]\n");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("   ");
            XmlWriter writer = XmlWriter.Create(ret, settings);
            XmlSerializer serializer = new XmlSerializer(typeof(BookMetaData));
            serializer.Serialize(writer, m_MetaData);
            writer.Close();

            ret.Append("\n[/MetaData]\n");

            ret.Append("[Objects]\n");
            foreach( BBeBObject obj in Objects )
                ret.Append( obj.ToString() + "\n" );
            ret.Append("[/Objects]\n");

            return ret.ToString();
        }

		/// <summary>
		/// Finds an object given it's ID.
		/// </summary>
		/// <param name="id">The ID of the object</param>
		/// <returns>The found object, or null if object not found.</returns>
		public BBeBObject FindObject(ushort id)
		{
			foreach (BBeBObject obj in m_Objects)
			{
				if (obj.ID == id)
				{
					return obj;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the first instance of an object given the type.
		/// </summary>
		/// <param name="objType">The object type</param>
		/// <returns>The first object whose type matches objType or null if no object found.</returns>
		public BBeBObject FindFirstObject(Type objType)
		{
			foreach (BBeBObject obj in m_Objects)
			{
				if (obj.GetType() == objType)
				{
					return obj;
				}
			}

			return null;
		}
	}
}
