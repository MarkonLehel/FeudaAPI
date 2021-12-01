using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models.Data
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
        Farm,
        Town
    }

    public enum Seasons
    {
        Summer = 0,
        Fall = 1,
        Winter = 2,
        Spring = 3
    }
    

    public static class Data
    {
        public static Random random = new Random();

        public static List<Building> BuildingList = new()
        {
            new Building(BuildingType.House, TileType.Field, 30, 30, false),
            new Building(BuildingType.Woodcutter, TileType.Forest, 30, 30, true),
            new Building(BuildingType.Mine, TileType.Mountain, 30, 30, true),
            new Building(BuildingType.Farm, TileType.Field, 30, 30, true),
            new Building(BuildingType.Town, TileType.Field, 0, 0, false),
        };

        public static List<SeasonData> SeasonList = new()
        {
            new SeasonData(0,null,-1,0,null,0,0,null,0),
            new SeasonData(0, 1, -1, 0, null, -3, 0, null, 0)
        };


        public static Dictionary<string, Building> BuildingTypeConv = new Dictionary<string, Building>()
        {
            {"house", GetBuildingDataForType(BuildingType.House) },
            {"woodcutter", GetBuildingDataForType(BuildingType.Woodcutter) },
            {"mine", GetBuildingDataForType(BuildingType.Mine) },
            {"farm", GetBuildingDataForType(BuildingType.Farm) },
        };

        public static Dictionary<Seasons, SeasonData> SeasonTypeConv = new Dictionary<Seasons, SeasonData>()
        {
            {Seasons.Summer, SeasonList[0] },
            {Seasons.Fall, SeasonList[0] },
            {Seasons.Winter, SeasonList[1] },
            {Seasons.Spring, SeasonList[0] }
        };

        public static Building GetBuildingDataForType(BuildingType type)
        {
            return BuildingList.Where((b) => b.BuildingType == type).First();
        }

        //This contains all the buildings, new ones are added here
        

    }
    


}
