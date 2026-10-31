﻿using Radical.ComponentModel;
using Radical.ComponentModel.Messaging;
using Radical.Validation;
using System;
using System.Threading.Tasks;

namespace Radical.Messaging
{
    /// <summary>
    /// Defines the priority of a message subscription, controlling the order in which subscribers are invoked.
    /// </summary>
    public enum SubscriptionPriority
    {
        /// <summary>Highest priority; subscriber is invoked first.</summary>
        Highest = 3,
        /// <summary>High priority.</summary>
        High = 2,
        /// <summary>Above-normal priority.</summary>
        AboveNormal = 1,
        /// <summary>Normal (default) priority.</summary>
        Normal = 0,
        /// <summary>Below-normal priority.</summary>
        BelowNormal = -1,
        /// <summary>Low priority.</summary>
        Low = -2,
        /// <summary>Lowest priority; subscriber is invoked last.</summary>
        Lowest = -3
    }

    class Subscription
    {
        readonly IDispatcher dispatcher;
        readonly Delegate action;
        readonly Func<object, object, Task> callback;
        readonly Func<object, object, bool> callbackFilter;

        // action is the callback as supplied by the subscriber, used to unsubscribe it;
        // callback and callbackFilter are adapted to the untyped message.
        public Subscription(object subscriber, object sender, Delegate action, Func<object, object, Task> callback, Func<object, object, bool> callbackFilter, InvocationModel invocationModel, IDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(nameof(subscriber)).IsNotNull();
            Ensure.That(action).Named(nameof(action)).IsNotNull();
            Ensure.That(callback).Named(nameof(callback)).IsNotNull();
            Ensure.That(callbackFilter).Named(nameof(callbackFilter)).IsNotNull();
            Ensure.That(dispatcher).Named(nameof(dispatcher)).IsNotNull();

            Subscriber = subscriber;
            Sender = sender;
            this.action = action;
            this.callback = callback;
            this.callbackFilter = callbackFilter;
            InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            Priority = SubscriptionPriority.Normal;
        }

        public object Subscriber { get; }

        public object Sender { get; }

        public InvocationModel InvocationModel { get; }

        public SubscriptionPriority Priority { get; }

        /// <summary>
        /// Gets a reference to the callback delegate of this subscription.
        /// </summary>
        /// <returns>The callback delegate.</returns>
        public Delegate GetAction()
        {
            return action;
        }

        public bool ShouldInvoke(object sender, object message)
        {
            return callbackFilter(sender, message);
        }

        /// <summary>
        /// The subscriber invocation model is based on the <see cref="InvocationModel" /> property.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        /// <returns>A task that represents the asynchronous invocation.</returns>
        public Task InvokeAsync(object sender, object message)
        {
            return InvokeCore(sender, message, InvocationModel);
        }

        /// <summary>
        /// The subscriber is invoked in the same thread of the dispatcher.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        /// <returns>A task that represents the asynchronous invocation.</returns>
        public Task DirectInvokeAsync(object sender, object message)
        {
            return InvokeCore(sender, message, InvocationModel.Default);
        }

        Task InvokeCore(object sender, object message, InvocationModel invocationModel)
        {
            Task task;
            if (invocationModel == InvocationModel.Safe && !dispatcher.IsSafe)
            {
                // Only the synchronous part of the callback is marshalled: once it
                // awaits, it resumes on the dispatcher thread through its synchronization context.
                task = dispatcher.Dispatch(() => callback(sender, message));
            }
            else
            {
                task = callback(sender, message);
            }

            return task ?? Task.CompletedTask;
        }
    }
}
