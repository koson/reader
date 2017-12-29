using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
    public class BBeByteBuffer : ByteBuffer
    {
        public bool isTag()
        {
			return (peekShort() & 0xf500) == 0xf500;
        }

        public ushort getTag( )
        {
            ushort id = getShort();
            return (ushort)(id & 0x00ff);
        }

        public void putTag(TagId tag)
        {
            putShort((ushort)(0xf500 | (ushort)tag));
        }

        public void putTagShort(TagId tag, ushort val)
        {
            putTag(tag);
            putShort(val);
        }

        public void putTagBytes(TagId tag, byte[] bytes)
        {
            putTag(tag);
            put(bytes);
        }

        public void putTagBytes(TagId tag, byte[] bytes, int length)
        {
            putTag(tag);
            put(bytes, 0, length);
        }

        public void getTagInt(out int tagId, out int id)
        {
            tagId = getTag();
            id = getInt();
        }

        public void putTagInt(TagId tag, int val)
        {
            putTag(tag);
            putInt(val);
        }

        public void AlignPosition(long alignment)
        {
            long mod = Position % alignment;
            if (mod != 0)
            {
                Position = Position + (alignment - mod);
            }
        }
    }
}
