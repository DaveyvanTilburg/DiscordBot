using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Libraries;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    public class CommandList
    {
        private readonly List<BotCommand> _botCommands;

        private CommandList(DiscordClient discord, Library insults, DiscordGuild targetGuild)
        {
            _botCommands = new List<BotCommand>
            {
                new AddInsult(insults),
                new RemoveInsult(insults),
                new RunInsultNow(insults, targetGuild),
                new ShowAllInsults(discord, insults)
            };

            _botCommands.Add(new ShowCommands(_botCommands));
        }

        public static async Task<CommandList> Create(DiscordClient discord, DiscordGuild targetGuild)
        {
            var insults = await Library.Create(Library.LibraryType.Insults);

            var result = new CommandList(discord, insults, targetGuild);
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