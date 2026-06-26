using System;
using System.Collections.Generic;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using OthelloGame.Domain.Class;
using OthelloGame.Domain;
using System.Runtime.Serialization.Formatters;

Console.Clear();
Console.WriteLine("=== SELAMAT DATANG DI GAME OTHELLO ===");

IPlayer player1 = new Player("Player 1 (Hitam)", PieceColor.Black);
IPlayer player2 = new Player("Player 2 (Putih)", PieceColor.White);
var players = new List<IPlayer> { player1, player2 };

IBoard board = new Board(8);

GameController controller = new GameController(players, board);

controller.OnTurnChanged += (currentPlayer) =>
{
    Console.WriteLine($"\nGiliran: {currentPlayer.Name} [{currentPlayer.Color}]");
};
controller.OnMoveMade += (player, pos) =>
{
    Console.WriteLine($"[AKSI] {player.Name} menaruh bidak di ({pos.Row}, {pos.Column})");
};
controller.OnTurnSkipped += (player) =>
{
    
};
controller.OnGameOver += (winner) =>
{
    
};
controller.StartGame();
DrawConsoleBoard(controller.board);

while (controller.Status == GameStatus.InProgress)
{
    Console.Clear();
    Console.WriteLine("=== GAME OTHELLO ===");
    DrawConsoleBoard(controller.board);
    
    Console.WriteLine($"\nGiliran: {controller.CurrentPlayer.Name} [{controller.CurrentPlayer.Color}]");
    Console.Write("Masukkan langkah (Baris Kolom, contoh: 2 3): ");
    
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    string[] parts = input.Split(' ');
    if (parts.Length == 2 && int.TryParse(parts[0], out int r) && int.TryParse(parts[1], out int c))
    {
        var targetPosition = new Position(r, c);
        bool moveSuccessful = controller.PlayTurn(targetPosition);
        
        if (!moveSuccessful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n[ERROR] Langkah tidak sah atau kotak terisi! Tekan Enter untuk mengulang.");
            Console.ResetColor();
            Console.ReadLine(); 
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n[ERROR] Format salah! Ketik angka dipisah spasi. Tekan Enter untuk mengulang.");
        Console.ResetColor();
        Console.ReadLine();
    }
}


static void DrawConsoleBoard(IBoard board)
{
    Console.WriteLine("\n    0  1  2  3  4  5  6  7 ");
    Console.WriteLine("   =========================");

    for (int r = 0; r < board.Size; r++) 
    {
        Console.Write($"{r} |");
        for (int c =0; c <board.Size; c++)
        {
            var piece = board.Grid[r][c].Piece;
            if (piece == null)
            {
                Console.Write(" . ");
            }
            else if (piece.Color == PieceColor.Black)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" B ");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" W ");
                Console.ResetColor();
            }
        }
        Console.WriteLine(" | ");
    }
    Console.WriteLine("   =========================");
}