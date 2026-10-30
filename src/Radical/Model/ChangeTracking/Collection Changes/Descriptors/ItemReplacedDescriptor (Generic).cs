namespace Radical.ChangeTracking.Specialized
{
    /// <summary>
    /// Describes a change where a collection item was replaced by a new item.
    /// </summary>
    /// <typeparam name="T">The type of the collection item.</typeparam>
    public class ItemReplacedDescriptor<T> : ItemChangedDescriptor<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemReplacedDescriptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="newItem">The new item.</param>
        /// <param name="replacedItem">The replaced item.</param>
        /// <param name="index">The index.</param>
        public ItemReplacedDescriptor(T newItem, T replacedItem, int index)
            : base(newItem, index)
        {
            ReplacedItem = replacedItem;
        }

        /// <summary>
        /// Gets the replaced item.
        /// </summary>
        /// <value>The replaced item.</value>
        public T ReplacedItem
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the new item.
        /// </summary>
        /// <value>The new item.</value>
        public T NewItem
        {
            get { return Item; }
        }
    }
}