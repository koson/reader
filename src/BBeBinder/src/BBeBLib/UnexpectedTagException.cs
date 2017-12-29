using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{

	[global::System.Serializable]
	public class UnexpectedTagException : ApplicationException
	{
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public UnexpectedTagException() { }
		public UnexpectedTagException(string message) : base(message) { }
		public UnexpectedTagException(string message, Exception inner) : base(message, inner) { }
		protected UnexpectedTagException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
