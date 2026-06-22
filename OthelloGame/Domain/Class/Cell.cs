using OthelloGame.Domain.Interfaces;

namespace OthelloGame.Domain.Class;

public class Cell : ICell
{
    public int Row { get; }
    public int Column { get; }
    public IPiece? Piece { get; set; }
    public bool IsEmpty => Piece == null;

    public Cell(int row, int column)
    {
        Row = row;
        Column = column;
        Piece = null; 
    }
}