using System;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Libraries;
using DSharpPlus;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    public abstract class LibraryCommandBase : BotCommand
    {
        private readonly DiscordClient _discord;
        protected readonly Library _library;
        private readonly LibraryType _libraryType;

        protected LibraryCommandBase(Library library, LibraryType libraryType, DiscordClient discordClient)
        {
            _library = library;
            _libraryType = libraryType;
            _discord = discordClient;
        }

        bool BotCommand.Is(MessageCreateEventArgs e)
        {
            return 
                e.Message.Content.StartsWith($"-sb add {_libraryType} ", StringComparison.OrdinalIgnoreCase) ||
                e.Message.Content.StartsWith($"-sb remove {_libraryType} ", StringComparison.OrdinalIgnoreCase) ||
                e.Message.Content.StartsWith($"-sb {_libraryType}", StringComparison.OrdinalIgnoreCase) ||
                e.Message.Content.StartsWith($"-sb show all {_libraryType}s", StringComparison.OrdinalIgnoreCase);
        }

        async Task BotCommand.Run(MessageCreateEventArgs e)
        {
            if (e.Message.Content.StartsWith($"-sb add {_libraryType} ", StringComparison.OrdinalIgnoreCase))
            {
                string valueToAdd = e.Message.Content.Substring(9 + _libraryType.ToString().Length);
                int newKey = await _library.Add(valueToAdd);

                await e.Message.RespondAsync($"{_libraryType} added to library id:{newKey}");
            }
            else if (e.Message.Content.StartsWith($"-sb remove {_libraryType} ", StringComparison.OrdinalIgnoreCase))
            {
                string insultKeyToRemove = e.Message.Content.Substring(23);

                if (!int.TryParse(insultKeyToRemove, out int insultKey))
                    await e.Message.RespondAsync($"Please supply a {_libraryType} key");
                else
                {
                    bool successful = await _library.Remove(insultKey);

                    if (successful)
                        await e.Message.RespondAsync("Successfully removed");
                    else
                        await e.Message.RespondAsync("Invalid key");
                }
            }
            else if (e.Message.Content.StartsWith($"-sb {_libraryType}", StringComparison.OrdinalIgnoreCase))
            {
                await Run(e);
            }
            else if(e.Message.Content.StartsWith($"-sb show all {_libraryType}s", StringComparison.OrdinalIgnoreCase))
            {
                DiscordChannel channelToUser = await _discord.CreateDmAsync(e.Author);
                await channelToUser.SendMessageAsync(_library.GetSummary());
            }
        }

        protected abstract Task Run(MessageCreateEventArgs e);

        IEnumerable<(string title, string content)> BotCommand.GetCommandInfo()
        {
            yield return ($"Add an {_libraryType} to the library", $"-sb add {_libraryType} {{message that includes @user}}");
            yield return ($"Remove {_libraryType} from the library", $"-sb remove {_libraryType} {{id}}");
            yield return ($"Run the action for {_libraryType} now", $"-sb {_libraryType}");
            yield return ($"Show all current {_libraryType} (will be a DM)", $"-sb show all {_libraryType}s");
        }
    }
}