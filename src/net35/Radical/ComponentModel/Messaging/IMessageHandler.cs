using System;
namespace Topics.Radical.ComponentModel.Messaging
{
	/// <summary>
	/// An <c>IMessageHandler</c> is a component spcialized
	/// in the operation of handling messages, primarly delivered
	/// by an <c>IMessageBroker</c>. It is designed to be registered
	/// in an Invertion of Control system in order to do automatic message
	/// handling.
	/// </summary>
    [Obsolete( "The Radical message broker now supports POCO messages.", false )]
	public interface IMessageHandler
	{
		/// <summary>
		/// Handles the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		void Handle( IMessage message );

		/// <summary>
		/// Determines if this message handler is interested in handling the given message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns><c>True</c> if this message handler is interested in handling the given message; otherwise <c>false</c>.</returns>
		bool ShouldHandle( IMessage message );
	}
}