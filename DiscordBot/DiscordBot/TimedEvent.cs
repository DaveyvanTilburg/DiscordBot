using System.Timers;

namespace DiscordBot
{
    public abstract class TimedEvent
    {
        private readonly Timer _timer = new Timer();

        public TimedEvent(int interval)
        {
            _timer.Interval = interval;
            _timer.AutoReset = true;

            _timer.Elapsed += GetElapsedEventHandler();
        }

        protected abstract ElapsedEventHandler GetElapsedEventHandler();

        public void Start()
        {
            _timer.Enabled = true;
        }
    }
}