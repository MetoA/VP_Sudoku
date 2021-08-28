using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace VP_Sudoku
{
    public class SudokuSolver
    {
        /// <summary>
        /// In a given grid, checks wether the value to be input is compliant with the sudoku rules.
        /// </summary>
        /// <param name="grid">The grid to be checked.</param>
        /// <param name="row">The row in the grid to be checked.</param>
        /// <param name="col">The column in the grid to be checked.</param>
        /// <param name="num">The value being checked.</param>
        /// <returns></returns>
        public static bool IsValid(GridCellDTO[,] grid, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
                if (grid[row, i].value == num) return false;

            for (int i = 0; i < 9; i++)
                if (grid[i, col].value == num) return false;

            int rowStart = row - row % 3;
            int colStart = col - col % 3;

            for (int i = rowStart; i < rowStart + 3; i++)
                for (int j = colStart; j < colStart + 3; j++)
                    if (grid[i, j].value == num) return false;

            return true;
        }

        /// <summary>
        /// Solves the specified matrix of GridCellDTO.
        /// </summary>
        /// <param name="grid">The grid to be solved.</param>
        /// <returns>Wether or not the grid was solved.</returns>
        public static bool SolveMatrix(GridCellDTO[,] grid)
        {
            int row = -1;
            int col = -1;
            bool isDone = true;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (grid[i, j].value == 0)
                    {
                        row = i;
                        col = j;

                        isDone = false;
                        break;
                    }
                }

                if (!isDone) break;
            }

            if (isDone) return true;

            for (int num = 1; num <= 9; num++)
            {
                if (!grid[row, col].isLocked && IsValid(grid, row, col, num))
                {
                    grid[row, col].value = num;
                    if (SolveMatrix(grid))
                    {
                        return true;
                    }
                    else
                    {
                        grid[row, col].value = 0;
                    }
                }
            }

            return false;
        }
    }
}
