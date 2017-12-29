using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class TextTag : BBeBTag
	{
		string m_strText;

		public TextTag( string text )
			: base(TagId.Text)
		{
			m_strText = text;
		}

		public string Value
		{
			get { return m_strText; }
			set { m_strText = value; }
		}

		public override string ToString()
		{
			return "Text: " + m_strText;
		}
	}
}
