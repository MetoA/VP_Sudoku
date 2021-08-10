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
        string fileName;

        public Panel GridPanel
        {
            get => gridPanel;
        }

        public Form1()
        {
            InitializeComponent();
            btnSolve.Enabled = false;
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            game = new Game(this);
            game.NewGame();

            btnSolve.Enabled = true;
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            game.Solve();
        }

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFile();
        }
    }
}
