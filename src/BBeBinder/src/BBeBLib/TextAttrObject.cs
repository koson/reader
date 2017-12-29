using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	public class TextAttrObject : BBeBObject
	{
		public TextAttrObject(ushort id)
			: base(id)
		{
            Type = ObjectType.TextAtr;
		}

        public void createDefaultTags( BlockAlignment blockAlignment )
        {
            // UTF-16LE string containing font name
            byte[] fontname = { 22, 0, // 11 chars
				    (byte)'I', 0, (byte)'W', 0, (byte)'A', 0, 0x0E, 0x66, (byte)'-', 0, 0x2D, 0x4E, 0x30,
				    0x7D, (byte)'N', 0, (byte)'-', 0, (byte)'e', 0, (byte)'b', 0 };

            Tags.Add(new UInt16Tag(TagId.RubyOverhang, 0x0000));
            Tags.Add(new UInt16Tag(TagId.EmpLinePosition, 0x0001));
            Tags.Add(new UInt16Tag(TagId.EmpLineMode, 0x0000));
            Tags.Add(new UInt16Tag(TagId.EmpDotsPosition, 0x0001));
            Tags.Add(new UInt16Tag(TagId.FontSize, 100));
            Tags.Add(new UInt16Tag(TagId.FontWidth, 0xfff6));
            Tags.Add(new UInt16Tag(TagId.FontEscapement, 0x0000));
            Tags.Add(new UInt16Tag(TagId.FontOrientation, 0x0000));
            Tags.Add(new UInt16Tag(TagId.FontWeight, 400));
            Tags.Add(new ByteArrayTag(TagId.FontFacename, fontname));
            Tags.Add(new UInt32Tag(TagId.TextColor, 0));
            Tags.Add(new UInt32Tag(TagId.TextBgColor, 0x00ff));
            Tags.Add(new UInt16Tag(TagId.WordSpace, 0x0019));
            Tags.Add(new UInt16Tag(TagId.LetterSpace, 0x0000));
            Tags.Add(new UInt16Tag(TagId.BaseLineSkip, 0x008c));
            Tags.Add(new UInt16Tag(TagId.LineSpace, 0x000a));
            Tags.Add(new UInt16Tag(TagId.ParIndent, 0));
            Tags.Add(new UInt16Tag(TagId.ParSkip, 0));
            Tags.Add(new UInt16Tag(TagId.LineWidth, 0x0002));
            Tags.Add(new UInt32Tag(TagId.LineColor, 0));
			Tags.Add(new UInt16Tag(TagId.BlockAlignment, (ushort)blockAlignment));
            Tags.Add(new UInt16Tag(TagId.FontUnknownTwo, 0x0001));
            Tags.Add(new UInt16Tag(TagId.FontUnknownThree, 0x0000));
            Tags.Add(new UInt16Tag(TagId.RubyAlign, 0x0001));
        }
	}
}
