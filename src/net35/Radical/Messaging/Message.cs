using System;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Validation;

namespace Topics.Radical.Messaging
{
	/// <summary>
	/// A concrete message.
	/// </summary>
    [Obsolete( "The Radical message broker now supports POCO messages.", false )]
	public abstract class Message : IMessage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Message"/> class.
		/// </summary>
		/// <param name="sender">The sender.</param>
		protected Message( Object sender )
		{
			Ensure.That( sender ).Named( "sender" ).IsNotNull();

			this.Sender = sender;
		}

		/// <summary>
		/// Gets the message sender.
		/// </summary>
		/// <value>The message sender.</value>
		public Object Sender
		{
			get;
			private set;
		}
	}
}