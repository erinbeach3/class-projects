using System.Collections.Generic;

namespace ClassExamples.Chess.Pieces
{
	public class Rook : ChessPiece
	{
		public Rook(Hue hue): base(hue) {  }

		public override PieceType Type => PieceType.Rook;

		internal override IEnumerable<ChessMove> GetValidMoves() => ChessMove.RookMoves((int)CurrentSquare);
	}
}
