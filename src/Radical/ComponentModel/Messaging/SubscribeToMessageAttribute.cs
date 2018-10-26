using System;

namespace Radical.ComponentModel.Messaging
{
    /// <summary>
    /// The SubscribeToMessageAttribute is used to mark <c>IMessageHandler</c>s
    /// to allow automatic message subscription during the registration process
    /// of the <c>IMessageHandler</c> into an Invertion of Control container,
    /// specifically extended to handle ths scenario.
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
    [Obsolete( "Not required anymore, will be removed in the version.", false )]
    public sealed class SubscribeToMessageAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeToMessageAttribute"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        public SubscribeToMessageAttribute( Type messageType )
        {
            if ( !typeof( IMessage ).IsAssignableFrom( messageType ) )
            {
                throw new ArgumentException( Resources.Exceptions.SubscribeToMessageAttributeInvalidMessageType );
            }

            this.MessageType = messageType;
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        public Type MessageType
        {
            get;
            private set;
        }
    }
}