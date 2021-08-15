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

        public static async Task<GameDTO> GetSudokuTableAsync(string path)
        {
            string message = await client.GetStringAsync(path);
            var data = JsonConvert.DeserializeObject<GameDTO>(message);

            return data;
        } 
    }

    [Serializable]
    public class GameDTO
    {
        public bool response;
        public string size;
        public List<GridCellDTO> squares;
    }

    [Serializable]
    public class GridCellDTO
    {
        public int x;
        public int y;
        public int value = 0;
        public bool isLocked;

        public GridCellDTO()
        {

        }

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
}
