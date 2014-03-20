using System;
using System.Reflection;

namespace Topics.Radical.ComponentModel.Messaging
{
	/// <summary>
	/// The SubscribeToMessageAttribute is used to mark <c>IMessageHandler</c>s
	/// to allow automatic message subscription during the registration process
	/// of the <c>IMessageHandler</c> into an Invertion of Control container,
	/// specifically extended to handle ths scenario.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
    [System.Obsolete( "The Radical message broker now supports POCO messages.", false )]
    public sealed class SubscribeToMessageAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SubscribeToMessageAttribute"/> class.
		/// </summary>
		/// <param name="messageType">Type of the message.</param>
		public SubscribeToMessageAttribute( Type messageType )
		{
            //if( !typeof( IMessage ).GetTypeInfo().IsAssignableFrom( messageType.GetTypeInfo() ) )
            //{
            //    throw new ArgumentException( "missing::SubscribeToMessageAttributeInvalidMessageType" );
            //}

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