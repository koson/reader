using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	/// <summary>
	/// This isn't a *real* tag. That is it doesn't map directly to a tag
	/// in the BBeB stream. Instead it maps to four tags in a specific order:
	/// 
	///   TagId.StreamFlags
	///   ushort             (the flag values)
	///   TagId.StreamSize
	///   uint               (the number of bytes in this stream)
	///   TagId.StreamStart
	///   byte[stream size]  (as specified by the stream size)
	///   TagId.StreamEnd
	/// 
	/// <remarks>This data is *always* uncompressed/unscrambled/unencrypted.</remarks>
	/// </summary>
	public class StreamTagGroup : BBeBTag
	{
		StreamContents m_eContents = StreamContents.Unknown;	// This is LSB of the StreamFlags

		byte[] m_Data = null;

		public StreamTagGroup()
			: base(TagId.StreamGroup)
		{
		}

		/// <summary>
		/// The plaintext data in this stream.
		/// </summary>
		public byte[] Data
		{
			get { return m_Data; }
			set { m_Data = value; }
		}

		public StreamContents Contents
		{
			get { return m_eContents; }
			set { m_eContents = value; }
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("Stream ({0}): ", m_eContents.ToString());
			ByteArrayTag.FormatByteArray(sb, Data);

			return sb.ToString();
		}
	}
}
