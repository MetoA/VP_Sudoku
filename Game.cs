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
        public GridCellDTO[,] currentBoard { get; set; }
        public GridCellDTO[,] initialBoard { get; set; }
        public GridCellDTO[,] solvedBoard { get; set; }
        public GameDTO gameDTO { get; set; }
        public int score { get; set; }
        public int livesLeft { get; set; }
        public long playTime { get; set; }
        public string difficulty { get; set; }

        public Game(string difficulty)
        {
            currentBoard = new GridCellDTO[9, 9];
            initialBoard = new GridCellDTO[9, 9];
            solvedBoard = new GridCellDTO[9, 9];
            score = 200;
            playTime = 0;

            this.difficulty = difficulty;
            livesLeft = int.Parse(difficulty) + 2;
        }

        public string playTimeToTime()
        {
            long minutes = (long)Math.Floor(playTime / 60f);
            long seconds = playTime % 60;

            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }
}
