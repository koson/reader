using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib.Serializer
{
	/// <summary>
	/// A BinaryReader that knows how to write the types (and in the way) that
	/// is required for BBeB streams. This is namely strings and TagId's.
	/// Strings are length prefixed instead of null terminated, and TagId's
	/// are prefixed with 0xf5. Encoding is always Unicode.
	/// </summary>
	public class BBeBinaryReader : BinaryReader
	{
		public BBeBinaryReader(Stream stream) : base(stream, Encoding.Unicode)
		{
		}

		public BBeBinaryReader(byte[] data)
			: base(new MemoryStream(data), Encoding.Unicode)
		{
		}

		public bool IsNextCharTag()
		{
			return (PeekChar() & 0xff00) == 0xf500;
		}

		public TagId ReadTag()
		{
			ushort val = ReadUInt16();

			if ((val & 0xFF00) != 0xf500)
			{
				throw new InvalidTagException("Tag does not start with 0xf5", val);
			}

			return (TagId)(val & 0xff);
		}

		public override string ReadString()
		{
			ushort wNumBytes = ReadUInt16();
			if (wNumBytes % sizeof(char) != 0)
			{
				throw new InvalidDataException("String size not a multiple of character size");
			}
			return new string(ReadChars(wNumBytes / sizeof(char)));
		}
	}
}
