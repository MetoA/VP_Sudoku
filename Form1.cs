using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VP_Sudoku
{
    public partial class Form1 : Form
    {
        Game game = null;
        GridCell[,] gridCells;
        int countTracker;
        string fileName;
        string selectedDifficulty;

        public Form1()
        {
            InitializeComponent();
            formInit();
        }

        #region EVENTS
        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            createNewGame();
        }

        /// <summary>
        /// Solves the current active game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSolve_Click(object sender, EventArgs e)
        {
            solveGame();
        }

        /// <summary>
        /// Saves the current active game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            highscoreTimer.Stop();
            saveFile();
            highscoreTimer.Start();
        }

        /// <summary>
        /// Loads the selected game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            highscoreTimer.Stop();

            if (gridCells[0, 0] == null) createGrid();
            openFile();
            fillGridCells(game.currentBoard, gridCells);

            lblPlayTime.Text = "Play time: " + game.playTimeToTime();
            livesToolStrip.Text = "Lives left: " + game.livesLeft;

            highscoreTimer.Start();
        }

        /// <summary>
        /// Logic when inputting a value in a grid cell.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cell_keyPressed(object sender, KeyPressEventArgs e)
        {
            GridCell cell = sender as GridCell;

            if (cell.IsLocked) return;

            if (e.KeyChar.ToString() == "\b")
            {
                cell.Clear();
                return;
            }

            int value;

            if (int.TryParse(e.KeyChar.ToString(), out value))
            {
                if (value == game.solvedBoard[cell.X, cell.Y].value) onCorrectKey(cell); 
                else  onWrongKey(cell);

                game.currentBoard[cell.X, cell.Y].value = value;
                cell.Value = value;
                cell.Text = cell.Value.ToString();

                if (countTracker <= 0) onWin();

                if(game.livesLeft <= 0) onLoss();
            }
        }

        /// <summary>
        /// Logic when the timer ticks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void highscoreTimer_Tick(object sender, EventArgs e)
        {
            game.score -= 10;
            game.playTime++;

            lblScore.Text = "Score: " + game.score;
            lblPlayTime.Text = "Play time: " + game.playTimeToTime();
        }

        /// <summary>
        /// Logic when a different difficulty is chosen from the combo box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void difficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDifficulty = difficultyComboBox.SelectedItem.ToString();
            lblHighScore.Text = "High score on difficulty: " + FileService.getHighScoreFromDifficulty(selectedDifficulty);
        }

        /// <summary>
        /// Prompts the user to save the current active game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(btnSave.Enabled == true) createFormExitDialog();
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            createNewGame();
        }

        /// <summary>
        /// Loads the selected game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFile();
        }

        /// <summary>
        /// Saves the current active game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveFile();
        }
        #endregion

        #region SAVING

        /// <summary>
        /// Saves the current active game to a .sud file.
        /// </summary>
        private void saveFile()
        {
            highscoreTimer.Stop();
            if (fileName == null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Sudoku file (*.sud)|*.sud";
                sfd.Title = "Save Sudoku game";
                sfd.FileName = fileName;

                if(sfd.ShowDialog() == DialogResult.OK) fileName = sfd.FileName;
            }

            if (fileName != null)
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, game);
                }
            }
            highscoreTimer.Start();
        }

        /// <summary>
        /// Loads the selected .sud file.
        /// </summary>
        private void openFile()
        {
            highscoreTimer.Stop();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Sudoku file (*.sud)|*.sud";
            ofd.Title = "Open Sudoku game";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;

                try
                {
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        game = (Game)formatter.Deserialize(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot read file: " + fileName);
                    fileName = null;
                    highscoreTimer.Start();
                    return;
                }
            }
            highscoreTimer.Start();
        }
        #endregion

        #region CUSTOM_FUNCTIONS

        /// <summary>
        /// Creates the GridCells in a panel, adds styling to them and the key press event.
        /// </summary>
        private void createGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    gridCells[i, j] = new GridCell
                    {
                        Size = new Size(40, 40),
                        Location = new Point(i * 40, j * 40),
                        X = i,
                        Y = j,
                        BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightCyan,
                        ForeColor = Color.Gray,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font(SystemFonts.DefaultFont.FontFamily, 20)
                    };
                    gridCells[i, j].FlatAppearance.BorderColor = Color.Black;
                    gridCells[i, j].KeyPress += cell_keyPressed;
                    gridPanel.Controls.Add(gridCells[i, j]);
                }
            }
        }

        /// <summary>
        /// Populates the panel's grid cells with information and styling.
        /// </summary>
        /// <param name="gridCellsDTO">Information from which to populate the panel.</param>
        /// <param name="gridCells">The grid cells which to be populated.</param>
        private void fillGridCells(GridCellDTO[,] gridCellsDTO, GridCell[,] gridCells)
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    gridCells[i, j].Enabled = true;
                    GridCellDTO cell = gridCellsDTO[i, j];
                    if (cell.value != 0)
                    {
                        cell.isLocked = true;
                        gridCells[i, j].Value = cell.value;
                        gridCells[i, j].Text = cell.value.ToString();
                        gridCells[i, j].IsLocked = cell.color != Color.Red;
                        gridCells[i, j].ForeColor = cell.color;
                    }
                    else
                    {
                        gridCells[i, j].Value = 0;
                        gridCells[i, j].Text = "";
                        gridCells[i, j].IsLocked = false;
                    }
                }
            }

        }

        /// <summary>
        /// Initializes an empty board for a specified game.
        /// </summary>
        /// <param name="board">The board to be initialized with default information.</param>
        /// <param name="game">The game from which to get information.</param>
        private void initializeBoard(GridCellDTO[,] board, GameDTO game)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    board[i, j] = new GridCellDTO(i, j);

            foreach (GridCellDTO cell in game.squares)
            {
                board[cell.x, cell.y].isLocked = true;
                board[cell.x, cell.y].value = cell.value;
                board[cell.x, cell.y].color = Color.Black;
            }
        }

        /// <summary>
        /// Prints information of a board. Purely for testing purposes.
        /// </summary>
        /// <param name="board">The board to be printed.</param>
        private void printGridCellDTOArray(GridCellDTO[,] board)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(board[i, j].ToString());
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Creates a message box prompting the user if they want to play another game.
        /// </summary>
        /// <param name="title">The title of the message box.</param>
        private void createGameResultDialog(string title)
        {
            DialogResult result = MessageBox.Show("Do you want to play another game?", title, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) createNewGame();
        }

        /// <summary>
        /// Creates a message box upon exiting the game, prompting the user if they want to save the current active game if there is one.
        /// </summary>
        private void createFormExitDialog()
        {
            highscoreTimer.Stop();

            DialogResult result = MessageBox.Show("Do you want to save your game before you leave?", "Attention!", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) saveFile();

            highscoreTimer.Start();
        }

        /// <summary>
        /// Initializes the form.
        /// </summary>
        private void formInit()
        {
            btnSolve.Enabled = false;
            btnSave.Enabled = false;

            gridCells = new GridCell[9, 9];
            selectedDifficulty = "1";

            difficultyComboBox.Items.Add("1");
            difficultyComboBox.Items.Add("2");
            difficultyComboBox.Items.Add("3");

            FileService.init();

            lblHighScore.Text = "High score on difficulty: " + FileService.getHighScoreFromDifficulty(selectedDifficulty);
        }

        /// <summary>
        /// Starts a new game to be played.
        /// </summary>
        private async void createNewGame()
        {
            btnSolve.Enabled = false;
            btnSave.Enabled = false;

            game = new Game(selectedDifficulty);
            game.gameDTO = await SudokuWebClient.GetSudokuTableAsync("http://www.cs.utep.edu/cheon/ws/sudoku/new/?size=9&level=" + selectedDifficulty);
            countTracker = 81 - game.gameDTO.squares.Count;

            gridPanel.Controls.Clear();
            createGrid();
            initializeBoard(game.currentBoard, game.gameDTO);
            initializeBoard(game.initialBoard, game.gameDTO);
            initializeBoard(game.solvedBoard, game.gameDTO);
            fillGridCells(game.initialBoard, gridCells);
            SudokuSolver.SolveMatrix(game.solvedBoard);

            livesToolStrip.Text = "Lives left: " + game.livesLeft;
            lblScore.Text = "Score: " + game.score;

            btnSave.Enabled = true;
            btnSolve.Enabled = true;

            highscoreTimer.Start();
        }

        /// <summary>
        /// Populates the grid cells with information of the solved game.
        /// </summary>
        private void solveGame()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!gridCells[i, j].IsLocked)
                    {
                        gridCells[i, j].Value = game.solvedBoard[i, j].value;
                        gridCells[i, j].Text = game.solvedBoard[i, j].value.ToString();
                        gridCells[i, j].ForeColor = Color.Blue;
                        gridCells[i, j].IsLocked = true;
                    }
                }
            }

            highscoreTimer.Stop();
        }

        /// <summary>
        /// Logic when inputting the correct value in a grid cell.
        /// </summary>
        /// <param name="cell">The grid cell which had value inputted.</param>
        private void onCorrectKey(GridCell cell)
        {
            cell.ForeColor = Color.Green;
            cell.IsLocked = true;

            game.currentBoard[cell.X, cell.Y].color = Color.Green;
            game.currentBoard[cell.X, cell.Y].isLocked = true;
            game.score += 50;

            countTracker--;

            lblScore.Text = "Score: " + game.score;
        }

        /// <summary>
        /// Logic when inputting the wrong value in a grid cell.
        /// </summary>
        /// <param name="cell">The grid cell which had value inputted.</param>
        private void onWrongKey(GridCell cell)
        {
            cell.ForeColor = Color.Red;

            game.currentBoard[cell.X, cell.Y].color = Color.Red;
            game.score -= 10;
            game.livesLeft -= 1;

            lblScore.Text = "Score: " + game.score;
            livesToolStrip.Text = "Lives left: " + game.livesLeft;
        }

        /// <summary>
        /// Writes the score to the correct file and prompts the user for a new game.
        /// </summary>
        private void onWin()
        {
            highscoreTimer.Stop();
            FileService.writeToFile(FileService.getPathOfDifficulty(selectedDifficulty), game.score.ToString());
            createGameResultDialog("You Won!");
        }

        /// <summary>
        /// Prompts the user for a new game, if denied, the grid cells will be disabled.
        /// </summary>
        private void onLoss()
        {
            highscoreTimer.Stop();
            createGameResultDialog("You Lost!");
            clear();
        }

        /// <summary>
        /// Disables the grid cells, save button and solve button.
        /// </summary>
        private void clear()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    gridCells[i, j].Enabled = false;
                }
            }

            btnSave.Enabled = false;
            btnSolve.Enabled = false;
        }
        #endregion
    }
}
