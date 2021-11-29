using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Player
    {
        public Player(string connectionID, string name)
        {
            Debug.WriteLine("---Player constructor start");
            ConnectionID = connectionID;
            PlayerName = name;
            PlayerBoard = new Board();
            Debug.WriteLine("---Player constructor end");

        }

        public bool IsAlive { get; set; } = true;
        public int WoodCount { get; set; } = 80;
        public int FoodCount { get; set; } = 40;
        public int OreCount { get; set; } = 80;
        public int SerfCount { get; set; } = 1;

        public Board PlayerBoard { get; set; }
        public string ConnectionID { get; }
        public string PlayerName { get; }
        
    }
}
