using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Topics.Radical.ComponentModel;
using Topics.Radical.ComponentModel.Messaging;
using System.Reflection;
using Topics.Radical.Validation;
using Topics.Radical.Linq;
using Topics.Radical.Reflection;
using System.Diagnostics;
using Topics.Radical.Conversions;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Topics.Radical.Messaging
{
    /// <summary>
    /// A message broker is a mediator used to dispatch and 
    /// broadcast messages to all the subscribers in the system.
    /// </summary>
    public class MessageBroker : IMessageBroker
    {
        /// <summary>
        /// A dictionary, of all the subscriptions, whose key is the message type
        /// and whose value is the list of Subscriptions for that message type
        /// </summary>
        readonly IDictionary<Type, IList<ISubscription>> msgSubsIndex = null;

        readonly CoreDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBroker"/> class.
        /// </summary>
        public MessageBroker( CoreDispatcher dispatcher )
        {
            this.dispatcher = dispatcher;
            this.msgSubsIndex = new Dictionary<Type, IList<ISubscription>>();
        }

        void SubscribeCore( Type messageType, ISubscription subscription )
        {
            if ( this.msgSubsIndex.ContainsKey( messageType ) )
            {
                var subscribers = this.msgSubsIndex[ messageType ];
                subscribers.Add( subscription );
            }
            else
            {
                this.msgSubsIndex.Add( messageType, new List<ISubscription>() { subscription } );
            }
        }

        ///// <summary>
        ///// Subscribes the given subscriber to notifications of the
        ///// given type of IMessage using the supplied callback.
        ///// </summary>
        ///// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        //public void Subscribe<T>( object subscriber, Action<T> callback )
        //    where T : class, IMessage
        //{
        //    this.Subscribe<T>( subscriber, InvocationModel.Default, callback );
        //}

        ///// <summary>
        ///// Subscribes the given subscriber to notifications of the 
        ///// given type of IMessage using the supplied callback only
        ///// if the sender is the specified reference.
        ///// </summary>
        ///// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="sender">The sender filter.</param>
        ///// <param name="callback">The callback.</param>
        //public void Subscribe<T>( object subscriber, object sender, Action<T> callback )
        //    where T : class, IMessage
        //{
        //    this.Subscribe<T>( subscriber, sender, InvocationModel.Default, callback );
        //}

        ///// <summary>
        ///// Subscribes the given subscriber to notifications of the
        ///// given type of IMessage using the supplied callback.
        ///// </summary>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="messageType">Type of the message.</param>
        ///// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        //public void Subscribe( object subscriber, Type messageType, Action<IMessage> callback )
        //{
        //    this.Subscribe( subscriber, messageType, InvocationModel.Default, callback );
        //}

        ///// <summary>
        ///// Subscribes the given subscriber to notifications of the
        ///// given type of IMessage using the supplied callback only
        ///// if the sender is the specified reference.
        ///// </summary>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="sender">The sender.</param>
        ///// <param name="messageType">Type of the message.</param>
        ///// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        //public void Subscribe( object subscriber, object sender, Type messageType, Action<IMessage> callback )
        //{
        //    this.Subscribe( subscriber, sender, messageType, InvocationModel.Default, callback );
        //}

        ///// <summary>
        ///// Subscribes the given subscriber to notifications of the
        ///// given type of IMessage using the supplied callback.
        ///// </summary>
        ///// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="invocationModel">The invocation model.</param>
        ///// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        //public void Subscribe<T>( object subscriber, InvocationModel invocationModel, Action<T> callback ) where T : class, IMessage
        //{
        //    Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
        //    Ensure.That( callback ).Named( () => callback ).IsNotNull();

        //    var subscription = new Subscription<T>( subscriber, callback, invocationModel, this.dispatcher );

        //    this.SubscribeCore( typeof( T ), subscription );
        //}

        ///// <summary>
        ///// Subscribes the given subscriber to notifications of the
        ///// given type of IMessage using the supplied callback.
        ///// </summary>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="messageType">Type of the message.</param>
        ///// <param name="invocationModel">The invocation model.</param>
        ///// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        //public void Subscribe( object subscriber, Type messageType, InvocationModel invocationModel, Action<IMessage> callback )
        //{
        //    Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
        //    Ensure.That( messageType ).Named( () => messageType ).IsNotNull().IsTrue( o => o.Is<IMessage>() );
        //    Ensure.That( callback ).Named( () => callback ).IsNotNull();

        //    var subscription = new Subscription<IMessage>( subscriber, callback, invocationModel, this.dispatcher );

        //    this.SubscribeCore( messageType, subscription );
        //}

        ///// <summary>
        ///// Subscribes the given subscriber to notifications of the
        ///// given type of IMessage using the supplied callback only
        ///// if the sender is the specified reference.
        ///// </summary>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="sender">The sender.</param>
        ///// <param name="messageType">Type of the message.</param>
        ///// <param name="invocationModel">The invocation model.</param>
        ///// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        //public void Subscribe( object subscriber, object sender, Type messageType, InvocationModel invocationModel, Action<IMessage> callback )
        //{
        //    Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
        //    Ensure.That( sender ).Named( () => sender ).IsNotNull();
        //    Ensure.That( messageType ).Named( () => messageType ).IsNotNull().IsTrue( o => o.Is<IMessage>() );
        //    Ensure.That( callback ).Named( () => callback ).IsNotNull();

        //    var subscription = new Subscription<IMessage>( subscriber, sender, callback, invocationModel, this.dispatcher );

        //    this.SubscribeCore( messageType, subscription );
        //}

        ///// <summary>
        ///// Subscribes the given subscriber to notifications of the
        ///// given type of IMessage using the supplied callback only
        ///// if the sender is the specified reference.
        ///// </summary>
        ///// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        ///// <param name="subscriber">The subscriber.</param>
        ///// <param name="sender">The sender filter.</param>
        ///// <param name="invocationModel">The invocation model.</param>
        ///// <param name="callback">The callback.</param>
        //public void Subscribe<T>( object subscriber, object sender, InvocationModel invocationModel, Action<T> callback ) where T : class, IMessage
        //{
        //    Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
        //    Ensure.That( sender ).Named( () => sender ).IsNotNull();
        //    Ensure.That( callback ).Named( () => callback ).IsNotNull();

        //    var subscription = new Subscription<T>( subscriber, sender, callback, invocationModel, this.dispatcher );

        //    this.SubscribeCore( typeof( T ), subscription );
        //}

        public void Subscribe( object subscriber, object sender, Type messageType, Action<object, object> callback )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( messageType ).Named( () => messageType ).IsNotNull();
            Ensure.That( callback ).Named( () => callback ).IsNotNull();

            var subscription = new Subscription(
                subscriber,
                sender,
                callback,
                InvocationModel.Default,
                this.dispatcher );

            this.SubscribeCore( messageType, subscription );
        }

        public void Subscribe( object subscriber, object sender, Type messageType, InvocationModel invocationModel, Action<object, object> callback )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( messageType ).Named( () => messageType ).IsNotNull();
            Ensure.That( invocationModel ).Named( () => invocationModel ).IsDefined();
            Ensure.That( callback ).Named( () => callback ).IsNotNull();

            var subscription = new Subscription(
                subscriber,
                sender,
                callback,
                invocationModel,
                this.dispatcher );

            this.SubscribeCore( messageType, subscription );
        }

        public void Subscribe( object subscriber, Type messageType, Action<object, object> callback )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( messageType ).Named( () => messageType ).IsNotNull();
            Ensure.That( callback ).Named( () => callback ).IsNotNull();

            var subscription = new Subscription(
                subscriber,
                callback,
                InvocationModel.Default,
                this.dispatcher );

            this.SubscribeCore( messageType, subscription );
        }

        public void Subscribe( object subscriber, Type messageType, InvocationModel invocationModel, Action<object, object> callback )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( messageType ).Named( () => messageType ).IsNotNull();
            Ensure.That( invocationModel ).Named( () => invocationModel ).IsDefined();
            Ensure.That( callback ).Named( () => callback ).IsNotNull();

            var subscription = new Subscription(
                subscriber,
                callback,
                invocationModel,
                this.dispatcher );

            this.SubscribeCore( messageType, subscription );
        }

        public void Subscribe<T>( object subscriber, Action<object, T> callback )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( callback ).Named( () => callback ).IsNotNull();

            var subscription = new Subscription(
                subscriber,
                callback,
                InvocationModel.Default,
                this.dispatcher );

            this.SubscribeCore( typeof( T ), subscription );
        }

        public void Subscribe<T>( object subscriber, object sender, Action<object, T> callback )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( callback ).Named( () => callback ).IsNotNull();

            var subscription = new Subscription(
                subscriber,
                sender,
                callback,
                InvocationModel.Default,
                this.dispatcher );

            this.SubscribeCore( typeof( T ), subscription );
        }

        public void Subscribe<T>( object subscriber, object sender, InvocationModel invocationModel, Action<object, T> callback )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( invocationModel ).Named( () => invocationModel ).IsDefined();
            Ensure.That( callback ).Named( () => callback ).IsNotNull();

            var subscription = new Subscription(
                subscriber,
                sender,
                callback,
                invocationModel,
                this.dispatcher );

            this.SubscribeCore( typeof( T ), subscription );
        }

        public void Subscribe<T>( object subscriber, InvocationModel invocationModel, Action<object, T> callback )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( invocationModel ).Named( () => invocationModel ).IsDefined();
            Ensure.That( callback ).Named( () => callback ).IsNotNull();

            var subscription = new Subscription(
                subscriber,
                callback,
                invocationModel,
                this.dispatcher );

            this.SubscribeCore( typeof( T ), subscription );
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from all the subcscriptions.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe( object subscriber )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();

            foreach ( var subscription in this.msgSubsIndex )
            {
                var count = subscription.Value.Count;
                for ( var k = count; k > 0; k-- )
                {
                    var sub = subscription.Value[ k - 1 ];
                    if ( sub.Subscriber == subscriber )
                    {
                        subscription.Value.Remove( sub );
                    }
                }
            }

            this.msgSubsIndex.Where( msgSubscriptions => msgSubscriptions.Value.Count == 0 )
                .ToList()
                .ForEach( kvp => this.msgSubsIndex.Remove( kvp ) );
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from all the messages
        /// posted by the given sender.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender.</param>
        public void Unsubscribe( Object subscriber, Object sender )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();

            this.msgSubsIndex.Where( msgSubscriptions =>
            {
                return msgSubscriptions.Value.Where( subscription =>
                {
                    return Object.Equals( subscription.Subscriber, subscriber ) && Object.Equals( subscription.Sender, sender );
                } ).Any();
            } )
            .ToList()
            .ForEach( kvp => this.msgSubsIndex.Remove( kvp ) );
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from all the subcscriptions to the supplied IMessage type.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe<T>( object subscriber )
            where T : class
        {
            Ensure.That( subscriber ).Named( "subscriber" ).IsNotNull();

            if ( this.msgSubsIndex.ContainsKey( typeof( T ) ) )
            {
                var allMessageSubscriptions = this.msgSubsIndex[ typeof( T ) ];
                allMessageSubscriptions.Where( subscription => Object.Equals( subscriber, subscription.Subscriber ) )
                    .ToList()
                    .ForEach( subscription => allMessageSubscriptions.Remove( subscription ) );
            }
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from all the messages,
        /// of the given type T, posted by the given sender.
        /// </summary>
        /// <typeparam name="T">The message type filter.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender.</param>
        public void Unsubscribe<T>( object subscriber, object sender )
            where T : class
        {
            Ensure.That( subscriber ).Named( "subscriber" ).IsNotNull();
            Ensure.That( sender ).Named( "sender" ).IsNotNull();

            if ( this.msgSubsIndex.ContainsKey( typeof( T ) ) )
            {
                var allMessageSubscriptions = this.msgSubsIndex[ typeof( T ) ];
                allMessageSubscriptions.Where( subscription => Object.Equals( subscriber, subscription.Subscriber ) && Object.Equals( sender, subscription.Sender ) )
                    .ToList()
                    .ForEach( subscription => allMessageSubscriptions.Remove( subscription ) );
            }
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from the subcscription to the supplied IMessage type.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callback">The callback to unsubscribe.</param>
        public void Unsubscribe<T>( object subscriber, Action<Object, T> callback )
            where T : class
        {
            Ensure.That( subscriber ).Named( "subscriber" ).IsNotNull();
            Ensure.That( callback ).Named( "callback" ).IsNotNull();

            if ( this.msgSubsIndex.ContainsKey( typeof( T ) ) )
            {
                var allMessageSubscriptions = this.msgSubsIndex[ typeof( T ) ];
                allMessageSubscriptions.Where( subscription =>
                {
                    return Object.Equals( subscriber, subscription.Subscriber )
                        && Object.Equals( callback, subscription.GetAction() );
                } )
                .ToList()
                .ForEach( subscription => allMessageSubscriptions.Remove( subscription ) );
            }
        }

        IEnumerable<ISubscription> GetSubscriptionsFor( Type messageType, Object sender )
        {
            var subscribtions = this.msgSubsIndex
                .Where( kvp => messageType.Is( kvp.Key ) )
                .SelectMany( kvp => kvp.Value );

            //var subscribtions = this.msgSubsIndex[ messageType ];
            var effectiveSubscribers = subscribtions.Where( s => s.Sender == null || s.Sender == sender )
                .AsReadOnly();

            return effectiveSubscribers;
        }

        /// <summary>
        /// Dispatches the specified message in a synchronus manner waiting for 
        /// the execution of all the subscribers.
        /// </summary>
        /// <param name="message">The message.</param>
        public async Task DispatchAsync( Object sender, Object message )
        {
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( message ).Named( () => message ).IsNotNull();

            var messageType = message.GetType();

            await Task.Run( () =>
            {
                message.As<IRequireToBeValid>( m => m.Validate() );
                var subscriptions = this.GetSubscriptionsFor( messageType, sender );
                //subscriptions.ForEach( subscription => subscription.DirectInvoke( message ) );
                if ( subscriptions.Any() )
                {
                    subscriptions.ForEach( subscription => subscription.Invoke( sender, message ) );
                }
            } );
        }

        /// <summary>
        /// Broadcasts the specified message in an asynchronus manner without
        /// waiting for the execution of the subscribers.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        public void Broadcast( Object sender, Object message )
        {
            Ensure.That( message ).Named( "message" ).IsNotNull();

            message.As<IRequireToBeValid>( m => m.Validate() );

            var subscriptions = this.GetSubscriptionsFor( message.GetType(), sender );

            if ( subscriptions.Any() )
            {
                subscriptions.ForEach( sub =>
                {
                    Task.Run( () => sub.Invoke( sender, message ) );
                } );
            }
        }
    }
}