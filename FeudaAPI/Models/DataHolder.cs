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
        public static Dictionary<string, BuildingType> BuildingTypeConv = new Dictionary<string, BuildingType>()
        {
            {"house", BuildingType.House },
            {"woodcutter", BuildingType.Woodcutter },
            {"mine", BuildingType.Mine },
            {"farm", BuildingType.Farm },
        };

        public static Building GetBuildingDataForType(BuildingType type)
        {
            return BuildingList.Where((b) => b.BuildingType == type).First();

        }
        public static List<Building> BuildingList = new()
        {
            new Building(BuildingType.House, TileType.Field, 30, 30),
            new Building(BuildingType.Woodcutter, TileType.Forest, 30, 30),
            new Building(BuildingType.Mine, TileType.Mountain, 30, 30),
            new Building(BuildingType.Farm, TileType.Field, 30, 30),
        };

    }
    


}
