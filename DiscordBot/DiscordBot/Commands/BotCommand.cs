using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace DiscordBot.Commands
{
    public interface BotCommand
    {
        bool Is(MessageCreateEventArgs e);
        Task Run(MessageCreateEventArgs e);
        (string title, string content) GetCommandInfo();
    }
}