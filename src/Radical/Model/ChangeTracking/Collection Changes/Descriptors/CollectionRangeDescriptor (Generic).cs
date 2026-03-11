namespace Radical.ChangeTracking.Specialized
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes a collection change that involves a range of items, such as an add-range or clear operation.
    /// </summary>
    /// <typeparam name="T">The type of the items in the range.</typeparam>
    public class CollectionRangeDescriptor<T> : CollectionChangeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionRangeDescriptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public CollectionRangeDescriptor(IEnumerable<T> items)
        {
            Items = items;
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