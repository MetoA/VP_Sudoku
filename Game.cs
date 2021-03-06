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
        /// <summary>
        /// The board which we start with.
        /// </summary>
        public GridCellDTO[,] initialBoard { get; set; }
        /// <summary>
        /// The solved board after it was initialized.
        /// </summary>
        public GridCellDTO[,] solvedBoard { get; set; }
        /// <summary>
        /// The current board which is being updated as you play.
        /// </summary>
        public GridCellDTO[,] currentBoard { get; set; }
        /// <summary>
        /// The information we get from the API call.
        /// </summary>
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

        /// <summary>
        /// Parses the ammount of seconds played into mm:ss (minutes, seconds).
        /// </summary>
        /// <returns>A parsed string from the ammount of seconds played.</returns>
        public string playTimeToTime()
        {
            long minutes = (long)Math.Floor(playTime / 60f);
            long seconds = playTime % 60;

            return string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }
}
