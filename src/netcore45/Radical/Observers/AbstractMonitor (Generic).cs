using Topics.Radical.ComponentModel;
using Windows.UI.Core;

namespace Topics.Radical.Observers
{
    /// <summary>
    /// A base abstract observer, based on the IMonitor interface
    /// </summary>
    /// <typeparam name="T">The monitored type.</typeparam>
    public abstract class AbstractMonitor<T> : AbstractMonitor, IMonitor<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        protected AbstractMonitor( T source )
            : base( source )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="dispatcher">The dispatcher.</param>
        protected AbstractMonitor( T source, CoreDispatcher dispatcher )
            : base( source, dispatcher )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor&lt;T&gt;"/> class.
        /// </summary>
        protected AbstractMonitor()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMonitor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        protected AbstractMonitor( CoreDispatcher dispatcher )
            : base( dispatcher )
        {

        }

        /// <summary>
        /// Gets the monitored source.
        /// </summary>
        /// <value>The monitored source.</value>
        public T Source
        {
            get { return ( T )this.WeakSource.Target; }
        }
    }
}
