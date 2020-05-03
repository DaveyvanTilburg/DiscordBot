using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using System.Timers;
using DiscordBot.Commands;
using NodaTime;

namespace DiscordBot
{
    class Program
    {
        private static DiscordClient _discord;
        private static ulong PinnedMessageId = 692849775690645555;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private const string Token = "TmpreU9ETTBNekl3TVRZM01EYzFPVEl3LlhwQURfQS5UM2lpcG5Pemk3RkJSNEkwZWJrbVdoVWdmYU0=";

        
        private static readonly Timer _insultTimer = new Timer();

        private static DiscordGuild _damageInc;
        private static DiscordChannel _jibberJabber;

        private static CommandList _commandList;

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Starting...");

            string decodedToken = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(Token));
            _discord = new DiscordClient(new DiscordConfiguration
            {
                Token = decodedToken,
                TokenType = TokenType.Bot
            });

            await _discord.ConnectAsync();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            //Actual
            Console.WriteLine("Getting Guild...");
            _damageInc = await _discord.GetGuildAsync(315087365502926848);

            Console.WriteLine("Getting Channel...");
            _jibberJabber = await _discord.GetChannelAsync(630368958498734099);

            Console.WriteLine("Getting pinned message...");
            var pinnedMessage = await _jibberJabber.GetMessageAsync(PinnedMessageId);

            var timedLocalTimeUpdate = new TimedLocalTimeUpdate(30000, pinnedMessage);
            Console.WriteLine("Starting local time updater!");
            timedLocalTimeUpdate.Start();

            //Mine
            //_damageInc = await _discord.GetGuildAsync(360705458966888448);
            //_jibberJabber = await _discord.GetChannelAsync(360705460053082124);

            _commandList = await CommandList.Create(_discord);

            Console.WriteLine("Listening to new messages!");
            _discord.MessageCreated += async e => { await _commandList.Run(e); };

            _insultTimer.Enabled = true;

            Console.WriteLine("Setup complete!");
            //Make it run unblocked indefinitely
            await Task.Delay(-1);
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            _discord.DisconnectAsync().GetAwaiter().GetResult();
        }
    }
}