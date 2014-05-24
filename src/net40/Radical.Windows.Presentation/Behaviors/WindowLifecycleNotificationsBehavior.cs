using System;
using System.ComponentModel;
using System.Windows;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Reflection;
using Topics.Radical.Windows.Behaviors;
using Topics.Radical.Windows.Presentation.ComponentModel;
using Topics.Radical.Windows.Presentation.Messaging;

namespace Topics.Radical.Windows.Presentation.Behaviors
{
	/// <summary>
	/// Wires the window lifecycle to a view model that requires lifecycle notifications.
	/// </summary>
	public class WindowLifecycleNotificationsBehavior : RadicalBehavior<Window>
	{
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
			this.broker = broker;
			this.conventionsHandler = conventionsHandler;

			if( !DesignTimeHelper.GetIsInDesignMode() )
			{
				this.loaded = ( s, e ) =>
				{
					if( this.conventionsHandler.ShouldNotifyViewModelLoaded( this.AssociatedObject, this.AssociatedObject.DataContext ) )
					{
                        this.broker.Broadcast( this, new ViewModelLoaded( this, this.AssociatedObject.DataContext ) );
					}
					
					if ( this.conventionsHandler.ShouldNotifyViewLoaded( this.AssociatedObject ) )
                    {
                        this.broker.Broadcast( this, new ViewLoaded( this, this.AssociatedObject ));
                    }

					var dc = this.AssociatedObject.DataContext as IExpectViewLoadedCallback;
					if( dc != null )
					{
						dc.OnViewLoaded();
					}
				};

				this.activated = ( s, e ) =>
				{
                    if ( this.AssociatedObject.DataContext != null && this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyActivatedAttribute>() )
                    {
                        this.broker.Broadcast( this, new ViewModelActivated( this, this.AssociatedObject.DataContext ) );
                    }

					var dc = this.AssociatedObject.DataContext as IExpectViewActivatedCallback;
					if( dc != null )
					{
						dc.OnViewActivated();
					}
				};

				this.rendered = ( s, e ) =>
				{
					if( this.AssociatedObject.DataContext != null && this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyShownAttribute>() )
					{
                        this.broker.Broadcast( this, new ViewModelShown( this, this.AssociatedObject.DataContext ) );
					}

					var dc = this.AssociatedObject.DataContext as IExpectViewShownCallback;
					if( dc != null )
					{
						dc.OnViewShown();
					}
				};

				this.closed = ( s, e ) =>
				{
					if ( this.AssociatedObject.DataContext != null && this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyClosedAttribute>() )
					{
                        this.broker.Broadcast( this, new ViewModelClosed( this, this.AssociatedObject.DataContext ) );
					}

					var dc = this.AssociatedObject.DataContext as IExpectViewClosedCallback;
					if( dc != null )
					{
						dc.OnViewClosed();
					}

					this.conventionsHandler.ViewReleaseHandler( this.AssociatedObject );
				};

				this.closing = ( s, e ) =>
				{
					//if( this.AssociatedObject.DataContext.GetType().IsAttributeDefined<NotifyClosingAttribute>() )
					//{
					//    var message = new ViewModelClosing( this, ( IViewModel )this.AssociatedObject.DataContext );

					//    //è asioncrono bisognerbbe marcarlo con asyn e qui fare await..oppure fare un dispatch
					//    this.broker.Broadcast( message );
					//}

					var dc = this.AssociatedObject.DataContext as IExpectViewClosingCallback;
					if( dc != null )
					{
						dc.OnViewClosing( e );
					}
				};
			}
		}

		/// <summary>
		/// Called after the behavior is attached to an AssociatedObject.
		/// </summary>
		protected override void OnAttached()
		{
			base.OnAttached();

			if( !DesignTimeHelper.GetIsInDesignMode() )
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
			if( !DesignTimeHelper.GetIsInDesignMode() )
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
