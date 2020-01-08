using Radical.ComponentModel;
using Radical.ComponentModel.ChangeTracking;
using System;

namespace Radical.Observers
{
    public static class MementoObserver
    {
        public static MementoMonitor Monitor(IChangeTrackingService source)
        {
            return new MementoMonitor(source);
        }

        public static MementoMonitor Monitor(IChangeTrackingService source, IDispatcher dispatcher)
        {
            return new MementoMonitor(source, dispatcher);
        }
    }

    public class MementoMonitor : AbstractMonitor<IChangeTrackingService>
    {
        EventHandler handler = null;

        public MementoMonitor(IChangeTrackingService source)
            : this(source, null)
        {

        }

        public MementoMonitor(IChangeTrackingService source, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            handler = (s, e) => OnChanged();

            Source.TrackingServiceStateChanged += handler;
        }

        protected override void OnStopMonitoring(bool targetDisposed)
        {
            if (!targetDisposed && WeakSource != null && WeakSource.IsAlive)
            {
                Source.TrackingServiceStateChanged -= handler;
            }

            handler = null;
        }
    }
}
