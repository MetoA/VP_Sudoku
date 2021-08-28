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

        public static void init()
        {
            Console.WriteLine(completePath);
            createFileIfNotExists(Path.Combine(completePath, "diff1.txt"));
            createFileIfNotExists(Path.Combine(completePath, "diff2.txt"));
            createFileIfNotExists(Path.Combine(completePath, "diff3.txt"));
        }

        public static void writeToFile(string path, string content)
        {
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(content);
            }
        }

        private static void createFileIfNotExists(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

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

        public static string getHighScoreFromDifficulty(string difficulty)
        {
            List<int> scores = readScoresFromFile(getPathOfDifficulty(difficulty));
            return scores.Count > 0 ? scores.Max().ToString() : "No scores yet!";
        }

        public static string getPathOfDifficulty(string difficulty)
        {
            return completePath + "\\diff" + difficulty + ".txt";
        }
    }
}
