using System;
using System.Collections.Generic;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using OthelloGame.Domain.Class;
using OthelloGame.Domain;

Console.Clear();
Console.WriteLine("=== SELAMAT DATANG DI GAME OTHELLO ===");

IPlayer player1 = new Player("Player 1 (Hitam)", PieceColor.Black);
IPlayer player2 = new Player("Player 2 (Putih)", PieceColor.White);
List<IPlayer> players = new List<IPlayer> { player1, player2 };

IBoard board = new Board(8);
GameController controller = new GameController(players, board);

bool isTurnSkipped = false;
controller.OnTurnSkipped += (IPlayer player) =>
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"\n[INFO] {player.Name} tidak memiliki langkah sah. Giliran dilewati.");
    Console.ResetColor();
    isTurnSkipped = true;
};

controller.OnGameOver += (IPlayer? winner) =>
{
    Console.Clear();
    Console.WriteLine("=== PERMAINAN SELESAI ===");
    DrawConsoleBoard(controller.Board);

    int skorHitamAkhir = controller.GetScore(players[0]);
    int skorPutihAkhir = controller.GetScore(players[1]);

    Console.WriteLine("====================================");
    Console.WriteLine($"SKOR AKHIR  |  Hitam: {skorHitamAkhir}  |  Putih: {skorPutihAkhir}");
    Console.WriteLine("====================================");

    if (winner != null)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        string warnaPemenang = winner.Color == PieceColor.Black ? "Hitam" : "Putih";
        int skorPemenang = controller.GetScore(winner);
        int skorKalah = winner.Color == PieceColor.Black ? skorPutihAkhir : skorHitamAkhir;
        Console.WriteLine($"SELAMAT! {winner.Name} MENANG!");
        Console.WriteLine($"dengan skor {skorPemenang} berbanding {skorKalah}");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"DRAW! Kedua pemain seimbang dengan skor {skorHitamAkhir} berbanding {skorPutihAkhir}");
    }

    Console.ResetColor();
    Console.WriteLine("====================================");
};

controller.StartGame();

while (controller.GameStatus == GameStatus.InProgress)
{
    Console.Clear();
    Console.WriteLine("=== GAME OTHELLO ===");
    DrawConsoleBoard(controller.Board);

    int skorHitam = controller.GetScore(players[0]);
    int skorPutih = controller.GetScore(players[1]);
    Console.WriteLine("------------------------------------");
    Console.WriteLine($"SKOR SEMENTARA | Hitam: {skorHitam} | Putih: {skorPutih}");
    Console.WriteLine("------------------------------------");

    Console.WriteLine($"\nGiliran: {controller.CurrentPlayer.Name}");

    IReadOnlyList<Position> validMoves = controller.GetValidMoves(controller.CurrentPlayer.Color);

    Console.WriteLine("\nLangkah tersedia:");
    for (int i = 0; i < validMoves.Count; i++)
    {
        Console.WriteLine($"  {i + 1}. Baris {validMoves[i].Row}, Kolom {validMoves[i].Column}");
    }

    Console.Write("\nPilih nomor langkah: ");
    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input)) continue;

    if (int.TryParse(input, out int pilihan) && pilihan >= 1 && pilihan <= validMoves.Count)
    {
        Position targetPosition = validMoves[pilihan - 1];
        controller.PlayTurn(targetPosition);
        if (isTurnSkipped)
{
        Console.WriteLine("Tekan Enter untuk melanjutkan...");
        Console.ReadLine();
        isTurnSkipped = false;
}
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n[ERROR] Input tidak valid. Masukkan nomor yang sesuai.");
        Console.WriteLine("\n Tekan Enter Untuk Memasukkan Langkah yang Valid.");
        Console.ResetColor();
        Console.ReadLine();
    }
}
        Console.WriteLine("\nPermainan telah Selesai. Tekan Enter untuk keluar...");
        Console.ReadLine();

static void DrawConsoleBoard(IBoard board)
{
    Console.WriteLine("\n    0  1  2  3  4  5  6  7 ");
    Console.WriteLine("   =========================");

    for (int r = 0; r < board.Size; r++)
    {
        Console.Write($"{r} |");
        for (int c = 0; c < board.Size; c++)
        {
            IPiece? piece = board.Grid[r][c].Piece;
            if (piece == null)
                Console.Write(" . ");
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