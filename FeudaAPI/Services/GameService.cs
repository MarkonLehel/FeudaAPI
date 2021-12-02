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
