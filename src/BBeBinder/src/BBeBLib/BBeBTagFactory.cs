using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Text;
using BBeBLib.Serializer;


namespace BBeBLib
{
	public class BBeBTagFactory
	{
		static bool s_bDebugMode = false;


		public static BBeBTag ReadTag(TagId eTagId, BBeBinaryReader tagReader, 
											ref List<BBeBTag> parsedTags, ObjectType eObjectType)
		{
			Debug.WriteLineIf(s_bDebugMode, " Tag: " + eTagId.ToString());

			BBeBTag tag = null;

			switch (eTagId)
			{
				case TagId.BeginPage:
					ushort[] sdata = new ushort[2];
					sdata[0] = tagReader.ReadUInt16();
					sdata[1] = tagReader.ReadUInt16();
					tag = new UInt16ArrayTag(eTagId, sdata);
					break;


				// Tags with no data
				case TagId.EndPage:
				case TagId.EOL:
				case TagId.EndButton:
				case TagId.BeginSup:
				case TagId.EndSup:
				case TagId.BeginSub:
				case TagId.EndSub:
				case TagId.ItalicBegin:
				case TagId.ItalicEnd:
				case TagId.BeginEmpLine:
				case TagId.EndEmpLine:
					tag = new BBeBTag(eTagId);
					break;

				case TagId.PageObjectIds:
					uint[] objIds = new uint[tagReader.ReadUInt16()];
					for (ushort i = 0; i < objIds.Length; i++)
					{
						objIds[i] = tagReader.ReadUInt32();
					}
					tag = new UInt32ArrayTag(eTagId, objIds);
					break;

				case TagId.StreamFlags:

					tag = StreamTagSerializer.Deserialize(tagReader, eObjectType);
					break;

				case TagId.PageList:
					uint[] pages = new uint[tagReader.ReadUInt16()];
					for (ushort i = 0; i < pages.Length; i++)
					{
						pages[i] = tagReader.ReadUInt32();
					}
					tag = new UInt32ArrayTag(eTagId, pages);
					// TODO - Add a description to the Wiki
					break;

				case TagId.EmpDotsPosition:
				case TagId.EmpLinePosition:
				case TagId.EmpLineMode:
				case TagId.RubyOverhang:
				case TagId.RubyAlign:
				case TagId.SetWaitProp:
				case TagId.BlockRule:
				case TagId.BlockWidth:
				case TagId.BlockHeight:
				case TagId.FontUnknownZero:
				case TagId.BlockAlignment:
				case TagId.FontUnknownTwo:
				case TagId.FontUnknownThree:
				case TagId.FontSize:
				case TagId.FontWidth:
				case TagId.FontWeight:
				case TagId.TextHeight:
				case TagId.FontEscapement:
				case TagId.FontOrientation:
				case TagId.TextWidth:
				case TagId.TopMargin:
				case TagId.HeadHeight:
				case TagId.HeadSep:
				case TagId.OddSideMargin:
				case TagId.EvenSideMargin:
				case TagId.FootSpace:
				case TagId.FootHeight:
				case TagId.SetEmptyView:
				case TagId.PagePosition:
				case TagId.BlockAttrUnknown0:
				case TagId.Layout:
				case TagId.BlockAttrUnknown3:
				case TagId.BlockAttrUnknown5:
				case TagId.BlockAttrUnknown6:
				case TagId.WordSpace:
				case TagId.LetterSpace:
				case TagId.CharSpace:
				case TagId.BaseLineSkip:
				case TagId.LineSpace:
				case TagId.LineWidth:
				case TagId.ParIndent:
                case TagId.ParSkip:
				case TagId.ButtonFlags:
				case TagId.LocationX:
				case TagId.LocationY:
				case TagId.Space:
					tag = new UInt16Tag(eTagId, tagReader.ReadUInt16());
					break;

				case TagId.RuledLine:
					tag = new ByteArrayTag(eTagId, tagReader.ReadBytes(10));
					break;

				case TagId.BGImageName:
					tag = new ByteArrayTag(eTagId, tagReader.ReadBytes(6));
					break;

				case TagId.EmpDotsCode:
					{
						EmpDotsCodeTag emDotsTag = new EmpDotsCodeTag(eTagId);
						tag = emDotsTag;

						emDotsTag.Value = tagReader.ReadUInt32();
						if (emDotsTag.Value != 0x0)
						{
							Debug.WriteLineIf(s_bDebugMode, "Got EmpDotsCode val = 0x" + emDotsTag.Value.ToString("x"));
						}

						// The font name tag always follows
						eTagId = tagReader.ReadTag();
						if (TagId.FontFacename != eTagId)
						{
							throw new InvalidTagException("Expected font: " + eTagId.ToString(), 0x0);
						}
						emDotsTag.FontFace = new StringTag(eTagId, tagReader.ReadString());

						// And the EmpDotsCode value
						emDotsTag.DotsCode = tagReader.ReadUInt16();

						// TODO - Add a description to the Wiki
					}
					break;

				case TagId.ImageRect:
					ushort[] bounds = new ushort[4];
					bounds[0] = tagReader.ReadUInt16();
					bounds[1] = tagReader.ReadUInt16();
					bounds[2] = tagReader.ReadUInt16();
					bounds[3] = tagReader.ReadUInt16();

					tag = new UInt16ArrayTag(eTagId, bounds);
					break;

                case TagId.ImageSize:
                {
                    ushort[] size = new ushort[2];
                    size[0] = tagReader.ReadUInt16();
                    size[1] = tagReader.ReadUInt16();

                    tag = new UInt16ArrayTag(eTagId, size);
                    break;
                }
				case TagId.ObjectInfoLink:
				case TagId.Link:
				case TagId.ParentPageTree:
				case TagId.ChildPageTree:
				case TagId.BlockAttrUnknown1:
				case TagId.BlockAttrUnknown4:
				case TagId.ImageStream:
				case TagId.TextColor:
				case TagId.TextBgColor:
				case TagId.LineColor:
				case TagId.FillColor:
				case TagId.BeginButton:
				case TagId.OddHeaderId:
				case TagId.EvenHeaderId:
				case TagId.OddFooterId:
				case TagId.EvenFooterId:
					tag = new UInt32Tag(eTagId, tagReader.ReadUInt32());
					break;

				case TagId.FontFacename:
                case TagId.UnknownStr1:
				case TagId.FontFileName:
                case TagId.UnknownStr3:
                case TagId.UnknownStr4:
					tag = new StringTag(eTagId, tagReader.ReadString());
					break;


                // Button Tags
                case TagId.BaseButtonStart:
                    tag = new UInt32Tag(eTagId, 0);
                    break;

                case TagId.FocusinButtonStart:
                    tag = new UInt32Tag(eTagId, 1);
                    break;

                case TagId.PushButtonStart:
                    tag = new UInt32Tag(eTagId, 2);
                    break;

                case TagId.UpButtonStart:
                    tag = new UInt32Tag(eTagId, 0);
                    break;

                case TagId.BaseButtonEnd:
                case TagId.FocusinButtonEnd:
                case TagId.PushButtonEnd:
                case TagId.UpButtonEnd:
                    tag = new IDOnlyTag(eTagId);
                    break;

                case TagId.StartActionsStart:
                case TagId.StartActionsEnd:
                    tag = new IDOnlyTag(eTagId);
                    break;

                case TagId.JumpTo:
                {
                    uint[] size = new uint[2];
                    size[0] = tagReader.ReadUInt32();
                    size[1] = tagReader.ReadUInt32();

                    tag = new UInt32ArrayTag(eTagId, size);
                    break;
                }

				case TagId.KomaPlot:
					ushort width = tagReader.ReadUInt16();
					ushort height = tagReader.ReadUInt16();
					uint refobj = tagReader.ReadUInt32();
					uint unknown = tagReader.ReadUInt32();
					tag = new KomaPlotTag(width, height, refobj, unknown);
					break;

                case TagId.SendMessage:
                    ushort parms = tagReader.ReadUInt16();
                    ushort ssize = tagReader.ReadUInt16();
                    byte[] data = tagReader.ReadBytes(ssize);
                    string s1 = System.Text.Encoding.Unicode.GetString( data );
                    ssize = tagReader.ReadUInt16();
                    data = tagReader.ReadBytes(ssize);
                    string s2 = System.Text.Encoding.Unicode.GetString(data);

                    tag = new MessageTag(eTagId, parms, s1, s2 );

                    break;

                case TagId.ObjectEnd:
//                    tag = new IDOnlyTag(eTagId);                    
					break;

				default:
					throw new InvalidTagException("Unsupported tag: " + eTagId.ToString(), (ushort)eTagId);
			}

            if ( tag != null )
                parsedTags.Add(tag);
			return tag;
		}

		public static List<BBeBTag> ParseAllTags(BBeBObject obj, byte[] tagBytes)
		{
			List<BBeBTag> parsedTags = obj.Tags;

			MemoryStream tagStream = new MemoryStream(tagBytes);
			BBeBinaryReader tagReader = new BBeBinaryReader(tagStream);

			while (tagReader.BaseStream.Position < tagBytes.Length)
			{
				TagId eTagId = tagReader.ReadTag();

                // This method adds the tag it created to the parsed tags array
                // Changed because some tags read other tags (as they need the data from the other tags
                // e.g. StreamSize tag includes StreamStart and StreamEnd tags), and we lose these tags otherwise
				BBeBTag tag = ReadTag(eTagId, tagReader, ref parsedTags, obj.Type);
			}

			return parsedTags;
		}
	}
}
