using DiscordBot.Libraries;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Commands.LibraryCommands
{
    public class RemoveCommand : BotCommand
    {
        private Library _library;
        private LibraryType _libraryType;

        public RemoveCommand(Library library, LibraryType libraryType)
        {
            _library = library;
            _libraryType = libraryType;
        }

        public IEnumerable<(string title, string content)> GetCommandInfo()
        {
            yield return ($"Remove {_libraryType} from the library", $"-sb remove {_libraryType} {{id}}");
        }

        public bool Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith($"-sb remove {_libraryType} ", StringComparison.OrdinalIgnoreCase);
        }

        public async Task Run(MessageCreateEventArgs e)
        {
            string keyToRemove = e.Message.Content.Substring(23);

            if (!int.TryParse(keyToRemove, out int entryKey))
                await e.Message.RespondAsync($"Please supply a valid {_libraryType} key");
            else
            {
                bool successful = await _library.Remove(entryKey);

                if (successful)
                    await e.Message.RespondAsync("Successfully removed");
                else
                    await e.Message.RespondAsync("Invalid key");
            }
        }
    }
}