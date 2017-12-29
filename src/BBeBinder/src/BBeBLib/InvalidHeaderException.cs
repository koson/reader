using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class InvalidHeaderException : ApplicationException
	{
		public InvalidHeaderException(string msg)
			: base(msg)
		{
		}
	}
}
