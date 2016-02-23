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
    [Contract]
    [Obsolete( "The Radical message broker now supports POCO messages.", false )]
    public interface IMessageHandler<T> : IMessageHandler
        where T : IMessage
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Handle( T message );
    }
}