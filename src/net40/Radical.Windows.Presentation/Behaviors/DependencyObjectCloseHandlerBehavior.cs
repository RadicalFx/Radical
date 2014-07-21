using System.Windows;
using System.Windows.Interactivity;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Messaging;

namespace Topics.Radical.Windows.Presentation.Behaviors
{
    /// <summary>
    /// Special behavior to handle view close requests.
    /// </summary>
    public class DependencyObjectCloseHandlerBehavior : Behavior<DependencyObject>
    {
        readonly IMessageBroker broker;
        readonly IConventionsHandler conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyObjectCloseHandlerBehavior"/> class.
        /// </summary>
        /// <param name="broker">The broker.</param>
        /// <param name="conventions">The conventions.</param>
        public DependencyObjectCloseHandlerBehavior( IMessageBroker broker, IConventionsHandler conventions )
        {
            this.broker = broker;
            this.conventions = conventions;
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            var view = this.AssociatedObject;
            this.broker.Subscribe<CloseViewRequest>( this, InvocationModel.Safe, ( s, m ) =>
            {
                var dc = this.conventions.GetViewDataContext( view, this.conventions.DefaultViewDataContextSearchBehavior );
                if ( m.ViewOwner == dc )
                {
                    var w = this.conventions.FindHostingWindowOf( m.ViewOwner );
                    if ( w != null )
                    {
#if !SILVERLIGHT
                        if ( m.DialogResult.HasValue )
                        {
                            w.DialogResult = m.DialogResult;
                        }
#endif
                        w.Close();
                    }
                }
            } );
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject,
        /// but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            if ( this.broker != null )
            {
                this.broker.Unsubscribe( this );
            }

            base.OnDetaching();
        }
    }
}