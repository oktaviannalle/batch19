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
var players = new List<IPlayer> { player1, player2 };