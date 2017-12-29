using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using BBeBLib.Serializer;


namespace BBeBLib
{
	public class PageObject : BBeBObject
	{
		List<BBeBObject> m_lstChildren = new List<BBeBObject>();
		BBeBObject m_InfoObj = null;
		BBeBObject m_LinkObj = null;
		List<BBeBTag> m_StreamTags = new List<BBeBTag>();

		public PageObject(ushort id) : base(id)
		{
            Type = ObjectType.Page;
		}

		public List<BBeBObject> Children
		{
			get { return m_lstChildren; }
			set { m_lstChildren = value; }
		}

		public BBeBObject InfoObj
		{
			get { return m_InfoObj; }
			set { m_InfoObj = value; }
		}

		public BBeBObject LinkObj
		{
			get { return m_LinkObj; }
			set { m_LinkObj = value; }
		}

		public List<BBeBTag> StreamTags
		{
			get { return m_StreamTags; }
			set { m_StreamTags = value; }
		}

		public void Serialize()
		{
			if (InfoObj != null)
			{
				Tags.Add(new UInt32Tag(TagId.ObjectInfoLink, InfoObj.ID));
			}

			if (LinkObj != null)
			{
				Tags.Add(new UInt32Tag(TagId.Link, LinkObj.ID));
			}

			uint[] ids = new uint[m_lstChildren.Count];
			int i = 0;
			foreach (BBeBObject c in m_lstChildren)
				ids[i++] = c.ID;
			Tags.Add(new UInt32ArrayTag(TagId.PageObjectIds, ids));

			StreamTagGroup streamTags = new StreamTagGroup();
			MemoryStream stream = new MemoryStream();
			BBeBinaryWriter writer = new BBeBinaryWriter(stream);

			BBeBSerializer.SerializeTags(m_StreamTags, writer);
			writer.Flush();

			byte[] data = new byte[writer.Position];
			Array.Copy(stream.GetBuffer(), data, writer.Position);
			streamTags.Data = data;

			Tags.Add(streamTags);
		}

		public override void WriteDebugInfo(TextWriter writer)
		{
			writer.WriteLine("{0}/{1}, ID={2}", this.GetType().Name, Type.ToString(), ID);

			writer.WriteLine(ToString());
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("   Info: {0}", m_InfoObj == null ? "<null>" : (m_InfoObj.GetType().ToString() + ':' + m_InfoObj.ID));
			sb.AppendLine();

			sb.AppendFormat("   Link: {0}", m_LinkObj == null ? "<null>" : (m_LinkObj.GetType().ToString() + ':' + m_LinkObj.ID));
			sb.AppendLine();

			sb.AppendLine("   Children:");
			int idx = 0;
			foreach (BBeBObject obj in m_lstChildren)
			{
				sb.AppendFormat("     [{0}]: {1}:{2}", idx++, obj.GetType().ToString(), obj.ID);
				sb.AppendLine();
			}
			
			sb.AppendLine("   Tags:");
			idx = 0;
			foreach (BBeBTag tag in m_StreamTags)
			{
				sb.AppendFormat("     [{0}]: {1}", idx++, tag.ToString());
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
