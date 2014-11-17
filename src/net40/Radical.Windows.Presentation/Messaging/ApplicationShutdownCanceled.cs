using System;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618
    /// <summary>
	/// Notifies that the application shutdown has been canceled.
	/// </summary>
	public class ApplicationShutdownCanceled : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationShutdownCanceled"/> class.
		/// </summary>
		/// <param name="reason">The reason.</param>
		public ApplicationShutdownCanceled( Boot.ApplicationShutdownReason reason )
		{
			this.Reason = reason;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationShutdownCanceled" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="reason">The reason.</param>
		[Obsolete( "The Radical message broker now supports POCO messages, use the default contructor, will be removed in the next version.", false )]
		public ApplicationShutdownCanceled( Object sender, Boot.ApplicationShutdownReason reason )
            : base( sender )
        {
            this.Reason = reason;
        }

        /// <summary>
        /// Gets the shutdown reason.
        /// </summary>
        /// <value>
        /// The shutdown reason.
        /// </value>
        public Boot.ApplicationShutdownReason Reason { get; private set; }
    }
#pragma warning restore 0618
}
