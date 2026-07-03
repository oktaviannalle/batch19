    using System;
    using System.Collections.Generic;
    using Serilog;
    using OthelloGame.Domain.Enum;
    using OthelloGame.Domain.Interfaces;
    using OthelloGame.Domain;

    public class GameController
    {
        private readonly IBoard _board;
        private readonly IReadOnlyList<IPlayer> _players;
        private readonly Func<PieceColor, IPiece> _pieceGenerator; 
        
        private int _currentPlayerIndex;
        private GameStatus _status;

        private static readonly IReadOnlyList<Position> _directions = new List<Position>
        {
            new Position(-1, -1), new Position(-1, 0), new Position(-1, 1),
            new Position( 0, -1),                      new Position( 0,  1),
            new Position( 1, -1), new Position( 1, 0), new Position( 1,  1)
        };

        public IBoard Board => _board;
        public IPlayer CurrentPlayer => _players[_currentPlayerIndex];
        public GameStatus GameStatus => _status;

        public event Action<IPlayer>? OnTurnSkipped;
        public event Action<IPlayer?>? OnGameOver;

        public GameController(IReadOnlyList<IPlayer> players, IBoard board, Func<PieceColor, IPiece> pieceGenerator)
        {
            _players = players;
            _board = board;
            _pieceGenerator = pieceGenerator;
            _status = GameStatus.NotStarted;
        }

        public void StartGame()
        {
            _status = GameStatus.InProgress;

            if (_players[0].Color == PieceColor.Black)
            {
                _currentPlayerIndex = 0;
            }
            else
            {
                _currentPlayerIndex = 1;
            }
            Log.Information("Game Othello dimulai. Pemain pertama yang jalan: {FirstPlayerColor}", CurrentPlayer.Color);
        }

        public bool PlayTurn(Position position)
        {
            if (_status != GameStatus.InProgress)
            {
                return false;
            }
            Log.Debug("Pemain {PlayerColor} mencoba menaruh bidak di koordinat Row: {Row}, Column: {Column}", CurrentPlayer.Color, position.Row, position.Column);
            
            if (!IsValidMove(position, CurrentPlayer.Color))
            {
                Log.Warning("Langkah DITOLAK! Pemain {PlayerColor} gagal menaruh bidak di Row: {Row}, Column: {Column}", CurrentPlayer.Color, position.Row, position.Column);
                return false;
            }

            PlacePiece(position, CurrentPlayer.Color);
            FlipPieces(position, CurrentPlayer.Color);

            Log.Information("Langkah SAH. Pemain {PlayerColor} menaruh bidak di Row: {Row}, Column: {Column}", CurrentPlayer.Color, position.Row, position.Column);
            if (IsGameOver())
            {
                _status = GameStatus.Finished;
                OnGameOver?.Invoke(GetWinner());
                return true;
            }

            SwitchPlayer();

            if (!HasValidMoves(CurrentPlayer.Color))
            {
                OnTurnSkipped?.Invoke(CurrentPlayer);
                SwitchPlayer();

                if (!HasValidMoves(CurrentPlayer.Color))
                {
                    _status = GameStatus.Finished;
                    OnGameOver?.Invoke(GetWinner());
                }
            }

            return true;
        }

        public IReadOnlyList<Position> GetValidMoves(PieceColor color)
        {
            List<Position> moves = new List<Position>();
            for (int row = 0; row < _board.Size; row++)
            {
                for (int column = 0; column < _board.Size; column++)
                {
                    Position pos = new Position(row, column);
                    if (IsValidMove(pos, color))
                    {
                        moves.Add(pos);
                    }
                }
            }
            return moves;
        }

        public int GetScore(IPlayer player)
        {
            return CountPieces(player.Color);
        }

        public bool IsValidMove(Position position, PieceColor color)
        {
            if (!IsInBounds(position))
            {
                return false;
            }
            if (!_board.Grid[position.Row][position.Column].IsEmpty)
            {
                return false;
            }
            
            return GetFlippablePositions(position, color).Count > 0;
        }

        public IPlayer? GetWinner()
        {
            int hitam = CountPieces(PieceColor.Black);
            int putih = CountPieces(PieceColor.White);

            if (hitam == putih)
            {
                return null;
            }

            PieceColor warnaMenang = hitam > putih ? PieceColor.Black : PieceColor.White;
            
            foreach (IPlayer p in _players)
            {
                if (p.Color == warnaMenang)
                {
                    return p;
                }
            }

            return null;
        }

        private void SwitchPlayer()
        {
            _currentPlayerIndex = 1 - _currentPlayerIndex;
        }

        private bool HasValidMoves(PieceColor color)
        {
            return GetValidMoves(color).Count > 0;
        }

        private bool IsGameOver()
        {
            return !HasValidMoves(PieceColor.Black) && !HasValidMoves(PieceColor.White);
        }

        private bool IsInBounds(Position p)
        {
            return p.Row >= 0 && p.Row < _board.Size && p.Column >= 0 && p.Column < _board.Size;
        }

        private PieceColor Lawan(PieceColor color)
        {
            if (color == PieceColor.Black)
            {
                return PieceColor.White;
            }
            return PieceColor.Black;
        }

        private void PlacePiece(Position position, PieceColor color)
        {
            _board.Grid[position.Row][position.Column].Piece = _pieceGenerator(color);
        }

        private void FlipPieces(Position position, PieceColor color)
        {
            foreach (Position pos in GetFlippablePositions(position, color))
            {
                _board.Grid[pos.Row][pos.Column].Piece = _pieceGenerator(color);
            }
        }

        private IReadOnlyList<Position> GetFlippablePositions(Position position, PieceColor color)
        {
            List<Position> semua = new List<Position>();
            foreach (Position arah in _directions)
            {
                semua.AddRange(GetFlippableInDirection(position, arah, color));
            }
            return semua;
        }

        private IReadOnlyList<Position> GetFlippableInDirection(Position position, Position arah, PieceColor color)
        {
            List<Position> kandidat = new List<Position>();
            PieceColor lawan = Lawan(color);

            int row = position.Row + arah.Row;
            int column = position.Column + arah.Column;

            while (IsInBounds(new Position(row, column)) &&
                !_board.Grid[row][column].IsEmpty &&
                _board.Grid[row][column].Piece!.Color == lawan)
            {
                kandidat.Add(new Position(row, column));
                row += arah.Row;
                column += arah.Column;
            }

            if (kandidat.Count > 0 &&
                IsInBounds(new Position(row, column)) &&
                !_board.Grid[row][column].IsEmpty &&
                _board.Grid[row][column].Piece!.Color == color)
            {
                return kandidat;
            }

            return new List<Position>();
        }

        private int CountPieces(PieceColor color)
        {
            int count = 0;
            for (int row = 0; row < _board.Size; row++)
            {
                for (int column = 0; column < _board.Size; column++)
                {
                    IPiece? piece = _board.Grid[row][column].Piece;
                    if (piece != null && piece.Color == color)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }