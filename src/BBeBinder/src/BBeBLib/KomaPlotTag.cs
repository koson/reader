using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using BBeBLib.Serializer;


namespace BBeBLib
{
	public class KomaPlotTag : BBeBTag
	{
		ushort m_wHeight;
		ushort m_wWidth;
		uint m_dwRefObj;
		uint m_dwUnknown;


		public KomaPlotTag( ushort height, ushort width, uint refobj, uint unknown ) : base(TagId.KomaPlot)
		{
			m_wHeight = height;
			m_wWidth = width;
			m_dwRefObj = refobj;
			m_dwUnknown = unknown;
		}

		public byte[] Serialize()
		{
			MemoryStream stream = new MemoryStream();
			BBeBinaryWriter writer = new BBeBinaryWriter(stream);

			writer.Write(TagId.KomaPlot);
			writer.Write(m_wHeight);
			writer.Write(m_wWidth);
			writer.Write(m_dwRefObj);
			writer.Write(m_dwUnknown);
			writer.Flush();

			byte[] data = new byte[stream.Position];
			Array.Copy(stream.GetBuffer(), data, stream.Position);

			return data;
		}

		public override string ToString()
		{
			return string.Format("KomaPlot  width={0}, height={1}, refobj={2}, ??={3}",
				m_wWidth, m_wHeight, m_dwRefObj, m_dwUnknown);
		}
	}
}
