using FeudaAPI.Models.Data;

namespace FeudaAPI.Models.GameEvents
{
    public abstract class PlayerEvent : GenericEvent
    {
        Player player;
        protected PlayerEvent(Player player, int? turnsAffected, int takesEffectInTurns, string description, EventType eventType) : base(turnsAffected, takesEffectInTurns, description, eventType)
        {
            this.player = player;
        }
    }
}
