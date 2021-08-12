using System;
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
        public GridCellDTO[,] gridCellsDTO { get; set; }
        public GameDTO gameDTO { get; set; }

        public Game()
        {
            //NewGame();
            this.gridCellsDTO = new GridCellDTO[9, 9];
        }

        public async Task<GameDTO> NewGame()
        {
            this.gameDTO = await SudokuWebClient.GetSudokuTableAsync("http://www.cs.utep.edu/cheon/ws/sudoku/new/?size=9&level=3");
            return gameDTO;
        }

        //public void Solve()
        //{
        //    SudokuSolver.Solve(gridCells);
        //}

    }
}
