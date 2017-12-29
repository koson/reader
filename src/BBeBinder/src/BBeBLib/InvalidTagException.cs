using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class InvalidTagException : ApplicationException
	{
		ushort m_wVal;

		public InvalidTagException(string msg, ushort wVal)
			: base(msg)
		{
			m_wVal = wVal;
		}

		public ushort Value
		{
			get { return m_wVal; }
		}
	}
}
