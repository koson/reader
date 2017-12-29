using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class UInt32ArrayTag : BBeBTag
	{
		uint[] m_TagData;

        public uint[] Value
        {
            get { return m_TagData; }
        }

		public UInt32ArrayTag(BBeBLib.TagId eId, uint[] tagData)
			: base(eId)
		{
			m_TagData = tagData;
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder(base.ToString());
            ret.Append(" :");
            foreach (uint val in m_TagData)
                ret.Append(" " + val);
            return ret.ToString();
        }
    }
}
