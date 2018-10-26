namespace Radical.ChangeTracking.Specialized
{
    using System;

    public class ItemReplacedDescriptor<T> : ItemChangedDescriptor<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemReplacedDescriptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="newItem">The new item.</param>
        /// <param name="replacedItem">The replaced item.</param>
        /// <param name="index">The index.</param>
        public ItemReplacedDescriptor( T newItem, T replacedItem, Int32 index )
            : base( newItem, index)
        {
            this.ReplacedItem = replacedItem;
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
            get { return this.Item; }
        }
    }
}