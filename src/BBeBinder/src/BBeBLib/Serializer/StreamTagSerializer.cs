using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace BBeBLib.Serializer
{
	public class StreamTagSerializer
	{
		private static ushort s_wEncodingByte;
		private static bool s_bDebugMode = false;

		public static ushort EncodingByte
		{
			get
			{
				return s_wEncodingByte;
			}
			set
			{
				// It appears that only the bottom byte is the pseudo encryption byte.
				s_wEncodingByte = (ushort)(value & 0xff);
			}
		}


		public static void Serialize(BBeBinaryWriter writer, StreamTagGroup tagGroup)
		{
			writer.Write(TagId.StreamFlags);

			ushort wFlags = (ushort)((ushort)(tagGroup.Contents) | (ushort)(StreamFormatFlags.None));
			writer.Write(wFlags);

			writer.Write(TagId.StreamSize);
			writer.Write((uint)tagGroup.Data.Length);

			writer.Write(TagId.StreamStart);

			writer.Write(tagGroup.Data);

			writer.Write(TagId.StreamEnd);
		}

		/// <summary>
		/// Read a group of stream tags (which all come in order) from the BBeB reader.
		/// </summary>
		/// <param name="tagReader">The reader to do the reading with</param>
		/// <returns>A new tag representing the uncompressed data in the stream.</returns>
		public static StreamTagGroup Deserialize(BBeBinaryReader tagReader, ObjectType eObjectType)
		{
			ushort wFlags = tagReader.ReadUInt16();

			StreamFormatFlags eStreamFlags = (StreamFormatFlags)(wFlags & 0xff00);
			StreamContents eContents = (StreamContents)(wFlags & 0x00ff);

			TagId eTagId = tagReader.ReadTag();
			if (eTagId != TagId.StreamSize)
			{
				throw new UnexpectedTagException("Expected a StreamSize tag: " + eTagId.ToString());
			}

			uint dwStreamSize = tagReader.ReadUInt32();

			eTagId = tagReader.ReadTag();
			if (eTagId != TagId.StreamStart)
			{
				throw new UnexpectedTagException("Expected a StreamStart tag: " + eTagId.ToString());
			}

			byte[] streamData = tagReader.ReadBytes((int)dwStreamSize);

			try
			{
				eTagId = tagReader.ReadTag();
			}
			catch (InvalidTagException ex)
			{
				Debug.WriteLineIf(s_bDebugMode, "Not a tag at end of stream: 0x" + ex.Value.ToString("x"));
				// dataTag.AppendShort(ex.Value);

				// Temporarily
				eTagId = TagId.StreamEnd;
			}
			if (eTagId != TagId.StreamEnd)
			{
				throw new UnexpectedTagException("Expected a StreamEnd tag: " + eTagId.ToString());
			}

			StreamTagGroup tag = new StreamTagGroup();

			tag.Contents = eContents;

			StreamTagSerializer streamTagSerializer = new StreamTagSerializer();

			tag.Data = streamTagSerializer.Convert(streamData, eStreamFlags, 
													StreamFormatFlags.None, eObjectType);

			return tag;
		}


		/// <summary>
		/// Scramble *or* unscramble an array of data. Because the data is XOR'ed with a value
		/// this is a reversible operation, and scrambling is the same as unscrambling.
		/// </summary>
		/// <param name="data">The data to scramble</param>
		/// <param name="eObjectType">The data's object type. Certain types will only have so much 
		/// of their data scrambled.</param>
		private static void XorData(byte[] data, ObjectType eObjectType)
		{
			// Unscramble the data
			byte xorKey = (byte)((data.Length % s_wEncodingByte) + 0x0F);
			int l = data.Length;

			if (eObjectType == ObjectType.ImageStream || eObjectType == ObjectType.Font || eObjectType == ObjectType.Sound)
			{
				// For image streams, fonts and sounds, the scrambled length is fixed at 0x400
				l = Math.Min(l, 0x400);
			}

			while (l-- > 0)
			{
				data[l] ^= xorKey;
			}
		}

		private static byte[] UnscrambleAndUncompress(byte[] data, StreamFormatFlags eInputFormat, ObjectType eObjectType)
		{
			byte[] scdata = (byte[])data.Clone();

			if ((eInputFormat & StreamFormatFlags.Scrambled) == StreamFormatFlags.Scrambled)
			{
				// Unscramble the data
				XorData(scdata, eObjectType);
			}

			if ((eInputFormat & StreamFormatFlags.Compressed) == StreamFormatFlags.Compressed)
			{
				int len = (scdata[0]) + (scdata[1] << 8) +
							(scdata[2] << 16) + (scdata[3] << 24);

				ICSharpCode.SharpZipLib.Zip.Compression.Inflater decompressor =
					new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();

				decompressor.SetInput(scdata, sizeof(uint),
								scdata.Length - sizeof(uint));

				byte[] byInflated = new byte[len];

				int nBytesDecompressed = decompressor.Inflate(byInflated);
				if (nBytesDecompressed != len)
				{
					throw new ApplicationException("Didn't decompress all of the text stream");
				}

				scdata = byInflated;
			}

			return scdata;
		}

		public byte[] Convert(byte[] data, StreamFormatFlags eInputFormat, 
								StreamFormatFlags eOutputFormat, ObjectType eObjectType)
		{
			if (eInputFormat == eOutputFormat)
			{
				return data;
			}
			if ((eInputFormat & StreamFormatFlags.Encrypted) == StreamFormatFlags.Encrypted)
			{
				throw new InvalidDataException("Can't handle encrypted streams");
			}
			if ((eOutputFormat & StreamFormatFlags.Encrypted) == StreamFormatFlags.Encrypted)
			{
				throw new InvalidDataException("Can't handle encrypted streams");
			}

			if (eOutputFormat == StreamFormatFlags.None)
			{
				return UnscrambleAndUncompress(data, eInputFormat, eObjectType);
			}
			else
			{
				byte[] compressed = data;

				if ((eInputFormat & StreamFormatFlags.Compressed) == StreamFormatFlags.Compressed)
				{
					MemoryStream compStream = new MemoryStream();
					ZLib.Compress(data, data.Length, compStream);
					compStream.Flush();
					compressed = compStream.GetBuffer();
				}

				byte[] scrambled = compressed;

				if ((eInputFormat & StreamFormatFlags.Scrambled) == StreamFormatFlags.Scrambled)
				{
					XorData(scrambled, eObjectType);
				}

				return scrambled;
			}
		}
	}
}
