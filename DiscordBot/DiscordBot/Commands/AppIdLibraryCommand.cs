using System.Threading.Tasks;
using DiscordBot.Libraries;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    public class AppIdLibraryCommand : LibraryCommandBase
    {
        private  AppIdLibraryCommand(Library library, LibraryType libraryType, DiscordClient discordClient) : base(library, libraryType, discordClient)
        {
        }

        public static async Task<AppIdLibraryCommand> Create(DiscordClient discordClient)
        {
            var insults = await Library.Create(LibraryType.AppId);

            var result = new AppIdLibraryCommand(insults, LibraryType.AppId, discordClient);
            return result;
        }

        protected override async Task Run(MessageCreateEventArgs e)
        {
            string appId = e.Message.Content.Substring(9);

            var client = new SteamNewsClient();
            string url = await client.GetNews(appId);

            await e.Message.RespondAsync(url);
        }
    }
}