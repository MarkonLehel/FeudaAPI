using FeudaAPI.Models;
using FeudaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FeudaAPI.Hubs
{
    //TODO: This needs a singleton data storage class service to create a connection with the bgservice
    public class GameHub : Hub<IGameHubClient>
    {
        private GameDataService _gameDataService;
        public GameHub( GameDataService gameDataService)
        {
            _gameDataService = gameDataService;
        }

        public override async Task OnConnectedAsync()
        {
            Debug.WriteLine("-----------------------------Client connection.");
        }

        #region Lobby
        //TODO: Use the identifier received from the URL instead of passing it as first parameter to most functions
        //TODO: In case of a lobby browser, implement OnConnectedAsync and OnDisconnectedAsync to send data to a lobby browser
        //TODO: Add logging to the entire class
        //TODO: Change void reutn types to Task
        public async Task<IActionResult> CreateLobby(string lobbyName, string hostName)
        {
            Debug.WriteLine("-----------------------------Received a request to start the lobby");
            try {
                string lobbyIdentifier = _gameDataService.AddLobby(lobbyName, Context.ConnectionId, hostName);
                Debug.WriteLine("A lobby was successfully created with the identifier of " + lobbyIdentifier);
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyIdentifier);
                Debug.WriteLine("Client was added to group.");
                return new JsonResult(lobbyIdentifier);
            } catch { 
            return new ConflictResult();
            }
        }

        public async Task<IActionResult> CloseLobby(string lobbyIdentifier)
        {
            Lobby lobby = _gameDataService.GetLobby(lobbyIdentifier);

            if (Context.ConnectionId == lobby.HostConnectionID && !lobby.Game.IsRunning) { 
                _gameDataService.RemoveLobby(lobbyIdentifier, Context.ConnectionId);

                foreach (Player player in lobby.ConnectedPlayers)
                {
                    await Groups.RemoveFromGroupAsync(player.ConnectionID, lobbyIdentifier);
                }

                return new OkResult();
            } else {
                return new BadRequestResult();
            } 
        }


        public async Task<IActionResult> ConnectToLobby(string lobbyIdentifier, string playerName)
        {
            Lobby lobby = _gameDataService.GetLobby(lobbyIdentifier);
            if (lobby != null)
            {
                if (!lobby.IsPlayerConnected(Context.ConnectionId) && 
                   !lobby.KicketClientIDs.Contains(Context.ConnectionId) &&
                   !lobby.Game.IsRunning)
                {
                    //Add client to group
                    await Groups.AddToGroupAsync(Context.ConnectionId, lobbyIdentifier);
                    _gameDataService.AddPlayerToLobby(lobbyIdentifier, Context.ConnectionId, playerName);

                    //Send update to clients
                    SendUpdateLobbyPlayers(lobbyIdentifier);

                    return new OkResult();
                }
                return new ConflictResult();
            }
            return new NotFoundResult();
        }

        public async Task DisconnectFromLobby(string lobbyIdentifier)
        {
            Lobby lobby = _gameDataService.GetLobby(lobbyIdentifier);
            if (lobby != null)
            {
                //Remove client from lobby
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyIdentifier);
                _gameDataService.RemovePlayerFromLobby(lobbyIdentifier, Context.ConnectionId);

                //Update info on other clients
                await Clients.Group(lobbyIdentifier).updateLobbyPlayers(lobby.ConnectedPlayers);
            }
        }

        public async void KickPlayerFromLobby(string lobbyIdentifier, string playerName)
        {
            Lobby lobby = _gameDataService.GetLobby(lobbyIdentifier);
            if (Context.ConnectionId == lobby.HostConnectionID && !lobby.Game.IsRunning)
            {
                string clientConnectionID = lobby.GetPlayerByName(playerName).ConnectionID;

                //Remove the client from the group and the server
                await Clients.Client(clientConnectionID).disconnectFromGame("You have been kicked by the host!");
                await Groups.RemoveFromGroupAsync(clientConnectionID, lobbyIdentifier);

                //Remove the player
                _gameDataService.KickPlayerFromLobby(lobbyIdentifier, playerName);

                //Update data on other clients
                SendUpdateLobbyPlayers(lobbyIdentifier);
            }
        }
        #endregion

        #region Messaging
        public async Task<IActionResult> SendMessage(string lobbyIdentifier, string message)
        {
            Message msg = _gameDataService.AddMessageToLobby(lobbyIdentifier, Context.ConnectionId, message);
            await Clients.OthersInGroup(lobbyIdentifier).getNewMessage(msg);
            return new OkResult();
        }


        public async Task<List<Message>> GetMessages(string lobbyIdentifier)
        {
            return await Task.Run(() => { return _gameDataService.GetMessagesForLobby(lobbyIdentifier); });
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
            await Clients.Group(lobbyIdentifier).updateLobbyPlayers(_gameDataService.GetLobby(lobbyIdentifier).ConnectedPlayers);
        }
    }
}
