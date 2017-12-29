using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class PageAttrObject : BBeBObject
	{
		public PageAttrObject(ushort id)
			: base(id)
		{
            Type = ObjectType.PageAtr;
		}

        public void createDefaultTags()
        {
            Tags.Add(new UInt16Tag(TagId.TopMargin, 0x0000)); // 5
            Tags.Add(new UInt16Tag(TagId.HeadHeight, 0x0000)); // 53
            Tags.Add(new UInt16Tag(TagId.HeadSep, 0x0000)); // 5
            Tags.Add(new UInt16Tag(TagId.OddSideMargin, 0x0000)); // 42
            Tags.Add(new UInt16Tag(TagId.EvenSideMargin, 0x0000)); // 42
			Tags.Add(new UInt16Tag(TagId.TextHeight, BBeB.ReaderPageHeight)); // 674
            Tags.Add(new UInt16Tag(TagId.TextWidth, BBeB.ReaderPageWidth)); // 516
            Tags.Add(new UInt16Tag(TagId.FootSpace, 0x0000)); // 58
            Tags.Add(new UInt16Tag(TagId.FootHeight, 0x0000)); // 53
            Tags.Add(new UInt16Tag(TagId.Layout, 0x0034)); // 52
            Tags.Add(new UInt16Tag(TagId.PagePosition, 0x0000));
            Tags.Add(new UInt16Tag(TagId.SetEmptyView, 0x0001));
            Tags.Add(new UInt16Tag(TagId.SetWaitProp, 0x0002));

            byte[] sixBytes = { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Tags.Add(new ByteArrayTag(TagId.BGImageName, sixBytes));
        }
	}
}
