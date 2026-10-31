using Radical.ComponentModel.Messaging;
using Radical.Reflection;
using Radical.Validation;
using System;
using System.Threading.Tasks;

namespace Radical.Messaging
{
    /// <summary>
    /// A base implementation of the IHandleMessage generic interface.
    /// </summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    public abstract class AbstractMessageHandler<T> : IHandleMessage<T>
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        /// <returns>A task that completes when the message has been handled.</returns>
        public abstract Task HandleAsync(object sender, T message);

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        /// <returns>A task that completes when the message has been handled.</returns>
        public virtual Task HandleAsync(object sender, object message)
        {
            Ensure.That(message)
                .Named("message")
                .IsNotNull()
                .IsTrue(msg => msg.GetType().Is<T>());

            return HandleAsync(sender, (T)message);
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        [Obsolete(ObsoleteMessages.Handle, error: true)]
        public void Handle(object sender, T message)
        {
            throw new NotSupportedException(ObsoleteMessages.Handle);
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        [Obsolete(ObsoleteMessages.Handle, error: true)]
        public void Handle(object sender, object message)
        {
            throw new NotSupportedException(ObsoleteMessages.Handle);
        }

        /// <summary>
        /// Determines if this message handler is interested in handling the given message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>True</c> if this message handler is interested in handling the given message; otherwise <c>false</c>.
        /// </returns>
        public bool ShouldHandle(object sender, object message)
        {
            Ensure.That(message)
                .Named("message")
                .IsNotNull()
                .IsTrue(msg => msg.GetType().Is<T>());

            return OnShouldHandle(sender, (T)message);
        }

        /// <summary>
        /// Determines if this message handler is interested in handling the given message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>True</c> if this message handler is interested in handling the given message; otherwise <c>false</c>.
        /// </returns>
        protected virtual bool OnShouldHandle(object sender, T message)
        {
            return true;
        }
    }
}