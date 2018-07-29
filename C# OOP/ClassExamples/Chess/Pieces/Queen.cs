using System.Collections.Generic;

namespace ClassExamples.Chess.Pieces
{
	public class Queen : ChessPiece
	{
		public Queen(Hue hue): base(hue) {  }

		public override PieceType Type => PieceType.Queen;

		internal override IEnumerable<ChessMove> GetValidMoves() => ChessMove.QueenMoves((int)CurrentSquare);
	}
}
