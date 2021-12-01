using FeudaAPI.Models.DataHolder;
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
        
        private Seasons CurrentSeason { get; set; } = Seasons.Summer;
        private SeasonData CurrentSeasonData { get; set; } = Data.SeasonTypeConv[Seasons.Summer];
        public int TurnCount { get; set; } = 1;

        private List<GameEvent> upcomingGameEvents = new();
        private List<GameEvent> activeGameEvents = new();


        public void CalculateTurn()
        {
            TurnCount++;
            AdvanceGameEvents();
            

            
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

        //Implement season data into the calculation
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

        private void ChangeToNextSeason() {
            int seasonIndex = (int)CurrentSeason;


            if (seasonIndex < 3)
            {
                CurrentSeason = (Seasons)seasonIndex + 1;
            } else
            {
                CurrentSeason = 0;
            }
            CurrentSeasonData = Data.SeasonTypeConv[CurrentSeason];
        }

        private void AdvanceGameEvents()
        {
            foreach (GameEvent ev in activeGameEvents)
            {
                if (ev.turnsAffected == 0)
                {
                    activeGameEvents.Remove(ev);
                }
                else
                {
                    ev.turnsAffected--;
                }
            }

            foreach (GameEvent ev in upcomingGameEvents)
            {
                if (ev.takesEffectInTurns == 0)
                {
                    activeGameEvents.Add(ev);
                }
                else
                {
                    ev.takesEffectInTurns--;
                }
            }
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
            int _foodIncome = player.SerfCount * CurrentSeasonData.perSerfFoodModifier;
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
                            case TileType.Mountain:
                                if(CurrentSeasonData.directOreModifier == null) { 
                                _oreIncome += tile.HasImprovement ?
                                    (tile.BaseTileIncome * 2) + CurrentSeasonData.additionalOreIncomeModifier
                                    : tile.BaseTileIncome;
                                } else {
                                    _oreIncome = (int)CurrentSeasonData.directOreModifier;
                                }
                                break;

                            case TileType.Forest:
                                if(CurrentSeasonData.directWoodModifier == null) { 
                                _woodIncome += tile.HasImprovement ?
                                    (tile.BaseTileIncome * 2) + CurrentSeasonData.additionalWoodIncomeModifier
                                    : tile.BaseTileIncome;
                                } else {
                                    _woodIncome = (int)CurrentSeasonData.directWoodModifier;
                                }
                                break;

                            case TileType.Field:
                                if(CurrentSeasonData.directFoodModifier == null) { 
                                _foodIncome += tile.HasImprovement ?
                                    (tile.BaseTileIncome * 2) + CurrentSeasonData.additionalFoodIncomeModifier
                                    : tile.BaseTileIncome;
                                } else {
                                    _foodIncome = (int)CurrentSeasonData.directFoodModifier;
                                }
                                break;
                        }
                    }
                }
            }
            //Apply GameEventModifiers here
            player.FoodCount += _foodIncome;
            player.WoodCount += _woodIncome;
            player.OreCount += _oreIncome;

        }
    }
}
