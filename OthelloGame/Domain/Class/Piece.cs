using OthelloGame.Domain.Interfaces; // Agar kenal dengan IPiece
using OthelloGame.Domain.Enum;      // Agar kenal dengan PieceColor

namespace OthelloGame.Domain.Class;  // Sesuai dengan nama folder 'Class' kamu

public class Piece : IPiece
{
    public PieceColor Color { get; set; }

    public Piece(PieceColor color)
    {
        Color = color;
    }
}