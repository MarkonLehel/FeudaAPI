using FeudaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeudaAPI.Hubs
{
    public interface IGameHubClient
    {
        Task DisconnectFromGame(string message);
        Task UpdateLobbyPlayers(List<string> playerNameList);
        Task GetNewMessage(Message msg);
        Task GetTurnGameData(TurnDataObject turnData);
        Task GetActiveLobbyList(List<LobbyListing> lobbyNames);

        Task InitGame();
        Task PlayerDefeated();
        Task EndGame();
    }
    
}
