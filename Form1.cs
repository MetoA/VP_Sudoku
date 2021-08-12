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
            //await game.NewGame();
            gridPanel.Controls.Clear();
            createGrid();
            fillGridCells(game.gameDTO, this.gridCells);
            Console.WriteLine("Finished task");

            btnSolve.Enabled = true;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Solving...");
            SudokuSolver.Solve(gridCells);
            Console.WriteLine("Solved!");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFile();
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
                cell.Value = value;
                cell.Text = cell.Value.ToString();
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


        public void createGrid()
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

        private void fillGridCells(GameDTO game, GridCell[,] gridCells)
        {
            foreach (GridCellDTO cell in game.squares)
            {
                gridCells[cell.x, cell.y].Value = cell.value;
                gridCells[cell.x, cell.y].Text = cell.value.ToString();
                gridCells[cell.x, cell.y].IsLocked = true;
                gridCells[cell.x, cell.y].ForeColor = Color.Black;
            }
        }
    }
}
