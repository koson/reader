using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
    class HeaderObject : BBeBObject
    {
        public HeaderObject(ushort id)
			: base(id)
		{
            Type = ObjectType.Header;
		}
    }
}
