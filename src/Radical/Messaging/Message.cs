using System;
using Radical.ComponentModel.Messaging;
using Radical.Validation;

namespace Radical.Messaging
{
    /// <summary>
    /// A concrete message.
    /// </summary>
    [Obsolete( "The Radical message broker now supports POCO messages, will be removed in the next version.", false )]
    public abstract class Message : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        protected Message()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        [Obsolete( "The Radical message broker now supports POCO messages, use the default contructor, will be removed in the next version.", false )]
        protected Message( Object sender )
        {
            Ensure.That( sender ).Named( "sender" ).IsNotNull();

            this.Sender = sender;
        }

        /// <summary>
        /// Gets the message sender.
        /// </summary>
        /// <value>The message sender.</value>
        public Object Sender
        {
            get;
            private set;
        }

        void ILegacyMessageCompatibility.SetSenderForBackwardCompatibility( object sender )
        {
            Ensure.That( this.Sender )
                .WithMessage( "Invalid sender" )
                .If( s =>
                {
                    return s != null && s != sender;
                } )
                .ThenThrow( e =>
                {
                    return new ArgumentException( e.GetFullErrorMessage() );
                } );
            this.Sender = sender;
        }
    }
}