namespace Radical.ChangeTracking.Specialized
{
    public class ItemMovedDescriptor<T> : ItemChangedDescriptor<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMovedDescriptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="newIndex">The new index.</param>
        /// <param name="oldIndex">The old index.</param>
        public ItemMovedDescriptor( T item, int newIndex, int oldIndex )
            : base( item, newIndex)
        {
            this.OldIndex = oldIndex;
        }

        /// <summary>
        /// Gets the new index.
        /// </summary>
        /// <value>The new index.</value>
        public int NewIndex
        {
            get { return this.Index; }
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