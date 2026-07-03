using OthelloGame.Domain.Interfaces;

namespace OthelloGame.Domain.Class;

public class Board : IBoard
{
    public ICell[][] Grid { get; }
    public int Size { get; }

    public Board(int size)
    {
        Size = size;
        Grid = new ICell[size][];

        for (int row = 0; row < size; row++)
        {
            Grid[row] = new ICell[size];
            for (int column = 0; column < size; column++)
            {
                Grid[row][column] = new Cell(row, column);
            }
        }
    }
}