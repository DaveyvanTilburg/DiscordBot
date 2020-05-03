using DiscordBot.Libraries;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Commands.LibraryCommands
{
    public abstract class RunCommand : BotCommand
    {
        protected Library _library;
        private LibraryType _libraryType;

        public RunCommand(Library library, LibraryType libraryType)
        {
            _library = library;
            _libraryType = libraryType;
        }

        public IEnumerable<(string title, string content)> GetCommandInfo()
        {
            yield return ($"Run the action for {_libraryType} now", $"-sb run {_libraryType}");
        }

        public bool Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith($"-sb run {_libraryType}", StringComparison.OrdinalIgnoreCase);
        }

        public abstract Task Run(MessageCreateEventArgs e);
    }
}