using System.Collections.Generic;

namespace DiscordBot.SteamGames
{
    public class SteamAppsResponse
    {
        public SteamAppsMessage Response { get; set; }
    }

    public class SteamAppsMessage
    {
        public int Game_Count { get; set; }
        public List<SteamApp> Games { get; set; }
    }

    public class SteamApp
    {
        public string AppId { get; set; }
    }
}