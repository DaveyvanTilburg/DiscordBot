using DiscordBot.Libraries;
using DiscordBot.SteamNews;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace DiscordBot
{
    public class TimedSteamAppNewsUpdate : TimedEvent
    {
        private SteamNewsClient _steamNewsClient;
        private Library _steamAppsNewsLibrary;

        private TimedSteamAppNewsUpdate(int interval, Library steamAppsNewsLibrary) : base(interval)
        {
            _steamAppsNewsLibrary = steamAppsNewsLibrary;
        }

        public static async Task<TimedEvent> Create(int interval, Library steamAppsNewsLibrary)
        {
            var result = new TimedSteamAppNewsUpdate(interval, steamAppsNewsLibrary);
            result._steamNewsClient = await SteamNewsClient.Create();

            return result;
        }

        protected override ElapsedEventHandler GetElapsedEventHandler()
        {
            return async(object sender, ElapsedEventArgs e) => 
            {
                Console.WriteLine($"{DateTime.Now:s}| Fetching steam news");
                await _steamNewsClient.GetNews(_steamAppsNewsLibrary.GetValues(), false);
            };
        }
    }
}