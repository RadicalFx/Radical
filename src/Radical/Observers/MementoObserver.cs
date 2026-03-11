using Radical.ComponentModel;
using Radical.ComponentModel.ChangeTracking;
using System;

namespace Radical.Observers
{
    /// <summary>
    /// Provides static factory methods for creating <see cref="MementoMonitor"/> instances.
    /// </summary>
    public static class MementoObserver
    {
        /// <summary>
        /// Creates a <see cref="MementoMonitor"/> that observes the specified <see cref="IChangeTrackingService"/>.
        /// </summary>
        /// <param name="source">The change tracking service to monitor.</param>
        /// <returns>A new <see cref="MementoMonitor"/> instance.</returns>
        public static MementoMonitor Monitor(IChangeTrackingService source)
        {
            return new MementoMonitor(source);
        }

        /// <summary>
        /// Creates a <see cref="MementoMonitor"/> that observes the specified <see cref="IChangeTrackingService"/> using the given dispatcher.
        /// </summary>
        /// <param name="source">The change tracking service to monitor.</param>
        /// <param name="dispatcher">The dispatcher used to marshal change notifications.</param>
        /// <returns>A new <see cref="MementoMonitor"/> instance.</returns>
        public static MementoMonitor Monitor(IChangeTrackingService source, IDispatcher dispatcher)
        {
            return new MementoMonitor(source, dispatcher);
        }
    }

    /// <summary>
    /// A monitor that observes an <see cref="IChangeTrackingService"/> and raises a notification when its tracking state changes.
    /// </summary>
    public class MementoMonitor : AbstractMonitor<IChangeTrackingService>
    {
        EventHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoMonitor"/> class with the specified source.
        /// </summary>
        /// <param name="source">The change tracking service to monitor.</param>
        public MementoMonitor(IChangeTrackingService source)
            : this(source, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoMonitor"/> class with the specified source and dispatcher.
        /// </summary>
        /// <param name="source">The change tracking service to monitor.</param>
        /// <param name="dispatcher">The dispatcher used to marshal change notifications.</param>
        public MementoMonitor(IChangeTrackingService source, IDispatcher dispatcher)
            : base(source, dispatcher)
        {
            handler = (s, e) => OnChanged();

            Source.TrackingServiceStateChanged += handler;
        }

        /// <summary>
        /// Called when monitoring is stopped; unsubscribes from the <see cref="IChangeTrackingService.TrackingServiceStateChanged"/> event.
        /// </summary>
        /// <param name="targetDisposed"><c>true</c> if the monitored target has been disposed; otherwise, <c>false</c>.</param>
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
