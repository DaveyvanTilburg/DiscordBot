using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot
{
    public class SteamNewsClient
    {
        public async Task<string> GetNews()
        {
            var request = WebRequest.CreateHttp("http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid=548430&count=1&maxlength=300&format=json");
            request.AllowAutoRedirect = true;
            WebResponse webResponse = await request.GetResponseAsync();

            string response;
            await using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                response = reader.ReadToEnd();
            }

            SteamNewsModel steamNews = JsonConvert.DeserializeObject<SteamNewsModel>(response);

            return steamNews.AppNews.NewsItems[0].Url;
        }
    }
}