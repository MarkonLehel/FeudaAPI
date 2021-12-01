using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
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

        public abstract void takeEffectOnGame();
        
        

    }
}
