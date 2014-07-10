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
		readonly IConventionsHandler conventionsHandler;

		RoutedEventHandler loaded = null;
		EventHandler activated = null;
		EventHandler rendered = null;
		EventHandler closed = null;
		CancelEventHandler closing = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="WindowLifecycleNotificationsBehavior"/> class.
		/// </summary>
		/// <param name="broker">The broker.</param>
		/// <param name="conventionsHandler">The conventions handler.</param>
		public WindowLifecycleNotificationsBehavior( IMessageBroker broker, IConventionsHandler conventionsHandler )
		{
            Ensure.That( broker ).Named( () => broker ).IsNotNull();
            Ensure.That( conventionsHandler ).Named( () => conventionsHandler ).IsNotNull();

			this.broker = broker;
			this.conventionsHandler = conventionsHandler;

			if( !DesignTimeHelper.GetIsInDesignMode() )
			{
                logger.Debug( "We are not running within a designer." );
                logger.Debug( "Ready to attach events." );

				this.loaded = ( s, e ) =>
				{
                    logger.Debug( "Loaded event raised." );
                    Ensure.That( this.AssociatedObject ).Named( "AssociatedObject" ).IsNotNull();

					if( this.conventionsHandler.ShouldNotifyViewModelLoaded( this.AssociatedObject, this.AssociatedObject.DataContext ) )
					{
                        logger.Debug( "ShouldNotifyViewModelLoaded -> true." );

                        this.broker.Broadcast( this, new ViewModelLoaded( this, this.AssociatedObject.DataContext ) );

                        logger.Debug( "Message broadcasted." );
					}
					
					if ( this.conventionsHandler.ShouldNotifyViewLoaded( this.AssociatedObject ) )
                    {
                        logger.Debug( "ShouldNotifyViewLoaded -> true." );

                        this.broker.Broadcast( this, new ViewLoaded( this, this.AssociatedObject ));

                        logger.Debug( "Message broadcasted." );
                    }

					var dc = this.AssociatedObject.DataContext as IExpectViewLoadedCallback;
					if( dc != null )
					{
                        logger.Debug( "DataContext is IExpectViewLoadedCallback." );

						dc.OnViewLoaded();

                        logger.Debug( "DataContext.OnViewLoaded() invoked." );
					}
				};

                logger.Debug( "Loaded event attached." );

				this.activated = ( s, e ) =>
				{
                    if ( this.AssociatedObject.DataContext != null && this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyActivatedAttribute>() )
                    {
                        this.broker.Broadcast( this, new ViewModelActivated( this, this.AssociatedObject.DataContext ) );
                    }

					var dc = this.AssociatedObject.DataContext as IExpectViewActivatedCallback;
					if( dc != null )
					{
                        logger.Debug( "DataContext is IExpectViewActivatedCallback." );

						dc.OnViewActivated();

                        logger.Debug( "DataContext.OnViewActivated() invoked." );
					}
				};

                logger.Debug( "Activated event attached." );

				this.rendered = ( s, e ) =>
				{
                    logger.Debug( "Rendered event raised." );

					if( this.AssociatedObject.DataContext != null && this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyShownAttribute>() )
					{
                        this.broker.Broadcast( this, new ViewModelShown( this, this.AssociatedObject.DataContext ) );
					}

					var dc = this.AssociatedObject.DataContext as IExpectViewShownCallback;
					if( dc != null )
					{
                        logger.Debug( "DataContext is IExpectViewShownCallback." );

						dc.OnViewShown();

                        logger.Debug( "DataContext.OnViewShown() invoked." );
					}
				};


                logger.Debug( "Rendered event attached." );

				this.closed = ( s, e ) =>
				{
                    logger.Debug( "Closed event raised." );

					if ( this.AssociatedObject.DataContext != null && this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyClosedAttribute>() )
					{
                        this.broker.Broadcast( this, new ViewModelClosed( this, this.AssociatedObject.DataContext ) );
					}

					var dc = this.AssociatedObject.DataContext as IExpectViewClosedCallback;
					if( dc != null )
					{
                        logger.Debug( "DataContext is IExpectViewClosedCallback." );

						dc.OnViewClosed();

                        logger.Debug( "DataContext.OnViewClosed() invoked." );
					}

					this.conventionsHandler.ViewReleaseHandler( this.AssociatedObject );
				};


                logger.Debug( "Closed event attached." );

				this.closing = ( s, e ) =>
				{
                    logger.Debug( "Closing event raised." );

					//if( this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyClosingAttribute>() )
					//{
					//    var message = new ViewModelClosing( this, ( IViewModel )this.AssociatedObject.DataContext );

					//    //è asioncrono bisognerbbe marcarlo con asyn e qui fare await..oppure fare un dispatch
					//    this.broker.Broadcast( message );
					//}

					var dc = this.AssociatedObject.DataContext as IExpectViewClosingCallback;
					if( dc != null )
					{
                        logger.Debug( "DataContext is IExpectViewClosingCallback." );

						dc.OnViewClosing( e );

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
