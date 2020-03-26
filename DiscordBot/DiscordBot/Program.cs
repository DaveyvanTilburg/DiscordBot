using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Timers;

namespace DiscordBot
{
    class Program
    {
        private static DiscordClient _discord;
        private static DiscordChannel _jibberJabber;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static string GreetingMessage = "Hello";
        private static List<string> RandomInsults = new List<string>();
        private static Random Random = new Random();

        private static Timer aTimer = new Timer();

        private static ulong PinnedMessageId = 692849775690645555;
        private static DiscordMessage PinnedMessage;

        private static async Task MainAsync(string[] args)
        {
            aTimer.Interval = 30000;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;

            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NjkyODM0MzIwMTY3MDc1OTIw.Xn0SXw.tYulKVBgRQneCvsDaUua977Y71M",
                TokenType = TokenType.Bot
            });

            _discord.MessageCreated += async e =>
            {
                if (e.Message.Content.StartsWith("-sneakysbot say hi to ", StringComparison.OrdinalIgnoreCase))
                {
                    IEnumerable<DiscordUser> users = e.Message.MentionedUsers;

                    foreach (DiscordUser user in users)
                        await e.Message.RespondAsync($"{GreetingMessage} <@{user.Id}>");
                }

                if (e.Message.Content.StartsWith("-sneakysbot set greeting as "))
                {
                    GreetingMessage = e.Message.Content.Substring(28);

                    await e.Message.RespondAsync("Greeting message changed");
                }

                if (e.Message.Content.StartsWith("-sneakysbot add insult "))
                {
                    RandomInsults.Add(e.Message.Content.Substring(23));

                    await e.Message.RespondAsync("Insult added to library");
                }

                if (e.Message.Content.StartsWith("-sneakysbot randomly insult"))
                {
                    IEnumerable<DiscordUser> users = e.Message.MentionedUsers;

                    foreach (DiscordUser user in users)
                        await e.Message.RespondAsync($"<@{user.Id}> {RandomInsults[Random.Next(0, RandomInsults.Count)]}");
                }
            };

            await _discord.ConnectAsync();

            _jibberJabber = await _discord.GetChannelAsync(630368958498734099);
            PinnedMessage = await _jibberJabber.GetMessageAsync(PinnedMessageId);

            aTimer.Enabled = true;

            while (true)
            {
                string command = Console.ReadLine() ?? string.Empty;

                if (command.Equals("exit"))
                {
                    await _discord.DisconnectAsync();
                    break;
                }

                //var embed = new DiscordEmbedBuilder
                //{
                //    Title = "Test title",
                //    Description = command
                //};

                //embed.AddField("Field name", "field value")
                //    .WithFooter("Field footer")
                //    .Build();

                //await _jibberJabber.SendMessageAsync(embed: embed);

                await _jibberJabber.SendMessageAsync(command);
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            string nlTime = $"NL: {DateTime.UtcNow.AddHours(1):HH:mm}";
            string ukTime = $"UK: {DateTime.UtcNow:HH:mm}";
            string usTime = $"US: {DateTime.UtcNow.AddHours(-8):HH:mm}";

            PinnedMessage.ModifyAsync($"{nlTime}\r\n{ukTime}\r\n{usTime}");
        }
    }
}