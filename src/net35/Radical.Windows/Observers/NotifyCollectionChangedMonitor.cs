using System;
using System.Collections.Specialized;
using Topics.Radical.ComponentModel;

namespace Topics.Radical.Observers
{
	public class NotifyCollectionChangedMonitor : AbstractMonitor<INotifyCollectionChanged>
	{
		public static NotifyCollectionChangedMonitor For( INotifyCollectionChanged source )
		{
			return new NotifyCollectionChangedMonitor( source );
		}

		NotifyCollectionChangedEventHandler handler = null;

		public NotifyCollectionChangedMonitor( INotifyCollectionChanged source )
			: base( source )
		{

		}

		public NotifyCollectionChangedMonitor()
			: base()
		{

		}

		public NotifyCollectionChangedMonitor( INotifyCollectionChanged source, IDispatcher dispatcher )
			: base( source, dispatcher )
		{

		}

		public NotifyCollectionChangedMonitor( IDispatcher dispatcher )
			: base( dispatcher )
		{

		}

		protected override void StartMonitoring( object source )
		{
			base.StartMonitoring( source );

			handler = ( s, e ) => this.OnChanged();
			this.Source.CollectionChanged += handler;
		}

		public void Observe( INotifyCollectionChanged source )
		{
			this.StopMonitoring();
			this.StartMonitoring( source );
		}

		protected override void OnStopMonitoring( Boolean targetDisposed )
		{
			if( !targetDisposed && this.WeakSource != null && this.WeakSource.IsAlive )
			{
				this.Source.CollectionChanged -= handler;
			}

			handler = null;
		}
	}
}
