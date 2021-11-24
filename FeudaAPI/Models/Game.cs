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

        public void BuildBuilding(Player player, Building building, Coordinate pos)
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
            }
        }

        public void CalculateTurnForPlayer(Player player)
        {
            RefreshPlayerResources(player);

            if(player.SerfCount > 0 && player.FoodCount < 0)
            {
                player.FoodCount = 0;
                KillRandomSerf(player);
            } else if (player.SerfCount == 0) {
                player.IsAlive = false;
            } else {

            }
            
                
        }


        private void KillRandomSerf(Player player)
        {
            
        }

        private void SpawnRandomSerf(Player player)
        {

        }

        private void RefreshPlayerResources(Player player)
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
