using System;

namespace Topics.Radical.ComponentModel
{
	/// <summary>
	/// Delegate used to get property value
	/// </summary>
	public delegate Object EntityItemViewValueGetter<T, TValue>( EntityItemViewValueGetterArgs<T, TValue> args ); //where T : class;

	/// <summary>
	/// Delegate used to set property value
	/// </summary>
	public delegate void EntityItemViewValueSetter<T, TValue>( EntityItemViewValueSetterArgs<T, TValue> args ); //where T : class;

	public abstract class EntityItemViewValueArgs<T, TValue>
		//where T : class
	{
		protected EntityItemViewValueArgs( IEntityItemView<T> item, String propertyName )
		{
			this.Item = item;
			this.PropertyName = propertyName;
		}

		public IEntityItemView<T> Item
		{
			get;
			private set;
		}

		public String PropertyName
		{
			get;
			private set;
		}
	}
	public class EntityItemViewValueGetterArgs<T, TValue> : EntityItemViewValueArgs<T, TValue>
		//where T : class
	{
		public EntityItemViewValueGetterArgs( IEntityItemView<T> item, String propertyName )
			: base( item, propertyName )
		{

		}
	}

	public class EntityItemViewValueSetterArgs<T, TValue> : EntityItemViewValueArgs<T, TValue>
		//where T : class
	{
		public EntityItemViewValueSetterArgs( IEntityItemView<T> item, String propertyName, TValue value )
			: base( item, propertyName )
		{
			this.Value = value;
		}

		public TValue Value
		{
			get;
			private set;
		}
	}
}
