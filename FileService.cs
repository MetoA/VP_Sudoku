using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VP_Sudoku
{
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

        /// <summary>
        /// Depending on the difficulty, a different file will be read and the highest score will be returned.
        /// </summary>
        /// <param name="difficulty">The difficulty of the game.</param>
        /// <returns>The highest score in the specified file or "No scores yet!" if there are none.</returns>
        public static string getHighScoreFromDifficulty(string difficulty)
        {
            List<int> scores = readScoresFromFile(getPathOfDifficulty(difficulty));
            return scores.Count > 0 ? scores.Max().ToString() : "No scores yet!";
        }

        /// <summary>
        /// A method to return the path based on difficulty.
        /// </summary>
        /// <param name="difficulty">The difficulty of the game.</param>
        /// <returns>A string of the path of the file.</returns>
        public static string getPathOfDifficulty(string difficulty)
        {
            return completePath + "\\diff" + difficulty + ".txt";
        }
    }
}
