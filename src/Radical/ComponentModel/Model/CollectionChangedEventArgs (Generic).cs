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
        public CollectionChangedEventArgs( CollectionChangeType changeType )
            : this( changeType, -1, -1, default( T ) )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="changeType">Type of the change.</param>
        /// <param name="index">The index.</param>
        public CollectionChangedEventArgs( CollectionChangeType changeType, Int32 index )
            : this( changeType, index, -1, default( T ) )
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionChangedEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="changeType">Type of the change.</param>
        /// <param name="index">The index.</param>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="item">The item.</param>
        public CollectionChangedEventArgs( CollectionChangeType changeType, Int32 index, Int32 oldIndex, T item )
        {
            //if( trace.IsDebugLevel )
            //{
            //    trace.Debug( "CollectionChangedEventArgs.ctor( CollectionChangeType, Int32, {0} )", typeof( T ).Name );
            //    trace.Debug( "changeType: {0}", changeType );
            //    trace.Debug( "index: {0}", index );
            //    trace.Debug( "oldIndex: {0}", oldIndex );
            //    trace.Debug( "item: {0}", item == null ? "<null>" : item.ToString() );
            //}

            this.ChangeType = changeType;
            this.Index = index;
            this.OldIndex = oldIndex;
            this.Item = item;
        }

        /// <summary>
        /// The type of change occurred in the collection
        /// </summary>
        /// <value>The type of the change.</value>
        public CollectionChangeType ChangeType
        {
            get;
            private set;
        }

        /// <summary>
        /// The Index of the changed Item
        /// </summary>
        /// <value>The index.</value>
        public Int32 Index
        {
            get;
            private set;
        }

        /// <summary>
        /// The Old Index of the changed item, eg in case of a Move operation
        /// </summary>
        /// <value>The old index.</value>
        public Int32 OldIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// A reference to the changed Item, this filed will be null
        /// according to the ChangeType value
        /// </summary>
        /// <value>The item.</value>
        public T Item
        {
            get;
            private set;
        }
    }
}
