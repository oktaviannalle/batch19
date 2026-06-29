using System;
using System.Collections.Generic;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using OthelloGame.Domain.Class;
using OthelloGame.Domain;

public class GameController
{
    private readonly IBoard _board;
    private readonly IReadOnlyList<IPlayer> _players;
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
    public GameStatus GameStatus  => _status;

    public event Action<IPlayer>?  OnTurnSkipped;
    public event Action<IPlayer?>? OnGameOver;

    public GameController(IReadOnlyList<IPlayer> players, IBoard board)
    {
        if (players == null || players.Count != 2)
            throw new ArgumentException("Othello butuh tepat 2 pemain.");
        if (players[0].Color == players[1].Color)
            throw new ArgumentException("Kedua pemain tidak boleh memakai warna yang sama.");

        _players = players;
        _board   = board;
        _status  = GameStatus.NotStarted;
    }

    public void StartGame()
    {
        _status = GameStatus.InProgress;

        for (int r = 0; r < _board.Size; r++)
            for (int c = 0; c < _board.Size; c++)
                _board.Grid[r][c].Piece = null;

        _board.Grid[3][3].Piece = new Piece(PieceColor.White);
        _board.Grid[3][4].Piece = new Piece(PieceColor.Black);
        _board.Grid[4][3].Piece = new Piece(PieceColor.Black);
        _board.Grid[4][4].Piece = new Piece(PieceColor.White);

        _currentPlayerIndex = _players[0].Color == PieceColor.Black ? 0 : 1;
    }

    public bool PlayTurn(Position position)
    {
        if (_status != GameStatus.InProgress) return false;
        if (!IsValidMove(position, CurrentPlayer.Color)) return false;

        PlacePiece(position, CurrentPlayer.Color);
        FlipPieces(position, CurrentPlayer.Color);

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
        for (int r = 0; r < _board.Size; r++)
            for (int c = 0; c < _board.Size; c++)
            {
                Position pos = new Position(r, c);
                if (IsValidMove(pos, color)) moves.Add(pos);
            }
        return moves;
    }

    public int GetScore(IPlayer player) => CountPieces(player.Color);

    public bool IsValidMove(Position position, PieceColor color)
    {
        if (!IsInBounds(position)) return false;
        if (!_board.Grid[position.Row][position.Column].IsEmpty) return false;
        return GetFlippablePositions(position, color).Count > 0;
    }

    public IPlayer? GetWinner()
    {
        int hitam = CountPieces(PieceColor.Black);
        int putih = CountPieces(PieceColor.White);

        if (hitam == putih) return null;

        PieceColor warnaMenang = hitam > putih ? PieceColor.Black : PieceColor.White;
        foreach (IPlayer p in _players)
            if (p.Color == warnaMenang) return p;

        return null;
    }

    private void SwitchPlayer()
    {
        _currentPlayerIndex = 1 - _currentPlayerIndex;
    }

    private bool HasValidMoves(PieceColor color) => GetValidMoves(color).Count > 0;

    private bool IsGameOver() =>
        !HasValidMoves(PieceColor.Black) && !HasValidMoves(PieceColor.White);

    private bool IsInBounds(Position p) =>
        p.Row >= 0 && p.Row < _board.Size &&
        p.Column >= 0 && p.Column < _board.Size;

    private PieceColor Lawan(PieceColor color) =>
        color == PieceColor.Black ? PieceColor.White : PieceColor.Black;

    private void PlacePiece(Position position, PieceColor color)
    {
        _board.Grid[position.Row][position.Column].Piece = new Piece(color);
    }

    private void FlipPieces(Position position, PieceColor color)
    {
        foreach (Position pos in GetFlippablePositions(position, color))
        {
            _board.Grid[pos.Row][pos.Column].Piece = new Piece(color);
        }
    }
        private IReadOnlyList<Position> GetFlippablePositions(Position position, PieceColor color)
    {
        List<Position> semua = new List<Position>();
        foreach (Position arah in _directions)
            semua.AddRange(GetFlippableInDirection(position, arah, color));
        return semua;
    }
    private IReadOnlyList<Position> GetFlippableInDirection(
        Position position, Position arah, PieceColor color)
    {
        List<Position> kandidat = new List<Position>();
        PieceColor lawan = Lawan(color);

        int r = position.Row + arah.Row;
        int c = position.Column + arah.Column;

        while (IsInBounds(new Position(r, c)) &&
               !_board.Grid[r][c].IsEmpty &&
               _board.Grid[r][c].Piece!.Color == lawan)
        {
            kandidat.Add(new Position(r, c));
            r += arah.Row;
            c += arah.Column;
        }

        if (kandidat.Count > 0 &&
            IsInBounds(new Position(r, c)) &&
            !_board.Grid[r][c].IsEmpty &&
            _board.Grid[r][c].Piece!.Color == color)
            return kandidat;

        return new List<Position>();
    }
        private int CountPieces(PieceColor color)
    {
        int count = 0;
        for (int r = 0; r < _board.Size; r++)
            for (int c = 0; c < _board.Size; c++)
            {
                IPiece? piece = _board.Grid[r][c].Piece;
                if (piece != null && piece.Color == color) count++;
            }
        return count;
    }
}