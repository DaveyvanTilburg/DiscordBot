using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using DiscordBot.Libraries;

namespace DiscordBot.Commands
{
    public class AddInsult : BotCommand
    {
        private readonly Library _insults;

        public AddInsult(Library library)
        {
            _insults = library;
        }

        bool BotCommand.Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith("-sb add insult ");
        }

        async Task BotCommand.Run(MessageCreateEventArgs e)
        {
            string insultToAdd = e.Message.Content.Substring(23);
            int insultKey = await _insults.Add(insultToAdd);

            await e.Message.RespondAsync($"Insult added to library id:{insultKey}");
        }

        (string title, string content) BotCommand.GetCommandInfo()
        {
            return ("Add an insult to the insult library", "-sb add insult {message that includes @user}");
        }
    }
}