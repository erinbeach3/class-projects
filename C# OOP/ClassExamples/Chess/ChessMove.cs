using ClassExamples.Chess.Board;
using ClassExamples.Chess.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace ClassExamples.Chess
{

	internal class ChessMove
	{
		internal ChessMove(int startSquare, int destSquare, MoveType type = MoveType.Default):
			this(new ChessSquare(startSquare), new ChessSquare(destSquare), type)
		{	}

		internal ChessMove(ChessSquare startSquare, int destSquare, MoveType type = MoveType.Default):
			this(startSquare, new ChessSquare(destSquare), type)
		{ }

		internal ChessMove(ChessSquare startSquare, ChessSquare destSquare, MoveType type = MoveType.Default)
		{
			StartSquare = startSquare;
			DestinationSquare = destSquare;
			MoveType = type;
		}

		internal ChessSquare StartSquare { get; private set; }
		internal ChessSquare DestinationSquare { get; private set; }
		internal MoveType MoveType { get; set; }
		internal PieceType? PromoteTo { get; set; }

		public override string ToString()
		{
			// Apply chess notation rules
			if (MoveType == MoveType.Default) return $"{StartSquare} {DestinationSquare}";
			if (MoveType.HasFlag(MoveType.Capture))
			{
				string r = $"{StartSquare}x{DestinationSquare}";
				if (MoveType.HasFlag(MoveType.EnPassant)) r = r + "e.p.";
				return r;
			}
			if (MoveType.HasFlag(MoveType.KingSideCastle)) return "O-O";
			if (MoveType.HasFlag(MoveType.QueenSideCastle)) return "O-O-O";
			if (MoveType.HasFlag(MoveType.Promotion) && PromoteTo.HasValue)
			{
				char to = PromoteTo.Value.ToString()[0];
				return $"{StartSquare} {DestinationSquare}({to})";
			}
			// Maybe .. throw an exception to alert us to unhandled situations.
			return base.ToString();
		}

		#region Enumerations of Piece Moves

		internal static IEnumerable<ChessMove> BishopMoves(int startSquare)
		{
			return DiagonalMoves(startSquare, true);
		}

		internal static IEnumerable<ChessMove> RookMoves(int startSquare)
		{
			return RectangularMoves(startSquare, true);
		}

		internal static IEnumerable<ChessMove> QueenMoves(int startSquare)
		{
			return BishopMoves(startSquare).Concat(RookMoves(startSquare));
		}

		internal static IEnumerable<ChessMove> KingMoves(int startSquare)
		{
			return DiagonalMoves(startSquare, false).Concat(RectangularMoves(startSquare, false));
		}

		private static IEnumerable<ChessMove> DiagonalMoves(int startSquare, bool repeat)
		{
			return ChessBoard.ExtendFromSquare(startSquare, 1, 1, repeat)
				.Concat(ChessBoard.ExtendFromSquare(startSquare, 1, -1, repeat))
				.Concat(ChessBoard.ExtendFromSquare(startSquare, -1, 1, repeat))
				.Concat(ChessBoard.ExtendFromSquare(startSquare, -1, -1, repeat))
				.Select(sq => new ChessMove(startSquare, sq));
		}

		private static IEnumerable<ChessMove> RectangularMoves(int startSquare, bool repeat)
		{
			return ChessBoard.ExtendFromSquare(startSquare, 1, 0, repeat)
				.Concat(ChessBoard.ExtendFromSquare(startSquare, -1, 0, repeat))
				.Concat(ChessBoard.ExtendFromSquare(startSquare, 0, 1, repeat))
				.Concat(ChessBoard.ExtendFromSquare(startSquare, 0, -1, repeat))
				.Select(sq => new ChessMove(startSquare, sq));
		}

		#endregion
	}
}
