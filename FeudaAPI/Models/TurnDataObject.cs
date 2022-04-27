using FeudaAPI.Models.Data;
using FeudaAPI.Models.GameEvents;
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

        public GameBoard playerBoard;
        public Seasons currentSeason;
        public int turnCount;
        public List<GenericEvent> seeableEventsForPlayer;
        public List<GenericEvent> eventsInEffect;

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

            seeableEventsForPlayer = new();
            seeableEventsForPlayer.AddRange(game.upcomingGameEvents
                .Where(ev => ev.takesEffectInTurns <= player.incomingEventAwareness));
            seeableEventsForPlayer.AddRange(player.upcomingPlayerEvents
                .Where(ev => ev.takesEffectInTurns <= player.incomingEventAwareness));

            eventsInEffect = new();
            eventsInEffect.AddRange(game.activeGameEvents);
            eventsInEffect.AddRange(player.activePlayerEvents);



            //Include boards of allies
            //Include boards of players that have died
        }
    }
}
