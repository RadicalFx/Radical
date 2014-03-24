using System;
using System.Linq.Expressions;
using Topics.Radical.Linq;
using Topics.Radical.Observers;
using System.Diagnostics;

namespace Topics.Radical.Design
{
	class LiveDesignTimeProperty<T, TValue> : DesignTimeProperty<T, TValue>
	{
		readonly Expression<Func<TValue>> liveValueProperty;
		readonly Func<TValue> liveValueDelegate;

		public LiveDesignTimeProperty( DesignTimeHost<T> host, DesignTimeProperty property, Expression<Func<TValue>> liveValueProperty )
			: base( host, property )
		{
			PropertyObserver.For( host )
				.Observe( liveValueProperty.GetMemberName(), ( s, e ) =>
				{
					this.host.RaisePropertyChanged( this );
				} );

			this.liveValueProperty = liveValueProperty;
			this.liveValueDelegate = this.liveValueProperty.Compile();
		}

		public override object GetValue( object component )
		{
			return this.liveValueDelegate();
		}
	}
}