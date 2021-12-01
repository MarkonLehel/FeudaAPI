using FeudaAPI.GameEvents;
using FeudaAPI.Models.DataHolder;
using System;
using System.Collections.Generic;

namespace FeudaAPI.Models
{
    public class Game
    {



        public bool IsRunning { get; set; } = false;
        private int _tick = 0;
        
        public Seasons CurrentSeason { get; set; } = Seasons.Summer;
        private SeasonData CurrentSeasonData { get; set; } = Data.SeasonTypeConv[Seasons.Summer];
        public int TurnCount { get; set; } = 1;

        public List<GameEvent> upcomingGameEvents { get; } = new();
        public List<GameEvent> activeGameEvents { get; } = new();


        public Dictionary<string, TurnDataObject> CalculateTurn(List<Player> playerList)
        {
            Dictionary<string, TurnDataObject> dataDict = new();
            TurnCount++;
            foreach (Player player in playerList)
            {
                dataDict.Add(player.ConnectionID, CalculateTurnForPlayer(player));
            }
            AdvanceEvents();
            return dataDict;
        }

        public TurnDataObject CalculateTurnForPlayer(Player player)
        {

            CalculatePlayerResources(player);

            CheckPlayerStatus(player);

            CalculatePlayerScore(player);

            return new TurnDataObject();
        }

        #region Player actions
        public bool BuildBuilding(Player player, Building building, Coordinate pos)
        {
            Tile tile = player.PlayerBoard.GetTile(pos);
            if (player.OreCount >= building.OrePrice && player.WoodCount >= building.WoodPrice && !tile.HasBuilding)
            {
                player.OreCount -= building.OrePrice;
                player.WoodCount -= building.WoodPrice;
                tile.HasImprovement = building.IsTileImprovement;
                tile.HasBuilding = true;
                if (building.BuildingType == BuildingType.House)
                    SpawnRandomSerf(player);
                player.NumberOfBuildings++;
                return true;
            }
            return false;
        }
        public bool MoveSerf(Player player, Coordinate from, Coordinate to)
        {
            return player.PlayerBoard.MoveSerf(from, to);
        }
        #endregion
        
        #region Callable events
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
        #endregion

        private void AdvanceEvents()
        {
            if (upcomingGameEvents.Count > 0)
            {
                foreach (GameEvent ev in upcomingGameEvents)
                {
                    if (ev.takesEffectInTurns == 0)
                    {
                        ev.TriggerEffectsOnStart();
                        activeGameEvents.Add(ev);
                        upcomingGameEvents.Remove(ev);
                    }
                    else
                    {
                        ev.takesEffectInTurns--;
                    }
                }
            }

            if (activeGameEvents.Count > 0) 
            { 
                foreach (GameEvent ev in activeGameEvents)
                {
                    if (ev.turnsAffected == 0)
                    {
                        activeGameEvents.Remove(ev);
                    } else {
                        ev.TriggerEffectsPerTurn();
                        ev.turnsAffected--;
                    }
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
                    if (tile.Building.BuildingType == BuildingType.House ||
                        tile.Building.BuildingType == BuildingType.Town)
                        validSpawns.Add(tile);
                }
            }
            validSpawns[DataHolder.Data.random.Next(validSpawns.Count)].HasSerf = true;
        }

        #region Calculations
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
                                _oreIncome += Math.Clamp(CalculateResourceIncome(tile,
                                    CurrentSeasonData.additionalOreIncomeModifier,
                                    CurrentSeasonData.directOreModifier), 0 ,10);
                                break;

                            case TileType.Forest:
                                _woodIncome += Math.Clamp(CalculateResourceIncome(tile,
                                    CurrentSeasonData.additionalWoodIncomeModifier,
                                    CurrentSeasonData.directWoodModifier), 0, 10);
                                break;

                            case TileType.Field:
                                _foodIncome += Math.Clamp(CalculateResourceIncome(tile,
                                      CurrentSeasonData.additionalFoodIncomeModifier,
                                      CurrentSeasonData.directFoodModifier), 0, 10);
                                break;
                        }
                    }
                }
            }

            //Posibility to add events that are season specific here
            List<GameEvent> totalActiveEventsForPlayer = new();
            totalActiveEventsForPlayer.AddRange(activeGameEvents);
            totalActiveEventsForPlayer.AddRange(player.activePlayerEvents);
            if (activeGameEvents.Count > 0) {
                foreach (GameEvent ev in totalActiveEventsForPlayer)
                {
                    _foodIncome = ev.EffectFoodIncome(_foodIncome);
                    _woodIncome = ev.EffectWoodIncome(_woodIncome);
                    _oreIncome = ev.EffectOreIncome(_oreIncome);
                }
            }

            player.FoodIncomeLastTurn = _foodIncome;
            player.WoodIncomeLastTurn = _woodIncome;
            player.OreIncomeLastTurn = _oreIncome;

            player.FoodCount += _foodIncome;
            player.WoodCount += _woodIncome;
            player.OreCount += _oreIncome;
        }
        private int CalculatePlayerScore(Player player)
        {
            return player.SerfCount * 10 +
                player.OreCount + player.WoodCount + player.FoodCount +
                player.NumberOfBuildings * 5 +
                player.SurvivedUntilTurn != null ? (int)player.SurvivedUntilTurn : 0;
        }
        private void CheckPlayerStatus(Player player)
        {
            if (player.SerfCount > 0 && player.FoodCount < 0)
            {
                player.FoodCount = 0;
                KillRandomSerf(player);
                player.SerfCount--;
            }
            else if (player.SerfCount == 0)
            {
                player.IsAlive = false;
                player.SurvivedUntilTurn = TurnCount;
            }
        }
        private int CalculateResourceIncome(Tile tile, int additionalModifier, int? directModifier)
        {
            int value = 0;
            if (directModifier == null)
            {
                value += tile.HasImprovement ? (tile.BaseTileIncome * 2) : tile.BaseTileIncome;
                value += additionalModifier;
            } else {
                value = (int)CurrentSeasonData.directFoodModifier;
            }
            return value;
        }
        #endregion
    }
}
