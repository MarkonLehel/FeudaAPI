using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Player
    {


        public string ConnectionID { get; }
        public string PlayerName { get; }
        public Player(string connectionID, string name)
        {
            ConnectionID = connectionID;
            PlayerName = name;
        }
    }
}
