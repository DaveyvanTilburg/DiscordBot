using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Libraries;
using DSharpPlus;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    public class CommandList
    {
        private readonly List<BotCommand> _botCommands;

        private CommandList(params BotCommand[] botCommands)
        {
            _botCommands = new List<BotCommand>(botCommands);

            _botCommands.Add(new ShowCommonSteamApps());
            _botCommands.Add(new ShowCommands(_botCommands));
        }

        public static async Task<CommandList> Create(DiscordClient discordClient, Library steamAppNewsLibrary)
        {
            BotCommand appIdCommand = new LibraryCommandStructure(steamAppNewsLibrary, LibraryType.SteamAppNews, discordClient, await SteamAppNewsCommand.Create(steamAppNewsLibrary, LibraryType.SteamAppNews));

            var result = new CommandList(appIdCommand);
            return result;
        }

        public BotCommand GetBotCommand(Type type)
        {
            return _botCommands.FirstOrDefault(c => c.GetType() == type);
        }

        public async Task Run(MessageCreateEventArgs e)
        {
            BotCommand command = _botCommands.FirstOrDefault(c => c.Is(e));

            if (command != null)
                await command.Run(e);
        }
    }
}