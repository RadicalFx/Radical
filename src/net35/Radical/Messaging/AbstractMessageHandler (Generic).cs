using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Validation;
using Topics.Radical.Reflection;

namespace Topics.Radical.Messaging
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
		public abstract void Handle( object sender, T message );

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
		public virtual void Handle(object sender, object message )
		{
			Ensure.That( message )
				.Named( "message" )
				.IsNotNull()
				.IsTrue( msg => msg.GetType().Is<T>() );

			this.Handle( sender, ( T )message );
		}

        /// <summary>
        /// Determines if this message handler is interested in handling the given message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>True</c> if this message handler is interested in handling the given message; otherwise <c>false</c>.
        /// </returns>
		public bool ShouldHandle( object sender, object message )
		{
			Ensure.That( message )
				.Named( "message" )
				.IsNotNull()
				.IsTrue( msg => msg.GetType().Is<T>() );

			return this.OnShouldHandle( sender, ( T )message );
		}

        /// <summary>
        /// Determines if this message handler is interested in handling the given message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>True</c> if this message handler is interested in handling the given message; otherwise <c>false</c>.
        /// </returns>
		protected virtual bool OnShouldHandle( object sender, T message )
		{
			return true;
		}
	}
}