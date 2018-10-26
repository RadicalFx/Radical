using Radical.ComponentModel.Messaging;
using Radical.Validation;
using Radical.Reflection;
using System;

namespace Radical.Messaging
{
    /// <summary>
    /// A base implementation of the IMessageHandler generic interface.
    /// </summary>
    /// <typeparam name="T">The type of the message.</typeparam>
    [Obsolete( "The Radical message broker now supports POCO messages, inherits from AbstractMessageHandler<T> or implement IHandleMessage<T> interface directly.", false )]
    public abstract class MessageHandler<T> : IMessageHandler<T> where T : class, IMessage
    {
        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void Handle( T message );

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public virtual void Handle( IMessage message )
        {
            Ensure.That( message )
                .Named( "message" )
                .IsNotNull()
                .IsTrue( msg => msg.GetType().Is<T>() );

            this.Handle( ( T )message );
        }

        /// <summary>
        /// Determines if this message handler is interested in handling the given message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>True</c> if this message handler is interested in handling the given message; otherwise <c>false</c>.
        /// </returns>
        public bool ShouldHandle( IMessage message )
        {
            Ensure.That( message )
                .Named( "message" )
                .IsNotNull()
                .IsTrue( msg => msg.GetType().Is<T>() );

            return this.OnShouldHandle( ( T )message );
        }

        /// <summary>
        /// Determines if this message handler is interested in handling the given message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>True</c> if this message handler is interested in handling the given message; otherwise <c>false</c>.
        /// </returns>
        protected virtual bool OnShouldHandle( T message )
        {
            return true;
        }
    }
}