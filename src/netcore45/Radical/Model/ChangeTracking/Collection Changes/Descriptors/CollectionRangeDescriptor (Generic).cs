namespace Topics.Radical.ChangeTracking.Specialized
{
	using System.Collections.Generic;

	public class CollectionRangeDescriptor<T> : CollectionChangeDescriptor<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionRangeDescriptor&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="items">The items.</param>
		public CollectionRangeDescriptor( IEnumerable<T> items )
		{
			this.Items = items;
		}

		/// <summary>
		/// Gets the range of items.
		/// </summary>
		/// <value>The items.</value>
		public IEnumerable<T> Items
		{
			get;
			private set;
		}
	}
}