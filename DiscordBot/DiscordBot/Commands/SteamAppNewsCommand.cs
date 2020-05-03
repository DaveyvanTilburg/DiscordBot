using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Commands.LibraryCommands;
using DiscordBot.Libraries;
using DiscordBot.SteamNews;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    public class SteamAppNewsCommand : RunCommand
    {
        private SteamNewsClient _steamNewsClient;

        private  SteamAppNewsCommand(Library library, LibraryType libraryType) : base(library, libraryType)
        {
        }

        public static async Task<SteamAppNewsCommand> Create(Library library, LibraryType libraryType)
        {
            var result = new SteamAppNewsCommand(library, libraryType);
            result._steamNewsClient = await SteamNewsClient.Create();

            return result;
        }

        public override async Task Run(MessageCreateEventArgs e)
        {
            Console.WriteLine($"{DateTime.UtcNow:s} | SteamAppNews: Getting updates...");
            IEnumerable<string> urls = await _steamNewsClient.GetNews(_library.GetValues(), true);

            foreach(string url in urls)
                await e.Message.RespondAsync(url);

            Console.WriteLine($"{DateTime.UtcNow:s} | SteamAppNews: Finished!");
        }
    }
}