namespace OthelloGame.Domain.Interfaces;

public interface IBoard
{
    ICell[][] Grid { get; }
    int Size { get; }
}