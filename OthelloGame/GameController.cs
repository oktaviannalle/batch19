using System;
using System.Collections.Generic;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using OthelloGame.Domain.Class;
using OthelloGame.Domain;
using System.ComponentModel;
using System.Reflection.Metadata;

public class GameController
{
    //fields (Private Variables) (-)
    private readonly IBoard _board;
    private readonly IReadOnlyList<IPlayer> _players;
    private int _currentPlayerIndex;
    private GameStatus _status;

    private static readonly IReadOnlyList<Position> _directions = new List<Position> //menyimpan 8 arah untuk pengecekan pola
    {
        new Position(-1, -1), new Position(-1, 0), new Position(-1, 1),
        new Position (0, -1),                       new Position(0, 1),
        new Position(1, -1), new Position(1, 0),   new Position(1, 1)
    };

    // properties (Public Attributes) (+)
    public IBoard board => _board;
    public IReadOnlyList<IPlayer> Players => _players;
    public IPlayer CurrentPlayer => _players[_currentPlayerIndex];
    public GameStatus Status => _status;

    // events (+)
    public event Action<IReadOnlyList<Position>>? OnBoardChanged;
    public event Action<IPlayer>? OnTurnChanged;
    public event Action<IPlayer, Position>? OnMoveMade;
    public event Action<IPlayer>? OnTurnSkipped;
    public event Action<IPlayer?>? OnGameOver;

    // constructor (+)
    public GameController(IReadOnlyList<IPlayer> players, IBoard board)
    {
        _players = players;
        _board = board;
        _status = GameStatus.NotStarted;

        ValidatePlayers();
    }
    // public method (fungsi API) (+)
    public void StartGame()
    {
        _status = GameStatus.InProgress;
        InitializeBoard();
        PlaceInitialPieces();
        SetBlackAsFirstPlayer();

        OnTurnChanged?.Invoke(CurrentPlayer);
    }

    public bool IsValidMove(Position position, PieceColor color)
    {
        if (!IsInBounds(position)) return false;

        if (!_board.Grid[position.Row][position.Column].IsEmpty) return false;

        var flippable = GetFlippablePositions(position, color);
        return flippable.Count > 0;
    }

    public IReadOnlyList<Position> GetValidMoves(PieceColor color)
    {
        var validMoves = new List<Position>();

        for (int r = 0; r < _board.Size; r++)
        {
            for (int c = 0; c < _board.Size; c++) 
            {
                var pos = new Position(r, c);

                if (IsValidMove(pos, color))
                {
                    validMoves.Add(pos);
                }
            }
        }
        return validMoves;
    }

    public int GetScore(IPlayer player)
    {
        // Mengambil skor berdasarkan warna dari player tersebut
        return CountPieces(player.Color);
    }

    public bool PlayTurn(Position position)
    {
        if (_status != GameStatus.InProgress) return false;
        if (!IsValidMove(position, CurrentPlayer.Color)) return false;

        var changedPositions = new List<Position> { position };
        changedPositions.AddRange(GetFlippablePositions(position, CurrentPlayer.Color));

        PlacePiece(position, CurrentPlayer.Color);
        FlipPieces(position, CurrentPlayer.Color);

        OnMoveMade?.Invoke(CurrentPlayer, position);
        OnBoardChanged?.Invoke(changedPositions);

        if (IsGameOver())
        {
            FinishGame();
            return true;
        }
        
        SwitchPlayer();

        if (!HasValidMoves(CurrentPlayer.Color))
        {
            HandleSkippedTurn();
        }
        return true;
    }

    public IPlayer? GetWinner()
    {
        int blackScore = CountPieces(PieceColor.Black);
        int whiteScore = CountPieces(PieceColor.White);

        if (blackScore > whiteScore)
        {
            foreach (var p in _players) if (p.Color == PieceColor.Black) return p;
        }
        else if (whiteScore > blackScore)
        {
            foreach (var p in _players) if (p.Color == PieceColor.White) return p;
        }
        return null;
    }
    // private methods
    private void ValidatePlayers()
    {
        if (_players == null || _players.Count != 2)
        {
            throw new ArgumentException("Permainan Othello harus memiliki tepat 2 pemain.");
        }
        if (_players[0].Color == _players[1].Color)
        {
            throw new ArgumentException("Kedua Pemain tidak boleh memilih warna yang sama");
        }
    }

