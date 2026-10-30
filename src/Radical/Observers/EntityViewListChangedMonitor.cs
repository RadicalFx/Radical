using Radical.ComponentModel;
using System.ComponentModel;

namespace Radical.Observers
{
    /// <summary>
    /// A monitor that observes <see cref="IEntityView"/> list-changed events and raises a notification when the list changes.
    /// </summary>
    public class EntityViewListChangedMonitor : AbstractMonitor<IEntityView>
    {
        ListChangedEventHandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewListChangedMonitor"/> class with the specified source.
        /// </summary>
        /// <param name="source">The <see cref="IEntityView"/> to monitor.</param>
        public EntityViewListChangedMonitor(IEntityView source)
            : base(source)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewListChangedMonitor"/> class with no source.
        /// </summary>
        public EntityViewListChangedMonitor()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewListChangedMonitor"/> class with the specified source and dispatcher.
        /// </summary>
        /// <param name="source">The <see cref="IEntityView"/> to monitor.</param>
        /// <param name="dispatcher">The dispatcher used to marshal change notifications.</param>
        public EntityViewListChangedMonitor(IEntityView source, IDispatcher dispatcher)
            : base(source, dispatcher)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewListChangedMonitor"/> class with the specified dispatcher.
        /// </summary>
        /// <param name="dispatcher">The dispatcher used to marshal change notifications.</param>
        public EntityViewListChangedMonitor(IDispatcher dispatcher)
            : base(dispatcher)
        {

        }

        /// <summary>
        /// Starts monitoring the specified source by subscribing to its ListChanged event.
        /// </summary>
        /// <param name="source">The source object to monitor.</param>
        protected override void StartMonitoring(object source)
        {
            base.StartMonitoring(source);

            handler = (s, e) => OnChanged();
            Source.ListChanged += handler;
        }

        /// <summary>
        /// Stops monitoring the current source and begins monitoring the specified <see cref="IEntityView"/>.
        /// </summary>
        /// <param name="source">The new <see cref="IEntityView"/> to observe.</param>
        public void Observe(IEntityView source)
        {
            StopMonitoring();
            StartMonitoring(source);
        }

        /// <summary>
        /// Called when monitoring is stopped; unsubscribes from the ListChanged event.
        /// </summary>
        /// <param name="targetDisposed"><c>true</c> if the monitored target has been disposed; otherwise, <c>false</c>.</param>
        protected override void OnStopMonitoring(bool targetDisposed)
        {
            if (!targetDisposed && WeakSource != null && WeakSource.IsAlive)
            {
                Source.ListChanged -= handler;
            }

            handler = null;
        }
    }
}
