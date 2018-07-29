using ClassExamples.Chess.Board;
using System.Collections.Generic;
using System.Linq;

namespace ClassExamples.Chess.Pieces
{
	public class Knight : ChessPiece
	{
		public Knight(Hue hue): base(hue) { }

		public override PieceType Type => PieceType.Knight;

		internal override IEnumerable<ChessMove> GetValidMoves()
		{
			return ChessBoard.ExtendFromSquare((int)CurrentSquare, 2, 1)
				.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, 2, -1))
				.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, -2, 1))
				.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, -2, -1))
				.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, 1, 2))
				.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, -1, 2))
				.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, 1, -2))
				.Concat(ChessBoard.ExtendFromSquare((int)CurrentSquare, -1, -2))
				.Select(sq => new ChessMove((int)CurrentSquare, sq));
		}
	}
}
