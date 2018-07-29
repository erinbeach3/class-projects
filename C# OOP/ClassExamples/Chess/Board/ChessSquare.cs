using ClassExamples.Chess.Pieces;

namespace ClassExamples.Chess.Board
{
	public struct ChessSquare
	{
		internal static readonly ChessSquare None = new ChessSquare(-1);

		internal ChessSquare(int number)
		{
			Number = number;
			Piece = null;
		}

		internal int Number { get; private set; }
		internal bool IsNone => Number < 0;
		internal bool IsOccupied => Piece != null;
		internal ChessPiece Piece { get; set; }

		public Rank Rank => (Rank)ChessBoard.RankOf(Number);
		public File File => (File)ChessBoard.FileOf(Number);
		public Hue Hue => ChessBoard.HueOf(Number);

		public string Name => $"{Rank}{File}";

		public override string ToString() => Name;

		public override bool Equals(object obj)
		{
			if (obj is ChessSquare sq)
			{
				return sq.Number == Number;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Number;
		}

		public static explicit operator int(ChessSquare square) => square.Number;
	}
}
