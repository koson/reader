using System;
using System.Collections.Generic;
using System.Text;

namespace BBeBLib
{
	/// <summary>
	/// TODO Not the correct location for this enumeration
	/// </summary>
	public enum TagType
	{
		Unknown,
		H1,
		H2,
		H3,
		H4,
		H5,
		H6,
		text,
		P,
		B,
		I,
		BR,
		SUP,
		SUB,
		IMG
	}

	// I believe this should be depricated
    public enum ObjectFlags
    {
        NONE = 0x0,
        COMPRESSED = 0x0100,
        SCRAMBLED = 0x0200,
        ENCRYPTED = 0x8000,
        PAGE_NUMBERS = 0x0081,
        PAGE_LAYOUT = 0x0082,
        TOC_51 = 0x0051
    }

	public enum StreamContents
	{
		Unknown = 0x0,
		JpegImage = 0x11,
		PngImage = 0x12,
		BmpImage = 0x13,
		GifImage = 0x14,
		TableOfContents = 0x51,
		PageNumbers = 0x81,
		PageLayout = 0x82
	}

    [Flags()]
    public enum StreamFormatFlags
    {
        None = 0x0,
        Compressed = 0x0100,	// (handled and assumed default)
        Scrambled = 0x0200,		// (not yet handled)
        Encrypted = 0x8000,		// (no idea how to handle)
    }    

    public enum ButtonType
    {
        BaseButton = 0,
        FocusinButton = 1,
        PushButton = 2,
        UpButton = 3,
		Unknown = 4
    }

    /// <summary>
    /// Object types. See http://www.sven.de/librie/Librie/LrfObject
    /// </summary>
    public enum ObjectType
    {
        ObjectStart = 0x00,
        PageTree = 0x01,
        Page = 0x02,
        Header = 0x03,
        Footer = 0x04,
        PageAtr = 0x05,
        Block = 0x06,
        BlockAtr = 0x07,
        MiniPage = 0x08,
        BlockList = 0x09,
        Text = 0x0a,
        TextAtr = 0x0b,
        Image = 0x0c,
        Canvas = 0x0d,
        ParagraphAtr = 0x0e,
        Invalid_0f = 0x0f,
        Invalid_10 = 0x10,
        ImageStream = 0x11,
        Import = 0x12,
        Button = 0x13,
        Window = 0x14,
        PopUpWin = 0x15,
        Sound = 0x16,
        PlaneStream = 0x17,
        Invalid_18 = 0x18,
        Font = 0x19,
        ObjectInfo = 0x1a,
        Invalid_1b = 0x1b,
        BookAtr = 0x1c,
        SimpleText = 0x1d,
        TOC = 0x1e,
        Invalid_1f = 0x1f
    }

	public enum BlockAlignment
	{
		Left = 1,
		Center = 4
	}

    /// <summary>
    /// The tag numbers (added to 0xF500). See http://www.sven.de/librie/Librie/LrfTag
    /// </summary>
    public enum TagId
    {
        ObjectStart = 0x00,
        ObjectEnd = 0x01,
        ObjectInfoLink = 0x02,
        Link = 0x03,
        StreamSize = 0x04,
        StreamStart = 0x05,
        StreamEnd = 0x06,

        OddHeaderId = 0x07,    // A Pointer to a header object ?
        EvenHeaderId = 0x08,    // A Pointer to a header object ?
        OddFooterId = 0x09,    // A Pointer to a header object ?
        EvenFooterId = 0x0A,    // A Pointer to a header object ?

        PageObjectIds = 0x0B,

        FontSize = 0x11,
        FontWidth = 0x12,
        FontEscapement = 0x13,
        FontOrientation = 0x14,
        FontWeight = 0x15,
        FontFacename = 0x16,
        TextColor = 0x17,
        TextBgColor = 0x18,
        WordSpace = 0x19,
        LetterSpace = 0x1A,
        BaseLineSkip = 0x1B,
        LineSpace = 0x1C,
        ParIndent = 0x1D,
        ParSkip = 0x1E,

        // Page Attributes
        TopMargin = 0x21,
        HeadHeight = 0x22,
        HeadSep = 0x23,
        OddSideMargin = 0x24,
        EvenSideMargin = 0x2c,
        TextHeight = 0x25,
        TextWidth = 0x26,
        FootSpace = 0x27,
        FootHeight = 0x28,
        SetEmptyView = 0x2a,    // 1 - show, 2 - empty
        PagePosition = 0x2b,    // 0 - any, 1 - upper, 2 0 lower
        BGImageName= 0x29,
        
