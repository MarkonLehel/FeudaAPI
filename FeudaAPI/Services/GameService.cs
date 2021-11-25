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

namespace FeudaAPI.Services
{
    public class GameService : BackgroundService
    {
        private IHubContext<GameHub> _gameHub { get; set; }

        public GameService(IHubContext<GameHub> hubcontext)
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
