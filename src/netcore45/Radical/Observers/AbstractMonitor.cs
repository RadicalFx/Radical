using System;
using Topics.Radical.ComponentModel;
using Topics.Radical.Validation;
using System.Reflection;
using System.ComponentModel;
using Topics.Radical.Reflection;
using System.Diagnostics;
using Windows.UI.Core;

namespace Topics.Radical.Observers
{
    /// <summary>
    /// A base abstract observer, based on the IMonitor interface
    /// </summary>
    public abstract class AbstractMonitor : IMonitor
    {
        /// <summary>
        /// Occurs when the source monitored by this monitor changes.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Called in order to raise the Changed event.
        /// </summary>
        protected virtual void OnChanged()
        {
            if ( this.Dispatcher != null && !this.Dispatcher.HasThreadAccess )
            {
                this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.OnChanged() );
            }
            else
            {
                var handler = this.Changed;
                if ( handler != null )
                {
                    handler( this, EventArgs.Empty );
                }
            }
        }

        /// <summary>
        /// Gets the dispatcher.
        /// </summary>
        /// <value>The dispatcher.</value>
        protected CoreDispatcher Dispatcher { get; private set; }

        /// <summary>
        /// Asks this monitor to raise a change notification in order
        /// to trigger all the listeners.
        /// </summary>
        public void NotifyChanged()
        {
            this.OnChanged();
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
        protected AbstractMonitor( CoreDispatcher dispatcher )
        {
            this.Dispatcher = dispatcher;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        protected AbstractMonitor( Object source )
        {
            Ensure.That( source ).Named( "source" ).IsNotNull();

            this.StartMonitoring( source );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        protected AbstractMonitor( Object source, CoreDispatcher dispatcher )
            : this( source )
        {
            this.Dispatcher = dispatcher;
        }

        EventHandler disposed = null;

        /// <summary>
        /// Starts monitoring the given source object.
        /// </summary>
        /// <param name="source">The source.</param>
        protected virtual void StartMonitoring( Object source )
        {
            this.WeakSource = new WeakReference( source );

            disposed = ( s, e ) =>
            {
                disposed = null;

                this.StopMonitoring( true );
            };
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
            this.StopMonitoring( false );
        }

        void StopMonitoring( Boolean targetDisposed )
        {
            this.OnStopMonitoring( targetDisposed );

            if ( !targetDisposed &&
                this.disposed != null &&
                this.WeakSource != null &&
                this.WeakSource.IsAlive )
            {

            }

            this.WeakSource = null;
        }

        /// <summary>
        /// Called in order to allow inheritors to stop the monitoring operations.
        /// </summary>
        /// <param name="targetDisposed"><c>True</c> if this call is subsequent to the Dispose of the monitored instance.</param>
        protected abstract void OnStopMonitoring( Boolean targetDisposed );
    }
}
