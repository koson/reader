using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class UInt16Tag : BBeBTag
	{
		ushort m_wValue;

        public ushort Value
        {
            get { return m_wValue; }
            set { m_wValue = value; }
        }

        
        public UInt16Tag(BBeBLib.TagId eId, ushort wValue)
			: base(eId)
		{
			m_wValue = wValue;
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder( base.ToString() );
            ret.Append(" : " + m_wValue);
            return ret.ToString();
        }
	}
}
