using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot.SteamGames
{
    public class SteamGamesClient
    {
        private SteamAppInfoResponse _steamAppInfoResponse;

        private const string Token = "MkI1NTAxN0ZBMzMyQTczOEEyMjQ2MzkxODNGNTQ4QzA=";
        private const string UrlSteamIdFormat = "http://api.steampowered.com/ISteamUser/ResolveVanityURL/v0001/?key={0}&vanityurl={1}";
        private const string UrlSteamAppsFormat = "http://api.steampowered.com/IPlayerService/GetOwnedGames/v0001/?key={0}&steamid={1}&format=json";
        private const string UrlSteamAppInfoFormat = "http://api.steampowered.com/ISteamApps/GetAppList/v0002/?key={0}&format=json";

        public async Task<IEnumerable<string>> GetListOfSteamAppsIdsFor(string userName)
        {
            string decodedToken = Encoding.ASCII.GetString(Convert.FromBase64String(Token));

            string steamId;

            if (userName.All(char.IsNumber))
                steamId = userName;
            else
            {
                string getSteamIdUrl = string.Format(UrlSteamIdFormat, decodedToken, userName);
                string serializedSteamIdResponse = await Get(getSteamIdUrl);
                var steamIdResponseObject = JsonConvert.DeserializeObject<SteamIdResponse>(serializedSteamIdResponse);

                steamId = steamIdResponseObject.Response.SteamId;
            }

            string getSteamAppsUrl = string.Format(UrlSteamAppsFormat, decodedToken, steamId);
            string serializedSteamAppsResponse = await Get(getSteamAppsUrl);
            var steamAppsResponse = JsonConvert.DeserializeObject<SteamAppsResponse>(serializedSteamAppsResponse);

            if (steamAppsResponse.Response.Game_Count == 0)
                return new string[0];

            var result = 
                from games in steamAppsResponse.Response.Games
                select games.AppId;

            return result;
        }

        public async Task<IEnumerable<string>> GetSteamAppNames(IEnumerable<string> appIds)
        {
            if (_steamAppInfoResponse == null)
                _steamAppInfoResponse = await GetSteamAppInfo();

            var result =
                from appInfo in _steamAppInfoResponse.AppList.Apps
                where appIds.Contains(appInfo.AppId)
                select appInfo.Name;

            return result;
        }

        private async Task<SteamAppInfoResponse> GetSteamAppInfo()
        {
            string decodedToken = Encoding.ASCII.GetString(Convert.FromBase64String(Token));

            string getSteamAppInfoUrl = string.Format(UrlSteamAppInfoFormat, decodedToken);
            string serializedSteamAppInfoResponse = await Get(getSteamAppInfoUrl);
            var steamAppInfoResponse = JsonConvert.DeserializeObject<SteamAppInfoResponse>(serializedSteamAppInfoResponse);

            return steamAppInfoResponse;
        }

        private async Task<string> Get(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.AllowAutoRedirect = true;
            WebResponse webResponse = await request.GetResponseAsync();

            string response;
            await using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                response = reader.ReadToEnd();
            }

            return response;
        }
    }
}