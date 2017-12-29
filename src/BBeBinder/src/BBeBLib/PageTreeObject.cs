using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class PageTreeObject : BBeBObject
	{
		List<PageObject> m_Pages = new List<PageObject>();

		public PageTreeObject(ushort id)
			: base(id)
		{
            Type = ObjectType.PageTree;
		}

		public List<PageObject> Pages
		{
			get { return m_Pages; }
			set { m_Pages = value; }
		}

        public void setObjectInfoLink(ushort id)
        {
            Tags.Add(new UInt32Tag(TagId.ObjectInfoLink, id));
        }


		public override string ToString()
		{
			return base.ToString() + string.Format("    Pages: " + m_Pages.Count);
		}
	}
}
