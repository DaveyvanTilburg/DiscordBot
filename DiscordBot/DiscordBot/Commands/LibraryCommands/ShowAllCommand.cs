using DiscordBot.Libraries;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Commands.LibraryCommands
{
    public class ShowAllCommand : BotCommand
    {
        private readonly DiscordClient _discordClient;
        private Library _library;
        private LibraryType _libraryType;

        public ShowAllCommand(Library library, LibraryType libraryType, DiscordClient discordClient)
        {
            _library = library;
            _libraryType = libraryType;
            _discordClient = discordClient;
        }

        public IEnumerable<(string title, string content)> GetCommandInfo()
        {
            yield return ($"Show all current {_libraryType} (will be a DM)", $"-sb show all {_libraryType}");
        }

        public bool Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith($"-sb show all {_libraryType}", StringComparison.OrdinalIgnoreCase);
        }

        public async Task Run(MessageCreateEventArgs e)
        {
            //DiscordChannel channelToUser = await _discordClient.CreateDmAsync(e.Author);
            //await channelToUser.SendMessageAsync(_library.GetSummary());

            await e.Message.RespondAsync(_library.GetSummary());
        }
    }
}