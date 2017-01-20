using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Validation;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Messaging
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

        /// <summary>
        /// The subscriber invocation model is based on the <see cref="InvocationModel" /> property.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        void Invoke( Object sender, Object message );

        /// <summary>
        /// The subscriber is invoked in the same thread of the dispatcher.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="message">The message.</param>
        void DirectInvoke( Object sender, Object message );
    }

#if FX45

    interface IAsyncSubscription : ISubscription
    {
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

        public PocoAsyncSubscription(Object subscriber, Func<Object, T, System.Threading.Tasks.Task> action, InvocationModel invocationModel, IDispatcher dispatcher)
        {
            Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
            Ensure.That(action).Named(() => action).IsNotNull();
            Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();
            Ensure.That(invocationModel).Is(InvocationModel.Default);

            this.Subscriber = subscriber;
            this.action = action;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        //public PocoSubscription(Object subscriber, Object sender, Action<Object, T> action, InvocationModel invocationModel, IDispatcher dispatcher)
        //{
        //    Ensure.That(subscriber).Named(() => subscriber).IsNotNull();
        //    Ensure.That(sender).Named(() => sender).IsNotNull();
        //    Ensure.That(action).Named(() => action).IsNotNull();
        //    Ensure.That(dispatcher).Named(() => dispatcher).IsNotNull();

        //    this.Subscriber = subscriber;
        //    this.action = action;
        //    this.Sender = sender;
        //    this.InvocationModel = invocationModel;
        //    this.dispatcher = dispatcher;

        //    this.Priority = SubscriptionPriority.Normal;
        //}

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
    }

#endif

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

        public PocoSubscription( Object subscriber, Action<Object, Object> action, InvocationModel invocationModel, IDispatcher dispatcher )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( action ).Named( () => action ).IsNotNull();
            Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        public PocoSubscription( Object subscriber, Object sender, Action<Object, Object> action, InvocationModel invocationModel, IDispatcher dispatcher )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( action ).Named( () => action ).IsNotNull();
            Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
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

        public void Invoke( Object sender, Object message )
        {
            InvokeCore( this.dispatcher, this.action, sender, message, this.InvocationModel );
        }

        public void DirectInvoke( Object sender, Object message )
        {
            InvokeCore( this.dispatcher, this.action, sender, message, InvocationModel.Default );
        }

        static void InvokeCore( IDispatcher dispatcher, Action<Object, Object> action, Object sender, Object message, InvocationModel type )
        {
            if ( type == InvocationModel.Safe && !dispatcher.IsSafe )
            {
                dispatcher.Dispatch<Object, Object>( sender, message, action );
            }
            else
            {
                action( sender, message );
            }
        }
    }

    class PocoSubscription<T> : ISubscription
    {
        readonly IDispatcher dispatcher;
        readonly Action<Object, T> action;

        public PocoSubscription( Object subscriber, Action<Object, T> action, InvocationModel invocationModel, IDispatcher dispatcher )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( action ).Named( () => action ).IsNotNull();
            Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        public PocoSubscription( Object subscriber, Object sender, Action<Object, T> action, InvocationModel invocationModel, IDispatcher dispatcher )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( action ).Named( () => action ).IsNotNull();
            Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
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

        public void Invoke( Object sender, Object message )
        {
            InvokeCore( this.dispatcher, this.action, sender, ( T )message, this.InvocationModel );
        }

        public void DirectInvoke( Object sender, Object message )
        {
            InvokeCore( this.dispatcher, this.action, sender, ( T )message, InvocationModel.Default );
        }

        static void InvokeCore( IDispatcher dispatcher, Action<Object, T> action, Object sender, T message, InvocationModel type )
        {
            if ( type == InvocationModel.Safe && !dispatcher.IsSafe )
            {
                dispatcher.Dispatch<Object, T>( sender, message, action );
            }
            else
            {
                action( sender, message );
            }
        }
    }

    class GenericSubscription<T> : ISubscription where T : IMessage
    {
        readonly IDispatcher dispatcher;
        readonly Action<T> action;

        public GenericSubscription( Object subscriber, Action<T> action, InvocationModel invocationModel, IDispatcher dispatcher )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( action ).Named( () => action ).IsNotNull();
            Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
            this.Sender = null;
            this.InvocationModel = invocationModel;
            this.dispatcher = dispatcher;

            this.Priority = SubscriptionPriority.Normal;
        }

        public GenericSubscription( Object subscriber, Object sender, Action<T> action, InvocationModel invocationModel, IDispatcher dispatcher )
        {
            Ensure.That( subscriber ).Named( () => subscriber ).IsNotNull();
            Ensure.That( sender ).Named( () => sender ).IsNotNull();
            Ensure.That( action ).Named( () => action ).IsNotNull();
            Ensure.That( dispatcher ).Named( () => dispatcher ).IsNotNull();

            this.Subscriber = subscriber;
            this.action = action;
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

        public void Invoke( Object sender, Object message )
        {
            var msg = ( T )message;
            InvokeCore( this.dispatcher, this.action, msg, this.InvocationModel );
        }

        public void DirectInvoke( Object sender, Object message )
        {
            var msg = ( T )message;
            InvokeCore( this.dispatcher, this.action, msg, InvocationModel.Default );
        }

        static void InvokeCore( IDispatcher dispatcher, Action<T> action, T message, InvocationModel type )
        {
            if ( type == InvocationModel.Safe && !dispatcher.IsSafe )
            {
                dispatcher.Dispatch( ( T )message, action );
            }
            else
            {
                action( ( T )message );
            }
        }
    }
}