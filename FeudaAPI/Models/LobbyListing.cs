using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class LobbyListing
    {
        public string LobbyIdentifier { get; set; }
        public int PlayerCount { get; set; }
        public string LobbyName { get; set; }

        public LobbyListing(string lobbyIdentifier, int playerCount, string lobbyName)
        {
            LobbyIdentifier = lobbyIdentifier;
            PlayerCount = playerCount;
            LobbyName = lobbyName;
        }

        public LobbyListing(Lobby lobby)
        {
            LobbyIdentifier = lobby.LobbyIdentifier;
            PlayerCount = lobby.ConnectedPlayers.Count;
            LobbyName = lobby.GameName;
        }
    }
}
