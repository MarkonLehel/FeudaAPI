using FeudaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Hubs
{
    public class GameHub : Hub
    {
        Dictionary<string, Lobby> lobbyDict = new();
        List<string> lobbyNamesInUse = new();

        #region Lobby
        //TODO: Use the identifier received from the URL instead of the call
        public async Task<IActionResult> CreateLobby(string lobbyName, string hostName)
        {
            //Check if lobby exists with the current name, if it does send conflict result
            if (!lobbyNamesInUse.Contains(lobbyName))
            {
                string lobbyIdentifier = Guid.NewGuid().ToString();

                lobbyDict.Add(lobbyIdentifier, new Lobby(Context.ConnectionId, lobbyIdentifier, lobbyName, hostName));
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyIdentifier);

                return new JsonResult(lobbyIdentifier);
            }
            return new ConflictResult();
        }

        public async void CloseLobby(string lobbyIdentifier)
        {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            //Check if the request came from the host
            if (Context.ConnectionId == lobby.HostConnectionID)
            {
                //Send disconnect to all clients in the game group so they can terminate connection
                await Clients.Group(lobbyIdentifier).SendAsync("disconnectFromGame", "The host closed the lobby.");

                //Remove all players from group, therefore deleting the entire group
                List<Player> playerList = lobby.ConnectedPlayers;
                foreach (Player player in playerList)
                {
                    await Groups.RemoveFromGroupAsync(player.ConnectionID, lobbyIdentifier);
                }

                //Remove game from dictionary
                lobbyDict.Remove(lobbyIdentifier);
                lobbyNamesInUse.Remove(lobby.GameName);
            }
        }

        public async Task<IActionResult> ConnectToLobby(string lobbyIdentifier, string playerName)
        {
            if (lobbyDict.TryGetValue(lobbyIdentifier, out Lobby lobby))
            {
                if (!lobby.IsPlayerConnected(Context.ConnectionId) && !lobby.KicketClientIDs.Contains(Context.ConnectionId))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, lobbyIdentifier);
                    lobby.AddPlayer(Context.ConnectionId, playerName);

                    //Send update to clients
                    SendUpdateLobbyPlayers(lobbyIdentifier);

                    //Update messages for joined client

                    return new OkResult();
                }
                return new ConflictResult();
            }
            return new NotFoundResult();
        }

        public async void DisconnectFromLobby(string lobbyIdentifier)
        {
            if (lobbyDict.TryGetValue(lobbyIdentifier, out Lobby lobby))
            {
                //Remove client from lobby
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyIdentifier);
                lobby.RemovePlayer(Context.ConnectionId);

                //Update info on other clients
                await Clients.Group(lobbyIdentifier).SendAsync("updateLobbyPlayers", lobby.ConnectedPlayers);
            }
        }

        public async void KickPlayerFromLobby(string lobbyIdentifier, string playerName)
        {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            if (Context.ConnectionId == lobby.HostConnectionID)
            {
                string clientConnectionID = lobby.GetPlayerByName(playerName).ConnectionID;
                await Clients.Client(clientConnectionID).SendAsync("disconnectFromGame", "You have been kicked by the host!");
                await Groups.RemoveFromGroupAsync(clientConnectionID, lobbyIdentifier);
                lobby.AddToKickList(clientConnectionID);
                lobby.RemovePlayer(clientConnectionID);
            }
        }
        #endregion

        #region Messaging
        public void SendMessage(string lobbyIdentifier, string message)
        {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            Message msg = new Message(lobby.GetPlayerByConnectionID(Context.ConnectionId).PlayerName, message);
            lobby.AddLobbyMessage(msg);
            SendUpdateLobbyPlayers(lobbyIdentifier);
        }


        public async Task<List<Message>> GetMessages(string lobbyIdentifier)
        {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            return await Task.Run(() => { return lobby.LobbyMessages; });
        }

        #endregion

        private async void SendUpdateLobbyPlayers(string lobbyIdentifier)
        {
            Lobby lobby = lobbyDict[lobbyIdentifier];
            await Clients.Group(lobbyIdentifier).SendAsync("updateLobbyPlayers", lobby.ConnectedPlayers);
        }
    }
}
