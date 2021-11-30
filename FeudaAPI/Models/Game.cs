using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Game
    {



        public bool IsRunning { get; set; } = false;
        private int _tick = 0;
        public int CurrentSeason { get; set; }
        public int TurnCount { get; set; } = 1;



        public void Run()
        {

        }

        public bool BuildBuilding(Player player, Building building, Coordinate pos)
        {
            Tile tile = player.PlayerBoard.GetTile(pos);
            if (player.OreCount >= building.OrePrice && player.WoodCount >= building.WoodPrice && !tile.HasBuilding)
            {
                player.OreCount -= building.OrePrice;
                player.WoodCount -= building.WoodPrice;
                tile.HasImprovement = building.IsTileImprovement;
                tile.HasBuilding = true;
                if (building.BuildingType == DataHolder.BuildingType.House)
                    SpawnRandomSerf(player);
                return true;
            }
            return false;
        }

        public TurnDataObject CalculateTurnForPlayer(Player player)
        {
            CalculatePlayerResources(player);

            if(player.SerfCount > 0 && player.FoodCount < 0)
            {
                player.FoodCount = 0;
                KillRandomSerf(player);
                player.SerfCount--;
            } else if (player.SerfCount == 0) {
                player.IsAlive = false;
            }

            return new TurnDataObject();

        }


        private void KillRandomSerf(Player player)
        {
            Tile serfTile = player.PlayerBoard.TilesWithSerfs[DataHolder.Data.random.Next(player.PlayerBoard.TilesWithSerfs.Count)];
            serfTile.HasSerf = false;
        }

        private void SpawnRandomSerf(Player player)
        {
            List<Tile> validSpawns = new();
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Tile tile = player.PlayerBoard.GetTile(x, y);
                    if (tile.Building.BuildingType == DataHolder.BuildingType.House ||
                        tile.Building.BuildingType == DataHolder.BuildingType.Town)
                        validSpawns.Add(tile);
                }
            }
            validSpawns[DataHolder.Data.random.Next(validSpawns.Count)].HasSerf = true;
        }

        private void CalculatePlayerResources(Player player)
        {
            int _woodIncome = 0;
            int _foodIncome = player.SerfCount * -1;
            int _oreIncome = 0;

            Board board = player.PlayerBoard;
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Tile tile = board.GetTile(x, y);
                    if (tile.HasSerf)
                    {
                        switch (tile.TileType)
                        {
                            case DataHolder.TileType.Mountain:
                                _oreIncome += tile.HasImprovement ? tile.BaseTileIncome * 2 : tile.BaseTileIncome;
                                break;
                            case DataHolder.TileType.Forest:
                                _woodIncome += tile.HasImprovement ? tile.BaseTileIncome * 2 : tile.BaseTileIncome;
                                break;

                            case DataHolder.TileType.Field:
                                _foodIncome += tile.HasImprovement ? tile.BaseTileIncome * 2 : tile.BaseTileIncome;
                                break;
                        }
                    }
                }
            }
            player.FoodCount += _foodIncome;
            player.WoodCount += _woodIncome;
            player.OreCount += _oreIncome;

        }
    }
}
