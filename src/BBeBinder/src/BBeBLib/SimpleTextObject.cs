using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class SimpleTextObject : BBeBObject
	{
		public SimpleTextObject(ushort id)
			: base(id)
		{
            Type = ObjectType.SimpleText;
		}
	}
}
