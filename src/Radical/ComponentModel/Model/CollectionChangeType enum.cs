
namespace Radical.ComponentModel
{
    /// <summary>
    /// Determines the type of change(s) occurred in
    /// a <see cref="IEntityCollection&lt;T&gt;"/>
    /// </summary>
    public enum CollectionChangeType
    {
        /// <summary>
        /// No changes have occured in the collection
        /// </summary>
        None,

        /// <summary>
        /// The sort order of the collection has changed
        /// </summary>
        SortChanged,

        /// <summary>
        /// An item has been added to the list
        /// </summary>
        ItemAdded,

        /// <summary>
        /// An item has been removed from the list
        /// </summary>
        ItemRemoved,

        /// <summary>
        /// An item has changed
        /// </summary>
        ItemChanged,

        /// <summary>
        /// When to many changes occurs this is the
        /// preferred way to notify listener.
        /// Eg. in this case the DataGrid/DataGridView
        /// will fully rebind to the list
        /// </summary>
        Reset,

        /// <summary>
        /// On item has been moved inside the collection
        /// </summary>
        ItemMoved,

        /// <summary>
        /// On item has been replaced, the value supplied by the CollectionChanged
        /// event represents the overwritten value.
        /// </summary>
        ItemReplaced
    }
}
