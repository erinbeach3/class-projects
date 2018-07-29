using System.Collections.Generic;
using System.Linq;
using ClassExamples.Chess;
using ClassExamples.Chess.Board;
using ClassExamples.Chess.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMath = System.Math;

namespace ClassExamples.UnitTests.Chess
{
	[TestClass]
	public class ChessMoveTest
	{
		private static int DistanceFromEdge(int square)
		{
			return DistanceFromEdge(square, out int rdist, out int fdist);
		}

		private static int DistanceFromEdge(int square, out int rankDistance, out int fileDistance)
		{
			int distanceOf(int v) => SMath.Min(v, SMath.Abs(7 - v));
			int rank = ChessBoard.RankOf(square), file = ChessBoard.FileOf(square);
			rankDistance = distanceOf(rank);
			fileDistance = distanceOf(file);
			return SMath.Min(rankDistance, fileDistance);
		}

		private static int DistanceBetween(int square1, int square2)
		{
			ChessBoard.RankAndFileOf(square1, out int r1, out int f1);
			ChessBoard.RankAndFileOf(square2, out int r2, out int f2);
			return SMath.Max(SMath.Abs(r1 - r2), SMath.Abs(f1 - f2));
		}

		private static int MapToFirstQuadrant(int square)
		{
			int reflect(int n) => (n < 4) ? n : 7 - n;
			int rank = ChessBoard.RankOf(square), file = ChessBoard.FileOf(square);
			rank = reflect(rank);
			file = reflect(file);
			return ChessBoard.SquareFromRankAndFile(rank, file);
		}

		[TestMethod]
		public void RookMoves()
		{
			for (int i = 0; i < 64; ++i)
			{
				IEnumerable<ChessMove> rookMoves = ChessMove.RookMoves(i);
				Assert.AreEqual(14, rookMoves.Count(), $"14 moves from square {ChessBoard.SquareName(i)}");
				int rank = ChessBoard.RankOf(i), file = ChessBoard.FileOf(i);
				foreach (ChessMove cm in rookMoves)
				{
					int destRank = (int)cm.DestinationSquare.Rank, destFile = (int)cm.DestinationSquare.File;
					Assert.IsTrue(rank == destRank || file == destFile, $"correct rank/file @ {cm}");
				}
			}
		}

		[TestMethod]
		public void BishopMoves()
		{
			for (int i = 0; i < 64; ++i)
			{
				IEnumerable<ChessMove> bishopMoves = ChessMove.BishopMoves(i);
				int distanceFromEdge = DistanceFromEdge(i);
				int count = 7 + 2 * distanceFromEdge;
				Assert.AreEqual(count, bishopMoves.Count(), $"# bishop moves from {ChessBoard.SquareName(i)}");
				Hue hue = ChessBoard.HueOf(i);
				Assert.IsTrue(bishopMoves.All(cm => cm.DestinationSquare.Hue == hue), "Bishop stays on same color");
			}
		}

		[TestMethod]
		public void KingMoves()
		{
			for (int i = 0; i < 64; ++i)
			{
				IEnumerable<ChessMove> kingMoves = ChessMove.KingMoves(i);
				int dist = DistanceFromEdge(i, out int rdist, out int fdist);
				int nmoves = 0;
				switch (dist)
				{
					case 0:
						nmoves = (rdist > 0 || fdist > 0) ? 5 : 3;
						break;
					default: nmoves = 8; break;
				}
				Assert.AreEqual(nmoves, kingMoves.Count(), $"# King moves from {i}");
			}
		}

		[TestMethod]
		public void KnightMoves()
		{
			Knight knight = new Knight(Hue.Light);
			for(int i=0;i<64;++i)
			{
				knight.CurrentSquare = new ChessSquare(i);
				IEnumerable<ChessMove> moves = knight.GetValidMoves();
				Assert.IsTrue(moves.All(cm => DistanceBetween((int)cm.StartSquare, (int)cm.DestinationSquare) == 2));
				int sq = MapToFirstQuadrant(i), nmoves = 2;
				switch(sq)
				{
					case 0: break;
					case 1:
					case 8:	nmoves = 3; break;
					case 2:
					case 3:
					case 9:
					case 16:
					case 24: nmoves = 4; break;
					case 10:
					case 11:
					case 17:
					case 25: nmoves = 6; break;
					default:
						if (sq > 27) Assert.Fail($"Oops - bad mapping ({sq})");
						nmoves = 8;
						break;
				}
				Assert.AreEqual(nmoves, moves.Count(), $"# Knight moves @ {i}");
			}
		}

		[TestMethod]
		public void PawnMoves()
		{
			for(int file=0;file<8;++file)
			{
				Pawn p = new Pawn(Hue.Light);
				bool onEdge = file == 0 || file == 7;
				for(int rank=1;rank<8;++rank)
				{
					p.CurrentSquare = new ChessSquare(ChessBoard.SquareFromRankAndFile(rank, file));
					var moves = p.GetValidMoves();
					int expectedMoves = 3;
					if (rank == 1) expectedMoves++;
					if (onEdge) expectedMoves--;
					if (rank == 7) expectedMoves = 0;
					//Console.WriteLine($"{p.CurrentSquare} {moves.Count()}");
					Assert.AreEqual(expectedMoves, moves.Count());
				}
				p = new Pawn(Hue.Dark);
				for(int rank=6;rank>=0;--rank)
				{
					p.CurrentSquare = new ChessSquare(ChessBoard.SquareFromRankAndFile(rank, file));
					var moves = p.GetValidMoves();
					int expectedMoves = 3;
					if (rank == 6) expectedMoves++;
					if (onEdge) expectedMoves--;
					if (rank == 0) expectedMoves = 0;
					//Console.WriteLine($"{p.CurrentSquare} {moves.Count()}");
					Assert.AreEqual(expectedMoves, moves.Count());
				}
			}
		}

		[TestMethod]
		public void QueenMoves()
		{
			for(int i=0;i<64;++i)
			{
				var moves = ChessMove.QueenMoves(i);
				int distanceFromEdge = DistanceFromEdge(i);
				int count = 21 + 2 * distanceFromEdge;
				Assert.AreEqual(count, moves.Count());
			}
		}
	}
}
