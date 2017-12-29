using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class InvalidBookException : ApplicationException
	{
		public InvalidBookException(string msg)
			: base(msg)
		{
		}
	}
}
