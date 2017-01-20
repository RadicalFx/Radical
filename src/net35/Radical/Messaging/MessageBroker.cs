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

#if !SILVERLIGHT
//using log4net;
using Topics.Radical.Diagnostics;
#endif

#if FX40
using System.Threading.Tasks;
#endif

namespace Topics.Radical.Messaging
{
    /// <summary>
    /// A message broker is a mediator used to dispatch and 
    /// broadcast messages to all the subscribers in the system.
    /// </summary>
    public class MessageBroker : IMessageBroker
    {
        class SubscriptionsContainer
        {
            public SubscriptionsContainer(Type messageType)
            {
                this.MessageType = messageType;
                this.Subscriptions = new List<ISubscription>();
            }

            public Type MessageType { get; private set; }

            public List<ISubscription> Subscriptions { get; private set; }
        }

#if !SILVERLIGHT
        static readonly TraceSource logger = new TraceSource(typeof(MessageBroker).FullName);
        //        static readonly ILog logger = LogManager.GetLogger( typeof( MessageBroker ) );
#endif

#if FX40
        TaskFactory factory = null;
#endif

        readonly IDispatcher dispatcher;

        /// <summary>
        /// A dictionary, of all the subscriptions, whose key is the message type
        /// and whose value is the list of Subscriptions for that message type
        /// </summary>
        readonly List<SubscriptionsContainer> msgSubsIndex = null;
        readonly ReaderWriterLockSlim msgSubsIndexLock = new ReaderWriterLockSlim();

        //Dictionary<String, IScopedMessageBroker> topics = new Dictionary<string, IScopedMessageBroker>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBroker"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public MessageBroker(IDispatcher dispatcher)
        {
            Ensure.That(dispatcher).Named("dispatcher").IsNotNull();

            this.dispatcher = dispatcher;
            this.msgSubsIndex = new List<SubscriptionsContainer>();

#if FX40
            this.factory = new TaskFactory();
#endif
        }

#if FX40

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBroker"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="factory">The factory.</param>
        public MessageBroker(IDispatcher dispatcher, TaskFactory factory)
        {
            Ensure.That(dispatcher).Named("dispatcher").IsNotNull();
            Ensure.That(factory).Named(() => factory).IsNotNull();

            this.dispatcher = dispatcher;
            this.factory = factory;
            this.msgSubsIndex = new List<SubscriptionsContainer>();
        }

#endif

