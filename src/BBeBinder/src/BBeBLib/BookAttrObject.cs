using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class BookAttrObject : BBeBObject
	{
		public BookAttrObject(ushort id)
			: base(id)
		{
            Type = ObjectType.BookAtr;
		}

        public void createDefaultTags()
        {
            Tags.Add(new UInt16Tag(TagId.RubyAlign, 0x0002));
            Tags.Add(new UInt16Tag(TagId.RubyOverhang, 0x0000));
            Tags.Add(new UInt16Tag(TagId.EmpDotsPosition, 0x0001));

            EmpDotsCodeTag tag = new EmpDotsCodeTag( TagId.EmpDotsCode);
            tag.Value = 0;
            tag.FontFace = new StringTag(TagId.FontFacename, "");
            tag.DotsCode = (ushort)46;
            Tags.Add(tag);

            Tags.Add(new UInt16Tag(TagId.EmpLinePosition, 0x0002));
            Tags.Add(new UInt16Tag(TagId.EmpLineMode, 0x0010));
            Tags.Add(new UInt16Tag(TagId.SetWaitProp, 0x0002));

/*
            byte[] tmp78 = { 0, 0, 0, 0, 0x16, (byte)0xf5, 0, 0, 0x1, 0x30 };
            head.AddTag(TagId.EmpDotsCode, tmp78);
*/
        }
	}
}
