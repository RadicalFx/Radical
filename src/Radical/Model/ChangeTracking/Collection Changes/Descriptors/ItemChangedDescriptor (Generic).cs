namespace Radical.ChangeTracking.Specialized
{
    /// <summary>
    /// Describes a change where a collection item was modified.
    /// </summary>
    /// <typeparam name="T">The type of the collection item.</typeparam>
    public class ItemChangedDescriptor<T> : CollectionChangeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemChangedDescriptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="index">The index.</param>
        public ItemChangedDescriptor(T item, int index)
        {
            Item = item;
            Index = index;
        }

        /// <summary>
        /// Gets the item involved in the change.
        /// </summary>
        /// <value>The item.</value>
        public T Item
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the index of the item.
        /// </summary>
        /// <value>The index.</value>
        public int Index
        {
            get;
            private set;
        }
    }
}