namespace Topics.Radical.ComponentModel.Messaging
{
	/// <summary>
    /// An <c>IHandleMessage</c> is a component spcialized
	/// in the operation of handling messages, primarly delivered
	/// by an <c>IMessageBroker</c>. It is designed to be registered
	/// in an Invertion of Control system in order to do automatic message
	/// handling.
	/// </summary>
    public interface IHandleMessage<T> : IHandleMessage
        //where T : IMessage
	{
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
		void Handle( object sender, T message );
	}
}