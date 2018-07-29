using System;

namespace ClassExamples.Chess
{
	[Flags]
	internal enum MoveType
	{
		NonCapture = 0x0001,
		Capture = 0x0002,
		Promotion = 0x0004,
		EnPassant = 0x0008,
		KingSideCastle = 0x0010,
		QueenSideCastle = 0x0020,
		Check = 0x0040,
		CheckMate = 0x0080,
		Draw = 0x0100,
		Default = 0x03
	}
}
