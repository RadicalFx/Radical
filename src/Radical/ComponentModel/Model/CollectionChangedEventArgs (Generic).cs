namespace Radical.ComponentModel
{
    using System;

    /// <summary>
    /// Provides additional data for the CollectionChanged event.
    /// </summary>
    /// <typeparam name="T">The type of the item managed by this instance.</typeparam>
    public class CollectionChangedEventArgs<T> : EventArgs //where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="changeType">Type of the change.</param>
        public CollectionChangedEventArgs(CollectionChangeType changeType)
            : this(changeType, -1, -1, default(T))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="changeType">Type of the change.</param>
        /// <param name="index">The index.</param>
        public CollectionChangedEventArgs(CollectionChangeType changeType, int index)
            : this(changeType, index, -1, default(T))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="changeType">Type of the change.</param>
        /// <param name="index">The index.</param>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="item">The item.</param>
        public CollectionChangedEventArgs(CollectionChangeType changeType, int index, int oldIndex, T item)
        {
            ChangeType = changeType;
            Index = index;
            OldIndex = oldIndex;
            Item = item;
        }

        /// <summary>
        /// The type of change occurred in the collection
        /// </summary>
        /// <value>The type of the change.</value>
        public CollectionChangeType ChangeType { get; }

        /// <summary>
        /// The Index of the changed Item
        /// </summary>
        /// <value>The index.</value>
        public int Index { get; }

        /// <summary>
        /// The Old Index of the changed item, e.g. in case of a Move operation
        /// </summary>
        /// <value>The old index.</value>
        public int OldIndex { get; }

        /// <summary>
        /// A reference to the changed Item, this filed will be null
        /// according to the ChangeType value
        /// </summary>
        /// <value>The item.</value>
        public T Item { get; }
    }
}
