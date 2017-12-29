using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	class UInt16ArrayTag : BBeBTag
	{
		ushort[] m_TagData;

        public ushort[] Value
        {
            get { return m_TagData; }
        }
        
        public UInt16ArrayTag(BBeBLib.TagId eId, ushort[] tagData)
			: base(eId)
		{
			m_TagData = tagData;
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder(base.ToString());
            ret.Append(" :");
            foreach (ushort v in m_TagData)
                ret.Append(" " + v);
            return ret.ToString();
        }
    }
}
