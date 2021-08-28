# VP_Sudoku

Classic sudoku featuring saving and loading games, a solver, scores and time tracking.

## 1 Guide to playing

### 1.1 Sudoku rules
If you are not familiar with sudoku rules i recommend reading this article to get a sense of how the game plays: [Sudoku for dummies](https://masteringsudoku.com/sudoku-rules-beginners/)

### 1.2 Start Screen
![Sudoku Start Menu](https://imgur.com/aIPT4wk.jpg)
This screen appears when you launch the application. It consists of options to play the game, open a saved game, solve a game and save one (if one is started already).
Score tracking and highscores for given difficulty, lives left and play time in minutes and seconds.
The difficulties ranging from easiest to hardest: 
 - 1
 - 2
 - 3

### 1.3 New Game
![New Game](https://imgur.com/Rcfs5Fr.jpg)
Here on the new game screen, we can see the grid filled with the initial numbers which are not modifable. Each group of 9 is separated by color. The game information is gathered from an [api](http://www.cs.utep.edu/cheon/ws/sudoku/) call.

### 1.4 Playing the game
![Playing a game](https://imgur.com/of1WEw5.jpg)
We can see the correct values are green and incorrect are red. Notice that Lives Left is decreased by one, once it reaches zero it's game over!
Also notice the score is far below zero, as the time ticks, the score gets decreased each second. However, by getting a correct value you gain points, this means that you have to be fast and correct if you want to get the highest scores.

### 1.5 Solved game
![Solved game](https://imgur.com/FHAjLpC.jpg)
When we decide to give up because the game is too hard, we have our trusty solve button, all of the blue numbers are numbers that were not entered by the user or numbers that were incorrect. Note that this does not contribute to the list of scores for that difficulty. Game and score pauses when the solve button is clicked.

### 1.6 Losing a game
![Losing a game](https://imgur.com/4rGA1DZ.jpg)
Upon losing we get prompted if we want to play another game, by clicking yes new game is created as usual.

### 1.7 Winning a game
![Winning a game](https://imgur.com/ySZl7Sj.jpg)
Upon winning we get prompted if we want to play another game, when winning the score is added to the file of scores for that certain difficulty. The scoring system is a bit strict :)


## 2 The Code

### 2.1 The main Game class
```C#
[Serializable]
    class Game
    {
        /// <summary>
        /// The board which we start with.
        /// </summary>
        public GridCellDTO[,] initialBoard { get; set; }
        /// <summary>
        /// The solved board after it was initialized.
        /// </summary>
        public GridCellDTO[,] solvedBoard { get; set; }
        /// <summary>
        /// The current board which is being updated as you play.
        /// </summary>
        public GridCellDTO[,] currentBoard { get; set; }
        /// <summary>
        /// The information we get from the API call.
        /// </summary>
        public GameDTO gameDTO { get; set; }
        public int score { get; set; }
        public int livesLeft { get; set; }
        public long playTime { get; set; }
        public string difficulty { get; set; }

        public Game(string difficulty)
        {
            currentBoard = new GridCellDTO[9, 9];
            initialBoard = new GridCellDTO[9, 9];
            solvedBoard = new GridCellDTO[9, 9];
            score = 200;
            playTime = 0;

            this.difficulty = difficulty;
            livesLeft = int.Parse(difficulty) + 2;
        }
    }
```

### 2.2 The HTTP client
```C#
public static class SudokuWebClient
    {
        static HttpClient client = new HttpClient();

        /// <summary>
        /// Gets the sudoku table information from an API.
        /// </summary>
        /// <param name="path">The URL at which the async HTTP GET will be performed.</param>
        /// <returns></returns>
        public static async Task<GameDTO> GetSudokuTableAsync(string path)
        {
            string message = await client.GetStringAsync(path);
            var data = JsonConvert.DeserializeObject<GameDTO>(message);

            return data;
        } 
    }
```
The typical response from an API call looks like this:
```json
{
    response: true,
    size: 4,
    squares: [
        {
        x: 0,
        y: 3,
        value: 1
        },
        {
        x: 1,
        y: 0,
        value: 4
        },
        {
        x: 2,
        y: 1,
        value: 3
        },
        {
        x: 3,
        y: 3,
        value: 3
        }
    ]
}
```
This information is stored in the GameDTO class:
```C#
public class GameDTO
    {
        public bool response;
        public string size;
        public List<GridCellDTO> squares;
    }
```
Where GridCellDTO is a representation of a value in the squares array in the json:
```C#
[Serializable]
    public class GridCellDTO
    {
        public int x;
        public int y;
        public int value = 0;
        public bool isLocked;
        public Color color { get; set; }

        public GridCellDTO(){}

        public GridCellDTO(int x, int y, int value = 0, bool isLocked = false)
        {
            this.x = x;
            this.y = y;
            this.value = value;
            this.isLocked = isLocked;
        }

        public override string ToString()
        {
            return $"[x={x}, y={y}, val={value}]";
        }
    }
```

### 2.3 Algorithms
This is a backtracking algorithm where we go cell by cell and try a value, if it is valid we continue on until eventually we reach a solution or we don't, in which case the algorithm goes back a certain number of cells and tries a different value, and so on until the result is reached.
The algorithm for solving sudoku boards:
```c#
public class SudokuSolver
    {
        /// <summary>
        /// Solves the specified matrix of GridCellDTO.
        /// </summary>
        /// <param name="grid">The grid to be solved.</param>
        /// <returns>Wether or not the grid was solved.</returns>
        public static bool SolveMatrix(GridCellDTO[,] grid)
        {
            int row = -1;
            int col = -1;
            bool isDone = true;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (grid[i, j].value == 0)
                    {
                        row = i;
                        col = j;

                        isDone = false;
                        break;
                    }
                }

                if (!isDone) break;
            }

            if (isDone) return true;

            for (int num = 1; num <= 9; num++)
            {
                if (!grid[row, col].isLocked && IsValid(grid, row, col, num))
                {
                    grid[row, col].value = num;
                    if (SolveMatrix(grid))
                    {
                        return true;
                    }
                    else
                    {
                        grid[row, col].value = 0;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// In a given grid, checks wether the value to be input is compliant with the sudoku rules.
        /// </summary>
        /// <param name="grid">The grid to be checked.</param>
        /// <param name="row">The row in the grid to be checked.</param>
        /// <param name="col">The column in the grid to be checked.</param>
        /// <param name="num">The value being checked.</param>
        /// <returns></returns>
        public static bool IsValid(GridCellDTO[,] grid, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
                if (grid[row, i].value == num) return false;

            for (int i = 0; i < 9; i++)
                if (grid[i, col].value == num) return false;

            int rowStart = row - row % 3;
            int colStart = col - col % 3;

            for (int i = rowStart; i < rowStart + 3; i++)
                for (int j = colStart; j < colStart + 3; j++)
                    if (grid[i, j].value == num) return false;

            return true;
        }
    }
```

### 2.4 Working with files
```c#
class FileService
    {
        static string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static string completePath = Path.Combine(systemPath, "Sudoku_Scores");

        /// <summary>
        /// Creates 3 files which contain scores that you create during gameplay.
        /// Typically these files are located in C:/ProgramData/, however there is a Console.WriteLine() to show the your save location.
        /// </summary>
        public static void init()
        {
            Console.WriteLine(completePath);
            createFileIfNotExists(Path.Combine(completePath, "diff1.txt"));
            createFileIfNotExists(Path.Combine(completePath, "diff2.txt"));
            createFileIfNotExists(Path.Combine(completePath, "diff3.txt"));
        }

        /// <summary>
        /// Writes the achieved score to the specified file.
        /// </summary>
        /// <param name="path">Path of the file where the content will be written.</param>
        /// <param name="content">The content which will be written in the file.</param>
        public static void writeToFile(string path, string content)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(content);
            }
        }

        /// <summary>
        /// Creates the file if it does not exist.
        /// </summary>
        /// <param name="path">The path of the file to be created if it does not exist.</param>
        private static void createFileIfNotExists(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        /// <summary>
        /// Reads all the scores from a given file and returns them.
        /// </summary>
        /// <param name="path">The path of the file that is being read.</param>
        /// <returns>A list of all the scores in the specified file.</returns>
        private static List<int> readScoresFromFile(string path)
        {
            List<int> scores = new List<int>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        scores.Add(int.Parse(line));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return scores;
        }
    }
```
