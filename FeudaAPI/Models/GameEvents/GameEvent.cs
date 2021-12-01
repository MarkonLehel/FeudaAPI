using FeudaAPI.Models;

namespace FeudaAPI.GameEvents
{
    public abstract class GameEvent
    {


        public int? turnsAffected;
        public int takesEffectInTurns;
        public Game game;

        protected GameEvent(Game game, int? turnsAffected, int takesEffectInTurns)
        {
            this.game = game;
            this.turnsAffected = turnsAffected;
            this.takesEffectInTurns = takesEffectInTurns;
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
