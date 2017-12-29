using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class StringTag : BBeBTag
	{
		string m_strVal;

		public StringTag(BBeBLib.TagId eId, string val)
			: base(eId)
		{
			m_strVal = val;
		}

		public string Value
		{
			get { return m_strVal; }
			set { m_strVal = value; }
		}

		public override string ToString()
		{
			return m_strVal;
		}
	}
}
