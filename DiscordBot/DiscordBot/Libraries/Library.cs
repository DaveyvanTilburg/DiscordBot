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
            _libraryType = libraryType;
            _path = $"./Libraries//{libraryType}.json";
        }

        private LibraryType _libraryType;
        private readonly string _path;
        private readonly ConcurrentDictionary<int, string> _items = new ConcurrentDictionary<int, string>();
        private readonly Random _random = new Random();

        public static async Task<Library> Create(LibraryType libraryType)
        {
            var result = new Library(libraryType);
            await result.Load();

            return result;
        }

        public async Task<int> Add(string sentence)
        {
            int newKey = _items.Keys.Count != 0 ? _items.Keys.Max() + 1 : 1;
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
            if (_items.Count == 0)
                return $"Library for {_libraryType} is empty";

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

        public IEnumerable<string> GetValues()
        {
            return _items.Values;
        }

        private async Task Save()
        {
            string serializedContent = JsonConvert.SerializeObject(_items);

            FileInfo _fileInfo = new FileInfo(_path);
            _fileInfo.Directory.Create();

            await File.WriteAllTextAsync(_path, serializedContent);
        }

        private async Task Load()
        {
            if (!File.Exists(_path))
                return;

            string fileContents = await File.ReadAllTextAsync(_path);

            var items = JsonConvert.DeserializeObject<Dictionary<int, string>>(fileContents);
            foreach ((int key, string value) in items)
                _items.TryAdd(key, value);
        }
    }
}