using OthelloGame.Domain.Enum;

namespace OthelloGame.Domain.Interfaces;

public interface IPiece
{
    PieceColor Color { get; set; }
}