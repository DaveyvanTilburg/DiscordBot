using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Libraries;
using DSharpPlus;
using DiscordBot.Commands.LibraryCommands;
using System.Linq;

namespace DiscordBot.Commands
{
    public class LibraryCommandStructure : BotCommand
    {
        private List<BotCommand> _botCommands;

        public LibraryCommandStructure(Library library, LibraryType libraryType, DiscordClient discordClient, BotCommand runCommand)
        {
            _botCommands = new List<BotCommand>
            {
                new AddCommand(library, libraryType),
                new RemoveCommand(library, libraryType),
                runCommand,
                new ShowAllCommand(library, libraryType, discordClient)
            };
        }

        bool BotCommand.Is(MessageCreateEventArgs e)
        {
            return _botCommands.Any(c => c.Is(e));
        }

        async Task BotCommand.Run(MessageCreateEventArgs e)
        {
            BotCommand requestedCommand = _botCommands.FirstOrDefault(c => c.Is(e));

            await requestedCommand?.Run(e);
        }

        IEnumerable<(string title, string content)> BotCommand.GetCommandInfo()
        {
            return _botCommands.SelectMany(c => c.GetCommandInfo());
        }
    }
}