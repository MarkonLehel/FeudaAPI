using FeudaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Hubs
{
    public interface IGameHubClient
    {
        Task disconnectFromGame(string message);
        Task updateLobbyPlayers(List<Player> playerList);
        Task getNewMessage(Message msg);
        Task getTurnGameData(TurnDataObject turnData);

        Task initGame();

        Task endGame();
    }
    
}
