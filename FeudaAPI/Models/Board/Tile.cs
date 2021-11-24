using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeudaAPI.Models.Board;
using FeudaAPI.Models.Enums;

namespace FeudaAPI.Models
{
    public class Tile
    {

        public Coordinate Coordinate { get; }
        public TileType? TileType { get; set; } = null;
        public int BaseTileIncome { get; } = 1;
        public bool HasSerf { get; set; }
        public bool HasImprovement { get; set; }

        public Tile(Coordinate coordinate, TileType tileType)
        {
            Coordinate = coordinate;
            TileType = tileType;
        }
    }
}
