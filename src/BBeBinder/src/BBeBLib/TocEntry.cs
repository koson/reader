using System;
using System.Collections.Generic;
using System.Text;


namespace BBeBLib
{
    /// <summary>
    /// A table of contents entry.
    /// </summary>
    public class TocEntry
    {
        string m_strTitle;
        List<TocEntry> m_Children = new List<TocEntry>();
		object m_Tag = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="title">The TOC entry title.</param>
		public TocEntry(string title)
		{
			m_strTitle = title;
		}

		/// <summary>
		/// The sub TOC items.
		/// </summary>
        public List<TocEntry> Children
        {
            get { return m_Children; }
            set { m_Children = value; }
        }

		/// <summary>
		/// The text for this entry.
		/// </summary>
		public string Title
		{
			get { return m_strTitle; }
			set { m_strTitle = value; }
		}

		/// <summary>
		/// A user defined object that can be associated with this TocEntry.
		/// </summary>
		public object Tag
		{
			get { return m_Tag; }
			set { m_Tag = value; }
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() + m_strTitle.GetHashCode() + m_Children.Count.GetHashCode();
		}

		/// <summary>
		/// Determines whether the specified Object is equal to the current Object.
		/// This routine will do a "deep" comparison which includes all of the oejbect children.
		/// </summary>
		/// <param name="obj">The Object to compare with this Object.</param>
		/// <returns>true if they are equal, false if not.</returns>
		public override bool Equals(object obj)
		{
			TocEntry rhs = (TocEntry)obj;

			if (this.m_strTitle != rhs.Title)
			{
				return false;
			}

#if false
			if (this.m_Tag != rhs.m_Tag)
			{
				return false;
			}
#endif
			if (this.m_Children.Count != rhs.m_Children.Count)
			{
				return false;
			}

			for (int i = 0; i < m_Children.Count; i++)
			{
				if (!this.m_Children[i].Equals(rhs.m_Children[i]))
				{
					return false;
				}
			}

			return true;
		}
    }
}
