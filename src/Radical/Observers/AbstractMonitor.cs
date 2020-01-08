using Radical.ComponentModel;
using Radical.Diagnostics;
using Radical.Reflection;
using Radical.Validation;
using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Radical.Observers
{
    /// <summary>
    /// A base abstract observer, based on the IMonitor interface
    /// </summary>
    public abstract class AbstractMonitor : IMonitor
    {
        static readonly TraceSource logger = new TraceSource(typeof(IMonitor).FullName);

        /// <summary>
        /// Occurs when the source monitored by this monitor changes.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Called in order to raise the Changed event.
        /// </summary>
        protected virtual void OnChanged()
        {
            if (Dispatcher != null && !Dispatcher.IsSafe)
            {
                Dispatcher.Dispatch(() => OnChanged());
            }
            else
            {
                if (WeakSource == null)
                {
                    logger.Warning
                    (
                        "Raising the Changed event even if the monitored source is null. ({0})",
                        GetType().ToString("SN")
                    );
                }

                if (WeakSource != null && !WeakSource.IsAlive)
                {
                    logger.Warning
                    (
                        "Raising the Changed event even if the monitored source is not alive anymore. ({0})",
                        GetType().ToString("SN")
                    );
                }

                var handler = Changed;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the dispatcher.
        /// </summary>
        /// <value>The dispatcher.</value>
        protected IDispatcher Dispatcher { get; private set; }

        /// <summary>
        /// Asks this monitor to raise a change notification in order
        /// to trigger all the listeners.
        /// </summary>
        public void NotifyChanged()
        {
            OnChanged();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor"/> class.
        /// </summary>
        protected AbstractMonitor()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        protected AbstractMonitor(IDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        protected AbstractMonitor(object source)
        {
            Ensure.That(source).Named("source").IsNotNull();

            StartMonitoring(source);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        protected AbstractMonitor(object source, IDispatcher dispatcher)
            : this(source)
        {
            Dispatcher = dispatcher;
        }

        EventHandler disposed = null;

        /// <summary>
        /// Starts monitoring the given source object.
        /// </summary>
        /// <param name="source">The source.</param>
        protected virtual void StartMonitoring(object source)
        {
            WeakSource = new WeakReference(source);

            disposed = (s, e) =>
            {
                ((IComponent)s).Disposed -= disposed;
                disposed = null;

                StopMonitoring(true);
            };

            var ic = source as IComponent;
            if (ic != null)
            {
                ic.Disposed += disposed;
            }
        }

        /// <summary>
        /// Gets the weak source.
        /// </summary>
        /// <value>The weak source.</value>
        protected WeakReference WeakSource
        {
            get;
            private set;
        }

        /// <summary>
        /// Stops all the monitoring operation.
        /// </summary>
        public void StopMonitoring()
        {
            StopMonitoring(false);
        }

        void StopMonitoring(bool targetDisposed)
        {
            OnStopMonitoring(targetDisposed);

            if (!targetDisposed &&
                disposed != null &&
                WeakSource != null &&
                WeakSource.IsAlive)
            {
                var ic = WeakSource.Target as IComponent;
                if (ic != null)
                {
                    ic.Disposed -= disposed;
                }
            }

            WeakSource = null;
        }

        /// <summary>
        /// Called in order to allow inheritors to stop the monitoring operations.
        /// </summary>
        /// <param name="targetDisposed"><c>True</c> if this call is subsequent to the Dispose of the monitored instance.</param>
        protected abstract void OnStopMonitoring(bool targetDisposed);
    }
}
