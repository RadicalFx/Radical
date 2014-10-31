using System;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618
    /// <summary>
	/// Notifies that a request to shutdown request has been issued.
	/// </summary>
	public class ApplicationShutdownRequested : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationShutdownRequested"/> class.
		/// </summary>
		/// <param name="reason">The reason.</param>
		public ApplicationShutdownRequested( Boot.ApplicationShutdownReason reason )
		{
			this.Reason = reason;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationShutdownRequested" /> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="reason">The reason.</param>
		[Obsolete( "The Radical message broker now supports POCO messages, use the default contructor, will be removed in the next version.", false )]
		public ApplicationShutdownRequested( Object sender, Boot.ApplicationShutdownReason reason )
			: base( sender )
		{
            this.Reason = reason;
		}

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApplicationShutdownRequested"/> is cancelled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cancel; otherwise, <c>false</c>.
        /// </value>
        public Boolean Cancel { get; set; }

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
