using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Game
    {



        public bool IsRunning { get; set; } = false;
        private int _tick = 0;
        public int CurrentSeason { get; set; }
        public int TurnCount { get; set; } = 1;



        public void Run()
        {

        }

        public void BuildBuilding(Player player, Building building, Coordinate pos)
        {
            //if()


        }

        public void CalculateTurnForPlayer(Player player)
        {

        }


        private void CalculatePlayerResources()
        {

        }
        
    }
}
