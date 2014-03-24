using System;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;

namespace Topics.Radical.Windows.Behaviors
{
	public interface INotifyAttachedOjectLoaded
	{
		event EventHandler AttachedObjectLoaded;
		T GetAttachedObject<T>() where T : FrameworkElement;
	}

	public abstract class RadicalBehavior<T> : Behavior<T>,
		INotifyAttachedOjectLoaded, IWeakEventListener
		where T : FrameworkElement
	{
		RoutedEventHandler loaded;

		/// <summary>
		/// Initializes a new instance of the <see cref="RadicalBehavior&lt;T&gt;"/> class.
		/// </summary>
		public RadicalBehavior()
		{
			loaded = ( s, e ) => this.OnAttachedObjectLoaded();
		}

		protected override void OnAttached()
		{
			base.OnAttached();

			LoadedWeakEventManager.AddListener( this.AssociatedObject, this );
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			LoadedWeakEventManager.RemoveListener( this.AssociatedObject, this );
		}

		public event EventHandler AttachedObjectLoaded;

		protected virtual void OnAttachedObjectLoaded()
		{
			if( this.AttachedObjectLoaded != null )
			{
				this.AttachedObjectLoaded( this, EventArgs.Empty );
			}
		}

		public T GetAttachedObject<T>() where T : FrameworkElement
		{
			return this.AssociatedObject as T;
		}

		bool IWeakEventListener.ReceiveWeakEvent( Type managerType, object sender, EventArgs e )
		{
			return this.OnReceiveWeakEvent( managerType, sender, e );
		}

		protected virtual Boolean OnReceiveWeakEvent( Type managerType, object sender, EventArgs e )
		{
			if( managerType == typeof( LoadedWeakEventManager ) )
			{
				this.OnAttachedObjectLoaded();
			}
			else
			{
				// unrecognized event
				return false;
			}

			return true;
		}
	}
}