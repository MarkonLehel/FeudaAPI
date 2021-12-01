using FeudaAPI.GameEvents;
using System.Collections.Generic;

namespace FeudaAPI.Models
{
    public class Player
    {
        public Player(string connectionID, string name)
        {
            ConnectionID = connectionID;
            PlayerName = name;
            PlayerBoard = new Board();
        }
        public int? SurvivedUntilTurn { get; set; } = null;
        public bool IsAlive { get; set; }

        public int WoodIncomeLastTurn { get; set; }
        public int FoodIncomeLastTurn { get; set; }
        public int OreIncomeLastTurn { get; set; }

        public int WoodCount { get; set; } = 80;
        public int FoodCount { get; set; } = 40;
        public int OreCount { get; set; } = 80;
        public int SerfCount { get; set; } = 1;
        public int NumberOfBuildings { get; set; } = 0;

        public Board PlayerBoard { get; set; }
        public string ConnectionID { get; }
        public string PlayerName { get; }

        public List<GameEvent> upcomingPlayerEvents { get; } = new();
        public List<GameEvent> activePlayerEvents { get; } = new();
        public int incomingEventAwareness { get; set; } = 20;
        public int currentScore { get; set; }

        public void AdvancePlayerEvents()
        {
            if (activePlayerEvents.Count > 0)
            {
                foreach (GameEvent ev in upcomingPlayerEvents)
                {
                    if (ev.takesEffectInTurns == 0)
                    {
                        ev.TriggerEffectsOnStart();
                        activePlayerEvents.Add(ev);
                        upcomingPlayerEvents.Remove(ev);
                    }
                    else
                    {
                        ev.takesEffectInTurns--;
                    }
                }
            }

            if (activePlayerEvents.Count > 0) { 
                foreach (GameEvent ev in activePlayerEvents)
                {
                    if (ev.turnsAffected == 0)
                    {
                        activePlayerEvents.Remove(ev);
                    }
                    else
                    {
                        ev.TriggerEffectsPerTurn();
                        ev.turnsAffected--;
                    }
                }
            }
        }
    }
}
