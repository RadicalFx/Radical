namespace Radical.ChangeTracking.Specialized
{
    /// <summary>
    /// Describes a change where a collection item was moved from one index to another.
    /// </summary>
    /// <typeparam name="T">The type of the collection item.</typeparam>
    public class ItemMovedDescriptor<T> : ItemChangedDescriptor<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMovedDescriptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="oldIndex">The old index.</param>
        public ItemMovedDescriptor(T item, int newIndex, int oldIndex)
            : base(item, newIndex)
        {
            OldIndex = oldIndex;
        }

        /// <summary>
        /// Gets the new index.
        /// </summary>
        /// <value>The new index.</value>
        public int NewIndex
        {
            get { return Index; }
        }

        /// <summary>
        /// Gets the old index.
        /// </summary>
        /// <value>The old index.</value>
        public int OldIndex
        {
            get;
            private set;
        }
    }
}