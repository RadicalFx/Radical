using System;
namespace Topics.Radical.ComponentModel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILegacyMessageCompatibility
    {
        /// <summary>
        /// Sets the sender for backward compatibility.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void SetSenderForBackwardCompatibility(object sender);
    }

    /// <summary>
    /// An IMessage is the minum contract that a message needs to implement
    /// in order to be managed by an <c>IMessageBroker</c>.
    /// </summary>
    [Obsolete( "The Radical message broker now supports POCO messages, will be removed in the next version.", false )]
    public interface IMessage : ILegacyMessageCompatibility
    {
        /// <summary>
        /// Gets the message sender.
        /// </summary>
        /// <value>The message sender.</value>
        object Sender { get; }
    }
}