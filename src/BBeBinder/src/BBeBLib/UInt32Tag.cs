using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class UInt32Tag : BBeBTag
	{
		uint m_dwValue;

        public uint Value
        {
            get { return m_dwValue; }
            set { m_dwValue = value; }
        }
        
        public UInt32Tag(BBeBLib.TagId eId, uint dwValue)
			: base(eId)
		{
			m_dwValue = dwValue;
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder(base.ToString());
            ret.Append(" : " + m_dwValue);
            return ret.ToString();
        }

	}
}
