using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Timers;
using DiscordBot.Commands;

namespace DiscordBot
{
    class Program
    {
        private static DiscordClient _discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static readonly Timer _timer = new Timer();
        private static readonly Timer _insultTimer = new Timer();

        private static ulong PinnedMessageId = 692849775690645555;
        private static DiscordMessage _pinnedMessage;

        private static DiscordGuild _damageInc;
        private static DiscordChannel _jibberJabber;

        private static CommandList _commandList;
        private static RunInsultNow _insultCommand;

        private static async Task MainAsync(string[] args)
        {
            _timer.Interval = 30000;
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;

            _insultTimer.Interval = 21600000;
            _insultTimer.Elapsed += OnInsultTimedEvent;
            _insultTimer.AutoReset = true;

            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "NjkyODM0MzIwMTY3MDc1OTIw.Xn5fBA.iC19Wnz8Lk-DOcPN4JANG3XRW6k",
                TokenType = TokenType.Bot
            });

            _discord.MessageCreated += async e => { await _commandList.Run(e); };

            await _discord.ConnectAsync();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            _damageInc = await _discord.GetGuildAsync(315087365502926848);
            _jibberJabber = await _discord.GetChannelAsync(630368958498734099);

            //_damageInc = await _discord.GetGuildAsync(360705458966888448);
            //_jibberJabber = await _discord.GetChannelAsync(360705460053082124);

            _pinnedMessage = await _jibberJabber.GetMessageAsync(PinnedMessageId);

            _commandList = await CommandList.Create(_discord, _damageInc);
            _insultCommand = (RunInsultNow)_commandList.GetBotCommand(typeof(RunInsultNow));

            _timer.Enabled = true;
            _insultTimer.Enabled = true;

            while (true)
            {
                string command = Console.ReadLine() ?? string.Empty;

                if (command.Equals("exit"))
                    break;

                await _jibberJabber.SendMessageAsync(command);
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            string nlTime = $"NL: {DateTime.UtcNow.AddHours(1):HH:mm}";
            string ukTime = $"UTC: {DateTime.UtcNow:HH:mm}";
            string usTime = $"CST: {DateTime.UtcNow.AddHours(-6):HH:mm}";

            _pinnedMessage.ModifyAsync($"{nlTime}\r\n{ukTime}\r\n{usTime}").GetAwaiter().GetResult();
        }

        private static void OnInsultTimedEvent(object source, ElapsedEventArgs e)
        {
            _insultCommand.Insult(_damageInc, _jibberJabber).GetAwaiter().GetResult();
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            _discord.DisconnectAsync().GetAwaiter().GetResult();
        }
    }
}