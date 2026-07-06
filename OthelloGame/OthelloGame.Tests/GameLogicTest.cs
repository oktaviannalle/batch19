using NUnit.Framework;
using OthelloGame.Domain; 
using OthelloGame.Domain.Class;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace OthelloGame.Tests
{
    [TestFixture]
    public class GameLogicTests
    {
        [Test]
        public void Position_WhenInitialized_ShouldStoreRowAndColumnCorrectly()
        {
            int expectedRow = 3;
            int expectedColumn = 4;

            Position pos = new Position(expectedRow, expectedColumn);

            Assert.That(pos.Row, Is.EqualTo(expectedRow), "Row tidak tersimpan dengan benar.");
            Assert.That(pos.Column, Is.EqualTo(expectedColumn), "Column tidak tersimpan dengan benar.");
        }

        [Test]
        public void GameController_WhenCreated_StatusShouldBeNotStarted()
        {
            IPlayer player1 = new Player("Player 1", PieceColor.Black);
            IPlayer player2 = new Player("Player 2", PieceColor.White);
            List<IPlayer> players = new List<IPlayer> { player1, player2 };
            
            IBoard board = new Board(8);
            Func<PieceColor, IPiece> pieceGenerator = (color) => new Piece(color);

            GameController controller = new GameController(players, board, pieceGenerator);

            Assert.That(controller.GameStatus, Is.EqualTo(GameStatus.NotStarted), "Status game awal harusnya NotStarted.");
        }

        // TES 3: Memastikan game dimulai dengan benar dan giliran pertama adalah Hitam
        [Test]
        public void StartGame_WhenCalled_ShouldSetStatusToInProgressAndBlackPlaysFirst()
        {
            // 1. Arrange
            IPlayer player1 = new Player("Player 1", PieceColor.Black);
            IPlayer player2 = new Player("Player 2", PieceColor.White);
            List<IPlayer> players = new List<IPlayer> { player1, player2 };
            IBoard board = new Board(8);
            Func<PieceColor, IPiece> pieceGenerator = (color) => new Piece(color);
            GameController controller = new GameController(players, board, pieceGenerator);

            // 2. Act
            controller.StartGame();

            // 3. Assert
            Assert.That(controller.GameStatus, Is.EqualTo(GameStatus.InProgress), "Status game harus berubah menjadi InProgress.");
            Assert.That(controller.CurrentPlayer.Color, Is.EqualTo(PieceColor.Black), "Pemain pertama (Hitam) harus menjadi giliran pertama.");
        }

        // TES 4: Memastikan PlayTurn menolak posisi di luar papan (Out of Bounds)
        [Test]
        public void PlayTurn_WhenPositionIsOutOfBounds_ShouldReturnFalse()
        {
            // 1. Arrange
            IPlayer player1 = new Player("Player 1", PieceColor.Black);
            IPlayer player2 = new Player("Player 2", PieceColor.White);
            List<IPlayer> players = new List<IPlayer> { player1, player2 };
            IBoard board = new Board(8);
            Func<PieceColor, IPiece> pieceGenerator = (color) => new Piece(color);
            GameController controller = new GameController(players, board, pieceGenerator);
            controller.StartGame();
    
            Position invalidPosition = new Position(9, 9); 

            // 2. Act
            bool result = controller.PlayTurn(invalidPosition);

            // 3. Assert
            Assert.That(result, Is.False, "Langkah di luar papan harus ditolak (return false).");
        }

        // TES 5: Memastikan PlayTurn menolak langkah saat status game bukan InProgress
        [Test]
        public void PlayTurn_WhenGameIsNotStarted_ShouldReturnFalse()
        {
            // 1. Arrange
            IPlayer player1 = new Player("Player 1", PieceColor.Black);
            IPlayer player2 = new Player("Player 2", PieceColor.White);
            List<IPlayer> players = new List<IPlayer> { player1, player2 };
            IBoard board = new Board(8);
            Func<PieceColor, IPiece> pieceGenerator = (color) => new Piece(color);
            GameController controller = new GameController(players, board, pieceGenerator);
            
            // Perhatikan: controller.StartGame() sengaja TIDAK dipanggil di sini

            Position validPosition = new Position(3, 2); 

            // 2. Act
            bool result = controller.PlayTurn(validPosition);

            // 3. Assert
            Assert.That(result, Is.False, "Tidak bisa menaruh bidak jika game belum dimulai.");
            
        }
    }
}