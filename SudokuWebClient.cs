using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

using Newtonsoft.Json;

namespace VP_Sudoku
{
    public static class SudokuWebClient
    {
        static HttpClient client = new HttpClient();

        public static async Task<GameJson> GetSudokuTalbeAsync(string path)
        {
            string message = await client.GetStringAsync(path);
            var data = JsonConvert.DeserializeObject<GameJson>(message);

            return data;
        } 
    }

    public class GameJson
    {
        public bool response;
        public string size;
        public List<GridCellJson> squares;
    }

    public class GridCellJson
    {
        public int x;
        public int y;
        public int value;
    }
}
