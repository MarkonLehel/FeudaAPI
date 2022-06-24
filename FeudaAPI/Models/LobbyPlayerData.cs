namespace FeudaAPI.Models
{
    public class LobbyPlayerData
    {

        public string playerName { get; set; }
        public bool isHost { get; set; }

        public LobbyPlayerData(string playerName, bool isHost)
        {
            this.playerName = playerName;
            this.isHost = isHost;
        }
    }
}
