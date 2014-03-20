namespace Topics.Radical.Collections
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using Topics.Radical.ComponentModel;

	/// <summary>
	/// A read-only collection of items.
	/// </summary>
	/// <typeparam name="T">The type of the collection item.</typeparam>
	public class ReadOnlyCollection<T> : System.Collections.Generic.IReadOnlyCollection<T>
	{
		/// <summary>
		/// Gets the inner list.
		/// </summary>
		/// <value>The inner list.</value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1002:DoNotExposeGenericLists" )]
		protected List<T> InnerList
		{
			get;
			private set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadOnlyCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="source">The source.</param>
		public ReadOnlyCollection( IEnumerable<T> source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			this.InnerList = new List<T>( source );
		}

		#region ICollection Members

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is less than zero.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.
		/// -or-
		/// <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.
		/// -or-
		/// The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
		/// </exception>
		public void CopyTo( Array array, int index )
		{
			( ( ICollection )this.InnerList ).CopyTo( array, index );
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
		/// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="index"/> is less than zero.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// 	<paramref name="array"/> is multidimensional.
		/// -or-
		/// <paramref name="index"/> is equal to or greater than the length of <paramref name="array"/>.
		/// -or-
		/// The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.
		/// </exception>
		/// <exception cref="T:System.ArgumentException">
		/// The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
		/// </exception>
		public void CopyTo( T[] array, int index )
		{
			( ( ICollection )this.InnerList ).CopyTo( array, index );
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
		/// </returns>
		public int Count
		{
			get { return this.InnerList.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
		/// </summary>
		/// <value></value>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
		/// </returns>
		public bool IsSynchronized
		{
			get { return ( ( ICollection )this.InnerList ).IsSynchronized; }
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
		/// </returns>
		public object SyncRoot
		{
			get { return ( ( ICollection )this.InnerList ).SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator GetEnumerator()
		{
			return ( ( IEnumerable<T> )this ).GetEnumerator();
		}

		#endregion

		#region IEnumerable<T> Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.InnerList.GetEnumerator();
		}

		#endregion
	}
}
