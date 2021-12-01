using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeudaAPI.Models;
using FeudaAPI.Models.Data;

namespace FeudaAPI.Models
{
    public class Tile
    {

        public Coordinate Coordinate { get; }
        public TileType? TileType { get; set; } = null;
        public int BaseTileIncome { get; set; } = 1;
        public bool HasSerf { get; set; } = false;
        public bool HasImprovement { get; set; } = false;

        public Building Building { get; set; } = null;
        public bool HasBuilding { get; set; } = false;

        public Tile(Coordinate coordinate, TileType? tileType)
        {
            Coordinate = coordinate;
            TileType = tileType;
        }
    }
}
