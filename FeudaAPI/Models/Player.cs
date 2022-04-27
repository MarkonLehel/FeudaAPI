using FeudaAPI.Models.Factories;
using FeudaAPI.Models.GameEvents;
using System.Collections.Generic;

namespace FeudaAPI.Models
{
    public class Player
    {
        public Player(string connectionID, string name)
        {
            ConnectionID = connectionID;
            PlayerName = name;
        }
        public int? SurvivedUntilTurn { get; set; } = null;
        public bool IsAlive { get; set; } = true;

        public int WoodIncomeLastTurn { get; set; }
        public int FoodIncomeLastTurn { get; set; }
        public int OreIncomeLastTurn { get; set; }

        public int WoodCount { get; set; } = 80;
        public int FoodCount { get; set; } = 40;
        public int OreCount { get; set; } = 80;
        public int SerfCount { get; set; } = 1;
        public int NumberOfBuildings { get; set; } = 0;

        public GameBoard PlayerBoard { get; set; }
        public string ConnectionID { get; }
        public string PlayerName { get; }

        public List<PlayerEvent> upcomingPlayerEvents { get; } = new();
        public List<PlayerEvent> activePlayerEvents { get; } = new();
        public int incomingEventAwareness { get; set; } = 20;
        public int currentScore { get; set; }
    }
}
