using FeudaAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Services
{
    public class GameDataService
    {
        public List<Lobby> activeGames { get; } = new();

        public Dictionary<string, Lobby> lobbyDict { get; } = new();
        public List<string> lobbyNamesInUse { get; } = new();


        public Lobby GetLobby(string lobbyIdentifier)
        {
            lobbyDict.TryGetValue(lobbyIdentifier, out Lobby lobby);
            return lobby;
        }


        public string AddLobby(string lobbyName, string hostConnectionID, string hostName) {
            Debug.WriteLine("AddLobby has been called");
            if (!lobbyNamesInUse.Contains(lobbyName))
            {
                string lobbyIdentifier = Guid.NewGuid().ToString();

                lobbyDict.Add(lobbyIdentifier, new Lobby(hostConnectionID, lobbyIdentifier, lobbyName, hostName));
                Debug.WriteLine("Lobby successfully added");
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
        public void StartGame(string lobbyIdentifier, string hostConnectionID) {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            lobby.Game.IsRunning = true;
            activeGames.Add(lobby);
            }
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
    }

}
