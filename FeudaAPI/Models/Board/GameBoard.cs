using FeudaAPI.Models.Data;
using System.Collections.Generic;

namespace FeudaAPI.Models
{
    public class GameBoard
    {   
        public Tile[,] BoardTiles { get; } = new Tile[5, 5];
        public List<Tile> TilesWithSerfs { get; } = new();
    }
}

