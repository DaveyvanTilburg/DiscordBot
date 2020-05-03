using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Libraries;
using DiscordBot.SteamNews;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    public class AppIdLibraryCommand : LibraryCommandBase
    {
        private SteamNewsClient _steamNewsClient;

        private  AppIdLibraryCommand(Library library, LibraryType libraryType, DiscordClient discordClient) : base(library, libraryType, discordClient)
        {
        }

        public static async Task<AppIdLibraryCommand> Create(DiscordClient discordClient)
        {
            var insults = await Library.Create(LibraryType.SteamAppNews);

            var result = new AppIdLibraryCommand(insults, LibraryType.SteamAppNews, discordClient);
            result._steamNewsClient = await SteamNewsClient.Create();
            return result;
        }

        protected override async Task Run(MessageCreateEventArgs e)
        {
            IEnumerable<string> urls = await _steamNewsClient.GetNews(_library.GetValues());

            foreach(string url in urls)
                await e.Message.RespondAsync(url);
        }
    }
}