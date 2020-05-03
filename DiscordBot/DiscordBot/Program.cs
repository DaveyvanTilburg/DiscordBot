using System;
using System.Threading.Tasks;
using DSharpPlus;
using DiscordBot.Commands;
using DiscordBot.Libraries;

namespace DiscordBot
{
    class Program
    {
        private static DiscordClient _discord;

        private const string Token = "TmpreU9ETTBNekl3TVRZM01EYzFPVEl3LlhwQURfQS5UM2lpcG5Pemk3RkJSNEkwZWJrbVdoVWdmYU0=";

        private static CommandList _commandList;

        static void Main(string[] args)
        {
            MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
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

            Console.WriteLine("Getting Channel...");
            var _jibberJabber = await _discord.GetChannelAsync(630368958498734099);

            Console.WriteLine("Getting pinned message...");
            var pinnedMessage = await _jibberJabber.GetMessageAsync(692849775690645555);

            var timedLocalTimeUpdate = new TimedLocalTimeUpdate(30000, pinnedMessage);
            Console.WriteLine("Starting local time updater!");
            timedLocalTimeUpdate.Start();

            var steamAppNewsLibrary = await Library.Create(LibraryType.SteamAppNews);
            var timedSteamAppNewsUpdate = await TimedSteamAppNewsUpdate.Create(60000, steamAppNewsLibrary); //3600000 = 1 hour
            Console.WriteLine("Starting steam app news updater!");
            timedSteamAppNewsUpdate.Start();

            _commandList = await CommandList.Create(_discord, steamAppNewsLibrary);

            Console.WriteLine("Listening to new messages!");
            _discord.MessageCreated += async e => { await _commandList.Run(e); };

            Console.WriteLine("Setup complete!");
            await Task.Delay(-1);
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            _discord.DisconnectAsync().GetAwaiter().GetResult();
        }
    }
}