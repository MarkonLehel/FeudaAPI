using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeudaAPI.Models.Enums;

namespace FeudaAPI.Models.Board
{
    public class Building
    {
        public List<TileType> BuildableOn = new();

        public Dictionary<string, int> TileModifier = new Dictionary<string, int>
        {
            { "Food", 1 },
            { "Wood", 1 },
            { "Ore", 1 },
            { "Gold", 1 }
        };


    }
}
