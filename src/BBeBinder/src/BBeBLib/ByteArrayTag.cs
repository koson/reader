using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class ByteArrayTag : BBeBTag
	{
		byte[] m_byTagData;

        public byte[] Value
        {
            get { return m_byTagData; }
        }

		public ByteArrayTag(BBeBLib.TagId eId, byte[] byTagData)
			: base(eId)
		{
			m_byTagData = byTagData;
		}

		public static void FormatByteArray(StringBuilder sb, byte[] data)
		{
			int len = Math.Min(50, data.Length);

			sb.AppendFormat("({0} of {1} B):", len, data.Length);

			for (int i = 0; i < len; i++)
			{
				sb.Append(" 0x" + data[i].ToString("x2"));
			}
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder(base.ToString());

			ret.Append(' ');
			FormatByteArray(ret, m_byTagData);
			
			return ret.ToString();
        }
    }
}
