using ClassExamples.Chess.Board;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassExamples.Chess.Pieces
{
	public class Pawn : ChessPiece
	{
		internal Pawn(Hue hue): 
			base(hue) { }

		public override PieceType Type => PieceType.Pawn;

		public override ChessSquare CurrentSquare
		{ get => base.CurrentSquare;
			internal set
			{
				bool illegal = (Hue == Hue.Light) ?
					value.Rank < Rank.Two : value.Rank > Rank.Seven;
				if (illegal) throw new Exception($"Illegal Square: {Name} {value}");
				base.CurrentSquare = value;
			}
		}

		internal override IEnumerable<ChessMove> GetValidMoves()
		{
			int dir = Hue == Hue.Light ? 1 : -1;
			var attackMoves = ChessBoard.ExtendFromSquare((int)CurrentSquare, dir, -1)
				.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, dir, 1))
				.Select(sq => new ChessMove(CurrentSquare, sq, MoveType.Capture));
			var moves = ChessBoard.ExtendFromSquare((int)CurrentSquare, dir, 0);
			if (MoveCount == 0) moves = moves.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, 2 * dir, 0));
			return attackMoves.Concat(moves.Select(sq => new ChessMove(CurrentSquare, sq)));
		}
	}
}
