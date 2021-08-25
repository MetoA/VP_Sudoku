﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace VP_Sudoku
{
    [Serializable]
    class Game
    {
        public GridCellDTO[,] currentBoard { get; set; }
        public GameDTO gameDTO { get; set; }

        public GridCellDTO[,] initialBoard { get; set; }
        public GridCellDTO[,] solvedBoard { get; set; }
        public int score { get; set; }

        public Game()
        {
            this.currentBoard = new GridCellDTO[9, 9];
            this.initialBoard = new GridCellDTO[9, 9];
            this.solvedBoard = new GridCellDTO[9, 9];
            this.score = 200;
        }

        //public async Task<GameDTO> NewGame()
        //{
        //    this.gameDTO = await SudokuWebClient.GetSudokuTableAsync("http://www.cs.utep.edu/cheon/ws/sudoku/new/?size=9&level=3");
        //    return gameDTO;
        //}

        //public void Solve()
        //{
        //    SudokuSolver.Solve(gridCells);
        //}

    }
}
