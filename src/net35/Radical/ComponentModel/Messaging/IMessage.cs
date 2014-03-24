using System;
namespace Topics.Radical.ComponentModel.Messaging
{
    /// <summary>
    /// An IMessage is the minum contract that a message needs to implement
    /// in order to be managed by an <c>IMessageBroker</c>.
    /// </summary>
    [Obsolete( "The Radical message broker now supports POCO messages.", false )]
    public interface IMessage
    {
        /// <summary>
        /// Gets the message sender.
        /// </summary>
        /// <value>The message sender.</value>
        object Sender { get; }
    }
}