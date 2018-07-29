using ClassExamples.Chess.Board;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassExamples.UnitTests.Chess
{
	[TestClass]
	public class BoardTest
	{
		[TestMethod]
		public void PieceCount()
		{
			ChessBoard board = ChessBoard.EmptyBoard();
			Assert.AreEqual(0, board.PieceCount, "empty board");
			board = ChessBoard.NewGameBoard();
			Assert.AreEqual(32, board.PieceCount, "new game board");
		}
	}
}
