using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeudaAPI.Models.DataHolder;

namespace FeudaAPI.Models
{
    public class Building
    {
        //Maybe one of these 2 constructors can be simplified
        public Building(BuildingType buildingType, List<TileType> buildableOn, int woodPrice, int orePrice, bool isImprovement)
        {
            BuildingType = buildingType;
            BuildableOn = buildableOn;
            WoodPrice = woodPrice;
            OrePrice = orePrice;
            IsTileImprovement = isImprovement;
        }

        public Building(BuildingType buildingType, TileType buildableOn, int woodPrice, int orePrice, bool isImprovement)
        {
            BuildingType = buildingType;
            BuildableOn = new List<TileType>() { buildableOn };
            WoodPrice = woodPrice;
            OrePrice = orePrice;
            IsTileImprovement = isImprovement;
        }

        public List<TileType> BuildableOn = new();

        public bool IsTileImprovement { get; set; }

        public BuildingType BuildingType { get; set; }

        public int WoodPrice { get; set; }
        public int OrePrice { get; set; }


    }
}
