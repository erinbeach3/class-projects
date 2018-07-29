using ClassExamples.Chess.Board;
using System;
using System.Collections.Generic;

namespace ClassExamples.Chess.Pieces
{
	public abstract class ChessPiece
	{
		private ChessSquare _currentSquare = ChessSquare.None;

		protected ChessPiece(Hue hue)
		{
			Hue = hue;
		}

		public Hue Hue { get; private set; }
		public abstract PieceType Type { get; }
		public string Name => $"{Hue} {Type}";
		public bool IsCaptured { get; internal set; }
		public int MoveCount { get; private set; }
		public virtual ChessSquare CurrentSquare
		{
			get => _currentSquare;
			internal set
			{
				if (!ChessBoard.IsOnBoard(value))
					throw new Exception($"Illegal Square: {value.Number}");
				if (!_currentSquare.IsNone) MoveCount++;
				_currentSquare = value;
			}
		}

		internal abstract IEnumerable<ChessMove> GetValidMoves();
		public override string ToString() => Name;
	}
}
