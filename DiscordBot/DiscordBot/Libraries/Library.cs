using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot.Libraries
{
    public class Library
    {
        private Library(LibraryType libraryType)
        {
            _path = $"./Libraries/{libraryType}.json";
        }

        private readonly string _path;
        private readonly ConcurrentDictionary<int, string> _items = new ConcurrentDictionary<int, string>();
        private readonly Random _random = new Random(456);

        public static async Task<Library> Create(LibraryType libraryType)
        {
            var result = new Library(libraryType);
            await result.Load();

            return result;
        }

        public async Task<int> Add(string sentence)
        {
            int newKey = _items.Keys.Max() + 1;
            _items.TryAdd(newKey, sentence);

            await Save();
            return newKey;
        }

        public async Task<bool> Remove(int id)
        {
            if (!_items.ContainsKey(id))
                return false;

            _items.TryRemove(id, out string value);

            await Save();
            return true;
        }

        public string GetSummary()
        {
            var result = new StringBuilder();

            foreach (KeyValuePair<int, string> keyValuePair in _items)
                result.AppendLine($"Id: {keyValuePair.Key}; Value: {keyValuePair.Value}");

            return result.ToString();
        }

        public string GetRandomEntry()
        {
            if (_items.TryGetValue(_random.Next(0, _items.Count), out string value))
                return value;

            return "Something went wrong";
        }

        private async Task Save()
        {
            string serializedContent = JsonConvert.SerializeObject(_items);

            await File.WriteAllTextAsync(_path, serializedContent);
        }

        private async Task Load()
        {
            string fileContents = await File.ReadAllTextAsync(_path);

            var items = JsonConvert.DeserializeObject<Dictionary<int, string>>(fileContents);
            foreach (var item in items)
                _items.TryAdd(item.Key, item.Value);
        }

        public enum LibraryType
        {
            Insults = 0
        }
    }
}