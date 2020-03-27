using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;
using DiscordBot.Libraries;

namespace DiscordBot.Commands
{
    public class RemoveInsult : BotCommand
    {
        private readonly Library _insults;

        public RemoveInsult(Library library)
        {
            _insults = library;
        }

        bool BotCommand.Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith("-sb remove insult ");
        }

        async Task BotCommand.Run(MessageCreateEventArgs e)
        {
            string insultKeyToRemove = e.Message.Content.Substring(23);

            if (!int.TryParse(insultKeyToRemove, out int insultKey))
                await e.Message.RespondAsync("Please supply a insult key");
            else
            {
                bool successful = await _insults.Remove(insultKey);

                if (successful)
                    await e.Message.RespondAsync("Successfully removed");
                else
                    await e.Message.RespondAsync("Invalid key");
            }
        }

        (string title, string content) BotCommand.GetCommandInfo()
        {
            return ("Remove insult from the library", "-sb remove insult {id}");
        }
    }
}