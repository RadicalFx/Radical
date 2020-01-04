namespace Radical.ComponentModel.Messaging
{
    /// <summary>
    /// An <c>IHandleMessage</c> is a component specialized
    /// in the operation of handling messages, primarily delivered
    /// by an <c>IMessageBroker</c>. It is designed to be registered
    /// in an Inversion of Control system in order to do automatic message
    /// handling.
    /// </summary>
    [Contract]
    public interface IHandleMessage<T> : IHandleMessage
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        void Handle(object sender, T message);
    }
}