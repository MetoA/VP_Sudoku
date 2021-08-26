
namespace VP_Sudoku
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gridPanel = new System.Windows.Forms.Panel();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.btnSolve = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.highscoreTimer = new System.Windows.Forms.Timer(this.components);
            this.lblScore = new System.Windows.Forms.Label();
            this.lblPlayTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gridPanel
            // 
            this.gridPanel.Location = new System.Drawing.Point(16, 15);
            this.gridPanel.Margin = new System.Windows.Forms.Padding(4);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.Size = new System.Drawing.Size(480, 443);
            this.gridPanel.TabIndex = 0;
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(541, 15);
            this.btnNewGame.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(100, 28);
            this.btnNewGame.TabIndex = 1;
            this.btnNewGame.Text = "New Game";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // btnSolve
            // 
            this.btnSolve.Location = new System.Drawing.Point(541, 64);
            this.btnSolve.Margin = new System.Windows.Forms.Padding(4);
            this.btnSolve.Name = "btnSolve";
            this.btnSolve.Size = new System.Drawing.Size(100, 28);
            this.btnSolve.TabIndex = 2;
            this.btnSolve.Text = "Solve";
            this.btnSolve.UseVisualStyleBackColor = true;
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(541, 172);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(541, 224);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(100, 28);
            this.btnOpen.TabIndex = 4;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // highscoreTimer
            // 
            this.highscoreTimer.Interval = 1000;
            this.highscoreTimer.Tick += new System.EventHandler(this.highscoreTimer_Tick);
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(538, 292);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(49, 17);
            this.lblScore.TabIndex = 5;
            this.lblScore.Text = "Score:";
            // 
            // lblPlayTime
            // 
            this.lblPlayTime.AutoSize = true;
            this.lblPlayTime.Location = new System.Drawing.Point(903, 9);
            this.lblPlayTime.Name = "lblPlayTime";
            this.lblPlayTime.Size = new System.Drawing.Size(69, 17);
            this.lblPlayTime.TabIndex = 6;
            this.lblPlayTime.Text = "Play time:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.lblPlayTime);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSolve);
            this.Controls.Add(this.btnNewGame);
            this.Controls.Add(this.gridPanel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel gridPanel;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Button btnSolve;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Timer highscoreTimer;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblPlayTime;
    }
}

