using FeudaAPI.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Lobby
    {
        public Lobby( string hostConnectionID, string lobbyIdentifier, string gameName, string hostName)
        {
            HostConnectionID = hostConnectionID;
            LobbyIdentifier = lobbyIdentifier;
            GameName = gameName;
            ConnectedPlayers.Add(new Player(hostConnectionID, hostName));
        }

        public string HostConnectionID { get; }
        public string LobbyIdentifier { get; }
        public string GameName { get; }
        public List<Player> ConnectedPlayers { get; }
        public List<Message> LobbyMessages { get; } = new();
        public List<Message> GameMessages { get; set; }
        public List<string> KicketClientIDs { get; set; } = new();
        public Game Game { get; }

        #region Messages
        public void AddLobbyMessage(Message message)
        {
            LobbyMessages.Add(message);
        }
        public void AddGameMessage(Message message)
        {
            GameMessages.Add(message);
        }
        #endregion

        #region Player
        public void AddPlayer(string connectionID, string playerName)
        {
            ConnectedPlayers.Add(new Player(connectionID, playerName));
        }

        public void AddToKickList(string connectionID)
        {
            if (!KicketClientIDs.Contains(connectionID))
                KicketClientIDs.Add(connectionID);
        }

        public void RemovePlayer(string connectionID)
        {
            ConnectedPlayers.Remove(ConnectedPlayers.Where(p => p.ConnectionID == connectionID).First());
        }

        public bool IsPlayerConnected(string connectionID)
        {
            foreach (Player player in ConnectedPlayers)
            {
                if (player.ConnectionID == connectionID)
                    return true;
            }
            return false;
        }

        public Player GetPlayerByConnectionID(string connectionID)
        {
            return ConnectedPlayers.Where(p => p.ConnectionID == connectionID).First();
        }

        public Player GetPlayerByName(string name)
        {
            return ConnectedPlayers.Where(p => p.PlayerName == name).First();
        }
        #endregion
    }
}
