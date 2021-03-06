﻿using System.Collections.Generic;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    public class ShowCommands : BotCommand
    {
        private readonly IReadOnlyCollection<BotCommand> _commands;

        public ShowCommands(IReadOnlyCollection<BotCommand> commands)
        {
            _commands = commands;
        }

        bool BotCommand.Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith("-sb");
        }

        async Task BotCommand.Run(MessageCreateEventArgs e)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Commands",
                Color = DiscordColor.Azure
            };

            foreach (BotCommand command in _commands)
            {
                foreach((string title, string content) in command.GetCommandInfo())
                    embed.AddField(title, content);
                
            }

            embed.Build();

            await e.Message.RespondAsync(embed: embed);
        }
        
        IEnumerable<(string title, string content)> BotCommand.GetCommandInfo()
        {
            yield return ("Get this help window", "-sb");
        }
    }
}