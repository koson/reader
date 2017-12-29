using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using BBeBLib;


namespace BBeBLib
{
	public class BBeBObjectFactory
	{
		static bool s_bDebugMode = false;

		private void ParseTagData(BBeBObject obj, byte[] tagBytes)
		{
			BBeBTagFactory.ParseAllTags(obj, tagBytes);
		}

		private PageObject ReadPageObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			PageObject obj = new PageObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private TocObject ReadTocObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			TocObject obj = new TocObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private PageTreeObject ReadPageTreeObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			PageTreeObject obj = new PageTreeObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private BookAttrObject ReadBookAttrObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			BookAttrObject obj = new BookAttrObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private BlockObject ReadBlockObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			BlockObject obj = new BlockObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private BlockAttrObject ReadBlockAttrObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			BlockAttrObject obj = new BlockAttrObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private TextObject ReadTextObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			TextObject obj = new TextObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private TextAttrObject ReadTextAttrObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			TextAttrObject obj = new TextAttrObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private SimpleTextObject ReadSimpleTextObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			SimpleTextObject obj = new SimpleTextObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private ImageObject ReadImageObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			ImageObject obj = new ImageObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private ImageStreamObject ReadImageStreamObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			ImageStreamObject obj = new ImageStreamObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

		private PageAttrObject ReadPageAttrObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
		{
			PageAttrObject obj = new PageAttrObject(nObjId);

			ParseTagData(obj, tagBytes);

			return obj;
		}

        private ObjectInfoObject ReadObjectInfoObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
        {
            ObjectInfoObject obj = new ObjectInfoObject(nObjId);

            ParseTagData(obj, tagBytes);

            return obj;
        }

        private HeaderObject ReadHeaderObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
        {
            HeaderObject obj = new HeaderObject(nObjId);

            ParseTagData(obj, tagBytes);

            return obj;
        }

        private ButtonObject ReadButtonObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
        {
            ButtonObject obj = new ButtonObject(nObjId);

            ParseTagData(obj, tagBytes);

            return obj;
        }

        private PopUpWinObject ReadPopUpWinObject(BinaryReader reader, ushort nObjId, byte[] tagBytes)
        {
            PopUpWinObject obj = new PopUpWinObject(nObjId);

            ParseTagData(obj, tagBytes);

            return obj;
        }

        public BBeBObject CreateObject(BinaryReader reader, uint nObjId, uint nObjLen)
		{
			long nStartPos = reader.BaseStream.Position;

			ushort nObjStartMarker = reader.ReadUInt16();
			if (nObjStartMarker != 0xf500)
			{
				throw new InvalidTagException("Object didn't start with 0xf500: " + nObjStartMarker.ToString(), nObjStartMarker);
			}

			ushort id = reader.ReadUInt16();
			if (id != nObjId)
			{
				throw new InvalidDataException("Object ID mismatch.");
			}

			ushort zero = reader.ReadUInt16();
			if (zero != 0x0)
			{
				throw new InvalidDataException("Object didn't have zero as second word.");
			}
			ushort nObjType = reader.ReadUInt16();
			ObjectType objType = (ObjectType)nObjType;

			Debug.WriteLineIf(s_bDebugMode, "Obj: " + objType.ToString() + " id=" + id);

			BBeBObject obj = null;

			long nBytesSoFar = reader.BaseStream.Position - nStartPos;
			byte[] tagBytes = reader.ReadBytes((int)(nObjLen - nBytesSoFar));

			switch (objType)
			{
				case ObjectType.Page:
					obj = ReadPageObject(reader, id, tagBytes);
					break;
				case ObjectType.TOC:
					obj = ReadTocObject(reader, id, tagBytes);
					break;
				case ObjectType.PageTree:
					obj = ReadPageTreeObject(reader, id, tagBytes);
					break;
				case ObjectType.PageAtr:
					obj = ReadPageAttrObject(reader, id, tagBytes);
					break;
				case ObjectType.BookAtr:
					obj = ReadBookAttrObject(reader, id, tagBytes);
					break;
				case ObjectType.Block:
					obj = ReadBlockObject(reader, id, tagBytes);
					break;
				case ObjectType.BlockAtr:
					obj = ReadBlockAttrObject(reader, id, tagBytes);
					break;
				case ObjectType.Text:
					obj = ReadTextObject(reader, id, tagBytes);
					break;
				case ObjectType.TextAtr:
					obj = ReadTextAttrObject(reader, id, tagBytes);
					break;
				case ObjectType.SimpleText:
					obj = ReadSimpleTextObject(reader, id, tagBytes);
					break;
				case ObjectType.Image:
					obj = ReadImageObject(reader, id, tagBytes);
					break;
				case ObjectType.ImageStream:
					obj = ReadImageStreamObject(reader, id, tagBytes);
					break;
				case ObjectType.ObjectInfo:
					obj = ReadObjectInfoObject(reader, id, tagBytes);
					break;
                case ObjectType.Header:
                    obj = ReadHeaderObject(reader, id, tagBytes);
                    break;
                case ObjectType.Button:
                    obj = ReadButtonObject(reader, id, tagBytes);
                    break;
                case ObjectType.PopUpWin:
                    obj = ReadPopUpWinObject(reader, id, tagBytes);
                    break;
                default:
					throw new InvalidDataException("Unsupported object type: " + objType.ToString());
			}

			return obj;
		}
	}
}
