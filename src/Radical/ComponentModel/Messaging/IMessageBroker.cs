using System;

namespace Radical.ComponentModel.Messaging
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
        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe( object subscriber, object sender, Type messageType, Action<object, object> callback );

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe( object subscriber, object sender, Type messageType, InvocationModel invocationModel, Action<object, object> callback );

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe( object subscriber, Type messageType, Action<object, object> callback );

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe( object subscriber, Type messageType, InvocationModel invocationModel, Action<object, object> callback );

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>( object subscriber, Action<object, T> callback );

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe(object subscriber, object sender, Type messageType, Func<object, object, bool> callbackFilter, Action<object, object> callback);

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe(object subscriber, object sender, Type messageType, InvocationModel invocationModel, Func<object, object, bool> callbackFilter, Action<object, object> callback);

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe(object subscriber, Type messageType, Func<object, object, bool> callbackFilter, Action<object, object> callback);

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe(object subscriber, Type messageType, InvocationModel invocationModel, Func<object, object, bool> callbackFilter, Action<object, object> callback);

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>(object subscriber, Func<object, T, bool> callbackFilter, Action<object, T> callback);
        
        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>(object subscriber, Func<object, T, System.Threading.Tasks.Task> callback);

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>(object subscriber, Func<object, T, System.Threading.Tasks.Task<bool>> callbackFilter, Func<object, T, System.Threading.Tasks.Task> callback);

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>( object subscriber, object sender, Action<object, T> callback );

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>( object subscriber, object sender, InvocationModel invocationModel, Action<object, T> callback );

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>( object subscriber, InvocationModel invocationModel, Action<object, T> callback );
        
        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>(object subscriber, object sender, Func<object, T, bool> callbackFilter, Action<object, T> callback);

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>(object subscriber, object sender, InvocationModel invocationModel, Func<object, T, bool> callbackFilter, Action<object, T> callback);

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callbackFilter">The filter invoked to determine if the callback shopuld be invoked.</param>
        /// <param name="callback">The callback.</param>
        void Subscribe<T>(object subscriber, InvocationModel invocationModel, Func<object, T, bool> callbackFilter, Action<object, T> callback);

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
        void Unsubscribe<T>( Object subscriber );

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
        void Unsubscribe<T>( Object subscriber, Object sender );
            //where T : class, IMessage;

        /// <summary>
        /// Unsubscribes the specified subscriber from the subcscription to the supplied IMessage type.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callback">The callback to unsubscribe.</param>
        void Unsubscribe<T>( Object subscriber, Delegate callback );

        /// <summary>
        /// Broadcasts the specified message in an asynchronus manner without
        /// waiting for the execution of the subscribers.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        void Broadcast( Object sender, Object message );
        
        /// <summary>
        /// Broadcasts the specified message in an asynchronus manner without
        /// waiting for the execution of the subscribers.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        System.Threading.Tasks.Task BroadcastAsync( Object sender, Object message );

        /// <summary>
        /// Dispatches the specified message in a synchronus manner waiting for
        /// the execution of all the subscribers.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message to dispatch.</param>
        void Dispatch( Object sender, Object message );
    }
}