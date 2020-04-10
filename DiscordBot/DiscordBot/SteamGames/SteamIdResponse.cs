namespace DiscordBot.SteamGames
{
    public class SteamIdResponse
    {
        public SteamIdMessage Response { get; set; }
    }

    public class SteamIdMessage
    {
        public bool Success { get; set; }
        public string SteamId { get; set; }
    }
}