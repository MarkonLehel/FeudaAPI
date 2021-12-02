using FeudaAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using FeudaAPI.Models;

namespace FeudaAPI.Services
{
    //TODO: This needs a singleton data storage class service to create a connection with the hub
    public class GameService : BackgroundService
    {

        private IHubContext<GameHub, IGameHubClient> _gameHub;
        private GameDataService _gameDataService;

        public GameService(IHubContext<GameHub, IGameHubClient> hubcontext, GameDataService gameDataService)
        {
            _gameDataService = gameDataService;
            _gameHub = hubcontext;
        }
        private DateTime lastWrite = DateTime.UtcNow;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Currently this is the line that makes this an async function, Task.Yield literally does only this.
            await Task.Yield();
            while (!stoppingToken.IsCancellationRequested)
            {

                foreach (Lobby lobby in _gameDataService.activeGames)
                {
                    Game game = lobby.Game;
                    if (game.IsRunning && (game.lastUpdateInterval == null ||
                        ((DateTime)game.lastUpdateInterval - DateTime.UtcNow).TotalSeconds < -2) )
                    {
                        if(game.TurnCount >= 1000)
                        {
                            await _gameHub.Clients.Group(lobby.LobbyIdentifier).endGame();
                            _gameDataService.RemoveLobby(lobby.LobbyIdentifier, lobby.HostConnectionID);
                        }

                        game.lastUpdateInterval = DateTime.UtcNow;
                        Dictionary<string, TurnDataObject> gameTurnData = game.CalculateTurn(lobby.ConnectedPlayers);
                        foreach (KeyValuePair<string,TurnDataObject> keyValuePair in gameTurnData)
                        {
                            await _gameHub.Clients.Client(keyValuePair.Key).getTurnGameData(keyValuePair.Value);
                        }
                    }
                }


                TimeSpan duration = lastWrite - DateTime.UtcNow;
                if (duration.TotalSeconds < -1)
                {
                    lastWrite = DateTime.UtcNow;
                    Debug.WriteLine("Running at " + DateTime.UtcNow + ", active lobbies:");

                    if (_gameDataService.lobbyDict.Count() > 0)
                    {
                        foreach (KeyValuePair<string, Lobby> kvp in _gameDataService.lobbyDict)
                        {
                            Debug.WriteLine("Name: " + kvp.Value.GameName + " Key: " + kvp.Key);
                        }
                    }
                    else
                    {
                        Debug.WriteLine("No active lobbies.");
                    }
                }
            }
        }
    }
}
