using Microsoft.Phone.Shell;
using Topics.Radical.ComponentModel.Messaging;
using Topics.Radical.Phone.Shell.Services;

namespace Topics.Radical.Windows.Phone.Shell.Services
{
	/// <summary>
	/// A Phone application service used to handle the lifecycle of the application.
	/// </summary>
	public sealed class LifecycleAwareService : PhoneApplicationService
	{
		readonly IMessageBroker broker;
		readonly INavigationService navigationService;

		/// <summary>
		/// Initializes a new instance of the <see cref="LifecycleAwareService"/> class.
		/// </summary>
		/// <param name="broker">The broker.</param>
		/// <param name="navigationService">The navigation service.</param>
		public LifecycleAwareService( IMessageBroker broker, INavigationService navigationService )
		{
			this.broker = broker;
			this.navigationService = navigationService;

			this.Launching += OnLaunching;
			this.Activated += OnActivated;
			this.Closing += OnClosing;
			this.Deactivated += OnDeactivated;

			//this.navigationService.Resumed += new EventHandler( OnResumed );
		}

		//void OnResumed( object sender, EventArgs e )
		//{
		//    var message = new LifecycleMessage( this, LifecycleEvent.Resumed );
		//    this.broker.Broadcast( message );
		//}

		void OnDeactivated( object sender, DeactivatedEventArgs e )
		{
			var message = new LifecycleMessage( this, LifecycleEvent.Suspending );
            this.broker.Broadcast( this, message );
		}

		void OnClosing( object sender, ClosingEventArgs e )
		{
			var message = new LifecycleMessage( this, LifecycleEvent.Closing );
            this.broker.Broadcast( this, message );
		}

		void OnActivated( object sender, ActivatedEventArgs e )
		{
			var message = new LifecycleMessage( this, LifecycleEvent.Resuming );
			this.broker.Broadcast(this, message );
		}

		void OnLaunching( object sender, LaunchingEventArgs e )
		{
			var message = new LifecycleMessage( this, LifecycleEvent.Starting );
            this.broker.Broadcast( this, message );
		}
	}
}