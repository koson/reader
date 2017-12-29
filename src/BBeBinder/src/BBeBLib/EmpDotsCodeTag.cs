using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class EmpDotsCodeTag : BBeBTag
	{
		uint m_nVal;
		StringTag m_FontFace;
		ushort m_DotsCode;

		public EmpDotsCodeTag(BBeBLib.TagId eId) : base(eId)
		{
		}

		/// <summary>
		/// No idea what this is. So far all books have had a value of zero.
		/// </summary>
		public uint Value
		{
			get { return m_nVal; }
			set { m_nVal = value; }
		}

		public StringTag FontFace
		{
			get { return m_FontFace; }
			set { m_FontFace = value; }
		}

		public ushort DotsCode
		{
			get { return m_DotsCode; }
			set { m_DotsCode = value; }
		}

		public override string ToString()
		{
			StringBuilder ret = new StringBuilder( base.ToString() );
            ret.AppendFormat("  Value={0} FontFace={1}, DotsCode={2}", Value, FontFace, DotsCode);

            return ret.ToString();
		}
	}
}
