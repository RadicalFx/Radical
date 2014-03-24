using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Topics.Radical;
using Topics.Radical.Reflection;
using Topics.Radical.Linq;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Validation;
using Topics.Radical.Windows.Phone.Shell.Services;
using Topics.Radical.Model;

namespace Topics.Radical.Windows.Phone.Shell.Presentation
{
    /// <summary>
    /// A base view model that can handle app lifecycle changes.
    /// </summary>
    public abstract class LifecycleAwareViewModel : AbstractViewModel
    {
        readonly IMessageBroker broker;

        /// <summary>
        /// Initializes a new instance of the <see cref="LifecycleAwareViewModel"/> class.
        /// </summary>
        /// <param name="broker">The broker.</param>
        protected LifecycleAwareViewModel( IMessageBroker broker )
        {
            Ensure.That( broker ).Named( () => broker ).IsNotNull();

            this.broker = broker;

            this.broker.Subscribe<LifecycleMessage>( this, ( s, msg ) =>
            {
                switch ( msg.Event )
                {
                    case LifecycleEvent.Closing:
                        this.OnClosing();
                        break;
                    case LifecycleEvent.Obscured:
                        this.OnObscured();
                        break;
                    case LifecycleEvent.Resumed:
                        this.OnResumed();
                        break;
                    case LifecycleEvent.Resuming:
                        this.OnResuming();
                        break;
                    case LifecycleEvent.Starting:
                        this.OnStarting();
                        break;
                    case LifecycleEvent.Suspending:
                        this.OnSuspending();
                        break;
                    case LifecycleEvent.Unobscured:
                        this.OnUnobscured();
                        break;
                }
            } );
        }

        /// <summary>
        /// Occurs when the app is closing.
        /// </summary>
        protected virtual void OnClosing()
        {

        }

        /// <summary>
        /// Occurs when the app is obscured.
        /// </summary>
        protected virtual void OnObscured()
        {

        }

        /// <summary>
        /// Occurs when the app is resumed.
        /// </summary>
        protected virtual void OnResumed()
        {

        }

        /// <summary>
        /// Occurs when the app is resuming.
        /// </summary>
        protected virtual void OnResuming()
        {

        }

        /// <summary>
        /// Occurs when the app is starting.
        /// </summary>
        protected virtual void OnStarting()
        {

        }

        /// <summary>
        /// Occurs when the app is suspending.
        /// </summary>
        protected virtual void OnSuspending()
        {

        }

        /// <summary>
        /// Occurs when the app is unobscured.
        /// </summary>
        protected virtual void OnUnobscured()
        {

        }
    }
}
