using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
    class MessageTag : BBeBTag
    {
        public ushort parameters;
        public string param1;
        public string param2;

        public MessageTag(BBeBLib.TagId eId, ushort p, string s1, string s2)
			: base(eId)
		{
            parameters = p;
            param1 = s1;
            param2 = s2;
		}

		public override string ToString()
		{
			return base.ToString();
		}

    }
}
