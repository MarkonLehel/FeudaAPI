using FeudaAPI.GameEvents;
using FeudaAPI.Models.DataHolder;
using System.Collections.Generic;
using System.Linq;

namespace FeudaAPI.Models
{
    public struct TurnDataObject
    {
        public bool isAlive;
        public int survivedUntil;

        public int woodIncomeLastTurn;
        public int foodIncomeLastTurn;
        public int oreIncomeLastTurn;

        public int woodCount;
        public int foodCount;
        public int oreCount;

        public int serfCount;
        public int score;
        public int incomingEventAwareness;

        public Board playerBoard;
        public Seasons currentSeason;
        public int turnCount;
        public List<GameEvent> seeableEventsForPlayer;
        public List<GameEvent> eventsInEffect;

        public TurnDataObject(Player player, Game game)
        {
            isAlive = player.IsAlive;
            survivedUntil = player.SurvivedUntilTurn != null ? (int)player.SurvivedUntilTurn : game.TurnCount;

            woodIncomeLastTurn = player.WoodIncomeLastTurn;
            foodIncomeLastTurn = player.FoodIncomeLastTurn;
            oreIncomeLastTurn = player.OreIncomeLastTurn;

            woodCount = player.WoodCount;
            foodCount = player.FoodCount;
            oreCount = player.OreCount;

            serfCount = player.SerfCount;
            score = player.currentScore;
            incomingEventAwareness = player.incomingEventAwareness;

            playerBoard = player.PlayerBoard;
            currentSeason = game.CurrentSeason;
            turnCount = game.TurnCount;

            seeableEventsForPlayer = (List<GameEvent>)game.upcomingGameEvents.Concat(player.upcomingPlayerEvents).
                Where(ev => ev.takesEffectInTurns <= player.incomingEventAwareness);
            eventsInEffect = (List<GameEvent>)game.activeGameEvents.Concat(player.activePlayerEvents);

            //Include boards of allies
            //Include boards of players that have died
        }
    }
}
