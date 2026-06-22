using OthelloGame.Domain.Interfaces;
using OthelloGame.Domain.Enum;     

namespace OthelloGame.Domain.Class;

public class Player : IPlayer
{
    public string Name { get; }
    public PieceColor Color { get; }

    public Player(string name, PieceColor color)
    {
        Name = name;
        Color = color;
    }
}