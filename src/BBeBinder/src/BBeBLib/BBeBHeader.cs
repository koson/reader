using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using BBeBLib;

namespace BBeBLib
{
	public class BBeBHeader
	{
        public const int LRF_DIRECTION_FORWARDS = 0x01;
        public const int LRF_DIRECTION_BACKWARDS = 0x10;

		public char[] signature = new char[4];
		public ushort wVersion = 999;
		public ushort wPseudoEncByte;
		public uint dwRootObjectId;
		private ulong ddwNumOfObjects;
		private ulong ddwObjIndexOffset;
		public uint dwUnknown1;
		public byte byBindingDir = BBeBHeader.LRF_DIRECTION_FORWARDS;
		public byte byPadding1;
		public ushort wDPI = 170;
		public ushort wPadding2;
		public ushort wScreenWidth = BBeB.ReaderPageWidth;
		public ushort wScreenHeight = BBeB.ReaderPageHeight;
		public byte byColorDepth = 24;
		public byte byPadding3;
		public byte[] byUnkonwn2 = new byte[20];
		public uint dwTocObjectId;
		public uint dwTocObjectOffset;
		public ushort wDocInfoCompSize;
		public ushort wThumbnailFlags;	// MSB = ThumbnailFormat, LSB = ThumbnailType
		public uint dwThumbSize;

		public BBeBHeader()
		{
			signature[0] = 'L';
			signature[1] = 'R';
			signature[2] = 'F';
			signature[3] = '\0';
		}

		public ulong ObjectIndexOffset
		{
			get { return ddwObjIndexOffset; }
			set { ddwObjIndexOffset = value; }
		}

		public ulong NumberOfObjects
		{
			get { return ddwNumOfObjects; }
			set { ddwNumOfObjects = value; }
		}

		public StreamFormatFlags ThumbnailFormat
		{
			get
			{
				return (StreamFormatFlags)(wThumbnailFlags & 0xff00);
			}
			set
			{
				wThumbnailFlags = (ushort)((wThumbnailFlags & 0x00ff) | ((ushort)value & 0xff00));
			}
		}

		public StreamContents ThumbnailType
		{
			get
			{
				return (StreamContents)(wThumbnailFlags & 0x00ff);
			}
			set {
				wThumbnailFlags = (ushort)((wThumbnailFlags & 0xff00) | ((ushort)value & 0x00ff));
			}
		}

		public void Validate()
		{
			if (signature[0] != 'L' ||
				signature[1] != 'R' ||
				signature[2] != 'F' ||
				signature[3] != '\0')
			{
				throw new InvalidHeaderException("Invalid signature");
			}

			if (wVersion == 0)
			{
				throw new InvalidHeaderException("Invalid version");
			}

			switch (ThumbnailType)
			{
				case StreamContents.BmpImage:
				case StreamContents.GifImage:
				case StreamContents.JpegImage:
				case StreamContents.PngImage:
					break;
				default:
					throw new InvalidHeaderException("Invalid thumbnail image type: " + ThumbnailType.ToString());
			}
  		}

		public void WriteDebugInfo(TextWriter writer)
		{
			writer.WriteLine("Header:");
			writer.WriteLine("======================");
			writer.WriteLine(ToString());
			writer.WriteLine();
		}

        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
		    ret.AppendLine( "signature - " + signature );
            ret.AppendLine("Version - " + wVersion );
            ret.AppendLine("PseudoEncByte - " + wPseudoEncByte );
            ret.AppendLine("RootObjectId - " + dwRootObjectId );
            ret.AppendLine("NumOfObjects - " + ddwNumOfObjects );
            ret.AppendLine("ObjIndexOffset - " + ddwObjIndexOffset );
            ret.AppendLine("Unknown1 - " + dwUnknown1 );
            ret.AppendLine("BindingDir - " + byBindingDir );
            ret.AppendLine("Padding1 - " + byPadding1 );
            ret.AppendLine("DPI - " + wDPI );
            ret.AppendLine("Padding2 - " + wPadding2 );
            ret.AppendLine("ScreenWidth - " + wScreenWidth );
            ret.AppendLine("ScreenHeight - " + wScreenHeight );
		    ret.AppendLine( "ColorDepth - " + byColorDepth );
		    ret.AppendLine( "Padding3 - " + byPadding3 );
//		    public byte[] byUnkonwn2 = new byte[0x14];
		    ret.AppendLine( "TocObjectId - " + dwTocObjectId );
		    ret.AppendLine( "TocObjectOffset - " + dwTocObjectOffset );
		    ret.AppendLine( "DocInfoCompSize - " + wDocInfoCompSize );
            ret.AppendLine( "ThumbSize - " + dwThumbSize );

            return ret.ToString();
        }
	}
}
