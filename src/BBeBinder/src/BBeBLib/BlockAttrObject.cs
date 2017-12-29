using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class BlockAttrObject : BBeBObject
	{
		public BlockAttrObject(ushort id)
			: base(id)
		{
            Type = ObjectType.BlockAtr;
		}

        public void createDefaultTags( ushort width, ushort height )
        {
			Tags.Add(new UInt16Tag(TagId.BlockWidth, width));
			Tags.Add(new UInt16Tag(TagId.BlockHeight, height));
            Tags.Add(new UInt16Tag(TagId.BlockRule, 0x0012));
            Tags.Add(new UInt32Tag(TagId.BlockAttrUnknown1, 0x00ff));
            Tags.Add(new UInt16Tag(TagId.Layout, 0x0034));
            Tags.Add(new UInt16Tag(TagId.BlockAttrUnknown3, 0x0000));
            Tags.Add(new UInt32Tag(TagId.BlockAttrUnknown4, 0));
            Tags.Add(new UInt16Tag(TagId.BlockAttrUnknown0, 0x0001));
            Tags.Add(new UInt16Tag(TagId.BlockAttrUnknown5, 0x0000));
            Tags.Add(new UInt16Tag(TagId.BlockAttrUnknown6, 0x0000));

            byte[] sixBytes = { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Tags.Add(new ByteArrayTag( TagId.BGImageName, sixBytes ) );
        }
	}
}
