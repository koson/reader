using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class ImageObject : BBeBObject
	{
		public static string getImageTypeAsString(StreamContents format)
		{
			switch (format)
			{
				case StreamContents.JpegImage:
					return "jpg";
				case StreamContents.PngImage:
					return "png";
				case StreamContents.GifImage:
					return "gif";
				case StreamContents.BmpImage:
					return "bmp";
				default:
					return "unknown";
			}
		}

		public ImageObject(ushort id)
			: base(id)
		{
            Type = ObjectType.Image;
		}

        public void setRect( ushort x, ushort y, ushort w, ushort h )
        {
            UInt16ArrayTag tag = (UInt16ArrayTag)FindFirstTag(TagId.ImageRect);
            if (tag == null)
            {
                tag = new UInt16ArrayTag(TagId.ImageRect, new ushort[4]);
                this.Tags.Add(tag);
            }
            tag.Value[0] = x;
            tag.Value[1] = y;
            tag.Value[2] = w;
            tag.Value[3] = h;
        }

        public void setSize( ushort w, ushort h )
        {
            UInt16ArrayTag tag = (UInt16ArrayTag)FindFirstTag(TagId.ImageSize);
            if (tag == null)
            {
                tag = new UInt16ArrayTag(TagId.ImageSize, new ushort[2]);
                this.Tags.Add(tag);
            }
            tag.Value[0] = w;
            tag.Value[1] = h;
        }

        public void setImageStreamId( uint id )
        {
            UInt32Tag tag = (UInt32Tag)FindFirstTag(TagId.ImageStream);
            if (tag == null)
            {
                tag = new UInt32Tag(TagId.ImageStream, 0);
                this.Tags.Add(tag);
            }
            tag.Value = id;
        }
	}
}
