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

        private const string Token = "TmpreU9ETTBNekl3TVRZM01EYzFPVEl3LlhwQURfQS5UM2lpcG5Pemk3RkJSNEkwZWJrbVdoVWdmYU0=";

        private static readonly Timer _timer = new Timer();
        private static readonly Timer _insultTimer = new Timer();

        private static ulong PinnedMessageId = 692849775690645555;
        private static DiscordMessage _pinnedMessage;

        private static DiscordGuild _damageInc;
        private static DiscordChannel _jibberJabber;

        private static CommandList _commandList;
        private static InsultLibraryCommand _insultCommand;

        private static async Task MainAsync(string[] args)
        {
            _timer.Interval = 30000;
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;

            _insultTimer.Interval = 21600000;
            _insultTimer.Elapsed += OnInsultTimedEvent;
            _insultTimer.AutoReset = true;

            string decodedToken = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(Token));
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = decodedToken,
                TokenType = TokenType.Bot
            });

            _discord.MessageCreated += async e => { await _commandList.Run(e); };

            await _discord.ConnectAsync();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
             
            //Actual
            _damageInc = await _discord.GetGuildAsync(315087365502926848);
            _jibberJabber = await _discord.GetChannelAsync(630368958498734099);

            //Mine
            //_damageInc = await _discord.GetGuildAsync(360705458966888448);
            //_jibberJabber = await _discord.GetChannelAsync(360705460053082124);

            _pinnedMessage = await _jibberJabber.GetMessageAsync(PinnedMessageId);

            _commandList = await CommandList.Create(_discord);
            _insultCommand = (InsultLibraryCommand)_commandList.GetBotCommand(typeof(InsultLibraryCommand));

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
            TimeZoneInfo cstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"); 
            DateTime cst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstTimeZone);

            TimeZoneInfo amsterdamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W.Europe Standard Time"); 
            DateTime amsterdamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, amsterdamTimeZone);

            string nlTime = $"NL: {amsterdamTime:HH:mm}";
            string ukTime = $"UTC: {DateTime.UtcNow:HH:mm}";
            string usTime = $"CST: {cst:HH:mm}";

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