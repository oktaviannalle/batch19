using OthelloGame.Domain.Enum;

namespace OthelloGame.Domain.Interfaces;

public interface IPlayer
{
    string Name { get; }
    PieceColor Color { get; }
}