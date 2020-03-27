using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using DiscordBot.Libraries;
using DSharpPlus;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    public class ShowAllInsults : BotCommand
    {
        private readonly DiscordClient _discord;
        private readonly Library _insults;

        public ShowAllInsults(DiscordClient discord, Library library)
        {
            _discord = discord;
            _insults = library;
        }

        bool BotCommand.Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith("-sb show all insults");
        }

        async Task BotCommand.Run(MessageCreateEventArgs e)
        {
            DiscordChannel channelToUser = await _discord.CreateDmAsync(e.Author);
            await channelToUser.SendMessageAsync(_insults.GetSummary());
        }

        (string title, string content) BotCommand.GetCommandInfo()
        {
            return ("Show all current insults (will be a DM)", "-sb show all insults");
        }
    }
}