        void SubscribeCore(Type messageType, ISubscription subscription)
        {
            msgSubsIndexLock.EnterUpgradeableReadLock();
            try
            {
                if (this.msgSubsIndex.Any(sc => sc.MessageType == messageType))
                {
                    var allMessageSubscriptions = this.msgSubsIndex.Single(sc => sc.MessageType == messageType).Subscriptions;
                    msgSubsIndexLock.EnterWriteLock();
                    try
                    {
                        allMessageSubscriptions.Add(subscription);
                    }
                    finally
                    {
                        msgSubsIndexLock.ExitWriteLock();
                    }
                }
                else
                {
                    msgSubsIndexLock.EnterWriteLock();
                    try
                    {
                        var sc = new SubscriptionsContainer(messageType);
                        sc.Subscriptions.Add(subscription);
                        this.msgSubsIndex.Add(sc);
                    }
                    finally
                    {
                        msgSubsIndexLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                msgSubsIndexLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of IMessage using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        public void Subscribe<T>(object subscriber, Action<T> callback)
            where T : class, IMessage
        {
            this.Subscribe<T>(subscriber, InvocationModel.Default, callback);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callback">The callback.</param>
        public void Subscribe<T>(object subscriber, Action<Object, T> callback)
        {
            this.Subscribe<T>(subscriber, InvocationModel.Default, callback);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the 
        /// given type of IMessage using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="callback">The callback.</param>
        public void Subscribe<T>(object subscriber, object sender, Action<T> callback)
            where T : class, IMessage
        {
            this.Subscribe<T>(subscriber, sender, InvocationModel.Default, callback);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="callback">The callback.</param>
        public void Subscribe<T>(object subscriber, object sender, Action<Object, T> callback)
        {
            this.Subscribe<T>(subscriber, sender, InvocationModel.Default, callback);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of IMessage using the supplied callback.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        public void Subscribe(object subscriber, Type messageType, Action<IMessage> callback)
        {
            this.Subscribe(subscriber, messageType, InvocationModel.Default, callback);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="callback">The callback.</param>
        public void Subscribe(object subscriber, Type messageType, Action<Object, Object> callback)
        {
            this.Subscribe(subscriber, messageType, InvocationModel.Default, callback);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of IMessage using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        public void Subscribe(object subscriber, object sender, Type messageType, Action<IMessage> callback)
        {
            this.Subscribe(subscriber, sender, messageType, InvocationModel.Default, callback);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="callback">The callback.</param>
        public void Subscribe(object subscriber, object sender, Type messageType, Action<Object, Object> callback)
        {
            this.Subscribe(subscriber, sender, messageType, InvocationModel.Default, callback);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of IMessage using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        public void Subscribe<T>(object subscriber, InvocationModel invocationModel, Action<T> callback) where T : class, IMessage
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new GenericSubscription<T>(subscriber, callback, invocationModel, this.dispatcher);

            this.SubscribeCore(typeof(T), subscription);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback.</param>
        public void Subscribe<T>(object subscriber, InvocationModel invocationModel, Action<Object, T> callback)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new PocoSubscription<T>(subscriber, callback, invocationModel, this.dispatcher);

            this.SubscribeCore(typeof(T), subscription);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of IMessage using the supplied callback.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        public void Subscribe(object subscriber, Type messageType, InvocationModel invocationModel, Action<IMessage> callback)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(messageType).Named(() => messageType).IsNotNull().IsTrue(o => o.Is<IMessage>());
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new GenericSubscription<IMessage>(subscriber, callback, invocationModel, this.dispatcher);

            this.SubscribeCore(messageType, subscription);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of message using the supplied callback only.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback.</param>
        public void Subscribe(object subscriber, Type messageType, InvocationModel invocationModel, Action<Object, Object> callback)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(messageType).Named(() => messageType).IsNotNull(); //.IsTrue( o => o.Is<IMessage>() );
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new PocoSubscription(subscriber, callback, invocationModel, this.dispatcher);

            this.SubscribeCore(messageType, subscription);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of IMessage using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback to invoke in order to notify the message arrival.</param>
        public void Subscribe(object subscriber, object sender, Type messageType, InvocationModel invocationModel, Action<IMessage> callback)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();
            Ensure.That(messageType).Named(() => messageType).IsNotNull().IsTrue(o => o.Is<IMessage>());
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new GenericSubscription<IMessage>(subscriber, sender, callback, invocationModel, this.dispatcher);

            this.SubscribeCore(messageType, subscription);
        }

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
        public void Subscribe(object subscriber, object sender, Type messageType, InvocationModel invocationModel, Action<Object, Object> callback)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();
            Ensure.That(messageType).Named(() => messageType).IsNotNull().IsTrue(o => o.Is<IMessage>());
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new PocoSubscription(subscriber, sender, callback, invocationModel, this.dispatcher);

            this.SubscribeCore(messageType, subscription);
        }

        /// <summary>
        /// Subscribes the given subscriber to notifications of the
        /// given type of IMessage using the supplied callback only
        /// if the sender is the specified reference.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender filter.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <param name="callback">The callback.</param>
        public void Subscribe<T>(object subscriber, object sender, InvocationModel invocationModel, Action<T> callback) where T : class, IMessage
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new GenericSubscription<T>(subscriber, sender, callback, invocationModel, this.dispatcher);

            this.SubscribeCore(typeof(T), subscription);
        }

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
        public void Subscribe<T>(object subscriber, object sender, InvocationModel invocationModel, Action<Object, T> callback)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new PocoSubscription<T>(subscriber, sender, callback, invocationModel, this.dispatcher);

            this.SubscribeCore(typeof(T), subscription);
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from all the subcscriptions.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe(object subscriber)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();

            msgSubsIndexLock.EnterUpgradeableReadLock();
            try
            {
                foreach (var subscription in this.msgSubsIndex)
                {
                    var count = subscription.Subscriptions.Count;
                    for (var k = count; k > 0; k--)
                    {
                        var sub = subscription.Subscriptions[k - 1];
                        if (sub.Subscriber == subscriber)
                        {
                            msgSubsIndexLock.EnterWriteLock();
                            try
                            {
                                subscription.Subscriptions.Remove(sub);
                            }
                            finally
                            {
                                msgSubsIndexLock.ExitWriteLock();
                            }
                        }
                    }
                }

                this.msgSubsIndex.Where(msgSubscriptions => msgSubscriptions.Subscriptions.Count == 0)
                    .ToList()
                    .ForEach(kvp =>
                   {
                       msgSubsIndexLock.EnterWriteLock();
                       try
                       {
                           this.msgSubsIndex.Remove(kvp);
                       }
                       finally
                       {
                           msgSubsIndexLock.ExitWriteLock();
                       }
                   });
            }
            finally
            {
                msgSubsIndexLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from all the messages
        /// posted by the given sender.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender.</param>
        public void Unsubscribe(Object subscriber, Object sender)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();

            msgSubsIndexLock.EnterUpgradeableReadLock();
            try
            {
                this.msgSubsIndex.Where(msgSubscriptions =>
               {
                   return msgSubscriptions.Subscriptions.Where(subscription =>
                   {
                       return Object.Equals(subscription.Subscriber, subscriber)
                              && Object.Equals(subscription.Sender, sender);
                   })
                   .Any();
               })
                .ToList()
                .ForEach(kvp =>
               {
                   msgSubsIndexLock.EnterWriteLock();
                   try
                   {
                       this.msgSubsIndex.Remove(kvp);
                   }
                   finally
                   {
                       msgSubsIndexLock.ExitWriteLock();
                   }
               });
            }
            finally
            {
                msgSubsIndexLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from all the subcscriptions to the supplied IMessage type.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        public void Unsubscribe<T>(object subscriber)
        {
            Ensure.That(subscriber).Named("subscriber").IsNotNull();

            msgSubsIndexLock.EnterUpgradeableReadLock();
            try
            {
                if (this.msgSubsIndex.Any(sc => sc.MessageType == typeof(T)))
                {
                    var allMessageSubscriptions = this.msgSubsIndex.Single(sc => sc.MessageType == typeof(T)).Subscriptions;
                    allMessageSubscriptions.Where(subscription =>
                   {
                       return Object.Equals(subscriber, subscription.Subscriber);
                   })
                    .ToList()
                    .ForEach(subscription =>
                   {
                       msgSubsIndexLock.EnterWriteLock();
                       try
                       {
                           allMessageSubscriptions.Remove(subscription);
                       }
                       finally
                       {
                           msgSubsIndexLock.ExitWriteLock();
                       }
                   });
                }
            }
            finally
            {
                msgSubsIndexLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from all the messages,
        /// of the given type T, posted by the given sender.
        /// </summary>
        /// <typeparam name="T">The message type filter.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="sender">The sender.</param>
        public void Unsubscribe<T>(object subscriber, object sender)
        {
            Ensure.That(subscriber).Named("subscriber").IsNotNull();
            Ensure.That(sender).Named("sender").IsNotNull();

            msgSubsIndexLock.EnterUpgradeableReadLock();
            try
            {
                if (this.msgSubsIndex.Any(sc => sc.MessageType == typeof(T)))
                {
                    var allMessageSubscriptions = this.msgSubsIndex.Single(sc => sc.MessageType == typeof(T)).Subscriptions;
                    allMessageSubscriptions.Where(subscription =>
                   {
                       return Object.Equals(subscriber, subscription.Subscriber)
                              && Object.Equals(sender, subscription.Sender);
                   })
                    .ToList()
                    .ForEach(subscription =>
                   {
                       msgSubsIndexLock.EnterWriteLock();
                       try
                       {
                           allMessageSubscriptions.Remove(subscription);
                       }
                       finally
                       {
                           msgSubsIndexLock.ExitWriteLock();
                       }

                   });
                }
            }
            finally
            {
                msgSubsIndexLock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// Unsubscribes the specified subscriber from the subcscription to the supplied IMessage type.
        /// </summary>
        /// <typeparam name="T">The type of message the subecriber is interested in.</typeparam>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="callback">The callback to unsubscribe.</param>
        public void Unsubscribe<T>(object subscriber, Delegate callback)
        {
            Ensure.That(subscriber).Named("subscriber").IsNotNull();
            Ensure.That(callback).Named("callback").IsNotNull();

            msgSubsIndexLock.EnterUpgradeableReadLock();
            try
            {
                if (this.msgSubsIndex.Any(sc => sc.MessageType == typeof(T)))
                {
                    var allMessageSubscriptions = this.msgSubsIndex.Single(sc => sc.MessageType == typeof(T)).Subscriptions;
                    allMessageSubscriptions.Where(subscription =>
                   {
                       return Object.Equals(subscriber, subscription.Subscriber)
                              && Object.Equals(callback, subscription.GetAction());
                   })
                    .ToList()
                    .ForEach(subscription =>
                   {
                       msgSubsIndexLock.EnterWriteLock();
                       try
                       {
                           allMessageSubscriptions.Remove(subscription);
                       }
                       finally
                       {
                           msgSubsIndexLock.ExitWriteLock();
                       }
                   });
                }
            }
            finally
            {
                msgSubsIndexLock.ExitUpgradeableReadLock();
            }
        }

        IEnumerable<ISubscription> GetSubscriptionsFor(Type messageType, Object sender)
        {
            msgSubsIndexLock.EnterReadLock();
            try
            {
                var subscriptions = this.msgSubsIndex
                                        .Where(kvp => messageType.Is(kvp.MessageType))
                                        .SelectMany(kvp => kvp.Subscriptions);

                var effectiveSubscribers = subscriptions.Where(s => s.Sender == null || s.Sender == sender)
                                                        .OrderByDescending(s => s.Priority)
                                                        .AsReadOnly();

                return effectiveSubscribers;
            }
            finally
            {
                msgSubsIndexLock.ExitReadLock();
            }
        }

#if !SILVERLIGHT

        /// <summary>
        /// Dispatches the specified message in a synchronus manner waiting for 
        /// the execution of all the subscribers.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        public void Dispatch<T>(T message)
            where T : class, IMessage
        {
            //Ensure.That( message ).Named( () => message ).IsNotNull();

            this.Dispatch(typeof(T), message);
        }

        /// <summary>
        /// Dispatches the specified message in a synchronus manner waiting for
        /// the execution of all the subscribers.
        /// </summary>
        /// <param name="message">The message to dispatch.</param>
        public void Dispatch(IMessage message)
        {
            Ensure.That(message).Named(() => message).IsNotNull();

            this.Dispatch(message.GetType(), message);
        }

        /// <summary>
        /// Dispatches the specified message in a synchronus manner waiting for
        /// the execution of all the subscribers.
        /// </summary>
        /// <param name="messageType">The Type of the message to dispatch.</param>
        /// <param name="message">The message to dispatch.</param>
        public void Dispatch(Type messageType, IMessage message)
        {
            Ensure.That(messageType).Named(() => messageType).IsNotNull();
            Ensure.That(message).Named(() => message).IsNotNull().IsTrue(m => m.GetType().Is(messageType));

            message.As<IRequireToBeValid>(m => m.Validate());

            var subscriptions = this.GetSubscriptionsFor(messageType, message.Sender);
            var anySubscription = subscriptions.Any();

            if (!anySubscription)
            {
                logger.Warning("No Subscribers for the given message type: {0}", messageType.ToString("SN"));
            }
            else
            {
                subscriptions.ForEach(subscription => subscription.DirectInvoke(message.Sender, message));
            }
        }

        public void Dispatch(Object sender, Object message)
        {
            Ensure.That(sender).Named(() => sender).IsNotNull();
            Ensure.That(message).Named(() => message).IsNotNull();

            message.As<IRequireToBeValid>(m => m.Validate());
            message.As<ILegacyMessageCompatibility>(m => m.SetSenderForBackwardCompatibility(sender));

            var messageType = message.GetType();
            var subscriptions = this.GetSubscriptionsFor(messageType, sender);
            var anySubscription = subscriptions.Any();

            if (!anySubscription)
            {
                logger.Warning("No Subscribers for the given message type: {0}", messageType.ToString("SN"));
            }
            else
            {
                subscriptions.ForEach(subscription => subscription.DirectInvoke(sender, message));
            }
        }
#endif

        /// <summary>
        /// Broadcasts the specified message in an asynchronus manner without
        /// waiting for the execution of the subscribers.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        public void Broadcast<T>(T message)
            where T : class, IMessage
        {
            this.Broadcast(typeof(T), message);
        }

        /// <summary>
        /// Broadcasts the specified message in an asynchronus manner without
        /// waiting for the execution of the subscribers.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="message">The message.</param>
        public void Broadcast(Type messageType, IMessage message)
        {
            Ensure.That(message).Named("message").IsNotNull();

            message.As<IRequireToBeValid>(m => m.Validate());

            var subscriptions = this.GetSubscriptionsFor(messageType, message.Sender);

            if (subscriptions.Any())
            {
#if FX40
                subscriptions.ForEach(sub =>
               {
                   this.factory.StartNew(() =>
                   {
                       sub.Invoke(message.Sender, message);
                   });
               });
#else
                subscriptions.ForEach( sub => ThreadPool.QueueUserWorkItem( o =>
                {
                    var msg = ( IMessage )o;
                    sub.Invoke( msg.Sender, msg );
                }, message ) );
#endif
            }
        }

        public void Broadcast(Object sender, Object message)
        {
            Ensure.That(message).Named(() => message).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();

            message.As<IRequireToBeValid>(m => m.Validate());
            message.As<ILegacyMessageCompatibility>(m => m.SetSenderForBackwardCompatibility(sender));

            var subscriptions = this.GetSubscriptionsFor(message.GetType(), sender);

            if (subscriptions.Any())
            {
#if FX40
                subscriptions.ForEach(sub =>
               {
                   this.factory.StartNew(() =>
                   {
                       sub.Invoke(sender, message);
                   });
               });
#else
                subscriptions.ForEach( sub => ThreadPool.QueueUserWorkItem( o =>
                {
                    var state = ( Object[] )o;
                    sub.Invoke( state[ 0 ], state[ 1 ] );
                }, new[] { sender, message } ) );
#endif
            }
        }

#if FX45
        /// <summary>
        /// Broadcasts the specified message in an asynchronus manner without
        /// waiting for the execution of the subscribers.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        public Task BroadcastAsync(Object sender, Object message)
        {
            Ensure.That(message).Named(() => message).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();

            message.As<IRequireToBeValid>(m => m.Validate());
            message.As<ILegacyMessageCompatibility>(m => m.SetSenderForBackwardCompatibility(sender));

            var subscriptions = this.GetSubscriptionsFor(message.GetType(), sender);

            var tasks = new List<Task>();
            if (subscriptions.Any())
            {
                subscriptions.ForEach(sub =>
               {
                   var _as = sub as IAsyncSubscription;
                   if (_as != null)
                   {
                       tasks.Add(_as.InvokeAsync(sender, message));
                   }
                   else
                   {
                       tasks.Add(this.factory.StartNew(() =>
                       {
                           sub.Invoke(sender, message);
                       }));
                   }
               });

            }

            return Task.WhenAll(tasks.ToArray());
        }

        public void Subscribe<T>(object subscriber, Func<object, T, Task> callback)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(callback).Named(() => callback).IsNotNull();

            var subscription = new PocoAsyncSubscription<T>(subscriber, callback, InvocationModel.Default, this.dispatcher);

            this.SubscribeCore(typeof(T), subscription);
        }

#endif
    }
}