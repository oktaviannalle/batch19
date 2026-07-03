using System;
using System.Collections.Generic;
using Serilog;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using OthelloGame.Domain;

public class GameUI
{
    private readonly GameController _controller;
    private readonly IReadOnlyList<IPlayer> _players;
    private bool _isTurnSkipped = false;

    public GameUI(GameController controller, IReadOnlyList<IPlayer> players)
    {
        _controller = controller;
        _players = players;

        _controller.OnTurnSkipped += HandleTurnSkipped;
        _controller.OnGameOver += HandleGameOver;
    }

    public void Start()
    {
        Console.Clear();
        Console.WriteLine("=== SELAMAT DATANG DI GAME OTHELLO ===");

        _controller.StartGame();

        while (_controller.GameStatus == GameStatus.InProgress)
        {
            Console.Clear();
            Console.WriteLine("=== GAME OTHELLO ===");

            IReadOnlyList<Position> validMoves = _controller.GetValidMoves(_controller.CurrentPlayer.Color);

            DrawConsoleBoard(_controller.Board, validMoves);

            int skorHitam = _controller.GetScore(_players[0]);
            int skorPutih = _controller.GetScore(_players[1]);
            
            Console.WriteLine("------------------------------------");
            Console.WriteLine($"SKOR SEMENTARA | Hitam: {skorHitam} | Putih: {skorPutih}");
            Console.WriteLine("------------------------------------");

            Console.WriteLine($"\nGiliran: {_controller.CurrentPlayer.Name}");

            Console.WriteLine("\nLangkah tersedia:");
            for (int i = 0; i < validMoves.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. Baris {validMoves[i].Row}, Kolom {validMoves[i].Column}");
            }

            Console.Write("\nPilih nomor langkah: ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (int.TryParse(input, out int pilihan) && pilihan >= 1 && pilihan <= validMoves.Count)
            {
                Position targetPosition = validMoves[pilihan - 1];
                _controller.PlayTurn(targetPosition);
                
                if (_isTurnSkipped)
                {
                    Console.WriteLine("Tekan Enter untuk melanjutkan...");
                    Console.ReadLine();
                    _isTurnSkipped = false;
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
    }

    private void HandleTurnSkipped(IPlayer player)
    {
        Log.Warning("Giliran dilewati (Turn Skipped) untuk pemain: {PlayerName} karena tidak ada langkah sah", player.Name);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n[INFO] {player.Name} tidak memiliki langkah sah. Giliran dilewati.");
        Console.ResetColor();
        _isTurnSkipped = true;
    }

    private void HandleGameOver(IPlayer? winner)
    {
        Console.Clear();
        Console.WriteLine("=== PERMAINAN SELESAI ===");

        DrawConsoleBoard(_controller.Board, new List<Position>());

        int skorHitamAkhir = _controller.GetScore(_players[0]);
        int skorPutihAkhir = _controller.GetScore(_players[1]);

        string namaPemenang = winner != null ? winner.Name : "DRAW/SERI";
        Log.Information("Game Over! Pemenang: {WinnerName} | Skor Akhir - Hitam: {SkorHitam}, Putih: {SkorPutih}", namaPemenang, skorHitamAkhir, skorPutihAkhir);

        Console.WriteLine("====================================");
        Console.WriteLine($"SKOR AKHIR  |  Hitam: {skorHitamAkhir}  |  Putih: {skorPutihAkhir}");
        Console.WriteLine("====================================");

        if (winner != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int skorPemenang = _controller.GetScore(winner);
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
    }

    private void DrawConsoleBoard(IBoard board, IReadOnlyList<Position> validMoves)
    {
        Console.WriteLine("\n    0  1  2  3  4  5  6  7 ");
        Console.WriteLine("   =========================");

        for (int row = 0; row < board.Size; row++)
        {
            Console.Write($"{row} |");
            for (int column = 0; column < board.Size; column++)
            {
                IPiece? piece = board.Grid[row][column].Piece;

                if (piece == null)
                {
                    bool isLangkahValid = false;
                    foreach (Position move in validMoves)
                    {
                        if (move.Row == row && move.Column == column)
                        {
                            isLangkahValid = true;
                            break;
                        }
                    }

                    if (isLangkahValid)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" * ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(" . ");
                    }
                }
                else if (piece.Color == PieceColor.Black)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" ○ ");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" ● ");
                    Console.ResetColor();
                }
            }
            Console.WriteLine(" | ");
        }
        Console.WriteLine("   =========================");
    }
}