using FeudaAPI.Models;
using FeudaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Hubs
{
    public class GameHub : Hub<IGameHubClient>
    {

        public override async Task<IActionResult> OnConnectedAsync()
        {
            Debug.WriteLine("-----------------------------Client connection.");
            return new OkResult();
        }

        #region Lobby
        //TODO: Use the identifier received from the URL instead of passing it as first parameter to most functions

        //TODO: In case of a lobby browser, implement OnConnectedAsync and OnDisconnectedAsync to send data to a lobby browser
        //TODO: Add logging to the entire class

        public async Task<IActionResult> CreateLobby(string lobbyName, string hostName)
        {
            Debug.WriteLine("-----------------------------Received a request to start the lobby");
            //Check if lobby exists with the current name, if it does send conflict result
            if (!GameService.lobbyNamesInUse.Contains(lobbyName))
            {
                string lobbyIdentifier = Guid.NewGuid().ToString();

                GameService.lobbyDict.Add(lobbyIdentifier, new Lobby(this, Context.ConnectionId, lobbyIdentifier, lobbyName, hostName));
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyIdentifier);

                return new JsonResult(lobbyIdentifier);
            }
            return new ConflictResult();
        }

        public async void CloseLobby(string lobbyIdentifier)
        {
            Lobby lobby = GameService.lobbyDict[lobbyIdentifier];
            //Check if the request came from the host
            if (Context.ConnectionId == lobby.HostConnectionID && !lobby.Game.IsRunning)
            {
                //Send disconnect to all clients in the game group so they can terminate connection
                await Clients.Group(lobbyIdentifier).disconnectFromGame("The host closed the lobby.");
                //Remove all players from group, therefore deleting the entire group
                List<Player> playerList = lobby.ConnectedPlayers;
                foreach (Player player in playerList)
                {
                    await Groups.RemoveFromGroupAsync(player.ConnectionID, lobbyIdentifier);
                }

                //Remove game from dictionary
                GameService.lobbyDict.Remove(lobbyIdentifier);
                GameService.lobbyNamesInUse.Remove(lobby.GameName);
            }
        }

        public async Task<IActionResult> ConnectToLobby(string lobbyIdentifier, string playerName)
        {
            if (GameService.lobbyDict.TryGetValue(lobbyIdentifier, out Lobby lobby))
            {
                if (!lobby.IsPlayerConnected(Context.ConnectionId) && 
                   !lobby.KicketClientIDs.Contains(Context.ConnectionId) &&
                   !lobby.Game.IsRunning)
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
            if (GameService.lobbyDict.TryGetValue(lobbyIdentifier, out Lobby lobby))
            {
                //Remove client from lobby
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyIdentifier);
                lobby.RemovePlayer(Context.ConnectionId);

                //Update info on other clients
                await Clients.OthersInGroup(lobbyIdentifier).updateLobbyPlayers(lobby.ConnectedPlayers);
            }
        }

        public async void KickPlayerFromLobby(string lobbyIdentifier, string playerName)
        {
            Lobby lobby = GameService.lobbyDict[lobbyIdentifier];
            if (Context.ConnectionId == lobby.HostConnectionID && !lobby.Game.IsRunning)
            {
                string clientConnectionID = lobby.GetPlayerByName(playerName).ConnectionID;

                //Remove the client from the group and the server
                await Clients.Client(clientConnectionID).disconnectFromGame("You have been kicked by the host!");
                await Groups.RemoveFromGroupAsync(clientConnectionID, lobbyIdentifier);

                //Blacklist the player
                lobby.AddToKickList(clientConnectionID);
                lobby.RemovePlayer(clientConnectionID);

                //Update data on other clients
                SendUpdateLobbyPlayers(lobbyIdentifier);
            }
        }
        #endregion

        #region Messaging
        public async Task<IActionResult> SendMessage(string lobbyIdentifier, string message)
        {
            Lobby lobby = GameService.lobbyDict[lobbyIdentifier];
            Message msg = new Message(lobby.GetPlayerByConnectionID(Context.ConnectionId).PlayerName, message);
            if (!lobby.Game.IsRunning)
            {
                lobby.AddLobbyMessage(msg);
            } else
            {
                lobby.AddGameMessage(msg);
            }
            await Clients.OthersInGroup(lobbyIdentifier).getNewMessage( msg);
            return new OkResult();
        }


        public async Task<List<Message>> GetMessages(string lobbyIdentifier)
        {
            Lobby lobby = GameService.lobbyDict[lobbyIdentifier];
            return await Task.Run(() => { return lobby.LobbyMessages; });
        }

        #endregion

        #region Game
        public async void InitGame()
        {


        }

        private async void RunGame(string lobbyIdentifier)
        {

        }


        public async void MoveSerf()
        {

        }

        public async void BuildBuilding()
        {

        }



        private async void SendTurnUpdateGameData()
        {

        }

        private async void EndGame()
        {

        }




        #endregion

        private async void SendUpdateLobbyPlayers(string lobbyIdentifier)
        {
            Lobby lobby = GameService.lobbyDict[lobbyIdentifier];
            await Clients.Group(lobbyIdentifier).updateLobbyPlayers(lobby.ConnectedPlayers);
        }
    }
}
