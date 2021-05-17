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
        Game game;

        public Form1()
        {
            InitializeComponent();
            game = new Game(this.gridPanel);
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            game.NewGame();
        }
    }
}
