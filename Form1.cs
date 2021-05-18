using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VP_Sudoku
{
    public partial class Form1 : Form
    {
        Game game = null;

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
    }
}
