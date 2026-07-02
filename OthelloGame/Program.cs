using System;
using System.Collections.Generic;
using OthelloGame.Domain.Enum;
using OthelloGame.Domain.Interfaces;
using OthelloGame.Domain.Class;

IPlayer player1 = new Player("Player 1 (Hitam)", PieceColor.Black);
IPlayer player2 = new Player("Player 2 (Putih)", PieceColor.White);
List<IPlayer> players = new List<IPlayer> { player1, player2 };

IBoard board = new Board(8);

// fungsi pembuat bidak 
Func<PieceColor, IPiece> pieceGenerator = (color) => new Piece(color);


board.Grid[3][3].Piece = pieceGenerator(PieceColor.White);
board.Grid[3][4].Piece = pieceGenerator(PieceColor.Black);
board.Grid[4][3].Piece = pieceGenerator(PieceColor.Black);
board.Grid[4][4].Piece = pieceGenerator(PieceColor.White);


GameController controller = new GameController(players, board, pieceGenerator);


GameUI ui = new GameUI(controller, players);
ui.Start();