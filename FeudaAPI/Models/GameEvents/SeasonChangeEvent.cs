
namespace FeudaAPI.Models.GameEvents
{
    public class SeasonChangeEvent : GameEvent
    {
        public SeasonChangeEvent(Game game) : base(game, null, 50, "A season change.", Data.EventType.SeasonChange)
        {

        }

        public override void TriggerEffectsOnStart()
        {
            game.ChangeToNextSeason();
            game.AddGameEvent(new SeasonChangeEvent(game));
        }
    }
}
