using System.Collections.Generic;

namespace ClassExamples.Chess.Pieces
{
	public class King : ChessPiece
	{
		public King(Hue hue): base(hue) { }

		public override PieceType Type => PieceType.King;

		internal override IEnumerable<ChessMove> GetValidMoves() => ChessMove.KingMoves((int)CurrentSquare);
	}
}
