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
    //panel clear controls on new game, add controls on creating grid
    public partial class Form1 : Form
    {
        Game game = null;
        GridCell[,] gridCells;
        string fileName;
        int countTracker;

        public Form1()
        {
            InitializeComponent();
            btnSolve.Enabled = false;
            this.gridCells = new GridCell[9, 9];

        }

        #region EVENTS
        private async void btnNewGame_Click(object sender, EventArgs e)
        {
            btnSolve.Enabled = false;
            game = new Game();
            game.gameDTO = await SudokuWebClient.GetSudokuTableAsync("http://www.cs.utep.edu/cheon/ws/sudoku/new/?size=9&level=3");
            countTracker = 81 - game.gameDTO.squares.Count;
            gridPanel.Controls.Clear();

            createGrid();
            initializeBoard(game.currentBoard, game.gameDTO);
            initializeBoard(game.initialBoard, game.gameDTO);
            initializeBoard(game.solvedBoard, game.gameDTO);
            fillGridCells(game.initialBoard, this.gridCells);
            printGridCellDTOArray(game.currentBoard);
            SudokuSolver.SolveMatrix(game.solvedBoard);
            highscoreTimer.Start();
            lblScore.Text = "Score: " + game.score;

            btnSolve.Enabled = true;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Solving...");
            //SudokuSolver.Solve(gridCells);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!gridCells[i,j].IsLocked)
                    {
                        gridCells[i, j].Value = game.solvedBoard[i, j].value;
                        gridCells[i, j].Text = game.solvedBoard[i, j].value.ToString();
                        gridCells[i, j].ForeColor = Color.Blue;
                        gridCells[i, j].IsLocked = true;
                    }
                }
            }
            highscoreTimer.Stop();
            Console.WriteLine("Solved! Thank you for trying.");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            highscoreTimer.Stop();
            saveFile();
            Console.WriteLine("Saved!");
            highscoreTimer.Start();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            highscoreTimer.Stop();
            openFile();
            fillGridCells(game.currentBoard, gridCells);
            Console.WriteLine("Opened!");
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
                //Console.WriteLine("Backspace pressed!");
            }

            int value;

            if (int.TryParse(e.KeyChar.ToString(), out value))
            {
                if (value == game.solvedBoard[cell.X, cell.Y].value) //Correct
                {
                    cell.ForeColor = Color.Green;
                    cell.IsLocked = true;
                    countTracker--;

                    game.currentBoard[cell.X, cell.Y].color = Color.Green;
                    game.currentBoard[cell.X, cell.Y].isLocked = true;

                    game.score += 20;
                    lblScore.Text = "Score: " + game.score;
                }
                else //Wrong
                {
                    cell.ForeColor = Color.Red;
                    game.currentBoard[cell.X, cell.Y].color = Color.Red;
                    game.score -= 10;
                    lblScore.Text = "Score: " + game.score;
                }

                game.currentBoard[cell.X, cell.Y].value = value;
                cell.Value = value;
                cell.Text = cell.Value.ToString();

                if (countTracker <= 0)
                {
                    highscoreTimer.Stop();
                    Console.WriteLine("FINISHED");
                }
            }

        }
        #endregion

        #region SAVING

        private void saveFile()
        {
            if (fileName == null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Sudoku file (*.sud)|*.sud";
                sfd.Title = "Save Sudoku game";
                sfd.FileName = fileName;

                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    fileName = sfd.FileName;
                }
            }

            if (fileName != null)
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fileStream, game);
                }
            }
        }

        private void openFile()
        {
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
                    return;
                }
            }
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
                    GridCellDTO cell = gridCellsDTO[i, j];
                    if (cell.value != 0)
                    {
                        cell.isLocked = true;
                        gridCells[cell.x, cell.y].Value = cell.value;
                        gridCells[cell.x, cell.y].Text = cell.value.ToString();
                        gridCells[cell.x, cell.y].IsLocked = cell.color != Color.Red;
                        gridCells[cell.x, cell.y].ForeColor = cell.color;
                    }
                    else
                    {
                        gridCells[cell.x, cell.y].Value = 0;
                        gridCells[cell.x, cell.y].Text = "";
                        gridCells[cell.x, cell.y].IsLocked = false;
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
        #endregion

        private void highscoreTimer_Tick(object sender, EventArgs e)
        {
            game.score -= 10;
            lblScore.Text = "Score: " + game.score;
        }
    }
}
