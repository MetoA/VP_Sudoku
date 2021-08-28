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
            this.Text = "Sudoku!";
            InitializeComponent();
            formInit();
        }

        #region EVENTS
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            createNewGame();
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            solveGame();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            highscoreTimer.Stop();
            saveFile();
            highscoreTimer.Start();
        }

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

        private void highscoreTimer_Tick(object sender, EventArgs e)
        {
            game.score -= 10;
            game.playTime++;

            lblScore.Text = "Score: " + game.score;
            lblPlayTime.Text = "Play time: " + game.playTimeToTime();
        }

        private void difficultyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDifficulty = difficultyComboBox.SelectedItem.ToString();
            lblHighScore.Text = "High score on difficulty: " + FileService.getHighScoreFromDifficulty(selectedDifficulty);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(btnSave.Enabled == true) createFormExitDialog();
        }
        #endregion

        #region SAVING

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

        private void createGameResultDialog(string title)
        {
            DialogResult result = MessageBox.Show("Do you want to play another game?", title, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) createNewGame();
            //if (result == DialogResult.No)
        }

        private void createFormExitDialog()
        {
            highscoreTimer.Stop();

            DialogResult result = MessageBox.Show("Do you want to save your game before you leave?", "Attention!", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) saveFile();
            //if (result == DialogResult.No)

            highscoreTimer.Start();
        }

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

        private void onWrongKey(GridCell cell)
        {
            cell.ForeColor = Color.Red;

            game.currentBoard[cell.X, cell.Y].color = Color.Red;
            game.score -= 10;
            game.livesLeft -= 1;

            lblScore.Text = "Score: " + game.score;
            livesToolStrip.Text = "Lives left: " + game.livesLeft;
        }

        private void onWin()
        {
            highscoreTimer.Stop();
            FileService.writeToFile(FileService.getPathOfDifficulty(selectedDifficulty), game.score.ToString());
            createGameResultDialog("You Won!");
        }

        private void onLoss()
        {
            highscoreTimer.Stop();
            createGameResultDialog("You Lost!");
            clear();
            livesToolStrip.Text = "Lives left: " + game.livesLeft;
        }

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

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            createNewGame();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveFile();
        }
    }
}
