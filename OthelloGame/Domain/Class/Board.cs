using OthelloGame.Domain.Interfaces;
namespace OthelloGame.Domain.Class;
public class Board : IBoard
{
    public ICell[][]Grid {get; }
    public int Size {get; }
    public Board(int size)
    {
        Size = size;
        Grid = new ICell[size][];

        for(int i = 0; i < size; i++)
        {
            Grid[i]=new ICell[size];
            for (int j = 0; j < size; j++)
            {
                Grid[i][j] = new Cell(i, j);
            }
        }
    }
}