    private void SetBlackAsFirstPlayer()
    {
        if (_players[0].Color == PieceColor.Black)
        {
            _currentPlayerIndex = 0;
        }
        else
        {
            _currentPlayerIndex = 1;
        }    
    }

    private void InitializeBoard() //memastikan semua kotak kosong sebelum mulai
    {
        for (int r = 0; r < _board.Size; r++)
        {
            for (int c = 0; c < _board.Size; c++)
            {
                _board.Grid[r][c].Piece = null;
            }
        }
    }

    private void PlaceInitialPieces()
    {
        //posisi menyilang empat bidak awal
        _board.Grid[3][3].Piece = new Piece(PieceColor.White);
        _board.Grid[3][4].Piece = new Piece(PieceColor.Black);
        _board.Grid[4][3].Piece = new Piece(PieceColor.Black);
        _board.Grid[4][4].Piece = new Piece(PieceColor.White);
    }

    private bool IsInBounds(Position position)
    {
        return position.Row >= 0 && position.Row < _board.Size &&
                position.Column >= 0 && position.Column < _board.Size;
    }

    private PieceColor GetOpponentColor(PieceColor color)
    {
        if (color == PieceColor.Black)
        {
            return PieceColor.White;
        }
        else
        {
            return PieceColor.Black;
        }
    }

    private IReadOnlyList<Position> GetFlippablePositionsInDirection(Position position, Position direction, PieceColor color)
    {
        var flippable = new List<Position>();
        var opponentColor = GetOpponentColor(color);

        int currentRow = position.Row + direction.Row;
        int currentCol = position.Column + direction.Column;
        var currentPos = new Position(currentRow, currentCol);

        while (IsInBounds(currentPos) &&
                !_board.Grid[currentRow][currentCol].IsEmpty &&
                _board.Grid[currentRow][currentCol].Piece!.Color == opponentColor)
        {
            flippable.Add(currentPos);

            currentRow += direction.Row;
            currentCol += direction.Column;
            currentPos = new Position(currentRow, currentCol);
        }

        if (IsInBounds(currentPos) &&
            !_board.Grid[currentRow][currentCol].IsEmpty &&
            _board.Grid[currentRow][currentCol].Piece!.Color == color)
        {
            return flippable; 
        }
        return new List<Position>();
    }

    private IReadOnlyList<Position> GetFlippablePositions(Position position, PieceColor color)
    {
        var allFlippable = new List<Position>();

        foreach (var direction in _directions)
        {
            var flippableInDir = GetFlippablePositionsInDirection(position, direction, color);
            allFlippable.AddRange(flippableInDir);
        }
        return allFlippable;
    }

    private bool HasValidMoves(PieceColor color)
    {
        return GetValidMoves(color).Count > 0;
    }

    private int CountPieces(PieceColor color)
    {
        int count = 0;

        for (int r = 0; r < _board.Size; r++)
        {
            for (int c = 0; c < _board.Size; c++)
            {
                var piece = _board.Grid[r][c].Piece;
                if (piece != null && piece.Color == color)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void HandleSkippedTurn()
    {
        OnTurnSkipped?.Invoke(CurrentPlayer);
        SwitchPlayer();

        if (!HasValidMoves(CurrentPlayer.Color))
        {
            FinishGame();
        }
    }

    private bool IsGameOver()
    {
        return !HasValidMoves(PieceColor.Black) && !HasValidMoves(PieceColor.White);
    }

    private void FinishGame()
    {
        _status = GameStatus.Finished;
        OnGameOver?.Invoke(GetWinner());
    }

    private void PlacePiece(Position position, PieceColor color)
    {
        _board.Grid[position.Row][position.Column].Piece = new Piece(color);
    }

    private void FlipPieces(Position position, PieceColor color)
    {
        var flippablePositions = GetFlippablePositions(position, color);

        foreach (var pos in flippablePositions)
        {
            var piece = _board.Grid[pos.Row][pos.Column].Piece;
            if (piece != null)
            {
                piece.Color = color;
            }
        }
    }

    private void SwitchPlayer()
    {
        _currentPlayerIndex = 1 - _currentPlayerIndex;
        OnTurnChanged?.Invoke(CurrentPlayer);
    }
}