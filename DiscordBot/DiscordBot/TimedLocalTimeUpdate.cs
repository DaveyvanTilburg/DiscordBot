using DSharpPlus.Entities;
using NodaTime;
using System.Timers;

namespace DiscordBot
{
    public class TimedLocalTimeUpdate : TimedEvent
    {
        private DiscordMessage _discordMessage;

        public TimedLocalTimeUpdate(int interval, DiscordMessage discordMessage) : base(interval)
        {
            _discordMessage = discordMessage;
        }

        protected override ElapsedEventHandler GetElapsedEventHandler()
        {
            return new ElapsedEventHandler((object sender, ElapsedEventArgs e) =>
            {
                var currentTime = SystemClock.Instance.GetCurrentInstant();
                var amsterdamTimezone = DateTimeZoneProviders.Tzdb["Europe/Amsterdam"];
                var chicago = DateTimeZoneProviders.Tzdb["America/Chicago"];

                string nlTime = $"Amsterdam: {currentTime.InZone(amsterdamTimezone): HH:mm}";
                string ukTime = $"London: {currentTime.InUtc():HH:mm}";
                string usTime = $"Chicago: {currentTime.InZone(chicago): HH:mm}";

                string message = $"{nlTime}\r\n{ukTime}\r\n{usTime}";
                _discordMessage.ModifyAsync(message).GetAwaiter().GetResult();
            });
        }
    }
}