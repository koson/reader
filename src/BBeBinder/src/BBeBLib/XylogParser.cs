#if false
/* 
 * Copyright (C) 2006, Chris Mumford cmumford@cmumford.com
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA 02111-1307, USA.
 */
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace BBeBLib
{
    public class XylogParser
    {
        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void MSXML_InitInstance();

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void MSXML_Release();

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern int XYP_Create();

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern int XYP_GetVersion();

        [DllImport("XylogParser", CallingConvention=CallingConvention.StdCall)]
        public static extern void XYP_SetOutputFD(int fd);

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void XYP_SetTextCompressionFlag(int flag);

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void XYP_SetLrfFileName(string fname);

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern string XYP_GetLrfFileName();

#if false // This isn't corect
        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void XYP_SetXmlFileName(string fname);
#endif

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void XYP_SetProducerName(string producer);

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void XYP_SetScrambleNumber(int scramble);

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void XYP_SetAnchorFileName(string fname1, string fname2);

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void XYP_ParseDocument(string filename);

        [DllImport("XylogParser", CallingConvention = CallingConvention.StdCall)]
        public static extern void XYP_Release();
    }
}
#endif