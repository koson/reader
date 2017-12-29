using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class BBeBTag
	{
		BBeBLib.TagId m_eTagId;

		public BBeBTag(BBeBLib.TagId eId)
		{
			m_eTagId = eId;
		}

		public BBeBLib.TagId Id
		{
			get { return m_eTagId; }
			set { m_eTagId = value; }
		}

		public virtual void WriteDebugInfo(TextWriter writer)
		{
			writer.WriteLine("  {0}", ToString());
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            ret.AppendFormat( "{0} ({1})", m_eTagId.ToString(), this.GetType().Name);

            return ret.ToString();
        }
	}
}
