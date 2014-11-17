using System;
using Topics.Radical.Messaging;

namespace Topics.Radical.Windows.Presentation.Messaging
{
#pragma warning disable 0618
    /// <summary>
	/// Issue a request to shutdown the current application.
	/// </summary>
	public class ApplicationShutdownRequest : Message
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationShutdownRequest"/> class.
		/// </summary>
		public ApplicationShutdownRequest()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationShutdownRequest"/> class.
		/// </summary>
		/// <param name="sender">The sender.</param>
		[Obsolete( "The Radical message broker now supports POCO messages, use the default contructor, will be removed in the next version.", false )]
		public ApplicationShutdownRequest( Object sender )
			: base( sender )
		{

		}
    }
#pragma warning restore 0618
}
