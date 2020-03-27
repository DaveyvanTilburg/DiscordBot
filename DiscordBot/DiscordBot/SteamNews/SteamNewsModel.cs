using System.Collections.Generic;

namespace DiscordBot.SteamNews
{
    public class SteamNewsModel
    {
        public AppNews AppNews { get; set; }
    }

    public class AppNews
    {
        public int Count { get; set; }
        public int AppId { get; set; }

        public List<NewsItem> NewsItems { get; set; }
    }

    public class NewsItem
    {
        public long Date { get; set; }
        public string Url { get; set; }
    }
}