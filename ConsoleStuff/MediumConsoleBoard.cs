﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSharpChess;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace ConsoleStuff
{
    /* 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */
    public class MediumConsoleBoard
    {
        private const int PieceCellSize = 3;
        private const int CellBorderWidth = 2;
        private const int ConsoleCellSize = (CellBorderWidth + PieceCellSize);
        private readonly ChessBoard _board;

        public static string ToString(ChessBoard board)
        {
            return new MediumConsoleBoard(board).Build().ToString();
        }

        private MediumConsoleBoard(ChessBoard board)
        {
            _board = board;
        }

        private ConsolePanel.ConsolePanel Build()
        {
            var panels = GetPiecePanels();

            var boardSize = ((ConsoleCellSize -1) * 8) + 1; // Borders overlap
            var boardSquares = new ConsolePanel.ConsolePanel(boardSize, boardSize);

            foreach (var rank in Chess.Board.Ranks.Reverse())
            {
                foreach (var file in Chess.Board.Files)
                {
                    var loc = BoardLocation.At(file, rank);
                    var panel = panels[loc];

                    var panelX = (((int)file - 1) * (ConsoleCellSize - 1)) + 1;
                    var panelY = ((8 - rank) * (ConsoleCellSize - 1)) + 1;
                    boardSquares.PrintAt(panelX, panelY, panel);
                }
            }
            return boardSquares;
        }

        private IDictionary<BoardLocation, ConsolePanel.ConsolePanel> GetPiecePanels()
        {
            IDictionary<BoardLocation, ConsolePanel.ConsolePanel> panels = new ConcurrentDictionary<BoardLocation, ConsolePanel.ConsolePanel>();

            foreach (var file in Chess.Board.Files)
            {
                foreach (var rank in Chess.Board.Ranks)
                {
                    var at = BoardLocation.At(file, rank);
                    panels.Add(at, CreateConsoleCell(_board[at]));
                }
            }
            return panels;
        }

        private ConsolePanel.ConsolePanel CreateConsoleCell(BoardPiece boardPiece)
        {
            var cell = new ConsolePanel.ConsolePanel(PieceCellSize, PieceCellSize);

            var c = OneCharBoard.ToChar(boardPiece);
            c = c == '.' ? c = ' ' : c;
            cell.PrintAt(PieceCellSize / 2 + 1, PieceCellSize / 2 + 1, c);

            var border = CreateCellBorder();
            border.PrintAt(2, 2, cell);

            return border;
        }

        private static ConsolePanel.ConsolePanel CreateCellBorder()
        {
            var border = new ConsolePanel.ConsolePanel(ConsoleCellSize, ConsoleCellSize);
            var edgeWith = PieceCellSize;
            border.PrintAt(1, 1, $"+{'-'.Repeat(edgeWith)}+");

            int i = 0;
            for (; i < edgeWith; i++)
            {
                border.PrintAt(1, 2 + i, $"|{' '.Repeat(edgeWith)}|");
            }
            border.PrintAt(1, i + 2, $"+{'-'.Repeat(edgeWith)}+");
            return border;
        }
    }
}