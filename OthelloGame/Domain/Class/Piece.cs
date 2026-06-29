using OthelloGame.Domain.Interfaces; 
using OthelloGame.Domain.Enum;      

namespace OthelloGame.Domain.Class;  

public class Piece : IPiece
{
    public PieceColor Color { get; set; }

    public Piece(PieceColor color)
    {
        Color = color;
    }
}