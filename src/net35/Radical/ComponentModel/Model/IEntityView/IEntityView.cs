namespace Topics.Radical.ComponentModel
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    /// <summary>
    /// Extendes the System.ComponentModel.IBindingListView interface by providing
    /// advanced features.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix" )]
    public interface IEntityView : 
        ISupportInitialize,
        IBindingListView, 
        ITypedList, 
        INotifyPropertyChanged //, INotifyCollectionChanged
    {
        /// <summary>
        /// Cancels a pending AddNew operation.
        /// </summary>
        void CancelNew();

        /// <summary>
        /// Cancels a pending AddNew operation that is occurring 
        /// at the specified index.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        void CancelNew( Int32 itemIndex );

        /// <summary>
        /// Ends a pending AddNew operation.
        /// </summary>
        void EndNew();

        /// <summary>
        /// Ends a pending AddNew operation.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        void EndNew( Int32 itemIndex );

        /// <summary>
        /// Gets a value indicating whether this instance is adding a new item.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is adding a new item; otherwise, <c>false</c>.
        /// </value>
        Boolean IsAddingNew { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is array based.
        /// Being an arrya based instance means that this list is a read-only list.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is array based; otherwise, <c>false</c>.
        /// </value>
        Boolean IsArrayBased { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is currently filtered.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is currently filtered; otherwise, <c>false</c>.
        /// </value>
        Boolean IsFiltered { get; }

        /// <summary>
        /// Gets or sets the filter to be used to exclude items from the collection of items returned by the data source
        /// </summary>
        /// <returns>The filter used to filter out in the item collection returned by the data source. </returns>
        new IEntityItemViewFilter Filter { get; set; }

        /// <summary>
        /// Gets the underlying data source.
        /// </summary>
        /// <value>The data source.</value>
        IList DataSource { get; }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Occurs when the applyed filter changes.
        /// </summary>
        event EventHandler FilterChanged;

        /// <summary>
        /// Occurs when the sort changes.
        /// </summary>
        event EventHandler SortChanged;

        /// <summary>
        /// Gets a value indicating whether moving elements is allowed in this view.
        /// </summary>
        /// <value><c>true</c> if move is allowed; otherwise, <c>false</c>.</value>
        Boolean AllowMove { get; }

        /// <summary>
        /// Moves the element at the specified source index to the specified new index.
        /// </summary>
        /// <param name="sourceIndex">Index of the source element to move.</param>
        /// <param name="newIndex">The destination index.</param>
        void Move( Int32 sourceIndex, Int32 newIndex );

        void ApplySort( String sortDescriptions );
    }
}
