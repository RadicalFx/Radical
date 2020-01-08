using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Radical.ComponentModel
{
    /// <summary>
    /// Identifies a strongly typed collection, extends this IList, ICollection and IEnumerable generics interfaces.
    /// </summary>
    /// <typeparam name="T">The type of object that this collection uses.</typeparam>
    public interface IEntityCollection<T> :
        IList<T>,
        ISupportInitialize
    {
        /// <summary>
        /// Creates a new instance of the type managed by this collection.
        /// </summary>
        /// <returns>An instance of the type managed by this collection.</returns>
        /// <exception cref="NotSupportedException">The operation is not supported, <see cref="AllowNew"/> property is set to <c>false</c>.</exception>
        /// <exception cref="NullReferenceException">The creation process returned a null reference.</exception>
        T CreateNew();

        /// <summary>
        /// Gets a value indicating whether this instance is capable of creating a new instance of the managed type T.
        /// </summary>
        /// <value><c>true</c> if this instance allows the creation of new items; otherwise, <c>false</c>.</value>
        bool AllowNew { get; }


        /// <summary>
        /// Gets the default view.
        /// </summary>
        /// <value>The default view.</value>
        IEntityView<T> DefaultView { get; }

        /// <summary>
        /// Creates a new view.
        /// </summary>
        /// <returns>An instance of a view.</returns>
        IEntityView<T> CreateView();


        /// <summary>
        /// Moves the specified item, identified by the <paramref name="oldIndex"/> parameter,
        /// to a new position, identified by the <paramref name="newIndex"/> parameter, inside
        /// the collection.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        /// <exception cref="ArgumentOutOfRangeException">The supplied index (<paramref name="oldIndex" /> or <paramref name="newIndex" />)
        /// is outside the bounds of the collection.</exception>
        void Move(int oldIndex, int newIndex);

        /// <summary>
        /// Moves the specified item to a new position, identified by the <paramref name="newIndex"/> 
        /// parameter, inside the collection.
        /// </summary>
        /// <param name="item">The item to move.</param>
        /// <param name="newIndex">The new index.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="item"/> to move does not belong to this collection.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The supplied index (<paramref name="newIndex"/>)
        /// is outside the bounds of the collection.</exception>
        /// <exception cref="ArgumentNullException">The supplied <paramref name="item"/> is a null reference.</exception>
        void Move(T item, int newIndex);

        /// <summary>
        /// Occurs when this collection changes.
        /// </summary>
        event EventHandler<CollectionChangedEventArgs<T>> CollectionChanged;

        /// <summary>
        /// Signals that the initialization process is completed.
        /// </summary>
        /// <param name="notify">if set to <c>true</c> raises the <see cref="CollectionChanged"/> event.</param>
        void EndInit(bool notify);

        /// <summary>
        /// Gets a value indicating whether this instance is loading data.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is loading data; otherwise, <c>false</c>.
        /// </value>
        bool IsInitializing { get; }

        ///// <summary>
        ///// Sorts the items in this instance.
        ///// </summary>
        //void Sort();

        ///// <summary>
        ///// Sorts the items in this instance using the specified comparer.
        ///// </summary>
        ///// <param name="comparer">The comparer to use to sort items.</param>
        //void Sort( IComparer<T> comparer );

        /// <summary>
        /// Copies the elements of the IEntityCollection to a new array.
        /// </summary>
        /// <returns>An array containing copies of the elements of the IEntityCollection.</returns>
        T[] ToArray();

        /// <summary>
        /// Adds a range of items.
        /// </summary>
        /// <param name="list">The range of items to add.</param>
        void AddRange(IEnumerable<T> list);
    }
}
