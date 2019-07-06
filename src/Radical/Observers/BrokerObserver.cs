using Radical.ComponentModel.Messaging;
using Radical.Validation;
using System;

namespace Radical.Observers
{
    /// <summary>
    /// A static entry to simplify the creation a <see cref="MessageBrokerMonitor"/>.
    /// </summary>
    public static class BrokerObserver
    {
        /// <summary>
        /// Creates a new <see cref="MessageBrokerMonitor"/> that monitors 
        /// the specified message broker.
        /// </summary>
        /// <param name="broker">The message broker.</param>
        /// <returns>The new <see cref="MessageBrokerMonitor"/>.</returns>
        public static MessageBrokerMonitor Using(IMessageBroker broker)
        {
            return new MessageBrokerMonitor(broker);
        }
    }

    /// <summary>
    /// An observer to monitor messages that are handled by a given message broker.
    /// </summary>
    public class MessageBrokerMonitor :
        AbstractMonitor<IMessageBroker>
    {
        readonly IMessageBroker broker;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBrokerMonitor"/> class.
        /// </summary>
        /// <param name="broker">The message broker.</param>
        public MessageBrokerMonitor(IMessageBroker broker)
            : base(broker)
        {
            Ensure.That(broker).Named("broker").IsNotNull();
            this.broker = broker;
        }

        /// <summary>
        /// Waits for the specified message type and raise the Changed event whenever the
        /// specified message is dispatched or broadcasted.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <returns>This monitor instance.</returns>
        public MessageBrokerMonitor WaitingFor<TMessage>() where TMessage : class
        {
            return this.WaitingFor<TMessage>(m => true);
        }

        /// <summary>
        /// Waits for the specified message type and raise the Changed event whenever the
        /// specified message is dispatched or broadcasted.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="invocationModel">The invocation model.</param>
        /// <returns>This monitor instance.</returns>
        public MessageBrokerMonitor WaitingFor<TMessage>(InvocationModel invocationModel) where TMessage : class
        {
            return this.WaitingFor<TMessage>(m => true, invocationModel);
        }

        /// <summary>
        /// Waits for the specified message type and raise the Changed event 
        /// if the supplied confition is satisfied by the dispatched or broadcasted message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="filter">The filter condition.</param>
        /// <returns>This monitor instance.</returns>
        public MessageBrokerMonitor WaitingFor<TMessage>(Func<TMessage, bool> filter) where TMessage : class
        {
            this.broker.Subscribe<TMessage>(this, (sender, msg) =>
           {
               if (filter(msg))
               {
                   this.OnChanged();
               }
           });

            return this;
        }

        /// <summary>
        /// Waits for the specified message type and raise the Changed event 
        /// if the supplied confition is satisfied by the dispatched or broadcasted message.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="filter">The filter condition.</param>
        /// <param name="invocationModel">The invocation model.</param>
        /// <returns>This monitor instance.</returns>
        public MessageBrokerMonitor WaitingFor<TMessage>(Func<TMessage, bool> filter, InvocationModel invocationModel) where TMessage : class
        {
            this.broker.Subscribe<TMessage>(this, invocationModel, (sender, msg) =>
            {
                if (filter(msg))
                {
                    this.OnChanged();
                }
            });

            return this;
        }

        /// <summary>
        /// Called in order to allow inheritors to stop the monitoring operations.
        /// </summary>
        /// <param name="targetDisposed"><c>True</c> if this call is subsequent to the Dispose of the monitored instance.</param>
        protected override void OnStopMonitoring(bool targetDisposed)
        {
            this.broker.Unsubscribe(this);
        }
    }
}
