using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models.DataHolder
{
    public enum TileType
    {
        Mountain,
        Field,
        Forest,
        Town
    }

    public enum BuildingType
    {
        House,
        Woodcutter,
        Mine,
        Farm
    }

    

    public static class Data
    {
        public static Dictionary<string, Building> BuildingTypeConv = new Dictionary<string, Building>()
        {
            {"house", GetBuildingDataForType(BuildingType.House) },
            {"woodcutter", GetBuildingDataForType(BuildingType.Woodcutter) },
            {"mine", GetBuildingDataForType(BuildingType.Mine) },
            {"farm", GetBuildingDataForType(BuildingType.Farm) },
        };

        public static Building GetBuildingDataForType(BuildingType type)
        {
            return BuildingList.Where((b) => b.BuildingType == type).First();
        }

        //This contains all the buildings, new ones are added here
        public static List<Building> BuildingList = new()
        {
            new Building(BuildingType.House, TileType.Field, 30, 30, false),
            new Building(BuildingType.Woodcutter, TileType.Forest, 30, 30, true),
            new Building(BuildingType.Mine, TileType.Mountain, 30, 30, true),
            new Building(BuildingType.Farm, TileType.Field, 30, 30, true),
        };

    }
    


}
