using System.Collections.Generic;
using DSharpPlus.EventArgs;
using System.Threading.Tasks;
using DiscordBot.SteamGames;
using System.Linq;
using System.Text;

namespace DiscordBot.Commands
{
    public class ShowCommonSteamApps : BotCommand
    {
        public ShowCommonSteamApps() { }

        bool BotCommand.Is(MessageCreateEventArgs e)
        {
            return e.Message.Content.StartsWith("-sb show common steam apps");
        }

        async Task BotCommand.Run(MessageCreateEventArgs e)
        {
            var namedUserNames = e.Message.Content.Substring(26).Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

            var client = new SteamGamesClient();
            var matchingGames = new List<string>();
            foreach (string namedUserName in namedUserNames)
            {
                var games = await client.GetListOfSteamAppsIdsFor(namedUserName);
                if (!games.Any())
                {
                    await e.Message.RespondAsync($"Could not access data for {namedUserName}");
                    return;
                }

                if (matchingGames.Count == 0)
                    matchingGames.AddRange(games);
                else
                {
                    matchingGames = matchingGames.Intersect(games).ToList();
                }
            }

            var matchingGameNames = await client.GetSteamAppNames(matchingGames);

            var result = new StringBuilder();
            foreach (string matchingGameName in matchingGameNames)
                result.AppendLine(matchingGameName);

            await e.Message.RespondAsync(result.ToString());
        }
        
        IEnumerable<(string title, string content)> BotCommand.GetCommandInfo()
        {
            yield return ("Show all matching games for listed steam usernames", "-sb show common steam apps {username 1} {username 2} (can name any amount)");
        }
    }
}