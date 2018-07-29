using ClassExamples.Chess.Pieces;
using System;
using System.Collections.Generic;

namespace ClassExamples.Chess.Board
{
	public class ChessBoard
	{
		public const int SquareCount = 64;

		public static ChessBoard EmptyBoard() => new ChessBoard();

		public static ChessBoard NewGameBoard()
		{
			ChessBoard board = new ChessBoard();
			void addPieces(Rank rank, Hue hue)
			{
				board.PlacePiece(rank, File.A, new Rook(hue));
				board.PlacePiece(rank, File.H, new Rook(hue));
				board.PlacePiece(rank, File.B, new Knight(hue));
				board.PlacePiece(rank, File.G, new Knight(hue));
				board.PlacePiece(rank, File.C, new Bishop(hue));
				board.PlacePiece(rank, File.F, new Bishop(hue));
				board.PlacePiece(rank, File.D, new Queen(hue));
				board.PlacePiece(rank, File.E, new King(hue));
			}
			void AddPawns(Rank rank, Hue hue)
			{
				for (int i = 0; i < 8; ++i) board.PlacePiece(rank, (File)i, new Pawn(hue));
			}
			addPieces(Rank.One, Hue.Light);
			AddPawns(Rank.Two, Hue.Light);
			addPieces(Rank.Eight, Hue.Dark);
			AddPawns(Rank.Seven, Hue.Dark);
			return board;
		}

		private ChessSquare[] _squares;
		private ChessBoard()
		{
			_squares = new ChessSquare[64];
			for (int i = 0; i < SquareCount; ++i) _squares[i] = new ChessSquare(i);
		}

		public ChessSquare this[Rank rank, File file]
		{
			get
			{
				return _squares[SquareFromRankAndFile(rank, file)];
			}
		}
		public int PieceCount
		{
			get
			{
				int count = 0;
				for (int i = 0; i < _squares.Length; ++i) if (_squares[i].IsOccupied) count++;
				return count;
			}
		}

		public void PlacePiece(Rank rank, File file, ChessPiece piece)
		{
			int square = SquareFromRankAndFile(rank, file);
			_squares[square].Piece = piece;
		}

		#region Internal Static Helper Methods

		private const string FILES = "ABCDEFG";

		/* 
		 * These methods are non-public because the integer-based implementation of a chess board is irrelevant to end-users.
		 * End-users only see Rank (1 - 8) and File (A - G) via the ChessSquare.
		 * 
		*/

		internal static bool IsOnBoard(int square) => (square >= 0 && square < 64);
		internal static bool IsOnBoard(int rank, int file) => (rank >= 0 && rank < 8) && (file >= 0 && file < 8);
		internal static bool IsOnBoard(ChessSquare square) => IsOnBoard(square.Number);

		internal static string SquareName(int square)
		{
			if (!IsOnBoard(square)) throw new ArgumentException("Invalid Chess Square");
			int rank = RankOf(square), file = FileOf(square);
			return $"{rank + 1}{file}";
		}

		internal static IEnumerable<int> ExtendFromSquare(int startSquare, int deltaRank, int deltaFile, bool repeats = false)
		{
			int rank = RankOf(startSquare), file = FileOf(startSquare);
			do
			{
				rank += deltaRank;
				file += deltaFile;
				int destSquare = SquareFromRankAndFile(rank, file);
				if (IsOnBoard(rank, file)) yield return SquareFromRankAndFile(rank, file); else break;
			} while (repeats);
		}
		
		internal static int RankOf(int square) => square / 8;
		internal static int FileOf(int square) => square % 8;
		internal static bool RankAndFileOf(int square, out int rank, out int file)
		{
			rank = file = -1;
			if (!IsOnBoard(square)) return false;
			rank = RankOf(square);
			file = FileOf(square);
			return true;
		}

		internal static int SquareFromRankAndFile(Rank rank, File file)
		{
			return 8 * (int)rank + (int)file;
		}

		internal static int SquareFromRankAndFile(int rank, int file)
		{
			return 8 * rank + file;
		}

		internal static Hue HueOf(int square)
		{
			int r = RankOf(square), f = FileOf(square);
			bool reven = (r % 2) == 0, feven = (f % 2) == 0;
			return reven == feven ? Hue.Dark : Hue.Light;
		}

		#endregion
	}
}
