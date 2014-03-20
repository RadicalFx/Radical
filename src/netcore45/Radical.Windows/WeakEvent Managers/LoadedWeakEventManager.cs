using System;
using System.Windows;
using Topics.Radical.ComponentModel;
using Windows.UI.Xaml;

namespace Topics.Radical.Windows
{
	public sealed class LoadedWeakEventManager : WeakEventManager
	{
		static LoadedWeakEventManager GetCurrentManager()
		{
			var mt = typeof( LoadedWeakEventManager );

			var manager = ( LoadedWeakEventManager )WeakEventManager.GetCurrentManager( mt );
			if( manager == null )
			{
				manager = new LoadedWeakEventManager();
				WeakEventManager.SetCurrentManager( mt, manager );
			}

			return manager;
		}

		public static void AddListener( FrameworkElement source, IWeakEventListener listener )
		{
			LoadedWeakEventManager
				.GetCurrentManager()
				.ProtectedAddListener( source, listener );
		}

		public static void RemoveListener( FrameworkElement source, IWeakEventListener listener )
		{
			LoadedWeakEventManager
				.GetCurrentManager()
				.ProtectedRemoveListener( source, listener );
		}

		private LoadedWeakEventManager()
		{

		}

		void OnLoaded( object sender, RoutedEventArgs args )
		{
			base.DeliverEvent( sender, args );
		}

		/// <summary>
		/// When overridden in a derived class, starts listening for the event being managed. After <see cref="M:System.Windows.WeakEventManager.StartListening(System.Object)"/>  is first called, the manager should be in the state of calling <see cref="M:System.Windows.WeakEventManager.DeliverEvent(System.Object,System.EventArgs)"/> or <see cref="M:System.Windows.WeakEventManager.DeliverEventToList(System.Object,System.EventArgs,System.Windows.WeakEventManager.ListenerList)"/> whenever the relevant event from the provided source is handled.
		/// </summary>
		/// <param name="source">The source to begin listening on.</param>
		protected override void StartListening( object source )
		{
			var trigger = source as FrameworkElement;
			if( trigger != null )
			{
				trigger.Loaded += OnLoaded;
			}
		}

		/// <summary>
		/// When overridden in a derived class, stops listening on the provided source for the event being managed.
		/// </summary>
		/// <param name="source">The source to stop listening on.</param>
		protected override void StopListening( object source )
		{
			var trigger = source as FrameworkElement;
			if( trigger != null )
			{
				trigger.Loaded -= OnLoaded;
			}
		}
	}
}