		BlockAttrUnknown0 = 0x2e,
        BlockWidth = 0x31,
        BlockHeight = 0x32,
        BlockRule = 0x33,
        BlockAttrUnknown1 = 0x34,
        Layout = 0x35,
        BlockAttrUnknown3 = 0x36,
        BlockAttrUnknown4 = 0x37,
        BlockAttrUnknown5 = 0x38,
        BlockAttrUnknown6 = 0x39,

        FontUnknownZero = 0x3a,
        BlockAlignment = 0x3c,	// 1 = left, 4 = center
        FontUnknownTwo = 0x3d,
        FontUnknownThree = 0x3e,

        MiniPageHeight = 0x41,
        MiniPageWidth = 0x42,
        LocationY = 0x46,
        LocationX = 0x47,

        PutSound = 0x49,
        ImageRect = 0x4A,
        ImageSize = 0x4B,
        ImageStream = 0x4C,

        CanvasWidth = 0x51,
        CanvasHeight = 0x52,
        StreamFlags = 0x54,
        UnknownStr1 = 0x55,   // This may well be image name
        FontFileName = 0x59,
        UnknownStr3 = 0x5A,
        UnknownStr4 = 0x5D, //a string
        ViewPoint = 0x5B,
        PageList = 0x5C,
        FontFaceName = 0x5D,

        ButtonFlags = 0x61,
        BaseButtonStart = 0x62,
        BaseButtonEnd = 0x63,
        FocusinButtonStart = 0x64,
        FocusinButtonEnd = 0x65,
        PushButtonStart = 0x66,
        PushButtonEnd = 0x67,
        UpButtonStart = 0x68,
        UpButtonEnd = 0x69,
        StartActionsStart = 0x6A,
        StartActionsEnd = 0x6B,
        JumpTo = 0x6C,
        SendMessage = 0x6D,
        CloseWindow = 0x6E,

        RuledLine = 0x73,
        RubyAlign = 0x75,
        RubyOverhang = 0x76,
        EmpDotsPosition = 0x77,
        EmpDotsCode = 0x78,
        EmpLinePosition = 0x79,
        EmpLineMode = 0x7A,
        ChildPageTree = 0x7B,
        ParentPageTree = 0x7C,

        ItalicBegin = 0x81,
        ItalicEnd,

        BeginPage = 0xA1,
        EndPage = 0xA2,
        KomaGaiji = 0xA5,
        KomaEmpDotChar = 0xA6,
        BeginButton = 0xA7,
        EndButton = 0xA8,
        BeginRuby = 0xA9,
        EndRuby = 0xAA,
        BeginRubyBase = 0xAB,
        EndRubyBase = 0xAC,
        BeginRubyText = 0xAD,
        EndRubyText = 0xAE,

        KomaYokomoji = 0xB1,

        TateBegin = 0xB3,
        TateEnd = 0xB4,
        NekaseBegin = 0xB5,
        NekaseEnd = 0xB6,
        BeginSup = 0xB7,
        EndSup = 0xB8,
        BeginSub = 0xB9,
        EndSub = 0xBA,

		BeginEmpLine = 0xC1,
		EndEmpLine = 0xC2,

        BeginDrawChar = 0xC3,
        EndDrawChar = 0xC4,

        KomaAutoSpacing = 0xC8,

        Space = 0xCA,

        KomaPlot = 0xD1,
        EOL = 0xD2,
        Wait = 0xD4,
        SoundStop = 0xD6,
        MoveObj = 0xD7,
        BookFont = 0xD8,
        KomaPlotText = 0xD9,
        SetWaitProp = 0xDA,         // 1 = replay, 2 = noreplay
        CharSpace = 0xDD,

        LineWidth = 0xF1,
        LineColor = 0xF2,
        FillColor = 0xF3,
        LineMode = 0xF4,
        MoveTo = 0xF5,
        LineTo = 0xF6,
        DrawBox = 0xF7,
        DrawEllipse = 0xF8,

        Run = 0xF9,

		// All of the pseudo tag values are greater than 0xff

		Text = 0x100,			// This isn't a real tag value, It's a phony value for the TextTag object.
		StreamGroup = 0x101,	// For the StreamTagGroup pseudo tag
    };
}
