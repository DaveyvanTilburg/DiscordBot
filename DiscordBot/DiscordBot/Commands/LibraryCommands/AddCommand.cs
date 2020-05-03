using DiscordBot.Libraries;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Commands.LibraryCommands
{
    public class AddCommand : BotCommand
    {
        private Library _library;
        private LibraryType _libraryType;

        public AddCommand(Library library, LibraryType libraryType)
        {
            _library = library;
            _libraryType = libraryType;
        }

        public IEnumerable<(string title, string content)> GetCommandInfo()
        {
            yield return ($"Add an {_libraryType} to the library", $"-sb add {_libraryType}");
        }

        public bool Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith($"-sb add {_libraryType} ", StringComparison.OrdinalIgnoreCase);
        }

        public async Task Run(MessageCreateEventArgs e)
        {
            string valueToAdd = e.Message.Content.Substring(9 + _libraryType.ToString().Length);
            int newKey = await _library.Add(valueToAdd);

            await e.Message.RespondAsync($"{_libraryType} added to library id:{newKey}");
        }
    }
}