using FeudaAPI.Models;

namespace FeudaAPI.GameEvents
{
    public abstract class GenericEvent
    {

        //Adding an allowedInSeason is possible, allowing for events to be only effective in specific seasons
        public int? turnsAffected;
        public int takesEffectInTurns;
        public Game game;

        public string description = "";

        protected GenericEvent(Game game, int? turnsAffected, int takesEffectInTurns, string description)
        {
            this.game = game;
            this.turnsAffected = turnsAffected;
            this.takesEffectInTurns = takesEffectInTurns;
            this.description = description;
        }

        public virtual void TriggerEffectsOnStart() { }
        public virtual void TriggerEffectsPerTurn() { }

        #region EventResourceEffects
        public virtual int EffectFoodIncome(int currentFoodIncome)
        {
            return currentFoodIncome;
        }

        public virtual int EffectWoodIncome(int currentWoodIncome)
        {
            return currentWoodIncome;
        }

        public virtual int EffectOreIncome(int currentOreIncome)
        {
            return currentOreIncome;
        }
        #endregion
    }
}
