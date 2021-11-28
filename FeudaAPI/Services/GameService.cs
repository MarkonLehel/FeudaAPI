using FeudaAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FeudaAPI.Models;

namespace FeudaAPI.Services
{
    //TODO: This needs a singleton data storage class service to create a connection with the hub
    public class GameService : BackgroundService
    {

        public static Dictionary<string, Lobby> lobbyDict = new();
        public static List<string> lobbyNamesInUse = new();
        private static IHubContext<GameHub, IGameHubClient> _gameHub { get; set; }

        public GameService(IHubContext<GameHub, IGameHubClient> hubcontext)
        {
            _gameHub = hubcontext;
        }
        private DateTime lastWrite = DateTime.UtcNow;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan duration = lastWrite - DateTime.UtcNow;
                if (duration.TotalSeconds < -1)
                {
                    lastWrite = DateTime.UtcNow;
                    Debug.WriteLine("Running at " + DateTime.UtcNow + ", active lobbies:");

                    if (lobbyDict.Count() > 0)
                    {
                        foreach (KeyValuePair<string, Lobby> kvp in lobbyDict)
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
