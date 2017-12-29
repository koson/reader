using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib.Serializer
{
	public class CharacterMapper
	{
		Dictionary<char, char> m_Map = new Dictionary<char, char>();

		/// <summary>
		/// Add a character mapping.
		/// </summary>
		/// <param name="src">The original source character.</param>
		/// <param name="dst">The destination character to which src maps.</param>
		public void Add(char src, char dst)
		{
			m_Map[src] = dst;
		}

		/// <summary>
		/// Return the destination character to which the src character maps.
		/// If there isn't a specific mapping for the input character then it
		/// is returned unchanged.
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public char GetMap(char src)
		{
			return m_Map.ContainsKey(src) ? m_Map[src] : src;
		}
	}
}
