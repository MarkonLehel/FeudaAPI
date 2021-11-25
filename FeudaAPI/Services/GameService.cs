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
    public class GameService : BackgroundService
    {

        public static Dictionary<string, Lobby> lobbyDict = new();
        public static List<string> lobbyNamesInUse = new();
        private static IHubContext<GameHub, IGameHubClient> _gameHub { get; set; }

        public GameService(IHubContext<GameHub, IGameHubClient> hubcontext)
        {
            _gameHub = hubcontext;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Debug.WriteLine("Running at " + DateTime.UtcNow);

               


                //Game logic and process goes here
                //Along with data sending it seems

            }
        }
    }
}
