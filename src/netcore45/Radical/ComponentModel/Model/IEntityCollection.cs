namespace Topics.Radical.ComponentModel
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;

	/// <summary>
	/// Identifies a strongly typed collection, extends this IList, ICollection and IEnumerable generics interfaces.
	/// </summary>
	/// <typeparam name="T">The type of object that this collection uses.</typeparam>
	public interface IEntityCollection<T> :
		IList<T>,
		ICollection<T>,
		IEnumerable,
		IEnumerable<T>
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
		/// <value><c>true</c> if this.instace allo the creation of new items; otherwise, <c>false</c>.</value>
		Boolean AllowNew { get; }

		/// <summary>
		/// Moves the specified item, identified by the <paramref name="oldIndex"/> parameter,
		/// to a new position, identified by the <paramref name="newIndex"/> parameter, inside
		/// the collection.
		/// </summary>
		/// <param name="oldIndex">The old index.</param>
		/// <param name="newIndex">The new index.</param>
		/// <exception cref="ArgumentOutOfRangeException">The supplied index (<paramref name="oldIndex" /> or <paramref name="newIndex" />)
		/// is outside the bounds of the collection.</exception>
		void Move( Int32 oldIndex, Int32 newIndex );

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
		void Move( T item, Int32 newIndex );

		/// <summary>
		/// Occurs when this collection changes.
		/// </summary>
		event EventHandler<CollectionChangedEventArgs<T>> CollectionChanged;

		/// <summary>
		/// Signals that the initialization process is completed.
		/// </summary>
		/// <param name="notify">if set to <c>true</c> raises the <see cref="CollectionChanged"/> event.</param>
		void EndInit( Boolean notify );

		/// <summary>
		/// Gets a value indicating whether this instance is loading data.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is loading data; otherwise, <c>false</c>.
		/// </value>
		Boolean IsInitializing { get; }

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
		/// <param name="list">The range of items ot add.</param>
		void AddRange( IEnumerable<T> list );
	}
}
