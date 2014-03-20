using System;
using System.Linq.Expressions;

namespace Topics.Radical.Design
{
	internal class UnboundedDesignTimeProperty<T, TValue> : DesignTimeProperty<T, TValue>
	{
		public UnboundedDesignTimeProperty(DesignTimeHost<T> host, String propertyName )
			: base( host, propertyName )
		{

		}

		public override object GetValue( object component )
		{
			return null;
		}
	}
}
