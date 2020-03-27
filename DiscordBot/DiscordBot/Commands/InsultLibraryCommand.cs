using System;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Libraries;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    public class InsultLibraryCommand : LibraryCommandBase
    {
        private readonly Random _random = new Random();

        private InsultLibraryCommand(
            Library library, 
            LibraryType libraryType, 
            DiscordClient discordClient
            ) : base(library, libraryType, discordClient)
        {
        }

        public static async Task<InsultLibraryCommand> Create(DiscordClient discordClient)
        {
            var insults = await Library.Create(LibraryType.Insult);

            var result = new InsultLibraryCommand(insults, LibraryType.Insult, discordClient);
            return result;
        }

        protected override async Task Run(MessageCreateEventArgs e)
        {
            await Insult(e.Guild, e.Message.Channel);
        }

        public async Task Insult(DiscordGuild targetGuild, DiscordChannel targetChannel)
        {
            var members = targetGuild.GetAllMembersAsync().GetAwaiter().GetResult().ToList();
            var onlineMembers = members.Where(m => (m.Presence?.Status ?? UserStatus.Offline) == UserStatus.Online && !m.IsBot).ToList();

            var member = onlineMembers[_random.Next(0, onlineMembers.Count)];
            string insult = _library.GetRandomEntry();

            await targetChannel.SendMessageAsync(string.Format(insult, $"<@{member.Id}>"));
        }
    }
}