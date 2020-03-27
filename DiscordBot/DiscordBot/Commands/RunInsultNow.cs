using System;
using System.Linq;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using DiscordBot.Libraries;
using DSharpPlus.Entities;

namespace DiscordBot.Commands
{
    public class RunInsultNow : BotCommand
    {
        private readonly Library _insults;
        private readonly DiscordGuild _targetGuild;
        private readonly Random _random = new Random(123);

        public RunInsultNow(Library library, DiscordGuild targetGuild)
        {
            _insults = library;
            _targetGuild = targetGuild;
        }

        bool BotCommand.Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith("-sb test insult");
        }

        async Task BotCommand.Run(MessageCreateEventArgs e)
        {
            await Insult(_targetGuild, e.Message.Channel);
        }

        public async Task Insult(DiscordGuild targetGuild, DiscordChannel targetChannel)
        {
            var members = targetGuild.GetAllMembersAsync().GetAwaiter().GetResult().ToList();
            var onlineMembers = members.Where(m => m.Presence.Status == UserStatus.Online && !m.IsBot).ToList();

            var member = onlineMembers[_random.Next(0, onlineMembers.Count)];
            string insult = _insults.GetRandomEntry();

            await targetChannel.SendMessageAsync(string.Format(insult, $"<@{member.Id}>"));
        }

        (string title, string content) BotCommand.GetCommandInfo()
        {
            return ("Make me insult someone now", "-sb test insult");
        }
    }
}