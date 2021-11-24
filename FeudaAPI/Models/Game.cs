using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Game
    {


        public bool IsRunning { get; set; }
        private int _tick = 0;
        public int CurrentSeason { get; set; }
        public int TurnCount { get; set; }


        
    }
}
