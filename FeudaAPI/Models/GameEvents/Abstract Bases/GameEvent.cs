using FeudaAPI.GameEvents;
using FeudaAPI.Models.Data;

namespace FeudaAPI.Models.GameEvents
{
    public abstract class GameEvent : GenericEvent
    {
        protected Game game;
        protected GameEvent(Game game, int? turnsAffected, int takesEffectInTurns, string description, EventType eventType) : base(turnsAffected, takesEffectInTurns, description, eventType)
        {
            this.game = game;
        }
    }
}
