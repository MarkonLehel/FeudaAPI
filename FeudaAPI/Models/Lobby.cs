﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Models
{
    public class Lobby
    {

        public string HostConnectionID { get; }
        public string LobbyIdentifier { get; }
        public string GameName { get; }

        public List<Player> connectedPlayers { get; }

        List<Message> lobbyMessages = new();
        public Game Game { get; }

        public Lobby(string hostConnectionID, string lobbyIdentifier, string gameName, string hostName)
        {
            HostConnectionID = hostConnectionID;
            LobbyIdentifier = lobbyIdentifier;
            GameName = gameName;
            connectedPlayers.Add(new Player(hostConnectionID, hostName));
        }







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

    }
}