using NUnit.Framework;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using OthelloGame.Domain;
using OthelloGame.Domain.Class;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OthelloGame.Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        [SetUp]
        public void SetupLogger()
        {
            // Wajib di-set supaya Log.Information/Debug/Warning di GameController
            // tidak error, dan supaya TestCorrelator bisa menangkap log-nya.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestCorrelator()
                .CreateLogger();
        }

        // -----------------------------------------------------------
        // Helper: bersihkan seluruh papan lalu isi ulang sesuai kebutuhan test
        // -----------------------------------------------------------
        private static void ClearBoard(IBoard board)
        {
            for (int r = 0; r < board.Size; r++)
                for (int c = 0; c < board.Size; c++)
                    board.Grid[r][c].Piece = null;
        }

        private static void SetPiece(IBoard board, int row, int col, PieceColor color)
        {
            board.Grid[row][col].Piece = new Piece(color);
        }

        private static void SetupStandardOpening(IBoard board)
        {
            ClearBoard(board);
            SetPiece(board, 3, 3, PieceColor.White);
            SetPiece(board, 3, 4, PieceColor.Black);
            SetPiece(board, 4, 3, PieceColor.Black);
            SetPiece(board, 4, 4, PieceColor.White);
        }

        private static (List<IPlayer> players, IPlayer black, IPlayer white) CreatePlayers()
        {
            IPlayer black = new Player("Andi", PieceColor.Black);
            IPlayer white = new Player("Budi", PieceColor.White);
            return (new List<IPlayer> { black, white }, black, white);
        }

        private static GameController NewController(IBoard board, List<IPlayer> players)
        {
            Func<PieceColor, IPiece> pieceGenerator = color => new Piece(color);
            return new GameController(players, board, pieceGenerator);
        }

        // =============================================================
        // StartGame
        // =============================================================

        [Test]
        public void StartGame_WhenPlayer0IsBlack_CurrentPlayerIsBlack()
        {
            Board board = new Board(8);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            controller.StartGame();

            Assert.That(controller.CurrentPlayer.Color, Is.EqualTo(PieceColor.Black));
            Assert.That(controller.GameStatus, Is.EqualTo(GameStatus.InProgress));
        }

        [Test]
        public void StartGame_WhenPlayer0IsWhite_CurrentPlayerStillBlack()
        {
            Board board = new Board(8);
            IPlayer white = new Player("Budi", PieceColor.White);
            IPlayer black = new Player("Andi", PieceColor.Black);
            List<IPlayer> players = new List<IPlayer> { white, black }; // player[0] = White
            GameController controller = NewController(board, players);

            controller.StartGame();

            Assert.That(controller.CurrentPlayer.Color, Is.EqualTo(PieceColor.Black));
        }

        [Test]
        public void StartGame_LogsInformationWithFirstPlayerColor()
        {
            using (TestCorrelator.CreateContext())
            {
                Board board = new Board(8);
                (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
                GameController controller = NewController(board, players);

                controller.StartGame();

                Serilog.Events.LogEvent? evt = TestCorrelator.GetLogEventsFromCurrentContext()
                    .FirstOrDefault(e => e.MessageTemplate.Text.Contains("Game Othello dimulai"));

                Assert.That(evt, Is.Not.Null);
                Assert.That(evt!.Level, Is.EqualTo(Serilog.Events.LogEventLevel.Information));
                Assert.That(evt.Properties.ContainsKey("FirstPlayerColor"), Is.True);
            }
        }

        // =============================================================
        // IsValidMove — semua branch
        // =============================================================

        [Test]
        public void IsValidMove_OutOfBounds_ReturnsFalse()
        {
            Board board = new Board(8);
            SetupStandardOpening(board);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.IsValidMove(new Position(-1, 0), PieceColor.Black), Is.False);
            Assert.That(controller.IsValidMove(new Position(8, 0), PieceColor.Black), Is.False);
            Assert.That(controller.IsValidMove(new Position(0, -1), PieceColor.Black), Is.False);
            Assert.That(controller.IsValidMove(new Position(0, 8), PieceColor.Black), Is.False);
        }

        [Test]
        public void IsValidMove_CellOccupied_ReturnsFalse()
        {
            Board board = new Board(8);
            SetupStandardOpening(board);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.IsValidMove(new Position(3, 3), PieceColor.Black), Is.False);
        }

        [Test]
        public void IsValidMove_NoFlippablePieces_ReturnsFalse()
        {
            Board board = new Board(8);
            SetupStandardOpening(board);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.IsValidMove(new Position(0, 0), PieceColor.Black), Is.False);
        }

        [Test]
        public void IsValidMove_ClassicOpeningMove_ReturnsTrue()
        {
            Board board = new Board(8);
            SetupStandardOpening(board);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.IsValidMove(new Position(2, 3), PieceColor.Black), Is.True);
        }

        // =============================================================
        // IsValidMove — cover branch GetFlippableInDirection yang tersisa
        // (arah diagonal, line kena edge, line berujung sel kosong, dst)
        // =============================================================

        [Test]
        public void IsValidMove_DiagonalDirection_ReturnsTrue()
        {
            Board board = new Board(8);
            ClearBoard(board);
            SetPiece(board, 0, 0, PieceColor.Black);
            SetPiece(board, 1, 1, PieceColor.White);
            SetPiece(board, 2, 2, PieceColor.White);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            // Dari (3,3) arah diagonal kiri-atas: White, White, lalu anchor Black di (0,0)
            Assert.That(controller.IsValidMove(new Position(3, 3), PieceColor.Black), Is.True);
        }

        [Test]
        public void PlayTurn_DiagonalCapture_FlipsCorrectPieces()
        {
            Board board = new Board(8);
            ClearBoard(board);
            SetPiece(board, 0, 0, PieceColor.Black);
            SetPiece(board, 1, 1, PieceColor.White);
            SetPiece(board, 2, 2, PieceColor.White);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);
            controller.StartGame();

            controller.PlayTurn(new Position(3, 3));

            Assert.That(board.Grid[1][1].Piece!.Color, Is.EqualTo(PieceColor.Black));
            Assert.That(board.Grid[2][2].Piece!.Color, Is.EqualTo(PieceColor.Black));
        }

        [Test]
        public void IsValidMove_OpponentLineReachesEdgeWithoutAnchor_ReturnsFalse()
        {
            Board board = new Board(8);
            ClearBoard(board);
            // Baris opponent lari sampai mentok tepi papan, tidak pernah ketemu anchor Black
            SetPiece(board, 0, 5, PieceColor.White);
            SetPiece(board, 0, 6, PieceColor.White);
            SetPiece(board, 0, 7, PieceColor.White);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.IsValidMove(new Position(0, 4), PieceColor.Black), Is.False);
        }

        [Test]
        public void IsValidMove_OpponentLineEndsAtEmptyCell_ReturnsFalse()
        {
            Board board = new Board(8);
            ClearBoard(board);
            // Baris opponent berakhir di sel KOSONG (bukan anchor, bukan edge)
            SetPiece(board, 1, 5, PieceColor.White);
            SetPiece(board, 1, 6, PieceColor.White);
            // (1,7) sengaja dibiarkan kosong -> bukan anchor
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.IsValidMove(new Position(1, 4), PieceColor.Black), Is.False);
        }

        [Test]
        public void IsValidMove_AdjacentCellIsOwnColor_NoOpponentBetween_ReturnsFalse()
        {
            Board board = new Board(8);
            ClearBoard(board);
            // Sel sebelah langsung = warna sendiri, tidak ada opponent sama sekali di antaranya
            SetPiece(board, 2, 3, PieceColor.Black);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.IsValidMove(new Position(2, 2), PieceColor.Black), Is.False);
        }

        // =============================================================
        // GetValidMoves & GetScore
        // =============================================================

        [Test]
        public void GetValidMoves_StandardOpening_Returns4MovesForBlack()
        {
            Board board = new Board(8);
            SetupStandardOpening(board);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            IReadOnlyList<Position> moves = controller.GetValidMoves(PieceColor.Black);

            Assert.That(moves.Count, Is.EqualTo(4));
        }

        [Test]
        public void GetScore_StandardOpening_Returns2ForEachColor()
        {
            Board board = new Board(8);
            SetupStandardOpening(board);
            (List<IPlayer> players, IPlayer black, IPlayer white) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.GetScore(black), Is.EqualTo(2));
            Assert.That(controller.GetScore(white), Is.EqualTo(2));
        }

        // =============================================================
        // GetWinner — semua branch
        // =============================================================

        [Test]
        public void GetWinner_Tie_ReturnsNull()
        {
            Board board = new Board(8);
            ClearBoard(board);
            SetPiece(board, 0, 0, PieceColor.Black);
            SetPiece(board, 0, 1, PieceColor.White);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.GetWinner(), Is.Null);
        }

        [Test]
        public void GetWinner_BlackHasMore_ReturnsBlackPlayer()
        {
            Board board = new Board(8);
            ClearBoard(board);
            SetPiece(board, 0, 0, PieceColor.Black);
            SetPiece(board, 0, 1, PieceColor.Black);
            SetPiece(board, 0, 2, PieceColor.White);
            (List<IPlayer> players, IPlayer black, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.GetWinner(), Is.EqualTo(black));
        }

        [Test]
        public void GetWinner_WhiteHasMore_ReturnsWhitePlayer()
        {
            Board board = new Board(8);
            ClearBoard(board);
            SetPiece(board, 0, 0, PieceColor.White);
            SetPiece(board, 0, 1, PieceColor.White);
            SetPiece(board, 0, 2, PieceColor.Black);
            (List<IPlayer> players, IPlayer _, IPlayer white) = CreatePlayers();
            GameController controller = NewController(board, players);

            Assert.That(controller.GetWinner(), Is.EqualTo(white));
        }

        // =============================================================
        // PlayTurn — branch dasar
        // =============================================================

        [Test]
        public void PlayTurn_GameNotStarted_ReturnsFalse()
        {
            Board board = new Board(8);
            SetupStandardOpening(board);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);
            // TIDAK panggil StartGame()

            Assert.That(controller.PlayTurn(new Position(2, 3)), Is.False);
        }

        [Test]
        public void PlayTurn_InvalidMove_ReturnsFalseAndLogsWarning()
        {
            using (TestCorrelator.CreateContext())
            {
                Board board = new Board(8);
                SetupStandardOpening(board);
                (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
                GameController controller = NewController(board, players);
                controller.StartGame();

                bool result = controller.PlayTurn(new Position(0, 0));

                Assert.That(result, Is.False);
                IEnumerable<Serilog.Events.LogEvent> logs = TestCorrelator.GetLogEventsFromCurrentContext();
                Assert.That(logs.Any(e => e.Level == Serilog.Events.LogEventLevel.Warning), Is.True);
            }
        }

        [Test]
        public void PlayTurn_ValidMove_PlacesPieceFlipsAndLogs()
        {
            using (TestCorrelator.CreateContext())
            {
                Board board = new Board(8);
                SetupStandardOpening(board);
                (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
                GameController controller = NewController(board, players);
                controller.StartGame();

                bool result = controller.PlayTurn(new Position(2, 3));

                Assert.That(result, Is.True);
                Assert.That(board.Grid[2][3].IsEmpty, Is.False);
                Assert.That(board.Grid[3][3].Piece!.Color, Is.EqualTo(PieceColor.Black)); // ke-flip

                IEnumerable<Serilog.Events.LogEvent> logs = TestCorrelator.GetLogEventsFromCurrentContext();
                Assert.That(logs.Any(e => e.Level == Serilog.Events.LogEventLevel.Debug), Is.True);
                Assert.That(logs.Any(e => e.Level == Serilog.Events.LogEventLevel.Information), Is.True);
            }
        }

        [Test]
        public void PlayTurn_ValidMove_SwitchesToNextPlayer()
        {
            Board board = new Board(8);
            SetupStandardOpening(board);
            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);
            controller.StartGame();

            controller.PlayTurn(new Position(2, 3));

            Assert.That(controller.CurrentPlayer.Color, Is.EqualTo(PieceColor.White));
        }

        // =============================================================
        // PlayTurn — skip-turn branch (lawan tidak punya valid move)
        // =============================================================

        [Test]
        public void PlayTurn_WhenOpponentHasNoValidMoves_RaisesOnTurnSkippedAndContinues()
        {
            Board board = new Board(8);
            ClearBoard(board);
            // Baris atas: Black bisa capture 3 White sekaligus dari (0,4)
            SetPiece(board, 0, 0, PieceColor.Black);
            SetPiece(board, 0, 1, PieceColor.White);
            SetPiece(board, 0, 2, PieceColor.White);
            SetPiece(board, 0, 3, PieceColor.White);
            // Setup kedua: memberi Black langkah lanjutan agar game TIDAK berakhir.
            // Sengaja ditaruh di tepi papan (kolom 0) supaya sisi "cermin"-nya
            // jatuh di luar papan (out of bounds) dan TIDAK memberi White langkah valid juga.
            SetPiece(board, 5, 0, PieceColor.Black);
            SetPiece(board, 5, 1, PieceColor.White);

            (List<IPlayer> players, IPlayer _, IPlayer white) = CreatePlayers();
            GameController controller = NewController(board, players);
            controller.StartGame(); // current = Black

            IPlayer? skippedPlayer = null;
            controller.OnTurnSkipped += p => skippedPlayer = p;

            bool result = controller.PlayTurn(new Position(0, 4));

            Assert.That(result, Is.True);
            Assert.That(skippedPlayer, Is.EqualTo(white));           // White dilewati
            Assert.That(controller.CurrentPlayer.Color, Is.EqualTo(PieceColor.Black)); // balik ke Black
            Assert.That(controller.GameStatus, Is.EqualTo(GameStatus.InProgress));      // belum selesai
        }

        // =============================================================
        // PlayTurn — game-over branch (papan penuh, tidak ada valid move)
        // =============================================================

        [Test]
        public void PlayTurn_WhenBoardBecomesFull_EndsGameAndRaisesOnGameOver()
        {
            Board board = new Board(8);
            ClearBoard(board);
            // Isi seluruh board pola papan-catur KECUALI (7,7),
            // sehingga (7,6)=White dan (7,5)=Black -> langkah terakhir di (7,7) sah (capture 7,6).
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (r == 7 && c == 7) continue; // biarkan kosong, ini langkah terakhir
                    SetPiece(board, r, c, (r + c) % 2 == 0 ? PieceColor.Black : PieceColor.White);
                }
            }

            (List<IPlayer> players, IPlayer _, IPlayer _) = CreatePlayers();
            GameController controller = NewController(board, players);
            controller.StartGame(); // current = Black

            IPlayer? winnerFromEvent = null;
            bool gameOverRaised = false;
            controller.OnGameOver += w => { gameOverRaised = true; winnerFromEvent = w; };

            bool result = controller.PlayTurn(new Position(7, 7));

            Assert.That(result, Is.True);
            Assert.That(controller.GameStatus, Is.EqualTo(GameStatus.Finished));
            Assert.That(gameOverRaised, Is.True);
            Assert.That(winnerFromEvent, Is.EqualTo(controller.GetWinner()));
        }
    }
}