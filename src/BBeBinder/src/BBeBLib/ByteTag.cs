using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class ByteTag : BBeBTag
	{
		byte m_byValue;

		public ByteTag(BBeBLib.TagId eId, byte byValue) : base(eId)
		{
			m_byValue = byValue;
		}

        public byte Value
        {
            get { return m_byValue; }
        }


        public override string ToString()
        {
            StringBuilder ret = new StringBuilder(base.ToString());
            ret.Append(" : " + m_byValue);
            return ret.ToString();
        }

	}
}
