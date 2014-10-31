using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Messaging;
using Topics.Radical.Diagnostics;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows.Presentation.Behaviors
{
	/// <summary>
	/// Wires the window lifecycle to a view model that requires lifecycle notifications.
	/// </summary>
	public class WindowLifecycleNotificationsBehavior : RadicalBehavior<Window>
	{
        static readonly TraceSource logger = new TraceSource( typeof( WindowLifecycleNotificationsBehavior ).FullName );

		readonly IMessageBroker broker;
		readonly IConventionsHandler conventions;

		RoutedEventHandler loaded = null;
		EventHandler activated = null;
		EventHandler rendered = null;
		EventHandler closed = null;
		CancelEventHandler closing = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowLifecycleNotificationsBehavior"/> class.
		/// </summary>
		/// <param name="broker">The broker.</param>
		/// <param name="conventions">The conventions handler.</param>
		public WindowLifecycleNotificationsBehavior( IMessageBroker broker, IConventionsHandler conventions )
		{
            Ensure.That( broker ).Named( () => broker ).IsNotNull();
            Ensure.That( conventions ).Named( () => conventions ).IsNotNull();

			this.broker = broker;
			this.conventions = conventions;

			if( !DesignTimeHelper.GetIsInDesignMode() )
			{
                logger.Debug( "We are not running within a designer." );
                logger.Debug( "Ready to attach events." );

				this.loaded = ( s, e ) =>
				{
                    logger.Debug( "Loaded event raised." );
                    Ensure.That( this.AssociatedObject ).Named( "AssociatedObject" ).IsNotNull();

                    var view = this.AssociatedObject;
                    var dc = this.conventions.GetViewDataContext( view, this.conventions.DefaultViewDataContextSearchBehavior );

					if( this.conventions.ShouldNotifyViewModelLoaded( view, dc ) )
					{
                        logger.Debug( "ShouldNotifyViewModelLoaded -> true." );

                        this.broker.Broadcast( this, new ViewModelLoaded( dc ) );

                        logger.Debug( "Message broadcasted." );
					}
					
					if ( this.conventions.ShouldNotifyViewLoaded( view ) )
                    {
                        logger.Debug( "ShouldNotifyViewLoaded -> true." );

                        this.broker.Broadcast( this, new ViewLoaded( view ));

                        logger.Debug( "Message broadcasted." );
                    }

					var temp = dc as IExpectViewLoadedCallback;
                    if ( temp != null )
					{
                        logger.Debug( "DataContext is IExpectViewLoadedCallback." );

                        temp.OnViewLoaded();

                        logger.Debug( "DataContext.OnViewLoaded() invoked." );
					}
				};

                logger.Debug( "Loaded event attached." );

				this.activated = ( s, e ) =>
				{
                    var view = this.AssociatedObject;
                    var dc = this.conventions.GetViewDataContext( view, this.conventions.DefaultViewDataContextSearchBehavior );

                    if ( dc != null && dc.GetType().IsAttributeDefined<NotifyActivatedAttribute>() )
                    {
                        this.broker.Broadcast( this, new ViewModelActivated( dc ) );
                    }

					var temp = dc as IExpectViewActivatedCallback;
					if( temp != null )
					{
                        logger.Debug( "DataContext is IExpectViewActivatedCallback." );

						temp.OnViewActivated();

                        logger.Debug( "DataContext.OnViewActivated() invoked." );
					}
				};

                logger.Debug( "Activated event attached." );

				this.rendered = ( s, e ) =>
				{
                    logger.Debug( "Rendered event raised." );

                    var view = this.AssociatedObject;
                    var dc = this.conventions.GetViewDataContext( view, this.conventions.DefaultViewDataContextSearchBehavior );

					if( dc != null && dc.GetType().IsAttributeDefined<NotifyShownAttribute>() )
					{
                        this.broker.Broadcast( this, new ViewModelShown( dc ) );
					}

					var temp = dc as IExpectViewShownCallback;
					if( temp != null )
					{
                        logger.Debug( "DataContext is IExpectViewShownCallback." );

						temp.OnViewShown();

                        logger.Debug( "DataContext.OnViewShown() invoked." );
					}
				};


                logger.Debug( "Rendered event attached." );

				this.closed = ( s, e ) =>
				{
                    logger.Debug( "Closed event raised." );

                    var view = this.AssociatedObject;
                    var dc = this.conventions.GetViewDataContext( view, this.conventions.DefaultViewDataContextSearchBehavior );

					if ( dc != null && dc.GetType().IsAttributeDefined<NotifyClosedAttribute>() )
					{
                        this.broker.Broadcast( this, new ViewModelClosed(  dc ) );
					}

					var temp = dc as IExpectViewClosedCallback;
					if( temp != null )
					{
                        logger.Debug( "DataContext is IExpectViewClosedCallback." );

						temp.OnViewClosed();

                        logger.Debug( "DataContext.OnViewClosed() invoked." );
					}

					this.conventions.ViewReleaseHandler( view, ViewReleaseBehavior.Default );
				};


                logger.Debug( "Closed event attached." );

				this.closing = ( s, e ) =>
				{
                    logger.Debug( "Closing event raised." );

                    var view = this.AssociatedObject;
                    var dc = this.conventions.GetViewDataContext( view, this.conventions.DefaultViewDataContextSearchBehavior );

					var temp = dc as IExpectViewClosingCallback;
					if( temp != null )
					{
                        logger.Debug( "DataContext is IExpectViewClosingCallback." );

						temp.OnViewClosing( e );

                        logger.Debug( "DataContext.OnViewClosing() invoked." );
					}
				};


                logger.Debug( "Closing event attached." );
			}
            else
            {
                logger.Information("We are running within a designer.");
			}
		}

		/// <summary>
		/// Called after the behavior is attached to an AssociatedObject.
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();

            if ( !DesignTimeHelper.GetIsInDesignMode() )
			{
				this.AssociatedObject.Loaded += this.loaded;
				this.AssociatedObject.Activated += this.activated;
				this.AssociatedObject.ContentRendered += this.rendered;
				this.AssociatedObject.Closing += this.closing;
				this.AssociatedObject.Closed += this.closed;
			}
		}

		/// <summary>
		/// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
		/// </summary>
		protected override void OnDetaching()
		{
            if ( !DesignTimeHelper.GetIsInDesignMode() )
			{
				this.AssociatedObject.Loaded -= this.loaded;
				this.AssociatedObject.Activated -= this.activated;
				this.AssociatedObject.ContentRendered -= this.rendered;
				this.AssociatedObject.Closing -= this.closing;
				this.AssociatedObject.Closed -= this.closed;
			}
			base.OnDetaching();
		}
	}
}
