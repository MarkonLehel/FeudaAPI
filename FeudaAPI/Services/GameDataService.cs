using FeudaAPI.Models;
using System;
using System.Collections.Generic;

namespace FeudaAPI.Services
{
    public class GameDataService
    {
        public List<Lobby> activeGames { get; } = new();
        public Dictionary<string, Lobby> lobbyDict { get; } = new();
        public List<string> lobbyNamesInUse { get; } = new();

        #region Lobby
        public Lobby GetLobby(string lobbyIdentifier)
        {
            lobbyDict.TryGetValue(lobbyIdentifier, out Lobby lobby);
            return lobby;
        }
        public string AddLobby(string lobbyName, string hostConnectionID, string hostName) {
            if (!lobbyNamesInUse.Contains(lobbyName))
            {
                string lobbyIdentifier = Guid.NewGuid().ToString();

                lobbyDict.Add(lobbyIdentifier, new Lobby(hostConnectionID, lobbyIdentifier, lobbyName, hostName));
                return lobbyIdentifier;
            } else {
                throw new Exception("Lobby name already in use.");
            }
        }
        public void RemoveLobby(string lobbyIdentifier, string connectionID) {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            lobbyDict.Remove(lobbyIdentifier);
            lobbyNamesInUse.Remove(lobby.GameName);
        }
        public void AddPlayerToLobby(string lobbyIdentifier, string connectionID, string playerName) {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            lobby.AddPlayer(connectionID, playerName);
        }
        public void RemovePlayerFromLobby(string lobbyIdentifier, string connectionID)
        {
            lobbyDict[lobbyIdentifier].RemovePlayer(connectionID);
        }
        public void KickPlayerFromLobby(string lobbyIdentifier, string playerName) {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            Player player = lobby.GetPlayerByName(playerName);
            RemovePlayerFromLobby(lobbyIdentifier, player.ConnectionID);
            lobby.AddToKickList(player.ConnectionID);
        }
        #endregion

        #region Messaging
        public Message AddMessageToLobby(string lobbyIdentifier, string connectionID, string message) {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            Message msg = new Message(lobby.GetPlayerByConnectionID(connectionID).PlayerName, message);
            if (!lobby.Game.IsRunning)
            {
                lobby.AddLobbyMessage(msg);
            }
            else
            {
                lobby.AddGameMessage(msg);
            }
            return msg;
        }
        public List<Message> GetMessagesForLobby(string lobbyIdentifier) {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            if (!lobby.Game.IsRunning)
            {
                return lobby.LobbyMessages;
            }
            else
            {
                return lobby.GameMessages;
            }
        }
        #endregion

        #region Game
        public void StartGame(string lobbyIdentifier)
        {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            lobby.Game.IsRunning = true;
            activeGames.Add(lobby);
        }

        public bool BuildBuilding(string lobbyIdentifier, string clientID, string building, Coordinate cords)
        {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            Player player = lobby.GetPlayerByConnectionID(clientID);
            return lobby.Game.BuildBuilding(player, Models.DataHolder.Data.BuildingTypeConv[building], cords);
        }

        public bool MoveSerf(string lobbyIdentifier, string clientID, Coordinate from, Coordinate to)
        {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            Player player = lobby.GetPlayerByConnectionID(clientID);
            return lobby.Game.MoveSerf(player, from, to);
        }
        #endregion
    }

}
