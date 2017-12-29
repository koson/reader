using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
    public class ButtonObject : BBeBObject
    {
		ushort m_wFlags = 0x0010;	// No idea what this means
		uint[] m_dwJumpVals;
		ButtonType m_eType = ButtonType.Unknown;


        public ButtonObject(ushort id)
            : base(id)
        {
            Type = ObjectType.Button;
        }

		public ButtonType ButtonType
		{
			get { return m_eType; }
			set { m_eType = value; }
		}

		public ushort Flags
		{
			get { return m_wFlags; }
			set { m_wFlags = value; }
		}

		public void SetObjectIds(uint pageObjId, uint blockObjId)
		{
			m_dwJumpVals = new uint[2];

			m_dwJumpVals[0] = pageObjId;
			m_dwJumpVals[1] = blockObjId;
		}

		public void Serialize()
		{
			Tags.Add(new UInt16Tag(TagId.ButtonFlags, m_wFlags));

			switch (m_eType)
			{
				case ButtonType.PushButton:
					Tags.Add(new BBeBTag(TagId.PushButtonStart));
					break;
				case ButtonType.BaseButton:
					Tags.Add(new BBeBTag(TagId.BaseButtonStart));
					break;
				case ButtonType.FocusinButton:
					Tags.Add(new BBeBTag(TagId.FocusinButtonStart));
					break;
				case ButtonType.UpButton:
					Tags.Add(new BBeBTag(TagId.UpButtonStart));
					break;
			}

			Tags.Add(new BBeBTag(TagId.StartActionsStart));

			Tags.Add(new UInt32ArrayTag(TagId.JumpTo, m_dwJumpVals));

			Tags.Add(new BBeBTag(TagId.StartActionsEnd));

			switch (m_eType)
			{
				case ButtonType.PushButton:
					Tags.Add(new BBeBTag(TagId.PushButtonEnd));
					break;
				case ButtonType.BaseButton:
					Tags.Add(new BBeBTag(TagId.BaseButtonEnd));
					break;
				case ButtonType.FocusinButton:
					Tags.Add(new BBeBTag(TagId.FocusinButtonEnd));
					break;
				case ButtonType.UpButton:
					Tags.Add(new BBeBTag(TagId.UpButtonEnd));
					break;
			}
		}

		public void ParseButtonData()
		{
			bool bDone = false;

			// I hope that these tags always come in order or else this
			// routine won't work.
			for (int idx = 0; idx < Tags.Count; idx++)
			{
				BBeBTag tag = Tags[idx];
				switch (tag.Id)
				{
					case TagId.ButtonFlags:
						m_wFlags = ((UInt16Tag)tag).Value;
						break;

					case TagId.UpButtonStart:
						m_eType = ButtonType.UpButton;
						bDone = false;
						idx++;
						while (idx < Tags.Count && !bDone)
						{
							tag = Tags[idx];
							switch (tag.Id)
							{
								case TagId.StartActionsStart:
								case TagId.StartActionsEnd:
									break;
								case TagId.UpButtonEnd:
									bDone = true;
									break;
								case TagId.JumpTo:
									UInt32ArrayTag tag32 = (UInt32ArrayTag)tag;
									Debug.Assert(tag32.Value.Length == 2);
									m_dwJumpVals = tag32.Value;
									break;
								default:
									Debug.WriteLine("Unhandled button tag: " + tag.ToString());
									break;
							}
							if ( !bDone ) 
								idx++;
						}
						break;

					case TagId.FocusinButtonStart:
						m_eType = ButtonType.FocusinButton;
						bDone = false;
						idx++;
						while (idx < Tags.Count && !bDone)
						{
							tag = Tags[idx];
							switch (tag.Id)
							{
								case TagId.StartActionsStart:
								case TagId.StartActionsEnd:
									break;
								case TagId.FocusinButtonEnd:
									bDone = true;
									break;
								case TagId.JumpTo:
									UInt32ArrayTag tag32 = (UInt32ArrayTag)tag;
									Debug.Assert(tag32.Value.Length == 2);
									m_dwJumpVals = tag32.Value;
									break;
								default:
									Debug.WriteLine("Unhandled button tag: " + tag.ToString());
									break;
							}
							if (!bDone)
								idx++;
						}
						break;

					case TagId.BaseButtonStart:
						m_eType = ButtonType.BaseButton;
						bDone = false;
						idx++;
						while (idx < Tags.Count && !bDone)
						{
							tag = Tags[idx];
							switch (tag.Id)
							{
								case TagId.StartActionsStart:
								case TagId.StartActionsEnd:
									break;
								case TagId.BaseButtonEnd:
									bDone = true;
									break;
								case TagId.JumpTo:
									UInt32ArrayTag tag32 = (UInt32ArrayTag)tag;
									Debug.Assert(tag32.Value.Length == 2);
									m_dwJumpVals = tag32.Value;
									break;
								default:
									Debug.WriteLine("Unhandled button tag: " + tag.ToString());
									break;
							}
							if (!bDone)
								idx++;
						}
						break;

					case TagId.PushButtonStart:
						m_eType = ButtonType.PushButton;
						bDone = false;
						idx++;
						while (idx < Tags.Count && !bDone)
						{
							tag = Tags[idx];
							switch (tag.Id)
							{
								case TagId.StartActionsStart:
								case TagId.StartActionsEnd:
									break;
								case TagId.PushButtonEnd:
									bDone = true;
									break;
								case TagId.JumpTo:
									UInt32ArrayTag tag32 = (UInt32ArrayTag)tag;
									Debug.Assert(tag32.Value.Length == 2);
									m_dwJumpVals = tag32.Value;
									break;
								default:
									Debug.WriteLine("Unhandled button tag: " + tag.ToString());
									break;
							}
							if (!bDone)
								idx++;
						}
						break;
					default:
						Debug.Assert(false);
						break;
				}
			}
		}

		public override string ToString()
		{
			ParseButtonData();

			StringBuilder sb = new StringBuilder();

			sb.AppendFormat("{0} (Id = {1})\n", this.GetType().Name, ID);
			sb.AppendLine("   Type: " + m_eType.ToString());
			sb.AppendLine("   Flags: 0x" + m_wFlags.ToString("x4"));
			sb.Append("   Jumps:");
			if (m_dwJumpVals == null)
			{
				sb.AppendLine(" <none>");
			}
			else
			{
				foreach (uint jval in m_dwJumpVals)
				{
					sb.Append(" " + jval);
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}
    }
}
