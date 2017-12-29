using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class ZLib
	{
		/// <summary>
		/// Compress the input data to an output stream. The first four bytes
		/// is a UInt32 (in LSB format) that contains the size of the *decompressed*
		/// data.
		/// </summary>
		/// <param name="inputData">An array of bytes to compress</param>
		/// <param name="nNumInputBytes">The number of input bytes from inputData to compress</param>
		/// <param name="outStream">The stream to write the compressed bytes to.</param>
		/// <returns>The length of the compressed data (not including the length value
		/// that is written to the beginning of the stream)</returns>
		public static int Compress(byte[] inputData, int nNumInputBytes, Stream outStream )
		{
			if (inputData.Length == 0 || nNumInputBytes == 0)
			{
				throw new InvalidDataException("ZLib.Compress: No input data to compress.");
			}

			long nMaxCompressedDataLen = nNumInputBytes + ((nNumInputBytes / 1000) + 1) + 12 + sizeof(int);

			byte[] outBuff = new byte[nMaxCompressedDataLen];
			ICSharpCode.SharpZipLib.Zip.Compression.Deflater compresser =
				new ICSharpCode.SharpZipLib.Zip.Compression.Deflater();
			compresser.SetInput(inputData);
			compresser.Finish();

			int nCompressedDataLength = compresser.Deflate(outBuff);

			BinaryWriter writer = new BinaryWriter(outStream);
			writer.Write(nNumInputBytes);
			writer.Write(outBuff, 0, nCompressedDataLength);

			writer.Flush();

			return nCompressedDataLength;
		}


		/// <summary>
		/// Compress the input data and write that data using the supplied writer. 
		/// The first four bytes is a UInt32 (in LSB format) that contains 
		/// the size of the *decompressed* data.
		/// </summary>
		/// <param name="inputData">The data to compress</param>
		/// <param name="nNumInputBytes">The number of input bytes from inputData to compress</param>
		/// <param name="writer">The compressed data is written using this</param>
		/// <returns>The length of the compressed data (not including the length value
		/// that is written to the beginning of the stream)</returns>
		public static int Compress(byte[] inputData, int nNumInputBytes, BinaryWriter writer)
		{
			MemoryStream outStream = new MemoryStream();

			int nCompressedDataLength = Compress(inputData, nNumInputBytes, outStream);

			writer.Write(outStream.GetBuffer(), 0, (int)outStream.Position);

			return nCompressedDataLength;
		}

		/// <summary>
		/// Decompress data.
		/// </summary>
		/// <param name="byCompresedData">The data to decompress</param>
		/// <param name="dwDecompressedLen">The decompressed size that the compressed data will
		/// expand into.</param>
		/// <returns>The decompressed data</returns>
		public static byte[] Decompress(byte[] byCompresedData, uint dwDecompressedLen )
		{
			byte[] byUncompressedData = new byte[dwDecompressedLen];

			ICSharpCode.SharpZipLib.Zip.Compression.Inflater decompressor =
				new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();

			decompressor.SetInput(byCompresedData);

			int nBytesDecompressed = decompressor.Inflate(byUncompressedData);

			if (nBytesDecompressed != dwDecompressedLen)
			{
				throw new ApplicationException("Didn't decompress all of the metadata");
			}

			return byUncompressedData;
		}


		/// <summary>
		/// Decompress data from a BinaryReader.
		/// </summary>
		/// <param name="reader">The reader to use to read the metadata with. It is
		/// already positioned at the first byte of metadata.</param>
		/// <param name="nCompressedLen">The length of the compressed metadata. The first
		/// four bytes is the size of the uncompressed data.</param>
		/// <returns>The decompressed data</returns>
		public static byte[] Decompress(BinaryReader reader, int nCompressedLen)
		{
			uint dwDecompressedLen = reader.ReadUInt32();
			nCompressedLen -= sizeof(uint);	// Subtract size of size data

			byte[] byCompressedBytes = reader.ReadBytes(nCompressedLen);

			return Decompress(byCompressedBytes, dwDecompressedLen);
		}
	}
}
