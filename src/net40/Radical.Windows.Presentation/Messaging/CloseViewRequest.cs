using System;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618
    /// <summary>
	/// Issues a request to close the view currently associated with the view model that sends the message.
	/// </summary>
	public class CloseViewRequest : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CloseViewRequest"/> class.
		/// </summary>
		/// <param name="viewOwner">The view owner.</param>
		public CloseViewRequest( Object viewOwner )
		{
			this.ViewOwner = viewOwner;
#if !SILVERLIGHT
			this.DialogResult = null;
#endif
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CloseViewRequest"/> class.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="viewOwner">The view owner.</param>
		[Obsolete( "The Radical message broker now supports POCO messages, use the default contructor, will be removed in the next version.", false )]
		public CloseViewRequest( Object sender, Object viewOwner )
			: base( sender )
		{
			this.ViewOwner = viewOwner;
#if !SILVERLIGHT
            this.DialogResult = null;
#endif
        }

		/// <summary>
		/// Gets the view owner.
		/// </summary>
		public Object ViewOwner { get; private set; }

#if !SILVERLIGHT
        /// <summary>
        /// Gets or sets the dialog result.
        /// </summary>
        /// <value>
        /// The dialog result.
        /// </value>
        public bool? DialogResult { get; set; }

#endif
    }
#pragma warning restore 0618
}
