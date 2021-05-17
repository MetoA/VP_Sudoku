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

        public Panel GridPanel
        {
            get => gridPanel;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            game = new Game(this);
            game.NewGame();
        }
    }
}
