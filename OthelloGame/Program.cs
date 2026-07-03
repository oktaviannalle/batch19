﻿using System;
using System.Collections.Generic;
using Serilog;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using OthelloGame.Domain.Class;

// inisialisasi serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() 
    .WriteTo.Console()   
    .WriteTo.File("Logs/othello_game.log", rollingInterval: RollingInterval.Day) 
    .CreateLogger();

try
{
    Log.Information("=== APLIKASI OTHELLO DIMULAI ===");

    IPlayer player1 = new Player("Player 1 (Hitam)", PieceColor.Black);
    IPlayer player2 = new Player("Player 2 (Putih)", PieceColor.White);
    List<IPlayer> players = new List<IPlayer> { player1, player2 };

    IBoard board = new Board(8);

    Func<PieceColor, IPiece> pieceGenerator = (color) => new Piece(color);

    board.Grid[3][3].Piece = pieceGenerator(PieceColor.White);
    board.Grid[3][4].Piece = pieceGenerator(PieceColor.Black);
    board.Grid[4][3].Piece = pieceGenerator(PieceColor.Black);
    board.Grid[4][4].Piece = pieceGenerator(PieceColor.White);

    GameController controller = new GameController(players, board, pieceGenerator);

    // PERBAIKAN 1: Tambahkan pembuatan objek GameUI di sini
    GameUI ui = new GameUI(controller, players);

    // PERBAIKAN 2: Gunakan huruf kecil 'ui'
    ui.Start();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Terjadi kesalahan fatal sistem yang menghentikan aplikasi Othello!");
}
finally
{
    Log.Information("=== APLIKASI OTHELLO BERHENTI ===");
    Log.CloseAndFlush(); 
}
// PERBAIKAN 3: Ui.Start() yang tadi ada di baris ini sudah dihapus