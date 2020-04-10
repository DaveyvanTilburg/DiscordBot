using System.Collections.Generic;

namespace DiscordBot.SteamGames
{
    public class SteamAppInfoResponse
    {
        public SteamAppMessage AppList { get; set; }
    }

    public class SteamAppMessage
    {
        public List<SteamAppInfo> Apps { get; set; }
    }

    public class SteamAppInfo
    {
        public string Name { get; set; }
        public string AppId { get; set; }
    }
}