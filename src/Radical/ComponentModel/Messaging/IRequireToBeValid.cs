namespace Radical.ComponentModel.Messaging
{
    /// <summary>
    /// Identifies a message that has its own validation
    /// logic that must be called before Dispatch/Broadcast.
    /// </summary>
    public interface IRequireToBeValid //: IMessage
    {
        /// <summary>
        /// Validates the current message content.
        /// </summary>
        void Validate();
    }
}