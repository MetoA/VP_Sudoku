﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace VP_Sudoku
{
    class Game
    {
        Form1 mainForm; 

        GridCell[,] gridCells;
        Panel gridPanel;

        public Game(Form1 form)
        {
            this.mainForm = form;
            this.gridPanel = mainForm.GridPanel;
            this.gridCells = new GridCell[9, 9];

            //this.createGrid();
        }

        public void createGrid()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    gridCells[i, j] = new GridCell
                    {
                        Size = new Size(40, 40),
                        Location = new Point(i * 40, j * 40),
                        X = i,
                        Y = j,
                        BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray,
                        FlatStyle = FlatStyle.Flat,
                    };
                    gridCells[i, j].FlatAppearance.BorderColor = Color.Black;

                    gridCells[i, j].KeyPress += cell_keyPressed;

                    this.gridPanel.Controls.Add(gridCells[i, j]);
                }
            }
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

        public void NewGame()
        {
            this.gridPanel.Controls.Clear();
            this.createGrid();
        }
    }
}