using System.Collections.Generic;

namespace ClassExamples.Chess.Pieces
{
	public class Bishop : ChessPiece
	{
		public Bishop(Hue hue): base(hue) { }

		public override PieceType Type => PieceType.Bishop;

		internal override IEnumerable<ChessMove> GetValidMoves() => ChessMove.BishopMoves((int)CurrentSquare);
	}
}
