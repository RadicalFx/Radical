using Radical.Observers;
using System.Timers;

namespace Radical.Tests.Threading
{
    public class TimerTrigger : AbstractMonitor<Timer>
    {
        static Timer CreateDefault(int interval, TimerTriggerMode triggerMode)
        {
            var t = new Timer(interval);
            t.Enabled = false;
            //t.AutoReset = triggerMode == TimerTriggerMode.Always;

            return t;
        }

        readonly ElapsedEventHandler elapsedEventHandler = null;

        public TimerTrigger(int interval, TimerTriggerMode triggerMode)
            : base(CreateDefault(interval, triggerMode))
        {
            Mode = triggerMode;

            elapsedEventHandler = (s, e) =>
            {
                Stop();
                NotifyChanged();
                if (Mode == TimerTriggerMode.Always)
                {
                    Start();
                }
            };
        }

        protected override void StartMonitoring(object source)
        {
            base.StartMonitoring(source);

            var t = (Timer)source;
            t.Elapsed += elapsedEventHandler;
        }

        protected override void OnStopMonitoring(bool targetDisposed)
        {
            if (!targetDisposed)
            {
                Stop();

                if (WeakSource.IsAlive)
                {
                    Source.Elapsed -= elapsedEventHandler;
                }
            }
        }

        public void Start()
        {
            if (WeakSource.IsAlive)
            {
                Source.Enabled = true;
            }
        }

        public void Stop()
        {
            if (WeakSource.IsAlive)
            {
                Source.Enabled = false;
            }
        }

        public TimerTriggerMode Mode { get; private set; }
    }

    public enum TimerTriggerMode
    {
        Once,
        Always
    }
}