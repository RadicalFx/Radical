using System;
using Topics.Radical.ComponentModel;
using System.Windows;
using Topics.Radical.Validation;

namespace Topics.Radical.Windows
{
	public class Fact : IMonitor, IWeakEventListener
	{
		public static Fact Create( Func<Object, Boolean> evaluator )
		{
			return new Fact( evaluator );
		}

		readonly Func<Object, Boolean> evaluator;

		public Fact( Func<Object, Boolean> evaluator )
		{
			this.evaluator = evaluator;
		}

		public Boolean Eval( Object parameter )
		{
			return this.evaluator( parameter );
		}

		public event EventHandler Changed;

		public void NotifyChanged()
		{
			if( this.Changed != null )
			{
				this.Changed( this, EventArgs.Empty );
			}
		}

		public static implicit operator Boolean( Fact fact )
		{
			return fact.Eval( null );
		}

		public Fact AddMonitor( IMonitor source )
		{
			Ensure.That( source ).Named( "source" ).IsNotNull();

			MonitorChangedWeakEventManager.AddListener( source, this );

			return this;
		}

		public Fact RemoveMonitor( IMonitor source )
		{
			Ensure.That( source ).Named( "source" ).IsNotNull();
			MonitorChangedWeakEventManager.RemoveListener( source, this );

			return this;
		}

		void OnMonitorChanged( IMonitor source )
		{
			this.NotifyChanged();
		}

		Boolean IWeakEventListener.ReceiveWeakEvent( Type managerType, object sender, EventArgs e )
		{
			if( managerType == typeof( MonitorChangedWeakEventManager ) )
			{
				this.OnMonitorChanged( ( IMonitor )sender );
			}
			else
			{
				// unrecognized event
				return false;
			}

			return true;
		}

		void IMonitor.StopMonitoring()
		{
			//NOP
		}
	}
}
