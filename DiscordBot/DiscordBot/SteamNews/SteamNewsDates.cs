using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot.SteamNews
{
    public class SteamNewsDates
    {
        private SteamNewsDates()
        {
            _path = "./SteamNewsDates.json";
        }

        private readonly string _path;
        private readonly ConcurrentDictionary<string, long> _steamNewsDates = new ConcurrentDictionary<string, long>();

        public static async Task<SteamNewsDates> Create()
        {
            var result = new SteamNewsDates();
            await result.Load();

            return result;
        }

        public bool Check(string appId, long date)
        {
            if (_steamNewsDates.TryGetValue(appId, out long storedDate))
            {
                return date > storedDate;
            }

            return true;
        }

        public async Task AddOrUpdate(string appId, long date)
        {
            _steamNewsDates.AddOrUpdate(appId, date, (key, oldValue) => date);

            await Save();
        }

        private async Task Save()
        {
            string serializedContent = JsonConvert.SerializeObject(_steamNewsDates);

            await File.WriteAllTextAsync(_path, serializedContent);
        }

        private async Task Load()
        {
            if (!File.Exists(_path))
                return;

            string fileContents = await File.ReadAllTextAsync(_path);

            var items = JsonConvert.DeserializeObject<Dictionary<string, long>>(fileContents);
            foreach ((string key, long value) in items)
                _steamNewsDates.TryAdd(key, value);
        }
    }
}