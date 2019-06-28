using Radical.ComponentModel;
using Radical.ComponentModel.Messaging;
using Radical.Validation;
using System;

namespace Radical.Messaging
{
    interface ISubscription
    {
        Object Subscriber { get; }
        Object Sender { get; }

        InvocationModel InvocationModel { get; }

        SubscriptionPriority Priority { get; }

        /// <summary>
        /// Gets a reference to the callback delegate of this subscription.
        /// </summary>
        /// <returns></returns>
        Delegate GetAction();

        bool ShouldInvoke(Object sender, Object message);

        /// <summary>
        /// The subscriber invocation model is based on the <see cref="InvocationModel" /> property.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        void Invoke(Object sender, Object message);

        /// <summary>
        /// The subscriber is invoked in the same thread of the dispatcher.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        void DirectInvoke(Object sender, Object message);
    }

    interface IAsyncSubscription : ISubscription
    {
        //we can't do this now, till we have mixed async and sync subscriber
        //System.Threading.Tasks.Task<bool> ShouldInvokeAsync(Object sender, Object message);

        /// <summary>
        /// The subscriber invocation model is based on the <see cref="InvocationModel" /> property.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        System.Threading.Tasks.Task InvokeAsync(Object sender, Object message);

        /// <summary>
        /// The subscriber is invoked in the same thread of the dispatcher.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        System.Threading.Tasks.Task DirectInvokeAsync(Object sender, Object message);
    }

    class PocoAsyncSubscription<T> : IAsyncSubscription
    {
        readonly IDispatcher dispatcher;
        readonly Func<Object, T, System.Threading.Tasks.Task> action;
        readonly Func<Object, T, System.Threading.Tasks.Task<bool>> actionFilter;

        public PocoAsyncSubscription(Object subscriber, Func<Object, T, System.Threading.Tasks.Task> action, Func<Object, T, System.Threading.Tasks.Task<bool>> actionFilter, InvocationModel invocationModel, IDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(action).Named(() => action).IsNotNull();
            Ensure.That(actionFilter).Named(() => actionFilter).IsNotNull();
            Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();
            Ensure.That(invocationModel).Is(InvocationModel.Default);

            this.Subscriber = subscriber;
            this.action = action;
            this.actionFilter = actionFilter;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        public Delegate GetAction()
        {
            return this.action;
        }

        public Object Subscriber { get; private set; }

        public Object Sender { get; private set; }

        public InvocationModel InvocationModel { get; private set; }

        public SubscriptionPriority Priority { get; private set; }

        public void Invoke(Object sender, Object message)
        {
            throw new NotSupportedException();
        }

        public void DirectInvoke(Object sender, Object message)
        {
            throw new NotSupportedException();
        }

        static System.Threading.Tasks.Task InvokeCoreAsync(IDispatcher dispatcher, Func<Object, T, System.Threading.Tasks.Task> action, Object sender, T message, InvocationModel type)
        {
            return action(sender, message);
        }

        public System.Threading.Tasks.Task InvokeAsync(object sender, object message)
        {
            return InvokeCoreAsync(this.dispatcher, this.action, sender, (T)message, InvocationModel.Default);
        }

        public System.Threading.Tasks.Task DirectInvokeAsync(object sender, object message)
        {
            return InvokeCoreAsync(this.dispatcher, this.action, sender, (T)message, InvocationModel.Default);
        }

        public bool ShouldInvoke(object sender, object message)
        {
            return this.actionFilter(sender, (T)message).GetAwaiter().GetResult();
        }
    }

    public enum SubscriptionPriority
    {
        Highest = 3,
        High = 2,
        AboveNormal = 1,
        Normal = 0,
        BelowNormal = -1,
        Low = -2,
        Lowest = -3
    }

    class PocoSubscription : ISubscription
    {
        readonly IDispatcher dispatcher;
        readonly Action<Object, Object> action;
        private Func<object, object, bool> actionFilter;

        public PocoSubscription(Object subscriber, Action<Object, Object> action, Func<Object, Object, bool> actionFilter, InvocationModel invocationModel, IDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(action).Named(() => action).IsNotNull();
            Ensure.That(actionFilter).Named(() => actionFilter).IsNotNull();
            Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.actionFilter = actionFilter;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        public PocoSubscription(Object subscriber, Object sender, Action<Object, Object> action, Func<Object, Object, bool> actionFilter, InvocationModel invocationModel, IDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();
            Ensure.That(action).Named(() => action).IsNotNull();
            Ensure.That(actionFilter).Named(() => actionFilter).IsNotNull();
            Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.actionFilter = actionFilter;
            this.Sender = sender;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        public Delegate GetAction()
        {
            return this.action;
        }

        public Object Subscriber { get; private set; }

        public Object Sender { get; private set; }

        public InvocationModel InvocationModel { get; private set; }

        public SubscriptionPriority Priority { get; private set; }

        public void Invoke(Object sender, Object message)
        {
            InvokeCore(this.dispatcher, this.action, sender, message, this.InvocationModel);
        }

        public void DirectInvoke(Object sender, Object message)
        {
            InvokeCore(this.dispatcher, this.action, sender, message, InvocationModel.Default);
        }

        static void InvokeCore(IDispatcher dispatcher, Action<Object, Object> action, Object sender, Object message, InvocationModel type)
        {
            if (type == InvocationModel.Safe && !dispatcher.IsSafe)
            {
                dispatcher.Dispatch<Object, Object>(sender, message, action);
            }
            else
            {
                action(sender, message);
            }
        }

        public bool ShouldInvoke(object sender, object message)
        {
            return this.actionFilter(sender, message);
        }
    }

    class PocoSubscription<T> : ISubscription
    {
        readonly IDispatcher dispatcher;
        readonly Action<Object, T> action;
        private Func<object, T, bool> actionFilter;

        public PocoSubscription(Object subscriber, Action<Object, T> action, Func<Object, T, bool> actionFilter, InvocationModel invocationModel, IDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(action).Named(() => action).IsNotNull();
            Ensure.That(actionFilter).Named(() => actionFilter).IsNotNull();
            Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.actionFilter = actionFilter;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        public PocoSubscription(Object subscriber, Object sender, Action<Object, T> action, Func<Object, T, bool> actionFilter, InvocationModel invocationModel, IDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(sender).Named(() => sender).IsNotNull();
            Ensure.That(action).Named(() => action).IsNotNull();
            Ensure.That(actionFilter).Named(() => actionFilter).IsNotNull();
            Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.actionFilter = actionFilter;
            this.Sender = sender;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        public Delegate GetAction()
        {
            return this.action;
        }

        public Object Subscriber { get; private set; }

        public Object Sender { get; private set; }

        public InvocationModel InvocationModel { get; private set; }

        public SubscriptionPriority Priority { get; private set; }

        public void Invoke(Object sender, Object message)
        {
            InvokeCore(this.dispatcher, this.action, sender, (T)message, this.InvocationModel);
        }

        public void DirectInvoke(Object sender, Object message)
        {
            InvokeCore(this.dispatcher, this.action, sender, (T)message, InvocationModel.Default);
        }

        static void InvokeCore(IDispatcher dispatcher, Action<Object, T> action, Object sender, T message, InvocationModel type)
        {
            if (type == InvocationModel.Safe && !dispatcher.IsSafe)
            {
                dispatcher.Dispatch<Object, T>(sender, message, action);
            }
            else
            {
                action(sender, message);
            }
        }

        public bool ShouldInvoke(object sender, object message)
        {
            return this.actionFilter(sender, (T)message);
        }
    }
}