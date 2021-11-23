using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Lobby
    {
        public Lobby(string hostConnectionID, string lobbyIdentifier, string gameName, string hostName)
        {
            HostConnectionID = hostConnectionID;
            LobbyIdentifier = lobbyIdentifier;
            GameName = gameName;
            connectedPlayers.Add(new Player(hostConnectionID, hostName));
        }

        public string HostConnectionID { get; }
        public string LobbyIdentifier { get; }
        public string GameName { get; }
        public List<Player> connectedPlayers { get; }
        public List<Message> lobbyMessages { get; } = new();
        public Game Game { get; }


        public void AddLobbyMessage(Message message)
        {
            lobbyMessages.Add(message);
        }

        #region Player
        public void AddPlayer(string connectionID, string playerName)
        {
            connectedPlayers.Add(new Player(connectionID, playerName));
        }

        public void RemovePlayer(string connectionID)
        {
            connectedPlayers.Remove(connectedPlayers.Where(p => p.ConnectionID == connectionID).First());
        }

        public bool IsPlayerConnected(string connectionID)
        {
            foreach (Player player in connectedPlayers)
            {
                if (player.ConnectionID == connectionID)
                    return true;
            }
            return false;
        }

        public Player GetPlayerByConnectionID(string connectionID)
        {
            return connectedPlayers.Where(p => p.ConnectionID == connectionID).First();
        }
        #endregion
    }
}
