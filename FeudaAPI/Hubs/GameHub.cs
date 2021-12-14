using FeudaAPI.Models;
using FeudaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace FeudaAPI.Hubs
{
    //TODO: This needs a singleton data storage class service to create a connection with the bgservice
    public class GameHub : Hub<IGameHubClient>
    {
        private GameDataService _gameDataService;
        private readonly ILogger<GameDataService> _logger;
        public GameHub(GameDataService gameDataService , ILogger<GameDataService> logger)
        {
            _logger = logger;
            _gameDataService = gameDataService;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"User connected to server with connection ID {Context.ConnectionId}");
            List<Lobby> lobbyList = _gameDataService.GetLobbiesWhereGameNotStarted();
            await Clients.Client(Context.ConnectionId).getActiveLobbyList(lobbyList.ConvertAll(new Converter<Lobby, LobbyListing>((lb) => new LobbyListing(lb))));
        }
        public async Task<List<LobbyListing>> GetActiveLobbyListing()
        {
            await Task.Yield();
            return _gameDataService.GetLobbiesWhereGameNotStarted().ConvertAll(new Converter<Lobby, LobbyListing>((lb) => new LobbyListing(lb)));
        }

        #region Lobby
        //TODO: Use the identifier received from the URL instead of passing it as first parameter to most functions
        //TODO: In case of a lobby browser, implement OnConnectedAsync and OnDisconnectedAsync to send data to a lobby browser
        public async Task<IActionResult> CreateLobby(string lobbyName, string hostName)
        {
            _logger.LogInformation($"Received a request to start a lobby from user {hostName}({Context.ConnectionId}) with name {lobbyName}");
            try {
                string lobbyIdentifier = _gameDataService.AddLobby(lobbyName, Context.ConnectionId, hostName);
                _logger.LogInformation($"A lobby was successfully created with the identifier of {lobbyIdentifier} and name {lobbyName}");
                await Groups.AddToGroupAsync(Context.ConnectionId, lobbyIdentifier);
                _logger.LogInformation($"{hostName}({Context.ConnectionId}) was added to group {lobbyIdentifier}");
                return new JsonResult(lobbyIdentifier);
            } catch { 
            return new ConflictResult();
            }
        }

        public async Task<IActionResult> CloseLobby(string lobbyIdentifier)
        {
            Lobby lobby = _gameDataService.GetLobby(lobbyIdentifier);

            if (lobby.IsHost(Context.ConnectionId) && !lobby.Game.IsRunning) { 
                _gameDataService.RemoveLobby(lobbyIdentifier, Context.ConnectionId);

                foreach (Player player in lobby.ConnectedPlayers)
                {
                    await Groups.RemoveFromGroupAsync(player.ConnectionID, lobbyIdentifier);
                }
                _logger.LogInformation($"Lobby {lobbyIdentifier} was closed by the host {Context.ConnectionId}");
                return new OkResult();
            } else {
                return new UnauthorizedResult();
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
                    await SendUpdateToLobbyPlayers(lobbyIdentifier);
                    await Groups.AddToGroupAsync(Context.ConnectionId, lobbyIdentifier);
                    _gameDataService.AddPlayerToLobby(lobbyIdentifier, Context.ConnectionId, playerName);
                    _logger.LogInformation($"Player {playerName}({Context.ConnectionId} has joined lobby {lobbyIdentifier})");
                    //Send update to clients
                    

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
                _logger.LogInformation($"Player {lobby.GetPlayerByConnectionID(Context.ConnectionId).PlayerName}({Context.ConnectionId} has left lobby {lobbyIdentifier})");
                //Update info on other clients
                await Clients.Group(lobbyIdentifier).updateLobbyPlayers(lobby.ConnectedPlayers);
            }
        }

        public async Task<IActionResult> KickPlayerFromLobby(string lobbyIdentifier, string playerName)
        {
            Lobby lobby = _gameDataService.GetLobby(lobbyIdentifier);
            if (lobby.IsHost(Context.ConnectionId) && !lobby.Game.IsRunning)
            {
                string clientConnectionID = lobby.GetPlayerByName(playerName).ConnectionID;

                //Remove the client from the group and the server
                await Clients.Client(clientConnectionID).disconnectFromGame("You have been kicked by the host!");
                await Groups.RemoveFromGroupAsync(clientConnectionID, lobbyIdentifier);

                //Remove the player
                _logger.LogInformation($"The host just kicked {playerName}{lobby.GetPlayerByName(playerName).ConnectionID} from lobby {lobbyIdentifier}");
                _gameDataService.KickPlayerFromLobby(lobbyIdentifier, playerName);

                //Update data on other clients
                await SendUpdateToLobbyPlayers(lobbyIdentifier);
                return new OkResult();
            }
            return new UnauthorizedResult();
        }

        //Add method for getting active lobby list

        #endregion

        #region Messaging
        public async Task<IActionResult> SendMessage(string lobbyIdentifier, string message)
        {
            Message msg = _gameDataService.AddMessageToLobby(lobbyIdentifier, Context.ConnectionId, message);
            await Clients.OthersInGroup(lobbyIdentifier).getNewMessage(msg);
            _logger.LogInformation($"Lobby {lobbyIdentifier} just got a new message");
            return new OkResult();
        }


        public async Task<List<Message>> GetMessages(string lobbyIdentifier)
        {
            _logger.LogInformation($"A client just asked for the messages for lobby {lobbyIdentifier}");
            return await Task.Run(() => { return _gameDataService.GetMessagesForLobby(lobbyIdentifier); });
        }

        #endregion

        #region Game
        public async Task InitGame(string lobbyIdentifier)
        {
            Lobby lobby = _gameDataService.GetLobby(lobbyIdentifier);
            if (lobby != null && lobby.IsHost(Context.ConnectionId) && !lobby.Game.IsRunning)
            {
                _logger.LogInformation($"Game initialized for lobby {lobbyIdentifier}");
                await Clients.Group(lobbyIdentifier).initGame();
                _gameDataService.StartGame(lobbyIdentifier);
                
            }

        }

        public async Task<IActionResult> MoveSerf(string lobbyIdentifier, int fromX, int fromY, int toX, int toY)
        {
            await Task.Yield();
            _logger.LogInformation($"A serf was moved in lobby {lobbyIdentifier}");
            bool success = _gameDataService.MoveSerf(lobbyIdentifier, Context.ConnectionId, new Coordinate(fromX, fromY), new Coordinate(toX, toY));
            return success ? new OkResult() : new ConflictResult();
        }

        public async Task<IActionResult> BuildBuilding(string lobbyIdentifier, string building, int cordX, int cordY)
        {
            await Task.Yield();
            _logger.LogInformation($"A {building}");
            bool success = _gameDataService.BuildBuilding(lobbyIdentifier, Context.ConnectionId, building, new Coordinate(cordX, cordY));
            _logger.LogInformation($"A player tried to build a {building} in lobby {lobbyIdentifier}, he was {(success ? "succesfull" : "not succesfull")}");
            return success ? new OkResult() : new ConflictResult();
        }

        #endregion

        private async Task SendUpdateToLobbyPlayers(string lobbyIdentifier)
        {

            await Clients.Group(lobbyIdentifier).updateLobbyPlayers(_gameDataService.GetLobby(lobbyIdentifier).ConnectedPlayers);
        }
    }
}
