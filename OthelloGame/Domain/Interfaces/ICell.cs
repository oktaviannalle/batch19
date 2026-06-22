namespace OthelloGame.Domain.Interfaces;

public interface ICell
{
    int Row { get; }
    int Column { get; }
    IPiece? Piece { get; set; }
    bool IsEmpty { get; }
}