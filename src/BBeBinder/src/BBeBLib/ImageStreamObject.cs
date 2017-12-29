using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace BBeBLib
{
	public class ImageStreamObject : StreamObject
	{
		public ImageStreamObject(ushort id) : base(id)
		{
            Type = ObjectType.ImageStream;
		}
	}
}
