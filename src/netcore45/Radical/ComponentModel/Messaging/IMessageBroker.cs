using System;
using System.Threading.Tasks;

namespace Topics.Radical.ComponentModel.Messaging
{
    /// <summary>
    /// Defines the invocation model for the message subscription.
    /// </summary>
    public enum InvocationModel
    {
        /// <summary>
        /// The subscriber is invoked in the context and thread of the dispatcher of the message.
        /// If the message is broadcasted the subscriber is invoked in a different thread.
        /// </summary>
        Default,

        /// <summary>
        /// The subscriber is invoked in the main thread marshaling the call even 
        /// if the message has been broadcasted.
        /// </summary>
        Safe
    }

    /// <summary>
    /// A message broker is a mediator used to dispatch and 
    /// broadcast messages to all the subscribers in the system.
    /// </summary>
    public interface IMessageBroker
    {
        void Subscribe( object subscriber, object sender, Type messageType, Action<object, object> callback );
        
        void Subscribe( object subscriber, object sender, Type messageType, InvocationModel invocationModel, Action<object, object> callback );
        
        void Subscribe( object subscriber, Type messageType, Action<object, object> callback );
        
        void Subscribe( object subscriber, Type messageType, InvocationModel invocationModel, Action<object, object> callback );
        
        void Subscribe<T>( object subscriber, Action<object, T> callback );
        
        void Subscribe<T>( object subscriber, object sender, Action<object, T> callback );
        
        void Subscribe<T>( object subscriber, object sender, InvocationModel invocationModel, Action<object, T> callback );
        
        void Subscribe<T>( object subscriber, InvocationModel invocationModel, Action<object, T> callback );

        /// <summary>
        /// Unsubscribes the specified subscriber from all the subcscriptions.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        void Unsubscribe( Object subscriber );

        /// <summary>
        /// Unsubscribes the specified subscriber from all the subcscriptions to the supplied IMessage type.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        void Unsubscribe<T>( Object subscriber ) where T : class;

        /// <summary>
        /// Unsubscribes the specified subscriber from all the messages
        /// posted by the given sender.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender.</param>
        void Unsubscribe( Object subscriber, Object sender );

        /// <summary>
        /// Unsubscribes the specified subscriber from all the messages,
        /// of the given type T, posted by the given sender.
        /// </summary>
        /// <typeparam name="T">The message type filter.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender.</param>
        void Unsubscribe<T>( Object subscriber, Object sender ) where T : class;

        /// <summary>
        /// Unsubscribes the specified subscriber from the subcscription to the supplied IMessage type.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callback">The callback to unsubscribe.</param>
        void Unsubscribe<T>( Object subscriber, Action<object, T> callback ) where T : class;
        
        /// <summary>
        /// Broadcasts the specified message in an asynchronus manner without
        /// waiting for the execution of the subscribers.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        void Broadcast( object sender, object message );

        /// <summary>
        /// Dispatches the specified message in a synchronus manner waiting for 
        /// the execution of all the subscribers.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        Task DispatchAsync( Object sennder, Object message );
    }
}