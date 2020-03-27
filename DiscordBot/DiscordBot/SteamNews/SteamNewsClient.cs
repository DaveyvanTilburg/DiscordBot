using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot.SteamNews
{
    public class SteamNewsClient
    {
        private SteamNewsDates _steamNewsDates;

        private SteamNewsClient()
        {
        }

        public static async Task<SteamNewsClient> Create()
        {
            var result = new SteamNewsClient
            {
                _steamNewsDates = await SteamNewsDates.Create()
            };

            return result;
        }

        public async Task<IEnumerable<string>> GetNews(IEnumerable<string> appIds)
        {
            var result = new List<string>();

            foreach (string appId in appIds)
            {
                var request = WebRequest.CreateHttp($"http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid={appId}&count=1&maxlength=300&format=json");
                request.AllowAutoRedirect = true;
                WebResponse webResponse = await request.GetResponseAsync();

                string response;
                await using (Stream stream = webResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    response = reader.ReadToEnd();
                }

                SteamNewsModel steamNews = JsonConvert.DeserializeObject<SteamNewsModel>(response);
                NewsItem newsItem = steamNews?.AppNews?.NewsItems?.FirstOrDefault();

                if (newsItem == null)
                    continue;

                if (!_steamNewsDates.Check(appId, newsItem.Date)) 
                    continue;

                result.Add(steamNews.AppNews.NewsItems[0].Url);
                await _steamNewsDates.AddOrUpdate(appId, newsItem.Date);
            }

            return result;
        }
    }
}