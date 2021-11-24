using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeudaAPI.Models.DataHolder;

namespace FeudaAPI.Models
{
    public class Building
    {
        public List<TileType> BuildableOn = new();

        public BuildingType BuildingType { get; set; }

        public int WoodPrice { get; set; }
        public int OrePrice { get; set; }

        public Building(BuildingType buildingType, List<TileType> buildableOn, int woodPrice, int orePrice)
        {
            BuildingType = buildingType;
            BuildableOn = buildableOn;
            WoodPrice = woodPrice;
            OrePrice = orePrice;
        }

        public Building(BuildingType buildingType, TileType buildableOn, int woodPrice, int orePrice)
        {
            BuildingType = buildingType;
            BuildableOn = new List<TileType>() { buildableOn };
            WoodPrice = woodPrice;
            OrePrice = orePrice;
        }
    }
}
